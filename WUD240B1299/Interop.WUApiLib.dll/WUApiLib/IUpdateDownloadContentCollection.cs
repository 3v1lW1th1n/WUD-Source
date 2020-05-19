namespace WUApiLib
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.CustomMarshalers;

    [ComImport, Guid("BC5513C8-B3B8-4BF7-A4D4-361C0D8C88BA"), TypeLibType((short) 0x10c0)]
    public interface IUpdateDownloadContentCollection : IEnumerable
    {
        [DispId(0)]
        IUpdateDownloadContent this[int index] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0)] get; }
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType="", MarshalTypeRef=typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie="")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
        [DispId(0x60020001)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }
}

