using System;
using Fusion;
using GorillaExtensions;
using GorillaGameModes;
using GorillaTag;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000533 RID: 1331
[NetworkBehaviourWeaved(1)]
internal class GameModeSerializer : GorillaSerializerMasterOnly, IStateAuthorityChanged
{
	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06002024 RID: 8228 RVA: 0x000A1D74 File Offset: 0x0009FF74
	// (set) Token: 0x06002025 RID: 8229 RVA: 0x000A1D9A File Offset: 0x0009FF9A
	[Networked]
	[NetworkedWeaved(0, 1)]
	private unsafe int gameModeKeyInt
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing GameModeSerializer.gameModeKeyInt. Networked properties can only be accessed when Spawned() has been called.");
			}
			return this.Ptr[0];
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing GameModeSerializer.gameModeKeyInt. Networked properties can only be accessed when Spawned() has been called.");
			}
			this.Ptr[0] = value;
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06002026 RID: 8230 RVA: 0x000A1DC1 File Offset: 0x0009FFC1
	public GorillaGameManager GameModeInstance
	{
		get
		{
			return this.gameModeInstance;
		}
	}

	// Token: 0x06002027 RID: 8231 RVA: 0x000A1DCC File Offset: 0x0009FFCC
	protected override bool OnSpawnSetupCheck(PhotonMessageInfoWrapped wrappedInfo, out GameObject outTargetObject, out Type outTargetType)
	{
		outTargetObject = null;
		outTargetType = null;
		NetPlayer player = NetworkSystem.Instance.GetPlayer(wrappedInfo.senderID);
		if (player != null)
		{
			GorillaNot.IncrementRPCCall(wrappedInfo, "OnSpawnSetupCheck");
		}
		GameModeSerializer activeNetworkHandler = GorillaGameModes.GameMode.ActiveNetworkHandler;
		if (player != null && player.InRoom)
		{
			if (!player.IsMasterClient)
			{
				GTDev.LogError<string>("SPAWN FAIL NOT MASTER :" + player.UserId + player.NickName, null);
				GorillaNot.instance.SendReport("trying to inappropriately create game managers", player.UserId, player.NickName);
				return false;
			}
			if (!this.netView.IsRoomView)
			{
				GTDev.LogError<string>("SPAWN FAIL ROOM VIEW" + player.UserId + player.NickName, null);
				GorillaNot.instance.SendReport("creating game manager as player object", player.UserId, player.NickName);
				return false;
			}
			if (activeNetworkHandler.IsNotNull() && activeNetworkHandler != this)
			{
				GTDev.LogError<string>("DUPLICATE CHECK" + player.UserId + player.NickName, null);
				GorillaNot.instance.SendReport("trying to create multiple game managers", player.UserId, player.NickName);
				return false;
			}
		}
		else if ((activeNetworkHandler.IsNotNull() && activeNetworkHandler != this) || !this.netView.IsRoomView)
		{
			GTDev.LogError<string>("ACTIVE HANDLER CHECK FAIL" + ((player != null) ? player.UserId : null) + ((player != null) ? player.NickName : null), null);
			GTDev.LogError<string>("existing game manager! destroying newly created manager", null);
			return false;
		}
		object[] instantiationData = wrappedInfo.punInfo.photonView.InstantiationData;
		if (instantiationData != null && instantiationData.Length >= 1)
		{
			object obj = instantiationData[0];
			if (obj is int)
			{
				int num = (int)obj;
				this.gameModeKey = (GameModeType)num;
				this.gameModeInstance = GorillaGameModes.GameMode.GetGameModeInstance(this.gameModeKey);
				if (this.gameModeInstance.IsNull() || !this.gameModeInstance.ValidGameMode())
				{
					return false;
				}
				this.serializeTarget = this.gameModeInstance;
				base.transform.parent = VRRigCache.Instance.NetworkParent;
				return true;
			}
		}
		GTDev.LogError<string>("missing instantiation data", null);
		return false;
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x000A1FDA File Offset: 0x000A01DA
	internal void Init(int gameModeType)
	{
		Debug.Log("<color=red>Init called</color>");
		this.gameModeKeyInt = gameModeType;
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x000A1FED File Offset: 0x000A01ED
	protected override void OnSuccesfullySpawned(PhotonMessageInfoWrapped info)
	{
		this.netView.GetView.AddCallbackTarget(this);
		GorillaGameModes.GameMode.SetupGameModeRemote(this);
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x000A2006 File Offset: 0x000A0206
	protected override void OnBeforeDespawn()
	{
		GorillaGameModes.GameMode.RemoveNetworkLink(this);
	}

	// Token: 0x0600202B RID: 8235 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void OnFailedSpawn()
	{
	}

	// Token: 0x0600202C RID: 8236 RVA: 0x000A200E File Offset: 0x000A020E
	[PunRPC]
	internal void RPC_ReportTag(int taggedPlayer, PhotonMessageInfo info)
	{
		this.ReportTag(NetworkSystem.Instance.GetPlayer(taggedPlayer), new PhotonMessageInfoWrapped(info));
	}

	// Token: 0x0600202D RID: 8237 RVA: 0x000A2027 File Offset: 0x000A0227
	[PunRPC]
	internal void RPC_ReportHit(PhotonMessageInfo info)
	{
		this.ReportHit(new PhotonMessageInfoWrapped(info));
	}

	// Token: 0x0600202E RID: 8238 RVA: 0x000A2038 File Offset: 0x000A0238
	[Rpc(RpcSources.All, RpcTargets.All)]
	internal unsafe void RPC_ReportTag(int taggedPlayer, RpcInfo info = default(RpcInfo))
	{
		if (!this.InvokeRpc)
		{
			NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
			if (base.Runner.Stage != SimulationStages.Resimulate)
			{
				int localAuthorityMask = base.Object.GetLocalAuthorityMask();
				if ((localAuthorityMask & 7) == 0)
				{
					NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void GameModeSerializer::RPC_ReportTag(System.Int32,Fusion.RpcInfo)", base.Object, 7);
				}
				else
				{
					if (base.Runner.HasAnyActiveConnections())
					{
						int num = 8;
						num += 4;
						SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, num);
						byte* data = SimulationMessage.GetData(ptr);
						int num2 = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 1), data);
						*(int*)(data + num2) = taggedPlayer;
						num2 += 4;
						ptr->Offset = num2 * 8;
						base.Runner.SendRpc(ptr);
					}
					if ((localAuthorityMask & 7) != 0)
					{
						info = RpcInfo.FromLocal(base.Runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
						goto IL_12;
					}
				}
			}
			return;
		}
		this.InvokeRpc = false;
		IL_12:
		this.ReportTag(NetworkSystem.Instance.GetPlayer(taggedPlayer), new PhotonMessageInfoWrapped(info));
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x000A2184 File Offset: 0x000A0384
	[Rpc(RpcSources.All, RpcTargets.All)]
	internal unsafe void RPC_ReportHit(RpcInfo info = default(RpcInfo))
	{
		if (!this.InvokeRpc)
		{
			NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
			if (base.Runner.Stage != SimulationStages.Resimulate)
			{
				int localAuthorityMask = base.Object.GetLocalAuthorityMask();
				if ((localAuthorityMask & 7) == 0)
				{
					NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void GameModeSerializer::RPC_ReportHit(Fusion.RpcInfo)", base.Object, 7);
				}
				else
				{
					if (base.Runner.HasAnyActiveConnections())
					{
						int capacityInBytes = 8;
						SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, capacityInBytes);
						byte* data = SimulationMessage.GetData(ptr);
						int num = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 2), data);
						ptr->Offset = num * 8;
						base.Runner.SendRpc(ptr);
					}
					if ((localAuthorityMask & 7) != 0)
					{
						info = RpcInfo.FromLocal(base.Runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
						goto IL_12;
					}
				}
			}
			return;
		}
		this.InvokeRpc = false;
		IL_12:
		this.ReportHit(new PhotonMessageInfoWrapped(info));
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x000A22A4 File Offset: 0x000A04A4
	private void ReportTag(NetPlayer taggedPlayer, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "ReportTag");
		NetPlayer sender = info.Sender;
		this.gameModeInstance.ReportTag(taggedPlayer, sender);
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x000A22D4 File Offset: 0x000A04D4
	private void ReportHit(PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "ReportContactWithLavaRPC");
		NetPlayer sender = info.Sender;
		bool flag = ZoneManagement.instance.IsZoneActive(GTZone.customMaps);
		bool flag2 = false;
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			InfectionLavaController instance = InfectionLavaController.Instance;
			flag2 = (instance != null && instance.LavaCurrentlyActivated && (instance.SurfaceCenter - rigContainer.Rig.syncPos).sqrMagnitude < 2500f && instance.LavaPlane.GetDistanceToPoint(rigContainer.Rig.syncPos) < 5f);
		}
		if (flag || flag2)
		{
			this.GameModeInstance.HitPlayer(info.Sender);
		}
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x000A238E File Offset: 0x000A058E
	[PunRPC]
	internal void RPC_BroadcastRoundComplete(PhotonMessageInfo info)
	{
		this.BroadcastRoundComplete(info);
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x000A239C File Offset: 0x000A059C
	private void BroadcastRoundComplete(PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "BroadcastRoundComplete");
		if (info.Sender.IsMasterClient)
		{
			this.gameModeInstance.HandleRoundComplete();
		}
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x000A23C2 File Offset: 0x000A05C2
	protected override void FusionDataRPC(string method, NetPlayer targetPlayer, params object[] parameters)
	{
		Debug.Log(this.gameModeData.GetType().Name);
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x000A23D9 File Offset: 0x000A05D9
	protected override void FusionDataRPC(string method, RpcTarget target, params object[] parameters)
	{
		base.FusionDataRPC(method, target, parameters);
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x000A23E4 File Offset: 0x000A05E4
	void IStateAuthorityChanged.StateAuthorityChanged()
	{
		GameModeSerializer.FusionGameModeOwnerChanged(NetworkSystem.Instance.GetPlayer(base.Object.StateAuthority));
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x000A240D File Offset: 0x000A060D
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.gameModeKeyInt = this._gameModeKeyInt;
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x000A2425 File Offset: 0x000A0625
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._gameModeKeyInt = this.gameModeKeyInt;
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x000A243C File Offset: 0x000A063C
	[NetworkRpcWeavedInvoker(1, 7, 7)]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_ReportTag@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		int num2 = *(int*)(data + num);
		num += 4;
		int taggedPlayer = num2;
		RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
		behaviour.InvokeRpc = true;
		((GameModeSerializer)behaviour).RPC_ReportTag(taggedPlayer, info);
	}

	// Token: 0x0600203B RID: 8251 RVA: 0x000A24AC File Offset: 0x000A06AC
	[NetworkRpcWeavedInvoker(2, 7, 7)]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_ReportHit@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
		behaviour.InvokeRpc = true;
		((GameModeSerializer)behaviour).RPC_ReportHit(info);
	}

	// Token: 0x04002450 RID: 9296
	[WeaverGenerated]
	[DefaultForProperty("gameModeKeyInt", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private int _gameModeKeyInt;

	// Token: 0x04002451 RID: 9297
	private GameModeType gameModeKey;

	// Token: 0x04002452 RID: 9298
	private GorillaGameManager gameModeInstance;

	// Token: 0x04002453 RID: 9299
	private FusionGameModeData gameModeData;

	// Token: 0x04002454 RID: 9300
	private Type currentGameDataType;

	// Token: 0x04002455 RID: 9301
	public static Action<NetPlayer> FusionGameModeOwnerChanged;
}
