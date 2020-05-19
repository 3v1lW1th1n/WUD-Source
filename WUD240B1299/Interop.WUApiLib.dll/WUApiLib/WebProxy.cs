namespace WUApiLib
{
    using System.Runtime.InteropServices;

    [ComImport, Guid("174C81FE-AECD-4DAE-B8A0-2C6318DD86A8"), CoClass(typeof(WebProxyClass))]
    public interface WebProxy : IWebProxy
    {
    }
}

