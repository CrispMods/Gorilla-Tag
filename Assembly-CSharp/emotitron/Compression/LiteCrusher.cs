using System;
using UnityEngine;

namespace emotitron.Compression
{
	// Token: 0x02000C9D RID: 3229
	[Serializable]
	public abstract class LiteCrusher
	{
		// Token: 0x0600518A RID: 20874 RVA: 0x001BDCFC File Offset: 0x001BBEFC
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

		// Token: 0x040053D6 RID: 21462
		[SerializeField]
		protected int bits;
	}
}
