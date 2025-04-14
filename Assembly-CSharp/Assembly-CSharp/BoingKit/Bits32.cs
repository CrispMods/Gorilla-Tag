using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE9 RID: 3305
	[Serializable]
	public struct Bits32
	{
		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06005346 RID: 21318 RVA: 0x0019A7FB File Offset: 0x001989FB
		public int IntValue
		{
			get
			{
				return this.m_bits;
			}
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x0019A803 File Offset: 0x00198A03
		public Bits32(int bits = 0)
		{
			this.m_bits = bits;
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x0019A80C File Offset: 0x00198A0C
		public void Clear()
		{
			this.m_bits = 0;
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x0019A815 File Offset: 0x00198A15
		public void SetBit(int index, bool value)
		{
			if (value)
			{
				this.m_bits |= 1 << index;
				return;
			}
			this.m_bits &= ~(1 << index);
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x0019A842 File Offset: 0x00198A42
		public bool IsBitSet(int index)
		{
			return (this.m_bits & 1 << index) != 0;
		}

		// Token: 0x040055C0 RID: 21952
		[SerializeField]
		private int m_bits;
	}
}
