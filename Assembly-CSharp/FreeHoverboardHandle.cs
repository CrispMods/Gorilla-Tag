using System;
using GorillaExtensions;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005CB RID: 1483
public class FreeHoverboardHandle : HoldableObject
{
	// Token: 0x060024DB RID: 9435 RVA: 0x00048FEE File Offset: 0x000471EE
	private void Awake()
	{
		this.hasParentBoard = (this.parentFreeBoard != null);
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x0010444C File Offset: 0x0010264C
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

	// Token: 0x060024DD RID: 9437 RVA: 0x001044BC File Offset: 0x001026BC
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

	// Token: 0x060024DE RID: 9438 RVA: 0x00030607 File Offset: 0x0002E807
	public override void DropItemCleanup()
	{
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x000306DC File Offset: 0x0002E8DC
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		throw new NotImplementedException();
	}

	// Token: 0x040028FF RID: 10495
	[SerializeField]
	private FreeHoverboardInstance parentFreeBoard;

	// Token: 0x04002900 RID: 10496
	private bool hasParentBoard;

	// Token: 0x04002901 RID: 10497
	[SerializeField]
	private Vector3 defaultHoldPosLeft;

	// Token: 0x04002902 RID: 10498
	[SerializeField]
	private Vector3 defaultHoldPosRight;

	// Token: 0x04002903 RID: 10499
	[SerializeField]
	private Quaternion defaultHoldAngleLeft;

	// Token: 0x04002904 RID: 10500
	[SerializeField]
	private Quaternion defaultHoldAngleRight;

	// Token: 0x04002905 RID: 10501
	private int noHapticsUntilFrame = -1;
}
