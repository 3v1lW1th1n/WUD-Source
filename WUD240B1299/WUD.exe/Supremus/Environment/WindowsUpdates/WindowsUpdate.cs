namespace Supremus.Environment.WindowsUpdates
{
    using System;

    public class WindowsUpdate
    {
        private string mKBArticleID;
        private int mRevisionNumber;
        private string mTitle;
        private Guid mUpdateID;

        public WindowsUpdate(string KBArticleID, string Title, Guid UpdateID, int RevisionNumber)
        {
            this.mKBArticleID = KBArticleID;
            this.mTitle = Title;
            this.mUpdateID = UpdateID;
            this.mRevisionNumber = RevisionNumber;
        }

        public string KBArticleID =>
            this.mKBArticleID;

        public int RevisionNumber =>
            this.mRevisionNumber;

        public string Title =>
            this.mTitle;

        public Guid UpdateID =>
            this.mUpdateID;
    }
}

