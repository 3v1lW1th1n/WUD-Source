namespace WUD
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UpdateCategory : IComparable
    {
        public int ID;
        public string Description;
        public UpdateCategory(int id, string description)
        {
            this.ID = id;
            this.Description = description;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is UpdateCategory))
            {
                throw new InvalidCastException("CompareTo() against object not of type UpdateCategory.");
            }
            UpdateCategory category = (UpdateCategory) obj;
            if (this.ID < category.ID)
            {
                return -1;
            }
            if (this.ID > category.ID)
            {
                return 1;
            }
            return 0;
        }
    }
}

