using System;
using System.Collections.Generic;
using Fusion;
using GorillaGameModes;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200055E RID: 1374
public class GorillaGuardianManager : GorillaGameManager
{
	// Token: 0x17000373 RID: 883
	// (get) Token: 0x060021BC RID: 8636 RVA: 0x000A6E5F File Offset: 0x000A505F
	// (set) Token: 0x060021BD RID: 8637 RVA: 0x000A6E67 File Offset: 0x000A5067
	public bool isPlaying { get; private set; }

	// Token: 0x060021BE RID: 8638 RVA: 0x000A6E70 File Offset: 0x000A5070
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

	// Token: 0x060021BF RID: 8639 RVA: 0x000A6ED4 File Offset: 0x000A50D4
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

	// Token: 0x060021C0 RID: 8640 RVA: 0x000A6F38 File Offset: 0x000A5138
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x000A6F40 File Offset: 0x000A5140
	internal override void NetworkLinkSetup(GameModeSerializer netSerializer)
	{
		base.NetworkLinkSetup(netSerializer);
		netSerializer.AddRPCComponent<GuardianRPCs>();
	}

	// Token: 0x060021C2 RID: 8642 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void AddFusionDataBehaviour(NetworkObject behaviour)
	{
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnSerializeRead(object newData)
	{
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x00042E31 File Offset: 0x00041031
	public override object OnSerializeWrite()
	{
		return null;
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x000A6F50 File Offset: 0x000A5150
	public override bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		return this.IsPlayerGuardian(myPlayer) && !this.IsHoldingPlayer();
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x000A6F66 File Offset: 0x000A5166
	public override bool CanJoinFrienship(NetPlayer player)
	{
		return player != null && !this.IsPlayerGuardian(player);
	}

	// Token: 0x060021C7 RID: 8647 RVA: 0x000A6F78 File Offset: 0x000A5178
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

	// Token: 0x060021C8 RID: 8648 RVA: 0x000A6FD4 File Offset: 0x000A51D4
	public void RequestEjectGuardian(NetPlayer player)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.EjectGuardian(player);
			return;
		}
		GorillaGameModes.GameMode.ActiveNetworkHandler.SendRPC("GuardianRequestEject", false, Array.Empty<object>());
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x000A6FFC File Offset: 0x000A51FC
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

	// Token: 0x060021CA RID: 8650 RVA: 0x000A7058 File Offset: 0x000A5258
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

	// Token: 0x060021CB RID: 8651 RVA: 0x000A70DC File Offset: 0x000A52DC
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

	// Token: 0x060021CC RID: 8652 RVA: 0x000A71A0 File Offset: 0x000A53A0
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

	// Token: 0x060021CD RID: 8653 RVA: 0x000A731C File Offset: 0x000A551C
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

	// Token: 0x060021CE RID: 8654 RVA: 0x000A758E File Offset: 0x000A578E
	private bool CheckLaunchRetriggerDelay(VRRig launchedRig)
	{
		return launchedRig.fxSettings.callSettings[7].CallLimitSettings.CheckCallTime(Time.time);
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x000A75AC File Offset: 0x000A57AC
	private bool IsHoldingPlayer()
	{
		return this.IsHoldingPlayer(true) || this.IsHoldingPlayer(false);
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x000A75C0 File Offset: 0x000A57C0
	private bool IsHoldingPlayer(bool leftHand)
	{
		return (leftHand && EquipmentInteractor.instance.leftHandHeldEquipment != null && EquipmentInteractor.instance.leftHandHeldEquipment is HoldableHand) || (!leftHand && EquipmentInteractor.instance.rightHandHeldEquipment != null && EquipmentInteractor.instance.rightHandHeldEquipment is HoldableHand);
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x000A761C File Offset: 0x000A581C
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

	// Token: 0x060021D2 RID: 8658 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000A7690 File Offset: 0x000A5890
	public override GameModeType GameType()
	{
		return GameModeType.Guardian;
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x000A7693 File Offset: 0x000A5893
	public override string GameModeName()
	{
		return "GUARDIAN";
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x000A769A File Offset: 0x000A589A
	public void PlaySlapEffect(Vector3 location, Vector3 direction)
	{
		this.LocalPlaySlapEffect(location, direction);
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000A76A4 File Offset: 0x000A58A4
	private void LocalPlaySlapEffect(Vector3 location, Vector3 direction)
	{
		ObjectPools.instance.Instantiate(this.slapImpactPrefab, location, Quaternion.LookRotation(direction));
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000A76BE File Offset: 0x000A58BE
	public void PlaySlamEffect(Vector3 location, Vector3 direction)
	{
		this.LocalPlaySlamEffect(location, direction);
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000A76C8 File Offset: 0x000A58C8
	private void LocalPlaySlamEffect(Vector3 location, Vector3 direction)
	{
		ObjectPools.instance.Instantiate(this.slamImpactPrefab, location, Quaternion.LookRotation(direction));
	}

	// Token: 0x04002553 RID: 9555
	[Space]
	[SerializeField]
	private float slapFrontAlignmentThreshold = 0.7f;

	// Token: 0x04002554 RID: 9556
	[SerializeField]
	private float slapBackAlignmentThreshold = 0.7f;

	// Token: 0x04002555 RID: 9557
	[SerializeField]
	private float launchMinimumStrength = 6f;

	// Token: 0x04002556 RID: 9558
	[SerializeField]
	private float launchStrengthMultiplier = 1f;

	// Token: 0x04002557 RID: 9559
	[SerializeField]
	private float launchGroundHeadCheckDist = 1.2f;

	// Token: 0x04002558 RID: 9560
	[SerializeField]
	private float launchGroundHandCheckDist = 0.4f;

	// Token: 0x04002559 RID: 9561
	[SerializeField]
	private float launchGroundKickup = 3f;

	// Token: 0x0400255A RID: 9562
	[Space]
	[SerializeField]
	private float slamTriggerTapSpeed = 7f;

	// Token: 0x0400255B RID: 9563
	[SerializeField]
	private float slamMaxTapSpeed = 16f;

	// Token: 0x0400255C RID: 9564
	[SerializeField]
	private float slamTriggerAngle = 0.7f;

	// Token: 0x0400255D RID: 9565
	[SerializeField]
	private float slamRadius = 2.4f;

	// Token: 0x0400255E RID: 9566
	[SerializeField]
	private float slamMinStrengthMultiplier = 3f;

	// Token: 0x0400255F RID: 9567
	[SerializeField]
	private float slamMaxStrengthMultiplier = 10f;

	// Token: 0x04002560 RID: 9568
	[Space]
	[SerializeField]
	private GameObject slapImpactPrefab;

	// Token: 0x04002561 RID: 9569
	[SerializeField]
	private GameObject slamImpactPrefab;

	// Token: 0x04002562 RID: 9570
	[Space]
	[SerializeField]
	private float hapticStrength = 1f;

	// Token: 0x04002563 RID: 9571
	[SerializeField]
	private float hapticDuration = 1f;

	// Token: 0x04002565 RID: 9573
	private float requiredGuardianDistance = 10f;

	// Token: 0x04002566 RID: 9574
	private float maxLaunchVelocity = 20f;
}
