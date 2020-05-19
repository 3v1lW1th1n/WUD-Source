namespace WUApiLib
{
    using System.Runtime.InteropServices;

    [ComImport, Guid("0BB8531D-7E8D-424F-986C-A0B8F60A3E7B"), CoClass(typeof(UpdateServiceManagerClass))]
    public interface UpdateServiceManager : IUpdateServiceManager2
    {
    }
}

