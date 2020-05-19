namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("23857E3C-02BA-44A3-9423-B1C900805F37"), TypeLibType((short) 0x10c0)]
    public interface IUpdateServiceManager
    {
        [DispId(0x60020001)]
        IUpdateServiceCollection Services { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)]
        IUpdateService AddService([In, MarshalAs(UnmanagedType.BStr)] string ServiceID, [In, MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)]
        void RegisterServiceWithAU([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)]
        void RemoveService([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)]
        void UnregisterServiceWithAU([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020006)]
        IUpdateService AddScanPackageService([In, MarshalAs(UnmanagedType.BStr)] string serviceName, [In, MarshalAs(UnmanagedType.BStr)] string scanFileLocation, [In] int flags = 0);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60010007)]
        void SetOption([In, MarshalAs(UnmanagedType.BStr)] string optionName, [In, MarshalAs(UnmanagedType.Struct)] object optionValue);
    }
}

