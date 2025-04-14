using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
public class ManipulatableObject : HoldableObject
{
	// Token: 0x06001867 RID: 6247 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnStartManipulation(GameObject grabbingHand)
	{
	}

	// Token: 0x06001868 RID: 6248 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnStopManipulation(GameObject releasingHand, Vector3 releaseVelocity)
	{
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x00002076 File Offset: 0x00000276
	protected virtual bool ShouldHandDetach(GameObject hand)
	{
		return false;
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnHeldUpdate(GameObject hand)
	{
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnReleasedUpdate()
	{
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x00076D54 File Offset: 0x00074F54
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

	// Token: 0x0600186D RID: 6253 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x00076DB4 File Offset: 0x00074FB4
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		bool forLeftHand = grabbingHand == EquipmentInteractor.instance.leftHand;
		EquipmentInteractor.instance.UpdateHandEquipment(this, forLeftHand);
		this.isHeld = true;
		this.holdingHand = grabbingHand;
		this.OnStartManipulation(this.holdingHand);
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x00076DFC File Offset: 0x00074FFC
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

	// Token: 0x06001870 RID: 6256 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void DropItemCleanup()
	{
	}

	// Token: 0x04001B09 RID: 6921
	protected bool isHeld;

	// Token: 0x04001B0A RID: 6922
	protected GameObject holdingHand;
}
