namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), TypeLibType((short) 0x180), Guid("E01402D5-F8DA-43BA-A012-38894BD048F1")]
    public interface IInstallationProgressChangedCallback
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Invoke([In, MarshalAs(UnmanagedType.Interface)] IInstallationJob installationJob, [In, MarshalAs(UnmanagedType.Interface)] IInstallationProgressChangedCallbackArgs callbackArgs);
    }
}

