using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D17 RID: 3351
	[Serializable]
	public struct Bits32
	{
		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x0600549C RID: 21660 RVA: 0x00066F46 File Offset: 0x00065146
		public int IntValue
		{
			get
			{
				return this.m_bits;
			}
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x00066F4E File Offset: 0x0006514E
		public Bits32(int bits = 0)
		{
			this.m_bits = bits;
		}

		// Token: 0x0600549E RID: 21662 RVA: 0x00066F57 File Offset: 0x00065157
		public void Clear()
		{
			this.m_bits = 0;
		}

		// Token: 0x0600549F RID: 21663 RVA: 0x00066F60 File Offset: 0x00065160
		public void SetBit(int index, bool value)
		{
			if (value)
			{
				this.m_bits |= 1 << index;
				return;
			}
			this.m_bits &= ~(1 << index);
		}

		// Token: 0x060054A0 RID: 21664 RVA: 0x00066F8D File Offset: 0x0006518D
		public bool IsBitSet(int index)
		{
			return (this.m_bits & 1 << index) != 0;
		}

		// Token: 0x040056BA RID: 22202
		[SerializeField]
		private int m_bits;
	}
}
