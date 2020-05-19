namespace WMP11Integrator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public class formMain : Form
    {
        private Button buttonChangeDestination;
        private Button buttonChangeSource;
        private Button buttonIntegrate;
        private IContainer components;
        private FolderBrowserDialog folderBrowser;
        private Label labelDestination;
        private Label labelSource;
        private Label labelStatus;
        private OpenFileDialog openFile;
        private PictureBox pictureBar;
        private ProgressBar progressStatus;
        public string tempFolder;
        private TextBox textDestination;
        private TextBox textSource;

        public formMain()
        {
            this.InitializeComponent();
        }

        private void buttonChangeDestination_Click(object sender, EventArgs e)
        {
            if (this.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                this.textDestination.Text = this.folderBrowser.SelectedPath;
            }
            if ((this.textSource.Text.Length > 0) && (this.textDestination.Text.Length > 0))
            {
                this.buttonIntegrate.Enabled = true;
            }
        }

        private void buttonChangeSource_Click(object sender, EventArgs e)
        {
            if (this.openFile.ShowDialog() == DialogResult.OK)
            {
                this.textSource.Text = this.openFile.FileName;
            }
            if ((this.textSource.Text.Length > 0) && (this.textDestination.Text.Length > 0))
            {
                this.buttonIntegrate.Enabled = true;
            }
        }

        private void buttonIntegrate_Click(object sender, EventArgs e)
        {
            this.progressStatus.Value = 0;
            this.progressStatus.Maximum = 10;
            this.labelStatus.Text = "Extracting setup package...";
            this.Refresh();
            Application.DoEvents();
            if (Directory.Exists(this.tempFolder))
            {
                Directory.Delete(this.tempFolder, true);
            }
            this.startAndWait(this.textSource.Text, "/Q /C /T:\"" + this.tempFolder + "\"");
            this.progressStatus.Value++;
            this.labelStatus.Text = "Integrating user mode driver framework...";
            this.Refresh();
            Application.DoEvents();
            this.startAndWait(this.tempFolder + @"\umdf.exe", "/PASSIVE /NORESTART /INTEGRATE:\"" + this.textDestination.Text + "\"");
            this.progressStatus.Value++;
            this.labelStatus.Text = "Integrating microsoft compression client pack...";
            this.Refresh();
            Application.DoEvents();
            this.startAndWait(this.tempFolder + @"\WindowsXP-MSCompPackV1-x86.exe", "/PASSIVE /NORESTART /INTEGRATE:\"" + this.textDestination.Text + "\"");
            this.progressStatus.Value++;
            this.labelStatus.Text = "Integrating windows media format runtime...";
            this.Refresh();
            Application.DoEvents();
            this.startAndWait(this.tempFolder + @"\wmfdist11.exe", "/PASSIVE /NORESTART /INTEGRATE:\"" + this.textDestination.Text + "\"");
            this.progressStatus.Value++;
            this.labelStatus.Text = "Integrating windows media player 10 compatibility shim...";
            this.Refresh();
            Application.DoEvents();
            this.startAndWait(this.tempFolder + @"\wmpappcompat.exe", "/PASSIVE /NORESTART /INTEGRATE:\"" + this.textDestination.Text + "\"");
            this.progressStatus.Value++;
            this.labelStatus.Text = "Integrating windows media player 11...";
            this.Refresh();
            Application.DoEvents();
            this.startAndWait(this.tempFolder + @"\wmp11.exe", "/PASSIVE /NORESTART /INTEGRATE:\"" + this.textDestination.Text + "\"");
            this.progressStatus.Value++;
            this.labelStatus.Text = "Moving files around...";
            this.Refresh();
            Application.DoEvents();
            if (File.Exists(this.textDestination.Text + @"\I386\i386\msdelta.dll"))
            {
                File.Move(this.textDestination.Text + @"\I386\i386\msdelta.dll", this.textDestination.Text + @"\I386\msdelta.dll");
            }
            if (Directory.Exists(this.textDestination.Text + @"\I386\i386"))
            {
                Directory.Delete(this.textDestination.Text + @"\I386\i386");
            }
            this.progressStatus.Value++;
            this.labelStatus.Text = "Modifying DOSNET.INF...";
            this.Refresh();
            Application.DoEvents();
            List<string> list = new List<string>();
            TextReader reader = new StreamReader(this.textDestination.Text + @"\I386\DOSNET.INF");
            string item = null;
            while ((item = reader.ReadLine()) != null)
            {
                if (item.IndexOf(@"I386\i386\msdelta.dll") > 0)
                {
                    list.Add(item.Replace(@"I386\i386\msdelta.dll", @"I386\msdelta.dll"));
                }
                else
                {
                    list.Add(item);
                }
            }
            reader.Close();
            reader.Dispose();
            reader = null;
            TextWriter writer = new StreamWriter(this.textDestination.Text + @"\I386\DOSNET.INF", false);
            for (int i = 0; i < list.Count; i++)
            {
                writer.WriteLine(list[i]);
            }
            writer.Close();
            writer.Dispose();
            writer = null;
            this.progressStatus.Value++;
            this.labelStatus.Text = "Modifying TXTSETUP.SIF...";
            this.Refresh();
            Application.DoEvents();
            List<string> list2 = new List<string>();
            reader = new StreamReader(this.textDestination.Text + @"\I386\TXTSETUP.SIF");
            item = null;
            while ((item = reader.ReadLine()) != null)
            {
                list2.Add(item);
            }
            reader.Close();
            reader.Dispose();
            reader = null;
            writer = new StreamWriter(this.textDestination.Text + @"\I386\TXTSETUP.SIF", false);
            for (int j = 0; j < (list2.Count - 1); j++)
            {
                writer.WriteLine(list2[j]);
            }
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("[SourceDisksFiles]");
            writer.WriteLine("wmdrmsdk.dll = 100,,,,,,,2,0,0");
            writer.WriteLine("mfplat.dll   = 100,,,,,,,2,0,0");
            writer.Close();
            writer.Dispose();
            writer = null;
            this.progressStatus.Value++;
            this.labelStatus.Text = "Integration complete.";
            this.Refresh();
            Application.DoEvents();
            if (Directory.Exists(this.tempFolder))
            {
                Directory.Delete(this.tempFolder, true);
            }
            this.progressStatus.Value++;
            this.Refresh();
            Application.DoEvents();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            string text = this.Text;
            this.Text = text + " " + versionInfo.ProductMajorPart.ToString() + "." + versionInfo.ProductMinorPart.ToString() + " Build " + versionInfo.ProductBuildPart.ToString();
            this.tempFolder = Path.GetTempPath() + @"\WMP11FILES";
            this.tempFolder = this.tempFolder.Replace(@"\\", @"\");
            base.Left = (Screen.PrimaryScreen.WorkingArea.X + Screen.PrimaryScreen.WorkingArea.Width) - (base.Width + 20);
            base.Top = (Screen.PrimaryScreen.WorkingArea.Y + Screen.PrimaryScreen.WorkingArea.Height) - (base.Height + 20);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(formMain));
            this.pictureBar = new PictureBox();
            this.labelSource = new Label();
            this.textSource = new TextBox();
            this.labelDestination = new Label();
            this.textDestination = new TextBox();
            this.buttonChangeSource = new Button();
            this.buttonChangeDestination = new Button();
            this.buttonIntegrate = new Button();
            this.progressStatus = new ProgressBar();
            this.labelStatus = new Label();
            this.folderBrowser = new FolderBrowserDialog();
            this.openFile = new OpenFileDialog();
            ((ISupportInitialize) this.pictureBar).BeginInit();
            base.SuspendLayout();
            this.pictureBar.Image = (Image) manager.GetObject("pictureBar.Image");
            this.pictureBar.Location = new Point(0, 0xad);
            this.pictureBar.Name = "pictureBar";
            this.pictureBar.Size = new Size(500, 0x45);
            this.pictureBar.TabIndex = 0;
            this.pictureBar.TabStop = false;
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new Point(12, 9);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new Size(0x83, 14);
            this.labelSource.TabIndex = 1;
            this.labelSource.Text = "WMP11 Package Location";
            this.textSource.BackColor = SystemColors.Info;
            this.textSource.Location = new Point(12, 0x1a);
            this.textSource.Name = "textSource";
            this.textSource.ReadOnly = true;
            this.textSource.Size = new Size(0x18a, 20);
            this.textSource.TabIndex = 2;
            this.textSource.TabStop = false;
            this.labelDestination.AutoSize = true;
            this.labelDestination.Location = new Point(12, 0x4b);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new Size(0x87, 14);
            this.labelDestination.TabIndex = 3;
            this.labelDestination.Text = "Windows Source Location";
            this.textDestination.BackColor = SystemColors.Info;
            this.textDestination.Location = new Point(12, 0x5c);
            this.textDestination.Name = "textDestination";
            this.textDestination.ReadOnly = true;
            this.textDestination.Size = new Size(0x18a, 20);
            this.textDestination.TabIndex = 5;
            this.textDestination.TabStop = false;
            this.buttonChangeSource.Location = new Point(0x19c, 0x19);
            this.buttonChangeSource.Name = "buttonChangeSource";
            this.buttonChangeSource.Size = new Size(0x4b, 0x17);
            this.buttonChangeSource.TabIndex = 6;
            this.buttonChangeSource.Text = "Browse...";
            this.buttonChangeSource.UseVisualStyleBackColor = true;
            this.buttonChangeSource.Click += new EventHandler(this.buttonChangeSource_Click);
            this.buttonChangeDestination.Location = new Point(0x19c, 0x5b);
            this.buttonChangeDestination.Name = "buttonChangeDestination";
            this.buttonChangeDestination.Size = new Size(0x4b, 0x17);
            this.buttonChangeDestination.TabIndex = 7;
            this.buttonChangeDestination.Text = "Browse...";
            this.buttonChangeDestination.UseVisualStyleBackColor = true;
            this.buttonChangeDestination.Click += new EventHandler(this.buttonChangeDestination_Click);
            this.buttonIntegrate.Enabled = false;
            this.buttonIntegrate.Location = new Point(0x19c, 0x90);
            this.buttonIntegrate.Name = "buttonIntegrate";
            this.buttonIntegrate.Size = new Size(0x4b, 0x17);
            this.buttonIntegrate.TabIndex = 8;
            this.buttonIntegrate.Text = "Integrate";
            this.buttonIntegrate.UseVisualStyleBackColor = true;
            this.buttonIntegrate.Click += new EventHandler(this.buttonIntegrate_Click);
            this.progressStatus.Location = new Point(12, 0x90);
            this.progressStatus.Name = "progressStatus";
            this.progressStatus.Size = new Size(0x18a, 10);
            this.progressStatus.TabIndex = 9;
            this.labelStatus.Font = new Font("Arial", 7f);
            this.labelStatus.Location = new Point(12, 0x9d);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new Size(0x18a, 13);
            this.labelStatus.TabIndex = 10;
            this.folderBrowser.Description = "Locate Windows Source";
            this.folderBrowser.ShowNewFolderButton = false;
            this.openFile.DefaultExt = "exe";
            this.openFile.Filter = "Executable File (*.exe)|*.exe";
            this.openFile.Title = "Locate WMP11 Package";
            base.AutoScaleDimensions = new SizeF(6f, 14f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x1f3, 0xf1);
            base.Controls.Add(this.labelStatus);
            base.Controls.Add(this.progressStatus);
            base.Controls.Add(this.buttonIntegrate);
            base.Controls.Add(this.buttonChangeDestination);
            base.Controls.Add(this.buttonChangeSource);
            base.Controls.Add(this.textDestination);
            base.Controls.Add(this.labelDestination);
            base.Controls.Add(this.textSource);
            base.Controls.Add(this.labelSource);
            base.Controls.Add(this.pictureBar);
            this.Font = new Font("Arial", 8.25f);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "formMain";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "WMP11 Integrator";
            base.Load += new EventHandler(this.formMain_Load);
            ((ISupportInitialize) this.pictureBar).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void start(string filename, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = filename,
                Arguments = arguments,
                WorkingDirectory = this.tempFolder
            };
            Process.Start(startInfo).WaitForExit();
        }

        private void startAndWait(string filename, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = filename,
                Arguments = arguments,
                WorkingDirectory = this.tempFolder
            };
            Process process = Process.Start(startInfo);
            process.WaitForInputIdle();
            process.WaitForExit();
        }
    }
}

