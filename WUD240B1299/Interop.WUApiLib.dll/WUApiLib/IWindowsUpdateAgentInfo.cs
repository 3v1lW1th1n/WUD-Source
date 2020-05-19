namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10d0), Guid("85713FA1-7796-4FA2-BE3B-E2D6124DD373")]
    public interface IWindowsUpdateAgentInfo
    {
        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)]
        object GetInfo([In, MarshalAs(UnmanagedType.Struct)] object varInfoIdentifier);
    }
}

