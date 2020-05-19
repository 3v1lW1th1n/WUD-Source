namespace WUApiLib
{
    using System.Runtime.InteropServices;

    [ComImport, Guid("925CBC18-A2EA-4648-BF1C-EC8BADCFE20A"), CoClass(typeof(InstallationAgentClass))]
    public interface InstallationAgent : IInstallationAgent
    {
    }
}

