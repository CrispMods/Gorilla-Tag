using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using TagEffects;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class TagEffectsPackToggle : MonoBehaviour, ISpawnable
{
	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x060009CE RID: 2510 RVA: 0x00036EF7 File Offset: 0x000350F7
	// (set) Token: 0x060009CF RID: 2511 RVA: 0x00036EFF File Offset: 0x000350FF
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00036F08 File Offset: 0x00035108
	// (set) Token: 0x060009D1 RID: 2513 RVA: 0x00036F10 File Offset: 0x00035110
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060009D2 RID: 2514 RVA: 0x00036F19 File Offset: 0x00035119
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x00036F22 File Offset: 0x00035122
	private void OnEnable()
	{
		this.Apply();
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00036F2A File Offset: 0x0003512A
	private void OnDisable()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		this.Remove();
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00036F3A File Offset: 0x0003513A
	public void Apply()
	{
		this._rig.CosmeticEffectPack = this.tagEffectPack;
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00036F4D File Offset: 0x0003514D
	public void Remove()
	{
		this._rig.CosmeticEffectPack = null;
	}

	// Token: 0x04000BC7 RID: 3015
	private VRRig _rig;

	// Token: 0x04000BC8 RID: 3016
	[SerializeField]
	private TagEffectPack tagEffectPack;
}
