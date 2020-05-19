namespace WUD.SimpleInterprocessCommunications
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class EnumWindowsCollection : ReadOnlyCollectionBase
    {
        public void Add(IntPtr hWnd)
        {
            EnumWindowsItem item = new EnumWindowsItem(hWnd);
            base.InnerList.Add(item);
        }

        public EnumWindowsItem this[int index] =>
            ((EnumWindowsItem) base.InnerList[index]);
    }
}

