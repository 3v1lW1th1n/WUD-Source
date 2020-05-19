namespace WUD.SimpleInterprocessCommunications
{
    using System;

    public class DataReceivedEventArgs
    {
        private string channelName = "";
        private object data;
        private DateTime received;
        private DateTime sent;

        internal DataReceivedEventArgs(string channelName, object data, DateTime sent)
        {
            this.channelName = channelName;
            this.data = data;
            this.sent = sent;
            this.received = DateTime.Now;
        }

        public string ChannelName =>
            this.channelName;

        public object Data =>
            this.data;

        public DateTime Received =>
            this.received;

        public DateTime Sent =>
            this.sent;
    }
}

