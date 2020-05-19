namespace WUApiLib
{
    using System.Runtime.InteropServices;

    [ComImport, Guid("4A2F5C31-CFD9-410E-B7FB-29A653973A0F"), CoClass(typeof(AutomaticUpdatesClass))]
    public interface AutomaticUpdates : IAutomaticUpdates2
    {
    }
}

