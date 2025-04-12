using System;
using System.Collections.Generic;
using Fusion;
using GorillaGameModes;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200055F RID: 1375
public class GorillaGuardianManager : GorillaGameManager
{
	// Token: 0x17000374 RID: 884
	// (get) Token: 0x060021C4 RID: 8644 RVA: 0x00045F8F File Offset: 0x0004418F
	// (set) Token: 0x060021C5 RID: 8645 RVA: 0x00045F97 File Offset: 0x00044197
	public bool isPlaying { get; private set; }

	// Token: 0x060021C6 RID: 8646 RVA: 0x000F4428 File Offset: 0x000F2628
	public override void StartPlaying()
	{
		base.StartPlaying();
		this.isPlaying = true;
		if (PhotonNetwork.IsMasterClient)
		{
			foreach (GorillaGuardianZoneManager gorillaGuardianZoneManager in GorillaGuardianZoneManager.zoneManagers)
			{
				gorillaGuardianZoneManager.StartPlaying();
			}
		}
	}

	// Token: 0x060021C7 RID: 8647 RVA: 0x000F448C File Offset: 0x000F268C
	public override void StopPlaying()
	{
		base.StopPlaying();
		this.isPlaying = false;
		if (PhotonNetwork.IsMasterClient)
		{
			foreach (GorillaGuardianZoneManager gorillaGuardianZoneManager in GorillaGuardianZoneManager.zoneManagers)
			{
				gorillaGuardianZoneManager.StopPlaying();
			}
		}
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x00045FA0 File Offset: 0x000441A0
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x00045FA8 File Offset: 0x000441A8
	internal override void NetworkLinkSetup(GameModeSerializer netSerializer)
	{
		base.NetworkLinkSetup(netSerializer);
		netSerializer.AddRPCComponent<GuardianRPCs>();
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void AddFusionDataBehaviour(NetworkObject behaviour)
	{
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnSerializeRead(object newData)
	{
	}

	// Token: 0x060021CC RID: 8652 RVA: 0x00037F8B File Offset: 0x0003618B
	public override object OnSerializeWrite()
	{
		return null;
	}

	// Token: 0x060021CD RID: 8653 RVA: 0x00045FB8 File Offset: 0x000441B8
	public override bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		return this.IsPlayerGuardian(myPlayer) && !this.IsHoldingPlayer();
	}

	// Token: 0x060021CE RID: 8654 RVA: 0x00045FCE File Offset: 0x000441CE
	public override bool CanJoinFrienship(NetPlayer player)
	{
		return player != null && !this.IsPlayerGuardian(player);
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x000F44F0 File Offset: 0x000F26F0
	public bool IsPlayerGuardian(NetPlayer player)
	{
		using (List<GorillaGuardianZoneManager>.Enumerator enumerator = GorillaGuardianZoneManager.zoneManagers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsPlayerGuardian(player))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x00045FDF File Offset: 0x000441DF
	public void RequestEjectGuardian(NetPlayer player)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.EjectGuardian(player);
			return;
		}
		GorillaGameModes.GameMode.ActiveNetworkHandler.SendRPC("GuardianRequestEject", false, Array.Empty<object>());
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x000F454C File Offset: 0x000F274C
	public void EjectGuardian(NetPlayer player)
	{
		foreach (GorillaGuardianZoneManager gorillaGuardianZoneManager in GorillaGuardianZoneManager.zoneManagers)
		{
			if (gorillaGuardianZoneManager.IsPlayerGuardian(player))
			{
				gorillaGuardianZoneManager.SetGuardian(null);
			}
		}
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000F45A8 File Offset: 0x000F27A8
	public void LaunchPlayer(NetPlayer launcher, Vector3 velocity)
	{
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(launcher, out rigContainer))
		{
			return;
		}
		if (Vector3.Magnitude(VRRigCache.Instance.localRig.Rig.transform.position - rigContainer.Rig.transform.position) > this.requiredGuardianDistance + Mathf.Epsilon)
		{
			return;
		}
		if (velocity.sqrMagnitude > this.maxLaunchVelocity * this.maxLaunchVelocity)
		{
			return;
		}
		GTPlayer.Instance.DoLaunch(velocity);
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000F462C File Offset: 0x000F282C
	public override void LocalTag(NetPlayer taggedPlayer, NetPlayer taggingPlayer, bool bodyHit, bool leftHand)
	{
		base.LocalTag(taggedPlayer, taggingPlayer, bodyHit, leftHand);
		if (bodyHit)
		{
			return;
		}
		RigContainer rigContainer;
		Vector3 vector;
		if (VRRigCache.Instance.TryGetVrrig(taggedPlayer, out rigContainer) && this.CheckSlap(taggingPlayer, taggedPlayer, leftHand, out vector))
		{
			GorillaGameModes.GameMode.ActiveNetworkHandler.SendRPC("GuardianLaunchPlayer", taggedPlayer, new object[]
			{
				vector
			});
			rigContainer.Rig.ApplyLocalTrajectoryOverride(vector);
			GorillaGameModes.GameMode.ActiveNetworkHandler.SendRPC("ShowSlapEffects", true, new object[]
			{
				rigContainer.Rig.transform.position,
				vector.normalized
			});
			this.LocalPlaySlapEffect(rigContainer.Rig.transform.position, vector.normalized);
		}
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000F46F0 File Offset: 0x000F28F0
	private bool CheckSlap(NetPlayer slapper, NetPlayer target, bool leftHand, out Vector3 velocity)
	{
		velocity = Vector3.zero;
		if (this.IsHoldingPlayer(leftHand))
		{
			return false;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(slapper, out rigContainer))
		{
			return false;
		}
		Vector3 vector = (leftHand ? GTPlayer.Instance.leftHandCenterVelocityTracker : GTPlayer.Instance.rightHandCenterVelocityTracker).GetAverageVelocity(true, 0.15f, false);
		Vector3 rhs = leftHand ? rigContainer.Rig.leftHandHoldsPlayer.transform.right : rigContainer.Rig.rightHandHoldsPlayer.transform.right;
		if (Vector3.Dot(vector.normalized, rhs) < this.slapFrontAlignmentThreshold && Vector3.Dot(vector.normalized, rhs) > this.slapBackAlignmentThreshold)
		{
			return false;
		}
		if (vector.magnitude < this.launchMinimumStrength)
		{
			return false;
		}
		vector = Vector3.ClampMagnitude(vector, this.maxLaunchVelocity);
		RigContainer rigContainer2;
		if (!VRRigCache.Instance.TryGetVrrig(target, out rigContainer2))
		{
			return false;
		}
		if (this.IsRigBeingHeld(rigContainer2.Rig) || rigContainer2.Rig.IsLocalTrajectoryOverrideActive())
		{
			return false;
		}
		if (!this.CheckLaunchRetriggerDelay(rigContainer2.Rig))
		{
			return false;
		}
		vector *= this.launchStrengthMultiplier;
		Vector3 vector2;
		if (rigContainer2.Rig.IsOnGround(this.launchGroundHeadCheckDist, this.launchGroundHandCheckDist, out vector2))
		{
			vector += vector2 * this.launchGroundKickup * Mathf.Clamp01(1f - Vector3.Dot(vector2, vector.normalized));
		}
		velocity = vector;
		return true;
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x000F486C File Offset: 0x000F2A6C
	public override void HandleHandTap(NetPlayer tappingPlayer, Tappable hitTappable, bool leftHand, Vector3 handVelocity, Vector3 tapSurfaceNormal)
	{
		base.HandleHandTap(tappingPlayer, hitTappable, leftHand, handVelocity, tapSurfaceNormal);
		if (hitTappable != null)
		{
			TappableGuardianIdol tappableGuardianIdol = hitTappable as TappableGuardianIdol;
			if (tappableGuardianIdol != null && tappableGuardianIdol.isActivationReady)
			{
				tappableGuardianIdol.isActivationReady = false;
				GorillaTagger.Instance.StartVibration(leftHand, GorillaTagger.Instance.tapHapticStrength * this.hapticStrength, GorillaTagger.Instance.tapHapticDuration * this.hapticDuration);
			}
		}
		if (!this.IsPlayerGuardian(tappingPlayer))
		{
			return;
		}
		if (this.IsHoldingPlayer(leftHand))
		{
			return;
		}
		float num = Vector3.Dot(Vector3.down, handVelocity);
		if (num < this.slamTriggerTapSpeed || Vector3.Dot(Vector3.down, handVelocity.normalized) < this.slamTriggerAngle)
		{
			return;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(tappingPlayer, out rigContainer))
		{
			return;
		}
		VRMap vrmap = leftHand ? rigContainer.Rig.leftHand : rigContainer.Rig.rightHand;
		Vector3 b = vrmap.rigTarget.rotation * vrmap.trackingPositionOffset * rigContainer.Rig.scaleFactor;
		Vector3 vector = vrmap.rigTarget.position - b;
		float num2 = Mathf.Clamp01((num - this.slamTriggerTapSpeed) / (this.slamMaxTapSpeed - this.slamTriggerTapSpeed));
		num2 = Mathf.Lerp(this.slamMinStrengthMultiplier, this.slamMaxStrengthMultiplier, num2);
		for (int i = 0; i < RoomSystem.PlayersInRoom.Count; i++)
		{
			RigContainer rigContainer2;
			if (RoomSystem.PlayersInRoom[i] != tappingPlayer && VRRigCache.Instance.TryGetVrrig(RoomSystem.PlayersInRoom[i], out rigContainer2))
			{
				VRRig rig = rigContainer2.Rig;
				if (!this.IsRigBeingHeld(rig) && this.CheckLaunchRetriggerDelay(rig))
				{
					Vector3 position = rig.transform.position;
					if (Vector3.SqrMagnitude(position - vector) < this.slamRadius * this.slamRadius)
					{
						Vector3 vector2 = (position - vector).normalized * num2;
						vector2 = Vector3.ClampMagnitude(vector2, this.maxLaunchVelocity);
						GorillaGameModes.GameMode.ActiveNetworkHandler.SendRPC("GuardianLaunchPlayer", RoomSystem.PlayersInRoom[i], new object[]
						{
							vector2
						});
					}
				}
			}
		}
		this.LocalPlaySlamEffect(vector, Vector3.up);
		GorillaGameModes.GameMode.ActiveNetworkHandler.SendRPC("ShowSlamEffect", true, new object[]
		{
			vector,
			Vector3.up
		});
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x00046005 File Offset: 0x00044205
	private bool CheckLaunchRetriggerDelay(VRRig launchedRig)
	{
		return launchedRig.fxSettings.callSettings[7].CallLimitSettings.CheckCallTime(Time.time);
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x00046023 File Offset: 0x00044223
	private bool IsHoldingPlayer()
	{
		return this.IsHoldingPlayer(true) || this.IsHoldingPlayer(false);
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000F4AE0 File Offset: 0x000F2CE0
	private bool IsHoldingPlayer(bool leftHand)
	{
		return (leftHand && EquipmentInteractor.instance.leftHandHeldEquipment != null && EquipmentInteractor.instance.leftHandHeldEquipment is HoldableHand) || (!leftHand && EquipmentInteractor.instance.rightHandHeldEquipment != null && EquipmentInteractor.instance.rightHandHeldEquipment is HoldableHand);
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000F4B3C File Offset: 0x000F2D3C
	private bool IsRigBeingHeld(VRRig rig)
	{
		if (EquipmentInteractor.instance.leftHandHeldEquipment != null)
		{
			HoldableHand holdableHand = EquipmentInteractor.instance.leftHandHeldEquipment as HoldableHand;
			if (holdableHand != null && holdableHand.Rig == rig)
			{
				return true;
			}
		}
		if (EquipmentInteractor.instance.rightHandHeldEquipment != null)
		{
			HoldableHand holdableHand2 = EquipmentInteractor.instance.rightHandHeldEquipment as HoldableHand;
			if (holdableHand2 != null && holdableHand2.Rig == rig)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x00046037 File Offset: 0x00044237
	public override GameModeType GameType()
	{
		return GameModeType.Guardian;
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x0004603A File Offset: 0x0004423A
	public override string GameModeName()
	{
		return "GUARDIAN";
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x00046041 File Offset: 0x00044241
	public void PlaySlapEffect(Vector3 location, Vector3 direction)
	{
		this.LocalPlaySlapEffect(location, direction);
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x0004604B File Offset: 0x0004424B
	private void LocalPlaySlapEffect(Vector3 location, Vector3 direction)
	{
		ObjectPools.instance.Instantiate(this.slapImpactPrefab, location, Quaternion.LookRotation(direction));
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x00046065 File Offset: 0x00044265
	public void PlaySlamEffect(Vector3 location, Vector3 direction)
	{
		this.LocalPlaySlamEffect(location, direction);
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x0004606F File Offset: 0x0004426F
	private void LocalPlaySlamEffect(Vector3 location, Vector3 direction)
	{
		ObjectPools.instance.Instantiate(this.slamImpactPrefab, location, Quaternion.LookRotation(direction));
	}

	// Token: 0x04002559 RID: 9561
	[Space]
	[SerializeField]
	private float slapFrontAlignmentThreshold = 0.7f;

	// Token: 0x0400255A RID: 9562
	[SerializeField]
	private float slapBackAlignmentThreshold = 0.7f;

	// Token: 0x0400255B RID: 9563
	[SerializeField]
	private float launchMinimumStrength = 6f;

	// Token: 0x0400255C RID: 9564
	[SerializeField]
	private float launchStrengthMultiplier = 1f;

	// Token: 0x0400255D RID: 9565
	[SerializeField]
	private float launchGroundHeadCheckDist = 1.2f;

	// Token: 0x0400255E RID: 9566
	[SerializeField]
	private float launchGroundHandCheckDist = 0.4f;

	// Token: 0x0400255F RID: 9567
	[SerializeField]
	private float launchGroundKickup = 3f;

	// Token: 0x04002560 RID: 9568
	[Space]
	[SerializeField]
	private float slamTriggerTapSpeed = 7f;

	// Token: 0x04002561 RID: 9569
	[SerializeField]
	private float slamMaxTapSpeed = 16f;

	// Token: 0x04002562 RID: 9570
	[SerializeField]
	private float slamTriggerAngle = 0.7f;

	// Token: 0x04002563 RID: 9571
	[SerializeField]
	private float slamRadius = 2.4f;

	// Token: 0x04002564 RID: 9572
	[SerializeField]
	private float slamMinStrengthMultiplier = 3f;

	// Token: 0x04002565 RID: 9573
	[SerializeField]
	private float slamMaxStrengthMultiplier = 10f;

	// Token: 0x04002566 RID: 9574
	[Space]
	[SerializeField]
	private GameObject slapImpactPrefab;

	// Token: 0x04002567 RID: 9575
	[SerializeField]
	private GameObject slamImpactPrefab;

	// Token: 0x04002568 RID: 9576
	[Space]
	[SerializeField]
	private float hapticStrength = 1f;

	// Token: 0x04002569 RID: 9577
	[SerializeField]
	private float hapticDuration = 1f;

	// Token: 0x0400256B RID: 9579
	private float requiredGuardianDistance = 10f;

	// Token: 0x0400256C RID: 9580
	private float maxLaunchVelocity = 20f;
}
