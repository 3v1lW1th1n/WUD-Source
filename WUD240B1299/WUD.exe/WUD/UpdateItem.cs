namespace WUD
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UpdateItem : IComparable
    {
        public string ID;
        public int Category;
        public DateTime PublishDate;
        public string Title;
        public string Description;
        public string Article;
        public string Filename;
        public string URL;
        public UpdateItem(string id, int category, DateTime publishdate, string title, string description, string article, string filename, string url)
        {
            this.ID = id;
            this.Category = category;
            this.PublishDate = publishdate;
            this.Title = title;
            this.Description = description;
            this.Article = article;
            this.Filename = filename;
            this.URL = url;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is UpdateItem))
            {
                throw new InvalidCastException("CompareTo() against object not of type UpdateItem.");
            }
            UpdateItem item = (UpdateItem) obj;
            if (this.PublishDate < item.PublishDate)
            {
                return 1;
            }
            if (this.PublishDate > item.PublishDate)
            {
                return -1;
            }
            return 0;
        }
    }
}

