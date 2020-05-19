namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10d0), Guid("04C6895D-EAF2-4034-97F3-311DE9BE413A")]
    public interface IUpdateSearcher3 : IUpdateSearcher2
    {
        [DispId(0x60020001)]
        bool CanAutomaticallyUpgradeService { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020003)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] set; }
        [DispId(0x60020004)]
        bool IncludePotentiallySupersededUpdates { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] set; }
        [DispId(0x60020007), ComAliasName("WUApiLib.ServerSelection")]
        WUApiLib.ServerSelection ServerSelection { [return: ComAliasName("WUApiLib.ServerSelection")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020007)] get; [param: In, ComAliasName("WUApiLib.ServerSelection")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020007)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020008)]
        ISearchJob BeginSearch([In, MarshalAs(UnmanagedType.BStr)] string criteria, [In, MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In, MarshalAs(UnmanagedType.Struct)] object state);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020009)]
        ISearchResult EndSearch([In, MarshalAs(UnmanagedType.Interface)] ISearchJob searchJob);
        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000a)]
        string EscapeString([In, MarshalAs(UnmanagedType.BStr)] string unescaped);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000b)]
        IUpdateHistoryEntryCollection QueryHistory([In] int startIndex, [In] int Count);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000c)]
        ISearchResult Search([In, MarshalAs(UnmanagedType.BStr)] string criteria);
        [DispId(0x6002000d)]
        bool Online { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000d)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000d)] set; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000e)]
        int GetTotalHistoryCount();
        [DispId(0x6002000f)]
        string ServiceID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000f)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x6002000f)] set; }
        [DispId(0x60030001)]
        bool IgnoreDownloadPriority { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] set; }
        [DispId(0x60040001), ComAliasName("WUApiLib.SearchScope")]
        WUApiLib.SearchScope SearchScope { [return: ComAliasName("WUApiLib.SearchScope")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60040001)] get; [param: In, ComAliasName("WUApiLib.SearchScope")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60040001)] set; }
    }
}

