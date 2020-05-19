namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, ComConversionLoss, Guid("7B929C68-CCDC-4226-96B1-8724600B54C2"), TypeLibType((short) 0x10c0)]
    public interface IUpdateInstaller
    {
        [DispId(0x60020001)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020002)]
        bool IsForced { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] set; }
        [DispId(0x60020003), ComAliasName("WUApiLib.wireHWND")]
        IntPtr ParentHwnd { [return: ComAliasName("WUApiLib.wireHWND")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003), TypeLibFunc((short) 1)] get; [param: In, ComAliasName("WUApiLib.wireHWND")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003), TypeLibFunc((short) 1)] set; }
        [DispId(0x60020004)]
        object parentWindow { [return: MarshalAs(UnmanagedType.IUnknown)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] get; [param: In, MarshalAs(UnmanagedType.IUnknown)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] set; }
        [DispId(0x60020005)]
        UpdateCollection Updates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020006)]
        IInstallationJob BeginInstall([In, MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In, MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In, MarshalAs(UnmanagedType.Struct)] object state);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020007)]
        IInstallationJob BeginUninstall([In, MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In, MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In, MarshalAs(UnmanagedType.Struct)] object state);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020008)]
        IInstallationResult EndInstall([In, MarshalAs(UnmanagedType.Interface)] IInstallationJob value);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020009)]
        IInstallationResult EndUninstall([In, MarshalAs(UnmanagedType.Interface)] IInstallationJob value);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000a)]
        IInstallationResult Install();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000b)]
        IInstallationResult RunWizard([In, MarshalAs(UnmanagedType.BStr)] string dialogTitle = "");
        [DispId(0x6002000c)]
        bool IsBusy { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000c)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000d)]
        IInstallationResult Uninstall();
        [DispId(0x6002000e)]
        bool AllowSourcePrompts { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000e)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000e)] set; }
        [DispId(0x6002000f)]
        bool RebootRequiredBeforeInstallation { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000f)] get; }
    }
}

