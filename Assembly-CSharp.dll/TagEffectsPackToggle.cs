using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using TagEffects;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class TagEffectsPackToggle : MonoBehaviour, ISpawnable
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000984 RID: 2436 RVA: 0x00035C37 File Offset: 0x00033E37
	// (set) Token: 0x06000985 RID: 2437 RVA: 0x00035C3F File Offset: 0x00033E3F
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000986 RID: 2438 RVA: 0x00035C48 File Offset: 0x00033E48
	// (set) Token: 0x06000987 RID: 2439 RVA: 0x00035C50 File Offset: 0x00033E50
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000988 RID: 2440 RVA: 0x00035C59 File Offset: 0x00033E59
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x0002F75F File Offset: 0x0002D95F
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00035C62 File Offset: 0x00033E62
	private void OnEnable()
	{
		this.Apply();
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00035C6A File Offset: 0x00033E6A
	private void OnDisable()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		this.Remove();
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00035C7A File Offset: 0x00033E7A
	public void Apply()
	{
		this._rig.CosmeticEffectPack = this.tagEffectPack;
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00035C8D File Offset: 0x00033E8D
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
