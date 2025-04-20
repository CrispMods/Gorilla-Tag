using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class GorillaFaceTextureReplacement : MonoBehaviour, ISpawnable
{
	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060008FD RID: 2301 RVA: 0x00036618 File Offset: 0x00034818
	// (set) Token: 0x060008FE RID: 2302 RVA: 0x00036620 File Offset: 0x00034820
	public bool IsSpawned { get; set; }

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060008FF RID: 2303 RVA: 0x00036629 File Offset: 0x00034829
	// (set) Token: 0x06000900 RID: 2304 RVA: 0x00036631 File Offset: 0x00034831
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06000901 RID: 2305 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnDespawn()
	{
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x0003663A File Offset: 0x0003483A
	public void OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00036643 File Offset: 0x00034843
	private void OnEnable()
	{
		this.myRig.GetComponent<GorillaMouthFlap>().SetFaceMaterialReplacement(this.newFaceMaterial);
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0003665B File Offset: 0x0003485B
	private void OnDisable()
	{
		this.myRig.GetComponent<GorillaMouthFlap>().ClearFaceMaterialReplacement();
	}

	// Token: 0x04000AB8 RID: 2744
	[SerializeField]
	private Material newFaceMaterial;

	// Token: 0x04000AB9 RID: 2745
	private VRRig myRig;
}
