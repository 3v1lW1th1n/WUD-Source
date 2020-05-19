namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    internal class DynamicDiskDataSource : IDynamicDataSource
    {
        public Stream GetSource(ZipEntry entry, string name)
        {
            Stream stream = null;
            if (name != null)
            {
                stream = File.OpenRead(name);
            }
            return stream;
        }
    }
}

