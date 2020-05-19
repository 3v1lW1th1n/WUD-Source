namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    public sealed class ZipExtraData : IDisposable
    {
        private byte[] data_;
        private int index_;
        private MemoryStream newEntry_;
        private int readValueLength_;
        private int readValueStart_;

        public ZipExtraData()
        {
            this.Clear();
        }

        public ZipExtraData(byte[] data)
        {
            if (data == null)
            {
                this.data_ = new byte[0];
            }
            else
            {
                this.data_ = data;
            }
        }

        public void AddData(byte data)
        {
            this.newEntry_.WriteByte(data);
        }

        public void AddData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.newEntry_.Write(data, 0, data.Length);
        }

        public void AddEntry(ITaggedData taggedData)
        {
            if (taggedData == null)
            {
                throw new ArgumentNullException("taggedData");
            }
            this.AddEntry(taggedData.TagID, taggedData.GetData());
        }

        public void AddEntry(int headerID, byte[] fieldData)
        {
            if ((headerID > 0xffff) || (headerID < 0))
            {
                throw new ArgumentOutOfRangeException("headerID");
            }
            int source = (fieldData == null) ? 0 : fieldData.Length;
            if (source > 0xffff)
            {
                throw new ArgumentOutOfRangeException("fieldData", "exceeds maximum length");
            }
            int num2 = (this.data_.Length + source) + 4;
            if (this.Find(headerID))
            {
                num2 -= this.ValueLength + 4;
            }
            if (num2 > 0xffff)
            {
                throw new ZipException("Data exceeds maximum length");
            }
            this.Delete(headerID);
            byte[] array = new byte[num2];
            this.data_.CopyTo(array, 0);
            int length = this.data_.Length;
            this.data_ = array;
            this.SetShort(ref length, headerID);
            this.SetShort(ref length, source);
            if (fieldData != null)
            {
                fieldData.CopyTo(array, length);
            }
        }

        public void AddLeInt(int toAdd)
        {
            this.AddLeShort((short) toAdd);
            this.AddLeShort((short) (toAdd >> 0x10));
        }

        public void AddLeLong(long toAdd)
        {
            this.AddLeInt((int) (((ulong) toAdd) & 0xffffffffL));
            this.AddLeInt((int) (toAdd >> 0x20));
        }

        public void AddLeShort(int toAdd)
        {
            this.newEntry_.WriteByte((byte) toAdd);
            this.newEntry_.WriteByte((byte) (toAdd >> 8));
        }

        public void AddNewEntry(int headerID)
        {
            byte[] fieldData = this.newEntry_.ToArray();
            this.newEntry_ = null;
            this.AddEntry(headerID, fieldData);
        }

        public void Clear()
        {
            if ((this.data_ == null) || (this.data_.Length != 0))
            {
                this.data_ = new byte[0];
            }
        }

        private ITaggedData Create(short tag, byte[] data, int offset, int count)
        {
            ITaggedData data2 = null;
            switch (tag)
            {
                case 10:
                    data2 = new NTTaggedData();
                    break;

                case 0x5455:
                    data2 = new ExtendedUnixData();
                    break;

                default:
                    data2 = new RawTaggedData(tag);
                    break;
            }
            data2.SetData(this.data_, this.readValueStart_, this.readValueLength_);
            return data2;
        }

        public bool Delete(int headerID)
        {
            bool flag = false;
            if (this.Find(headerID))
            {
                flag = true;
                int length = this.readValueStart_ - 4;
                byte[] destinationArray = new byte[this.data_.Length - (this.ValueLength + 4)];
                Array.Copy(this.data_, 0, destinationArray, 0, length);
                int sourceIndex = (length + this.ValueLength) + 4;
                Array.Copy(this.data_, sourceIndex, destinationArray, length, this.data_.Length - sourceIndex);
                this.data_ = destinationArray;
            }
            return flag;
        }

        public void Dispose()
        {
            if (this.newEntry_ != null)
            {
                this.newEntry_.Close();
            }
        }

        public bool Find(int headerID)
        {
            this.readValueStart_ = this.data_.Length;
            this.readValueLength_ = 0;
            this.index_ = 0;
            int num = this.readValueStart_;
            int num2 = headerID - 1;
            while ((num2 != headerID) && (this.index_ < (this.data_.Length - 3)))
            {
                num2 = this.ReadShortInternal();
                num = this.ReadShortInternal();
                if (num2 != headerID)
                {
                    this.index_ += num;
                }
            }
            bool flag = (num2 == headerID) && ((this.index_ + num) <= this.data_.Length);
            if (flag)
            {
                this.readValueStart_ = this.index_;
                this.readValueLength_ = num;
            }
            return flag;
        }

        private ITaggedData GetData(short tag)
        {
            ITaggedData data = null;
            if (this.Find(tag))
            {
                data = this.Create(tag, this.data_, this.readValueStart_, this.readValueLength_);
            }
            return data;
        }

        public byte[] GetEntryData()
        {
            if (this.Length > 0xffff)
            {
                throw new ZipException("Data exceeds maximum length");
            }
            return (byte[]) this.data_.Clone();
        }

        public Stream GetStreamForTag(int tag)
        {
            Stream stream = null;
            if (this.Find(tag))
            {
                stream = new MemoryStream(this.data_, this.index_, this.readValueLength_, false);
            }
            return stream;
        }

        public int ReadByte()
        {
            int num = -1;
            if ((this.index_ < this.data_.Length) && ((this.readValueStart_ + this.readValueLength_) > this.index_))
            {
                num = this.data_[this.index_];
                this.index_++;
            }
            return num;
        }

        private void ReadCheck(int length)
        {
            if ((this.readValueStart_ > this.data_.Length) || (this.readValueStart_ < 4))
            {
                throw new ZipException("Find must be called before calling a Read method");
            }
            if (this.index_ > ((this.readValueStart_ + this.readValueLength_) - length))
            {
                throw new ZipException("End of extra data");
            }
        }

        public int ReadInt()
        {
            this.ReadCheck(4);
            int num = ((this.data_[this.index_] + (this.data_[this.index_ + 1] << 8)) + (this.data_[this.index_ + 2] << 0x10)) + (this.data_[this.index_ + 3] << 0x18);
            this.index_ += 4;
            return num;
        }

        public long ReadLong()
        {
            this.ReadCheck(8);
            return ((this.ReadInt() & ((long) 0xffffffffL)) | (this.ReadInt() << 0x20));
        }

        public int ReadShort()
        {
            this.ReadCheck(2);
            int num = this.data_[this.index_] + (this.data_[this.index_ + 1] << 8);
            this.index_ += 2;
            return num;
        }

        private int ReadShortInternal()
        {
            if (this.index_ > (this.data_.Length - 2))
            {
                throw new ZipException("End of extra data");
            }
            int num = this.data_[this.index_] + (this.data_[this.index_ + 1] << 8);
            this.index_ += 2;
            return num;
        }

        private void SetShort(ref int index, int source)
        {
            this.data_[index] = (byte) source;
            this.data_[index + 1] = (byte) (source >> 8);
            index += 2;
        }

        public void Skip(int amount)
        {
            this.ReadCheck(amount);
            this.index_ += amount;
        }

        public void StartNewEntry()
        {
            this.newEntry_ = new MemoryStream();
        }

        public int CurrentReadIndex =>
            this.index_;

        public int Length =>
            this.data_.Length;

        public int UnreadCount
        {
            get
            {
                if ((this.readValueStart_ > this.data_.Length) || (this.readValueStart_ < 4))
                {
                    throw new ZipException("Find must be called before calling a Read method");
                }
                return ((this.readValueStart_ + this.readValueLength_) - this.index_);
            }
        }

        public int ValueLength =>
            this.readValueLength_;
    }
}

