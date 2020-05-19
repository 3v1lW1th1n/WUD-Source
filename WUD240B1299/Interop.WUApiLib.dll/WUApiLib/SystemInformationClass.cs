namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("C01B9BA0-BEA7-41BA-B604-D0A36F469133"), TypeLibType((short) 2), ClassInterface((short) 0)]
    public class SystemInformationClass : ISystemInformation, SystemInformation
    {
        [DispId(0x60020001)]
        public virtual string OemHardwareSupportLink { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }

        [DispId(0x60020002)]
        public virtual bool RebootRequired { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }

        [DispId(0x60020001)]
        public virtual string WUApiLib.ISystemInformation.OemHardwareSupportLink { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }

        [DispId(0x60020002)]
        public virtual bool WUApiLib.ISystemInformation.RebootRequired { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
    }
}

