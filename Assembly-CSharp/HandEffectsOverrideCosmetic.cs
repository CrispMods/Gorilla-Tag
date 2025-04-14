using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200022B RID: 555
public class HandEffectsOverrideCosmetic : MonoBehaviour, ISpawnable
{
	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x00042D7C File Offset: 0x00040F7C
	// (set) Token: 0x06000CAA RID: 3242 RVA: 0x00042D84 File Offset: 0x00040F84
	public bool IsSpawned { get; set; }

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000CAB RID: 3243 RVA: 0x00042D8D File Offset: 0x00040F8D
	// (set) Token: 0x06000CAC RID: 3244 RVA: 0x00042D95 File Offset: 0x00040F95
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06000CAD RID: 3245 RVA: 0x00042D9E File Offset: 0x00040F9E
	public void OnSpawn(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnDespawn()
	{
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x00042DA7 File Offset: 0x00040FA7
	public void OnEnable()
	{
		if (!this.isLeftHand)
		{
			this._rig.CosmeticHandEffectsOverride_Right.Add(this);
			return;
		}
		this._rig.CosmeticHandEffectsOverride_Left.Add(this);
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x00042DD4 File Offset: 0x00040FD4
	public void OnDisable()
	{
		if (!this.isLeftHand)
		{
			this._rig.CosmeticHandEffectsOverride_Right.Remove(this);
			return;
		}
		this._rig.CosmeticHandEffectsOverride_Left.Remove(this);
	}

	// Token: 0x04001014 RID: 4116
	public HandEffectsOverrideCosmetic.HandEffectType handEffectType;

	// Token: 0x04001015 RID: 4117
	public bool isLeftHand;

	// Token: 0x04001016 RID: 4118
	public HandEffectsOverrideCosmetic.EffectsOverride firstPerson;

	// Token: 0x04001017 RID: 4119
	public HandEffectsOverrideCosmetic.EffectsOverride thirdPerson;

	// Token: 0x04001018 RID: 4120
	private VRRig _rig;

	// Token: 0x0200022C RID: 556
	[Serializable]
	public class EffectsOverride
	{
		// Token: 0x0400101B RID: 4123
		public GameObject effectVFX;

		// Token: 0x0400101C RID: 4124
		public bool playHaptics;

		// Token: 0x0400101D RID: 4125
		public float hapticStrength = 0.5f;

		// Token: 0x0400101E RID: 4126
		public float hapticDuration = 0.5f;

		// Token: 0x0400101F RID: 4127
		public bool parentEffect;
	}

	// Token: 0x0200022D RID: 557
	public enum HandEffectType
	{
		// Token: 0x04001021 RID: 4129
		None,
		// Token: 0x04001022 RID: 4130
		FistBump,
		// Token: 0x04001023 RID: 4131
		HighFive
	}
}
