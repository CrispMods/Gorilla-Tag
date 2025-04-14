using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using TagEffects;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class TagEffectsPackToggle : MonoBehaviour, ISpawnable
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000984 RID: 2436 RVA: 0x00032DD5 File Offset: 0x00030FD5
	// (set) Token: 0x06000985 RID: 2437 RVA: 0x00032DDD File Offset: 0x00030FDD
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000986 RID: 2438 RVA: 0x00032DE6 File Offset: 0x00030FE6
	// (set) Token: 0x06000987 RID: 2439 RVA: 0x00032DEE File Offset: 0x00030FEE
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000988 RID: 2440 RVA: 0x00032DF7 File Offset: 0x00030FF7
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00032E00 File Offset: 0x00031000
	private void OnEnable()
	{
		this.Apply();
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00032E08 File Offset: 0x00031008
	private void OnDisable()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		this.Remove();
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00032E18 File Offset: 0x00031018
	public void Apply()
	{
		this._rig.CosmeticEffectPack = this.tagEffectPack;
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00032E2B File Offset: 0x0003102B
	public void Remove()
	{
		this._rig.CosmeticEffectPack = null;
	}

	// Token: 0x04000B82 RID: 2946
	private VRRig _rig;

	// Token: 0x04000B83 RID: 2947
	[SerializeField]
	private TagEffectPack tagEffectPack;
}
