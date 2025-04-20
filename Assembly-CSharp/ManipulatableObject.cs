using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
public class ManipulatableObject : HoldableObject
{
	// Token: 0x060018B1 RID: 6321 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnStartManipulation(GameObject grabbingHand)
	{
	}

	// Token: 0x060018B2 RID: 6322 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnStopManipulation(GameObject releasingHand, Vector3 releaseVelocity)
	{
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x00030498 File Offset: 0x0002E698
	protected virtual bool ShouldHandDetach(GameObject hand)
	{
		return false;
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnHeldUpdate(GameObject hand)
	{
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnReleasedUpdate()
	{
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x000CCED0 File Offset: 0x000CB0D0
	public virtual void LateUpdate()
	{
		if (this.isHeld)
		{
			if (this.holdingHand == null)
			{
				EquipmentInteractor.instance.ForceDropManipulatableObject(this);
				return;
			}
			this.OnHeldUpdate(this.holdingHand);
			if (this.ShouldHandDetach(this.holdingHand))
			{
				EquipmentInteractor.instance.ForceDropManipulatableObject(this);
				return;
			}
		}
		else
		{
			this.OnReleasedUpdate();
		}
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x000CCF30 File Offset: 0x000CB130
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		bool forLeftHand = grabbingHand == EquipmentInteractor.instance.leftHand;
		EquipmentInteractor.instance.UpdateHandEquipment(this, forLeftHand);
		this.isHeld = true;
		this.holdingHand = grabbingHand;
		this.OnStartManipulation(this.holdingHand);
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x000CCF78 File Offset: 0x000CB178
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		bool flag = releasingHand == EquipmentInteractor.instance.leftHand;
		Vector3 releaseVelocity = Vector3.zero;
		if (flag)
		{
			releaseVelocity = GTPlayer.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0.15f, false);
			EquipmentInteractor.instance.leftHandHeldEquipment = null;
		}
		else
		{
			releaseVelocity = GTPlayer.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0.15f, false);
			EquipmentInteractor.instance.rightHandHeldEquipment = null;
		}
		this.isHeld = false;
		this.holdingHand = null;
		this.OnStopManipulation(releasingHand, releaseVelocity);
		return true;
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x00030607 File Offset: 0x0002E807
	public override void DropItemCleanup()
	{
	}

	// Token: 0x04001B51 RID: 6993
	protected bool isHeld;

	// Token: 0x04001B52 RID: 6994
	protected GameObject holdingHand;
}
