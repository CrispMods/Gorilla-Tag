using System;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class DebugTestGrabber : MonoBehaviour
{
	// Token: 0x06000293 RID: 659 RVA: 0x0003105C File Offset: 0x0002F25C
	private void Awake()
	{
		if (this.grabber == null)
		{
			this.grabber = base.GetComponentInChildren<CrittersGrabber>();
		}
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00073344 File Offset: 0x00071544
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

	// Token: 0x06000295 RID: 661 RVA: 0x00073418 File Offset: 0x00071618
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

	// Token: 0x06000296 RID: 662 RVA: 0x000734FC File Offset: 0x000716FC
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

	// Token: 0x0400033F RID: 831
	public bool isGrabbing;

	// Token: 0x04000340 RID: 832
	public bool setIsGrabbing;

	// Token: 0x04000341 RID: 833
	public bool setRelease;

	// Token: 0x04000342 RID: 834
	public Collider[] colliders = new Collider[50];

	// Token: 0x04000343 RID: 835
	public bool isLeft;

	// Token: 0x04000344 RID: 836
	public float grabRadius = 0.05f;

	// Token: 0x04000345 RID: 837
	public Transform transformToFollow;

	// Token: 0x04000346 RID: 838
	public GorillaVelocityEstimator estimator;

	// Token: 0x04000347 RID: 839
	public CrittersGrabber grabber;

	// Token: 0x04000348 RID: 840
	public CrittersActorGrabber otherHand;

	// Token: 0x04000349 RID: 841
	private bool isHandGrabbingDisabled;

	// Token: 0x0400034A RID: 842
	private float grabDuration = 0.3f;

	// Token: 0x0400034B RID: 843
	private float remainingGrabDuration;
}
