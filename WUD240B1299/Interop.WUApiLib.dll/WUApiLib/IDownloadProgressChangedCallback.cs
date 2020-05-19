namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("8C3F1CDD-6173-4591-AEBD-A56A53CA77C1"), TypeLibType((short) 0x180), InterfaceType((short) 1)]
    public interface IDownloadProgressChangedCallback
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Invoke([In, MarshalAs(UnmanagedType.Interface)] IDownloadJob downloadJob, [In, MarshalAs(UnmanagedType.Interface)] IDownloadProgressChangedCallbackArgs callbackArgs);
    }
}

