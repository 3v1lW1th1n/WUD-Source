namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    using ICSharpCode.SharpZipLib;
    using System;

    public class DeflaterHuffman
    {
        private static readonly byte[] bit4Reverse = new byte[] { 0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15 };
        private const int BITLEN_NUM = 0x13;
        private static readonly int[] BL_ORDER = new int[] { 
            0x10, 0x11, 0x12, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2,
            14, 1, 15
        };
        private Tree blTree;
        private const int BUFSIZE = 0x4000;
        private short[] d_buf;
        private const int DIST_NUM = 30;
        private Tree distTree;
        private const int EOF_SYMBOL = 0x100;
        private int extra_bits;
        private byte[] l_buf;
        private int last_lit;
        private const int LITERAL_NUM = 0x11e;
        private Tree literalTree;
        public DeflaterPending pending;
        private const int REP_11_138 = 0x12;
        private const int REP_3_10 = 0x11;
        private const int REP_3_6 = 0x10;
        private static short[] staticDCodes;
        private static byte[] staticDLength;
        private static short[] staticLCodes = new short[0x11e];
        private static byte[] staticLLength = new byte[0x11e];

        static DeflaterHuffman()
        {
            int index = 0;
            while (index < 0x90)
            {
                staticLCodes[index] = BitReverse((0x30 + index) << 8);
                staticLLength[index++] = 8;
            }
            while (index < 0x100)
            {
                staticLCodes[index] = BitReverse((0x100 + index) << 7);
                staticLLength[index++] = 9;
            }
            while (index < 280)
            {
                staticLCodes[index] = BitReverse((-256 + index) << 9);
                staticLLength[index++] = 7;
            }
            while (index < 0x11e)
            {
                staticLCodes[index] = BitReverse((-88 + index) << 8);
                staticLLength[index++] = 8;
            }
            staticDCodes = new short[30];
            staticDLength = new byte[30];
            for (index = 0; index < 30; index++)
            {
                staticDCodes[index] = BitReverse(index << 11);
                staticDLength[index] = 5;
            }
        }

        public DeflaterHuffman(DeflaterPending pending)
        {
            this.pending = pending;
            this.literalTree = new Tree(this, 0x11e, 0x101, 15);
            this.distTree = new Tree(this, 30, 1, 15);
            this.blTree = new Tree(this, 0x13, 4, 7);
            this.d_buf = new short[0x4000];
            this.l_buf = new byte[0x4000];
        }

        public static short BitReverse(int toReverse) => 
            ((short) ((((bit4Reverse[toReverse & 15] << 12) | (bit4Reverse[(toReverse >> 4) & 15] << 8)) | (bit4Reverse[(toReverse >> 8) & 15] << 4)) | bit4Reverse[toReverse >> 12]));

        public void CompressBlock()
        {
            for (int i = 0; i < this.last_lit; i++)
            {
                int length = this.l_buf[i] & 0xff;
                int distance = this.d_buf[i];
                if (distance-- != 0)
                {
                    int code = Lcode(length);
                    this.literalTree.WriteSymbol(code);
                    int count = (code - 0x105) / 4;
                    if ((count > 0) && (count <= 5))
                    {
                        this.pending.WriteBits(length & ((((int) 1) << count) - 1), count);
                    }
                    int num6 = Dcode(distance);
                    this.distTree.WriteSymbol(num6);
                    count = (num6 / 2) - 1;
                    if (count > 0)
                    {
                        this.pending.WriteBits(distance & ((((int) 1) << count) - 1), count);
                    }
                }
                else
                {
                    this.literalTree.WriteSymbol(length);
                }
            }
            this.literalTree.WriteSymbol(0x100);
        }

        private static int Dcode(int distance)
        {
            int num = 0;
            while (distance >= 4)
            {
                num += 2;
                distance = distance >> 1;
            }
            return (num + distance);
        }

        public void FlushBlock(byte[] stored, int storedOffset, int storedLength, bool lastBlock)
        {
            this.literalTree.freqs[0x100] = (short) (this.literalTree.freqs[0x100] + 1);
            this.literalTree.BuildTree();
            this.distTree.BuildTree();
            this.literalTree.CalcBLFreq(this.blTree);
            this.distTree.CalcBLFreq(this.blTree);
            this.blTree.BuildTree();
            int blTreeCodes = 4;
            for (int i = 0x12; i > blTreeCodes; i--)
            {
                if (this.blTree.length[BL_ORDER[i]] > 0)
                {
                    blTreeCodes = i + 1;
                }
            }
            int num3 = ((((14 + (blTreeCodes * 3)) + this.blTree.GetEncodedLength()) + this.literalTree.GetEncodedLength()) + this.distTree.GetEncodedLength()) + this.extra_bits;
            int num4 = this.extra_bits;
            for (int j = 0; j < 0x11e; j++)
            {
                num4 += this.literalTree.freqs[j] * staticLLength[j];
            }
            for (int k = 0; k < 30; k++)
            {
                num4 += this.distTree.freqs[k] * staticDLength[k];
            }
            if (num3 >= num4)
            {
                num3 = num4;
            }
            if ((storedOffset >= 0) && ((storedLength + 4) < (num3 >> 3)))
            {
                this.FlushStoredBlock(stored, storedOffset, storedLength, lastBlock);
            }
            else if (num3 == num4)
            {
                this.pending.WriteBits(2 + (lastBlock ? 1 : 0), 3);
                this.literalTree.SetStaticCodes(staticLCodes, staticLLength);
                this.distTree.SetStaticCodes(staticDCodes, staticDLength);
                this.CompressBlock();
                this.Reset();
            }
            else
            {
                this.pending.WriteBits(4 + (lastBlock ? 1 : 0), 3);
                this.SendAllTrees(blTreeCodes);
                this.CompressBlock();
                this.Reset();
            }
        }

        public void FlushStoredBlock(byte[] stored, int storedOffset, int storedLength, bool lastBlock)
        {
            this.pending.WriteBits(lastBlock ? 1 : 0, 3);
            this.pending.AlignToByte();
            this.pending.WriteShort(storedLength);
            this.pending.WriteShort(~storedLength);
            this.pending.WriteBlock(stored, storedOffset, storedLength);
            this.Reset();
        }

        public bool IsFull() => 
            (this.last_lit >= 0x4000);

        private static int Lcode(int length)
        {
            if (length == 0xff)
            {
                return 0x11d;
            }
            int num = 0x101;
            while (length >= 8)
            {
                num += 4;
                length = length >> 1;
            }
            return (num + length);
        }

        public void Reset()
        {
            this.last_lit = 0;
            this.extra_bits = 0;
            this.literalTree.Reset();
            this.distTree.Reset();
            this.blTree.Reset();
        }

        public void SendAllTrees(int blTreeCodes)
        {
            this.blTree.BuildCodes();
            this.literalTree.BuildCodes();
            this.distTree.BuildCodes();
            this.pending.WriteBits(this.literalTree.numCodes - 0x101, 5);
            this.pending.WriteBits(this.distTree.numCodes - 1, 5);
            this.pending.WriteBits(blTreeCodes - 4, 4);
            for (int i = 0; i < blTreeCodes; i++)
            {
                this.pending.WriteBits(this.blTree.length[BL_ORDER[i]], 3);
            }
            this.literalTree.WriteTree(this.blTree);
            this.distTree.WriteTree(this.blTree);
        }

        public bool TallyDist(int distance, int length)
        {
            this.d_buf[this.last_lit] = (short) distance;
            this.l_buf[this.last_lit++] = (byte) (length - 3);
            int index = Lcode(length - 3);
            this.literalTree.freqs[index] = (short) (this.literalTree.freqs[index] + 1);
            if ((index >= 0x109) && (index < 0x11d))
            {
                this.extra_bits += (index - 0x105) / 4;
            }
            int num2 = Dcode(distance - 1);
            this.distTree.freqs[num2] = (short) (this.distTree.freqs[num2] + 1);
            if (num2 >= 4)
            {
                this.extra_bits += (num2 / 2) - 1;
            }
            return this.IsFull();
        }

        public bool TallyLit(int literal)
        {
            this.d_buf[this.last_lit] = 0;
            this.l_buf[this.last_lit++] = (byte) literal;
            this.literalTree.freqs[literal] = (short) (this.literalTree.freqs[literal] + 1);
            return this.IsFull();
        }

        private class Tree
        {
            private int[] bl_counts;
            private short[] codes;
            private DeflaterHuffman dh;
            public short[] freqs;
            public byte[] length;
            private int maxLength;
            public int minNumCodes;
            public int numCodes;

            public Tree(DeflaterHuffman dh, int elems, int minCodes, int maxLength)
            {
                this.dh = dh;
                this.minNumCodes = minCodes;
                this.maxLength = maxLength;
                this.freqs = new short[elems];
                this.bl_counts = new int[maxLength];
            }

            public void BuildCodes()
            {
                int length = this.freqs.Length;
                int[] numArray = new int[this.maxLength];
                int num = 0;
                this.codes = new short[this.freqs.Length];
                for (int i = 0; i < this.maxLength; i++)
                {
                    numArray[i] = num;
                    num += this.bl_counts[i] << (15 - i);
                }
                for (int j = 0; j < this.numCodes; j++)
                {
                    int num4 = this.length[j];
                    if (num4 > 0)
                    {
                        this.codes[j] = DeflaterHuffman.BitReverse(numArray[num4 - 1]);
                        numArray[num4 - 1] += ((int) 1) << (0x10 - num4);
                    }
                }
            }

            private void BuildLength(int[] childs)
            {
                this.length = new byte[this.freqs.Length];
                int num = childs.Length / 2;
                int num2 = (num + 1) / 2;
                int num3 = 0;
                for (int i = 0; i < this.maxLength; i++)
                {
                    this.bl_counts[i] = 0;
                }
                int[] numArray = new int[num];
                numArray[num - 1] = 0;
                for (int j = num - 1; j >= 0; j--)
                {
                    if (childs[(2 * j) + 1] != -1)
                    {
                        int maxLength = numArray[j] + 1;
                        if (maxLength > this.maxLength)
                        {
                            maxLength = this.maxLength;
                            num3++;
                        }
                        numArray[childs[2 * j]] = numArray[childs[(2 * j) + 1]] = maxLength;
                    }
                    else
                    {
                        int num7 = numArray[j];
                        this.bl_counts[num7 - 1]++;
                        this.length[childs[2 * j]] = (byte) numArray[j];
                    }
                }
                if (num3 != 0)
                {
                    int index = this.maxLength - 1;
                    do
                    {
                        while (this.bl_counts[--index] == 0)
                        {
                        }
                        do
                        {
                            this.bl_counts[index]--;
                            this.bl_counts[++index]++;
                            num3 -= ((int) 1) << ((this.maxLength - 1) - index);
                        }
                        while ((num3 > 0) && (index < (this.maxLength - 1)));
                    }
                    while (num3 > 0);
                    this.bl_counts[this.maxLength - 1] += num3;
                    this.bl_counts[this.maxLength - 2] -= num3;
                    int num9 = 2 * num2;
                    for (int k = this.maxLength; k != 0; k--)
                    {
                        int num11 = this.bl_counts[k - 1];
                        while (num11 > 0)
                        {
                            int num12 = 2 * childs[num9++];
                            if (childs[num12 + 1] == -1)
                            {
                                this.length[childs[num12]] = (byte) k;
                                num11--;
                            }
                        }
                    }
                }
            }

            public void BuildTree()
            {
                int length = this.freqs.Length;
                int[] numArray = new int[length];
                int num2 = 0;
                int num3 = 0;
                for (int i = 0; i < length; i++)
                {
                    int num5 = this.freqs[i];
                    if (num5 != 0)
                    {
                        int num7;
                        int index = num2++;
                        while ((index > 0) && (this.freqs[numArray[num7 = (index - 1) / 2]] > num5))
                        {
                            numArray[index] = numArray[num7];
                            index = num7;
                        }
                        numArray[index] = i;
                        num3 = i;
                    }
                }
                while (num2 < 2)
                {
                    int num8 = (num3 < 2) ? ++num3 : 0;
                    numArray[num2++] = num8;
                }
                this.numCodes = Math.Max(num3 + 1, this.minNumCodes);
                int num9 = num2;
                int[] childs = new int[(4 * num2) - 2];
                int[] numArray3 = new int[(2 * num2) - 1];
                int num10 = num9;
                for (int j = 0; j < num2; j++)
                {
                    int num12 = numArray[j];
                    childs[2 * j] = num12;
                    childs[(2 * j) + 1] = -1;
                    numArray3[j] = this.freqs[num12] << 8;
                    numArray[j] = j;
                }
                do
                {
                    int num13 = numArray[0];
                    int num14 = numArray[--num2];
                    int num15 = 0;
                    int num16 = 1;
                    while (num16 < num2)
                    {
                        if (((num16 + 1) < num2) && (numArray3[numArray[num16]] > numArray3[numArray[num16 + 1]]))
                        {
                            num16++;
                        }
                        numArray[num15] = numArray[num16];
                        num15 = num16;
                        num16 = (num16 * 2) + 1;
                    }
                    int num17 = numArray3[num14];
                    while (((num16 = num15) > 0) && (numArray3[numArray[num15 = (num16 - 1) / 2]] > num17))
                    {
                        numArray[num16] = numArray[num15];
                    }
                    numArray[num16] = num14;
                    int num18 = numArray[0];
                    num14 = num10++;
                    childs[2 * num14] = num13;
                    childs[(2 * num14) + 1] = num18;
                    int num19 = Math.Min((int) (numArray3[num13] & 0xff), (int) (numArray3[num18] & 0xff));
                    numArray3[num14] = num17 = ((numArray3[num13] + numArray3[num18]) - num19) + 1;
                    num15 = 0;
                    num16 = 1;
                    while (num16 < num2)
                    {
                        if (((num16 + 1) < num2) && (numArray3[numArray[num16]] > numArray3[numArray[num16 + 1]]))
                        {
                            num16++;
                        }
                        numArray[num15] = numArray[num16];
                        num15 = num16;
                        num16 = (num15 * 2) + 1;
                    }
                    while (((num16 = num15) > 0) && (numArray3[numArray[num15 = (num16 - 1) / 2]] > num17))
                    {
                        numArray[num16] = numArray[num15];
                    }
                    numArray[num16] = num14;
                }
                while (num2 > 1);
                if (numArray[0] != ((childs.Length / 2) - 1))
                {
                    throw new SharpZipBaseException("Heap invariant violated");
                }
                this.BuildLength(childs);
            }

            public void CalcBLFreq(DeflaterHuffman.Tree blTree)
            {
                int index = -1;
                int num5 = 0;
                while (num5 < this.numCodes)
                {
                    int num;
                    int num2;
                    int num3 = 1;
                    int num6 = this.length[num5];
                    if (num6 == 0)
                    {
                        num = 0x8a;
                        num2 = 3;
                    }
                    else
                    {
                        num = 6;
                        num2 = 3;
                        if (index != num6)
                        {
                            blTree.freqs[num6] = (short) (blTree.freqs[num6] + 1);
                            num3 = 0;
                        }
                    }
                    index = num6;
                    num5++;
                    while ((num5 < this.numCodes) && (index == this.length[num5]))
                    {
                        num5++;
                        if (++num3 >= num)
                        {
                            break;
                        }
                    }
                    if (num3 < num2)
                    {
                        blTree.freqs[index] = (short) (blTree.freqs[index] + ((short) num3));
                    }
                    else
                    {
                        if (index != 0)
                        {
                            blTree.freqs[0x10] = (short) (blTree.freqs[0x10] + 1);
                            continue;
                        }
                        if (num3 <= 10)
                        {
                            blTree.freqs[0x11] = (short) (blTree.freqs[0x11] + 1);
                            continue;
                        }
                        blTree.freqs[0x12] = (short) (blTree.freqs[0x12] + 1);
                    }
                }
            }

            public void CheckEmpty()
            {
                bool flag = true;
                for (int i = 0; i < this.freqs.Length; i++)
                {
                    if (this.freqs[i] != 0)
                    {
                        flag = false;
                    }
                }
                if (!flag)
                {
                    throw new SharpZipBaseException("!Empty");
                }
            }

            public int GetEncodedLength()
            {
                int num = 0;
                for (int i = 0; i < this.freqs.Length; i++)
                {
                    num += this.freqs[i] * this.length[i];
                }
                return num;
            }

            public void Reset()
            {
                for (int i = 0; i < this.freqs.Length; i++)
                {
                    this.freqs[i] = 0;
                }
                this.codes = null;
                this.length = null;
            }

            public void SetStaticCodes(short[] staticCodes, byte[] staticLengths)
            {
                this.codes = staticCodes;
                this.length = staticLengths;
            }

            public void WriteSymbol(int code)
            {
                this.dh.pending.WriteBits(this.codes[code] & 0xffff, this.length[code]);
            }

            public void WriteTree(DeflaterHuffman.Tree blTree)
            {
                int code = -1;
                int index = 0;
                while (index < this.numCodes)
                {
                    int num;
                    int num2;
                    int num3 = 1;
                    int num6 = this.length[index];
                    if (num6 == 0)
                    {
                        num = 0x8a;
                        num2 = 3;
                    }
                    else
                    {
                        num = 6;
                        num2 = 3;
                        if (code != num6)
                        {
                            blTree.WriteSymbol(num6);
                            num3 = 0;
                        }
                    }
                    code = num6;
                    index++;
                    while ((index < this.numCodes) && (code == this.length[index]))
                    {
                        index++;
                        if (++num3 >= num)
                        {
                            break;
                        }
                    }
                    if (num3 < num2)
                    {
                        while (num3-- > 0)
                        {
                            blTree.WriteSymbol(code);
                        }
                    }
                    else if (code != 0)
                    {
                        blTree.WriteSymbol(0x10);
                        this.dh.pending.WriteBits(num3 - 3, 2);
                    }
                    else
                    {
                        if (num3 <= 10)
                        {
                            blTree.WriteSymbol(0x11);
                            this.dh.pending.WriteBits(num3 - 3, 3);
                            continue;
                        }
                        blTree.WriteSymbol(0x12);
                        this.dh.pending.WriteBits(num3 - 11, 7);
                    }
                }
            }
        }
    }
}

