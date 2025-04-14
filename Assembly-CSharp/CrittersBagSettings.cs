using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class CrittersBagSettings : CrittersActorSettings
{
	// Token: 0x0600012D RID: 301 RVA: 0x0000874C File Offset: 0x0000694C
	public override void UpdateActorSettings()
	{
		base.UpdateActorSettings();
		CrittersBag crittersBag = (CrittersBag)this.parentActor;
		crittersBag.attachableCollider = this.attachableCollider;
		crittersBag.dropCube = this.dropCube;
		crittersBag.anchorLocation = this.anchorLocation;
		crittersBag.attachDisableColliders = this.attachDisableColliders;
		crittersBag.attachSound = this.attachSound;
		crittersBag.detachSound = this.detachSound;
	}

	// Token: 0x0400016E RID: 366
	public Collider attachableCollider;

	// Token: 0x0400016F RID: 367
	public BoxCollider dropCube;

	// Token: 0x04000170 RID: 368
	public CrittersAttachPoint.AnchoredLocationTypes anchorLocation;

	// Token: 0x04000171 RID: 369
	public List<Collider> attachDisableColliders;

	// Token: 0x04000172 RID: 370
	public AudioClip attachSound;

	// Token: 0x04000173 RID: 371
	public AudioClip detachSound;
}
