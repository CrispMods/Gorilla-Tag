using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000490 RID: 1168
[NetworkBehaviourWeaved(0)]
public class GameBallManager : NetworkComponent
{
	// Token: 0x06001C35 RID: 7221 RVA: 0x00088F38 File Offset: 0x00087138
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

	// Token: 0x06001C36 RID: 7222 RVA: 0x00089044 File Offset: 0x00087244
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

	// Token: 0x06001C37 RID: 7223 RVA: 0x0008907F File Offset: 0x0008727F
	private void ReportRPCCall(GameBallManager.RPC rpcCall, PhotonMessageInfo info, string susReason)
	{
		GorillaNot.instance.SendReport(string.Format("Reason: {0}   RPC: {1}", susReason, rpcCall), info.Sender.UserId, info.Sender.NickName);
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x000890B4 File Offset: 0x000872B4
	public GameBallId AddGameBall(GameBall gameBall)
	{
		int count = this.gameBallData.Count;
		this.gameBalls.Add(gameBall);
		GameBallData item = default(GameBallData);
		this.gameBallData.Add(item);
		gameBall.id = new GameBallId(count);
		return gameBall.id;
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x00089100 File Offset: 0x00087300
	public GameBall GetGameBall(GameBallId id)
	{
		if (!id.IsValid())
		{
			return null;
		}
		int index = id.index;
		return this.gameBalls[index];
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x0008912C File Offset: 0x0008732C
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

	// Token: 0x06001C3B RID: 7227 RVA: 0x00089214 File Offset: 0x00087414
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

	// Token: 0x06001C3C RID: 7228 RVA: 0x0008927C File Offset: 0x0008747C
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

	// Token: 0x06001C3D RID: 7229 RVA: 0x000893E0 File Offset: 0x000875E0
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

	// Token: 0x06001C3E RID: 7230 RVA: 0x00089480 File Offset: 0x00087680
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

	// Token: 0x06001C3F RID: 7231 RVA: 0x00089688 File Offset: 0x00087888
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

	// Token: 0x06001C40 RID: 7232 RVA: 0x00089730 File Offset: 0x00087930
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

	// Token: 0x06001C41 RID: 7233 RVA: 0x000898D0 File Offset: 0x00087AD0
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

	// Token: 0x06001C42 RID: 7234 RVA: 0x00089A38 File Offset: 0x00087C38
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

	// Token: 0x06001C43 RID: 7235 RVA: 0x00089AA8 File Offset: 0x00087CA8
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

	// Token: 0x06001C44 RID: 7236 RVA: 0x00089BD4 File Offset: 0x00087DD4
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

	// Token: 0x06001C45 RID: 7237 RVA: 0x00089C5C File Offset: 0x00087E5C
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

	// Token: 0x06001C46 RID: 7238 RVA: 0x00089D34 File Offset: 0x00087F34
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

	// Token: 0x06001C47 RID: 7239 RVA: 0x00089E2C File Offset: 0x0008802C
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

	// Token: 0x06001C48 RID: 7240 RVA: 0x00089EF0 File Offset: 0x000880F0
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

	// Token: 0x06001C49 RID: 7241 RVA: 0x00089F50 File Offset: 0x00088150
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

	// Token: 0x06001C4A RID: 7242 RVA: 0x0008A038 File Offset: 0x00088238
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

	// Token: 0x06001C4B RID: 7243 RVA: 0x0008A0CC File Offset: 0x000882CC
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

	// Token: 0x06001C4C RID: 7244 RVA: 0x0008A120 File Offset: 0x00088320
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

	// Token: 0x06001C4D RID: 7245 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void WriteDataFusion()
	{
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void ReadDataFusion()
	{
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001C50 RID: 7248 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x00002655 File Offset: 0x00000855
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06001C53 RID: 7251 RVA: 0x00002661 File Offset: 0x00000861
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x04001F43 RID: 8003
	[OnEnterPlay_SetNull]
	public static volatile GameBallManager Instance;

	// Token: 0x04001F44 RID: 8004
	public PhotonView photonView;

	// Token: 0x04001F45 RID: 8005
	private List<GameBall> gameBalls;

	// Token: 0x04001F46 RID: 8006
	private List<GameBallData> gameBallData;

	// Token: 0x04001F47 RID: 8007
	public const float MAX_LOCAL_MAGNITUDE_SQR = 6400f;

	// Token: 0x04001F48 RID: 8008
	private const float MAX_LAUNCHER_DISTANCE_SQR = 1f;

	// Token: 0x04001F49 RID: 8009
	public const float MAX_CATCH_DISTANCE_FROM_HAND_SQR = 25f;

	// Token: 0x04001F4A RID: 8010
	public const float MAX_DISTANCE_FROM_HAND_SQR = 6.25f;

	// Token: 0x04001F4B RID: 8011
	public const float MAX_THROW_VELOCITY_SQR = 1600f;

	// Token: 0x04001F4C RID: 8012
	private CallLimiter[] _callLimiters;

	// Token: 0x02000491 RID: 1169
	private enum RPC
	{
		// Token: 0x04001F4E RID: 8014
		RequestGrabBall,
		// Token: 0x04001F4F RID: 8015
		GrabBall,
		// Token: 0x04001F50 RID: 8016
		RequestThrowBall,
		// Token: 0x04001F51 RID: 8017
		ThrowBall,
		// Token: 0x04001F52 RID: 8018
		RequestLaunchBall,
		// Token: 0x04001F53 RID: 8019
		LaunchBall,
		// Token: 0x04001F54 RID: 8020
		TeleportBall,
		// Token: 0x04001F55 RID: 8021
		RequestSetBallPosition,
		// Token: 0x04001F56 RID: 8022
		Count
	}
}
