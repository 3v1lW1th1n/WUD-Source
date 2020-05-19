namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("6ABC136A-C3CA-4384-8171-CB2B1E59B8DC"), TypeLibType((short) 0x10c0)]
    public interface IAutomaticUpdatesSettings2 : IAutomaticUpdatesSettings
    {
        [DispId(0x60020001), ComAliasName("WUApiLib.AutomaticUpdatesNotificationLevel")]
        AutomaticUpdatesNotificationLevel NotificationLevel { [return: ComAliasName("WUApiLib.AutomaticUpdatesNotificationLevel")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, ComAliasName("WUApiLib.AutomaticUpdatesNotificationLevel")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020002)]
        bool ReadOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        bool Required { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004), ComAliasName("WUApiLib.AutomaticUpdatesScheduledInstallationDay")]
        AutomaticUpdatesScheduledInstallationDay ScheduledInstallationDay { [return: ComAliasName("WUApiLib.AutomaticUpdatesScheduledInstallationDay")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] get; [param: In, ComAliasName("WUApiLib.AutomaticUpdatesScheduledInstallationDay")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)] set; }
        [DispId(0x60020005)]
        int ScheduledInstallationTime { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)] set; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020006)]
        void Refresh();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020007)]
        void Save();
        [DispId(0x60030001)]
        bool IncludeRecommendedUpdates { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] set; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030002)]
        bool CheckPermission([In, ComAliasName("WUApiLib.AutomaticUpdatesUserType")] AutomaticUpdatesUserType userType, [In, ComAliasName("WUApiLib.AutomaticUpdatesPermissionType")] AutomaticUpdatesPermissionType permissionType);
    }
}

