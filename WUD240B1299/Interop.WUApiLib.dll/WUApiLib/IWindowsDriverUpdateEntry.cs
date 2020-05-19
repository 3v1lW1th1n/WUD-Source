namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10c0), Guid("ED8BFE40-A60B-42EA-9652-817DFCFA23EC")]
    public interface IWindowsDriverUpdateEntry
    {
        [DispId(0x60030001)]
        string DriverClass { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] get; }
        [DispId(0x60030002)]
        string DriverHardwareID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030002)] get; }
        [DispId(0x60030003)]
        string DriverManufacturer { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030003)] get; }
        [DispId(0x60030004)]
        string DriverModel { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030004)] get; }
        [DispId(0x60030005)]
        string DriverProvider { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030005)] get; }
        [DispId(0x60030006)]
        DateTime DriverVerDate { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030006)] get; }
        [DispId(0x60030007)]
        int DeviceProblemNumber { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030007)] get; }
        [DispId(0x60030008)]
        int DeviceStatus { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030008)] get; }
    }
}

