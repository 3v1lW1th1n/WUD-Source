namespace WUD
{
    using Supremus.Configuration;
    using Supremus.Encryption;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Windows.Forms;
    using WUD.SimpleInterprocessCommunications;

    internal static class Program
    {
        public static DownloadManager DLManager;
        public static string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Supremus Corporation\Windows Updates Downloader\WUD.settings");
        public static UpdateListManager ULManager;
        public static string UserDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Windows Updates Downloader");

        [STAThread]
        private static void Main(string[] args)
        {
            if (!Directory.Exists(Path.GetDirectoryName(SettingsPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
            }
            Environment.ExitCode = 0;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if ((args.Length == 4) && (args[0] == "-automated"))
            {
                string path = args[1].ToString();
                string str2 = args[2].ToString();
                bool flag = args[3].ToString().ToUpper().IndexOf("C") >= 0;
                bool flag2 = args[3].ToString().ToUpper().IndexOf("S") >= 0;
                if (!System.IO.File.Exists(path))
                {
                    Environment.ExitCode = 11;
                    Application.Exit();
                }
                else if (!Directory.Exists(str2))
                {
                    Environment.ExitCode = 12;
                    Application.Exit();
                }
                else if (!flag && !flag2)
                {
                    Environment.ExitCode = 13;
                    Application.Exit();
                }
                else
                {
                    ULManager = new UpdateListManager();
                    DLManager = new DownloadManager();
                    ULManager.Load(path);
                    if (flag)
                    {
                        for (int i = 0; i < ULManager.Updates.Count; i++)
                        {
                            if (ULManager.Updates[i].Category == 1)
                            {
                                DLManager.Queue.Add(i);
                            }
                        }
                    }
                    if (flag2)
                    {
                        for (int j = 0; j < ULManager.Updates.Count; j++)
                        {
                            if (ULManager.Updates[j].Category == 2)
                            {
                                DLManager.Queue.Add(j);
                            }
                        }
                    }
                    foreach (string str3 in Directory.GetFiles(str2))
                    {
                        bool flag3 = false;
                        for (int k = 0; k < ULManager.Updates.Count; k++)
                        {
                            if (ULManager.Updates[k].Filename.ToUpper() == Path.GetFileName(str3).ToUpper())
                            {
                                flag3 = true;
                                int? nullable = null;
                                for (int m = 0; m < DLManager.Queue.Count; m++)
                                {
                                    if (DLManager.Queue[m] == k)
                                    {
                                        nullable = new int?(m);
                                    }
                                }
                                if (nullable.HasValue)
                                {
                                    DLManager.Queue.RemoveAt(nullable.Value);
                                }
                            }
                        }
                        if (!flag3)
                        {
                            System.IO.File.Delete(str3);
                        }
                    }
                    DLManager.DownloadPath = str2;
                    DLManager.UseSubFolders = false;
                    Settings settings = new Settings(SettingsPath, "Settings");
                    if (settings.ReadBool("Proxy", "Enabled", false))
                    {
                        try
                        {
                            DLManager.Proxy = new WebProxy(settings.ReadString("Proxy", "Address", ""), settings.ReadInt("Proxy", "Port", 0x1f90));
                        }
                        catch (FormatException)
                        {
                            Environment.ExitCode = 14;
                            Application.Exit();
                            return;
                        }
                        if (settings.ReadBool("Proxy", "Authentication", false))
                        {
                            DLManager.Proxy.Credentials = new NetworkCredential(settings.ReadString("Proxy", "Username", ""), Supremus.Encryption.Encryption.Decrypt(settings.ReadString("Proxy", "Password", "")));
                        }
                    }
                    settings = null;
                    formMain mainForm = new formMain {
                        mode = OperatingMode.Automated
                    };
                    Application.Run(mainForm);
                    mainForm.Dispose();
                }
            }
            else if ((args.Length > 0) && (args[0] == "-install"))
            {
                UpdateListManager.InstallCompressedUL(args[1], Path.GetDirectoryName(SettingsPath));
                CopyData data = new CopyData();
                data.AssignHandle(Process.GetCurrentProcess().Handle);
                data.Channels.Add("WUD");
                data.Channels["WUD"].Send("Refresh");
                MessageBox.Show("Compressed UL file installed.", "Windows Updates Downloader", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ULManager = new UpdateListManager(Path.GetDirectoryName(SettingsPath));
                DLManager = new DownloadManager();
                formMain main2 = new formMain {
                    mode = OperatingMode.User
                };
                Application.Run(main2);
                main2.Dispose();
            }
        }
    }
}

