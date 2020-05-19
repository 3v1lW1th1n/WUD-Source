namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("D940F0F8-3CBB-4FD0-993F-471E7F2328AD"), TypeLibType((short) 0x10c0)]
    public interface IUpdateInstallationResult
    {
        [DispId(0x60020001)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool RebootRequired { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [ComAliasName("WUApiLib.OperationResultCode"), DispId(0x60020003)]
        OperationResultCode ResultCode { [return: ComAliasName("WUApiLib.OperationResultCode")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] get; }
    }
}

