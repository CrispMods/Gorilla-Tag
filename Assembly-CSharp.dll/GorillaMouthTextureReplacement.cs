using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class GorillaMouthTextureReplacement : MonoBehaviour, ISpawnable
{
	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060008DE RID: 2270 RVA: 0x000354CA File Offset: 0x000336CA
	// (set) Token: 0x060008DF RID: 2271 RVA: 0x000354D2 File Offset: 0x000336D2
	public bool IsSpawned { get; set; }

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060008E0 RID: 2272 RVA: 0x000354DB File Offset: 0x000336DB
	// (set) Token: 0x060008E1 RID: 2273 RVA: 0x000354E3 File Offset: 0x000336E3
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x060008E2 RID: 2274 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnDespawn()
	{
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x000354EC File Offset: 0x000336EC
	public void OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x000354F5 File Offset: 0x000336F5
	private void OnEnable()
	{
		this.myRig.GetComponent<GorillaMouthFlap>().SetMouthTextureReplacement(this.newMouthAtlas);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0003550D File Offset: 0x0003370D
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
