using System;
using System.Collections.Generic;
using Fusion;
using GorillaGameModes;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200056C RID: 1388
public class GorillaGuardianManager : GorillaGameManager
{
	// Token: 0x1700037B RID: 891
	// (get) Token: 0x0600221A RID: 8730 RVA: 0x00047334 File Offset: 0x00045534
	// (set) Token: 0x0600221B RID: 8731 RVA: 0x0004733C File Offset: 0x0004553C
	public bool isPlaying { get; private set; }

	// Token: 0x0600221C RID: 8732 RVA: 0x000F71A4 File Offset: 0x000F53A4
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

	// Token: 0x0600221D RID: 8733 RVA: 0x000F7208 File Offset: 0x000F5408
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

	// Token: 0x0600221E RID: 8734 RVA: 0x00047345 File Offset: 0x00045545
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x0004734D File Offset: 0x0004554D
	internal override void NetworkLinkSetup(GameModeSerializer netSerializer)
	{
		base.NetworkLinkSetup(netSerializer);
		netSerializer.AddRPCComponent<GuardianRPCs>();
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x00030607 File Offset: 0x0002E807
	public override void AddFusionDataBehaviour(NetworkObject behaviour)
	{
	}

	// Token: 0x06002221 RID: 8737 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnSerializeRead(object newData)
	{
	}

	// Token: 0x06002222 RID: 8738 RVA: 0x0003924B File Offset: 0x0003744B
	public override object OnSerializeWrite()
	{
		return null;
	}

	// Token: 0x06002223 RID: 8739 RVA: 0x0004735D File Offset: 0x0004555D
	public override bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		return this.IsPlayerGuardian(myPlayer) && !this.IsHoldingPlayer();
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x00047373 File Offset: 0x00045573
	public override bool CanJoinFrienship(NetPlayer player)
	{
		return player != null && !this.IsPlayerGuardian(player);
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x000F726C File Offset: 0x000F546C
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

	// Token: 0x06002226 RID: 8742 RVA: 0x00047384 File Offset: 0x00045584
	public void RequestEjectGuardian(NetPlayer player)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.EjectGuardian(player);
			return;
		}
		GorillaGameModes.GameMode.ActiveNetworkHandler.SendRPC("GuardianRequestEject", false, Array.Empty<object>());
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x000F72C8 File Offset: 0x000F54C8
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

	// Token: 0x06002228 RID: 8744 RVA: 0x000F7324 File Offset: 0x000F5524
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

	// Token: 0x06002229 RID: 8745 RVA: 0x000F73A8 File Offset: 0x000F55A8
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

	// Token: 0x0600222A RID: 8746 RVA: 0x000F746C File Offset: 0x000F566C
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

	// Token: 0x0600222B RID: 8747 RVA: 0x000F75E8 File Offset: 0x000F57E8
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

	// Token: 0x0600222C RID: 8748 RVA: 0x000473AA File Offset: 0x000455AA
	private bool CheckLaunchRetriggerDelay(VRRig launchedRig)
	{
		return launchedRig.fxSettings.callSettings[7].CallLimitSettings.CheckCallTime(Time.time);
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x000473C8 File Offset: 0x000455C8
	private bool IsHoldingPlayer()
	{
		return this.IsHoldingPlayer(true) || this.IsHoldingPlayer(false);
	}

	// Token: 0x0600222E RID: 8750 RVA: 0x000F785C File Offset: 0x000F5A5C
	private bool IsHoldingPlayer(bool leftHand)
	{
		return (leftHand && EquipmentInteractor.instance.leftHandHeldEquipment != null && EquipmentInteractor.instance.leftHandHeldEquipment is HoldableHand) || (!leftHand && EquipmentInteractor.instance.rightHandHeldEquipment != null && EquipmentInteractor.instance.rightHandHeldEquipment is HoldableHand);
	}

	// Token: 0x0600222F RID: 8751 RVA: 0x000F78B8 File Offset: 0x000F5AB8
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

	// Token: 0x06002230 RID: 8752 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x000473DC File Offset: 0x000455DC
	public override GameModeType GameType()
	{
		return GameModeType.Guardian;
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x000473DF File Offset: 0x000455DF
	public override string GameModeName()
	{
		return "GUARDIAN";
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000473E6 File Offset: 0x000455E6
	public void PlaySlapEffect(Vector3 location, Vector3 direction)
	{
		this.LocalPlaySlapEffect(location, direction);
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x000473F0 File Offset: 0x000455F0
	private void LocalPlaySlapEffect(Vector3 location, Vector3 direction)
	{
		ObjectPools.instance.Instantiate(this.slapImpactPrefab, location, Quaternion.LookRotation(direction));
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x0004740A File Offset: 0x0004560A
	public void PlaySlamEffect(Vector3 location, Vector3 direction)
	{
		this.LocalPlaySlamEffect(location, direction);
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x00047414 File Offset: 0x00045614
	private void LocalPlaySlamEffect(Vector3 location, Vector3 direction)
	{
		ObjectPools.instance.Instantiate(this.slamImpactPrefab, location, Quaternion.LookRotation(direction));
	}

	// Token: 0x040025AB RID: 9643
	[Space]
	[SerializeField]
	private float slapFrontAlignmentThreshold = 0.7f;

	// Token: 0x040025AC RID: 9644
	[SerializeField]
	private float slapBackAlignmentThreshold = 0.7f;

	// Token: 0x040025AD RID: 9645
	[SerializeField]
	private float launchMinimumStrength = 6f;

	// Token: 0x040025AE RID: 9646
	[SerializeField]
	private float launchStrengthMultiplier = 1f;

	// Token: 0x040025AF RID: 9647
	[SerializeField]
	private float launchGroundHeadCheckDist = 1.2f;

	// Token: 0x040025B0 RID: 9648
	[SerializeField]
	private float launchGroundHandCheckDist = 0.4f;

	// Token: 0x040025B1 RID: 9649
	[SerializeField]
	private float launchGroundKickup = 3f;

	// Token: 0x040025B2 RID: 9650
	[Space]
	[SerializeField]
	private float slamTriggerTapSpeed = 7f;

	// Token: 0x040025B3 RID: 9651
	[SerializeField]
	private float slamMaxTapSpeed = 16f;

	// Token: 0x040025B4 RID: 9652
	[SerializeField]
	private float slamTriggerAngle = 0.7f;

	// Token: 0x040025B5 RID: 9653
	[SerializeField]
	private float slamRadius = 2.4f;

	// Token: 0x040025B6 RID: 9654
	[SerializeField]
	private float slamMinStrengthMultiplier = 3f;

	// Token: 0x040025B7 RID: 9655
	[SerializeField]
	private float slamMaxStrengthMultiplier = 10f;

	// Token: 0x040025B8 RID: 9656
	[Space]
	[SerializeField]
	private GameObject slapImpactPrefab;

	// Token: 0x040025B9 RID: 9657
	[SerializeField]
	private GameObject slamImpactPrefab;

	// Token: 0x040025BA RID: 9658
	[Space]
	[SerializeField]
	private float hapticStrength = 1f;

	// Token: 0x040025BB RID: 9659
	[SerializeField]
	private float hapticDuration = 1f;

	// Token: 0x040025BD RID: 9661
	private float requiredGuardianDistance = 10f;

	// Token: 0x040025BE RID: 9662
	private float maxLaunchVelocity = 20f;
}
