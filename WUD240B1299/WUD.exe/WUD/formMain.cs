namespace WUD
{
    using Supremus.Configuration;
    using Supremus.Encryption;
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
        private CheckBox checkUseProxy;
        private CheckBox checkUseProxyAuthentication;
        private ComboBox comboUpdateLists;
        private ComboBox comboVersionCheck;
        private IContainer components;
        private ContextMenuStrip contextUpdateList;
        private CopyData copyData;
        private FormSizes CurrentSize;
        private FolderBrowserDialog folderDownload;
        private GroupBox groupDownloadProgress;
        private GroupBox groupOptions;
        private GroupBox groupProxy;
        private Label labelBytes;
        private Label labelCurrent;
        private Label labelDownloadFolder;
        private Label labelProxyPassword;
        private Label labelProxyPort;
        private Label labelProxyURL;
        private Label labelProxyUsername;
        private Label labelStatus;
        private Label labelToolTipText;
        private Label labelUpdateList;
        private Label labelWrittenBy;
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
        private List<DataGridView> updateDataGridViews = new List<DataGridView>();
        private TabControl updateTabControl;
        private List<TabPage> updateTabPages = new List<TabPage>();

        public formMain()
        {
            this.InitializeComponent();
            this.groupDownloadProgress.Top = 9;
            if (this.mode == OperatingMode.User)
            {
                this.setFormSize(FormSizes.Normal);
            }
            else if (this.mode == OperatingMode.Automated)
            {
                this.setFormSize(FormSizes.Downloading);
            }
            base.Width = this.MinimumSize.Width;
            base.Height = this.MinimumSize.Height;
            this.resizeForm();
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
                if (this.VersionCompare(strArray[1], versionInfo.ProductVersion) == -1)
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
            Process.Start("\"http://www.windowsupdatesdownloader.com/\"");
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
            this.loadDataGridViewsToQueue();
            if (Program.DLManager.Queue.Count > 0)
            {
                if (this.checkProductSubFolders.Checked)
                {
                    Program.DLManager.DownloadPath = Path.Combine(this._downloadpath, Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].Product + " " + Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].Platform + " " + Program.ULManager.Files[this.comboUpdateLists.SelectedIndex].Language);
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
                this.updateTabControl.Hide();
                this.labelWrittenBy.Hide();
                this.buttonAddUL.Hide();
                this.buttonRefreshULs.Hide();
                this.buttonManageULs.Hide();
                this.linkMSFN.Hide();
                this.checkCategorySubFolders.Hide();
                this.buttonDownload.Hide();
                this.groupDownloadProgress.Show();
                this.buttonCancel.Show();
                this.setFormSize(FormSizes.Downloading);
                this.resizeForm();
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
                this.setFormSize(FormSizes.NormalWithOptions);
            }
            else
            {
                this.buttonOptions.Text = "Show Options";
                this.setFormSize(FormSizes.Normal);
            }
            this.resizeForm();
        }

        private void buttonRefreshULs_Click(object sender, EventArgs e)
        {
            this.refreshULs();
        }

        private void checkallLinkLabel_Click(object sender, EventArgs e)
        {
            int tag = (int) ((LinkLabel) sender).Tag;
            foreach (DataGridViewRow row in (IEnumerable) this.updateDataGridViews[tag].Rows)
            {
                ((DataGridViewCheckBoxCell) row.Cells[1]).Value = true;
            }
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
            if (!this.checkShowDescriptions.Checked)
            {
                this.labelToolTipText.Visible = false;
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

        private void ClearTabs()
        {
            for (int i = 0; i < this.updateTabPages.Count; i++)
            {
                while (this.updateTabPages[i].Controls.Count > 0)
                {
                    this.updateTabPages[i].Controls.RemoveAt(0);
                }
                this.updateDataGridViews[i].Dispose();
                this.updateTabPages[i].Dispose();
            }
            this.updateDataGridViews.Clear();
            this.updateTabPages.Clear();
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
                    this.updateTabControl.Show();
                    this.labelWrittenBy.Show();
                    this.buttonAddUL.Show();
                    this.buttonRefreshULs.Show();
                    this.buttonManageULs.Show();
                    this.linkMSFN.Show();
                    this.checkCategorySubFolders.Show();
                    this.buttonDownload.Show();
                    if ((bool) this.buttonOptions.Tag)
                    {
                        this.setFormSize(FormSizes.NormalWithOptions);
                        this.resizeForm();
                    }
                    else
                    {
                        this.setFormSize(FormSizes.Normal);
                        this.resizeForm();
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
                    this.labelCurrent.Text = Program.ULManager.Updates[Program.DLManager.Queue[Program.DLManager.CurrentDownload.queueIndex]].Title;
                    this.labelBytes.Text = $"{Math.Round((double) (((float) Program.DLManager.CurrentDownload.bytesReceived) / 1048576f), 2):#,##0.00}" + " MB of " + $"{Math.Round((double) (((float) Program.DLManager.CurrentDownload.bytesAvailable) / 1048576f), 2):#,##0.00}" + " MB";
                    this.labelStatus.Text = "Downloading item " + Convert.ToString((int) (Program.DLManager.CurrentDownload.queueIndex + 1)) + " of " + Convert.ToString(Program.DLManager.Queue.Count);
                }
                else
                {
                    this.progressTotal.Value = Program.DLManager.CurrentDownload.queueIndex;
                    this.labelBytes.Text = $"{Math.Round((double) (((double) Program.DLManager.CurrentDownload.bytesReceived) / 1048576.0), 2):#,##0.00}" + " MB";
                    this.labelCurrent.Text = Program.ULManager.Updates[Program.DLManager.Queue[Program.DLManager.CurrentDownload.queueIndex]].Title;
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
        private void formMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataGridView view = (DataGridView) sender;
                view[e.ColumnIndex, e.RowIndex].Value = !((bool) view[e.ColumnIndex, e.RowIndex].Value);
            }
        }

        private void formMain_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DataGridView view = (DataGridView) sender;
                string article = Program.ULManager.Updates[(int) view[0, e.RowIndex].Value].Article;
                if (!string.IsNullOrEmpty(article))
                {
                    Process.Start(article);
                }
            }
        }

        private void formMain_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if ((this.checkShowDescriptions.Checked && (e.ColumnIndex == 2)) && (e.RowIndex >= 0))
            {
                DataGridView view = (DataGridView) sender;
                string description = Program.ULManager.Updates[(int) view[0, e.RowIndex].Value].Description;
                if (!string.IsNullOrEmpty(description))
                {
                    this.labelToolTipText.Text = description;
                    this.labelToolTipText.Visible = true;
                }
            }
        }

        private void formMain_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (this.labelToolTipText.Visible)
            {
                this.labelToolTipText.Visible = false;
            }
        }

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
                settings.WriteBool("Program", "ShowDescriptions", this.checkShowDescriptions.Checked);
                settings.WriteInt("Program", "VersionCheck", this.comboVersionCheck.SelectedIndex);
                settings.WriteBool("Proxy", "Enabled", this.checkUseProxy.Checked);
                settings.WriteString("Proxy", "Address", this.textboxProxyURL.Text);
                settings.WriteInt("Proxy", "ProxyPort", Convert.ToInt32(this.textboxProxyPort.Text));
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

        private void formMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataGridView view = (DataGridView) sender;
            if ((e.KeyChar == ' ') && (view.SelectedRows.Count > 0))
            {
                foreach (DataGridViewRow row in view.SelectedRows)
                {
                    this.formMain_CellContentClick(view, new DataGridViewCellEventArgs(1, row.Index));
                }
                e.Handled = true;
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
                this.updateTabControl.Hide();
                this.labelWrittenBy.Hide();
                this.buttonAddUL.Hide();
                this.buttonRefreshULs.Hide();
                this.buttonManageULs.Hide();
                this.linkMSFN.Hide();
                this.checkCategorySubFolders.Hide();
                this.buttonDownload.Hide();
                this.groupDownloadProgress.Show();
                this.buttonCancel.Show();
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
                else if (this.groupDownloadProgress.Visible && (base.WindowState == FormWindowState.Normal))
                {
                    this.setFormSize(FormSizes.Downloading);
                }
                else if (!this.groupDownloadProgress.Visible && (base.WindowState == FormWindowState.Normal))
                {
                    if ((bool) this.buttonOptions.Tag)
                    {
                        this.setFormSize(FormSizes.NormalWithOptions);
                    }
                    else
                    {
                        this.setFormSize(FormSizes.Normal);
                    }
                }
                this.resizeForm();
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
                if (this.updateDataGridViews.Count > 0)
                {
                    this.updateDataGridViews[0].Focus();
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
            this.updateTabControl = new TabControl();
            this.labelToolTipText = new Label();
            this.groupDownloadProgress.SuspendLayout();
            this.groupOptions.SuspendLayout();
            this.groupProxy.SuspendLayout();
            this.contextUpdateList.SuspendLayout();
            base.SuspendLayout();
            this.comboUpdateLists.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboUpdateLists.Font = new Font("Segoe UI", 8.25f);
            this.comboUpdateLists.FormattingEnabled = true;
            this.comboUpdateLists.Location = new Point(12, 0x22);
            this.comboUpdateLists.Name = "comboUpdateLists";
            this.comboUpdateLists.Size = new Size(0x157, 0x15);
            this.comboUpdateLists.TabIndex = 1;
            this.comboUpdateLists.SelectedIndexChanged += new EventHandler(this.comboUpdateLists_SelectedIndexChanged);
            this.buttonChangeFolder.Font = new Font("Segoe UI", 8.25f);
            this.buttonChangeFolder.Location = new Point(0x26d, 0x51);
            this.buttonChangeFolder.Name = "buttonChangeFolder";
            this.buttonChangeFolder.Size = new Size(0x4b, 0x18);
            this.buttonChangeFolder.TabIndex = 9;
            this.buttonChangeFolder.Text = "Change";
            this.buttonChangeFolder.UseVisualStyleBackColor = true;
            this.buttonChangeFolder.Click += new EventHandler(this.buttonChangeFolder_Click);
            this.buttonDownload.Font = new Font("Segoe UI", 8.25f);
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
            this.groupDownloadProgress.Font = new Font("Segoe UI", 8.25f);
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
            this.buttonCancel.Font = new Font("Segoe UI", 8.25f);
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
            this.groupOptions.Controls.Add(this.comboVersionCheck);
            this.groupOptions.Controls.Add(this.checkMinimizeToTray);
            this.groupOptions.Controls.Add(this.checkProductSubFolders);
            this.groupOptions.Controls.Add(this.checkCategorySubFolders);
            this.groupOptions.Font = new Font("Segoe UI", 8.25f);
            this.groupOptions.Location = new Point(12, 400);
            this.groupOptions.Name = "groupOptions";
            this.groupOptions.Size = new Size(0x2aa, 0x49);
            this.groupOptions.TabIndex = 13;
            this.groupOptions.TabStop = false;
            this.groupOptions.Text = "Options";
            this.checkRemoveUnlisted.AutoSize = true;
            this.checkRemoveUnlisted.Location = new Point(0x1c8, 0x2d);
            this.checkRemoveUnlisted.Name = "checkRemoveUnlisted";
            this.checkRemoveUnlisted.Size = new Size(0x87, 0x11);
            this.checkRemoveUnlisted.TabIndex = 20;
            this.checkRemoveUnlisted.Text = "Remove unlisted files";
            this.checkRemoveUnlisted.UseVisualStyleBackColor = true;
            this.checkRemoveUnlisted.Click += new EventHandler(this.checkRemoveUnlisted_Click);
            this.checkShowDescriptions.AutoSize = true;
            this.checkShowDescriptions.Location = new Point(0x1c8, 20);
            this.checkShowDescriptions.Name = "checkShowDescriptions";
            this.checkShowDescriptions.Size = new Size(0x9f, 0x11);
            this.checkShowDescriptions.TabIndex = 0x11;
            this.checkShowDescriptions.Text = "Show description tooltips";
            this.checkShowDescriptions.UseVisualStyleBackColor = true;
            this.checkShowDescriptions.CheckedChanged += new EventHandler(this.checkShowDescriptions_CheckedChanged);
            this.comboVersionCheck.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboVersionCheck.Font = new Font("Segoe UI", 8.25f);
            this.comboVersionCheck.FormattingEnabled = true;
            this.comboVersionCheck.Items.AddRange(new object[] { "Do not check for updates", "Check for updates", "Check for updates, including beta versions" });
            this.comboVersionCheck.Location = new Point(6, 0x2b);
            this.comboVersionCheck.Name = "comboVersionCheck";
            this.comboVersionCheck.Size = new Size(0x127, 0x15);
            this.comboVersionCheck.TabIndex = 0x12;
            this.checkMinimizeToTray.AutoSize = true;
            this.checkMinimizeToTray.Location = new Point(0x13a, 20);
            this.checkMinimizeToTray.Name = "checkMinimizeToTray";
            this.checkMinimizeToTray.Size = new Size(0x6c, 0x11);
            this.checkMinimizeToTray.TabIndex = 0x10;
            this.checkMinimizeToTray.Text = "Minimize to tray";
            this.checkMinimizeToTray.UseVisualStyleBackColor = true;
            this.checkProductSubFolders.AutoSize = true;
            this.checkProductSubFolders.Location = new Point(6, 20);
            this.checkProductSubFolders.Name = "checkProductSubFolders";
            this.checkProductSubFolders.Size = new Size(0x85, 0x11);
            this.checkProductSubFolders.TabIndex = 14;
            this.checkProductSubFolders.Text = "Product as subfolder";
            this.checkProductSubFolders.UseVisualStyleBackColor = true;
            this.checkProductSubFolders.CheckedChanged += new EventHandler(this.checkProductSubFolders_CheckedChanged);
            this.checkCategorySubFolders.AutoSize = true;
            this.checkCategorySubFolders.Location = new Point(0x9b, 20);
            this.checkCategorySubFolders.Name = "checkCategorySubFolders";
            this.checkCategorySubFolders.Size = new Size(0x8b, 0x11);
            this.checkCategorySubFolders.TabIndex = 15;
            this.checkCategorySubFolders.Text = "Category as subfolder";
            this.checkCategorySubFolders.UseVisualStyleBackColor = true;
            this.checkCategorySubFolders.CheckedChanged += new EventHandler(this.checkCategorySubFolders_CheckedChanged);
            this.notifyWUDIcon.Icon = (Icon) manager.GetObject("notifyWUDIcon.Icon");
            this.notifyWUDIcon.Text = "Windows Updates Downloader";
            this.notifyWUDIcon.MouseDoubleClick += new MouseEventHandler(this.notifyWUDIcon_MouseDoubleClick);
            this.labelDownloadFolder.AutoSize = true;
            this.labelDownloadFolder.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelDownloadFolder.Location = new Point(9, 0x43);
            this.labelDownloadFolder.Name = "labelDownloadFolder";
            this.labelDownloadFolder.Size = new Size(0x61, 13);
            this.labelDownloadFolder.TabIndex = 7;
            this.labelDownloadFolder.Text = "Download Folder";
            this.labelUpdateList.AutoSize = true;
            this.labelUpdateList.Font = new Font("Segoe UI", 8.25f);
            this.labelUpdateList.Location = new Point(9, 0x12);
            this.labelUpdateList.Name = "labelUpdateList";
            this.labelUpdateList.Size = new Size(0x41, 13);
            this.labelUpdateList.TabIndex = 0;
            this.labelUpdateList.Text = "Update List";
            this.buttonOptions.Font = new Font("Segoe UI", 8.25f);
            this.buttonOptions.ImageAlign = ContentAlignment.MiddleLeft;
            this.buttonOptions.Location = new Point(12, 360);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new Size(0x66, 0x18);
            this.buttonOptions.TabIndex = 11;
            this.buttonOptions.Tag = "";
            this.buttonOptions.Text = "Show Options";
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new EventHandler(this.buttonOptions_Click);
            this.labelWrittenBy.AutoSize = true;
            this.labelWrittenBy.Font = new Font("Segoe UI", 8.25f);
            this.labelWrittenBy.Location = new Point(0x20e, 0x26);
            this.labelWrittenBy.Name = "labelWrittenBy";
            this.labelWrittenBy.Size = new Size(170, 13);
            this.labelWrittenBy.TabIndex = 6;
            this.labelWrittenBy.Text = "Written by Jean-Sebastien Carle";
            this.linkMSFN.AutoSize = true;
            this.linkMSFN.Font = new Font("Segoe UI", 8.25f);
            this.linkMSFN.Location = new Point(0x23d, 0x12);
            this.linkMSFN.Name = "linkMSFN";
            this.linkMSFN.Size = new Size(0x7b, 13);
            this.linkMSFN.TabIndex = 5;
            this.linkMSFN.TabStop = true;
            this.linkMSFN.Text = "MSFN Support Forums";
            this.linkMSFN.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkMSFN_LinkClicked);
            this.textboxPath.BackColor = SystemColors.Info;
            this.textboxPath.Font = new Font("Segoe UI", 8.25f);
            this.textboxPath.ForeColor = SystemColors.InfoText;
            this.textboxPath.Location = new Point(12, 0x53);
            this.textboxPath.Name = "textboxPath";
            this.textboxPath.ReadOnly = true;
            this.textboxPath.Size = new Size(0x25b, 0x16);
            this.textboxPath.TabIndex = 8;
            this.stateImages.ImageStream = (ImageListStreamer) manager.GetObject("stateImages.ImageStream");
            this.stateImages.TransparentColor = Color.Transparent;
            this.stateImages.Images.SetKeyName(0, "cube2-grey.ico");
            this.stateImages.Images.SetKeyName(1, "cube2-yellow.ico");
            this.stateImages.Images.SetKeyName(2, "cube2-red.ico");
            this.stateImages.Images.SetKeyName(3, "cube2-green.ico");
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
            this.groupProxy.Font = new Font("Segoe UI", 8.25f);
            this.groupProxy.Location = new Point(12, 0x1df);
            this.groupProxy.Name = "groupProxy";
            this.groupProxy.Size = new Size(0x2aa, 0x53);
            this.groupProxy.TabIndex = 0x15;
            this.groupProxy.TabStop = false;
            this.groupProxy.Text = "Proxy Settings";
            this.checkUseProxyAuthentication.AutoSize = true;
            this.checkUseProxyAuthentication.Location = new Point(0x194, 20);
            this.checkUseProxyAuthentication.Name = "checkUseProxyAuthentication";
            this.checkUseProxyAuthentication.Size = new Size(0x7d, 0x11);
            this.checkUseProxyAuthentication.TabIndex = 0x1b;
            this.checkUseProxyAuthentication.Text = "Use Authentication";
            this.checkUseProxyAuthentication.UseVisualStyleBackColor = true;
            this.checkUseProxyAuthentication.CheckedChanged += new EventHandler(this.checkUseProxyAuthentication_CheckedChanged);
            this.textboxProxyPassword.Enabled = false;
            this.textboxProxyPassword.Location = new Point(0x21f, 0x38);
            this.textboxProxyPassword.Name = "textboxProxyPassword";
            this.textboxProxyPassword.Size = new Size(0x85, 0x16);
            this.textboxProxyPassword.TabIndex = 0x1f;
            this.textboxProxyPassword.UseSystemPasswordChar = true;
            this.labelProxyPassword.AutoSize = true;
            this.labelProxyPassword.Location = new Point(540, 40);
            this.labelProxyPassword.Name = "labelProxyPassword";
            this.labelProxyPassword.Size = new Size(0x38, 13);
            this.labelProxyPassword.TabIndex = 30;
            this.labelProxyPassword.Text = "Password";
            this.textboxProxyUsername.Enabled = false;
            this.textboxProxyUsername.Location = new Point(0x194, 0x38);
            this.textboxProxyUsername.Name = "textboxProxyUsername";
            this.textboxProxyUsername.Size = new Size(0x85, 0x16);
            this.textboxProxyUsername.TabIndex = 0x1d;
            this.labelProxyUsername.AutoSize = true;
            this.labelProxyUsername.Location = new Point(0x191, 40);
            this.labelProxyUsername.Name = "labelProxyUsername";
            this.labelProxyUsername.Size = new Size(0x3a, 13);
            this.labelProxyUsername.TabIndex = 0x1c;
            this.labelProxyUsername.Text = "Username";
            this.textboxProxyPort.Enabled = false;
            this.textboxProxyPort.Location = new Point(0x158, 0x38);
            this.textboxProxyPort.Name = "textboxProxyPort";
            this.textboxProxyPort.Size = new Size(0x36, 0x16);
            this.textboxProxyPort.TabIndex = 0x1a;
            this.labelProxyPort.AutoSize = true;
            this.labelProxyPort.Location = new Point(0x155, 40);
            this.labelProxyPort.Name = "labelProxyPort";
            this.labelProxyPort.Size = new Size(0x1c, 13);
            this.labelProxyPort.TabIndex = 0x19;
            this.labelProxyPort.Text = "Port";
            this.textboxProxyURL.Enabled = false;
            this.textboxProxyURL.Location = new Point(6, 0x38);
            this.textboxProxyURL.Name = "textboxProxyURL";
            this.textboxProxyURL.Size = new Size(0x14c, 0x16);
            this.textboxProxyURL.TabIndex = 0x18;
            this.labelProxyURL.AutoSize = true;
            this.labelProxyURL.Location = new Point(3, 40);
            this.labelProxyURL.Name = "labelProxyURL";
            this.labelProxyURL.Size = new Size(0x30, 13);
            this.labelProxyURL.TabIndex = 0x17;
            this.labelProxyURL.Text = "Address";
            this.checkUseProxy.AutoSize = true;
            this.checkUseProxy.Location = new Point(6, 20);
            this.checkUseProxy.Name = "checkUseProxy";
            this.checkUseProxy.Size = new Size(0x4b, 0x11);
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
            this.buttonAddUL.Location = new Point(0x167, 0x20);
            this.buttonAddUL.Name = "buttonAddUL";
            this.buttonAddUL.Size = new Size(0x18, 0x18);
            this.buttonAddUL.TabIndex = 2;
            this.buttonAddUL.UseVisualStyleBackColor = true;
            this.buttonAddUL.Click += new EventHandler(this.buttonAddUL_Click);
            this.buttonRefreshULs.BackgroundImage = (Image) manager.GetObject("buttonRefreshULs.BackgroundImage");
            this.buttonRefreshULs.BackgroundImageLayout = ImageLayout.Center;
            this.buttonRefreshULs.FlatAppearance.BorderColor = SystemColors.ButtonShadow;
            this.buttonRefreshULs.FlatAppearance.MouseDownBackColor = SystemColors.MenuHighlight;
            this.buttonRefreshULs.FlatAppearance.MouseOverBackColor = SystemColors.ButtonHighlight;
            this.buttonRefreshULs.Location = new Point(0x181, 0x20);
            this.buttonRefreshULs.Name = "buttonRefreshULs";
            this.buttonRefreshULs.Size = new Size(0x18, 0x18);
            this.buttonRefreshULs.TabIndex = 3;
            this.buttonRefreshULs.UseVisualStyleBackColor = true;
            this.buttonRefreshULs.Click += new EventHandler(this.buttonRefreshULs_Click);
            this.buttonManageULs.BackgroundImage = (Image) manager.GetObject("buttonManageULs.BackgroundImage");
            this.buttonManageULs.BackgroundImageLayout = ImageLayout.Center;
            this.buttonManageULs.FlatAppearance.BorderColor = SystemColors.ButtonShadow;
            this.buttonManageULs.FlatAppearance.MouseDownBackColor = SystemColors.MenuHighlight;
            this.buttonManageULs.FlatAppearance.MouseOverBackColor = SystemColors.ButtonHighlight;
            this.buttonManageULs.Location = new Point(0x19b, 0x20);
            this.buttonManageULs.Name = "buttonManageULs";
            this.buttonManageULs.Size = new Size(0x18, 0x18);
            this.buttonManageULs.TabIndex = 4;
            this.buttonManageULs.UseVisualStyleBackColor = true;
            this.buttonManageULs.Click += new EventHandler(this.buttonManageULs_Click);
            this.updateTabControl.Font = new Font("Segoe UI", 8.25f);
            this.updateTabControl.Location = new Point(12, 0x7b);
            this.updateTabControl.Name = "updateTabControl";
            this.updateTabControl.SelectedIndex = 0;
            this.updateTabControl.Size = new Size(0x2ac, 0xda);
            this.updateTabControl.TabIndex = 10;
            this.labelToolTipText.Font = new Font("Segoe UI", 9f, FontStyle.Italic);
            this.labelToolTipText.Location = new Point(0x81, 350);
            this.labelToolTipText.Name = "labelToolTipText";
            this.labelToolTipText.Padding = new Padding(2);
            this.labelToolTipText.Size = new Size(0x1de, 0x22);
            this.labelToolTipText.TabIndex = 0x2e;
            this.labelToolTipText.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x2c4, 0x2e3);
            base.Controls.Add(this.labelToolTipText);
            base.Controls.Add(this.groupProxy);
            base.Controls.Add(this.buttonOptions);
            base.Controls.Add(this.groupOptions);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.labelWrittenBy);
            base.Controls.Add(this.buttonDownload);
            base.Controls.Add(this.buttonManageULs);
            base.Controls.Add(this.linkMSFN);
            base.Controls.Add(this.buttonRefreshULs);
            base.Controls.Add(this.buttonAddUL);
            base.Controls.Add(this.textboxPath);
            base.Controls.Add(this.groupDownloadProgress);
            base.Controls.Add(this.updateTabControl);
            base.Controls.Add(this.labelUpdateList);
            base.Controls.Add(this.buttonChangeFolder);
            base.Controls.Add(this.labelDownloadFolder);
            base.Controls.Add(this.comboUpdateLists);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
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

        private void linkMSFN_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("\"http://www.msfn.org/board/index.php?showforum=147\"");
        }

        private void loadDataGridViewsToQueue()
        {
            foreach (DataGridView view in this.updateDataGridViews)
            {
                if (view.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in (IEnumerable) view.Rows)
                    {
                        if ((bool) row.Cells[1].Value)
                        {
                            Program.DLManager.Queue.Add((int) row.Cells[0].Value);
                        }
                    }
                }
            }
        }

        private void LoadUpdateList()
        {
            Program.ULManager.Load(this.comboUpdateLists.SelectedIndex);
            this.ClearTabs();
            for (int i = 0; i < Program.ULManager.Categories.Count; i++)
            {
                this.updateTabPages.Add(new TabPage(Program.ULManager.Categories[i].Description));
                this.updateDataGridViews.Add(new DataGridView());
                this.updateDataGridViews[i].CellContentClick += new DataGridViewCellEventHandler(this.formMain_CellContentClick);
                this.updateDataGridViews[i].CellContentDoubleClick += new DataGridViewCellEventHandler(this.formMain_CellContentDoubleClick);
                this.updateDataGridViews[i].KeyPress += new KeyPressEventHandler(this.formMain_KeyPress);
                this.updateDataGridViews[i].CellMouseEnter += new DataGridViewCellEventHandler(this.formMain_CellMouseEnter);
                this.updateDataGridViews[i].CellMouseLeave += new DataGridViewCellEventHandler(this.formMain_CellMouseLeave);
                this.updateDataGridViews[i].Dock = DockStyle.Fill;
                this.updateDataGridViews[i].BorderStyle = BorderStyle.None;
                this.updateDataGridViews[i].RowHeadersVisible = false;
                this.updateDataGridViews[i].ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                this.updateDataGridViews[i].SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.updateDataGridViews[i].ScrollBars = ScrollBars.Vertical;
                this.updateDataGridViews[i].ReadOnly = true;
                this.updateDataGridViews[i].AllowUserToAddRows = false;
                this.updateDataGridViews[i].AllowUserToDeleteRows = false;
                this.updateDataGridViews[i].AllowUserToOrderColumns = false;
                this.updateDataGridViews[i].AllowUserToResizeRows = false;
                this.updateDataGridViews[i].AllowUserToResizeColumns = true;
                this.updateDataGridViews[i].BackgroundColor = SystemColors.AppWorkspace;
                this.updateDataGridViews[i].DefaultCellStyle.ForeColor = SystemColors.WindowText;
                this.updateDataGridViews[i].DefaultCellStyle.BackColor = SystemColors.Window;
                this.updateDataGridViews[i].Columns.Add(new DataGridViewTextBoxColumn());
                this.updateDataGridViews[i].Columns.Add(new DataGridViewCheckBoxColumn());
                this.updateDataGridViews[i].Columns.Add(new DataGridViewLinkColumn());
                this.updateDataGridViews[i].Columns.Add(new DataGridViewTextBoxColumn());
                this.updateDataGridViews[i].Columns[0].Visible = false;
                ((DataGridViewCheckBoxColumn) this.updateDataGridViews[i].Columns[1]).TrueValue = true;
                this.updateDataGridViews[i].Columns[1].Width = 0x17;
                this.updateDataGridViews[i].Columns[2].HeaderText = "Item";
                this.updateDataGridViews[i].Columns[2].Width = 560;
                this.updateDataGridViews[i].Columns[3].HeaderText = "Date";
                this.updateDataGridViews[i].Columns[3].Width = 0x4b;
                for (int j = 0; j < Program.ULManager.Updates.Count; j++)
                {
                    if (Program.ULManager.Updates[j].Category == Program.ULManager.Categories[i].ID)
                    {
                        DataGridViewRow dataGridViewRow = new DataGridViewRow {
                            Cells = { 
                                new DataGridViewTextBoxCell(),
                                new DataGridViewCheckBoxCell()
                            }
                        };
                        if (string.IsNullOrEmpty(Program.ULManager.Updates[j].Article))
                        {
                            dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell());
                        }
                        else
                        {
                            dataGridViewRow.Cells.Add(new DataGridViewLinkCell());
                        }
                        dataGridViewRow.Cells.Add(new DataGridViewTextBoxCell());
                        dataGridViewRow.Cells[0].Value = j;
                        dataGridViewRow.Cells[1].Value = true;
                        if (string.IsNullOrEmpty(Program.ULManager.Updates[j].Article))
                        {
                            dataGridViewRow.Cells[2].Value = Program.ULManager.Updates[j].Title;
                        }
                        else
                        {
                            ((DataGridViewLinkCell) dataGridViewRow.Cells[2]).LinkBehavior = LinkBehavior.HoverUnderline;
                            if ((this.updateDataGridViews[i].Rows.Count % 2) == 0)
                            {
                                ((DataGridViewLinkCell) dataGridViewRow.Cells[2]).LinkColor = SystemColors.WindowText;
                            }
                            else
                            {
                                ((DataGridViewLinkCell) dataGridViewRow.Cells[2]).LinkColor = SystemColors.InfoText;
                            }
                            ((DataGridViewLinkCell) dataGridViewRow.Cells[2]).VisitedLinkColor = ((DataGridViewLinkCell) dataGridViewRow.Cells[2]).LinkColor;
                            ((DataGridViewLinkCell) dataGridViewRow.Cells[2]).ActiveLinkColor = ((DataGridViewLinkCell) dataGridViewRow.Cells[2]).LinkColor;
                            dataGridViewRow.Cells[2].Value = Program.ULManager.Updates[j].Title;
                        }
                        dataGridViewRow.Cells[3].Value = Program.ULManager.Updates[j].PublishDate.ToString("yyyy-MM-dd");
                        this.updateDataGridViews[i].Rows.Add(dataGridViewRow);
                    }
                }
                TabPage local1 = this.updateTabPages[i];
                local1.Text = local1.Text + " (" + this.updateDataGridViews[i].Rows.Count.ToString() + ")";
                this.updateTabPages[i].Controls.Add(this.updateDataGridViews[i]);
                Panel panel = new Panel {
                    BackColor = SystemColors.ControlLightLight,
                    Dock = DockStyle.Top
                };
                LinkLabel label = new LinkLabel {
                    Text = "Check All",
                    AutoSize = true,
                    Top = 3,
                    Left = 0,
                    Tag = i
                };
                label.Click += new EventHandler(this.checkallLinkLabel_Click);
                panel.Controls.Add(label);
                LinkLabel label2 = new LinkLabel {
                    Text = "Uncheck All",
                    AutoSize = true,
                    Top = label.Top,
                    Left = (label.Left + label.Width) + 3,
                    Tag = i
                };
                label2.Click += new EventHandler(this.uncheckallLinkLabel_Click);
                panel.Controls.Add(label2);
                LinkLabel label3 = new LinkLabel {
                    Text = "Uncheck Installed",
                    AutoSize = true,
                    Top = label.Top,
                    Left = (label2.Left + label2.Width) + 3,
                    Tag = i
                };
                panel.Height = label.Height + 6;
                this.updateTabPages[i].Controls.Add(panel);
                this.updateTabControl.TabPages.Add(this.updateTabPages[i]);
            }
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
        }

        private void refreshULs()
        {
            Program.ULManager = new UpdateListManager(Path.GetDirectoryName(Program.SettingsPath));
            this.ClearTabs();
            this.comboUpdateLists.Items.Clear();
            for (int i = 0; i < Program.ULManager.Files.Count; i++)
            {
                this.comboUpdateLists.Items.Add(Program.ULManager.Files[i].Product + " " + Program.ULManager.Files[i].Platform + " " + Program.ULManager.Files[i].Language + " (" + Program.ULManager.Files[i].LastUpdate.ToString("yyyy-MM-dd") + ")");
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

        private void resizeForm()
        {
            this.updateTabControl.Width = base.Width - (this.updateTabControl.Left + 0x1c);
            switch (this.CurrentSize)
            {
                case FormSizes.Normal:
                    this.updateTabControl.Height = base.Height - (this.updateTabControl.Top + 90);
                    break;

                case FormSizes.NormalWithOptions:
                    this.updateTabControl.Height = base.Height - (this.updateTabControl.Top + 0x10c);
                    break;
            }
            this.buttonOptions.Top = (this.updateTabControl.Top + this.updateTabControl.Height) + 0x13;
            this.buttonDownload.Top = (this.updateTabControl.Top + this.updateTabControl.Height) + 0x13;
            this.buttonDownload.Left = (this.updateTabControl.Left + this.updateTabControl.Width) - 0x4b;
            this.groupOptions.Top = (this.buttonOptions.Top + this.buttonOptions.Height) + 0x10;
            this.groupProxy.Top = (this.groupOptions.Top + this.groupOptions.Height) + 6;
        }

        private void setFormSize(FormSizes size)
        {
            switch (size)
            {
                case FormSizes.Normal:
                    this.MinimumSize = new Size(0x2d4, 0x1af);
                    this.MaximumSize = new Size(0, 0);
                    break;

                case FormSizes.NormalWithOptions:
                    this.MinimumSize = new Size(0x2d4, 0x261);
                    this.MaximumSize = new Size(0, 0);
                    break;

                case FormSizes.Downloading:
                    this.MinimumSize = new Size(0x2d4, 230);
                    this.MaximumSize = new Size(0x2d4, 230);
                    break;
            }
            this.CurrentSize = size;
        }

        private void uncheckallLinkLabel_Click(object sender, EventArgs e)
        {
            int tag = (int) ((LinkLabel) sender).Tag;
            foreach (DataGridViewRow row in (IEnumerable) this.updateDataGridViews[tag].Rows)
            {
                ((DataGridViewCheckBoxCell) row.Cells[1]).Value = false;
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

        private enum FormSizes
        {
            Normal,
            NormalWithOptions,
            Downloading
        }
    }
}

