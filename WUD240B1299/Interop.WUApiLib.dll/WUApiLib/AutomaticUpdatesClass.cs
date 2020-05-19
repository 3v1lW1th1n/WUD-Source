namespace WUApiLib
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, TypeLibType((short) 2), ClassInterface((short) 0), Guid("BFE18E9C-6D87-4450-B37C-E02F0B373803")]
    public class AutomaticUpdatesClass : IAutomaticUpdates2, AutomaticUpdates
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020001)]
        public virtual extern void DetectNow();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020007)]
        public virtual extern void EnableService();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020002)]
        public virtual extern void Pause();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020003)]
        public virtual extern void Resume();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020004)]
        public virtual extern void ShowSettingsDialog();

        [DispId(0x60030001)]
        public virtual IAutomaticUpdatesResults Results { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] get; }

        [DispId(0x60020006)]
        public virtual bool ServiceEnabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020006)] get; }

        [DispId(0x60020005)]
        public virtual IAutomaticUpdatesSettings Settings { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)] get; }

        [DispId(0x60030001)]
        public virtual IAutomaticUpdatesResults WUApiLib.IAutomaticUpdates2.Results { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60030001)] get; }

        [DispId(0x60020006)]
        public virtual bool WUApiLib.IAutomaticUpdates2.ServiceEnabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020006)] get; }

        [DispId(0x60020005)]
        public virtual IAutomaticUpdatesSettings WUApiLib.IAutomaticUpdates2.Settings { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime), DispId(0x60020005)] get; }
    }
}

