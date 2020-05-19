namespace WUApiLib
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.CustomMarshalers;

    [ComImport, Guid("503626A3-8E14-4729-9355-0FE664BD2321"), TypeLibType((short) 0x10c0)]
    public interface IUpdateExceptionCollection : IEnumerable
    {
        [DispId(0)]
        IUpdateException this[int index] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0)] get; }
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType="", MarshalTypeRef=typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie="")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
        [DispId(0x60020001)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }
}

