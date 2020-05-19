namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    public class ZipEntry : ICloneable
    {
        private string comment;
        private ulong compressedSize;
        private uint crc;
        private byte cryptoCheckValue_;
        private uint dosTime;
        private int externalFileAttributes;
        private byte[] extra;
        private int flags;
        private bool forceZip64_;
        private Known known;
        private ICSharpCode.SharpZipLib.Zip.CompressionMethod method;
        private string name;
        private long offset;
        private ulong size;
        private ushort versionMadeBy;
        private ushort versionToExtract;
        private long zipFileIndex;

        [Obsolete("Use Clone instead")]
        public ZipEntry(ZipEntry entry)
        {
            this.externalFileAttributes = -1;
            this.method = ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated;
            this.zipFileIndex = -1L;
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            this.known = entry.known;
            this.name = entry.name;
            this.size = entry.size;
            this.compressedSize = entry.compressedSize;
            this.crc = entry.crc;
            this.dosTime = entry.dosTime;
            this.method = entry.method;
            this.comment = entry.comment;
            this.versionToExtract = entry.versionToExtract;
            this.versionMadeBy = entry.versionMadeBy;
            this.externalFileAttributes = entry.externalFileAttributes;
            this.flags = entry.flags;
            this.zipFileIndex = entry.zipFileIndex;
            this.offset = entry.offset;
            this.forceZip64_ = entry.forceZip64_;
            if (entry.extra != null)
            {
                this.extra = new byte[entry.extra.Length];
                Array.Copy(entry.extra, 0, this.extra, 0, entry.extra.Length);
            }
        }

        public ZipEntry(string name) : this(name, 0, 0x2d, ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated)
        {
        }

        internal ZipEntry(string name, int versionRequiredToExtract) : this(name, versionRequiredToExtract, 0x2d, ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated)
        {
        }

        internal ZipEntry(string name, int versionRequiredToExtract, int madeByInfo, ICSharpCode.SharpZipLib.Zip.CompressionMethod method)
        {
            this.externalFileAttributes = -1;
            this.method = ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated;
            this.zipFileIndex = -1L;
            if (name == null)
            {
                throw new ArgumentNullException("ZipEntry name");
            }
            if (name.Length > 0xffff)
            {
                throw new ArgumentException("Name is too long", "name");
            }
            if ((versionRequiredToExtract != 0) && (versionRequiredToExtract < 10))
            {
                throw new ArgumentOutOfRangeException("versionRequiredToExtract");
            }
            this.DateTime = System.DateTime.Now;
            this.name = name;
            this.versionMadeBy = (ushort) madeByInfo;
            this.versionToExtract = (ushort) versionRequiredToExtract;
            this.method = method;
        }

        public static string CleanName(string name)
        {
            if (name == null)
            {
                return string.Empty;
            }
            if (Path.IsPathRooted(name))
            {
                name = name.Substring(Path.GetPathRoot(name).Length);
            }
            name = name.Replace(@"\", "/");
            while ((name.Length > 0) && (name[0] == '/'))
            {
                name = name.Remove(0, 1);
            }
            return name;
        }

        public object Clone()
        {
            ZipEntry entry = (ZipEntry) base.MemberwiseClone();
            if (this.extra != null)
            {
                entry.extra = new byte[this.extra.Length];
                Array.Copy(this.extra, 0, entry.extra, 0, this.extra.Length);
            }
            return entry;
        }

        public void ForceZip64()
        {
            this.forceZip64_ = true;
        }

        private bool HasDosAttributes(int attributes)
        {
            bool flag = false;
            if (((((byte) (this.known & Known.ExternalAttributes)) != 0) && ((this.HostSystem == 0) || (this.HostSystem == 10))) && ((this.ExternalFileAttributes & attributes) == attributes))
            {
                flag = true;
            }
            return flag;
        }

        public bool IsCompressionMethodSupported() => 
            IsCompressionMethodSupported(this.CompressionMethod);

        public static bool IsCompressionMethodSupported(ICSharpCode.SharpZipLib.Zip.CompressionMethod method)
        {
            if (method != ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated)
            {
                return (method == ICSharpCode.SharpZipLib.Zip.CompressionMethod.Stored);
            }
            return true;
        }

        public bool IsZip64Forced() => 
            this.forceZip64_;

        internal void ProcessExtraData(bool localHeader)
        {
            ZipExtraData data = new ZipExtraData(this.extra);
            if (data.Find(1))
            {
                if ((this.versionToExtract & 0xff) < 0x2d)
                {
                    throw new ZipException("Zip64 Extended information found but version is not valid");
                }
                this.forceZip64_ = true;
                if (data.ValueLength < 4)
                {
                    throw new ZipException("Extra data extended Zip64 information length is invalid");
                }
                if (localHeader || (this.size == 0xffffffffL))
                {
                    this.size = (ulong) data.ReadLong();
                }
                if (localHeader || (this.compressedSize == 0xffffffffL))
                {
                    this.compressedSize = (ulong) data.ReadLong();
                }
                if (!localHeader && (this.offset == 0xffffffffL))
                {
                    this.offset = data.ReadLong();
                }
            }
            else if (((this.versionToExtract & 0xff) >= 0x2d) && ((this.size == 0xffffffffL) || (this.compressedSize == 0xffffffffL)))
            {
                throw new ZipException("Zip64 Extended information required but is missing.");
            }
            if (data.Find(10))
            {
                if (data.ValueLength < 8)
                {
                    throw new ZipException("NTFS Extra data invalid");
                }
                data.ReadInt();
                while (data.UnreadCount >= 4)
                {
                    int num = data.ReadShort();
                    int amount = data.ReadShort();
                    if (num == 1)
                    {
                        if (amount >= 0x18)
                        {
                            long fileTime = data.ReadLong();
                            data.ReadLong();
                            data.ReadLong();
                            this.DateTime = System.DateTime.FromFileTime(fileTime);
                            return;
                        }
                        return;
                    }
                    data.Skip(amount);
                }
            }
            else if (data.Find(0x5455))
            {
                int valueLength = data.ValueLength;
                if (((data.ReadByte() & 1) != 0) && (valueLength >= 5))
                {
                    int seconds = data.ReadInt();
                    System.DateTime time = new System.DateTime(0x7b2, 1, 1, 0, 0, 0);
                    this.DateTime = (time.ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
                }
            }
        }

        public override string ToString() => 
            this.name;

        public bool CanDecompress
        {
            get
            {
                if ((this.Version > 0x2d) || (((this.Version != 10) && (this.Version != 11)) && ((this.Version != 20) && (this.Version != 0x2d))))
                {
                    return false;
                }
                return this.IsCompressionMethodSupported();
            }
        }

        public bool CentralHeaderRequiresZip64
        {
            get
            {
                if (!this.LocalHeaderRequiresZip64)
                {
                    return (this.offset >= 0xffffffffL);
                }
                return true;
            }
        }

        public string Comment
        {
            get => 
                this.comment;
            set
            {
                if ((value != null) && (value.Length > 0xffff))
                {
                    throw new ArgumentOutOfRangeException("value", "cannot exceed 65535");
                }
                this.comment = value;
            }
        }

        public long CompressedSize
        {
            get
            {
                if (((byte) (this.known & Known.CompressedSize)) == 0)
                {
                    return -1L;
                }
                return (long) this.compressedSize;
            }
            set
            {
                this.compressedSize = (ulong) value;
                this.known = (Known) ((byte) (this.known | Known.CompressedSize));
            }
        }

        public ICSharpCode.SharpZipLib.Zip.CompressionMethod CompressionMethod
        {
            get => 
                this.method;
            set
            {
                if (!IsCompressionMethodSupported(value))
                {
                    throw new NotSupportedException("Compression method not supported");
                }
                this.method = value;
            }
        }

        public long Crc
        {
            get
            {
                if (((byte) (this.known & Known.Crc)) == 0)
                {
                    return -1L;
                }
                return (long) (this.crc & 0xffffffffL);
            }
            set
            {
                if ((this.crc & 18446744069414584320L) != 0L)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.crc = (uint) value;
                this.known = (Known) ((byte) (this.known | Known.Crc));
            }
        }

        internal byte CryptoCheckValue
        {
            get => 
                this.cryptoCheckValue_;
            set
            {
                this.cryptoCheckValue_ = value;
            }
        }

        public System.DateTime DateTime
        {
            get
            {
                uint num = Math.Min((uint) 0x3b, (uint) (2 * (this.dosTime & 0x1f)));
                uint num2 = Math.Min((uint) 0x3b, (uint) ((this.dosTime >> 5) & 0x3f));
                uint num3 = Math.Min((uint) 0x17, (uint) ((this.dosTime >> 11) & 0x1f));
                uint num4 = Math.Max(1, Math.Min((uint) 12, (uint) ((this.dosTime >> 0x15) & 15)));
                uint num5 = ((this.dosTime >> 0x19) & 0x7f) + 0x7bc;
                return new System.DateTime((int) num5, (int) num4, Math.Max(1, Math.Min(System.DateTime.DaysInMonth((int) num5, (int) num4), ((int) (this.dosTime >> 0x10)) & 0x1f)), (int) num3, (int) num2, (int) num);
            }
            set
            {
                uint year = (uint) value.Year;
                uint month = (uint) value.Month;
                uint day = (uint) value.Day;
                uint hour = (uint) value.Hour;
                uint minute = (uint) value.Minute;
                uint second = (uint) value.Second;
                if (year < 0x7bc)
                {
                    year = 0x7bc;
                    month = 1;
                    day = 1;
                    hour = 0;
                    minute = 0;
                    second = 0;
                }
                else if (year > 0x83b)
                {
                    year = 0x83b;
                    month = 12;
                    day = 0x1f;
                    hour = 0x17;
                    minute = 0x3b;
                    second = 0x3b;
                }
                this.DosTime = (long) ((ulong) ((((((((year - 0x7bc) & 0x7f) << 0x19) | (month << 0x15)) | (day << 0x10)) | (hour << 11)) | (minute << 5)) | (second >> 1)));
            }
        }

        public long DosTime
        {
            get
            {
                if (((byte) (this.known & (Known.None | Known.Time))) == 0)
                {
                    return 0L;
                }
                return (long) this.dosTime;
            }
            set
            {
                this.dosTime = (uint) value;
                this.known = (Known) ((byte) (this.known | Known.None | Known.Time));
            }
        }

        public int ExternalFileAttributes
        {
            get
            {
                if (((byte) (this.known & Known.ExternalAttributes)) == 0)
                {
                    return -1;
                }
                return this.externalFileAttributes;
            }
            set
            {
                this.externalFileAttributes = value;
                this.known = (Known) ((byte) (this.known | Known.ExternalAttributes));
            }
        }

        public byte[] ExtraData
        {
            get => 
                this.extra;
            set
            {
                if (value == null)
                {
                    this.extra = null;
                }
                else
                {
                    if (value.Length > 0xffff)
                    {
                        throw new ArgumentOutOfRangeException("value");
                    }
                    this.extra = new byte[value.Length];
                    Array.Copy(value, 0, this.extra, 0, value.Length);
                }
            }
        }

        public int Flags
        {
            get => 
                this.flags;
            set
            {
                this.flags = value;
            }
        }

        public bool HasCrc =>
            (((byte) (this.known & Known.Crc)) != 0);

        public int HostSystem
        {
            get => 
                ((this.versionMadeBy >> 8) & 0xff);
            set
            {
                this.versionMadeBy = (ushort) (this.versionMadeBy & 0xff);
                this.versionMadeBy = (ushort) (this.versionMadeBy | ((ushort) ((value & 0xff) << 8)));
            }
        }

        public bool IsCrypted
        {
            get => 
                ((this.flags & 1) != 0);
            set
            {
                if (value)
                {
                    this.flags |= 1;
                }
                else
                {
                    this.flags &= -2;
                }
            }
        }

        public bool IsDirectory
        {
            get
            {
                int length = this.name.Length;
                return (((length > 0) && ((this.name[length - 1] == '/') || (this.name[length - 1] == '\\'))) || this.HasDosAttributes(0x10));
            }
        }

        public bool IsDOSEntry
        {
            get
            {
                if (this.HostSystem != 0)
                {
                    return (this.HostSystem == 10);
                }
                return true;
            }
        }

        public bool IsFile =>
            (!this.IsDirectory && !this.HasDosAttributes(8));

        public bool IsUnicodeText
        {
            get => 
                ((this.flags & 0x800) != 0);
            set
            {
                if (value)
                {
                    this.flags |= 0x800;
                }
                else
                {
                    this.flags &= -2049;
                }
            }
        }

        public bool LocalHeaderRequiresZip64
        {
            get
            {
                bool flag = this.forceZip64_;
                if (flag)
                {
                    return flag;
                }
                ulong compressedSize = this.compressedSize;
                if ((this.versionToExtract == 0) && this.IsCrypted)
                {
                    compressedSize += (ulong) 12L;
                }
                return (((this.size >= 0xffffffffL) || (compressedSize >= 0xffffffffL)) && ((this.versionToExtract == 0) || (this.versionToExtract >= 0x2d)));
            }
        }

        public string Name =>
            this.name;

        public long Offset
        {
            get => 
                this.offset;
            set
            {
                this.offset = value;
            }
        }

        public long Size
        {
            get
            {
                if (((byte) (this.known & (Known.None | Known.Size))) == 0)
                {
                    return -1L;
                }
                return (long) this.size;
            }
            set
            {
                this.size = (ulong) value;
                this.known = (Known) ((byte) (this.known | Known.None | Known.Size));
            }
        }

        public int Version
        {
            get
            {
                if (this.versionToExtract != 0)
                {
                    return this.versionToExtract;
                }
                int num = 10;
                if (this.CentralHeaderRequiresZip64)
                {
                    return 0x2d;
                }
                if (ICSharpCode.SharpZipLib.Zip.CompressionMethod.Deflated == this.method)
                {
                    return 20;
                }
                if (this.IsDirectory)
                {
                    return 20;
                }
                if (this.IsCrypted)
                {
                    return 20;
                }
                if (this.HasDosAttributes(8))
                {
                    num = 11;
                }
                return num;
            }
        }

        public int VersionMadeBy =>
            (this.versionMadeBy & 0xff);

        public long ZipFileIndex
        {
            get => 
                this.zipFileIndex;
            set
            {
                this.zipFileIndex = value;
            }
        }

        [Flags]
        private enum Known : byte
        {
            CompressedSize = 2,
            Crc = 4,
            ExternalAttributes = 0x10,
            None = 0,
            Size = 1,
            Time = 8
        }
    }
}

