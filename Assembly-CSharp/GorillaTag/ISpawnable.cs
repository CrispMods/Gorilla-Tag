using System;
using GorillaTag.CosmeticSystem;

namespace GorillaTag
{
	// Token: 0x02000BAB RID: 2987
	public interface ISpawnable
	{
		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06004BCD RID: 19405
		// (set) Token: 0x06004BCE RID: 19406
		bool IsSpawned { get; set; }

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06004BCF RID: 19407
		// (set) Token: 0x06004BD0 RID: 19408
		ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004BD1 RID: 19409
		void OnSpawn(VRRig rig);

		// Token: 0x06004BD2 RID: 19410
		void OnDespawn();
	}
}
