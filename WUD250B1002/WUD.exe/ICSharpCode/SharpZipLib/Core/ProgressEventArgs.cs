namespace ICSharpCode.SharpZipLib.Core
{
    using System;

    public class ProgressEventArgs : EventArgs
    {
        private bool continueRunning_ = true;
        private string name_;
        private long processed_;
        private long target_;

        public ProgressEventArgs(string name, long processed, long target)
        {
            this.name_ = name;
            this.processed_ = processed;
            this.target_ = target;
        }

        public bool ContinueRunning
        {
            get => 
                this.continueRunning_;
            set
            {
                this.continueRunning_ = value;
            }
        }

        public string Name =>
            this.name_;

        public float PercentComplete
        {
            get
            {
                if (this.target_ <= 0L)
                {
                    return 0f;
                }
                return ((((float) this.processed_) / ((float) this.target_)) * 100f);
            }
        }

        public long Processed =>
            this.processed_;

        public long Target =>
            this.target_;
    }
}

