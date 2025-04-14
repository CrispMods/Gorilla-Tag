using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class GorillaMouthTextureReplacement : MonoBehaviour, ISpawnable
{
	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060008DC RID: 2268 RVA: 0x000304D9 File Offset: 0x0002E6D9
	// (set) Token: 0x060008DD RID: 2269 RVA: 0x000304E1 File Offset: 0x0002E6E1
	public bool IsSpawned { get; set; }

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060008DE RID: 2270 RVA: 0x000304EA File Offset: 0x0002E6EA
	// (set) Token: 0x060008DF RID: 2271 RVA: 0x000304F2 File Offset: 0x0002E6F2
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x060008E0 RID: 2272 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnDespawn()
	{
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x000304FB File Offset: 0x0002E6FB
	public void OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x00030504 File Offset: 0x0002E704
	private void OnEnable()
	{
		this.myRig.GetComponent<GorillaMouthFlap>().SetMouthTextureReplacement(this.newMouthAtlas);
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0003051C File Offset: 0x0002E71C
	private void OnDisable()
	{
		this.myRig.GetComponent<GorillaMouthFlap>().ClearMouthTextureReplacement();
	}

	// Token: 0x04000AC5 RID: 2757
	[SerializeField]
	private Texture2D newMouthAtlas;

	// Token: 0x04000AC6 RID: 2758
	private VRRig myRig;
}
