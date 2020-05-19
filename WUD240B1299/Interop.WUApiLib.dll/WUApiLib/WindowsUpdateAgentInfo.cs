namespace WUApiLib
{
    using System.Runtime.InteropServices;

    [ComImport, Guid("85713FA1-7796-4FA2-BE3B-E2D6124DD373"), CoClass(typeof(WindowsUpdateAgentInfoClass))]
    public interface WindowsUpdateAgentInfo : IWindowsUpdateAgentInfo
    {
    }
}

