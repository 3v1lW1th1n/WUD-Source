namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("A976C28D-75A1-42AA-94AE-8AF8B872089A"), TypeLibType((short) 0x180), InterfaceType((short) 1)]
    public interface IUpdateLockdown
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void LockDown([In] int flags);
    }
}

