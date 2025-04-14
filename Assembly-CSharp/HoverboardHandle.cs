using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005C4 RID: 1476
public class HoverboardHandle : HoldableObject
{
	// Token: 0x060024A3 RID: 9379 RVA: 0x000B6750 File Offset: 0x000B4950
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

	// Token: 0x060024A4 RID: 9380 RVA: 0x000B67C0 File Offset: 0x000B49C0
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

	// Token: 0x060024A5 RID: 9381 RVA: 0x000B6899 File Offset: 0x000B4A99
	public override void DropItemCleanup()
	{
		if (this.parentVisual.gameObject.activeSelf)
		{
			this.parentVisual.DropFreeBoard();
		}
		this.parentVisual.SetNotHeld();
	}

	// Token: 0x060024A6 RID: 9382 RVA: 0x000B68C4 File Offset: 0x000B4AC4
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

	// Token: 0x040028CB RID: 10443
	[SerializeField]
	private HoverboardVisual parentVisual;

	// Token: 0x040028CC RID: 10444
	[SerializeField]
	private Quaternion defaultHoldAngleLeft;

	// Token: 0x040028CD RID: 10445
	[SerializeField]
	private Quaternion defaultHoldAngleRight;

	// Token: 0x040028CE RID: 10446
	[SerializeField]
	private Vector3 defaultHoldPosLeft;

	// Token: 0x040028CF RID: 10447
	[SerializeField]
	private Vector3 defaultHoldPosRight;

	// Token: 0x040028D0 RID: 10448
	private int noHapticsUntilFrame = -1;
}
