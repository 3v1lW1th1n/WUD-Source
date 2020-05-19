namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10c0), Guid("E4F14E1E-689D-4218-A0B9-BC189C484A01")]
    public interface IInstallationProgressChangedCallbackArgs
    {
        [DispId(0x60020001)]
        IInstallationProgress Progress { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }
}

