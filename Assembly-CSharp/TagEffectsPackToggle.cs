using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using TagEffects;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class TagEffectsPackToggle : MonoBehaviour, ISpawnable
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000982 RID: 2434 RVA: 0x00032AB1 File Offset: 0x00030CB1
	// (set) Token: 0x06000983 RID: 2435 RVA: 0x00032AB9 File Offset: 0x00030CB9
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000984 RID: 2436 RVA: 0x00032AC2 File Offset: 0x00030CC2
	// (set) Token: 0x06000985 RID: 2437 RVA: 0x00032ACA File Offset: 0x00030CCA
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000986 RID: 2438 RVA: 0x00032AD3 File Offset: 0x00030CD3
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00032ADC File Offset: 0x00030CDC
	private void OnEnable()
	{
		this.Apply();
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00032AE4 File Offset: 0x00030CE4
	private void OnDisable()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		this.Remove();
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00032AF4 File Offset: 0x00030CF4
	public void Apply()
	{
		this._rig.CosmeticEffectPack = this.tagEffectPack;
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00032B07 File Offset: 0x00030D07
	public void Remove()
	{
		this._rig.CosmeticEffectPack = null;
	}

	// Token: 0x04000B81 RID: 2945
	private VRRig _rig;

	// Token: 0x04000B82 RID: 2946
	[SerializeField]
	private TagEffectPack tagEffectPack;
}
