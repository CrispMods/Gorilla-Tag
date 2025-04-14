using System;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class DebugTestGrabber : MonoBehaviour
{
	// Token: 0x06000291 RID: 657 RVA: 0x00010CE4 File Offset: 0x0000EEE4
	private void Awake()
	{
		if (this.grabber == null)
		{
			this.grabber = base.GetComponentInChildren<CrittersGrabber>();
		}
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00010D00 File Offset: 0x0000EF00
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

	// Token: 0x06000293 RID: 659 RVA: 0x00010DD4 File Offset: 0x0000EFD4
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

	// Token: 0x06000294 RID: 660 RVA: 0x00010EB8 File Offset: 0x0000F0B8
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

	// Token: 0x0400033E RID: 830
	public bool isGrabbing;

	// Token: 0x0400033F RID: 831
	public bool setIsGrabbing;

	// Token: 0x04000340 RID: 832
	public bool setRelease;

	// Token: 0x04000341 RID: 833
	public Collider[] colliders = new Collider[50];

	// Token: 0x04000342 RID: 834
	public bool isLeft;

	// Token: 0x04000343 RID: 835
	public float grabRadius = 0.05f;

	// Token: 0x04000344 RID: 836
	public Transform transformToFollow;

	// Token: 0x04000345 RID: 837
	public GorillaVelocityEstimator estimator;

	// Token: 0x04000346 RID: 838
	public CrittersGrabber grabber;

	// Token: 0x04000347 RID: 839
	public CrittersActorGrabber otherHand;

	// Token: 0x04000348 RID: 840
	private bool isHandGrabbingDisabled;

	// Token: 0x04000349 RID: 841
	private float grabDuration = 0.3f;

	// Token: 0x0400034A RID: 842
	private float remainingGrabDuration;
}
