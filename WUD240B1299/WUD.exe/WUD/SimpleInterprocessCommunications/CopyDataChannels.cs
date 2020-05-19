namespace WUD.SimpleInterprocessCommunications
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    public class CopyDataChannels : DictionaryBase
    {
        private NativeWindow owner;

        internal CopyDataChannels(NativeWindow owner)
        {
            this.owner = owner;
        }

        public void Add(string channelName)
        {
            CopyDataChannel channel = new CopyDataChannel(this.owner, channelName);
            base.Dictionary.Add(channelName, channel);
        }

        public bool Contains(string channelName) => 
            base.Dictionary.Contains(channelName);

        public IEnumerator GetEnumerator() => 
            base.Dictionary.Values.GetEnumerator();

        protected override void OnClear()
        {
            foreach (CopyDataChannel channel in base.Dictionary.Values)
            {
                channel.Dispose();
            }
            base.OnClear();
        }

        public void OnHandleChange()
        {
            foreach (CopyDataChannel channel in base.Dictionary.Values)
            {
                channel.OnHandleChange();
            }
        }

        protected override void OnRemoveComplete(object key, object data)
        {
            ((CopyDataChannel) data).Dispose();
            base.OnRemove(key, data);
        }

        public void Remove(string channelName)
        {
            base.Dictionary.Remove(channelName);
        }

        public CopyDataChannel this[int index]
        {
            get
            {
                int num = 0;
                foreach (CopyDataChannel channel2 in base.Dictionary.Values)
                {
                    num++;
                    if (num == index)
                    {
                        return channel2;
                    }
                }
                return null;
            }
        }

        public CopyDataChannel this[string channelName] =>
            ((CopyDataChannel) base.Dictionary[channelName]);
    }
}

