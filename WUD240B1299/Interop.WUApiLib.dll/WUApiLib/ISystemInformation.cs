namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("ADE87BF7-7B56-4275-8FAB-B9B0E591844B"), TypeLibType((short) 0x10d0)]
    public interface ISystemInformation
    {
        [DispId(0x60020001)]
        string OemHardwareSupportLink { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool RebootRequired { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
    }
}

