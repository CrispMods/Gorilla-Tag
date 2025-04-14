using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005BD RID: 1469
public class FreeHoverboardHandle : HoldableObject
{
	// Token: 0x06002479 RID: 9337 RVA: 0x000B5BF8 File Offset: 0x000B3DF8
	private void Awake()
	{
		this.hasParentBoard = (this.parentFreeBoard != null);
	}

	// Token: 0x0600247A RID: 9338 RVA: 0x000B5C0C File Offset: 0x000B3E0C
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

	// Token: 0x0600247B RID: 9339 RVA: 0x000B5C7C File Offset: 0x000B3E7C
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

	// Token: 0x0600247C RID: 9340 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void DropItemCleanup()
	{
	}

	// Token: 0x0600247D RID: 9341 RVA: 0x00002628 File Offset: 0x00000828
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		throw new NotImplementedException();
	}

	// Token: 0x040028A0 RID: 10400
	[SerializeField]
	private FreeHoverboardInstance parentFreeBoard;

	// Token: 0x040028A1 RID: 10401
	private bool hasParentBoard;

	// Token: 0x040028A2 RID: 10402
	[SerializeField]
	private Vector3 defaultHoldPosLeft;

	// Token: 0x040028A3 RID: 10403
	[SerializeField]
	private Vector3 defaultHoldPosRight;

	// Token: 0x040028A4 RID: 10404
	[SerializeField]
	private Quaternion defaultHoldAngleLeft;

	// Token: 0x040028A5 RID: 10405
	[SerializeField]
	private Quaternion defaultHoldAngleRight;

	// Token: 0x040028A6 RID: 10406
	private int noHapticsUntilFrame = -1;
}
