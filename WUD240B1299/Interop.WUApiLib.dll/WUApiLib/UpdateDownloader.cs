namespace WUApiLib
{
    using System.Runtime.InteropServices;

    [ComImport, CoClass(typeof(UpdateDownloaderClass)), Guid("68F1C6F9-7ECC-4666-A464-247FE12496C3")]
    public interface UpdateDownloader : IUpdateDownloader
    {
    }
}

