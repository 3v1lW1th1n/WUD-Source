namespace Supremus.Environment.WindowsUpdates
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using WUApiLib;

    public class WindowsUpdateAgent : IDisposable
    {
        private bool mDisposed;
        private Dictionary<string, WindowsUpdate> mInstalledUpdates = new Dictionary<string, WindowsUpdate>();
        private IUpdateHistoryEntryCollection mUpdateHistoryCollection;
        private IUpdateSearcher mUpdateSearcher;
        private IUpdateSession mUpdateSession = new UpdateSessionClass();

        public WindowsUpdateAgent()
        {
            this.mUpdateSearcher = this.mUpdateSession.CreateUpdateSearcher();
            int totalHistoryCount = this.mUpdateSearcher.GetTotalHistoryCount();
            if (totalHistoryCount > 0)
            {
                this.mUpdateHistoryCollection = this.mUpdateSearcher.QueryHistory(0, totalHistoryCount);
                Regex regex = new Regex(@"KB\d{5,5}");
                foreach (IUpdateHistoryEntry entry in this.mUpdateHistoryCollection)
                {
                    Match match = regex.Match(entry.Title);
                    if (match.Success)
                    {
                        if (this.mInstalledUpdates.ContainsKey(match.Value))
                        {
                            if (this.mInstalledUpdates[match.Value].RevisionNumber < Convert.ToInt32(entry.UpdateIdentity.RevisionNumber))
                            {
                                this.mInstalledUpdates[match.Value] = new WindowsUpdate(match.Value, entry.Title, new Guid(entry.UpdateIdentity.UpdateID), Convert.ToInt32(entry.UpdateIdentity.RevisionNumber));
                            }
                        }
                        else
                        {
                            this.mInstalledUpdates.Add(match.Value, new WindowsUpdate(match.Value, entry.Title, new Guid(entry.UpdateIdentity.UpdateID), Convert.ToInt32(entry.UpdateIdentity.RevisionNumber)));
                        }
                    }
                }
                regex = null;
                this.mUpdateHistoryCollection = null;
            }
            this.mUpdateSearcher = null;
            this.mUpdateSession = null;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.mDisposed)
            {
                this.mInstalledUpdates = null;
                this.mDisposed = true;
            }
        }

        public bool IsInstalled(string KBArticleID) => 
            this.mInstalledUpdates.ContainsKey(KBArticleID);

        public List<WindowsUpdate> InstalledUpdates =>
            new List<WindowsUpdate>(this.mInstalledUpdates.Values);
    }
}

