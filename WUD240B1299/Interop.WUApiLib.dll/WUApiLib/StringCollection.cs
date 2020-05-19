namespace WUApiLib
{
    using System.Runtime.InteropServices;

    [ComImport, CoClass(typeof(StringCollectionClass)), Guid("EFF90582-2DDC-480F-A06D-60F3FBC362C3")]
    public interface StringCollection : IStringCollection
    {
    }
}

