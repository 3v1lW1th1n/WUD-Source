namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Core;
    using ICSharpCode.SharpZipLib.Encryption;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;

    public class ZipFile : IEnumerable, IDisposable
    {
        private IArchiveStorage archiveStorage_;
        private Stream baseStream_;
        private int bufferSize_;
        private string comment_;
        private bool commentEdited_;
        private bool contentsEdited_;
        private byte[] copyBuffer_;
        private const int DefaultBufferSize = 0x1000;
        private ZipEntry[] entries_;
        private bool isDisposed_;
        private bool isNewArchive_;
        private bool isStreamOwner;
        private byte[] key;
        public KeysRequiredEventHandler KeysRequired;
        private string name_;
        private ZipString newComment_;
        private long offsetOfFirstEntry;
        private string tempDirectory_;
        private long updateCount_;
        private IDynamicDataSource updateDataSource_;
        private IEntryFactory updateEntryFactory_;
        private Hashtable updateIndex_;
        private ArrayList updates_;
        private ICSharpCode.SharpZipLib.Zip.UseZip64 useZip64_;

        internal ZipFile()
        {
            this.useZip64_ = ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic;
            this.bufferSize_ = 0x1000;
            this.updateEntryFactory_ = new ZipEntryFactory();
            this.tempDirectory_ = string.Empty;
            this.entries_ = new ZipEntry[0];
            this.isNewArchive_ = true;
        }

        public ZipFile(FileStream file)
        {
            this.useZip64_ = ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic;
            this.bufferSize_ = 0x1000;
            this.updateEntryFactory_ = new ZipEntryFactory();
            this.tempDirectory_ = string.Empty;
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (!file.CanSeek)
            {
                throw new ArgumentException("Stream is not seekable", "file");
            }
            this.baseStream_ = file;
            this.name_ = file.Name;
            this.isStreamOwner = true;
            try
            {
                this.ReadEntries();
            }
            catch
            {
                this.DisposeInternal(true);
                throw;
            }
        }

        public ZipFile(Stream stream)
        {
            this.useZip64_ = ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic;
            this.bufferSize_ = 0x1000;
            this.updateEntryFactory_ = new ZipEntryFactory();
            this.tempDirectory_ = string.Empty;
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (!stream.CanSeek)
            {
                throw new ArgumentException("Stream is not seekable", "stream");
            }
            this.baseStream_ = stream;
            this.isStreamOwner = true;
            if (this.baseStream_.Length > 0L)
            {
                try
                {
                    this.ReadEntries();
                    return;
                }
                catch
                {
                    this.DisposeInternal(true);
                    throw;
                }
            }
            this.entries_ = new ZipEntry[0];
            this.isNewArchive_ = true;
        }

        public ZipFile(string name)
        {
            this.useZip64_ = ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic;
            this.bufferSize_ = 0x1000;
            this.updateEntryFactory_ = new ZipEntryFactory();
            this.tempDirectory_ = string.Empty;
            this.name_ = name;
            this.baseStream_ = File.OpenRead(name);
            this.isStreamOwner = true;
            try
            {
                this.ReadEntries();
            }
            catch
            {
                this.DisposeInternal(true);
                throw;
            }
        }

        public void AbortUpdate()
        {
            this.PostUpdateCleanup();
        }

        public void Add(ZipEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            this.CheckUpdating();
            if ((entry.Size != 0L) || (entry.CompressedSize != 0L))
            {
                throw new ZipException("Entry cannot have any data");
            }
            this.AddUpdate(new ZipUpdate(UpdateCommand.Add, entry));
        }

        public void Add(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            this.CheckUpdating();
            this.AddUpdate(new ZipUpdate(fileName, this.EntryFactory.MakeFileEntry(fileName)));
        }

        public void Add(IStaticDataSource dataSource, string entryName)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }
            this.CheckUpdating();
            this.AddUpdate(new ZipUpdate(dataSource, this.EntryFactory.MakeFileEntry(entryName)));
        }

        public void Add(string fileName, CompressionMethod compressionMethod)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (!ZipEntry.IsCompressionMethodSupported(compressionMethod))
            {
                throw new ZipException("Compression method not supported");
            }
            this.CheckUpdating();
            this.contentsEdited_ = true;
            ZipEntry entry = this.EntryFactory.MakeFileEntry(fileName);
            entry.CompressionMethod = compressionMethod;
            this.AddUpdate(new ZipUpdate(fileName, entry));
        }

        public void Add(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }
            this.CheckUpdating();
            ZipEntry entry = this.EntryFactory.MakeFileEntry(entryName, false);
            entry.CompressionMethod = compressionMethod;
            this.AddUpdate(new ZipUpdate(dataSource, entry));
        }

        public void Add(string fileName, CompressionMethod compressionMethod, bool useUnicodeText)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (!ZipEntry.IsCompressionMethodSupported(compressionMethod))
            {
                throw new ZipException("Compression method not supported");
            }
            this.CheckUpdating();
            this.contentsEdited_ = true;
            ZipEntry entry = this.EntryFactory.MakeFileEntry(fileName);
            entry.IsUnicodeText = useUnicodeText;
            entry.CompressionMethod = compressionMethod;
            this.AddUpdate(new ZipUpdate(fileName, entry));
        }

        public void Add(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod, bool useUnicodeText)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }
            this.CheckUpdating();
            ZipEntry entry = this.EntryFactory.MakeFileEntry(entryName, false);
            entry.IsUnicodeText = useUnicodeText;
            entry.CompressionMethod = compressionMethod;
            this.AddUpdate(new ZipUpdate(dataSource, entry));
        }

        public void AddDirectory(string directoryName)
        {
            if (directoryName == null)
            {
                throw new ArgumentNullException("directoryName");
            }
            this.CheckUpdating();
            ZipEntry entry = this.EntryFactory.MakeDirectoryEntry(directoryName);
            this.AddUpdate(new ZipUpdate(UpdateCommand.Add, entry));
        }

        private void AddEntry(ZipFile workFile, ZipUpdate update)
        {
            Stream source = null;
            if (update.Entry.IsFile)
            {
                source = update.GetSource();
                if (source == null)
                {
                    source = this.updateDataSource_.GetSource(update.Entry, update.Filename);
                }
            }
            if (source != null)
            {
                using (source)
                {
                    long length = source.Length;
                    if (update.OutEntry.Size < 0L)
                    {
                        update.OutEntry.Size = length;
                    }
                    else if (update.OutEntry.Size != length)
                    {
                        throw new ZipException("Entry size/stream size mismatch");
                    }
                    workFile.WriteLocalEntryHeader(update);
                    long position = workFile.baseStream_.Position;
                    using (Stream stream2 = workFile.GetOutputStream(update.OutEntry))
                    {
                        this.CopyBytes(update, stream2, source, length, true);
                    }
                    long num3 = workFile.baseStream_.Position;
                    update.OutEntry.CompressedSize = num3 - position;
                    if ((update.OutEntry.Flags & 8) == 8)
                    {
                        new ZipHelperStream(workFile.baseStream_).WriteDataDescriptor(update.OutEntry);
                    }
                    return;
                }
            }
            workFile.WriteLocalEntryHeader(update);
            update.OutEntry.CompressedSize = 0L;
        }

        private void AddUpdate(ZipUpdate update)
        {
            this.contentsEdited_ = true;
            int num = this.FindExistingUpdate(update.Entry.Name);
            if (num >= 0)
            {
                if (this.updates_[num] == null)
                {
                    this.updateCount_ += 1L;
                }
                this.updates_[num] = update;
            }
            else
            {
                num = this.updates_.Add(update);
                this.updateCount_ += 1L;
                this.updateIndex_.Add(update.Entry.Name, num);
            }
        }

        public void BeginUpdate()
        {
            if (this.Name == null)
            {
                this.BeginUpdate(new MemoryArchiveStorage(), new DynamicDiskDataSource());
            }
            else
            {
                this.BeginUpdate(new DiskArchiveStorage(this), new DynamicDiskDataSource());
            }
        }

        public void BeginUpdate(IArchiveStorage archiveStorage)
        {
            this.BeginUpdate(archiveStorage, new DynamicDiskDataSource());
        }

        public void BeginUpdate(IArchiveStorage archiveStorage, IDynamicDataSource dataSource)
        {
            if (this.IsEmbeddedArchive)
            {
                throw new ZipException("Cannot update embedded/SFX archives");
            }
            if (archiveStorage == null)
            {
                throw new ArgumentNullException("archiveStorage");
            }
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }
            this.archiveStorage_ = archiveStorage;
            this.updateDataSource_ = dataSource;
            this.updateIndex_ = new Hashtable();
            if (this.entries_ != null)
            {
                this.updates_ = new ArrayList(this.entries_.Length);
                foreach (ZipEntry entry in this.entries_)
                {
                    int num = this.updates_.Add(new ZipUpdate(entry));
                    this.updateIndex_.Add(entry.Name, num);
                }
            }
            else
            {
                this.updates_ = new ArrayList();
            }
            this.updateCount_ = this.updates_.Count;
            this.contentsEdited_ = false;
            this.commentEdited_ = false;
            this.newComment_ = null;
        }

        private static void CheckClassicPassword(CryptoStream classicCryptoStream, ZipEntry entry)
        {
            byte[] buffer = new byte[12];
            StreamUtils.ReadFully(classicCryptoStream, buffer);
            if (buffer[11] != entry.CryptoCheckValue)
            {
                throw new ZipException("Invalid password");
            }
        }

        private void CheckUpdating()
        {
            if (this.updates_ == null)
            {
                throw new ZipException("Cannot update until BeginUpdate has been called");
            }
        }

        public void Close()
        {
            this.DisposeInternal(true);
            GC.SuppressFinalize(this);
        }

        public void CommitUpdate()
        {
            this.CheckUpdating();
            try
            {
                this.updateIndex_.Clear();
                this.updateIndex_ = null;
                if (this.contentsEdited_)
                {
                    this.RunUpdates();
                }
                else if (this.commentEdited_)
                {
                    this.UpdateCommentOnly();
                }
                else if ((this.entries_ != null) && (this.entries_.Length == 0))
                {
                    byte[] comment = (this.newComment_ != null) ? this.newComment_.RawComment : ZipConstants.ConvertToArray(this.comment_);
                    using (ZipHelperStream stream = new ZipHelperStream(this.baseStream_))
                    {
                        stream.WriteEndOfCentralDirectory(0L, 0L, 0L, comment);
                    }
                }
            }
            finally
            {
                this.PostUpdateCleanup();
            }
        }

        private void CopyBytes(ZipUpdate update, Stream destination, Stream source, long bytesToCopy, bool updateCrc)
        {
            int num3;
            if (destination == source)
            {
                throw new InvalidOperationException("Destination and source are the same");
            }
            Crc32 crc = new Crc32();
            byte[] buffer = this.GetBuffer();
            long num = bytesToCopy;
            long num2 = 0L;
            do
            {
                int length = buffer.Length;
                if (bytesToCopy < length)
                {
                    length = (int) bytesToCopy;
                }
                num3 = source.Read(buffer, 0, length);
                if (num3 > 0)
                {
                    if (updateCrc)
                    {
                        crc.Update(buffer, 0, num3);
                    }
                    destination.Write(buffer, 0, num3);
                    bytesToCopy -= num3;
                    num2 += num3;
                }
            }
            while ((num3 > 0) && (bytesToCopy > 0L));
            if (num2 != num)
            {
                throw new ZipException($"Failed to copy bytes expected {num} read {num2}");
            }
            if (updateCrc)
            {
                update.OutEntry.Crc = crc.Value;
            }
        }

        private void CopyDescriptorBytes(ZipUpdate update, Stream dest, Stream source)
        {
            int descriptorSize = this.GetDescriptorSize(update);
            if (descriptorSize > 0)
            {
                byte[] buffer = this.GetBuffer();
                while (descriptorSize > 0)
                {
                    int count = Math.Min(buffer.Length, descriptorSize);
                    int num3 = source.Read(buffer, 0, count);
                    if (num3 <= 0)
                    {
                        throw new ZipException("Unxpected end of stream");
                    }
                    dest.Write(buffer, 0, num3);
                    descriptorSize -= num3;
                }
            }
        }

        private void CopyDescriptorBytesDirect(ZipUpdate update, Stream stream, ref long destinationPosition, long sourcePosition)
        {
            int descriptorSize = this.GetDescriptorSize(update);
            while (descriptorSize > 0)
            {
                int count = descriptorSize;
                byte[] buffer = this.GetBuffer();
                stream.Position = sourcePosition;
                int num3 = stream.Read(buffer, 0, count);
                if (num3 <= 0)
                {
                    throw new ZipException("Unxpected end of stream");
                }
                stream.Position = destinationPosition;
                stream.Write(buffer, 0, num3);
                descriptorSize -= num3;
                destinationPosition += num3;
                sourcePosition += num3;
            }
        }

        private void CopyEntry(ZipFile workFile, ZipUpdate update)
        {
            workFile.WriteLocalEntryHeader(update);
            if (update.Entry.CompressedSize > 0L)
            {
                long offset = update.Entry.Offset + 0x1aL;
                this.baseStream_.Seek(offset, SeekOrigin.Begin);
                uint num2 = this.ReadLEUshort();
                uint num3 = this.ReadLEUshort();
                this.baseStream_.Seek((long) (num2 + num3), SeekOrigin.Current);
                this.CopyBytes(update, workFile.baseStream_, this.baseStream_, update.Entry.CompressedSize, false);
            }
            this.CopyDescriptorBytes(update, workFile.baseStream_, this.baseStream_);
        }

        private void CopyEntryDataDirect(ZipUpdate update, Stream stream, bool updateCrc, ref long destinationPosition, ref long sourcePosition)
        {
            int num4;
            long compressedSize = update.Entry.CompressedSize;
            Crc32 crc = new Crc32();
            byte[] buffer = this.GetBuffer();
            long num2 = compressedSize;
            long num3 = 0L;
            do
            {
                int length = buffer.Length;
                if (compressedSize < length)
                {
                    length = (int) compressedSize;
                }
                stream.Position = sourcePosition;
                num4 = stream.Read(buffer, 0, length);
                if (num4 > 0)
                {
                    if (updateCrc)
                    {
                        crc.Update(buffer, 0, num4);
                    }
                    stream.Position = destinationPosition;
                    stream.Write(buffer, 0, num4);
                    destinationPosition += num4;
                    sourcePosition += num4;
                    compressedSize -= num4;
                    num3 += num4;
                }
            }
            while ((num4 > 0) && (compressedSize > 0L));
            if (num3 != num2)
            {
                throw new ZipException($"Failed to copy bytes expected {num2} read {num3}");
            }
            if (updateCrc)
            {
                update.OutEntry.Crc = crc.Value;
            }
        }

        private void CopyEntryDirect(ZipFile workFile, ZipUpdate update, ref long destinationPosition)
        {
            bool flag = false;
            if (update.Entry.Offset == destinationPosition)
            {
                flag = true;
            }
            if (!flag)
            {
                this.baseStream_.Position = destinationPosition;
                workFile.WriteLocalEntryHeader(update);
                destinationPosition = this.baseStream_.Position;
            }
            long sourcePosition = 0L;
            long offset = update.Entry.Offset + 0x1aL;
            this.baseStream_.Seek(offset, SeekOrigin.Begin);
            uint num3 = this.ReadLEUshort();
            uint num4 = this.ReadLEUshort();
            sourcePosition = (this.baseStream_.Position + num3) + num4;
            if (flag)
            {
                destinationPosition += (((sourcePosition - offset) + 0x1aL) + update.Entry.CompressedSize) + this.GetDescriptorSize(update);
            }
            else
            {
                if (update.Entry.CompressedSize > 0L)
                {
                    this.CopyEntryDataDirect(update, this.baseStream_, false, ref destinationPosition, ref sourcePosition);
                }
                this.CopyDescriptorBytesDirect(update, this.baseStream_, ref destinationPosition, sourcePosition);
            }
        }

        public static ZipFile Create(Stream outStream)
        {
            if (outStream == null)
            {
                throw new ArgumentNullException("outStream");
            }
            if (!outStream.CanWrite)
            {
                throw new ArgumentException("Stream is not writeable", "outStream");
            }
            if (!outStream.CanSeek)
            {
                throw new ArgumentException("Stream is not seekable", "outStream");
            }
            return new ZipFile { baseStream_ = outStream };
        }

        public static ZipFile Create(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            FileStream stream = File.Create(fileName);
            return new ZipFile { 
                name_ = fileName,
                baseStream_ = stream,
                isStreamOwner = true
            };
        }

        private Stream CreateAndInitDecryptionStream(Stream baseStream, ZipEntry entry)
        {
            CryptoStream classicCryptoStream = null;
            if ((entry.Version >= 50) && ((entry.Flags & 0x40) != 0))
            {
                throw new ZipException("Decryption method not supported");
            }
            PkzipClassicManaged managed = new PkzipClassicManaged();
            this.OnKeysRequired(entry.Name);
            if (!this.HaveKeys)
            {
                throw new ZipException("No password available for encrypted stream");
            }
            classicCryptoStream = new CryptoStream(baseStream, managed.CreateDecryptor(this.key, null), CryptoStreamMode.Read);
            CheckClassicPassword(classicCryptoStream, entry);
            return classicCryptoStream;
        }

        private Stream CreateAndInitEncryptionStream(Stream baseStream, ZipEntry entry)
        {
            CryptoStream stream = null;
            if ((entry.Version < 50) || ((entry.Flags & 0x40) == 0))
            {
                PkzipClassicManaged managed = new PkzipClassicManaged();
                this.OnKeysRequired(entry.Name);
                if (!this.HaveKeys)
                {
                    throw new ZipException("No password available for encrypted stream");
                }
                stream = new CryptoStream(new UncompressedStream(baseStream), managed.CreateEncryptor(this.key, null), CryptoStreamMode.Write);
                if ((entry.Crc < 0L) || ((entry.Flags & 8) != 0))
                {
                    WriteEncryptionHeader(stream, entry.DosTime << 0x10);
                    return stream;
                }
                WriteEncryptionHeader(stream, entry.Crc);
            }
            return stream;
        }

        public void Delete(ZipEntry entry)
        {
            this.CheckUpdating();
            int num = this.FindExistingUpdate(entry);
            if (num < 0)
            {
                throw new ZipException("Cannot find entry to delete");
            }
            this.contentsEdited_ = true;
            this.updates_[num] = null;
            this.updateCount_ -= 1L;
        }

        public bool Delete(string fileName)
        {
            this.CheckUpdating();
            bool flag = false;
            int num = this.FindExistingUpdate(fileName);
            if ((num < 0) || (this.updates_[num] == null))
            {
                throw new ZipException("Cannot find entry to delete");
            }
            flag = true;
            this.contentsEdited_ = true;
            this.updates_[num] = null;
            this.updateCount_ -= 1L;
            return flag;
        }

        protected virtual void Dispose(bool disposing)
        {
            this.DisposeInternal(disposing);
        }

        private void DisposeInternal(bool disposing)
        {
            if (!this.isDisposed_)
            {
                this.isDisposed_ = true;
                this.entries_ = null;
                if (this.IsStreamOwner && (this.baseStream_ != null))
                {
                    lock (this.baseStream_)
                    {
                        this.baseStream_.Close();
                    }
                }
            }
        }

        ~ZipFile()
        {
            this.Dispose(false);
        }

        public int FindEntry(string name, bool ignoreCase)
        {
            if (this.entries_ == null)
            {
                throw new InvalidOperationException("ZipFile has been closed");
            }
            for (int i = 0; i < this.entries_.Length; i++)
            {
                if (string.Compare(name, this.entries_[i].Name, ignoreCase, CultureInfo.InvariantCulture) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindExistingUpdate(ZipEntry entry)
        {
            int num = -1;
            string transformedFileName = this.GetTransformedFileName(entry.Name);
            if (this.updateIndex_.ContainsKey(transformedFileName))
            {
                num = (int) this.updateIndex_[transformedFileName];
            }
            return num;
        }

        private int FindExistingUpdate(string fileName)
        {
            int num = -1;
            string transformedFileName = this.GetTransformedFileName(fileName);
            if (this.updateIndex_.ContainsKey(transformedFileName))
            {
                num = (int) this.updateIndex_[transformedFileName];
            }
            return num;
        }

        private byte[] GetBuffer()
        {
            if (this.copyBuffer_ == null)
            {
                this.copyBuffer_ = new byte[this.bufferSize_];
            }
            return this.copyBuffer_;
        }

        private int GetDescriptorSize(ZipUpdate update)
        {
            int num = 0;
            if ((update.Entry.Flags & 8) != 0)
            {
                num = 12;
                if (update.Entry.LocalHeaderRequiresZip64)
                {
                    num = 20;
                }
            }
            return num;
        }

        public ZipEntry GetEntry(string name)
        {
            if (this.entries_ == null)
            {
                throw new InvalidOperationException("ZipFile has been closed");
            }
            int index = this.FindEntry(name, true);
            if (index < 0)
            {
                return null;
            }
            return (ZipEntry) this.entries_[index].Clone();
        }

        public IEnumerator GetEnumerator()
        {
            if (this.entries_ == null)
            {
                throw new InvalidOperationException("ZipFile has closed");
            }
            return new ZipEntryEnumerator(this.entries_);
        }

        public Stream GetInputStream(ZipEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            if (this.entries_ == null)
            {
                throw new InvalidOperationException("ZipFile has closed");
            }
            long zipFileIndex = entry.ZipFileIndex;
            if (((zipFileIndex < 0L) || (zipFileIndex >= this.entries_.Length)) || (this.entries_[(int) ((IntPtr) zipFileIndex)].Name != entry.Name))
            {
                zipFileIndex = this.FindEntry(entry.Name, true);
                if (zipFileIndex < 0L)
                {
                    throw new ZipException("Entry cannot be found");
                }
            }
            return this.GetInputStream(zipFileIndex);
        }

        public Stream GetInputStream(long entryIndex)
        {
            if (this.entries_ == null)
            {
                throw new InvalidOperationException("ZipFile is not open");
            }
            long start = this.LocateEntry(this.entries_[(int) ((IntPtr) entryIndex)]);
            CompressionMethod compressionMethod = this.entries_[(int) ((IntPtr) entryIndex)].CompressionMethod;
            Stream baseStream = new PartialInputStream(this.baseStream_, start, this.entries_[(int) ((IntPtr) entryIndex)].CompressedSize);
            if (this.entries_[(int) ((IntPtr) entryIndex)].IsCrypted)
            {
                baseStream = this.CreateAndInitDecryptionStream(baseStream, this.entries_[(int) ((IntPtr) entryIndex)]);
                if (baseStream == null)
                {
                    throw new ZipException("Unable to decrypt this entry");
                }
            }
            switch (compressionMethod)
            {
                case CompressionMethod.Stored:
                    return baseStream;

                case CompressionMethod.Deflated:
                    return new InflaterInputStream(baseStream, new Inflater(true));
            }
            throw new ZipException("Unsupported compression method " + compressionMethod);
        }

        private Stream GetOutputStream(ZipEntry entry)
        {
            Stream baseStream = this.baseStream_;
            if (entry.IsCrypted)
            {
                baseStream = this.CreateAndInitEncryptionStream(baseStream, entry);
            }
            CompressionMethod compressionMethod = entry.CompressionMethod;
            if (compressionMethod != CompressionMethod.Stored)
            {
                if (compressionMethod != CompressionMethod.Deflated)
                {
                    throw new ZipException("Unknown compression method " + entry.CompressionMethod);
                }
            }
            else
            {
                return new UncompressedStream(baseStream);
            }
            return new DeflaterOutputStream(baseStream, new Deflater(9, true)) { IsStreamOwner = false };
        }

        private string GetTransformedDirectoryName(string name) => 
            this.NameTransform?.TransformDirectory(name);

        private string GetTransformedFileName(string name) => 
            this.NameTransform?.TransformFile(name);

        private long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
        {
            using (ZipHelperStream stream = new ZipHelperStream(this.baseStream_))
            {
                return stream.LocateBlockWithSignature(signature, endLocation, minimumBlockSize, maximumVariableData);
            }
        }

        private long LocateEntry(ZipEntry entry) => 
            this.TestLocalHeader(entry, HeaderTest.Extract);

        private void ModifyEntry(ZipFile workFile, ZipUpdate update)
        {
            workFile.WriteLocalEntryHeader(update);
            long position = workFile.baseStream_.Position;
            if (update.Entry.IsFile && (update.Filename != null))
            {
                using (Stream stream = workFile.GetOutputStream(update.OutEntry))
                {
                    using (Stream stream2 = this.GetInputStream(update.Entry))
                    {
                        this.CopyBytes(update, stream, stream2, stream2.Length, true);
                    }
                }
            }
            long num2 = workFile.baseStream_.Position;
            update.Entry.CompressedSize = num2 - position;
        }

        private void OnKeysRequired(string fileName)
        {
            if (this.KeysRequired != null)
            {
                KeysRequiredEventArgs e = new KeysRequiredEventArgs(fileName, this.key);
                this.KeysRequired(this, e);
                this.key = e.Key;
            }
        }

        private void PostUpdateCleanup()
        {
            if (this.archiveStorage_ != null)
            {
                this.archiveStorage_.Dispose();
                this.archiveStorage_ = null;
            }
            this.updateDataSource_ = null;
            this.updates_ = null;
            this.updateIndex_ = null;
        }

        private void ReadEntries()
        {
            if (!this.baseStream_.CanSeek)
            {
                throw new ZipException("ZipFile stream must be seekable");
            }
            long endLocation = this.LocateBlockWithSignature(0x6054b50, this.baseStream_.Length, 0x16, 0xffff);
            if (endLocation < 0L)
            {
                throw new ZipException("Cannot find central directory");
            }
            ushort num2 = this.ReadLEUshort();
            ushort num3 = this.ReadLEUshort();
            ulong num4 = this.ReadLEUshort();
            ulong num5 = this.ReadLEUshort();
            ulong num6 = this.ReadLEUint();
            long num7 = this.ReadLEUint();
            uint num8 = this.ReadLEUshort();
            if (num8 > 0)
            {
                byte[] buffer = new byte[num8];
                StreamUtils.ReadFully(this.baseStream_, buffer);
                this.comment_ = ZipConstants.ConvertToString(buffer);
            }
            else
            {
                this.comment_ = string.Empty;
            }
            bool flag = false;
            if ((((num2 == 0xffff) || (num3 == 0xffff)) || ((num4 == 0xffffL) || (num5 == 0xffffL))) || ((num6 == 0xffffffffL) || (num7 == 0xffffffffL)))
            {
                flag = true;
                if (this.LocateBlockWithSignature(0x7064b50, endLocation, 0, 0x1000) < 0L)
                {
                    throw new ZipException("Cannot find Zip64 locator");
                }
                this.ReadLEUint();
                ulong num10 = this.ReadLEUlong();
                this.ReadLEUint();
                this.baseStream_.Position = (long) num10;
                long num11 = this.ReadLEUint();
                if (num11 != 0x6064b50L)
                {
                    throw new ZipException($"Invalid Zip64 Central directory signature at {num10:X}");
                }
                this.ReadLEUlong();
                this.ReadLEUshort();
                this.ReadLEUshort();
                this.ReadLEUint();
                this.ReadLEUint();
                num4 = this.ReadLEUlong();
                num5 = this.ReadLEUlong();
                num6 = this.ReadLEUlong();
                num7 = (long) this.ReadLEUlong();
            }
            this.entries_ = new ZipEntry[num4];
            if (!flag && (num7 < (((ulong) endLocation) - (((ulong) 4L) + num6))))
            {
                this.offsetOfFirstEntry = endLocation - ((4L + ((long) num6)) + num7);
                if (this.offsetOfFirstEntry <= 0L)
                {
                    throw new ZipException("Invalid embedded zip archive");
                }
            }
            this.baseStream_.Seek(this.offsetOfFirstEntry + num7, SeekOrigin.Begin);
            for (ulong i = 0L; i < num4; i += (ulong) 1L)
            {
                if (this.ReadLEUint() != 0x2014b50)
                {
                    throw new ZipException("Wrong Central Directory signature");
                }
                int madeByInfo = this.ReadLEUshort();
                int versionRequiredToExtract = this.ReadLEUshort();
                int flags = this.ReadLEUshort();
                int num16 = this.ReadLEUshort();
                uint num17 = this.ReadLEUint();
                uint num18 = this.ReadLEUint();
                long num19 = this.ReadLEUint();
                long num20 = this.ReadLEUint();
                int num21 = this.ReadLEUshort();
                int num22 = this.ReadLEUshort();
                int num23 = this.ReadLEUshort();
                this.ReadLEUshort();
                this.ReadLEUshort();
                uint num24 = this.ReadLEUint();
                long num25 = this.ReadLEUint();
                byte[] buffer2 = new byte[Math.Max(num21, num23)];
                StreamUtils.ReadFully(this.baseStream_, buffer2, 0, num21);
                ZipEntry entry = new ZipEntry(ZipConstants.ConvertToStringExt(flags, buffer2, num21), versionRequiredToExtract, madeByInfo, (CompressionMethod) num16) {
                    Crc = (long) (num18 & 0xffffffffL),
                    Size = num20 & ((long) 0xffffffffL),
                    CompressedSize = num19 & ((long) 0xffffffffL),
                    Flags = flags,
                    DosTime = (long) num17,
                    ZipFileIndex = (long) i,
                    Offset = num25,
                    ExternalFileAttributes = (int) num24
                };
                if ((flags & 8) == 0)
                {
                    entry.CryptoCheckValue = (byte) (num18 >> 0x18);
                }
                else
                {
                    entry.CryptoCheckValue = (byte) ((num17 >> 8) & 0xff);
                }
                if (num22 > 0)
                {
                    byte[] buffer3 = new byte[num22];
                    StreamUtils.ReadFully(this.baseStream_, buffer3);
                    entry.ExtraData = buffer3;
                }
                entry.ProcessExtraData(false);
                if (num23 > 0)
                {
                    StreamUtils.ReadFully(this.baseStream_, buffer2, 0, num23);
                    entry.Comment = ZipConstants.ConvertToStringExt(flags, buffer2, num23);
                }
                this.entries_[(int) ((IntPtr) i)] = entry;
            }
        }

        private uint ReadLEUint() => 
            ((uint) (this.ReadLEUshort() | (this.ReadLEUshort() << 0x10)));

        private ulong ReadLEUlong() => 
            (this.ReadLEUint() | (this.ReadLEUint() << 0x20));

        private ushort ReadLEUshort()
        {
            int num = this.baseStream_.ReadByte();
            if (num < 0)
            {
                throw new EndOfStreamException("End of stream");
            }
            int num2 = this.baseStream_.ReadByte();
            if (num2 < 0)
            {
                throw new EndOfStreamException("End of stream");
            }
            return (ushort) (((ushort) num) | ((ushort) (num2 << 8)));
        }

        private void Reopen()
        {
            if (this.Name == null)
            {
                throw new InvalidOperationException("Name is not known cannot Reopen");
            }
            this.Reopen(File.OpenRead(this.Name));
        }

        private void Reopen(Stream source)
        {
            if (source == null)
            {
                throw new ZipException("Failed to reopen archive - no source");
            }
            this.isNewArchive_ = false;
            this.baseStream_ = source;
            this.ReadEntries();
        }

        private void RunUpdates()
        {
            ZipFile file;
            long sizeEntries = 0L;
            long num2 = 0L;
            bool flag = true;
            bool flag2 = false;
            long destinationPosition = 0L;
            if (this.IsNewArchive)
            {
                file = this;
                file.baseStream_.Position = 0L;
                flag2 = true;
            }
            else if (this.archiveStorage_.UpdateMode == FileUpdateMode.Direct)
            {
                file = this;
                file.baseStream_.Position = 0L;
                flag2 = true;
                this.updates_.Sort(new UpdateComparer());
            }
            else
            {
                file = Create(this.archiveStorage_.GetTemporaryOutput());
                file.UseZip64 = this.UseZip64;
                if (this.key != null)
                {
                    file.key = (byte[]) this.key.Clone();
                }
            }
            try
            {
                foreach (ZipUpdate update in this.updates_)
                {
                    if (update != null)
                    {
                        switch (update.Command)
                        {
                            case UpdateCommand.Copy:
                                if (!flag2)
                                {
                                    goto Label_00EF;
                                }
                                this.CopyEntryDirect(file, update, ref destinationPosition);
                                break;

                            case UpdateCommand.Modify:
                                this.ModifyEntry(file, update);
                                break;

                            case UpdateCommand.Add:
                                goto Label_0107;
                        }
                    }
                    continue;
                Label_00EF:
                    this.CopyEntry(file, update);
                    continue;
                Label_0107:
                    if (!this.IsNewArchive && flag2)
                    {
                        file.baseStream_.Position = destinationPosition;
                    }
                    this.AddEntry(file, update);
                    if (flag2)
                    {
                        destinationPosition = file.baseStream_.Position;
                    }
                }
                if (!this.IsNewArchive && flag2)
                {
                    file.baseStream_.Position = destinationPosition;
                }
                long position = file.baseStream_.Position;
                foreach (ZipUpdate update2 in this.updates_)
                {
                    if (update2 != null)
                    {
                        sizeEntries += file.WriteCentralDirectoryHeader(update2.OutEntry);
                    }
                }
                byte[] comment = (this.newComment_ != null) ? this.newComment_.RawComment : ZipConstants.ConvertToArray(this.comment_);
                using (ZipHelperStream stream = new ZipHelperStream(file.baseStream_))
                {
                    stream.WriteEndOfCentralDirectory(this.updateCount_, sizeEntries, position, comment);
                }
                num2 = file.baseStream_.Position;
                foreach (ZipUpdate update3 in this.updates_)
                {
                    if (update3 != null)
                    {
                        if ((update3.CrcPatchOffset > 0L) && (update3.OutEntry.CompressedSize > 0L))
                        {
                            file.baseStream_.Position = update3.CrcPatchOffset;
                            file.WriteLEInt((int) update3.OutEntry.Crc);
                        }
                        if (update3.SizePatchOffset > 0L)
                        {
                            file.baseStream_.Position = update3.SizePatchOffset;
                            if (update3.OutEntry.LocalHeaderRequiresZip64)
                            {
                                file.WriteLeLong(update3.OutEntry.Size);
                                file.WriteLeLong(update3.OutEntry.CompressedSize);
                            }
                            else
                            {
                                file.WriteLEInt((int) update3.OutEntry.CompressedSize);
                                file.WriteLEInt((int) update3.OutEntry.Size);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            finally
            {
                if (flag2)
                {
                    if (flag)
                    {
                        file.baseStream_.Flush();
                        file.baseStream_.SetLength(num2);
                    }
                }
                else
                {
                    file.Close();
                }
            }
            if (flag)
            {
                if (flag2)
                {
                    this.isNewArchive_ = false;
                    file.baseStream_.Flush();
                    this.ReadEntries();
                }
                else
                {
                    this.baseStream_.Close();
                    this.Reopen(this.archiveStorage_.ConvertTemporaryToFinal());
                }
            }
            else
            {
                file.Close();
                if (!flag2 && (file.Name != null))
                {
                    File.Delete(file.Name);
                }
            }
        }

        public void SetComment(string comment)
        {
            this.CheckUpdating();
            this.newComment_ = new ZipString(comment);
            if (this.newComment_.RawLength > 0xffff)
            {
                this.newComment_ = null;
                throw new ZipException("Comment length exceeds maximum - 65535");
            }
            this.commentEdited_ = true;
        }

        void IDisposable.Dispose()
        {
            this.Close();
        }

        public bool TestArchive(bool testData) => 
            this.TestArchive(testData, TestStrategy.FindFirstError, null);

        public bool TestArchive(bool testData, TestStrategy strategy, ZipTestResultHandler resultHandler)
        {
            TestStatus status = new TestStatus(this);
            if (resultHandler != null)
            {
                resultHandler(status, null);
            }
            HeaderTest tests = testData ? (HeaderTest.Header | HeaderTest.Extract) : HeaderTest.Header;
            bool flag = true;
            try
            {
                for (int i = 0; flag && (i < this.Count); i++)
                {
                    if (resultHandler != null)
                    {
                        status.SetEntry(this[i]);
                        status.SetOperation(TestOperation.EntryHeader);
                        resultHandler(status, null);
                    }
                    try
                    {
                        this.TestLocalHeader(this[i], tests);
                    }
                    catch (ZipException exception)
                    {
                        status.AddError();
                        if (resultHandler != null)
                        {
                            resultHandler(status, $"Exception during test - '{exception.Message}'");
                        }
                        if (strategy == TestStrategy.FindFirstError)
                        {
                            flag = false;
                        }
                    }
                    if ((flag && testData) && this[i].IsFile)
                    {
                        int num3;
                        if (resultHandler != null)
                        {
                            status.SetOperation(TestOperation.EntryData);
                            resultHandler(status, null);
                        }
                        Stream inputStream = this.GetInputStream(this[i]);
                        Crc32 crc = new Crc32();
                        byte[] buffer = new byte[0x1000];
                        long num2 = 0L;
                        while ((num3 = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            crc.Update(buffer, 0, num3);
                            if (resultHandler != null)
                            {
                                num2 += num3;
                                status.SetBytesTested(num2);
                                resultHandler(status, null);
                            }
                        }
                        if (this[i].Crc != crc.Value)
                        {
                            status.AddError();
                            if (resultHandler != null)
                            {
                                resultHandler(status, "CRC mismatch");
                            }
                            if (strategy == TestStrategy.FindFirstError)
                            {
                                flag = false;
                            }
                        }
                        if ((this[i].Flags & 8) != 0)
                        {
                            ZipHelperStream stream2 = new ZipHelperStream(this.baseStream_);
                            DescriptorData data = new DescriptorData();
                            stream2.ReadDataDescriptor(this[i].LocalHeaderRequiresZip64, data);
                            if (this[i].Crc != data.Crc)
                            {
                                status.AddError();
                            }
                            if (this[i].CompressedSize != data.CompressedSize)
                            {
                                status.AddError();
                            }
                            if (this[i].Size != data.Size)
                            {
                                status.AddError();
                            }
                        }
                    }
                    if (resultHandler != null)
                    {
                        status.SetOperation(TestOperation.EntryComplete);
                        resultHandler(status, null);
                    }
                }
                if (resultHandler != null)
                {
                    status.SetOperation(TestOperation.MiscellaneousTests);
                    resultHandler(status, null);
                }
            }
            catch (Exception exception2)
            {
                status.AddError();
                if (resultHandler != null)
                {
                    resultHandler(status, $"Exception during test - '{exception2.Message}'");
                }
            }
            if (resultHandler != null)
            {
                status.SetOperation(TestOperation.Complete);
                status.SetEntry(null);
                resultHandler(status, null);
            }
            return (status.ErrorCount == 0);
        }

        private long TestLocalHeader(ZipEntry entry, HeaderTest tests)
        {
            lock (this.baseStream_)
            {
                bool flag = (tests & HeaderTest.Header) != 0;
                bool flag2 = (tests & HeaderTest.Extract) != 0;
                this.baseStream_.Seek(this.offsetOfFirstEntry + entry.Offset, SeekOrigin.Begin);
                if (this.ReadLEUint() != 0x4034b50)
                {
                    throw new ZipException($"Wrong local header signature @{this.offsetOfFirstEntry + entry.Offset:X}");
                }
                short num = (short) this.ReadLEUshort();
                short flags = (short) this.ReadLEUshort();
                short num3 = (short) this.ReadLEUshort();
                short num4 = (short) this.ReadLEUshort();
                short num5 = (short) this.ReadLEUshort();
                uint num6 = this.ReadLEUint();
                long num7 = this.ReadLEUint();
                long num8 = this.ReadLEUint();
                int num9 = this.ReadLEUshort();
                int num10 = this.ReadLEUshort();
                byte[] buffer = new byte[num9];
                StreamUtils.ReadFully(this.baseStream_, buffer);
                byte[] buffer2 = new byte[num10];
                StreamUtils.ReadFully(this.baseStream_, buffer2);
                ZipExtraData data = new ZipExtraData(buffer2);
                if (data.Find(1))
                {
                    if (num < 0x2d)
                    {
                        throw new ZipException($"Extra data contains Zip64 information but version {num / 10}.{num % 10} is not high enough");
                    }
                    if ((((uint) num8) != uint.MaxValue) && (((uint) num7) != uint.MaxValue))
                    {
                        throw new ZipException("Entry sizes not correct for Zip64");
                    }
                    num8 = data.ReadLong();
                    num7 = data.ReadLong();
                }
                else if ((num >= 0x2d) && ((((uint) num8) == uint.MaxValue) || (((uint) num7) == uint.MaxValue)))
                {
                    throw new ZipException("Required Zip64 extended information missing");
                }
                if (flag2 && entry.IsFile)
                {
                    if (!entry.IsCompressionMethodSupported())
                    {
                        throw new ZipException("Compression method not supported");
                    }
                    if ((num > 0x2d) || ((num > 20) && (num < 0x2d)))
                    {
                        throw new ZipException($"Version required to extract this entry not supported ({num})");
                    }
                    if ((flags & 0x3060) != 0)
                    {
                        throw new ZipException("The library does not support the zip version required to extract this entry");
                    }
                }
                if (flag)
                {
                    if (((((num <= 0x3f) && (num != 10)) && ((num != 11) && (num != 20))) && (((num != 0x15) && (num != 0x19)) && ((num != 0x1b) && (num != 0x2d)))) && ((((num != 0x2e) && (num != 50)) && ((num != 0x33) && (num != 0x34))) && (((num != 0x3d) && (num != 0x3e)) && (num != 0x3f))))
                    {
                        throw new ZipException($"Version required to extract this entry is invalid ({num})");
                    }
                    if ((flags & 0xc010) != 0)
                    {
                        throw new ZipException("Reserved bit flags cannot be set.");
                    }
                    if (((flags & 1) != 0) && (num < 20))
                    {
                        throw new ZipException($"Version required to extract this entry is too low for encryption ({num})");
                    }
                    if ((flags & 0x40) != 0)
                    {
                        if ((flags & 1) == 0)
                        {
                            throw new ZipException("Strong encryption flag set but encryption flag is not set");
                        }
                        if (num < 50)
                        {
                            throw new ZipException($"Version required to extract this entry is too low for encryption ({num})");
                        }
                    }
                    if (((flags & 0x20) != 0) && (num < 0x1b))
                    {
                        throw new ZipException($"Patched data requires higher version than ({num})");
                    }
                    if (flags != entry.Flags)
                    {
                        throw new ZipException("Central header/local header flags mismatch");
                    }
                    if (entry.CompressionMethod != ((CompressionMethod) num3))
                    {
                        throw new ZipException("Central header/local header compression method mismatch");
                    }
                    if (((flags & 0x40) != 0) && (num < 0x3e))
                    {
                        throw new ZipException("Strong encryption flag set but version not high enough");
                    }
                    if (((flags & 0x2000) != 0) && ((num4 != 0) || (num5 != 0)))
                    {
                        throw new ZipException("Header masked set but date/time values non-zero");
                    }
                    if (((flags & 8) == 0) && (num6 != ((uint) entry.Crc)))
                    {
                        throw new ZipException("Central header/local header crc mismatch");
                    }
                    if (((num8 == 0L) && (num7 == 0L)) && (num6 != 0))
                    {
                        throw new ZipException("Invalid CRC for empty entry");
                    }
                    if (entry.Name.Length > num9)
                    {
                        throw new ZipException("File name length mismatch");
                    }
                    string name = ZipConstants.ConvertToStringExt(flags, buffer);
                    if (name != entry.Name)
                    {
                        throw new ZipException("Central header and local header file name mismatch");
                    }
                    if (entry.IsDirectory && ((num7 != 0L) || (num8 != 0L)))
                    {
                        throw new ZipException("Directory cannot have size");
                    }
                    if (!ZipNameTransform.IsValidName(name, true))
                    {
                        throw new ZipException("Name is invalid");
                    }
                }
                if ((((flags & 8) == 0) || (num8 != 0L)) || (num7 != 0L))
                {
                    if (num8 != entry.Size)
                    {
                        throw new ZipException($"Size mismatch between central header({entry.Size}) and local header({num8})");
                    }
                    if (num7 != entry.CompressedSize)
                    {
                        throw new ZipException($"Compressed size mismatch between central header({entry.CompressedSize}) and local header({num7})");
                    }
                }
                int num11 = num9 + num10;
                return (((this.offsetOfFirstEntry + entry.Offset) + 30L) + num11);
            }
        }

        private void UpdateCommentOnly()
        {
            long length = this.baseStream_.Length;
            ZipHelperStream stream = null;
            if (this.archiveStorage_.UpdateMode == FileUpdateMode.Safe)
            {
                stream = new ZipHelperStream(this.archiveStorage_.MakeTemporaryCopy(this.baseStream_)) {
                    IsStreamOwner = true
                };
                this.baseStream_.Close();
                this.baseStream_ = null;
            }
            else if (this.archiveStorage_.UpdateMode == FileUpdateMode.Direct)
            {
                this.baseStream_ = this.archiveStorage_.OpenForDirectUpdate(this.baseStream_);
                stream = new ZipHelperStream(this.baseStream_);
            }
            else
            {
                this.baseStream_.Close();
                this.baseStream_ = null;
                stream = new ZipHelperStream(this.Name);
            }
            using (stream)
            {
                if (stream.LocateBlockWithSignature(0x6054b50, length, 0x16, 0xffff) < 0L)
                {
                    throw new ZipException("Cannot find central directory");
                }
                stream.Position += 0x10L;
                byte[] rawComment = this.newComment_.RawComment;
                stream.WriteLEShort(rawComment.Length);
                stream.Write(rawComment, 0, rawComment.Length);
                stream.SetLength(stream.Position);
            }
            if (this.archiveStorage_.UpdateMode == FileUpdateMode.Safe)
            {
                this.Reopen(this.archiveStorage_.ConvertTemporaryToFinal());
            }
            else
            {
                this.ReadEntries();
            }
        }

        private int WriteCentralDirectoryHeader(ZipEntry entry)
        {
            if (entry.CompressedSize < 0L)
            {
                throw new ZipException("Attempt to write central directory entry with unknown csize");
            }
            if (entry.Size < 0L)
            {
                throw new ZipException("Attempt to write central directory entry with unknown size");
            }
            if (entry.Crc < 0L)
            {
                throw new ZipException("Attempt to write central directory entry with unknown crc");
            }
            this.WriteLEInt(0x2014b50);
            this.WriteLEShort(0x2d);
            this.WriteLEShort(entry.Version);
            this.WriteLEShort(entry.Flags);
            this.WriteLEShort((byte) entry.CompressionMethod);
            this.WriteLEInt((int) entry.DosTime);
            this.WriteLEInt((int) entry.Crc);
            if (entry.IsZip64Forced() || (entry.CompressedSize >= 0xffffffffL))
            {
                this.WriteLEInt(-1);
            }
            else
            {
                this.WriteLEInt((int) (((ulong) entry.CompressedSize) & 0xffffffffL));
            }
            if (entry.IsZip64Forced() || (entry.Size >= 0xffffffffL))
            {
                this.WriteLEInt(-1);
            }
            else
            {
                this.WriteLEInt((int) entry.Size);
            }
            byte[] buffer = ZipConstants.ConvertToArray(entry.Flags, entry.Name);
            if (buffer.Length > 0xffff)
            {
                throw new ZipException("Entry name is too long.");
            }
            this.WriteLEShort(buffer.Length);
            ZipExtraData data = new ZipExtraData(entry.ExtraData);
            if (entry.CentralHeaderRequiresZip64)
            {
                data.StartNewEntry();
                if ((entry.Size >= 0xffffffffL) || (this.useZip64_ == ICSharpCode.SharpZipLib.Zip.UseZip64.On))
                {
                    data.AddLeLong(entry.Size);
                }
                if ((entry.CompressedSize >= 0xffffffffL) || (this.useZip64_ == ICSharpCode.SharpZipLib.Zip.UseZip64.On))
                {
                    data.AddLeLong(entry.CompressedSize);
                }
                if (entry.Offset >= 0xffffffffL)
                {
                    data.AddLeLong(entry.Offset);
                }
                data.AddNewEntry(1);
            }
            else
            {
                data.Delete(1);
            }
            byte[] entryData = data.GetEntryData();
            this.WriteLEShort(entryData.Length);
            this.WriteLEShort((entry.Comment != null) ? entry.Comment.Length : 0);
            this.WriteLEShort(0);
            this.WriteLEShort(0);
            if (entry.ExternalFileAttributes != -1)
            {
                this.WriteLEInt(entry.ExternalFileAttributes);
            }
            else if (entry.IsDirectory)
            {
                this.WriteLEUint(0x10);
            }
            else
            {
                this.WriteLEUint(0);
            }
            if (entry.Offset >= 0xffffffffL)
            {
                this.WriteLEUint(uint.MaxValue);
            }
            else
            {
                this.WriteLEUint((uint) ((int) entry.Offset));
            }
            if (buffer.Length > 0)
            {
                this.baseStream_.Write(buffer, 0, buffer.Length);
            }
            if (entryData.Length > 0)
            {
                this.baseStream_.Write(entryData, 0, entryData.Length);
            }
            byte[] buffer3 = (entry.Comment != null) ? Encoding.ASCII.GetBytes(entry.Comment) : new byte[0];
            if (buffer3.Length > 0)
            {
                this.baseStream_.Write(buffer3, 0, buffer3.Length);
            }
            return (((0x2e + buffer.Length) + entryData.Length) + buffer3.Length);
        }

        private static void WriteEncryptionHeader(Stream stream, long crcValue)
        {
            byte[] buffer = new byte[12];
            new Random().NextBytes(buffer);
            buffer[11] = (byte) (crcValue >> 0x18);
            stream.Write(buffer, 0, buffer.Length);
        }

        private void WriteLEInt(int value)
        {
            this.WriteLEShort(value & 0xffff);
            this.WriteLEShort(value >> 0x10);
        }

        private void WriteLeLong(long value)
        {
            this.WriteLEInt((int) (((ulong) value) & 0xffffffffL));
            this.WriteLEInt((int) (value >> 0x20));
        }

        private void WriteLEShort(int value)
        {
            this.baseStream_.WriteByte((byte) (value & 0xff));
            this.baseStream_.WriteByte((byte) ((value >> 8) & 0xff));
        }

        private void WriteLEUint(uint value)
        {
            this.WriteLEUshort((ushort) (value & 0xffff));
            this.WriteLEUshort((ushort) (value >> 0x10));
        }

        private void WriteLEUlong(ulong value)
        {
            this.WriteLEUint((uint) (value & 0xffffffffL));
            this.WriteLEUint((uint) (value >> 0x20));
        }

        private void WriteLEUshort(ushort value)
        {
            this.baseStream_.WriteByte((byte) (value & 0xff));
            this.baseStream_.WriteByte((byte) (value >> 8));
        }

        private void WriteLocalEntryHeader(ZipUpdate update)
        {
            ZipEntry outEntry = update.OutEntry;
            outEntry.Offset = this.baseStream_.Position;
            if (update.Command != UpdateCommand.Copy)
            {
                if (outEntry.CompressionMethod == CompressionMethod.Deflated)
                {
                    if (outEntry.Size == 0L)
                    {
                        outEntry.CompressedSize = outEntry.Size;
                        outEntry.Crc = 0L;
                        outEntry.CompressionMethod = CompressionMethod.Stored;
                    }
                }
                else if (outEntry.CompressionMethod == CompressionMethod.Stored)
                {
                    outEntry.Flags &= -9;
                }
                if (this.HaveKeys)
                {
                    outEntry.IsCrypted = true;
                    if (outEntry.Crc < 0L)
                    {
                        outEntry.Flags |= 8;
                    }
                }
                else
                {
                    outEntry.IsCrypted = false;
                }
                switch (this.useZip64_)
                {
                    case ICSharpCode.SharpZipLib.Zip.UseZip64.On:
                        outEntry.ForceZip64();
                        break;

                    case ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic:
                        if (outEntry.Size < 0L)
                        {
                            outEntry.ForceZip64();
                        }
                        break;
                }
            }
            this.WriteLEInt(0x4034b50);
            this.WriteLEShort(outEntry.Version);
            this.WriteLEShort(outEntry.Flags);
            this.WriteLEShort((byte) outEntry.CompressionMethod);
            this.WriteLEInt((int) outEntry.DosTime);
            if (!outEntry.HasCrc)
            {
                update.CrcPatchOffset = this.baseStream_.Position;
                this.WriteLEInt(0);
            }
            else
            {
                this.WriteLEInt((int) outEntry.Crc);
            }
            if (outEntry.LocalHeaderRequiresZip64)
            {
                this.WriteLEInt(-1);
                this.WriteLEInt(-1);
            }
            else
            {
                if ((outEntry.CompressedSize < 0L) || (outEntry.Size < 0L))
                {
                    update.SizePatchOffset = this.baseStream_.Position;
                }
                this.WriteLEInt((int) outEntry.CompressedSize);
                this.WriteLEInt((int) outEntry.Size);
            }
            byte[] buffer = ZipConstants.ConvertToArray(outEntry.Flags, outEntry.Name);
            if (buffer.Length > 0xffff)
            {
                throw new ZipException("Entry name too long.");
            }
            ZipExtraData data = new ZipExtraData(outEntry.ExtraData);
            if (outEntry.LocalHeaderRequiresZip64)
            {
                data.StartNewEntry();
                data.AddLeLong(outEntry.Size);
                data.AddLeLong(outEntry.CompressedSize);
                data.AddNewEntry(1);
            }
            else
            {
                data.Delete(1);
            }
            outEntry.ExtraData = data.GetEntryData();
            this.WriteLEShort(buffer.Length);
            this.WriteLEShort(outEntry.ExtraData.Length);
            if (buffer.Length > 0)
            {
                this.baseStream_.Write(buffer, 0, buffer.Length);
            }
            if (outEntry.LocalHeaderRequiresZip64)
            {
                if (!data.Find(1))
                {
                    throw new ZipException("Internal error cannot find extra data");
                }
                update.SizePatchOffset = this.baseStream_.Position + data.CurrentReadIndex;
            }
            if (outEntry.ExtraData.Length > 0)
            {
                this.baseStream_.Write(outEntry.ExtraData, 0, outEntry.ExtraData.Length);
            }
        }

        public int BufferSize
        {
            get => 
                this.bufferSize_;
            set
            {
                if (value < 0x400)
                {
                    throw new ArgumentOutOfRangeException("value", "cannot be below 1024");
                }
                if (this.bufferSize_ != value)
                {
                    this.bufferSize_ = value;
                    this.copyBuffer_ = null;
                }
            }
        }

        public long Count
        {
            get
            {
                if (this.entries_ == null)
                {
                    throw new InvalidOperationException("ZipFile is closed");
                }
                return (long) this.entries_.Length;
            }
        }

        public ZipEntry this[int index] =>
            ((ZipEntry) this.entries_[index].Clone());

        public IEntryFactory EntryFactory
        {
            get => 
                this.updateEntryFactory_;
            set
            {
                if (value == null)
                {
                    this.updateEntryFactory_ = new ZipEntryFactory();
                }
                else
                {
                    this.updateEntryFactory_ = value;
                }
            }
        }

        private bool HaveKeys =>
            (this.key != null);

        public bool IsEmbeddedArchive =>
            (this.offsetOfFirstEntry > 0L);

        public bool IsNewArchive =>
            this.isNewArchive_;

        public bool IsStreamOwner
        {
            get => 
                this.isStreamOwner;
            set
            {
                this.isStreamOwner = value;
            }
        }

        public bool IsUpdating =>
            (this.updates_ != null);

        private byte[] Key
        {
            get => 
                this.key;
            set
            {
                this.key = value;
            }
        }

        public string Name =>
            this.name_;

        public INameTransform NameTransform
        {
            get => 
                this.updateEntryFactory_.NameTransform;
            set
            {
                this.updateEntryFactory_.NameTransform = value;
            }
        }

        public string Password
        {
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    this.key = null;
                }
                else
                {
                    this.key = PkzipClassic.GenerateKeys(ZipConstants.ConvertToArray(value));
                }
            }
        }

        [Obsolete("Use the Count property instead")]
        public int Size =>
            this.entries_?.Length;

        public ICSharpCode.SharpZipLib.Zip.UseZip64 UseZip64
        {
            get => 
                this.useZip64_;
            set
            {
                this.useZip64_ = value;
            }
        }

        public string ZipFileComment =>
            this.comment_;

        [Flags]
        private enum HeaderTest
        {
            Extract = 1,
            Header = 2
        }

        public delegate void KeysRequiredEventHandler(object sender, KeysRequiredEventArgs e);

        private class PartialInputStream : InflaterInputStream
        {
            private Stream baseStream_;
            private long end_;
            private long filepos_;

            public PartialInputStream(Stream baseStream, long start, long length) : base(baseStream)
            {
                this.baseStream_ = baseStream;
                this.filepos_ = start;
                this.end_ = start + length;
            }

            public override void Close()
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (count > (this.end_ - this.filepos_))
                {
                    count = (int) (this.end_ - this.filepos_);
                    if (count == 0)
                    {
                        return 0;
                    }
                }
                lock (this.baseStream_)
                {
                    this.baseStream_.Seek(this.filepos_, SeekOrigin.Begin);
                    int num = this.baseStream_.Read(buffer, offset, count);
                    if (num > 0)
                    {
                        this.filepos_ += num;
                    }
                    return num;
                }
            }

            public override int ReadByte()
            {
                if (this.filepos_ == this.end_)
                {
                    return -1;
                }
                lock (this.baseStream_)
                {
                    long num2;
                    this.filepos_ = (num2 = this.filepos_) + 1L;
                    this.baseStream_.Seek(num2, SeekOrigin.Begin);
                    return this.baseStream_.ReadByte();
                }
            }

            public long SkipBytes(long count)
            {
                if (count < 0L)
                {
                    throw new ArgumentOutOfRangeException("count", "is less than zero");
                }
                if (count > (this.end_ - this.filepos_))
                {
                    count = this.end_ - this.filepos_;
                }
                this.filepos_ += count;
                return count;
            }

            public override int Available
            {
                get
                {
                    long num = this.end_ - this.filepos_;
                    if (num > 0x7fffffffL)
                    {
                        return 0x7fffffff;
                    }
                    return (int) num;
                }
            }
        }

        private class UncompressedStream : Stream
        {
            private Stream baseStream_;

            public UncompressedStream(Stream baseStream)
            {
                this.baseStream_ = baseStream;
            }

            public override void Close()
            {
            }

            public override void Flush()
            {
                this.baseStream_.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count) => 
                0;

            public override long Seek(long offset, SeekOrigin origin) => 
                0L;

            public override void SetLength(long value)
            {
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                this.baseStream_.Write(buffer, offset, count);
            }

            public override bool CanRead =>
                false;

            public override bool CanSeek =>
                false;

            public override bool CanWrite =>
                this.baseStream_.CanWrite;

            public override long Length =>
                0L;

            public override long Position
            {
                get => 
                    this.baseStream_.Position;
                set
                {
                }
            }
        }

        private enum UpdateCommand
        {
            Copy,
            Modify,
            Add
        }

        private class UpdateComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                ZipFile.ZipUpdate update = x as ZipFile.ZipUpdate;
                ZipFile.ZipUpdate update2 = y as ZipFile.ZipUpdate;
                if (update == null)
                {
                    if (update2 == null)
                    {
                        return 0;
                    }
                    return -1;
                }
                if (update2 != null)
                {
                    int num2 = ((update.Command == ZipFile.UpdateCommand.Copy) || (update.Command == ZipFile.UpdateCommand.Modify)) ? 0 : 1;
                    int num3 = ((update2.Command == ZipFile.UpdateCommand.Copy) || (update2.Command == ZipFile.UpdateCommand.Modify)) ? 0 : 1;
                    int num = num2 - num3;
                    if (num != 0)
                    {
                        return num;
                    }
                    long num4 = update.Entry.Offset - update2.Entry.Offset;
                    if (num4 < 0L)
                    {
                        return -1;
                    }
                    if (num4 == 0L)
                    {
                        return 0;
                    }
                }
                return 1;
            }
        }

        private class ZipEntryEnumerator : IEnumerator
        {
            private ZipEntry[] array;
            private int index = -1;

            public ZipEntryEnumerator(ZipEntry[] entries)
            {
                this.array = entries;
            }

            public bool MoveNext() => 
                (++this.index < this.array.Length);

            public void Reset()
            {
                this.index = -1;
            }

            public object Current =>
                this.array[this.index];
        }

        private class ZipString
        {
            private string comment_;
            private bool isSourceString_;
            private byte[] rawComment_;

            public ZipString(string comment)
            {
                this.comment_ = comment;
                this.isSourceString_ = true;
            }

            public ZipString(byte[] rawString)
            {
                this.rawComment_ = rawString;
            }

            private void MakeBytesAvailable()
            {
                if (this.rawComment_ == null)
                {
                    this.rawComment_ = ZipConstants.ConvertToArray(this.comment_);
                }
            }

            private void MakeTextAvailable()
            {
                if (this.comment_ == null)
                {
                    this.comment_ = ZipConstants.ConvertToString(this.rawComment_);
                }
            }

            public static implicit operator string(ZipFile.ZipString zipString)
            {
                zipString.MakeTextAvailable();
                return zipString.comment_;
            }

            public void Reset()
            {
                if (this.isSourceString_)
                {
                    this.rawComment_ = null;
                }
                else
                {
                    this.comment_ = null;
                }
            }

            public bool IsSourceString =>
                this.isSourceString_;

            public byte[] RawComment
            {
                get
                {
                    this.MakeBytesAvailable();
                    return (byte[]) this.rawComment_.Clone();
                }
            }

            public int RawLength
            {
                get
                {
                    this.MakeBytesAvailable();
                    return this.rawComment_.Length;
                }
            }
        }

        private class ZipUpdate
        {
            private ZipFile.UpdateCommand command_;
            private long crcPatchOffset_;
            private IStaticDataSource dataSource_;
            private ZipEntry entry_;
            private string filename_;
            private ZipEntry outEntry_;
            private long sizePatchOffset_;

            public ZipUpdate(ZipEntry entry) : this(ZipFile.UpdateCommand.Copy, entry)
            {
            }

            public ZipUpdate(IStaticDataSource dataSource, ZipEntry entry)
            {
                this.sizePatchOffset_ = -1L;
                this.crcPatchOffset_ = -1L;
                this.command_ = ZipFile.UpdateCommand.Add;
                this.entry_ = entry;
                this.dataSource_ = dataSource;
            }

            public ZipUpdate(ZipEntry original, ZipEntry updated)
            {
                this.sizePatchOffset_ = -1L;
                this.crcPatchOffset_ = -1L;
                throw new ZipException("Modify not currently supported");
            }

            public ZipUpdate(ZipFile.UpdateCommand command, ZipEntry entry)
            {
                this.sizePatchOffset_ = -1L;
                this.crcPatchOffset_ = -1L;
                this.command_ = command;
                this.entry_ = (ZipEntry) entry.Clone();
            }

            public ZipUpdate(string fileName, ZipEntry entry)
            {
                this.sizePatchOffset_ = -1L;
                this.crcPatchOffset_ = -1L;
                this.command_ = ZipFile.UpdateCommand.Add;
                this.entry_ = entry;
                this.filename_ = fileName;
            }

            [Obsolete]
            public ZipUpdate(string fileName, string entryName) : this(fileName, entryName, CompressionMethod.Deflated)
            {
            }

            [Obsolete]
            public ZipUpdate(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod)
            {
                this.sizePatchOffset_ = -1L;
                this.crcPatchOffset_ = -1L;
                this.command_ = ZipFile.UpdateCommand.Add;
                this.entry_ = new ZipEntry(entryName);
                this.entry_.CompressionMethod = compressionMethod;
                this.dataSource_ = dataSource;
            }

            [Obsolete]
            public ZipUpdate(string fileName, string entryName, CompressionMethod compressionMethod)
            {
                this.sizePatchOffset_ = -1L;
                this.crcPatchOffset_ = -1L;
                this.command_ = ZipFile.UpdateCommand.Add;
                this.entry_ = new ZipEntry(entryName);
                this.entry_.CompressionMethod = compressionMethod;
                this.filename_ = fileName;
            }

            public Stream GetSource()
            {
                Stream source = null;
                if (this.dataSource_ != null)
                {
                    source = this.dataSource_.GetSource();
                }
                return source;
            }

            public ZipFile.UpdateCommand Command =>
                this.command_;

            public long CrcPatchOffset
            {
                get => 
                    this.crcPatchOffset_;
                set
                {
                    this.crcPatchOffset_ = value;
                }
            }

            public ZipEntry Entry =>
                this.entry_;

            public string Filename =>
                this.filename_;

            public ZipEntry OutEntry
            {
                get
                {
                    if (this.outEntry_ == null)
                    {
                        this.outEntry_ = (ZipEntry) this.entry_.Clone();
                    }
                    return this.outEntry_;
                }
            }

            public long SizePatchOffset
            {
                get => 
                    this.sizePatchOffset_;
                set
                {
                    this.sizePatchOffset_ = value;
                }
            }
        }
    }
}

