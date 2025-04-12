using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000032 RID: 50
public class CrittersActorDeposit : MonoBehaviour
{
	// Token: 0x060000E8 RID: 232 RVA: 0x0006A540 File Offset: 0x00068740
	public void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody.IsNotNull())
		{
			CrittersActor component = other.attachedRigidbody.GetComponent<CrittersActor>();
			if (CrittersManager.instance.LocalAuthority() && component.IsNotNull() && this.CanDeposit(component) && this.IsAttachAvailable())
			{
				this.HandleDeposit(component);
			}
		}
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0006A594 File Offset: 0x00068794
	protected virtual bool CanDeposit(CrittersActor depositActor)
	{
		if (depositActor.crittersActorType != this.actorType)
		{
			return false;
		}
		CrittersActor crittersActor;
		if (CrittersManager.instance.actorById.TryGetValue(depositActor.parentActorId, out crittersActor))
		{
			return crittersActor.crittersActorType == CrittersActor.CrittersActorType.Grabber;
		}
		return depositActor.parentActorId == -1;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0002FF4C File Offset: 0x0002E14C
	private bool IsAttachAvailable()
	{
		return this.allowMultiAttach || this.currentAttach == null;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x0006A5E0 File Offset: 0x000687E0
	protected virtual void HandleDeposit(CrittersActor depositedActor)
	{
		this.currentAttach = depositedActor;
		depositedActor.ReleasedEvent.AddListener(new UnityAction<CrittersActor>(this.HandleDetach));
		CrittersActor grabbingActor = this.attachPoint;
		bool positionOverride = this.snapOnAttach;
		bool disableGrabbing = this.disableGrabOnAttach;
		depositedActor.GrabbedBy(grabbingActor, positionOverride, default(Quaternion), default(Vector3), disableGrabbing);
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0002FF64 File Offset: 0x0002E164
	protected virtual void HandleDetach(CrittersActor detachingActor)
	{
		this.currentAttach = null;
	}

	// Token: 0x04000120 RID: 288
	public CrittersActor attachPoint;

	// Token: 0x04000121 RID: 289
	public CrittersActor.CrittersActorType actorType;

	// Token: 0x04000122 RID: 290
	public bool disableGrabOnAttach;

	// Token: 0x04000123 RID: 291
	public bool allowMultiAttach;

	// Token: 0x04000124 RID: 292
	public bool snapOnAttach;

	// Token: 0x04000125 RID: 293
	private CrittersActor currentAttach;
}
