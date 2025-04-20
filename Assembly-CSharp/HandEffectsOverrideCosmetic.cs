using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000236 RID: 566
public class HandEffectsOverrideCosmetic : MonoBehaviour, ISpawnable
{
	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00039196 File Offset: 0x00037396
	// (set) Token: 0x06000CF5 RID: 3317 RVA: 0x0003919E File Offset: 0x0003739E
	public bool IsSpawned { get; set; }

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x000391A7 File Offset: 0x000373A7
	// (set) Token: 0x06000CF7 RID: 3319 RVA: 0x000391AF File Offset: 0x000373AF
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06000CF8 RID: 3320 RVA: 0x000391B8 File Offset: 0x000373B8
	public void OnSpawn(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnDespawn()
	{
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x000391C1 File Offset: 0x000373C1
	public void OnEnable()
	{
		if (!this.isLeftHand)
		{
			this._rig.CosmeticHandEffectsOverride_Right.Add(this);
			return;
		}
		this._rig.CosmeticHandEffectsOverride_Left.Add(this);
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x000391EE File Offset: 0x000373EE
	public void OnDisable()
	{
		if (!this.isLeftHand)
		{
			this._rig.CosmeticHandEffectsOverride_Right.Remove(this);
			return;
		}
		this._rig.CosmeticHandEffectsOverride_Left.Remove(this);
	}

	// Token: 0x0400105A RID: 4186
	public HandEffectsOverrideCosmetic.HandEffectType handEffectType;

	// Token: 0x0400105B RID: 4187
	public bool isLeftHand;

	// Token: 0x0400105C RID: 4188
	public HandEffectsOverrideCosmetic.EffectsOverride firstPerson;

	// Token: 0x0400105D RID: 4189
	public HandEffectsOverrideCosmetic.EffectsOverride thirdPerson;

	// Token: 0x0400105E RID: 4190
	private VRRig _rig;

	// Token: 0x02000237 RID: 567
	[Serializable]
	public class EffectsOverride
	{
		// Token: 0x04001061 RID: 4193
		public GameObject effectVFX;

		// Token: 0x04001062 RID: 4194
		public bool playHaptics;

		// Token: 0x04001063 RID: 4195
		public float hapticStrength = 0.5f;

		// Token: 0x04001064 RID: 4196
		public float hapticDuration = 0.5f;

		// Token: 0x04001065 RID: 4197
		public bool parentEffect;
	}

	// Token: 0x02000238 RID: 568
	public enum HandEffectType
	{
		// Token: 0x04001067 RID: 4199
		None,
		// Token: 0x04001068 RID: 4200
		FistBump,
		// Token: 0x04001069 RID: 4201
		HighFive
	}
}
