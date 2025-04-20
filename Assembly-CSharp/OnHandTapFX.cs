using System;
using UnityEngine;

// Token: 0x02000556 RID: 1366
internal struct OnHandTapFX : IFXEffectContext<HandEffectContext>
{
	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06002154 RID: 8532 RVA: 0x000F5798 File Offset: 0x000F3998
	public HandEffectContext effectContext
	{
		get
		{
			if (!this.isLeftHand)
			{
				return this.rig.GetRightHandEffect(this.surfaceIndex, this.volume, this.tapDir);
			}
			return this.rig.GetLeftHandEffect(this.surfaceIndex, this.volume, this.tapDir);
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06002155 RID: 8533 RVA: 0x000469B4 File Offset: 0x00044BB4
	public FXSystemSettings settings
	{
		get
		{
			return this.rig.fxSettings;
		}
	}

	// Token: 0x04002511 RID: 9489
	public VRRig rig;

	// Token: 0x04002512 RID: 9490
	public Vector3 tapDir;

	// Token: 0x04002513 RID: 9491
	public bool isLeftHand;

	// Token: 0x04002514 RID: 9492
	public int surfaceIndex;

	// Token: 0x04002515 RID: 9493
	public float volume;
}
