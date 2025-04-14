using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class GorillaMouthTextureReplacement : MonoBehaviour, ISpawnable
{
	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060008DE RID: 2270 RVA: 0x000307FD File Offset: 0x0002E9FD
	// (set) Token: 0x060008DF RID: 2271 RVA: 0x00030805 File Offset: 0x0002EA05
	public bool IsSpawned { get; set; }

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060008E0 RID: 2272 RVA: 0x0003080E File Offset: 0x0002EA0E
	// (set) Token: 0x060008E1 RID: 2273 RVA: 0x00030816 File Offset: 0x0002EA16
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x060008E2 RID: 2274 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnDespawn()
	{
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0003081F File Offset: 0x0002EA1F
	public void OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00030828 File Offset: 0x0002EA28
	private void OnEnable()
	{
		this.myRig.GetComponent<GorillaMouthFlap>().SetMouthTextureReplacement(this.newMouthAtlas);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x00030840 File Offset: 0x0002EA40
	private void OnDisable()
	{
		this.myRig.GetComponent<GorillaMouthFlap>().ClearMouthTextureReplacement();
	}

	// Token: 0x04000AC6 RID: 2758
	[SerializeField]
	private Texture2D newMouthAtlas;

	// Token: 0x04000AC7 RID: 2759
	private VRRig myRig;
}
