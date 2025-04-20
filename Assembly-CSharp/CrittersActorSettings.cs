using System;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class CrittersActorSettings : MonoBehaviour
{
	// Token: 0x06000120 RID: 288 RVA: 0x000310AA File Offset: 0x0002F2AA
	public virtual void OnEnable()
	{
		this.UpdateActorSettings();
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0006D55C File Offset: 0x0006B75C
	public virtual void UpdateActorSettings()
	{
		this.parentActor.usesRB = this.usesRB;
		this.parentActor.rb.isKinematic = !this.usesRB;
		this.parentActor.equipmentStorable = this.canBeStored;
		this.parentActor.storeCollider = this.storeCollider;
		this.parentActor.equipmentStoreTriggerCollider = this.equipmentStoreTriggerCollider;
	}

	// Token: 0x04000154 RID: 340
	public CrittersActor parentActor;

	// Token: 0x04000155 RID: 341
	public bool usesRB;

	// Token: 0x04000156 RID: 342
	public bool canBeStored;

	// Token: 0x04000157 RID: 343
	public CapsuleCollider storeCollider;

	// Token: 0x04000158 RID: 344
	public CapsuleCollider equipmentStoreTriggerCollider;
}
