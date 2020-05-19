namespace WUD
{
    using Supremus.Configuration;
    using Supremus.Encryption;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Windows.Forms;
    using WUD.SimpleInterprocessCommunications;

    public class formMain : Form
    {
        private string _downloadpath;
        private ToolStripMenuItem addUpdateListToolStripMenuItem;
        private BackgroundWorker backgroundVersionCheck;
        private Button buttonAddUL;
        private Button buttonCancel;
        private Button buttonChangeFolder;
        private Button buttonDownload;
        private Button buttonManageULs;
        private Button buttonOptions;
        private Button buttonRefreshULs;
        private CheckBox checkCategorySubFolders;
        private CheckBox checkMinimizeToTray;
        private CheckBox checkProductSubFolders;
        private CheckBox checkRemoveUnlisted;
        private CheckBox checkShowDescriptions;
        private CheckBox checkShowPublishDates;
        private CheckBox checkUseProxy;
        private CheckBox checkUseProxyAuthentication;
        private ComboBox comboUpdateLists;
        private ComboBox comboVersionCheck;
        private IContainer components;
        private ContextMenuStrip contextUpdateList;
        private CopyData copyData;
        private FolderBrowserDialog folderDownload;
        private GroupBox groupDownloadProgress;
        private GroupBox groupOptions;
        private GroupBox groupProxy;
        private Label labelBytes;
        private Label labelCheckSeperator;
        private Label labelCurrent;
        private Label labelDownloadFolder;
        private Label labelProxyPassword;
        private Label labelProxyPort;
        private Label labelProxyURL;
        private Label labelProxyUsername;
        private Label labelStatus;
        private Label labelUpdateList;
        private Label labelWrittenBy;
        private LinkLabel linkCheckAll;
        private LinkLabel linkLabel1;
        private LinkLabel linkMSFN;
        private ToolStripMenuItem manageToolStripMenuItem;
        private const int MF_BYPOSITION = 0x400;
        private const int MF_DISABLED = 2;
        public OperatingMode mode;
        private NotifyIcon notifyWUDIcon;
        private ProgressBar progressFile;
        private ProgressBar progressTotal;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ImageList stateImages;
        private TextBox textboxPath;
        private TextBox textboxProxyPassword;
        private TextBox textboxProxyPort;
        private TextBox textboxProxyURL;
        private TextBox textboxProxyUsername;
        private ToolStripSeparator toolStripSeparator1;
        private TriStateTreeView treeUpdates;

        public formMain()
        {
            this.InitializeComponent();
            this.groupDownloadProgress.Top = 9;
            base.Height = (this.buttonDownload.Top + this.buttonDownload.Height) + 0x29;
            this.buttonOptions.Tag = false;
        }

        private void backgroundVersionCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            WebClient client = new WebClient();
            string[] strArray = client.DownloadString("http://www.windowsupdatesdownloader.com/Version.ashx").Split(new char[] { '|' });
            client.Dispose();
            bool flag = false;
            int num = ((int) e.Argument) - 1;
            if ((num == 0) && (this.VersionCompare(strArray[0], versionInfo.ProductVersion) == -1))
            {
                flag = true;
                string[] strArray2 = strArray[0].Split(new char[] { '.' });
                if (MessageBox.Show("Version " + strArray2[0] + "." + strArray2[1] + " is now available. Would you like to download it now?", "Windows Updates Downloader", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    Process.Start("\"http://www.windowsupdatesdownloader.com/\"");
                }
            }
            if (num == 1)
            {
                if ((this.VersionCompare(strArray[1], strArray[0]) == -1) && (this.VersionCompare(strArray[1], versionInfo.ProductVersion) == -1))
                {
                    flag = true;
                    string[] strArray3 = strArray[1].Split(new char[] { '.' });
                    if (MessageBox.Show("Version " + strArray3[0] + "." + strArray3[1] + " Beta Build " + strArray3[2] + " is now available. Would you like to download it now?", "Windows Updates Downloader", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        Process.Start("\"http://www.windowsupdatesdownloader.com/\"");
                    }
                }
                else if (this.VersionCompare(strArray[0], versionInfo.ProductVersion) == -1)
                {
                    flag = true;
                    string[] strArray4 = strArray[0].Split(new char[] { '.' });
                    if (MessageBox.Show("Version " + strArray4[0] + "." + strArray4[1] + " is now available. Would you like to download it now?", "Windows Updates Downloader", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        Process.Start("\"http://www.windowsupdatesdownloader.com/\"");
                    }
                }
            }
            if (!flag)
            {
                string str = "";
                for (int i = 0; i < Program.ULManager.Files.Count; i++)
                {
                    client = new WebClient();
                    string str2 = client.DownloadString("http://www.windowsupdatesdownloader.com/LastUpdated.ashx?product=" + HttpUtility.UrlEncode(Program.ULManager.Files[i].Product) + "&servicepack=" + HttpUtility.UrlEncode(Program.ULManager.Files[i].ServicePack) + "&platform=" + HttpUtility.UrlEncode(Program.ULManager.Files[i].Platform) + "&language=" + HttpUtility.UrlEncode(Program.ULManager.Files[i].Language));
                    client.Dispose();
                    if (str2 != "404")
                    {
                        DateTime time = new DateTime(Convert.ToInt32(str2.Substring(0, 4)), Convert.ToInt32(str2.Substring(5, 2)), Convert.ToInt32(str2.Substring(8, 2)));
                        if (time > Program.ULManager.Files[i].LastUpdate)
                        {
                            if (Program.ULManager.Files[i].ServicePack == "any")
                            {
                                string str3 = str;
                                str = str3 + Program.ULManager.Files[i].Product + " " + Program.ULManager.Files[i].Platform + " " + Program.ULManager.Files[i].Language + " (" + time.ToString("yyyy-MM-dd") + ")\r\n";
                            }
                            else
                            {
                                string str4 = str;
                                str = str4 + Program.ULManager.Files[i].Product + " " + Program.ULManager.Files[i].ServicePack + " " + Program.ULManager.Files[i].Platform + " " + Program.ULManager.Files[i].Language + " (" + time.ToString("yyyy-MM-dd") + ")\r\n";
                            }
                        }
                    }
                }
                if ((str.Length > 0) && (MessageBox.Show("The following ULs have been updated:\r\n\r\n" + str + "\r\nWould you like to download them now?", "Windows Updates Downloader", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes))
                {
                    Process.Start("\"http://www.windowsupdatesdownloader.com/\"");
                }
            }
        }

        private void buttonAddUL_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://www.windowsupdatesdownloader.com/");
            }
            catch (Exception exception)
            {
                if (exception is Win32Exception)
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo("iexplore.exe", "http://www.windowsupdatesdownloader.com/"));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unable to launch default browser. Please visit http://www.windowsupdatesdownloader.com/ to download update lists.", "Windows Updates Downloader", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                else
                {
                    MessageBox.Show("Unable to launch default browser. Please visit http://www.windowsupdatesdownloader.com/ to download update lists.", "Windows Updates Downloader", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (this.mode == OperatingMode.User)
            {
                this.buttonCancel.Enabled = false;
                Program.DLManager.Abort();
            }
            if (this.mode == OperatingMode.Automated)
            {
                this.buttonCancel.Enabled = false;
                Program.DLManager.Abort();
                Environment.ExitCode = 1;
            }
        }

        private void buttonChangeFolder_Click(object sender, EventArgs e)
        {
            this.folderDownload.SelectedPath = this._downloadpath;
            this.folderDownload.ShowDialog(this);
            this._downloadpath = this.folderDownload.SelectedPath;
            this.folderDownload.Dispose();
            this.textboxPath.Text = this._downloadpath;
            this.refreshNodeStates();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            Program.DLManager.Queue.Clear();
            this.loadNodesToQueue(this.treeUpdates.Nodes);
            if (Program.DLManager.Queue.Count > 0)
            {
                if (this.checkProductSubFolders.Checked)
                {
                    Program.DLManager.DownloadPath = Path.Combine(this._downloadpath, (Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].Product + " " + Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].ServicePack + " " + Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].Platform + " " + Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].Language).Replace("  ", " "));
                }
                else
                {
                    Program.DLManager.DownloadPath = this._downloadpath;
                }
                Program.DLManager.UseSubFolders = this.checkCategorySubFolders.Checked;
                if (this.checkRemoveUnlisted.Checked && Directory.Exists(Program.DLManager.DownloadPath))
                {
                    this.removeUnlistedFiles(Program.DLManager.DownloadPath);
                }
                if (this.checkUseProxy.Checked)
                {
                    try
                    {
                        Program.DLManager.Proxy = new WebProxy(this.textboxProxyURL.Text, Convert.ToInt32(this.textboxProxyPort.Text));
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("The proxy address or the port is invalid.", "Windows Updates Downloader", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (this.checkUseProxyAuthentication.Checked)
                    {
                        Program.DLManager.Proxy.Credentials = new NetworkCredential(this.textboxProxyUsername.Text, this.textboxProxyPassword.Text);
                    }
                }
                this.labelUpdateList.Hide();
                this.comboUpdateLists.Hide();
                this.labelDownloadFolder.Hide();
                this.textboxPath.Hide();
                this.buttonChangeFolder.Hide();
                this.treeUpdates.Hide();
                this.labelWrittenBy.Hide();
                this.buttonAddUL.Hide();
                this.buttonRefreshULs.Hide();
                this.buttonManageULs.Hide();
                this.linkMSFN.Hide();
                this.checkCategorySubFolders.Hide();
                this.buttonDownload.Hide();
                this.groupDownloadProgress.Show();
                this.buttonCancel.Show();
                base.Height = (this.buttonCancel.Top + this.buttonCancel.Height) + 0x29;
                this.labelCurrent.Text = "";
                this.labelBytes.Text = "";
                this.labelStatus.Text = "";
                this.progressFile.Value = 0;
                this.progressTotal.Value = 0;
                this.progressTotal.Maximum = Program.DLManager.Queue.Count;
                Program.DLManager.Mode = OperatingMode.User;
                Program.DLManager.Download();
            }
        }

        private void buttonManageULs_Click(object sender, EventArgs e)
        {
            formManage manage = new formManage();
            manage.ShowDialog(this);
            this.refreshULs();
            manage.Dispose();
        }

        private void buttonOptions_Click(object sender, EventArgs e)
        {
            this.buttonOptions.Tag = !((bool) this.buttonOptions.Tag);
            if ((bool) this.buttonOptions.Tag)
            {
                this.buttonOptions.Text = "Hide Options";
                base.Height = (this.groupProxy.Top + this.groupProxy.Height) + 0x29;
            }
            else
            {
                this.buttonOptions.Text = "Show Options";
                base.Height = (this.buttonDownload.Top + this.buttonDownload.Height) + 0x29;
            }
        }

        private void buttonRefreshULs_Click(object sender, EventArgs e)
        {
            this.refreshULs();
        }

        private void checkCategorySubFolders_CheckedChanged(object sender, EventArgs e)
        {
            this.refreshNodeStates();
        }

        private void checkProductSubFolders_CheckedChanged(object sender, EventArgs e)
        {
            this.refreshNodeStates();
        }

        private void checkRemoveUnlisted_Click(object sender, EventArgs e)
        {
            if (this.checkRemoveUnlisted.Checked && (MessageBox.Show("IMPORTANT!\r\n\r\nENABLING THIS OPTION WITHOUT READING OR UNDERSTANDING THE FOLLOWING WARNING MAY CAUSE PERSONAL DATA LOSS OR CORRUPTION.\r\n\r\nWhen enabled, this option will, at the moment the Download button is clicked, compare the contents of the selected download folder to the files listed in the Update List. Any files or folders found in the selected download folder which are not listed in the Update List will be removed permanently. Enabling this option while selecting a download folder that contains other data will cause data loss once download begins. Only enable this option if you understand how this option works.\r\n\r\nAre you sure you wish to ENABLE this option?", "Windows Updates Downloader", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No))
            {
                this.checkRemoveUnlisted.Checked = false;
            }
        }

        private void checkShowDescriptions_CheckedChanged(object sender, EventArgs e)
        {
            this.treeUpdates.ShowNodeToolTips = this.checkShowDescriptions.Checked;
        }

        private void checkShowPublishDates_CheckedChanged(object sender, EventArgs e)
        {
            if (this.comboUpdateLists.SelectedIndex >= 0)
            {
                this.LoadUpdateList();
            }
        }

        private void checkUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkUseProxy.Checked)
            {
                this.textboxProxyURL.Enabled = true;
                this.textboxProxyPort.Enabled = true;
                this.checkUseProxyAuthentication.Enabled = true;
                if (this.checkUseProxyAuthentication.Checked)
                {
                    this.textboxProxyUsername.Enabled = true;
                    this.textboxProxyPassword.Enabled = true;
                }
                else
                {
                    this.textboxProxyUsername.Enabled = false;
                    this.textboxProxyPassword.Enabled = false;
                }
            }
            else
            {
                this.textboxProxyURL.Enabled = false;
                this.textboxProxyPort.Enabled = false;
                this.checkUseProxyAuthentication.Enabled = false;
                this.textboxProxyUsername.Enabled = false;
                this.textboxProxyPassword.Enabled = false;
            }
        }

        private void checkUseProxyAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkUseProxy.Checked)
            {
                this.textboxProxyURL.Enabled = true;
                this.textboxProxyPort.Enabled = true;
                this.checkUseProxyAuthentication.Enabled = true;
                if (this.checkUseProxyAuthentication.Checked)
                {
                    this.textboxProxyUsername.Enabled = true;
                    this.textboxProxyPassword.Enabled = true;
                }
                else
                {
                    this.textboxProxyUsername.Enabled = false;
                    this.textboxProxyPassword.Enabled = false;
                }
            }
            else
            {
                this.textboxProxyURL.Enabled = false;
                this.textboxProxyPort.Enabled = false;
                this.checkUseProxyAuthentication.Enabled = false;
                this.textboxProxyUsername.Enabled = false;
                this.textboxProxyPassword.Enabled = false;
            }
        }

        private void comboUpdateLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadUpdateList();
        }

        private void copyData_DataReceived(object sender, WUD.SimpleInterprocessCommunications.DataReceivedEventArgs e)
        {
            if (((e.ChannelName.Equals("WUD") && (e.Data != null)) && ((e.Data.GetType() == typeof(string)) && (((string) e.Data) == "Refresh"))) && (Program.DLManager.Status == DownloadManager.DownloadStatus.Idle))
            {
                this.refreshULs();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DownloadCompleteCallback()
        {
            if (this.progressTotal.InvokeRequired)
            {
                DownloadCompleteHandler method = new DownloadCompleteHandler(this.DownloadCompleteCallback);
                base.Invoke(method);
            }
            else
            {
                if (this.mode == OperatingMode.User)
                {
                    this.groupDownloadProgress.Hide();
                    this.buttonCancel.Hide();
                    this.buttonCancel.Enabled = true;
                    this.labelUpdateList.Show();
                    this.comboUpdateLists.Show();
                    this.labelDownloadFolder.Show();
                    this.textboxPath.Show();
                    this.buttonChangeFolder.Show();
                    this.refreshNodeStates();
                    this.treeUpdates.Show();
                    this.labelWrittenBy.Show();
                    this.buttonAddUL.Show();
                    this.buttonRefreshULs.Show();
                    this.buttonManageULs.Show();
                    this.linkMSFN.Show();
                    this.checkCategorySubFolders.Show();
                    this.buttonDownload.Show();
                    if ((bool) this.buttonOptions.Tag)
                    {
                        base.Height = (this.groupProxy.Top + this.groupProxy.Height) + 0x29;
                    }
                    else
                    {
                        base.Height = (this.buttonDownload.Top + this.buttonDownload.Height) + 0x29;
                    }
                    if (this.notifyWUDIcon.Visible)
                    {
                        this.notifyWUDIcon.ShowBalloonTip(0x3a98, "Windows Updates Downloader", "Download complete.", ToolTipIcon.Info);
                    }
                }
                if (this.mode == OperatingMode.Automated)
                {
                    Application.Exit();
                }
            }
        }

        private void DownloadProgressCallback()
        {
            if (this.progressTotal.InvokeRequired)
            {
                DownloadProgressHandler method = new DownloadProgressHandler(this.DownloadProgressCallback);
                base.Invoke(method);
            }
            else
            {
                if (Program.DLManager.CurrentDownload.bytesAvailable != -1)
                {
                    this.progressFile.Minimum = 0;
                    this.progressFile.Maximum = Program.DLManager.CurrentDownload.bytesAvailable;
                    this.progressFile.Value = Program.DLManager.CurrentDownload.bytesReceived;
                    this.progressTotal.Value = Program.DLManager.CurrentDownload.queueIndex;
                    string title = Program.ULManager.Updates[Program.DLManager.Queue[Program.DLManager.CurrentDownload.queueIndex]].Title;
                    if (this.checkShowPublishDates.Checked)
                    {
                        title = title + " [" + Program.ULManager.Updates[Program.DLManager.Queue[Program.DLManager.CurrentDownload.queueIndex]].PublishDate.ToString("yyyy-MM-dd") + "]";
                    }
                    this.labelCurrent.Text = title;
                    this.labelBytes.Text = $"{Math.Round((double) (((float) Program.DLManager.CurrentDownload.bytesReceived) / 1048576f), 2):#,##0.00}" + " MB of " + $"{Math.Round((double) (((float) Program.DLManager.CurrentDownload.bytesAvailable) / 1048576f), 2):#,##0.00}" + " MB";
                    this.labelStatus.Text = "Downloading item " + Convert.ToString((int) (Program.DLManager.CurrentDownload.queueIndex + 1)) + " of " + Convert.ToString(Program.DLManager.Queue.Count);
                }
                else
                {
                    this.progressTotal.Value = Program.DLManager.CurrentDownload.queueIndex;
                    this.labelBytes.Text = $"{Math.Round((double) (((double) Program.DLManager.CurrentDownload.bytesReceived) / 1048576.0), 2):#,##0.00}" + " MB";
                    string str2 = Program.ULManager.Updates[Program.DLManager.Queue[Program.DLManager.CurrentDownload.queueIndex]].Title;
                    if (this.checkShowPublishDates.Checked)
                    {
                        str2 = str2 + " [" + Program.ULManager.Updates[Program.DLManager.Queue[Program.DLManager.CurrentDownload.queueIndex]].PublishDate.ToString("yyyy-MM-dd") + "]";
                    }
                    this.labelCurrent.Text = str2;
                    this.labelBytes.Text = $"{Math.Round((double) (((float) Program.DLManager.CurrentDownload.bytesReceived) / 1048576f), 2):#,##0.00}" + " MB of " + $"{Math.Round((double) (((float) Program.DLManager.CurrentDownload.bytesAvailable) / 1048576f), 2):#,##0.00}" + " MB";
                    this.labelStatus.Text = "Downloading item " + Convert.ToString((int) (Program.DLManager.CurrentDownload.queueIndex + 1)) + " of " + Convert.ToString(Program.DLManager.Queue.Count);
                }
                if (this.notifyWUDIcon.Visible)
                {
                    this.notifyWUDIcon.Text = "Windows Updates Downloader\nDownloading item " + Convert.ToString((int) (Program.DLManager.CurrentDownload.queueIndex + 1)) + " of " + Convert.ToString(Program.DLManager.Queue.Count);
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern int DrawMenuBar(IntPtr hwnd);
        private void formMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mode == OperatingMode.User)
            {
                Settings settings = new Settings(Program.SettingsPath, "Settings");
                settings.WriteString("Program", "DownloadPath", this._downloadpath);
                if (this.comboUpdateLists.Items.Count > 0)
                {
                    settings.WriteString("Program", "LastUL", Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].Filename);
                }
                else
                {
                    settings.WriteString("Program", "LastUL", this._downloadpath);
                }
                settings.WriteBool("Program", "ProductSubFolders", this.checkProductSubFolders.Checked);
                settings.WriteBool("Program", "CategorySubFolders", this.checkCategorySubFolders.Checked);
                settings.WriteBool("Program", "MinimizeToTray", this.checkMinimizeToTray.Checked);
                settings.WriteBool("Program", "RemoveUnlisted", this.checkRemoveUnlisted.Checked);
                settings.WriteBool("Program", "ShowPublishDates", this.checkShowPublishDates.Checked);
                settings.WriteBool("Program", "ShowDescriptions", this.checkShowDescriptions.Checked);
                settings.WriteInt("Program", "VersionCheck", this.comboVersionCheck.SelectedIndex);
                settings.WriteBool("Proxy", "Enabled", this.checkUseProxy.Checked);
                settings.WriteString("Proxy", "Address", this.textboxProxyURL.Text);
                settings.WriteInt("Proxy", "Port", Convert.ToInt32(this.textboxProxyPort.Text));
                settings.WriteBool("Proxy", "Authentication", this.checkUseProxyAuthentication.Checked);
                settings.WriteString("Proxy", "Username", this.textboxProxyUsername.Text);
                settings.WriteString("Proxy", "Password", Supremus.Encryption.Encryption.Encrypt(this.textboxProxyPassword.Text));
                settings.Save();
                settings = null;
            }
        }

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((this.mode == OperatingMode.User) && (Program.DLManager.Status == DownloadManager.DownloadStatus.Running))
            {
                e.Cancel = true;
                this.buttonCancel.Enabled = false;
                Program.DLManager.Abort();
            }
            if (this.mode == OperatingMode.Automated)
            {
                e.Cancel = false;
                this.buttonCancel.Enabled = false;
                Program.DLManager.Abort();
            }
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            this.copyData = new CopyData();
            this.copyData.AssignHandle(base.Handle);
            this.copyData.Channels.Add("WUD");
            this.copyData.DataReceived += new WUD.SimpleInterprocessCommunications.DataReceivedEventHandler(this.copyData_DataReceived);
            Program.DLManager.ProgressCallback += new DownloadProgressHandler(this.DownloadProgressCallback);
            Program.DLManager.CompleteCallback += new DownloadCompleteHandler(this.DownloadCompleteCallback);
            if (this.mode == OperatingMode.User)
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                string text = this.Text;
                this.Text = text + " " + versionInfo.ProductMajorPart.ToString() + "." + versionInfo.ProductMinorPart.ToString() + " Build " + versionInfo.ProductBuildPart.ToString();
                Settings settings = new Settings(Program.SettingsPath, "Settings");
                this._downloadpath = settings.ReadString("Program", "DownloadPath", Program.UserDataPath);
                this.textboxPath.Text = this._downloadpath;
                this.checkProductSubFolders.Checked = settings.ReadBool("Program", "ProductSubFolders", false);
                this.checkCategorySubFolders.Checked = settings.ReadBool("Program", "CategorySubFolders", true);
                this.checkShowPublishDates.Checked = settings.ReadBool("Program", "ShowPublishDates", false);
                this.checkShowDescriptions.Checked = settings.ReadBool("Program", "ShowDescriptions", true);
                this.checkMinimizeToTray.Checked = settings.ReadBool("Program", "MinimizeToTray", false);
                this.checkRemoveUnlisted.Checked = settings.ReadBool("Program", "RemoveUnlisted", false);
                this.comboVersionCheck.SelectedIndex = settings.ReadInt("Program", "VersionCheck", 2);
                this.checkUseProxy.Checked = settings.ReadBool("Proxy", "Enabled", false);
                this.textboxProxyURL.Text = settings.ReadString("Proxy", "Address", "");
                this.textboxProxyPort.Text = settings.ReadInt("Proxy", "Port", 0x1f90).ToString();
                this.checkUseProxyAuthentication.Checked = settings.ReadBool("Proxy", "Authentication", false);
                this.textboxProxyUsername.Text = settings.ReadString("Proxy", "Username", "");
                this.textboxProxyPassword.Text = Supremus.Encryption.Encryption.Decrypt(settings.ReadString("Proxy", "Password", ""));
                this.comboUpdateLists.Items.Clear();
                for (int i = 0; i < Program.ULManager.Files.Count; i++)
                {
                    if (Program.ULManager.Files[i].ServicePack == "any")
                    {
                        this.comboUpdateLists.Items.Add(Program.ULManager.Files[i].Product + " " + Program.ULManager.Files[i].Platform + " " + Program.ULManager.Files[i].Language + " (" + Program.ULManager.Files[i].LastUpdate.ToString("yyyy-MM-dd") + ")");
                    }
                    else
                    {
                        this.comboUpdateLists.Items.Add(Program.ULManager.Files[i].Product + " " + Program.ULManager.Files[i].ServicePack + " " + Program.ULManager.Files[i].Platform + " " + Program.ULManager.Files[i].Language + " (" + Program.ULManager.Files[i].LastUpdate.ToString("yyyy-MM-dd") + ")");
                    }
                }
                if (this.comboUpdateLists.Items.Count > 0)
                {
                    this.comboUpdateLists.SelectedIndex = 0;
                    string str = settings.ReadString("Program", "LastUL", "");
                    if (str.Length > 0)
                    {
                        for (int j = 0; j < this.comboUpdateLists.Items.Count; j++)
                        {
                            if (Program.ULManager.Files[j].Filename == str)
                            {
                                this.comboUpdateLists.SelectedIndex = j;
                            }
                        }
                    }
                }
                this.refreshNodeStates();
                settings = null;
            }
            else if (this.mode == OperatingMode.Automated)
            {
                this.labelUpdateList.Hide();
                this.comboUpdateLists.Hide();
                this.labelDownloadFolder.Hide();
                this.textboxPath.Hide();
                this.buttonChangeFolder.Hide();
                this.treeUpdates.Hide();
                this.labelWrittenBy.Hide();
                this.buttonAddUL.Hide();
                this.buttonRefreshULs.Hide();
                this.buttonManageULs.Hide();
                this.linkMSFN.Hide();
                this.checkCategorySubFolders.Hide();
                this.buttonDownload.Hide();
                this.groupDownloadProgress.Show();
                this.buttonCancel.Show();
                base.Height = (this.buttonCancel.Top + this.buttonCancel.Height) + 0x29;
                IntPtr systemMenu = GetSystemMenu(base.Handle, 0);
                int menuItemCount = GetMenuItemCount(systemMenu);
                RemoveMenu(systemMenu, menuItemCount - 1, 0x402);
                RemoveMenu(systemMenu, menuItemCount - 2, 0x402);
                DrawMenuBar(base.Handle);
                this.labelCurrent.Text = "";
                this.labelBytes.Text = "";
                this.labelStatus.Text = "";
                this.progressFile.Value = 0;
                this.progressTotal.Value = 0;
                this.progressTotal.Maximum = Program.DLManager.Queue.Count;
                Program.DLManager.Mode = OperatingMode.Automated;
                Program.DLManager.Download();
            }
        }

        private void formMain_Resize(object sender, EventArgs e)
        {
            if (base.Visible)
            {
                if (this.checkMinimizeToTray.Checked && (base.WindowState == FormWindowState.Minimized))
                {
                    this.notifyWUDIcon.Visible = true;
                    base.Hide();
                }
                if (this.groupDownloadProgress.Visible && (base.WindowState == FormWindowState.Normal))
                {
                    base.Height = (this.buttonCancel.Top + this.buttonCancel.Height) + 0x29;
                }
                if (!this.groupDownloadProgress.Visible && (base.WindowState == FormWindowState.Normal))
                {
                    if ((bool) this.buttonOptions.Tag)
                    {
                        base.Height = (this.groupProxy.Top + this.groupProxy.Height) + 0x29;
                    }
                    else
                    {
                        base.Height = (this.buttonDownload.Top + this.buttonDownload.Height) + 0x29;
                    }
                }
            }
        }

        private void formMain_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (this.mode == OperatingMode.User)
            {
                if (this.comboUpdateLists.Items.Count <= 0)
                {
                    MessageBox.Show(this, "No update list found.\r\n\r\nTo use the Windows Updates Downloader, you must install an Update List (UL).\r\n\r\nSimply click on the plus icon to open up a list of available ULs, click on the UL you would like and choose Open.\r\n\r\nAfter the UL is installed, you can click on the Refresh icon and it will appear in the list of ULs.", "Windows Updates Downloader", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                if (this.comboVersionCheck.SelectedIndex > 0)
                {
                    this.backgroundVersionCheck.RunWorkerAsync(this.comboVersionCheck.SelectedIndex);
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern int GetMenuItemCount(IntPtr hmenu);
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);
        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(formMain));
            this.comboUpdateLists = new ComboBox();
            this.buttonChangeFolder = new Button();
            this.buttonDownload = new Button();
            this.folderDownload = new FolderBrowserDialog();
            this.groupDownloadProgress = new GroupBox();
            this.labelStatus = new Label();
            this.labelBytes = new Label();
            this.progressFile = new ProgressBar();
            this.labelCurrent = new Label();
            this.progressTotal = new ProgressBar();
            this.buttonCancel = new Button();
            this.groupOptions = new GroupBox();
            this.checkRemoveUnlisted = new CheckBox();
            this.checkShowDescriptions = new CheckBox();
            this.checkShowPublishDates = new CheckBox();
            this.comboVersionCheck = new ComboBox();
            this.checkMinimizeToTray = new CheckBox();
            this.checkProductSubFolders = new CheckBox();
            this.checkCategorySubFolders = new CheckBox();
            this.notifyWUDIcon = new NotifyIcon(this.components);
            this.labelDownloadFolder = new Label();
            this.labelUpdateList = new Label();
            this.buttonOptions = new Button();
            this.labelWrittenBy = new Label();
            this.linkMSFN = new LinkLabel();
            this.textboxPath = new TextBox();
            this.stateImages = new ImageList(this.components);
            this.backgroundVersionCheck = new BackgroundWorker();
            this.groupProxy = new GroupBox();
            this.checkUseProxyAuthentication = new CheckBox();
            this.textboxProxyPassword = new TextBox();
            this.labelProxyPassword = new Label();
            this.textboxProxyUsername = new TextBox();
            this.labelProxyUsername = new Label();
            this.textboxProxyPort = new TextBox();
            this.labelProxyPort = new Label();
            this.textboxProxyURL = new TextBox();
            this.labelProxyURL = new Label();
            this.checkUseProxy = new CheckBox();
            this.contextUpdateList = new ContextMenuStrip(this.components);
            this.addUpdateListToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.refreshToolStripMenuItem = new ToolStripMenuItem();
            this.manageToolStripMenuItem = new ToolStripMenuItem();
            this.buttonAddUL = new Button();
            this.buttonRefreshULs = new Button();
            this.buttonManageULs = new Button();
            this.linkCheckAll = new LinkLabel();
            this.labelCheckSeperator = new Label();
            this.linkLabel1 = new LinkLabel();
            this.treeUpdates = new TriStateTreeView();
            this.groupDownloadProgress.SuspendLayout();
            this.groupOptions.SuspendLayout();
            this.groupProxy.SuspendLayout();
            this.contextUpdateList.SuspendLayout();
            base.SuspendLayout();
            this.comboUpdateLists.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboUpdateLists.Font = new Font("Verdana", 8.25f);
            this.comboUpdateLists.FormattingEnabled = true;
            this.comboUpdateLists.Location = new Point(12, 0x19);
            this.comboUpdateLists.Name = "comboUpdateLists";
            this.comboUpdateLists.Size = new Size(0x157, 0x15);
            this.comboUpdateLists.TabIndex = 2;
            this.comboUpdateLists.SelectedIndexChanged += new EventHandler(this.comboUpdateLists_SelectedIndexChanged);
            this.buttonChangeFolder.Font = new Font("Verdana", 8.25f);
            this.buttonChangeFolder.Location = new Point(0x26d, 0x3f);
            this.buttonChangeFolder.Name = "buttonChangeFolder";
            this.buttonChangeFolder.Size = new Size(0x4b, 0x18);
            this.buttonChangeFolder.TabIndex = 7;
            this.buttonChangeFolder.Text = "Change";
            this.buttonChangeFolder.UseVisualStyleBackColor = true;
            this.buttonChangeFolder.Click += new EventHandler(this.buttonChangeFolder_Click);
            this.buttonDownload.Font = new Font("Verdana", 8.25f);
            this.buttonDownload.Location = new Point(0x26d, 360);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new Size(0x4b, 0x18);
            this.buttonDownload.TabIndex = 12;
            this.buttonDownload.Text = "Download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new EventHandler(this.buttonDownload_Click);
            this.folderDownload.Description = "Choose the folder where the updates should be downloaded.";
            this.groupDownloadProgress.Controls.Add(this.labelStatus);
            this.groupDownloadProgress.Controls.Add(this.labelBytes);
            this.groupDownloadProgress.Controls.Add(this.progressFile);
            this.groupDownloadProgress.Controls.Add(this.labelCurrent);
            this.groupDownloadProgress.Controls.Add(this.progressTotal);
            this.groupDownloadProgress.Font = new Font("Verdana", 8.25f);
            this.groupDownloadProgress.Location = new Point(12, 0x238);
            this.groupDownloadProgress.Name = "groupDownloadProgress";
            this.groupDownloadProgress.Size = new Size(0x2aa, 0x93);
            this.groupDownloadProgress.TabIndex = 0x20;
            this.groupDownloadProgress.TabStop = false;
            this.groupDownloadProgress.Text = "Download";
            this.groupDownloadProgress.Visible = false;
            this.labelStatus.Location = new Point(6, 0x75);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new Size(670, 0x15);
            this.labelStatus.TabIndex = 0x25;
            this.labelStatus.TextAlign = ContentAlignment.MiddleCenter;
            this.labelBytes.Location = new Point(6, 0x43);
            this.labelBytes.Name = "labelBytes";
            this.labelBytes.Size = new Size(670, 0x15);
            this.labelBytes.TabIndex = 0x23;
            this.labelBytes.TextAlign = ContentAlignment.MiddleCenter;
            this.progressFile.Location = new Point(6, 0x29);
            this.progressFile.Name = "progressFile";
            this.progressFile.Size = new Size(670, 0x17);
            this.progressFile.TabIndex = 0x22;
            this.labelCurrent.Location = new Point(6, 0x11);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new Size(670, 0x15);
            this.labelCurrent.TabIndex = 0x21;
            this.labelCurrent.TextAlign = ContentAlignment.MiddleCenter;
            this.progressTotal.Location = new Point(6, 0x5b);
            this.progressTotal.Name = "progressTotal";
            this.progressTotal.Size = new Size(670, 0x17);
            this.progressTotal.TabIndex = 0x24;
            this.buttonCancel.Font = new Font("Verdana", 8.25f);
            this.buttonCancel.Location = new Point(0x26d, 0xa5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x18);
            this.buttonCancel.TabIndex = 0x26;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Visible = false;
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            this.groupOptions.Controls.Add(this.checkRemoveUnlisted);
            this.groupOptions.Controls.Add(this.checkShowDescriptions);
            this.groupOptions.Controls.Add(this.checkShowPublishDates);
            this.groupOptions.Controls.Add(this.comboVersionCheck);
            this.groupOptions.Controls.Add(this.checkMinimizeToTray);
            this.groupOptions.Controls.Add(this.checkProductSubFolders);
            this.groupOptions.Controls.Add(this.checkCategorySubFolders);
            this.groupOptions.Font = new Font("Verdana", 8.25f);
            this.groupOptions.Location = new Point(12, 400);
            this.groupOptions.Name = "groupOptions";
            this.groupOptions.Size = new Size(0x2aa, 0x49);
            this.groupOptions.TabIndex = 13;
            this.groupOptions.TabStop = false;
            this.groupOptions.Text = "Options";
            this.checkRemoveUnlisted.AutoSize = true;
            this.checkRemoveUnlisted.Location = new Point(0x1c8, 0x2d);
            this.checkRemoveUnlisted.Name = "checkRemoveUnlisted";
            this.checkRemoveUnlisted.Size = new Size(0x94, 0x11);
            this.checkRemoveUnlisted.TabIndex = 20;
            this.checkRemoveUnlisted.Text = "Remove unlisted files";
            this.checkRemoveUnlisted.UseVisualStyleBackColor = true;
            this.checkRemoveUnlisted.Click += new EventHandler(this.checkRemoveUnlisted_Click);
            this.checkShowDescriptions.AutoSize = true;
            this.checkShowDescriptions.Location = new Point(0x1c8, 20);
            this.checkShowDescriptions.Name = "checkShowDescriptions";
            this.checkShowDescriptions.Size = new Size(0xa8, 0x11);
            this.checkShowDescriptions.TabIndex = 0x11;
            this.checkShowDescriptions.Text = "Show description tooltips";
            this.checkShowDescriptions.UseVisualStyleBackColor = true;
            this.checkShowDescriptions.CheckedChanged += new EventHandler(this.checkShowDescriptions_CheckedChanged);
            this.checkShowPublishDates.AutoSize = true;
            this.checkShowPublishDates.Location = new Point(0x13a, 20);
            this.checkShowPublishDates.Name = "checkShowPublishDates";
            this.checkShowPublishDates.Size = new Size(0x88, 0x11);
            this.checkShowPublishDates.TabIndex = 0x10;
            this.checkShowPublishDates.Text = "Show publish dates";
            this.checkShowPublishDates.UseVisualStyleBackColor = true;
            this.checkShowPublishDates.CheckedChanged += new EventHandler(this.checkShowPublishDates_CheckedChanged);
            this.comboVersionCheck.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboVersionCheck.Font = new Font("Verdana", 8.25f);
            this.comboVersionCheck.FormattingEnabled = true;
            this.comboVersionCheck.Items.AddRange(new object[] { "Do not check for updates", "Check for updates", "Check for updates, including beta versions" });
            this.comboVersionCheck.Location = new Point(6, 0x2b);
            this.comboVersionCheck.Name = "comboVersionCheck";
            this.comboVersionCheck.Size = new Size(0x127, 0x15);
            this.comboVersionCheck.TabIndex = 0x12;
            this.checkMinimizeToTray.AutoSize = true;
            this.checkMinimizeToTray.Location = new Point(0x13a, 0x2d);
            this.checkMinimizeToTray.Name = "checkMinimizeToTray";
            this.checkMinimizeToTray.Size = new Size(0x75, 0x11);
            this.checkMinimizeToTray.TabIndex = 0x13;
            this.checkMinimizeToTray.Text = "Minimize to tray";
            this.checkMinimizeToTray.UseVisualStyleBackColor = true;
            this.checkProductSubFolders.AutoSize = true;
            this.checkProductSubFolders.Location = new Point(6, 20);
            this.checkProductSubFolders.Name = "checkProductSubFolders";
            this.checkProductSubFolders.Size = new Size(0x8f, 0x11);
            this.checkProductSubFolders.TabIndex = 14;
            this.checkProductSubFolders.Text = "Product as subfolder";
            this.checkProductSubFolders.UseVisualStyleBackColor = true;
            this.checkProductSubFolders.CheckedChanged += new EventHandler(this.checkProductSubFolders_CheckedChanged);
            this.checkCategorySubFolders.AutoSize = true;
            this.checkCategorySubFolders.Location = new Point(0x9b, 20);
            this.checkCategorySubFolders.Name = "checkCategorySubFolders";
            this.checkCategorySubFolders.Size = new Size(0x99, 0x11);
            this.checkCategorySubFolders.TabIndex = 15;
            this.checkCategorySubFolders.Text = "Category as subfolder";
            this.checkCategorySubFolders.UseVisualStyleBackColor = true;
            this.checkCategorySubFolders.CheckedChanged += new EventHandler(this.checkCategorySubFolders_CheckedChanged);
            this.notifyWUDIcon.Icon = (Icon) manager.GetObject("notifyWUDIcon.Icon");
            this.notifyWUDIcon.Text = "Windows Updates Downloader";
            this.notifyWUDIcon.MouseDoubleClick += new MouseEventHandler(this.notifyWUDIcon_MouseDoubleClick);
            this.labelDownloadFolder.AutoSize = true;
            this.labelDownloadFolder.Font = new Font("Verdana", 8.25f);
            this.labelDownloadFolder.Location = new Point(12, 0x31);
            this.labelDownloadFolder.Name = "labelDownloadFolder";
            this.labelDownloadFolder.Size = new Size(0x66, 13);
            this.labelDownloadFolder.TabIndex = 5;
            this.labelDownloadFolder.Text = "Download Folder";
            this.labelUpdateList.AutoSize = true;
            this.labelUpdateList.Font = new Font("Verdana", 8.25f);
            this.labelUpdateList.Location = new Point(12, 9);
            this.labelUpdateList.Name = "labelUpdateList";
            this.labelUpdateList.Size = new Size(70, 13);
            this.labelUpdateList.TabIndex = 0;
            this.labelUpdateList.Text = "Update List";
            this.buttonOptions.Font = new Font("Verdana", 8.25f);
            this.buttonOptions.ImageAlign = ContentAlignment.MiddleLeft;
            this.buttonOptions.Location = new Point(12, 360);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new Size(0x66, 0x18);
            this.buttonOptions.TabIndex = 9;
            this.buttonOptions.Tag = "";
            this.buttonOptions.Text = "Show Options";
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new EventHandler(this.buttonOptions_Click);
            this.labelWrittenBy.AutoSize = true;
            this.labelWrittenBy.Font = new Font("Verdana", 8.25f);
            this.labelWrittenBy.Location = new Point(0x1f6, 9);
            this.labelWrittenBy.Name = "labelWrittenBy";
            this.labelWrittenBy.Size = new Size(0xc0, 13);
            this.labelWrittenBy.TabIndex = 10;
            this.labelWrittenBy.Text = "Written by Jean-Sebastien Carle";
            this.linkMSFN.AutoSize = true;
            this.linkMSFN.Font = new Font("Verdana", 8.25f);
            this.linkMSFN.Location = new Point(0x233, 0x1c);
            this.linkMSFN.Name = "linkMSFN";
            this.linkMSFN.Size = new Size(0x85, 13);
            this.linkMSFN.TabIndex = 4;
            this.linkMSFN.TabStop = true;
            this.linkMSFN.Text = "MSFN Support Forums";
            this.linkMSFN.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkMSFN_LinkClicked);
            this.textboxPath.BackColor = SystemColors.Info;
            this.textboxPath.Font = new Font("Verdana", 8.25f);
            this.textboxPath.Location = new Point(12, 0x41);
            this.textboxPath.Name = "textboxPath";
            this.textboxPath.ReadOnly = true;
            this.textboxPath.Size = new Size(0x25b, 0x15);
            this.textboxPath.TabIndex = 6;
            this.stateImages.ImageStream = (ImageListStreamer) manager.GetObject("stateImages.ImageStream");
            this.stateImages.TransparentColor = Color.Transparent;
            this.stateImages.Images.SetKeyName(0, "package-white.png");
            this.stateImages.Images.SetKeyName(1, "package-yellow.png");
            this.stateImages.Images.SetKeyName(2, "package-red.png");
            this.stateImages.Images.SetKeyName(3, "package-green.png");
            this.backgroundVersionCheck.DoWork += new DoWorkEventHandler(this.backgroundVersionCheck_DoWork);
            this.groupProxy.Controls.Add(this.checkUseProxyAuthentication);
            this.groupProxy.Controls.Add(this.textboxProxyPassword);
            this.groupProxy.Controls.Add(this.labelProxyPassword);
            this.groupProxy.Controls.Add(this.textboxProxyUsername);
            this.groupProxy.Controls.Add(this.labelProxyUsername);
            this.groupProxy.Controls.Add(this.textboxProxyPort);
            this.groupProxy.Controls.Add(this.labelProxyPort);
            this.groupProxy.Controls.Add(this.textboxProxyURL);
            this.groupProxy.Controls.Add(this.labelProxyURL);
            this.groupProxy.Controls.Add(this.checkUseProxy);
            this.groupProxy.Font = new Font("Verdana", 8.25f);
            this.groupProxy.Location = new Point(12, 0x1df);
            this.groupProxy.Name = "groupProxy";
            this.groupProxy.Size = new Size(0x2aa, 0x53);
            this.groupProxy.TabIndex = 0x15;
            this.groupProxy.TabStop = false;
            this.groupProxy.Text = "Proxy Settings";
            this.checkUseProxyAuthentication.AutoSize = true;
            this.checkUseProxyAuthentication.Location = new Point(0x194, 20);
            this.checkUseProxyAuthentication.Name = "checkUseProxyAuthentication";
            this.checkUseProxyAuthentication.Size = new Size(0x84, 0x11);
            this.checkUseProxyAuthentication.TabIndex = 0x17;
            this.checkUseProxyAuthentication.Text = "Use Authentication";
            this.checkUseProxyAuthentication.UseVisualStyleBackColor = true;
            this.checkUseProxyAuthentication.CheckedChanged += new EventHandler(this.checkUseProxyAuthentication_CheckedChanged);
            this.textboxProxyPassword.Enabled = false;
            this.textboxProxyPassword.Location = new Point(0x21f, 0x38);
            this.textboxProxyPassword.Name = "textboxProxyPassword";
            this.textboxProxyPassword.Size = new Size(0x85, 0x15);
            this.textboxProxyPassword.TabIndex = 0x1f;
            this.textboxProxyPassword.UseSystemPasswordChar = true;
            this.labelProxyPassword.AutoSize = true;
            this.labelProxyPassword.Location = new Point(0x21f, 40);
            this.labelProxyPassword.Name = "labelProxyPassword";
            this.labelProxyPassword.Size = new Size(0x3d, 13);
            this.labelProxyPassword.TabIndex = 30;
            this.labelProxyPassword.Text = "Password";
            this.textboxProxyUsername.Enabled = false;
            this.textboxProxyUsername.Location = new Point(0x194, 0x38);
            this.textboxProxyUsername.Name = "textboxProxyUsername";
            this.textboxProxyUsername.Size = new Size(0x85, 0x15);
            this.textboxProxyUsername.TabIndex = 0x1d;
            this.labelProxyUsername.AutoSize = true;
            this.labelProxyUsername.Location = new Point(0x194, 40);
            this.labelProxyUsername.Name = "labelProxyUsername";
            this.labelProxyUsername.Size = new Size(0x41, 13);
            this.labelProxyUsername.TabIndex = 0x1c;
            this.labelProxyUsername.Text = "Username";
            this.textboxProxyPort.Enabled = false;
            this.textboxProxyPort.Location = new Point(0x158, 0x38);
            this.textboxProxyPort.Name = "textboxProxyPort";
            this.textboxProxyPort.Size = new Size(0x36, 0x15);
            this.textboxProxyPort.TabIndex = 0x1b;
            this.labelProxyPort.AutoSize = true;
            this.labelProxyPort.Location = new Point(0x158, 40);
            this.labelProxyPort.Name = "labelProxyPort";
            this.labelProxyPort.Size = new Size(30, 13);
            this.labelProxyPort.TabIndex = 0x1a;
            this.labelProxyPort.Text = "Port";
            this.textboxProxyURL.Enabled = false;
            this.textboxProxyURL.Location = new Point(6, 0x38);
            this.textboxProxyURL.Name = "textboxProxyURL";
            this.textboxProxyURL.Size = new Size(0x14c, 0x15);
            this.textboxProxyURL.TabIndex = 0x19;
            this.labelProxyURL.AutoSize = true;
            this.labelProxyURL.Location = new Point(6, 40);
            this.labelProxyURL.Name = "labelProxyURL";
            this.labelProxyURL.Size = new Size(0x35, 13);
            this.labelProxyURL.TabIndex = 0x18;
            this.labelProxyURL.Text = "Address";
            this.checkUseProxy.AutoSize = true;
            this.checkUseProxy.Location = new Point(6, 20);
            this.checkUseProxy.Name = "checkUseProxy";
            this.checkUseProxy.Size = new Size(0x54, 0x11);
            this.checkUseProxy.TabIndex = 0x16;
            this.checkUseProxy.Text = "Use Proxy";
            this.checkUseProxy.UseVisualStyleBackColor = true;
            this.checkUseProxy.CheckedChanged += new EventHandler(this.checkUseProxy_CheckedChanged);
            this.contextUpdateList.Items.AddRange(new ToolStripItem[] { this.addUpdateListToolStripMenuItem, this.toolStripSeparator1, this.refreshToolStripMenuItem, this.manageToolStripMenuItem });
            this.contextUpdateList.Name = "contextUpdateList";
            this.contextUpdateList.Size = new Size(0x9f, 0x4c);
            this.addUpdateListToolStripMenuItem.Name = "addUpdateListToolStripMenuItem";
            this.addUpdateListToolStripMenuItem.Size = new Size(0x9e, 0x16);
            this.addUpdateListToolStripMenuItem.Text = "&Add Update List";
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0x9b, 6);
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new Size(0x9e, 0x16);
            this.refreshToolStripMenuItem.Text = "&Refresh";
            this.manageToolStripMenuItem.Name = "manageToolStripMenuItem";
            this.manageToolStripMenuItem.Size = new Size(0x9e, 0x16);
            this.manageToolStripMenuItem.Text = "&Manage";
            this.buttonAddUL.BackgroundImage = (Image) manager.GetObject("buttonAddUL.BackgroundImage");
            this.buttonAddUL.BackgroundImageLayout = ImageLayout.Center;
            this.buttonAddUL.FlatAppearance.BorderColor = SystemColors.ButtonShadow;
            this.buttonAddUL.FlatAppearance.MouseDownBackColor = SystemColors.MenuHighlight;
            this.buttonAddUL.FlatAppearance.MouseOverBackColor = SystemColors.ButtonHighlight;
            this.buttonAddUL.Location = new Point(0x167, 0x17);
            this.buttonAddUL.Name = "buttonAddUL";
            this.buttonAddUL.Size = new Size(0x18, 0x18);
            this.buttonAddUL.TabIndex = 0x2a;
            this.buttonAddUL.UseVisualStyleBackColor = true;
            this.buttonAddUL.Click += new EventHandler(this.buttonAddUL_Click);
            this.buttonRefreshULs.BackgroundImage = (Image) manager.GetObject("buttonRefreshULs.BackgroundImage");
            this.buttonRefreshULs.BackgroundImageLayout = ImageLayout.Center;
            this.buttonRefreshULs.FlatAppearance.BorderColor = SystemColors.ButtonShadow;
            this.buttonRefreshULs.FlatAppearance.MouseDownBackColor = SystemColors.MenuHighlight;
            this.buttonRefreshULs.FlatAppearance.MouseOverBackColor = SystemColors.ButtonHighlight;
            this.buttonRefreshULs.Location = new Point(0x181, 0x17);
            this.buttonRefreshULs.Name = "buttonRefreshULs";
            this.buttonRefreshULs.Size = new Size(0x18, 0x18);
            this.buttonRefreshULs.TabIndex = 0x2b;
            this.buttonRefreshULs.UseVisualStyleBackColor = true;
            this.buttonRefreshULs.Click += new EventHandler(this.buttonRefreshULs_Click);
            this.buttonManageULs.BackgroundImage = (Image) manager.GetObject("buttonManageULs.BackgroundImage");
            this.buttonManageULs.BackgroundImageLayout = ImageLayout.Center;
            this.buttonManageULs.FlatAppearance.BorderColor = SystemColors.ButtonShadow;
            this.buttonManageULs.FlatAppearance.MouseDownBackColor = SystemColors.MenuHighlight;
            this.buttonManageULs.FlatAppearance.MouseOverBackColor = SystemColors.ButtonHighlight;
            this.buttonManageULs.Location = new Point(0x19b, 0x17);
            this.buttonManageULs.Name = "buttonManageULs";
            this.buttonManageULs.Size = new Size(0x18, 0x18);
            this.buttonManageULs.TabIndex = 0x2c;
            this.buttonManageULs.UseVisualStyleBackColor = true;
            this.buttonManageULs.Click += new EventHandler(this.buttonManageULs_Click);
            this.linkCheckAll.AutoSize = true;
            this.linkCheckAll.Font = new Font("Verdana", 8.25f);
            this.linkCheckAll.Location = new Point(0x11b, 0x16e);
            this.linkCheckAll.Name = "linkCheckAll";
            this.linkCheckAll.Size = new Size(0x3d, 13);
            this.linkCheckAll.TabIndex = 0x2d;
            this.linkCheckAll.TabStop = true;
            this.linkCheckAll.Text = "Check All";
            this.linkCheckAll.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkCheckAll_LinkClicked);
            this.labelCheckSeperator.AutoSize = true;
            this.labelCheckSeperator.Font = new Font("Verdana", 8.25f);
            this.labelCheckSeperator.Location = new Point(0x157, 0x16e);
            this.labelCheckSeperator.Name = "labelCheckSeperator";
            this.labelCheckSeperator.Size = new Size(12, 13);
            this.labelCheckSeperator.TabIndex = 0x2e;
            this.labelCheckSeperator.Text = "/";
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new Font("Verdana", 8.25f);
            this.linkLabel1.Location = new Point(0x161, 0x16e);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(0x49, 13);
            this.linkLabel1.TabIndex = 0x2f;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Uncheck All";
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.treeUpdates.Font = new Font("Verdana", 8.25f);
            this.treeUpdates.HotTracking = true;
            this.treeUpdates.ImageIndex = 1;
            this.treeUpdates.Location = new Point(12, 0x5c);
            this.treeUpdates.Name = "treeUpdates";
            this.treeUpdates.SelectedImageIndex = 1;
            this.treeUpdates.ShowNodeToolTips = true;
            this.treeUpdates.Size = new Size(0x2ac, 0x106);
            this.treeUpdates.StateImageList = this.stateImages;
            this.treeUpdates.TabIndex = 8;
            this.treeUpdates.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(this.treeUpdates_NodeMouseDoubleClick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x2c4, 0x2e3);
            base.Controls.Add(this.linkLabel1);
            base.Controls.Add(this.labelCheckSeperator);
            base.Controls.Add(this.linkCheckAll);
            base.Controls.Add(this.buttonManageULs);
            base.Controls.Add(this.buttonRefreshULs);
            base.Controls.Add(this.groupProxy);
            base.Controls.Add(this.labelWrittenBy);
            base.Controls.Add(this.buttonAddUL);
            base.Controls.Add(this.textboxPath);
            base.Controls.Add(this.linkMSFN);
            base.Controls.Add(this.buttonOptions);
            base.Controls.Add(this.groupOptions);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.labelDownloadFolder);
            base.Controls.Add(this.labelUpdateList);
            base.Controls.Add(this.buttonDownload);
            base.Controls.Add(this.treeUpdates);
            base.Controls.Add(this.groupDownloadProgress);
            base.Controls.Add(this.buttonChangeFolder);
            base.Controls.Add(this.comboUpdateLists);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "formMain";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Windows Updates Downloader";
            base.Load += new EventHandler(this.formMain_Load);
            base.Shown += new EventHandler(this.formMain_Shown);
            base.FormClosed += new FormClosedEventHandler(this.formMain_FormClosed);
            base.FormClosing += new FormClosingEventHandler(this.formMain_FormClosing);
            base.Resize += new EventHandler(this.formMain_Resize);
            this.groupDownloadProgress.ResumeLayout(false);
            this.groupOptions.ResumeLayout(false);
            this.groupOptions.PerformLayout();
            this.groupProxy.ResumeLayout(false);
            this.groupProxy.PerformLayout();
            this.contextUpdateList.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void linkCheckAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.treeUpdates.CheckAll();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.treeUpdates.UncheckAll();
        }

        private void linkMSFN_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://www.msfn.org/board/windows-updates-downloader-f147.html");
            }
            catch (Exception exception)
            {
                if (exception is Win32Exception)
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo("iexplore.exe", "http://www.msfn.org/board/windows-updates-downloader-f147.html"));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unable to launch default browser. Please visit http://www.msfn.org/board/windows-updates-downloader-f147.html for support.", "Windows Updates Downloader", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                else
                {
                    MessageBox.Show("Unable to launch default browser. Please visit http://www.msfn.org/board/windows-updates-downloader-f147.html for support.", "Windows Updates Downloader", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void loadNodesToQueue(TreeNodeCollection treeNodes)
        {
            foreach (TreeNode node in treeNodes)
            {
                if (node.Nodes.Count > 0)
                {
                    this.loadNodesToQueue(node.Nodes);
                }
                if ((node.Parent != null) && (this.treeUpdates.GetChecked(node) == TriStateTreeView.CheckState.Checked))
                {
                    Program.DLManager.Queue.Add((int) node.Tag);
                }
            }
        }

        private void LoadUpdateList()
        {
            Program.ULManager.Load(this.comboUpdateLists.SelectedIndex);
            this.treeUpdates.BeginUpdate();
            this.treeUpdates.Nodes.Clear();
            this.treeUpdates.ShowNodeToolTips = this.checkShowDescriptions.Checked;
            for (int i = 0; i < Program.ULManager.Categories.Count; i++)
            {
                this.treeUpdates.Nodes.Add(Program.ULManager.Categories[i].Description);
                int num2 = this.treeUpdates.Nodes.Count - 1;
                this.treeUpdates.Nodes[num2].Tag = i;
                for (int j = 0; j < Program.ULManager.Updates.Count; j++)
                {
                    if (Program.ULManager.Updates[j].Category == Program.ULManager.Categories[i].ID)
                    {
                        this.treeUpdates.Nodes[num2].Nodes.Add(Program.ULManager.Updates[j].Title);
                        int num4 = this.treeUpdates.Nodes[num2].Nodes.Count - 1;
                        if (this.checkShowPublishDates.Checked)
                        {
                            TreeNode node1 = this.treeUpdates.Nodes[num2].Nodes[num4];
                            node1.Text = node1.Text + " [" + Program.ULManager.Updates[j].PublishDate.ToString("yyyy-MM-dd") + "]";
                        }
                        this.treeUpdates.Nodes[num2].Nodes[num4].ToolTipText = Program.ULManager.Updates[j].Description;
                        this.treeUpdates.Nodes[num2].Nodes[num4].Tag = j;
                        if (Program.ULManager.Updates[j].Article.Length > 0)
                        {
                            this.treeUpdates.Nodes[num2].Nodes[num4].NodeFont = new Font("Verdana", 8.25f, FontStyle.Underline);
                            this.treeUpdates.Nodes[num2].Nodes[num4].ForeColor = SystemColors.Highlight;
                        }
                        this.treeUpdates.Nodes[num2].Nodes[num4].StateImageIndex = 0;
                    }
                }
                TreeNode node2 = this.treeUpdates.Nodes[num2];
                node2.Text = node2.Text + " (" + this.treeUpdates.Nodes[num2].Nodes.Count.ToString() + ")";
                this.treeUpdates.Nodes[num2].NodeFont = new Font("Verdana", 8.25f, FontStyle.Bold);
                this.treeUpdates.Nodes[num2].StateImageIndex = 0;
            }
            this.treeUpdates.EndUpdate();
            this.refreshNodeStates();
        }

        private void notifyWUDIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            base.Show();
            base.WindowState = FormWindowState.Normal;
            this.notifyWUDIcon.Visible = false;
        }

        private void refreshNodeStates()
        {
            this.treeUpdates.BeginUpdate();
            this.updateNodeStates(this.treeUpdates.Nodes);
            this.treeUpdates.EndUpdate();
        }

        private void refreshULs()
        {
            Program.ULManager = new UpdateListManager(Path.GetDirectoryName(Program.SettingsPath));
            this.treeUpdates.BeginUpdate();
            this.treeUpdates.Nodes.Clear();
            this.treeUpdates.EndUpdate();
            this.comboUpdateLists.Items.Clear();
            for (int i = 0; i < Program.ULManager.Files.Count; i++)
            {
                if (Program.ULManager.Files[i].ServicePack == "any")
                {
                    this.comboUpdateLists.Items.Add(Program.ULManager.Files[i].Product + " " + Program.ULManager.Files[i].Platform + " " + Program.ULManager.Files[i].Language + " (" + Program.ULManager.Files[i].LastUpdate.ToString("yyyy-MM-dd") + ")");
                }
                else
                {
                    this.comboUpdateLists.Items.Add(Program.ULManager.Files[i].Product + " " + Program.ULManager.Files[i].ServicePack + " " + Program.ULManager.Files[i].Platform + " " + Program.ULManager.Files[i].Language + " (" + Program.ULManager.Files[i].LastUpdate.ToString("yyyy-MM-dd") + ")");
                }
            }
            if (this.comboUpdateLists.Items.Count > 0)
            {
                this.comboUpdateLists.SelectedIndex = 0;
                Settings settings = new Settings(Program.SettingsPath, "Settings");
                string str = settings.ReadString("Program", "LastUL", "");
                if (str.Length > 0)
                {
                    for (int j = 0; j < this.comboUpdateLists.Items.Count; j++)
                    {
                        if (Program.ULManager.Files[j].Filename == str)
                        {
                            this.comboUpdateLists.SelectedIndex = j;
                        }
                    }
                }
                settings = null;
            }
        }

        [DllImport("user32.dll")]
        private static extern int RemoveMenu(IntPtr hmenu, int npos, int wflags);
        private void removeUnlistedFiles(string folder)
        {
            foreach (string str in Directory.GetDirectories(folder))
            {
                if ((System.IO.File.GetAttributes(str) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                {
                    this.removeUnlistedFiles(str);
                }
            }
            foreach (string str2 in Directory.GetFiles(folder))
            {
                bool flag = false;
                for (int i = 0; i < Program.ULManager.Updates.Count; i++)
                {
                    if (Program.ULManager.Updates[i].Filename.ToUpper() == Path.GetFileName(str2).ToUpper())
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    System.IO.File.Delete(str2);
                }
            }
        }

        private void treeUpdates_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e.Node.Parent != null) && (Program.ULManager.Updates[(int) e.Node.Tag].Article.Length > 0))
            {
                Process.Start(Program.ULManager.Updates[(int) e.Node.Tag].Article);
            }
        }

        private int updateNodeStates(TreeNodeCollection treeNodes)
        {
            int num = 0;
            foreach (TreeNode node in treeNodes)
            {
                if (node.Nodes.Count > 0)
                {
                    switch (this.updateNodeStates(node.Nodes))
                    {
                        case -1:
                            node.StateImageIndex = 3;
                            break;

                        case 0:
                            node.StateImageIndex = 0;
                            break;

                        case 1:
                            node.StateImageIndex = 1;
                            break;
                    }
                }
                if (node.Parent != null)
                {
                    string str = this._downloadpath;
                    if (this.checkProductSubFolders.Checked)
                    {
                        str = Path.Combine(str, Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].Product + " " + Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].Platform + " " + Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].Language);
                    }
                    if (this.checkCategorySubFolders.Checked)
                    {
                        str = Path.Combine(str, Program.ULManager.Categories[(int) node.Parent.Tag].Description);
                    }
                    if (System.IO.File.Exists(Path.Combine(str, Program.ULManager.Updates[(int) node.Tag].Filename)))
                    {
                        node.StateImageIndex = 3;
                        num++;
                    }
                }
            }
            if (num == treeNodes.Count)
            {
                return -1;
            }
            if (num > 0)
            {
                return 1;
            }
            return 0;
        }

        private int VersionCompare(string version1, string version2)
        {
            string[] strArray = version1.Split(new char[] { '.' });
            string[] strArray2 = version2.Split(new char[] { '.' });
            int num = Convert.ToInt32(strArray[0]);
            int num2 = 0;
            if (strArray.Length >= 2)
            {
                num2 = Convert.ToInt32(strArray[1]);
            }
            int num3 = 0;
            if (strArray.Length >= 3)
            {
                num3 = Convert.ToInt32(strArray[2]);
            }
            int num4 = 0;
            if (strArray.Length >= 4)
            {
                num4 = Convert.ToInt32(strArray[3]);
            }
            int num5 = Convert.ToInt32(strArray2[0]);
            int num6 = 0;
            if (strArray2.Length >= 2)
            {
                num6 = Convert.ToInt32(strArray2[1]);
            }
            int num7 = 0;
            if (strArray2.Length >= 3)
            {
                num7 = Convert.ToInt32(strArray2[2]);
            }
            int num8 = 0;
            if (strArray2.Length >= 4)
            {
                num8 = Convert.ToInt32(strArray2[3]);
            }
            if (num > num5)
            {
                return -1;
            }
            if (num < num5)
            {
                return 1;
            }
            if (num2 > num6)
            {
                return -1;
            }
            if (num2 < num6)
            {
                return 1;
            }
            if (num3 > num7)
            {
                return -1;
            }
            if (num3 < num7)
            {
                return 1;
            }
            if (num4 > num8)
            {
                return -1;
            }
            if (num4 < num8)
            {
                return 1;
            }
            return 0;
        }
    }
}

