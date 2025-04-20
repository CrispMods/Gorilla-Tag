using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005D2 RID: 1490
public class HoverboardHandle : HoldableObject
{
	// Token: 0x06002505 RID: 9477 RVA: 0x00104E4C File Offset: 0x0010304C
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
		if (!GTPlayer.Instance.isHoverAllowed)
		{
			return;
		}
		if (Time.frameCount > this.noHapticsUntilFrame)
		{
			GorillaTagger.Instance.StartVibration(hoveringHand == EquipmentInteractor.instance.leftHand, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
		}
		this.noHapticsUntilFrame = Time.frameCount + 1;
	}

	// Token: 0x06002506 RID: 9478 RVA: 0x00104EBC File Offset: 0x001030BC
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		if (!GTPlayer.Instance.isHoverAllowed)
		{
			return;
		}
		bool flag = grabbingHand == EquipmentInteractor.instance.leftHand;
		Transform transform = flag ? VRRig.LocalRig.leftHand.rigTarget : VRRig.LocalRig.rightHand.rigTarget;
		Quaternion localRotation;
		Vector3 localPosition;
		if (!this.parentVisual.IsHeld)
		{
			localRotation = (flag ? this.defaultHoldAngleLeft : this.defaultHoldAngleRight);
			localPosition = (flag ? this.defaultHoldPosLeft : this.defaultHoldPosRight);
		}
		else
		{
			localRotation = transform.InverseTransformRotation(this.parentVisual.transform.rotation);
			localPosition = transform.InverseTransformPoint(this.parentVisual.transform.position);
		}
		this.parentVisual.SetIsHeld(flag, localPosition, localRotation, this.parentVisual.boardColor);
		EquipmentInteractor.instance.UpdateHandEquipment(this, flag);
	}

	// Token: 0x06002507 RID: 9479 RVA: 0x00049143 File Offset: 0x00047343
	public override void DropItemCleanup()
	{
		if (this.parentVisual.gameObject.activeSelf)
		{
			this.parentVisual.DropFreeBoard();
		}
		this.parentVisual.SetNotHeld();
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x00104F98 File Offset: 0x00103198
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (EquipmentInteractor.instance.rightHandHeldEquipment == this && releasingHand != EquipmentInteractor.instance.rightHand)
		{
			return false;
		}
		if (EquipmentInteractor.instance.leftHandHeldEquipment == this && releasingHand != EquipmentInteractor.instance.leftHand)
		{
			return false;
		}
		EquipmentInteractor.instance.UpdateHandEquipment(null, this.parentVisual.IsLeftHanded);
		this.parentVisual.SetNotHeld();
		return true;
	}

	// Token: 0x0400292A RID: 10538
	[SerializeField]
	private HoverboardVisual parentVisual;

	// Token: 0x0400292B RID: 10539
	[SerializeField]
	private Quaternion defaultHoldAngleLeft;

	// Token: 0x0400292C RID: 10540
	[SerializeField]
	private Quaternion defaultHoldAngleRight;

	// Token: 0x0400292D RID: 10541
	[SerializeField]
	private Vector3 defaultHoldPosLeft;

	// Token: 0x0400292E RID: 10542
	[SerializeField]
	private Vector3 defaultHoldPosRight;

	// Token: 0x0400292F RID: 10543
	private int noHapticsUntilFrame = -1;
}
