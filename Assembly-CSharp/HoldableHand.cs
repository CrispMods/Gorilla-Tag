using System;
using GorillaGameModes;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005BC RID: 1468
public class HoldableHand : HoldableObject
{
	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x06002471 RID: 9329 RVA: 0x000B5938 File Offset: 0x000B3B38
	public VRRig Rig
	{
		get
		{
			return this.myPlayer;
		}
	}

	// Token: 0x06002472 RID: 9330 RVA: 0x000B5940 File Offset: 0x000B3B40
	private void Start()
	{
		if (this.myPlayer.isOfflineVRRig)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002473 RID: 9331 RVA: 0x000B595C File Offset: 0x000B3B5C
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		GorillaGuardianManager gorillaGuardianManager = GameMode.ActiveGameMode as GorillaGuardianManager;
		if (gorillaGuardianManager != null && !this.myPlayer.creator.IsLocal && gorillaGuardianManager.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
		{
			bool flag = grabbingHand == EquipmentInteractor.instance.leftHand;
			this.myPlayer.netView.SendRPC("GrabbedByPlayer", this.myPlayer.Creator, new object[]
			{
				this.isBody,
				this.isLeftHand,
				flag
			});
			this.myPlayer.ApplyLocalGrabOverride(this.isBody, this.isLeftHand, grabbingHand.transform);
			EquipmentInteractor.instance.UpdateHandEquipment(this, flag);
			this.ClearOtherGrabs(flag);
		}
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000B5A34 File Offset: 0x000B3C34
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		GorillaGuardianManager gorillaGuardianManager = GameMode.ActiveGameMode as GorillaGuardianManager;
		if (gorillaGuardianManager != null && !this.myPlayer.creator.IsLocal)
		{
			bool flag = releasingHand == EquipmentInteractor.instance.leftHand;
			Vector3 vector = Vector3.zero;
			if (gorillaGuardianManager.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
			{
				vector = (flag ? GTPlayer.Instance.leftHandCenterVelocityTracker : GTPlayer.Instance.rightHandCenterVelocityTracker).GetAverageVelocity(true, 0.15f, false);
			}
			vector = Vector3.ClampMagnitude(vector, 20f);
			this.myPlayer.netView.SendRPC("DroppedByPlayer", this.myPlayer.Creator, new object[]
			{
				vector
			});
			this.myPlayer.ClearLocalGrabOverride();
			this.myPlayer.ApplyLocalTrajectoryOverride(vector);
			EquipmentInteractor.instance.UpdateHandEquipment(null, flag);
		}
		return true;
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x000B5B25 File Offset: 0x000B3D25
	public override void DropItemCleanup()
	{
		this.myPlayer.ClearLocalGrabOverride();
	}

	// Token: 0x06002477 RID: 9335 RVA: 0x000B5B34 File Offset: 0x000B3D34
	private void ClearOtherGrabs(bool grabbedLeft)
	{
		IHoldableObject holdableObject = grabbedLeft ? EquipmentInteractor.instance.rightHandHeldEquipment : EquipmentInteractor.instance.leftHandHeldEquipment;
		if (this.isBody)
		{
			if (holdableObject == this.myPlayer.leftHolds || holdableObject == this.myPlayer.rightHolds)
			{
				EquipmentInteractor.instance.UpdateHandEquipment(null, !grabbedLeft);
				return;
			}
		}
		else if (this.isLeftHand)
		{
			if (holdableObject == this.myPlayer.rightHolds || holdableObject == this.myPlayer.bodyHolds)
			{
				EquipmentInteractor.instance.UpdateHandEquipment(null, !grabbedLeft);
				return;
			}
		}
		else if (holdableObject == this.myPlayer.leftHolds || holdableObject == this.myPlayer.bodyHolds)
		{
			EquipmentInteractor.instance.UpdateHandEquipment(null, !grabbedLeft);
		}
	}

	// Token: 0x0400289D RID: 10397
	[SerializeField]
	private VRRig myPlayer;

	// Token: 0x0400289E RID: 10398
	[SerializeField]
	private bool isBody;

	// Token: 0x0400289F RID: 10399
	[SerializeField]
	private bool isLeftHand;
}
