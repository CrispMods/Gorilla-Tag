using System;

namespace BoingKit
{
	// Token: 0x02000CE7 RID: 3303
	public struct BitArray
	{
		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x0600533F RID: 21311 RVA: 0x0019A28C File Offset: 0x0019848C
		public int[] Blocks
		{
			get
			{
				return this.m_aBlock;
			}
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x0019A294 File Offset: 0x00198494
		private static int GetBlockIndex(int index)
		{
			return index / 4;
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x0019A299 File Offset: 0x00198499
		private static int GetSubIndex(int index)
		{
			return index % 4;
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x0019A2A0 File Offset: 0x001984A0
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

		// Token: 0x06005343 RID: 21315 RVA: 0x0019A2E2 File Offset: 0x001984E2
		private static bool IsBitSet(int index, int[] blocks)
		{
			return (blocks[BitArray.GetBlockIndex(index)] & 1 << BitArray.GetSubIndex(index)) != 0;
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x0019A2FC File Offset: 0x001984FC
		public BitArray(int capacity)
		{
			int num = (capacity + 4 - 1) / 4;
			this.m_aBlock = new int[num];
			this.Clear();
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x0019A324 File Offset: 0x00198524
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

		// Token: 0x06005346 RID: 21318 RVA: 0x0019A373 File Offset: 0x00198573
		public void Clear()
		{
			this.SetAllBits(false);
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x0019A37C File Offset: 0x0019857C
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

		// Token: 0x06005348 RID: 21320 RVA: 0x0019A3AF File Offset: 0x001985AF
		public void SetBit(int index, bool value)
		{
			BitArray.SetBit(index, value, this.m_aBlock);
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x0019A3BE File Offset: 0x001985BE
		public bool IsBitSet(int index)
		{
			return BitArray.IsBitSet(index, this.m_aBlock);
		}

		// Token: 0x040055AF RID: 21935
		private int[] m_aBlock;
	}
}
