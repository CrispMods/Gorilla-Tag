﻿using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200049C RID: 1180
[NetworkBehaviourWeaved(0)]
public class GameBallManager : NetworkComponent
{
	// Token: 0x06001C86 RID: 7302 RVA: 0x000DC524 File Offset: 0x000DA724
	protected override void Awake()
	{
		base.Awake();
		GameBallManager.Instance = this;
		this.gameBalls = new List<GameBall>(64);
		this.gameBallData = new List<GameBallData>(64);
		this._callLimiters = new CallLimiter[8];
		this._callLimiters[0] = new CallLimiter(50, 1f, 0.5f);
		this._callLimiters[1] = new CallLimiter(50, 1f, 0.5f);
		this._callLimiters[2] = new CallLimiter(25, 1f, 0.5f);
		this._callLimiters[3] = new CallLimiter(25, 1f, 0.5f);
		this._callLimiters[4] = new CallLimiter(10, 1f, 0.5f);
		this._callLimiters[5] = new CallLimiter(10, 1f, 0.5f);
		this._callLimiters[6] = new CallLimiter(10, 1f, 0.5f);
		this._callLimiters[7] = new CallLimiter(25, 1f, 0.5f);
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x000DC630 File Offset: 0x000DA830
	private bool ValidateCallLimits(GameBallManager.RPC rpcCall, PhotonMessageInfo info)
	{
		if (rpcCall < GameBallManager.RPC.RequestGrabBall || rpcCall >= GameBallManager.RPC.Count)
		{
			return false;
		}
		bool flag = this._callLimiters[(int)rpcCall].CheckCallTime(Time.time);
		if (!flag)
		{
			this.ReportRPCCall(rpcCall, info, "Too many RPC Calls!");
		}
		return flag;
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x00043B26 File Offset: 0x00041D26
	private void ReportRPCCall(GameBallManager.RPC rpcCall, PhotonMessageInfo info, string susReason)
	{
		GorillaNot.instance.SendReport(string.Format("Reason: {0}   RPC: {1}", susReason, rpcCall), info.Sender.UserId, info.Sender.NickName);
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x000DC66C File Offset: 0x000DA86C
	public GameBallId AddGameBall(GameBall gameBall)
	{
		int count = this.gameBallData.Count;
		this.gameBalls.Add(gameBall);
		GameBallData item = default(GameBallData);
		this.gameBallData.Add(item);
		gameBall.id = new GameBallId(count);
		return gameBall.id;
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x000DC6B8 File Offset: 0x000DA8B8
	public GameBall GetGameBall(GameBallId id)
	{
		if (!id.IsValid())
		{
			return null;
		}
		int index = id.index;
		return this.gameBalls[index];
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x000DC6E4 File Offset: 0x000DA8E4
	public GameBallId TryGrabLocal(Vector3 handPosition, int teamId)
	{
		int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
		GameBallId result = GameBallId.Invalid;
		float num = float.MaxValue;
		for (int i = 0; i < this.gameBalls.Count; i++)
		{
			if (this.gameBalls[i].onlyGrabTeamId == -1 || this.gameBalls[i].onlyGrabTeamId == teamId)
			{
				float sqrMagnitude = this.gameBalls[i].GetVelocity().sqrMagnitude;
				double num2 = 0.0625;
				if (sqrMagnitude > 2f)
				{
					num2 = 0.25;
				}
				float sqrMagnitude2 = (handPosition - this.gameBalls[i].transform.position).sqrMagnitude;
				if ((double)sqrMagnitude2 < num2 && sqrMagnitude2 < num)
				{
					result = this.gameBalls[i].id;
					num = sqrMagnitude2;
				}
			}
		}
		return result;
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x000DC7CC File Offset: 0x000DA9CC
	public void RequestGrabBall(GameBallId ballId, bool isLeftHand, Vector3 localPosition, Quaternion localRotation)
	{
		this.GrabBall(ballId, isLeftHand, localPosition, localRotation, NetPlayer.Get(PhotonNetwork.LocalPlayer));
		long num = BitPackUtils.PackHandPosRotForNetwork(localPosition, localRotation);
		this.photonView.RPC("RequestGrabBallRPC", RpcTarget.MasterClient, new object[]
		{
			ballId.index,
			isLeftHand,
			num
		});
		PhotonNetwork.SendAllOutgoingCommands();
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x000DC834 File Offset: 0x000DAA34
	[PunRPC]
	private void RequestGrabBallRPC(int gameBallIndex, bool isLeftHand, long packedPosRot, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestGrabBallRPC");
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (!this.ValidateCallLimits(GameBallManager.RPC.RequestGrabBall, info))
		{
			return;
		}
		if (gameBallIndex < 0 || gameBallIndex > this.gameBalls.Count)
		{
			this.ReportRPCCall(GameBallManager.RPC.RequestGrabBall, info, "gameBallIndex out of array.");
			return;
		}
		Vector3 vector;
		Quaternion quaternion;
		BitPackUtils.UnpackHandPosRotFromNetwork(packedPosRot, out vector, out quaternion);
		float num = 10000f;
		if (!vector.IsValid(num) || !quaternion.IsValid())
		{
			this.ReportRPCCall(GameBallManager.RPC.RequestGrabBall, info, "localPosition or localRotation is invalid.");
			return;
		}
		bool flag = true;
		GameBall gameBall = this.gameBalls[gameBallIndex];
		if (gameBall != null)
		{
			RigContainer rigContainer;
			if (VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
			{
				if (!rigContainer.Rig.IsPositionInRange(gameBall.transform.position, 25f))
				{
					flag = false;
					this.ReportRPCCall(GameBallManager.RPC.RequestGrabBall, info, "gameBall exceeds max catch distance.");
				}
			}
			else
			{
				flag = false;
				this.ReportRPCCall(GameBallManager.RPC.RequestGrabBall, info, "Cannot find VRRig for grabber.");
			}
			if (vector.sqrMagnitude > 25f)
			{
				flag = false;
				this.ReportRPCCall(GameBallManager.RPC.RequestGrabBall, info, "gameBall exceeds max catch distance.");
			}
		}
		else
		{
			flag = false;
			this.ReportRPCCall(GameBallManager.RPC.RequestGrabBall, info, "gameBall does not exist.");
		}
		if (flag)
		{
			this.photonView.RPC("GrabBallRPC", RpcTarget.All, new object[]
			{
				gameBallIndex,
				isLeftHand,
				packedPosRot,
				info.Sender
			});
			PhotonNetwork.SendAllOutgoingCommands();
		}
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x000DC998 File Offset: 0x000DAB98
	[PunRPC]
	private void GrabBallRPC(int gameBallIndex, bool isLeftHand, long packedPosRot, Player grabbedBy, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "GrabBallRPC");
		if (!this.ValidateCallLimits(GameBallManager.RPC.GrabBall, info))
		{
			return;
		}
		if (gameBallIndex < 0 || gameBallIndex > this.gameBalls.Count)
		{
			this.ReportRPCCall(GameBallManager.RPC.GrabBall, info, "gameBallIndex out of array.");
			return;
		}
		Vector3 localPosition;
		Quaternion localRotation;
		BitPackUtils.UnpackHandPosRotFromNetwork(packedPosRot, out localPosition, out localRotation);
		float num = 10000f;
		if (!localPosition.IsValid(num) || !localRotation.IsValid())
		{
			this.ReportRPCCall(GameBallManager.RPC.GrabBall, info, "localPosition or localRotation is invalid.");
			return;
		}
		this.GrabBall(new GameBallId(gameBallIndex), isLeftHand, localPosition, localRotation, NetPlayer.Get(grabbedBy));
	}

	// Token: 0x06001C8F RID: 7311 RVA: 0x000DCA38 File Offset: 0x000DAC38
	private void GrabBall(GameBallId gameBallId, bool isLeftHand, Vector3 localPosition, Quaternion localRotation, NetPlayer grabbedByPlayer)
	{
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(grabbedByPlayer, out rigContainer))
		{
			return;
		}
		GameBallData gameBallData = this.gameBallData[gameBallId.index];
		GameBall gameBall = this.gameBalls[gameBallId.index];
		GamePlayer gamePlayer = (gameBall.heldByActorNumber < 0) ? null : GamePlayer.GetGamePlayer(gameBall.heldByActorNumber);
		int num = (gamePlayer == null) ? -1 : gamePlayer.FindHandIndex(gameBallId);
		bool flag = gameBall.heldByActorNumber == PhotonNetwork.LocalPlayer.ActorNumber;
		int num2 = -1;
		if (gamePlayer != null)
		{
			gamePlayer.ClearGrabbedIfHeld(gameBallId);
			num2 = gamePlayer.teamId;
			if (num != -1 && flag)
			{
				GamePlayerLocal.instance.ClearGrabbed(num);
			}
		}
		BodyDockPositions myBodyDockPositions = rigContainer.Rig.myBodyDockPositions;
		Transform parent = isLeftHand ? myBodyDockPositions.leftHandTransform : myBodyDockPositions.rightHandTransform;
		if (!grabbedByPlayer.IsLocal)
		{
			gameBall.SetVisualOffset(true);
		}
		gameBall.transform.SetParent(parent);
		gameBall.transform.SetLocalPositionAndRotation(localPosition, localRotation);
		Rigidbody component = gameBall.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.isKinematic = true;
		}
		GamePlayer gamePlayer2 = GamePlayer.GetGamePlayer(grabbedByPlayer.ActorNumber);
		bool flag2 = num2 == gamePlayer2.teamId;
		bool flag3 = gameBall.lastHeldByActorNumber != grabbedByPlayer.ActorNumber;
		MonkeBall component2 = gameBall.GetComponent<MonkeBall>();
		if (component2 != null)
		{
			component2.OnGrabbed();
			if (!flag2 && flag3)
			{
				component2.OnSwitchHeldByTeam(gamePlayer2.teamId);
			}
		}
		gameBall.heldByActorNumber = grabbedByPlayer.ActorNumber;
		gameBall.lastHeldByActorNumber = gameBall.heldByActorNumber;
		gameBall.SetHeldByTeamId(gamePlayer2.teamId);
		int handIndex = GamePlayer.GetHandIndex(isLeftHand);
		gamePlayer2.SetGrabbed(gameBallId, handIndex);
		if (grabbedByPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
		{
			GamePlayerLocal.instance.SetGrabbed(gameBallId, GamePlayer.GetHandIndex(isLeftHand));
			GamePlayerLocal.instance.PlayCatchFx(isLeftHand);
		}
		gameBall.PlayCatchFx();
		if (component2 != null)
		{
			MonkeBallGame.Instance.OnBallGrabbed(gameBallId);
		}
	}

	// Token: 0x06001C90 RID: 7312 RVA: 0x000DCC40 File Offset: 0x000DAE40
	public void RequestThrowBall(GameBallId ballId, bool isLeftHand, Vector3 velocity, Vector3 angVelocity)
	{
		GameBall gameBall = this.GetGameBall(ballId);
		if (gameBall == null)
		{
			return;
		}
		Vector3 position = gameBall.transform.position;
		Quaternion rotation = gameBall.transform.rotation;
		this.ThrowBall(ballId, isLeftHand, position, rotation, velocity, angVelocity, NetPlayer.Get(PhotonNetwork.LocalPlayer));
		this.photonView.RPC("RequestThrowBallRPC", RpcTarget.MasterClient, new object[]
		{
			ballId.index,
			isLeftHand,
			position,
			rotation,
			velocity,
			angVelocity
		});
		PhotonNetwork.SendAllOutgoingCommands();
	}

	// Token: 0x06001C91 RID: 7313 RVA: 0x000DCCE8 File Offset: 0x000DAEE8
	[PunRPC]
	private void RequestThrowBallRPC(int gameBallIndex, bool isLeftHand, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestThrowBallRPC");
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (!this.ValidateCallLimits(GameBallManager.RPC.RequestThrowBall, info))
		{
			return;
		}
		if (!this.ValidateThrowBallParams(gameBallIndex, position, rotation, velocity, angVelocity))
		{
			this.ReportRPCCall(GameBallManager.RPC.RequestThrowBall, info, "ValidateThrowBallParams are invalid.");
			return;
		}
		if (this.gameBalls[gameBallIndex].heldByActorNumber != info.Sender.ActorNumber && this.gameBalls[gameBallIndex].lastHeldByActorNumber != info.Sender.ActorNumber && (this.gameBalls[gameBallIndex].heldByActorNumber != -1 || this.gameBalls[gameBallIndex].lastHeldByActorNumber != -1))
		{
			this.ReportRPCCall(GameBallManager.RPC.RequestThrowBall, info, "gameBall is not held by the thrower.");
			return;
		}
		bool flag = true;
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(info.Sender.ActorNumber), out rigContainer))
		{
			if ((rigContainer.Rig.transform.position - position).sqrMagnitude > 6.25f)
			{
				flag = false;
				this.ReportRPCCall(GameBallManager.RPC.RequestThrowBall, info, "gameBall distance exceeds max distance from hand.");
			}
		}
		else
		{
			flag = false;
			this.ReportRPCCall(GameBallManager.RPC.RequestThrowBall, info, "Player rig cannot be found for thrower.");
		}
		if (flag)
		{
			this.photonView.RPC("ThrowBallRPC", RpcTarget.All, new object[]
			{
				gameBallIndex,
				isLeftHand,
				position,
				rotation,
				velocity,
				angVelocity,
				info.Sender,
				info.SentServerTime
			});
			PhotonNetwork.SendAllOutgoingCommands();
		}
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x000DCE88 File Offset: 0x000DB088
	[PunRPC]
	private void ThrowBallRPC(int gameBallIndex, bool isLeftHand, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity, Player thrownBy, double throwTime, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "ThrowBallRPC");
		if (!this.ValidateCallLimits(GameBallManager.RPC.ThrowBall, info))
		{
			return;
		}
		if (!this.ValidateThrowBallParams(gameBallIndex, position, rotation, velocity, angVelocity))
		{
			this.ReportRPCCall(GameBallManager.RPC.ThrowBall, info, "ValidateThrowBallParams are invalid.");
			return;
		}
		if ((base.transform.position - position).sqrMagnitude > 6400f)
		{
			this.ReportRPCCall(GameBallManager.RPC.ThrowBall, info, "gameBall distance exceeds max distance from arena.");
			return;
		}
		if (double.IsNaN(throwTime) || double.IsInfinity(throwTime))
		{
			this.ReportRPCCall(GameBallManager.RPC.ThrowBall, info, "throwTime is not a valid value.");
			return;
		}
		float num = (float)(PhotonNetwork.Time - throwTime);
		if (num < -3f || num > 3f)
		{
			this.ReportRPCCall(GameBallManager.RPC.ThrowBall, info, "Throw time delta exceeds range.");
			return;
		}
		GameBall gameBall = this.gameBalls[gameBallIndex];
		position = 0.5f * Physics.gravity * gameBall.gravityMult * num * num + velocity * num + position;
		velocity = Physics.gravity * gameBall.gravityMult * num + velocity;
		rotation *= Quaternion.Euler(angVelocity * Time.deltaTime);
		this.ThrowBall(new GameBallId(gameBallIndex), isLeftHand, position, rotation, velocity, angVelocity, NetPlayer.Get(thrownBy));
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x000DCFF0 File Offset: 0x000DB1F0
	private bool ValidateThrowBallParams(int gameBallIndex, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity)
	{
		if (gameBallIndex < 0 || gameBallIndex >= this.gameBalls.Count)
		{
			return false;
		}
		float num = 10000f;
		if (position.IsValid(num) && rotation.IsValid())
		{
			float num2 = 10000f;
			if (velocity.IsValid(num2))
			{
				float num3 = 10000f;
				if (angVelocity.IsValid(num3))
				{
					return velocity.sqrMagnitude <= 1600f;
				}
			}
		}
		return false;
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x000DD060 File Offset: 0x000DB260
	private void ThrowBall(GameBallId gameBallId, bool isLeftHand, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angVelocity, NetPlayer thrownByPlayer)
	{
		GameBall gameBall = this.gameBalls[gameBallId.index];
		if (!thrownByPlayer.IsLocal)
		{
			gameBall.SetVisualOffset(true);
		}
		gameBall.transform.SetParent(null);
		gameBall.transform.SetLocalPositionAndRotation(position, rotation);
		Rigidbody component = gameBall.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.isKinematic = false;
			component.position = position;
			component.rotation = rotation;
			component.velocity = velocity;
			component.angularVelocity = angVelocity;
		}
		gameBall.heldByActorNumber = -1;
		MonkeBall monkeBall = MonkeBall.Get(gameBall);
		if (monkeBall != null)
		{
			monkeBall.ClearCannotGrabTeamId();
		}
		bool flag = thrownByPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber;
		int handIndex = GamePlayer.GetHandIndex(isLeftHand);
		RigContainer rigContainer;
		if (flag)
		{
			GamePlayerLocal.instance.gamePlayer.ClearGrabbed(handIndex);
			GamePlayerLocal.instance.ClearGrabbed(handIndex);
			GamePlayerLocal.instance.PlayThrowFx(isLeftHand);
		}
		else if (VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(thrownByPlayer.ActorNumber), out rigContainer))
		{
			GamePlayer component2 = rigContainer.Rig.GetComponent<GamePlayer>();
			if (component2 != null)
			{
				component2.ClearGrabbedIfHeld(gameBallId);
			}
		}
		gameBall.PlayThrowFx();
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x000DD18C File Offset: 0x000DB38C
	public void RequestLaunchBall(GameBallId ballId, Vector3 velocity)
	{
		GameBall gameBall = this.GetGameBall(ballId);
		if (gameBall == null)
		{
			return;
		}
		Vector3 position = gameBall.transform.position;
		Quaternion rotation = gameBall.transform.rotation;
		this.LaunchBall(ballId, position, rotation, velocity);
		this.photonView.RPC("RequestLaunchBallRPC", RpcTarget.MasterClient, new object[]
		{
			ballId.index,
			position,
			rotation,
			velocity
		});
		PhotonNetwork.SendAllOutgoingCommands();
	}

	// Token: 0x06001C96 RID: 7318 RVA: 0x000DD214 File Offset: 0x000DB414
	[PunRPC]
	private void RequestLaunchBallRPC(int gameBallIndex, Vector3 position, Quaternion rotation, Vector3 velocity, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestLaunchBallRPC");
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (!this.ValidateCallLimits(GameBallManager.RPC.RequestLaunchBall, info))
		{
			return;
		}
		if (!this.ValidateThrowBallParams(gameBallIndex, position, rotation, velocity, Vector3.zero))
		{
			this.ReportRPCCall(GameBallManager.RPC.RequestLaunchBall, info, "ValidateThrowBallParams are invalid.");
			return;
		}
		bool flag = true;
		if ((MonkeBallGame.Instance.BallLauncher.position - position).sqrMagnitude > 1f)
		{
			flag = false;
			this.ReportRPCCall(GameBallManager.RPC.RequestLaunchBall, info, "gameBall distance exceeds max distance from launcher.");
		}
		if (flag)
		{
			this.photonView.RPC("LaunchBallRPC", RpcTarget.All, new object[]
			{
				gameBallIndex,
				position,
				rotation,
				velocity,
				info.SentServerTime
			});
			PhotonNetwork.SendAllOutgoingCommands();
		}
	}

	// Token: 0x06001C97 RID: 7319 RVA: 0x000DD2EC File Offset: 0x000DB4EC
	[PunRPC]
	private void LaunchBallRPC(int gameBallIndex, Vector3 position, Quaternion rotation, Vector3 velocity, double throwTime, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "LaunchBallRPC");
		if (!this.ValidateCallLimits(GameBallManager.RPC.ThrowBall, info))
		{
			return;
		}
		if (!this.ValidateThrowBallParams(gameBallIndex, position, rotation, velocity, Vector3.zero))
		{
			this.ReportRPCCall(GameBallManager.RPC.LaunchBall, info, "ValidateThrowBallParams are invalid.");
			return;
		}
		float num = (float)(PhotonNetwork.Time - throwTime);
		if (num < -3f || num > 3f)
		{
			this.ReportRPCCall(GameBallManager.RPC.LaunchBall, info, "Throw time delta exceeds range.");
			return;
		}
		GameBall gameBall = this.gameBalls[gameBallIndex];
		position = 0.5f * Physics.gravity * gameBall.gravityMult * num * num + velocity * num + position;
		velocity = Physics.gravity * gameBall.gravityMult * num + velocity;
		this.LaunchBall(new GameBallId(gameBallIndex), position, rotation, velocity);
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x000DD3E4 File Offset: 0x000DB5E4
	private void LaunchBall(GameBallId gameBallId, Vector3 position, Quaternion rotation, Vector3 velocity)
	{
		GameBall gameBall = this.gameBalls[gameBallId.index];
		gameBall.transform.SetParent(null);
		gameBall.transform.SetLocalPositionAndRotation(position, rotation);
		Rigidbody component = gameBall.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.isKinematic = false;
			component.position = position;
			component.rotation = rotation;
			component.velocity = velocity;
			component.angularVelocity = Vector3.zero;
		}
		gameBall.heldByActorNumber = -1;
		gameBall.lastHeldByActorNumber = -1;
		gameBall.WasLaunched();
		MonkeBall monkeBall = MonkeBall.Get(gameBall);
		if (monkeBall != null)
		{
			monkeBall.ClearCannotGrabTeamId();
			monkeBall.TriggerDelayedResync();
		}
		gameBall.PlayThrowFx();
		GamePlayerLocal.instance.gamePlayer.ClearAllGrabbed();
		GamePlayerLocal.instance.ClearAllGrabbed();
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x000DD4A8 File Offset: 0x000DB6A8
	public void RequestTeleportBall(GameBallId id, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		this.photonView.RPC("TeleportBallRPC", RpcTarget.All, new object[]
		{
			id.index,
			position,
			rotation,
			velocity,
			angularVelocity
		});
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x000DD508 File Offset: 0x000DB708
	[PunRPC]
	private void TeleportBallRPC(int gameBallIndex, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "TeleportBallRPC");
		if (!this.ValidateCallLimits(GameBallManager.RPC.TeleportBall, info))
		{
			return;
		}
		if (gameBallIndex < 0 || gameBallIndex >= this.gameBalls.Count)
		{
			this.ReportRPCCall(GameBallManager.RPC.TeleportBall, info, "gameBallIndex is out of range.");
			return;
		}
		float num = 10000f;
		if (position.IsValid(num) && rotation.IsValid())
		{
			float num2 = 10000f;
			if (velocity.IsValid(num2))
			{
				float num3 = 10000f;
				if (angularVelocity.IsValid(num3))
				{
					if ((base.transform.position - position).sqrMagnitude > 6400f)
					{
						this.ReportRPCCall(GameBallManager.RPC.ThrowBall, info, "gameBall distance exceeds max distance from arena.");
						return;
					}
					GameBallId gameBallId = new GameBallId(gameBallIndex);
					this.TeleportBall(gameBallId, position, rotation, velocity, angularVelocity);
					return;
				}
			}
		}
		this.ReportRPCCall(GameBallManager.RPC.TeleportBall, info, "Ball params are invalid.");
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x000DD5F0 File Offset: 0x000DB7F0
	private void TeleportBall(GameBallId gameBallId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
	{
		int index = gameBallId.index;
		if (index < 0 || index >= this.gameBallData.Count)
		{
			return;
		}
		GameBallData gameBallData = this.gameBallData[index];
		GameBall gameBall = this.gameBalls[index];
		if (gameBall == null)
		{
			return;
		}
		gameBall.SetVisualOffset(false);
		gameBall.transform.SetLocalPositionAndRotation(position, rotation);
		Rigidbody component = gameBall.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.isKinematic = false;
			component.position = position;
			component.rotation = rotation;
			component.velocity = velocity;
			component.angularVelocity = angularVelocity;
		}
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x000DD684 File Offset: 0x000DB884
	public void RequestSetBallPosition(GameBallId ballId)
	{
		if (this.GetGameBall(ballId) == null)
		{
			return;
		}
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		this.photonView.RPC("RequestSetBallPositionRPC", RpcTarget.MasterClient, new object[]
		{
			ballId.index
		});
		PhotonNetwork.SendAllOutgoingCommands();
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x000DD6D8 File Offset: 0x000DB8D8
	[PunRPC]
	private void RequestSetBallPositionRPC(int gameBallIndex, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestSetBallPositionRPC");
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (!this.ValidateCallLimits(GameBallManager.RPC.RequestSetBallPosition, info))
		{
			return;
		}
		if (gameBallIndex < 0 || gameBallIndex >= this.gameBalls.Count)
		{
			this.ReportRPCCall(GameBallManager.RPC.RequestSetBallPosition, info, "gameBallIndex is out of range.");
			return;
		}
		GameBall gameBall = this.gameBalls[gameBallIndex];
		if (gameBall == null)
		{
			return;
		}
		if ((gameBall.transform.position - base.transform.position).sqrMagnitude > 6400f)
		{
			this.ReportRPCCall(GameBallManager.RPC.RequestSetBallPosition, info, "Ball position is outside of arena.");
			return;
		}
		Rigidbody component = gameBall.GetComponent<Rigidbody>();
		if (component == null)
		{
			return;
		}
		this.photonView.RPC("TeleportBallRPC", info.Sender, new object[]
		{
			gameBallIndex,
			gameBall.transform.position,
			gameBall.transform.rotation,
			component.velocity,
			component.angularVelocity
		});
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x00030607 File Offset: 0x0002E807
	public override void WriteDataFusion()
	{
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x00030607 File Offset: 0x0002E807
	public override void ReadDataFusion()
	{
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001CA3 RID: 7331 RVA: 0x00030709 File Offset: 0x0002E909
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x00030715 File Offset: 0x0002E915
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x04001F91 RID: 8081
	[OnEnterPlay_SetNull]
	public static volatile GameBallManager Instance;

	// Token: 0x04001F92 RID: 8082
	public PhotonView photonView;

	// Token: 0x04001F93 RID: 8083
	private List<GameBall> gameBalls;

	// Token: 0x04001F94 RID: 8084
	private List<GameBallData> gameBallData;

	// Token: 0x04001F95 RID: 8085
	public const float MAX_LOCAL_MAGNITUDE_SQR = 6400f;

	// Token: 0x04001F96 RID: 8086
	private const float MAX_LAUNCHER_DISTANCE_SQR = 1f;

	// Token: 0x04001F97 RID: 8087
	public const float MAX_CATCH_DISTANCE_FROM_HAND_SQR = 25f;

	// Token: 0x04001F98 RID: 8088
	public const float MAX_DISTANCE_FROM_HAND_SQR = 6.25f;

	// Token: 0x04001F99 RID: 8089
	public const float MAX_THROW_VELOCITY_SQR = 1600f;

	// Token: 0x04001F9A RID: 8090
	private CallLimiter[] _callLimiters;

	// Token: 0x0200049D RID: 1181
	private enum RPC
	{
		// Token: 0x04001F9C RID: 8092
		RequestGrabBall,
		// Token: 0x04001F9D RID: 8093
		GrabBall,
		// Token: 0x04001F9E RID: 8094
		RequestThrowBall,
		// Token: 0x04001F9F RID: 8095
		ThrowBall,
		// Token: 0x04001FA0 RID: 8096
		RequestLaunchBall,
		// Token: 0x04001FA1 RID: 8097
		LaunchBall,
		// Token: 0x04001FA2 RID: 8098
		TeleportBall,
		// Token: 0x04001FA3 RID: 8099
		RequestSetBallPosition,
		// Token: 0x04001FA4 RID: 8100
		Count
	}
}
