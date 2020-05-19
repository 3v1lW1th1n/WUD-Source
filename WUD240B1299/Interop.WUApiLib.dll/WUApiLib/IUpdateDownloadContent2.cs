namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10c0), Guid("C97AD11B-F257-420B-9D9F-377F733F6F68")]
    public interface IUpdateDownloadContent2 : IUpdateDownloadContent
    {
        [DispId(0x60020001)]
        string DownloadUrl { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60030001)]
        bool IsDeltaCompressedContent { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] get; }
    }
}

