using System;
using GorillaTag.CosmeticSystem;

namespace GorillaTag
{
	// Token: 0x02000B7E RID: 2942
	public interface ISpawnable
	{
		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06004A82 RID: 19074
		// (set) Token: 0x06004A83 RID: 19075
		bool IsSpawned { get; set; }

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06004A84 RID: 19076
		// (set) Token: 0x06004A85 RID: 19077
		ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004A86 RID: 19078
		void OnSpawn(VRRig rig);

		// Token: 0x06004A87 RID: 19079
		void OnDespawn();
	}
}
