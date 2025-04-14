using System;
using GorillaTag.CosmeticSystem;

namespace GorillaTag
{
	// Token: 0x02000B81 RID: 2945
	public interface ISpawnable
	{
		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06004A8E RID: 19086
		// (set) Token: 0x06004A8F RID: 19087
		bool IsSpawned { get; set; }

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06004A90 RID: 19088
		// (set) Token: 0x06004A91 RID: 19089
		ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004A92 RID: 19090
		void OnSpawn(VRRig rig);

		// Token: 0x06004A93 RID: 19091
		void OnDespawn();
	}
}
