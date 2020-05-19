namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10d0), Guid("0BB8531D-7E8D-424F-986C-A0B8F60A3E7B")]
    public interface IUpdateServiceManager2 : IUpdateServiceManager
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
        [DispId(0x60030001)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030002)]
        IUpdateServiceRegistration QueryServiceRegistration([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030003)]
        IUpdateServiceRegistration AddService2([In, MarshalAs(UnmanagedType.BStr)] string ServiceID, [In] int flags, [In, MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);
    }
}

