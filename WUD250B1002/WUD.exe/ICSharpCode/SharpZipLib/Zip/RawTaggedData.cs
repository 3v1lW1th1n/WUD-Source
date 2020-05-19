namespace ICSharpCode.SharpZipLib.Zip
{
    using System;

    public class RawTaggedData : ITaggedData
    {
        private byte[] data_;
        protected short tag_;

        public RawTaggedData(short tag)
        {
            this.tag_ = tag;
        }

        public byte[] GetData() => 
            this.data_;

        public void SetData(byte[] data, int offset, int count)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.data_ = new byte[count];
            Array.Copy(data, offset, this.data_, 0, count);
        }

        public byte[] Data
        {
            get => 
                this.data_;
            set
            {
                this.data_ = value;
            }
        }

        public short TagID
        {
            get => 
                this.tag_;
            set
            {
                this.tag_ = value;
            }
        }
    }
}

