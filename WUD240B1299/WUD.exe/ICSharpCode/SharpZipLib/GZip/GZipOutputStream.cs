namespace ICSharpCode.SharpZipLib.GZip
{
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using System;
    using System.IO;

    public class GZipOutputStream : DeflaterOutputStream
    {
        protected Crc32 crc;
        private bool headerWritten_;

        public GZipOutputStream(Stream baseOutputStream) : this(baseOutputStream, 0x1000)
        {
        }

        public GZipOutputStream(Stream baseOutputStream, int size) : base(baseOutputStream, new Deflater(-1, true), size)
        {
            this.crc = new Crc32();
        }

        public override void Close()
        {
            try
            {
                this.Finish();
            }
            finally
            {
                if (base.IsStreamOwner)
                {
                    base.baseOutputStream_.Close();
                }
            }
        }

        public override void Finish()
        {
            if (!this.headerWritten_)
            {
                this.WriteHeader();
            }
            base.Finish();
            int totalIn = base.deflater_.TotalIn;
            int num2 = (int) (((ulong) this.crc.Value) & 0xffffffffL);
            byte[] buffer = new byte[] { (byte) num2, (byte) (num2 >> 8), (byte) (num2 >> 0x10), (byte) (num2 >> 0x18), (byte) totalIn, (byte) (totalIn >> 8), (byte) (totalIn >> 0x10), (byte) (totalIn >> 0x18) };
            base.baseOutputStream_.Write(buffer, 0, buffer.Length);
        }

        public int GetLevel() => 
            base.deflater_.GetLevel();

        public void SetLevel(int level)
        {
            if (level < 1)
            {
                throw new ArgumentOutOfRangeException("level");
            }
            base.deflater_.SetLevel(level);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!this.headerWritten_)
            {
                this.WriteHeader();
            }
            this.crc.Update(buffer, offset, count);
            base.Write(buffer, offset, count);
        }

        private void WriteHeader()
        {
            if (!this.headerWritten_)
            {
                this.headerWritten_ = true;
                DateTime time2 = new DateTime(0x7b2, 1, 1);
                int num = (int) ((DateTime.Now.Ticks - time2.Ticks) / 0x989680L);
                byte[] buffer2 = new byte[] { 0x1f, 0x8b, 8, 0, 0, 0, 0, 0, 0, 0xff };
                buffer2[4] = (byte) num;
                buffer2[5] = (byte) (num >> 8);
                buffer2[6] = (byte) (num >> 0x10);
                buffer2[7] = (byte) (num >> 0x18);
                byte[] buffer = buffer2;
                base.baseOutputStream_.Write(buffer, 0, buffer.Length);
            }
        }
    }
}

