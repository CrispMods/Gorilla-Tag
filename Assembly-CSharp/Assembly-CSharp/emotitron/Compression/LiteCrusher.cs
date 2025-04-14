using System;
using UnityEngine;

namespace emotitron.Compression
{
	// Token: 0x02000C6F RID: 3183
	[Serializable]
	public abstract class LiteCrusher
	{
		// Token: 0x06005036 RID: 20534 RVA: 0x00186C60 File Offset: 0x00184E60
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

		// Token: 0x040052DC RID: 21212
		[SerializeField]
		protected int bits;
	}
}
