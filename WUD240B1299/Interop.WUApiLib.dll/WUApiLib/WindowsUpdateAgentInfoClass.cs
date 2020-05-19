namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("C2E88C2F-6F5B-4AAA-894B-55C847AD3A2D"), TypeLibType((short) 2), ClassInterface((short) 0)]
    public class WindowsUpdateAgentInfoClass : IWindowsUpdateAgentInfo, WindowsUpdateAgentInfo
    {
        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)]
        public virtual extern object GetInfo([In, MarshalAs(UnmanagedType.Struct)] object varInfoIdentifier);
    }
}

