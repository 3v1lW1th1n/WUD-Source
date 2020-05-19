namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10c0), Guid("345C8244-43A3-4E32-A368-65F073B76F36")]
    public interface IInstallationProgress
    {
        [DispId(0x60020001)]
        int CurrentUpdateIndex { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        int CurrentUpdatePercentComplete { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        int PercentComplete { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)]
        IUpdateInstallationResult GetUpdateResult([In] int updateIndex);
    }
}

