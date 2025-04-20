using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B58 RID: 2904
	public interface IHandEffectsTrigger
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06004890 RID: 18576
		IHandEffectsTrigger.Mode EffectMode { get; }

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06004891 RID: 18577
		Transform Transform { get; }

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06004892 RID: 18578
		VRRig Rig { get; }

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06004893 RID: 18579
		bool FingersDown { get; }

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06004894 RID: 18580
		bool FingersUp { get; }

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06004895 RID: 18581
		Vector3 Velocity { get; }

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06004896 RID: 18582
		bool RightHand { get; }

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06004897 RID: 18583
		TagEffectPack CosmeticEffectPack { get; }

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06004898 RID: 18584
		bool Static { get; }

		// Token: 0x06004899 RID: 18585
		void OnTriggerEntered(IHandEffectsTrigger other);

		// Token: 0x0600489A RID: 18586
		bool InTriggerZone(IHandEffectsTrigger t);

		// Token: 0x02000B59 RID: 2905
		public enum Mode
		{
			// Token: 0x040049D8 RID: 18904
			HighFive,
			// Token: 0x040049D9 RID: 18905
			FistBump,
			// Token: 0x040049DA RID: 18906
			Tag3P,
			// Token: 0x040049DB RID: 18907
			Tag1P,
			// Token: 0x040049DC RID: 18908
			HighFive_And_FistBump
		}
	}
}
