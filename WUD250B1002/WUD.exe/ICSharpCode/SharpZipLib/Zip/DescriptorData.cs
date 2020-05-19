namespace ICSharpCode.SharpZipLib.Zip
{
    using System;

    public class DescriptorData
    {
        private long compressedSize;
        private long crc;
        private long size;

        public long CompressedSize
        {
            get => 
                this.compressedSize;
            set
            {
                this.compressedSize = value;
            }
        }

        public long Crc
        {
            get => 
                this.crc;
            set
            {
                this.crc = value & ((long) 0xffffffffL);
            }
        }

        public long Size
        {
            get => 
                this.size;
            set
            {
                this.size = value;
            }
        }
    }
}

