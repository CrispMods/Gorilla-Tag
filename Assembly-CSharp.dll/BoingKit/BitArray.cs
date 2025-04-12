using System;

namespace BoingKit
{
	// Token: 0x02000CEA RID: 3306
	public struct BitArray
	{
		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x0600534B RID: 21323 RVA: 0x00065529 File Offset: 0x00063729
		public int[] Blocks
		{
			get
			{
				return this.m_aBlock;
			}
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x00065531 File Offset: 0x00063731
		private static int GetBlockIndex(int index)
		{
			return index / 4;
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x00065536 File Offset: 0x00063736
		private static int GetSubIndex(int index)
		{
			return index % 4;
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x001C7624 File Offset: 0x001C5824
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

		// Token: 0x0600534F RID: 21327 RVA: 0x0006553B File Offset: 0x0006373B
		private static bool IsBitSet(int index, int[] blocks)
		{
			return (blocks[BitArray.GetBlockIndex(index)] & 1 << BitArray.GetSubIndex(index)) != 0;
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x001C7668 File Offset: 0x001C5868
		public BitArray(int capacity)
		{
			int num = (capacity + 4 - 1) / 4;
			this.m_aBlock = new int[num];
			this.Clear();
		}

		// Token: 0x06005351 RID: 21329 RVA: 0x001C7690 File Offset: 0x001C5890
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

		// Token: 0x06005352 RID: 21330 RVA: 0x00065554 File Offset: 0x00063754
		public void Clear()
		{
			this.SetAllBits(false);
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x001C76E0 File Offset: 0x001C58E0
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

		// Token: 0x06005354 RID: 21332 RVA: 0x0006555D File Offset: 0x0006375D
		public void SetBit(int index, bool value)
		{
			BitArray.SetBit(index, value, this.m_aBlock);
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x0006556C File Offset: 0x0006376C
		public bool IsBitSet(int index)
		{
			return BitArray.IsBitSet(index, this.m_aBlock);
		}

		// Token: 0x040055C1 RID: 21953
		private int[] m_aBlock;
	}
}
