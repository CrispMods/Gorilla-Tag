using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200016C RID: 364
public class GorillaMouthTextureReplacement : MonoBehaviour, ISpawnable
{
	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000929 RID: 2345 RVA: 0x00036795 File Offset: 0x00034995
	// (set) Token: 0x0600092A RID: 2346 RVA: 0x0003679D File Offset: 0x0003499D
	public bool IsSpawned { get; set; }

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x000367A6 File Offset: 0x000349A6
	// (set) Token: 0x0600092C RID: 2348 RVA: 0x000367AE File Offset: 0x000349AE
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x0600092D RID: 2349 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnDespawn()
	{
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x000367B7 File Offset: 0x000349B7
	public void OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x000367C0 File Offset: 0x000349C0
	private void OnEnable()
	{
		this.myRig.GetComponent<GorillaMouthFlap>().SetMouthTextureReplacement(this.newMouthAtlas);
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x000367D8 File Offset: 0x000349D8
	private void OnDisable()
	{
		this.myRig.GetComponent<GorillaMouthFlap>().ClearMouthTextureReplacement();
	}

	// Token: 0x04000B0C RID: 2828
	[SerializeField]
	private Texture2D newMouthAtlas;

	// Token: 0x04000B0D RID: 2829
	private VRRig myRig;
}
