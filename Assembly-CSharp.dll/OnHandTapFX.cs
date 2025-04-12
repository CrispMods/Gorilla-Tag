using System;
using UnityEngine;

// Token: 0x02000549 RID: 1353
internal struct OnHandTapFX : IFXEffectContext<HandEffectContext>
{
	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060020FE RID: 8446 RVA: 0x000F2A1C File Offset: 0x000F0C1C
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

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x060020FF RID: 8447 RVA: 0x0004560F File Offset: 0x0004380F
	public FXSystemSettings settings
	{
		get
		{
			return this.rig.fxSettings;
		}
	}

	// Token: 0x040024BF RID: 9407
	public VRRig rig;

	// Token: 0x040024C0 RID: 9408
	public Vector3 tapDir;

	// Token: 0x040024C1 RID: 9409
	public bool isLeftHand;

	// Token: 0x040024C2 RID: 9410
	public int surfaceIndex;

	// Token: 0x040024C3 RID: 9411
	public float volume;
}
