namespace ICSharpCode.SharpZipLib.BZip2
{
    using System;
    using System.IO;

    public sealed class BZip2
    {
        private BZip2()
        {
        }

        public static void Compress(Stream inStream, Stream outStream, int blockSize)
        {
            if (inStream == null)
            {
                throw new ArgumentNullException("inStream");
            }
            if (outStream == null)
            {
                throw new ArgumentNullException("outStream");
            }
            using (inStream)
            {
                using (BZip2OutputStream stream = new BZip2OutputStream(outStream, blockSize))
                {
                    for (int i = inStream.ReadByte(); i != -1; i = inStream.ReadByte())
                    {
                        stream.WriteByte((byte) i);
                    }
                }
            }
        }

        public static void Decompress(Stream inStream, Stream outStream)
        {
            if (inStream == null)
            {
                throw new ArgumentNullException("inStream");
            }
            if (outStream == null)
            {
                throw new ArgumentNullException("outStream");
            }
            using (outStream)
            {
                using (BZip2InputStream stream = new BZip2InputStream(inStream))
                {
                    for (int i = stream.ReadByte(); i != -1; i = stream.ReadByte())
                    {
                        outStream.WriteByte((byte) i);
                    }
                }
            }
        }
    }
}

