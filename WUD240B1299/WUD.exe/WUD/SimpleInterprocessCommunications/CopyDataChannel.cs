namespace WUD.SimpleInterprocessCommunications
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    public class CopyDataChannel : IDisposable
    {
        private string channelName = "";
        private bool disposed;
        private NativeWindow owner;
        private bool recreateChannel;
        private const int WM_COPYDATA = 0x4a;

        internal CopyDataChannel(NativeWindow owner, string channelName)
        {
            this.owner = owner;
            this.channelName = channelName;
            this.addChannel();
        }

        private void addChannel()
        {
            SetProp(this.owner.Handle, this.channelName, (int) this.owner.Handle);
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                if (this.channelName.Length > 0)
                {
                    this.removeChannel();
                }
                this.channelName = "";
                this.disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~CopyDataChannel()
        {
            this.Dispose();
        }

        [DllImport("user32", CharSet=CharSet.Auto)]
        private static extern int GetProp(IntPtr hwnd, string lpString);
        public void OnHandleChange()
        {
            this.removeChannel();
            this.recreateChannel = true;
        }

        private void removeChannel()
        {
            RemoveProp(this.owner.Handle, this.channelName);
        }

        [DllImport("user32", CharSet=CharSet.Auto)]
        private static extern int RemoveProp(IntPtr hwnd, string lpString);
        public int Send(object obj)
        {
            int num = 0;
            if (this.disposed)
            {
                throw new InvalidOperationException("Object has been disposed");
            }
            if (this.recreateChannel)
            {
                this.addChannel();
            }
            CopyDataObjectData graph = new CopyDataObjectData(obj, this.channelName);
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream serializationStream = new MemoryStream();
            formatter.Serialize(serializationStream, graph);
            serializationStream.Flush();
            int length = (int) serializationStream.Length;
            if (length > 0)
            {
                byte[] buffer = new byte[length];
                serializationStream.Seek(0L, SeekOrigin.Begin);
                serializationStream.Read(buffer, 0, length);
                IntPtr destination = Marshal.AllocCoTaskMem(length);
                Marshal.Copy(buffer, 0, destination, length);
                EnumWindows windows = new EnumWindows();
                windows.GetWindows();
                foreach (EnumWindowsItem item in windows.Items)
                {
                    if (!item.Handle.Equals(this.owner.Handle) && (GetProp(item.Handle, this.channelName) != 0))
                    {
                        COPYDATASTRUCT lParam = new COPYDATASTRUCT {
                            cbData = length,
                            dwData = IntPtr.Zero,
                            lpData = destination
                        };
                        SendMessage(item.Handle, 0x4a, (int) this.owner.Handle, ref lParam);
                        num += (Marshal.GetLastWin32Error() == 0) ? 1 : 0;
                    }
                }
                Marshal.FreeCoTaskMem(destination);
            }
            serializationStream.Close();
            return num;
        }

        [DllImport("user32", CharSet=CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, ref COPYDATASTRUCT lParam);
        [DllImport("user32", CharSet=CharSet.Auto)]
        private static extern int SetProp(IntPtr hwnd, string lpString, int hData);

        public string ChannelName =>
            this.channelName;

        [StructLayout(LayoutKind.Sequential)]
        private struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }
    }
}

