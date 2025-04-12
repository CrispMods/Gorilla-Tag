using System;
using System.Collections.Generic;
using GorillaGameModes;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B37 RID: 2871
	[Serializable]
	public class ModeTagEffect
	{
		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x0600477B RID: 18299 RVA: 0x0005DB1A File Offset: 0x0005BD1A
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

		// Token: 0x0400491F RID: 18719
		[SerializeField]
		private GameModeType[] modes;

		// Token: 0x04004920 RID: 18720
		private HashSet<GameModeType> modesHash;

		// Token: 0x04004921 RID: 18721
		public TagEffectPack tagEffect;

		// Token: 0x04004922 RID: 18722
		public bool blockTagOverride;

		// Token: 0x04004923 RID: 18723
		public bool blockFistBumpOverride;

		// Token: 0x04004924 RID: 18724
		public bool blockHiveFiveOverride;
	}
}
