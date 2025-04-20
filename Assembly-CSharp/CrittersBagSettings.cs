using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class CrittersBagSettings : CrittersActorSettings
{
	// Token: 0x06000143 RID: 323 RVA: 0x0006DC98 File Offset: 0x0006BE98
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
		crittersBag.blockAttachTypes = this.blockAttachTypes;
	}

	// Token: 0x04000180 RID: 384
	public Collider attachableCollider;

	// Token: 0x04000181 RID: 385
	public BoxCollider dropCube;

	// Token: 0x04000182 RID: 386
	public CrittersAttachPoint.AnchoredLocationTypes anchorLocation;

	// Token: 0x04000183 RID: 387
	public List<Collider> attachDisableColliders;

	// Token: 0x04000184 RID: 388
	public AudioClip attachSound;

	// Token: 0x04000185 RID: 389
	public AudioClip detachSound;

	// Token: 0x04000186 RID: 390
	public List<CrittersActor.CrittersActorType> blockAttachTypes;
}
