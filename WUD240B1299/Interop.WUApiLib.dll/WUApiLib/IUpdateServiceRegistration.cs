namespace WUApiLib
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 0x10c0), DefaultMember("RegistrationState"), Guid("DDE02280-12B3-4E0B-937B-6747F6ACB286")]
    public interface IUpdateServiceRegistration
    {
        [ComAliasName("WUApiLib.UpdateServiceRegistrationState"), DispId(0)]
        UpdateServiceRegistrationState RegistrationState { [return: ComAliasName("WUApiLib.UpdateServiceRegistrationState")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0)] get; }
        [DispId(0x60020001)]
        string ServiceID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool IsPendingRegistrationWithAU { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        IUpdateService2 Service { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] get; }
    }
}

