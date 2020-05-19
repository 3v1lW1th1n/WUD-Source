namespace ICSharpCode.SharpZipLib.Zip
{
    using System;

    public class TestStatus
    {
        private long bytesTested_;
        private ZipEntry entry_;
        private bool entryValid_;
        private int errorCount_;
        private ZipFile file_;
        private TestOperation operation_;

        public TestStatus(ZipFile file)
        {
            this.file_ = file;
        }

        internal void AddError()
        {
            this.errorCount_++;
            this.entryValid_ = false;
        }

        internal void SetBytesTested(long value)
        {
            this.bytesTested_ = value;
        }

        internal void SetEntry(ZipEntry entry)
        {
            this.entry_ = entry;
            this.entryValid_ = true;
            this.bytesTested_ = 0L;
        }

        internal void SetOperation(TestOperation operation)
        {
            this.operation_ = operation;
        }

        public long BytesTested =>
            this.bytesTested_;

        public ZipEntry Entry =>
            this.entry_;

        public bool EntryValid =>
            this.entryValid_;

        public int ErrorCount =>
            this.errorCount_;

        public ZipFile File =>
            this.file_;

        public TestOperation Operation =>
            this.operation_;
    }
}

