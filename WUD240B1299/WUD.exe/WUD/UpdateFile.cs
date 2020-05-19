namespace WUD
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UpdateFile : IComparable
    {
        public string Product;
        public string ServicePack;
        public string Platform;
        public string Language;
        public DateTime LastUpdate;
        public string Filename;
        public UpdateFile(string product, string servicepack, string platform, string language, DateTime lastupdate, string filename)
        {
            this.Product = product;
            this.ServicePack = servicepack;
            this.Platform = platform;
            this.Language = language;
            this.LastUpdate = lastupdate;
            this.Filename = filename;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is UpdateFile))
            {
                throw new InvalidCastException("CompareTo() against object not of type UpdateFile.");
            }
            UpdateFile file = (UpdateFile) obj;
            return this.Product.CompareTo(file.Product);
        }
    }
}

