namespace WUD.SimpleInterprocessCommunications
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    public class CopyData : NativeWindow, IDisposable
    {
        private CopyDataChannels channels;
        private bool disposed;
        private const int WM_COPYDATA = 0x4a;
        private const int WM_DESTROY = 2;

        public event DataReceivedEventHandler DataReceived;

        public CopyData()
        {
            this.channels = new CopyDataChannels(this);
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.channels.Clear();
                this.channels = null;
                this.disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~CopyData()
        {
            this.Dispose();
        }

        protected void OnDataReceived(DataReceivedEventArgs e)
        {
            this.DataReceived(this, e);
        }

        protected override void OnHandleChange()
        {
            this.channels.OnHandleChange();
            base.OnHandleChange();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x4a)
            {
                COPYDATASTRUCT copydatastruct = new COPYDATASTRUCT();
                copydatastruct = (COPYDATASTRUCT) Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));
                if (copydatastruct.cbData > 0)
                {
                    byte[] destination = new byte[copydatastruct.cbData];
                    Marshal.Copy(copydatastruct.lpData, destination, 0, copydatastruct.cbData);
                    MemoryStream serializationStream = new MemoryStream(destination);
                    BinaryFormatter formatter = new BinaryFormatter();
                    CopyDataObjectData data = (CopyDataObjectData) formatter.Deserialize(serializationStream);
                    if (this.channels.Contains(data.Channel))
                    {
                        DataReceivedEventArgs e = new DataReceivedEventArgs(data.Channel, data.Data, data.Sent);
                        this.OnDataReceived(e);
                        m.Result = (IntPtr) 1;
                    }
                }
            }
            else if (m.Msg == 2)
            {
                this.channels.OnHandleChange();
                base.OnHandleChange();
            }
            base.WndProc(ref m);
        }

        public CopyDataChannels Channels =>
            this.channels;

        [StructLayout(LayoutKind.Sequential)]
        private struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }
    }
}

