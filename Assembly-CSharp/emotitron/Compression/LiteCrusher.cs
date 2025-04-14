using System;
using UnityEngine;

namespace emotitron.Compression
{
	// Token: 0x02000C6C RID: 3180
	[Serializable]
	public abstract class LiteCrusher
	{
		// Token: 0x0600502A RID: 20522 RVA: 0x00186698 File Offset: 0x00184898
		public static int GetBitsForMaxValue(uint maxvalue)
		{
			for (int i = 0; i < 32; i++)
			{
				if (maxvalue >> i == 0U)
				{
					return i;
				}
			}
			return 32;
		}

		// Token: 0x040052CA RID: 21194
		[SerializeField]
		protected int bits;
	}
}
