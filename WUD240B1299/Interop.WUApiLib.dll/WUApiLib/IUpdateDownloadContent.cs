namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10c0), Guid("54A2CB2D-9A0C-48B6-8A50-9ABB69EE2D02")]
    public interface IUpdateDownloadContent
    {
        [DispId(0x60020001)]
        string DownloadUrl { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }
}

