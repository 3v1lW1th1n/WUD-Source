namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10c0), Guid("D31A5BAC-F719-4178-9DBB-5E2CB47FD18A")]
    public interface IDownloadProgress
    {
        [DispId(0x60020001)]
        decimal CurrentUpdateBytesDownloaded { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        decimal CurrentUpdateBytesToDownload { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        int CurrentUpdateIndex { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        int PercentComplete { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] get; }
        [DispId(0x60020005)]
        decimal TotalBytesDownloaded { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)] get; }
        [DispId(0x60020006)]
        decimal TotalBytesToDownload { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020006)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020007)]
        IUpdateDownloadResult GetUpdateResult([In] int updateIndex);
        [ComAliasName("WUApiLib.DownloadPhase"), DispId(0x60020008)]
        DownloadPhase CurrentUpdateDownloadPhase { [return: ComAliasName("WUApiLib.DownloadPhase")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020008)] get; }
        [DispId(0x60020009)]
        int CurrentUpdatePercentComplete { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020009)] get; }
    }
}

