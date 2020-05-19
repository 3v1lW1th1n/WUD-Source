namespace WUD
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class DownloadManager
    {
        private bool AbortDownload;
        private ManualResetEvent allDone = new ManualResetEvent(false);
        public DownloadInfo CurrentDownload = new DownloadInfo();
        public string DownloadPath = "";
        private Thread DownloadThread;
        private FileStream downStream;
        private BinaryWriter downWriter;
        public OperatingMode Mode;
        public WebProxy Proxy;
        public List<int> Queue = new List<int>();
        private HttpWebRequest Request;
        private HttpWebResponse Response;
        private Stream ResponseStream;
        public DownloadStatus Status;
        public bool UseSubFolders;

        public event DownloadCompleteHandler CompleteCallback;

        public event DownloadProgressHandler ProgressCallback;

        public void Abort()
        {
            this.AbortDownload = true;
        }

        public void Download()
        {
            this.AbortDownload = false;
            this.Status = DownloadStatus.Running;
            this.DownloadThread = new Thread(new ThreadStart(this.processQueue));
            this.DownloadThread.Start();
        }

        private void processQueue()
        {
            if (!Directory.Exists(this.DownloadPath))
            {
                Directory.CreateDirectory(this.DownloadPath);
            }
            string path = "";
            string str2 = "";
            for (int i = 0; i < Program.DLManager.Queue.Count; i++)
            {
                this.allDone.Reset();
                this.CurrentDownload.Initialize();
                if (this.UseSubFolders)
                {
                    path = Path.Combine(this.DownloadPath, Program.ULManager.Categories[Program.ULManager.getCategoryIndex(Program.ULManager.Updates[Program.DLManager.Queue[i]].Category)].Description);
                }
                else
                {
                    path = this.DownloadPath;
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                str2 = Path.Combine(path, Program.ULManager.Updates[Program.DLManager.Queue[i]].Filename);
                this.downStream = new FileStream(str2, FileMode.Create);
                this.downWriter = new BinaryWriter(this.downStream);
                this.Request = (HttpWebRequest) WebRequest.Create(Program.ULManager.Updates[Program.DLManager.Queue[i]].URL);
                if (this.Proxy != null)
                {
                    this.Request.Proxy = this.Proxy;
                }
                this.CurrentDownload.queueIndex = i;
                this.Request.BeginGetResponse(new AsyncCallback(this.ResponseCallback), null);
                this.allDone.WaitOne();
                if (this.AbortDownload)
                {
                    System.IO.File.Delete(str2);
                    break;
                }
            }
            this.CompleteCallback();
            this.Status = DownloadStatus.Idle;
        }

        private void ReadCallBack(IAsyncResult asyncResult)
        {
            int count = this.ResponseStream.EndRead(asyncResult);
            if (!this.AbortDownload && (count > 0))
            {
                this.downWriter.Write(this.CurrentDownload.Buffer, 0, count);
                this.CurrentDownload.bytesReceived += count;
                this.ProgressCallback();
                this.ResponseStream.BeginRead(this.CurrentDownload.Buffer, 0, this.CurrentDownload.bufferSize, new AsyncCallback(this.ReadCallBack), null);
            }
            else
            {
                this.ResponseStream.Close();
                this.Response.Close();
                this.downWriter.Close();
                this.downStream.Close();
                this.allDone.Set();
            }
        }

        private void ResponseCallback(IAsyncResult asyncResult)
        {
            try
            {
                this.Response = (HttpWebResponse) this.Request.EndGetResponse(asyncResult);
            }
            catch (WebException exception)
            {
                this.downWriter.Close();
                this.downStream.Close();
                string downloadPath = "";
                if (this.UseSubFolders)
                {
                    downloadPath = Path.Combine(this.DownloadPath, Program.ULManager.Categories[Program.ULManager.getCategoryIndex(Program.ULManager.Updates[Program.DLManager.Queue[this.CurrentDownload.queueIndex]].Category)].Description);
                }
                else
                {
                    downloadPath = this.DownloadPath;
                }
                System.IO.File.Delete(Path.Combine(downloadPath, Program.ULManager.Updates[Program.DLManager.Queue[this.CurrentDownload.queueIndex]].Filename));
                if ((exception.Status == WebExceptionStatus.ProtocolError) && (((HttpWebResponse) exception.Response).StatusCode == HttpStatusCode.NotFound))
                {
                    if (this.Mode == OperatingMode.User)
                    {
                        MessageBox.Show("\"" + Program.ULManager.Updates[Program.DLManager.Queue[this.CurrentDownload.queueIndex]].Filename + "\" was not found on the remote server. This file will be skipped.", "Windows Updates Downloader", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    if (this.Mode == OperatingMode.User)
                    {
                        MessageBox.Show(exception.Message, "Windows Updates Downloader", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    if (this.Mode == OperatingMode.Automated)
                    {
                        Environment.ExitCode = 15;
                    }
                    this.Abort();
                }
                this.allDone.Set();
                return;
            }
            string str2 = this.Response.Headers["Content-Length"];
            if (str2 != null)
            {
                this.CurrentDownload.bytesAvailable = Convert.ToInt32(str2);
            }
            this.ResponseStream = this.Response.GetResponseStream();
            this.ResponseStream.BeginRead(this.CurrentDownload.Buffer, 0, this.CurrentDownload.bufferSize, new AsyncCallback(this.ReadCallBack), null);
        }

        public class DownloadInfo
        {
            public byte[] Buffer;
            public int bufferSize;
            public int bytesAvailable;
            public int bytesReceived;
            public int queueIndex;

            public void Initialize()
            {
                this.bufferSize = 0x1000;
                this.Buffer = new byte[this.bufferSize];
                this.bytesAvailable = -1;
                this.bytesReceived = 0;
                this.queueIndex = 0;
            }
        }

        public enum DownloadStatus
        {
            Idle,
            Running
        }
    }
}

