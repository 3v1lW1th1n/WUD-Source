namespace WUApiLib
{
    using System.Runtime.InteropServices;

    [ComImport, CoClass(typeof(UpdateSessionClass)), Guid("918EFD1E-B5D8-4C90-8540-AEB9BDC56F9D")]
    public interface UpdateSession : IUpdateSession3
    {
    }
}

