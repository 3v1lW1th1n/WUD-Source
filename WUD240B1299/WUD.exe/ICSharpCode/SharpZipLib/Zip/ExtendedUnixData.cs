namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    public class ExtendedUnixData : ITaggedData
    {
        private DateTime createTime_ = new DateTime(0x7b2, 1, 1);
        private Flags flags_;
        private DateTime lastAccessTime_ = new DateTime(0x7b2, 1, 1);
        private DateTime modificationTime_ = new DateTime(0x7b2, 1, 1);

        public byte[] GetData()
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            {
                using (ZipHelperStream stream2 = new ZipHelperStream(stream))
                {
                    stream2.IsStreamOwner = false;
                    stream2.WriteByte((byte) this.flags_);
                    if (((byte) (this.flags_ & Flags.ModificationTime)) != 0)
                    {
                        DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        TimeSpan span = (TimeSpan) (this.modificationTime_.ToUniversalTime() - time.ToUniversalTime());
                        int totalSeconds = (int) span.TotalSeconds;
                        stream2.WriteLEInt(totalSeconds);
                    }
                    if (((byte) (this.flags_ & Flags.AccessTime)) != 0)
                    {
                        DateTime time2 = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        TimeSpan span2 = (TimeSpan) (this.lastAccessTime_.ToUniversalTime() - time2.ToUniversalTime());
                        int num2 = (int) span2.TotalSeconds;
                        stream2.WriteLEInt(num2);
                    }
                    if (((byte) (this.flags_ & Flags.CreateTime)) != 0)
                    {
                        DateTime time3 = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        TimeSpan span3 = (TimeSpan) (this.createTime_.ToUniversalTime() - time3.ToUniversalTime());
                        int num3 = (int) span3.TotalSeconds;
                        stream2.WriteLEInt(num3);
                    }
                    buffer = stream.ToArray();
                }
            }
            return buffer;
        }

        public static bool IsValidValue(DateTime value)
        {
            if (value < new DateTime(0x76d, 12, 13, 20, 0x2d, 0x34))
            {
                return (value <= new DateTime(0x7f6, 1, 0x13, 3, 14, 7));
            }
            return true;
        }

        public void SetData(byte[] data, int index, int count)
        {
            using (MemoryStream stream = new MemoryStream(data, index, count, false))
            {
                using (ZipHelperStream stream2 = new ZipHelperStream(stream))
                {
                    this.flags_ = (Flags) ((byte) stream2.ReadByte());
                    if ((((byte) (this.flags_ & Flags.ModificationTime)) != 0) && (count >= 5))
                    {
                        int seconds = stream2.ReadLEInt();
                        DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        this.modificationTime_ = (time.ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
                    }
                    if (((byte) (this.flags_ & Flags.AccessTime)) != 0)
                    {
                        int num2 = stream2.ReadLEInt();
                        DateTime time3 = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        this.lastAccessTime_ = (time3.ToUniversalTime() + new TimeSpan(0, 0, 0, num2, 0)).ToLocalTime();
                    }
                    if (((byte) (this.flags_ & Flags.CreateTime)) != 0)
                    {
                        int num3 = stream2.ReadLEInt();
                        DateTime time5 = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        this.createTime_ = (time5.ToUniversalTime() + new TimeSpan(0, 0, 0, num3, 0)).ToLocalTime();
                    }
                }
            }
        }

        public DateTime AccessTime
        {
            get => 
                this.lastAccessTime_;
            set
            {
                if (!IsValidValue(value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.flags_ = (Flags) ((byte) (this.flags_ | Flags.AccessTime));
                this.lastAccessTime_ = value;
            }
        }

        public DateTime CreateTime
        {
            get => 
                this.createTime_;
            set
            {
                if (!IsValidValue(value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.flags_ = (Flags) ((byte) (this.flags_ | Flags.CreateTime));
                this.createTime_ = value;
            }
        }

        private Flags Include
        {
            get => 
                this.flags_;
            set
            {
                this.flags_ = value;
            }
        }

        public DateTime ModificationTime
        {
            get => 
                this.modificationTime_;
            set
            {
                if (!IsValidValue(value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.flags_ = (Flags) ((byte) (this.flags_ | Flags.ModificationTime));
                this.modificationTime_ = value;
            }
        }

        public short TagID =>
            0x5455;

        [Flags]
        public enum Flags : byte
        {
            AccessTime = 2,
            CreateTime = 4,
            ModificationTime = 1
        }
    }
}

