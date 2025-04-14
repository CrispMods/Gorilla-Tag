using System;
using UnityEngine;

// Token: 0x02000548 RID: 1352
internal struct OnHandTapFX : IFXEffectContext<HandEffectContext>
{
	// Token: 0x17000361 RID: 865
	// (get) Token: 0x060020F6 RID: 8438 RVA: 0x000A4AD8 File Offset: 0x000A2CD8
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

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060020F7 RID: 8439 RVA: 0x000A4B28 File Offset: 0x000A2D28
	public FXSystemSettings settings
	{
		get
		{
			return this.rig.fxSettings;
		}
	}

	// Token: 0x040024B9 RID: 9401
	public VRRig rig;

	// Token: 0x040024BA RID: 9402
	public Vector3 tapDir;

	// Token: 0x040024BB RID: 9403
	public bool isLeftHand;

	// Token: 0x040024BC RID: 9404
	public int surfaceIndex;

	// Token: 0x040024BD RID: 9405
	public float volume;
}
