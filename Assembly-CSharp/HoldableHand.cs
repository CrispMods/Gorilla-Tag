using System;
using GorillaGameModes;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005CA RID: 1482
public class HoldableHand : HoldableObject
{
	// Token: 0x170003BD RID: 957
	// (get) Token: 0x060024D3 RID: 9427 RVA: 0x00048FBE File Offset: 0x000471BE
	public VRRig Rig
	{
		get
		{
			return this.myPlayer;
		}
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x00048FC6 File Offset: 0x000471C6
	private void Start()
	{
		if (this.myPlayer.isOfflineVRRig)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060024D5 RID: 9429 RVA: 0x001041BC File Offset: 0x001023BC
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

	// Token: 0x060024D6 RID: 9430 RVA: 0x00104294 File Offset: 0x00102494
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

	// Token: 0x060024D7 RID: 9431 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x060024D8 RID: 9432 RVA: 0x00048FE1 File Offset: 0x000471E1
	public override void DropItemCleanup()
	{
		this.myPlayer.ClearLocalGrabOverride();
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x00104388 File Offset: 0x00102588
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

	// Token: 0x040028FC RID: 10492
	[SerializeField]
	private VRRig myPlayer;

	// Token: 0x040028FD RID: 10493
	[SerializeField]
	private bool isBody;

	// Token: 0x040028FE RID: 10494
	[SerializeField]
	private bool isLeftHand;
}
