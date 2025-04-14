using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005BE RID: 1470
public class FreeHoverboardHandle : HoldableObject
{
	// Token: 0x06002481 RID: 9345 RVA: 0x000B6078 File Offset: 0x000B4278
	private void Awake()
	{
		this.hasParentBoard = (this.parentFreeBoard != null);
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x000B608C File Offset: 0x000B428C
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

	// Token: 0x06002483 RID: 9347 RVA: 0x000B60FC File Offset: 0x000B42FC
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		if (!GTPlayer.Instance.isHoverAllowed)
		{
			return;
		}
		bool flag = grabbingHand == EquipmentInteractor.instance.leftHand;
		if (this.hasParentBoard)
		{
			FreeHoverboardManager.instance.SendGrabBoardRPC(this.parentFreeBoard);
			Transform transform = flag ? VRRig.LocalRig.leftHand.rigTarget : VRRig.LocalRig.rightHand.rigTarget;
			Quaternion rot = transform.InverseTransformRotation(base.transform.rotation);
			Vector3 pos = transform.InverseTransformPoint(base.transform.position);
			GTPlayer.Instance.GrabPersonalHoverboard(flag, pos, rot, this.parentFreeBoard.boardColor);
			return;
		}
		Quaternion rot2 = flag ? this.defaultHoldAngleLeft : this.defaultHoldAngleRight;
		Vector3 pos2 = flag ? this.defaultHoldPosLeft : this.defaultHoldPosRight;
		GTPlayer.Instance.GrabPersonalHoverboard(flag, pos2, rot2, VRRig.LocalRig.playerColor);
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void DropItemCleanup()
	{
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x00002628 File Offset: 0x00000828
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		throw new NotImplementedException();
	}

	// Token: 0x040028A6 RID: 10406
	[SerializeField]
	private FreeHoverboardInstance parentFreeBoard;

	// Token: 0x040028A7 RID: 10407
	private bool hasParentBoard;

	// Token: 0x040028A8 RID: 10408
	[SerializeField]
	private Vector3 defaultHoldPosLeft;

	// Token: 0x040028A9 RID: 10409
	[SerializeField]
	private Vector3 defaultHoldPosRight;

	// Token: 0x040028AA RID: 10410
	[SerializeField]
	private Quaternion defaultHoldAngleLeft;

	// Token: 0x040028AB RID: 10411
	[SerializeField]
	private Quaternion defaultHoldAngleRight;

	// Token: 0x040028AC RID: 10412
	private int noHapticsUntilFrame = -1;
}
