namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 2), ClassInterface((short) 0), Guid("317E92FC-1679-46FD-A0B5-F08914DD8623")]
    public class InstallationAgentClass : IInstallationAgent, InstallationAgent
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)]
        public virtual extern void RecordInstallationResult([In, MarshalAs(UnmanagedType.BStr)] string installationResultCookie, [In] int HResult, [In, MarshalAs(UnmanagedType.Interface)] StringCollection extendedReportingData);
    }
}

