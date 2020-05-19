namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("925CBC18-A2EA-4648-BF1C-EC8BADCFE20A"), TypeLibType((short) 0x10c0)]
    public interface IInstallationAgent
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)]
        void RecordInstallationResult([In, MarshalAs(UnmanagedType.BStr)] string installationResultCookie, [In] int HResult, [In, MarshalAs(UnmanagedType.Interface)] StringCollection extendedReportingData);
    }
}

