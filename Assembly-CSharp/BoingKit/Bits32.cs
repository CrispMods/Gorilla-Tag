using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE6 RID: 3302
	[Serializable]
	public struct Bits32
	{
		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x0600533A RID: 21306 RVA: 0x0019A233 File Offset: 0x00198433
		public int IntValue
		{
			get
			{
				return this.m_bits;
			}
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x0019A23B File Offset: 0x0019843B
		public Bits32(int bits = 0)
		{
			this.m_bits = bits;
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x0019A244 File Offset: 0x00198444
		public void Clear()
		{
			this.m_bits = 0;
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x0019A24D File Offset: 0x0019844D
		public void SetBit(int index, bool value)
		{
			if (value)
			{
				this.m_bits |= 1 << index;
				return;
			}
			this.m_bits &= ~(1 << index);
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x0019A27A File Offset: 0x0019847A
		public bool IsBitSet(int index)
		{
			return (this.m_bits & 1 << index) != 0;
		}

		// Token: 0x040055AE RID: 21934
		[SerializeField]
		private int m_bits;
	}
}
