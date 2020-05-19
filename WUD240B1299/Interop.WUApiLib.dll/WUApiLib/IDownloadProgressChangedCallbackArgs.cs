namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("324FF2C6-4981-4B04-9412-57481745AB24"), TypeLibType((short) 0x10c0)]
    public interface IDownloadProgressChangedCallbackArgs
    {
        [DispId(0x60020001)]
        IDownloadProgress Progress { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }
}

