using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE9 RID: 3305
	[Serializable]
	public struct Bits32
	{
		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06005346 RID: 21318 RVA: 0x000654D0 File Offset: 0x000636D0
		public int IntValue
		{
			get
			{
				return this.m_bits;
			}
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x000654D8 File Offset: 0x000636D8
		public Bits32(int bits = 0)
		{
			this.m_bits = bits;
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x000654E1 File Offset: 0x000636E1
		public void Clear()
		{
			this.m_bits = 0;
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x000654EA File Offset: 0x000636EA
		public void SetBit(int index, bool value)
		{
			if (value)
			{
				this.m_bits |= 1 << index;
				return;
			}
			this.m_bits &= ~(1 << index);
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x00065517 File Offset: 0x00063717
		public bool IsBitSet(int index)
		{
			return (this.m_bits & 1 << index) != 0;
		}

		// Token: 0x040055C0 RID: 21952
		[SerializeField]
		private int m_bits;
	}
}
