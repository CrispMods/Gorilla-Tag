using System;
using System.Collections.Generic;
using GorillaGameModes;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B61 RID: 2913
	[Serializable]
	public class ModeTagEffect
	{
		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060048B8 RID: 18616 RVA: 0x0005F531 File Offset: 0x0005D731
		public HashSet<GameModeType> Modes
		{
			get
			{
				if (this.modesHash == null)
				{
					this.modesHash = new HashSet<GameModeType>(this.modes);
				}
				return this.modesHash;
			}
		}

		// Token: 0x04004A02 RID: 18946
		[SerializeField]
		private GameModeType[] modes;

		// Token: 0x04004A03 RID: 18947
		private HashSet<GameModeType> modesHash;

		// Token: 0x04004A04 RID: 18948
		public TagEffectPack tagEffect;

		// Token: 0x04004A05 RID: 18949
		public bool blockTagOverride;

		// Token: 0x04004A06 RID: 18950
		public bool blockFistBumpOverride;

		// Token: 0x04004A07 RID: 18951
		public bool blockHiveFiveOverride;
	}
}
