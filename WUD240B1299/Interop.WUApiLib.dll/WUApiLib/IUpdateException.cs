namespace WUApiLib
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("A376DD5E-09D4-427F-AF7C-FED5B6E1C1D6"), TypeLibType((short) 0x10c0), DefaultMember("Message")]
    public interface IUpdateException
    {
        [DispId(0)]
        string Message { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0)] get; }
        [DispId(0x60020001)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [ComAliasName("WUApiLib.UpdateExceptionContext"), DispId(0x60020002)]
        UpdateExceptionContext Context { [return: ComAliasName("WUApiLib.UpdateExceptionContext")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
    }
}

