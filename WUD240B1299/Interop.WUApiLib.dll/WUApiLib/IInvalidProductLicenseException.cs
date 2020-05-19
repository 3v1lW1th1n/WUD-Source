namespace WUApiLib
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("A37D00F5-7BB0-4953-B414-F9E98326F2E8"), TypeLibType((short) 0x10c0), DefaultMember("Message")]
    public interface IInvalidProductLicenseException : IUpdateException
    {
        [DispId(0)]
        string Message { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0)] get; }
        [DispId(0x60020001)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [ComAliasName("WUApiLib.UpdateExceptionContext"), DispId(0x60020002)]
        UpdateExceptionContext Context { [return: ComAliasName("WUApiLib.UpdateExceptionContext")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60030001)]
        string Product { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] get; }
    }
}

