namespace WUApiLib
{
    using System.Runtime.InteropServices;

    [ComImport, Guid("ADE87BF7-7B56-4275-8FAB-B9B0E591844B"), CoClass(typeof(SystemInformationClass))]
    public interface SystemInformation : ISystemInformation
    {
    }
}

