namespace ICSharpCode.SharpZipLib.Core
{
    using System;

    public class ScanFailureEventArgs
    {
        private bool continueRunning_;
        private System.Exception exception_;
        private string name_;

        public ScanFailureEventArgs(string name, System.Exception e)
        {
            this.name_ = name;
            this.exception_ = e;
            this.continueRunning_ = true;
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

        public System.Exception Exception =>
            this.exception_;

        public string Name =>
            this.name_;
    }
}

