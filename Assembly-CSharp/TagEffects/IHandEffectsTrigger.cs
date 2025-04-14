using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B2B RID: 2859
	public interface IHandEffectsTrigger
	{
		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06004747 RID: 18247
		IHandEffectsTrigger.Mode EffectMode { get; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06004748 RID: 18248
		Transform Transform { get; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06004749 RID: 18249
		VRRig Rig { get; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600474A RID: 18250
		bool FingersDown { get; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x0600474B RID: 18251
		bool FingersUp { get; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x0600474C RID: 18252
		Vector3 Velocity { get; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x0600474D RID: 18253
		bool RightHand { get; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x0600474E RID: 18254
		TagEffectPack CosmeticEffectPack { get; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x0600474F RID: 18255
		bool Static { get; }

		// Token: 0x06004750 RID: 18256
		void OnTriggerEntered(IHandEffectsTrigger other);

		// Token: 0x06004751 RID: 18257
		bool InTriggerZone(IHandEffectsTrigger t);

		// Token: 0x02000B2C RID: 2860
		public enum Mode
		{
			// Token: 0x040048E3 RID: 18659
			HighFive,
			// Token: 0x040048E4 RID: 18660
			FistBump,
			// Token: 0x040048E5 RID: 18661
			Tag3P,
			// Token: 0x040048E6 RID: 18662
			Tag1P,
			// Token: 0x040048E7 RID: 18663
			HighFive_And_FistBump
		}
	}
}
