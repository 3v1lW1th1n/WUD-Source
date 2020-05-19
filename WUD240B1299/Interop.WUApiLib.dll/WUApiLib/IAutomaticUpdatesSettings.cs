namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("2EE48F22-AF3C-405F-8970-F71BE12EE9A2"), TypeLibType((short) 0x10c0)]
    public interface IAutomaticUpdatesSettings
    {
        [DispId(0x60020001), ComAliasName("WUApiLib.AutomaticUpdatesNotificationLevel")]
        AutomaticUpdatesNotificationLevel NotificationLevel { [return: ComAliasName("WUApiLib.AutomaticUpdatesNotificationLevel")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, ComAliasName("WUApiLib.AutomaticUpdatesNotificationLevel")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020002)]
        bool ReadOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        bool Required { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [ComAliasName("WUApiLib.AutomaticUpdatesScheduledInstallationDay"), DispId(0x60020004)]
        AutomaticUpdatesScheduledInstallationDay ScheduledInstallationDay { [return: ComAliasName("WUApiLib.AutomaticUpdatesScheduledInstallationDay")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] get; [param: In, ComAliasName("WUApiLib.AutomaticUpdatesScheduledInstallationDay")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] set; }
        [DispId(0x60020005)]
        int ScheduledInstallationTime { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)] set; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020006)]
        void Refresh();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020007)]
        void Save();
    }
}

