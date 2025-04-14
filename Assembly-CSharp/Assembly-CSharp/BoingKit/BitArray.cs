using System;

namespace BoingKit
{
	// Token: 0x02000CEA RID: 3306
	public struct BitArray
	{
		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x0600534B RID: 21323 RVA: 0x0019A854 File Offset: 0x00198A54
		public int[] Blocks
		{
			get
			{
				return this.m_aBlock;
			}
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x0019A85C File Offset: 0x00198A5C
		private static int GetBlockIndex(int index)
		{
			return index / 4;
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x0019A861 File Offset: 0x00198A61
		private static int GetSubIndex(int index)
		{
			return index % 4;
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x0019A868 File Offset: 0x00198A68
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

		// Token: 0x0600534F RID: 21327 RVA: 0x0019A8AA File Offset: 0x00198AAA
		private static bool IsBitSet(int index, int[] blocks)
		{
			return (blocks[BitArray.GetBlockIndex(index)] & 1 << BitArray.GetSubIndex(index)) != 0;
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x0019A8C4 File Offset: 0x00198AC4
		public BitArray(int capacity)
		{
			int num = (capacity + 4 - 1) / 4;
			this.m_aBlock = new int[num];
			this.Clear();
		}

		// Token: 0x06005351 RID: 21329 RVA: 0x0019A8EC File Offset: 0x00198AEC
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

		// Token: 0x06005352 RID: 21330 RVA: 0x0019A93B File Offset: 0x00198B3B
		public void Clear()
		{
			this.SetAllBits(false);
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x0019A944 File Offset: 0x00198B44
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

		// Token: 0x06005354 RID: 21332 RVA: 0x0019A977 File Offset: 0x00198B77
		public void SetBit(int index, bool value)
		{
			BitArray.SetBit(index, value, this.m_aBlock);
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x0019A986 File Offset: 0x00198B86
		public bool IsBitSet(int index)
		{
			return BitArray.IsBitSet(index, this.m_aBlock);
		}

		// Token: 0x040055C1 RID: 21953
		private int[] m_aBlock;
	}
}
