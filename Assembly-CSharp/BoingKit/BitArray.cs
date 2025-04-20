using System;

namespace BoingKit
{
	// Token: 0x02000D18 RID: 3352
	public struct BitArray
	{
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060054A1 RID: 21665 RVA: 0x00066F9F File Offset: 0x0006519F
		public int[] Blocks
		{
			get
			{
				return this.m_aBlock;
			}
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x00066FA7 File Offset: 0x000651A7
		private static int GetBlockIndex(int index)
		{
			return index / 4;
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x00066FAC File Offset: 0x000651AC
		private static int GetSubIndex(int index)
		{
			return index % 4;
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x001CF708 File Offset: 0x001CD908
		private static void SetBit(int index, bool value, int[] blocks)
		{
			int blockIndex = BitArray.GetBlockIndex(index);
			int subIndex = BitArray.GetSubIndex(index);
			if (value)
			{
				blocks[blockIndex] |= 1 << subIndex;
				return;
			}
			blocks[blockIndex] &= ~(1 << subIndex);
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x00066FB1 File Offset: 0x000651B1
		private static bool IsBitSet(int index, int[] blocks)
		{
			return (blocks[BitArray.GetBlockIndex(index)] & 1 << BitArray.GetSubIndex(index)) != 0;
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x001CF74C File Offset: 0x001CD94C
		public BitArray(int capacity)
		{
			int num = (capacity + 4 - 1) / 4;
			this.m_aBlock = new int[num];
			this.Clear();
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x001CF774 File Offset: 0x001CD974
		public void Resize(int capacity)
		{
			int num = (capacity + 4 - 1) / 4;
			if (num <= this.m_aBlock.Length)
			{
				return;
			}
			int[] array = new int[num];
			int i = 0;
			int num2 = this.m_aBlock.Length;
			while (i < num2)
			{
				array[i] = this.m_aBlock[i];
				i++;
			}
			this.m_aBlock = array;
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x00066FCA File Offset: 0x000651CA
		public void Clear()
		{
			this.SetAllBits(false);
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x001CF7C4 File Offset: 0x001CD9C4
		public void SetAllBits(bool value)
		{
			int num = value ? -1 : 1;
			int i = 0;
			int num2 = this.m_aBlock.Length;
			while (i < num2)
			{
				this.m_aBlock[i] = num;
				i++;
			}
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x00066FD3 File Offset: 0x000651D3
		public void SetBit(int index, bool value)
		{
			BitArray.SetBit(index, value, this.m_aBlock);
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x00066FE2 File Offset: 0x000651E2
		public bool IsBitSet(int index)
		{
			return BitArray.IsBitSet(index, this.m_aBlock);
		}

		// Token: 0x040056BB RID: 22203
		private int[] m_aBlock;
	}
}
