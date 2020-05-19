namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10c0), Guid("DAA4FDD0-4727-4DBE-A1E7-745DCA317144")]
    public interface IDownloadResult
    {
        [DispId(0x60020001)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [ComAliasName("WUApiLib.OperationResultCode"), DispId(0x60020002)]
        OperationResultCode ResultCode { [return: ComAliasName("WUApiLib.OperationResultCode")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)]
        IUpdateDownloadResult GetUpdateResult([In] int updateIndex);
    }
}

