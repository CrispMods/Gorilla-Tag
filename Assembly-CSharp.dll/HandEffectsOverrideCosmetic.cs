using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200022B RID: 555
public class HandEffectsOverrideCosmetic : MonoBehaviour, ISpawnable
{
	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000CAB RID: 3243 RVA: 0x00037ED6 File Offset: 0x000360D6
	// (set) Token: 0x06000CAC RID: 3244 RVA: 0x00037EDE File Offset: 0x000360DE
	public bool IsSpawned { get; set; }

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00037EE7 File Offset: 0x000360E7
	// (set) Token: 0x06000CAE RID: 3246 RVA: 0x00037EEF File Offset: 0x000360EF
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06000CAF RID: 3247 RVA: 0x00037EF8 File Offset: 0x000360F8
	public void OnSpawn(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnDespawn()
	{
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x00037F01 File Offset: 0x00036101
	public void OnEnable()
	{
		if (!this.isLeftHand)
		{
			this._rig.CosmeticHandEffectsOverride_Right.Add(this);
			return;
		}
		this._rig.CosmeticHandEffectsOverride_Left.Add(this);
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x00037F2E File Offset: 0x0003612E
	public void OnDisable()
	{
		if (!this.isLeftHand)
		{
			this._rig.CosmeticHandEffectsOverride_Right.Remove(this);
			return;
		}
		this._rig.CosmeticHandEffectsOverride_Left.Remove(this);
	}

	// Token: 0x04001015 RID: 4117
	public HandEffectsOverrideCosmetic.HandEffectType handEffectType;

	// Token: 0x04001016 RID: 4118
	public bool isLeftHand;

	// Token: 0x04001017 RID: 4119
	public HandEffectsOverrideCosmetic.EffectsOverride firstPerson;

	// Token: 0x04001018 RID: 4120
	public HandEffectsOverrideCosmetic.EffectsOverride thirdPerson;

	// Token: 0x04001019 RID: 4121
	private VRRig _rig;

	// Token: 0x0200022C RID: 556
	[Serializable]
	public class EffectsOverride
	{
		// Token: 0x0400101C RID: 4124
		public GameObject effectVFX;

		// Token: 0x0400101D RID: 4125
		public bool playHaptics;

		// Token: 0x0400101E RID: 4126
		public float hapticStrength = 0.5f;

		// Token: 0x0400101F RID: 4127
		public float hapticDuration = 0.5f;

		// Token: 0x04001020 RID: 4128
		public bool parentEffect;
	}

	// Token: 0x0200022D RID: 557
	public enum HandEffectType
	{
		// Token: 0x04001022 RID: 4130
		None,
		// Token: 0x04001023 RID: 4131
		FistBump,
		// Token: 0x04001024 RID: 4132
		HighFive
	}
}
