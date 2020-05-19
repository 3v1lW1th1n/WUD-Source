namespace WUApiLib
{
    using System.Runtime.InteropServices;

    [ComImport, Guid("04C6895D-EAF2-4034-97F3-311DE9BE413A"), CoClass(typeof(UpdateSearcherClass))]
    public interface UpdateSearcher : IUpdateSearcher3
    {
    }
}

