namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10c0), Guid("D9A59339-E245-4DBD-9686-4D5763E39624")]
    public interface IInstallationBehavior
    {
        [DispId(0x60020001)]
        bool CanRequestUserInput { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [ComAliasName("WUApiLib.InstallationImpact"), DispId(0x60020002)]
        InstallationImpact Impact { [return: ComAliasName("WUApiLib.InstallationImpact")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003), ComAliasName("WUApiLib.InstallationRebootBehavior")]
        InstallationRebootBehavior RebootBehavior { [return: ComAliasName("WUApiLib.InstallationRebootBehavior")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        bool RequiresNetworkConnectivity { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] get; }
    }
}

