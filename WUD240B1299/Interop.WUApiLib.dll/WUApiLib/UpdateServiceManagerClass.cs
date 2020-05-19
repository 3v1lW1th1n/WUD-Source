namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 2), ClassInterface((short) 0), Guid("F8D253D9-89A4-4DAA-87B6-1168369F0B21")]
    public class UpdateServiceManagerClass : IUpdateServiceManager2, UpdateServiceManager
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020006)]
        public virtual extern IUpdateService AddScanPackageService([In, MarshalAs(UnmanagedType.BStr)] string serviceName, [In, MarshalAs(UnmanagedType.BStr)] string scanFileLocation, [In] int flags = 0);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)]
        public virtual extern IUpdateService AddService([In, MarshalAs(UnmanagedType.BStr)] string ServiceID, [In, MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030003)]
        public virtual extern IUpdateServiceRegistration AddService2([In, MarshalAs(UnmanagedType.BStr)] string ServiceID, [In] int flags, [In, MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030002)]
        public virtual extern IUpdateServiceRegistration QueryServiceRegistration([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)]
        public virtual extern void RegisterServiceWithAU([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)]
        public virtual extern void RemoveService([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60010007)]
        public virtual extern void SetOption([In, MarshalAs(UnmanagedType.BStr)] string optionName, [In, MarshalAs(UnmanagedType.Struct)] object optionValue);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)]
        public virtual extern void UnregisterServiceWithAU([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);

        [DispId(0x60030001)]
        public virtual string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] set; }

        [DispId(0x60020001)]
        public virtual IUpdateServiceCollection Services { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }

        [DispId(0x60030001)]
        public virtual string WUApiLib.IUpdateServiceManager2.ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] set; }

        [DispId(0x60020001)]
        public virtual IUpdateServiceCollection WUApiLib.IUpdateServiceManager2.Services { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }
}

