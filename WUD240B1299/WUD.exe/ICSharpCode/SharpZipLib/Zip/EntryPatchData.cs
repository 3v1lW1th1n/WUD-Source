namespace ICSharpCode.SharpZipLib.Zip
{
    using System;

    internal class EntryPatchData
    {
        private long crcPatchOffset_;
        private long sizePatchOffset_;

        public long CrcPatchOffset
        {
            get => 
                this.crcPatchOffset_;
            set
            {
                this.crcPatchOffset_ = value;
            }
        }

        public long SizePatchOffset
        {
            get => 
                this.sizePatchOffset_;
            set
            {
                this.sizePatchOffset_ = value;
            }
        }
    }
}

