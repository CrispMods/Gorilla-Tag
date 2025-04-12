using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B2E RID: 2862
	public interface IHandEffectsTrigger
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06004753 RID: 18259
		IHandEffectsTrigger.Mode EffectMode { get; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06004754 RID: 18260
		Transform Transform { get; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06004755 RID: 18261
		VRRig Rig { get; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06004756 RID: 18262
		bool FingersDown { get; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06004757 RID: 18263
		bool FingersUp { get; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06004758 RID: 18264
		Vector3 Velocity { get; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06004759 RID: 18265
		bool RightHand { get; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x0600475A RID: 18266
		TagEffectPack CosmeticEffectPack { get; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x0600475B RID: 18267
		bool Static { get; }

		// Token: 0x0600475C RID: 18268
		void OnTriggerEntered(IHandEffectsTrigger other);

		// Token: 0x0600475D RID: 18269
		bool InTriggerZone(IHandEffectsTrigger t);

		// Token: 0x02000B2F RID: 2863
		public enum Mode
		{
			// Token: 0x040048F5 RID: 18677
			HighFive,
			// Token: 0x040048F6 RID: 18678
			FistBump,
			// Token: 0x040048F7 RID: 18679
			Tag3P,
			// Token: 0x040048F8 RID: 18680
			Tag1P,
			// Token: 0x040048F9 RID: 18681
			HighFive_And_FistBump
		}
	}
}
