using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class DebugTestGrabber : MonoBehaviour
{
	// Token: 0x060002BF RID: 703 RVA: 0x000321C6 File Offset: 0x000303C6
	private void Awake()
	{
		if (this.grabber == null)
		{
			this.grabber = base.GetComponentInChildren<CrittersGrabber>();
		}
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00075990 File Offset: 0x00073B90
	private void LateUpdate()
	{
		if (this.transformToFollow != null)
		{
			base.transform.rotation = this.transformToFollow.rotation;
			base.transform.position = this.transformToFollow.position;
		}
		if (this.grabber == null)
		{
			return;
		}
		if (!this.isGrabbing && this.setIsGrabbing)
		{
			this.setIsGrabbing = false;
			this.isGrabbing = true;
			this.remainingGrabDuration = this.grabDuration;
		}
		else if (this.isGrabbing && this.setRelease)
		{
			this.setRelease = false;
			this.isGrabbing = false;
			this.DoRelease();
		}
		if (this.isGrabbing && this.remainingGrabDuration > 0f)
		{
			this.remainingGrabDuration -= Time.deltaTime;
			this.DoGrab();
		}
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00075A64 File Offset: 0x00073C64
	private void DoGrab()
	{
		this.grabber.grabbing = true;
		int num = Physics.OverlapSphereNonAlloc(base.transform.position, this.grabRadius, this.colliders, LayerMask.GetMask(new string[]
		{
			"GorillaInteractable"
		}));
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				CrittersActor componentInParent = this.colliders[i].GetComponentInParent<CrittersActor>();
				if (!(componentInParent == null) && componentInParent.usesRB && componentInParent.CanBeGrabbed(this.grabber))
				{
					this.isHandGrabbingDisabled = true;
					if (componentInParent.equipmentStorable)
					{
						componentInParent.localCanStore = true;
					}
					componentInParent.GrabbedBy(this.grabber, false, default(Quaternion), default(Vector3), false);
					this.grabber.grabbedActors.Add(componentInParent);
					this.remainingGrabDuration = 0f;
					return;
				}
			}
		}
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00075B48 File Offset: 0x00073D48
	private void DoRelease()
	{
		this.grabber.grabbing = false;
		for (int i = this.grabber.grabbedActors.Count - 1; i >= 0; i--)
		{
			CrittersActor crittersActor = this.grabber.grabbedActors[i];
			crittersActor.Released(true, crittersActor.transform.rotation, crittersActor.transform.position, this.estimator.linearVelocity, default(Vector3));
			if (i < this.grabber.grabbedActors.Count)
			{
				this.grabber.grabbedActors.RemoveAt(i);
			}
		}
		if (this.isHandGrabbingDisabled)
		{
			this.isHandGrabbingDisabled = false;
		}
	}

	// Token: 0x04000370 RID: 880
	public bool isGrabbing;

	// Token: 0x04000371 RID: 881
	public bool setIsGrabbing;

	// Token: 0x04000372 RID: 882
	public bool setRelease;

	// Token: 0x04000373 RID: 883
	public Collider[] colliders = new Collider[50];

	// Token: 0x04000374 RID: 884
	public bool isLeft;

	// Token: 0x04000375 RID: 885
	public float grabRadius = 0.05f;

	// Token: 0x04000376 RID: 886
	public Transform transformToFollow;

	// Token: 0x04000377 RID: 887
	public GorillaVelocityEstimator estimator;

	// Token: 0x04000378 RID: 888
	public CrittersGrabber grabber;

	// Token: 0x04000379 RID: 889
	public CrittersActorGrabber otherHand;

	// Token: 0x0400037A RID: 890
	private bool isHandGrabbingDisabled;

	// Token: 0x0400037B RID: 891
	private float grabDuration = 0.3f;

	// Token: 0x0400037C RID: 892
	private float remainingGrabDuration;
}
