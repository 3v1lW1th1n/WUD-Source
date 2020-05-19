namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 2), ClassInterface((short) 0), Guid("5BAF654A-5A07-4264-A255-9FF54C7151E7")]
    public class UpdateDownloaderClass : IUpdateDownloader, UpdateDownloader
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)]
        public virtual extern IDownloadJob BeginDownload([In, MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In, MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In, MarshalAs(UnmanagedType.Struct)] object state);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020006)]
        public virtual extern IDownloadResult Download();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020007)]
        public virtual extern IDownloadResult EndDownload([In, MarshalAs(UnmanagedType.Interface)] IDownloadJob value);

        [DispId(0x60020001)]
        public virtual string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] set; }

        [DispId(0x60020002)]
        public virtual bool IsForced { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] set; }

        [ComAliasName("WUApiLib.DownloadPriority"), DispId(0x60020003)]
        public virtual DownloadPriority Priority { [return: ComAliasName("WUApiLib.DownloadPriority")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] get; [param: In, ComAliasName("WUApiLib.DownloadPriority")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] set; }

        [DispId(0x60020004)]
        public virtual UpdateCollection Updates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] set; }

        [DispId(0x60020001)]
        public virtual string WUApiLib.IUpdateDownloader.ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] set; }

        [DispId(0x60020002)]
        public virtual bool WUApiLib.IUpdateDownloader.IsForced { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] set; }

        [DispId(0x60020003), ComAliasName("WUApiLib.DownloadPriority")]
        public virtual DownloadPriority WUApiLib.IUpdateDownloader.Priority { [return: ComAliasName("WUApiLib.DownloadPriority")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] get; [param: In, ComAliasName("WUApiLib.DownloadPriority")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] set; }

        [DispId(0x60020004)]
        public virtual UpdateCollection WUApiLib.IUpdateDownloader.Updates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] set; }
    }
}

