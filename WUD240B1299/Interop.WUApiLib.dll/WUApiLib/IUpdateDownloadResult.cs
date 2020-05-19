namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10c0), Guid("BF99AF76-B575-42AD-8AA4-33CBB5477AF1")]
    public interface IUpdateDownloadResult
    {
        [DispId(0x60020001)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002), ComAliasName("WUApiLib.OperationResultCode")]
        OperationResultCode ResultCode { [return: ComAliasName("WUApiLib.OperationResultCode")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
    }
}

