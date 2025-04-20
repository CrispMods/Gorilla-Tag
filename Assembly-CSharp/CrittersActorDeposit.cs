using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000035 RID: 53
public class CrittersActorDeposit : MonoBehaviour
{
	// Token: 0x060000F9 RID: 249 RVA: 0x0006C658 File Offset: 0x0006A858
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

	// Token: 0x060000FA RID: 250 RVA: 0x0006C6AC File Offset: 0x0006A8AC
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

	// Token: 0x060000FB RID: 251 RVA: 0x00030F21 File Offset: 0x0002F121
	private bool IsAttachAvailable()
	{
		return this.allowMultiAttach || this.currentAttach == null;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x0006C6F8 File Offset: 0x0006A8F8
	protected virtual void HandleDeposit(CrittersActor depositedActor)
	{
		this.currentAttach = depositedActor;
		depositedActor.ReleasedEvent.AddListener(new UnityAction<CrittersActor>(this.HandleDetach));
		CrittersActor grabbingActor = this.attachPoint;
		bool positionOverride = this.snapOnAttach;
		bool disableGrabbing = this.disableGrabOnAttach;
		depositedActor.GrabbedBy(grabbingActor, positionOverride, default(Quaternion), default(Vector3), disableGrabbing);
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00030F39 File Offset: 0x0002F139
	protected virtual void HandleDetach(CrittersActor detachingActor)
	{
		this.currentAttach = null;
	}

	// Token: 0x04000129 RID: 297
	public CrittersActor attachPoint;

	// Token: 0x0400012A RID: 298
	public CrittersActor.CrittersActorType actorType;

	// Token: 0x0400012B RID: 299
	public bool disableGrabOnAttach;

	// Token: 0x0400012C RID: 300
	public bool allowMultiAttach;

	// Token: 0x0400012D RID: 301
	public bool snapOnAttach;

	// Token: 0x0400012E RID: 302
	private CrittersActor currentAttach;
}
