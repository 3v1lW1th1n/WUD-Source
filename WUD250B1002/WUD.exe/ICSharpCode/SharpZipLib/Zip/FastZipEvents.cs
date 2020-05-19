namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib.Core;
    using System;

    public class FastZipEvents
    {
        public CompletedFileHandler CompletedFile;
        public DirectoryFailureHandler DirectoryFailure;
        public FileFailureHandler FileFailure;
        public ProcessDirectoryHandler ProcessDirectory;
        public ProcessFileHandler ProcessFile;
        public ProgressHandler Progress;
        private TimeSpan progressInterval_ = TimeSpan.FromSeconds(3.0);

        public bool OnCompletedFile(string file)
        {
            bool continueRunning = true;
            if (this.CompletedFile != null)
            {
                ScanEventArgs e = new ScanEventArgs(file);
                this.CompletedFile(this, e);
                continueRunning = e.ContinueRunning;
            }
            return continueRunning;
        }

        public bool OnDirectoryFailure(string directory, Exception e)
        {
            bool continueRunning = false;
            if (this.DirectoryFailure != null)
            {
                ScanFailureEventArgs args = new ScanFailureEventArgs(directory, e);
                this.DirectoryFailure(this, args);
                continueRunning = args.ContinueRunning;
            }
            return continueRunning;
        }

        public bool OnFileFailure(string file, Exception e)
        {
            bool continueRunning = false;
            if (this.FileFailure != null)
            {
                ScanFailureEventArgs args = new ScanFailureEventArgs(file, e);
                this.FileFailure(this, args);
                continueRunning = args.ContinueRunning;
            }
            return continueRunning;
        }

        public bool OnProcessDirectory(string directory, bool hasMatchingFiles)
        {
            bool continueRunning = true;
            if (this.ProcessDirectory != null)
            {
                DirectoryEventArgs e = new DirectoryEventArgs(directory, hasMatchingFiles);
                this.ProcessDirectory(this, e);
                continueRunning = e.ContinueRunning;
            }
            return continueRunning;
        }

        public bool OnProcessFile(string file)
        {
            bool continueRunning = true;
            if (this.ProcessFile != null)
            {
                ScanEventArgs e = new ScanEventArgs(file);
                this.ProcessFile(this, e);
                continueRunning = e.ContinueRunning;
            }
            return continueRunning;
        }

        public TimeSpan ProgressInterval
        {
            get => 
                this.progressInterval_;
            set
            {
                this.progressInterval_ = value;
            }
        }
    }
}

