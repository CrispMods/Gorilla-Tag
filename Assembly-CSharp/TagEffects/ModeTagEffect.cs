using System;
using System.Collections.Generic;
using GorillaGameModes;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B34 RID: 2868
	[Serializable]
	public class ModeTagEffect
	{
		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x0600476F RID: 18287 RVA: 0x0015403A File Offset: 0x0015223A
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

		// Token: 0x0400490D RID: 18701
		[SerializeField]
		private GameModeType[] modes;

		// Token: 0x0400490E RID: 18702
		private HashSet<GameModeType> modesHash;

		// Token: 0x0400490F RID: 18703
		public TagEffectPack tagEffect;

		// Token: 0x04004910 RID: 18704
		public bool blockTagOverride;

		// Token: 0x04004911 RID: 18705
		public bool blockFistBumpOverride;

		// Token: 0x04004912 RID: 18706
		public bool blockHiveFiveOverride;
	}
}
