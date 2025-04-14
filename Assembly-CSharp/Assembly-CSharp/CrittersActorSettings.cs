using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class CrittersActorSettings : MonoBehaviour
{
	// Token: 0x0600010F RID: 271 RVA: 0x000081F2 File Offset: 0x000063F2
	public virtual void OnEnable()
	{
		this.UpdateActorSettings();
	}

	// Token: 0x06000110 RID: 272 RVA: 0x000081FC File Offset: 0x000063FC
	public virtual void UpdateActorSettings()
	{
		this.parentActor.usesRB = this.usesRB;
		this.parentActor.rb.isKinematic = !this.usesRB;
		this.parentActor.equipmentStorable = this.canBeStored;
		this.parentActor.storeCollider = this.storeCollider;
		this.parentActor.equipmentStoreTriggerCollider = this.equipmentStoreTriggerCollider;
	}

	// Token: 0x0400014B RID: 331
	public CrittersActor parentActor;

	// Token: 0x0400014C RID: 332
	public bool usesRB;

	// Token: 0x0400014D RID: 333
	public bool canBeStored;

	// Token: 0x0400014E RID: 334
	public CapsuleCollider storeCollider;

	// Token: 0x0400014F RID: 335
	public CapsuleCollider equipmentStoreTriggerCollider;
}
