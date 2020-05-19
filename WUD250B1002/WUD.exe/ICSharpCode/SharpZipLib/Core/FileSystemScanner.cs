namespace ICSharpCode.SharpZipLib.Core
{
    using System;
    using System.IO;

    public class FileSystemScanner
    {
        private bool alive_;
        public CompletedFileHandler CompletedFile;
        public DirectoryFailureHandler DirectoryFailure;
        private IScanFilter directoryFilter_;
        public FileFailureHandler FileFailure;
        private IScanFilter fileFilter_;
        public ProcessDirectoryHandler ProcessDirectory;
        public ProcessFileHandler ProcessFile;

        public FileSystemScanner(IScanFilter fileFilter)
        {
            this.fileFilter_ = fileFilter;
        }

        public FileSystemScanner(string filter)
        {
            this.fileFilter_ = new PathFilter(filter);
        }

        public FileSystemScanner(IScanFilter fileFilter, IScanFilter directoryFilter)
        {
            this.fileFilter_ = fileFilter;
            this.directoryFilter_ = directoryFilter;
        }

        public FileSystemScanner(string fileFilter, string directoryFilter)
        {
            this.fileFilter_ = new PathFilter(fileFilter);
            this.directoryFilter_ = new PathFilter(directoryFilter);
        }

        private void OnCompleteFile(string file)
        {
            if (this.CompletedFile != null)
            {
                ScanEventArgs e = new ScanEventArgs(file);
                this.CompletedFile(this, e);
                this.alive_ = e.ContinueRunning;
            }
        }

        private void OnDirectoryFailure(string directory, Exception e)
        {
            if (this.DirectoryFailure == null)
            {
                this.alive_ = false;
            }
            else
            {
                ScanFailureEventArgs args = new ScanFailureEventArgs(directory, e);
                this.DirectoryFailure(this, args);
                this.alive_ = args.ContinueRunning;
            }
        }

        private void OnFileFailure(string file, Exception e)
        {
            if (this.FileFailure == null)
            {
                this.alive_ = false;
            }
            else
            {
                ScanFailureEventArgs args = new ScanFailureEventArgs(file, e);
                this.FileFailure(this, args);
                this.alive_ = args.ContinueRunning;
            }
        }

        private void OnProcessDirectory(string directory, bool hasMatchingFiles)
        {
            if (this.ProcessDirectory != null)
            {
                DirectoryEventArgs e = new DirectoryEventArgs(directory, hasMatchingFiles);
                this.ProcessDirectory(this, e);
                this.alive_ = e.ContinueRunning;
            }
        }

        private void OnProcessFile(string file)
        {
            if (this.ProcessFile != null)
            {
                ScanEventArgs e = new ScanEventArgs(file);
                this.ProcessFile(this, e);
                this.alive_ = e.ContinueRunning;
            }
        }

        public void Scan(string directory, bool recurse)
        {
            this.alive_ = true;
            this.ScanDir(directory, recurse);
        }

        private void ScanDir(string directory, bool recurse)
        {
            try
            {
                string[] files = Directory.GetFiles(directory);
                bool hasMatchingFiles = false;
                for (int i = 0; i < files.Length; i++)
                {
                    if (!this.fileFilter_.IsMatch(files[i]))
                    {
                        files[i] = null;
                    }
                    else
                    {
                        hasMatchingFiles = true;
                    }
                }
                this.OnProcessDirectory(directory, hasMatchingFiles);
                if (this.alive_ && hasMatchingFiles)
                {
                    foreach (string str in files)
                    {
                        try
                        {
                            if (str != null)
                            {
                                this.OnProcessFile(str);
                                if (!this.alive_)
                                {
                                    goto Label_0090;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            this.OnFileFailure(str, exception);
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                this.OnDirectoryFailure(directory, exception2);
            }
        Label_0090:
            if (this.alive_ && recurse)
            {
                try
                {
                    foreach (string str2 in Directory.GetDirectories(directory))
                    {
                        if ((this.directoryFilter_ == null) || this.directoryFilter_.IsMatch(str2))
                        {
                            this.ScanDir(str2, true);
                            if (!this.alive_)
                            {
                                return;
                            }
                        }
                    }
                }
                catch (Exception exception3)
                {
                    this.OnDirectoryFailure(directory, exception3);
                }
            }
        }
    }
}

