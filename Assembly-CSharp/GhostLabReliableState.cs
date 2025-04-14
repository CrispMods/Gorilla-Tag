using System;
using Fusion;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Scripting;

// Token: 0x020000AF RID: 175
[NetworkBehaviourWeaved(11)]
public class GhostLabReliableState : NetworkComponent
{
	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000467 RID: 1127 RVA: 0x0001A150 File Offset: 0x00018350
	// (set) Token: 0x06000468 RID: 1128 RVA: 0x0001A17A File Offset: 0x0001837A
	[Networked]
	[NetworkedWeaved(0, 11)]
	private unsafe GhostLabData NetData
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing GhostLabReliableState.NetData. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(GhostLabData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing GhostLabReliableState.NetData. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(GhostLabData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0001A1A5 File Offset: 0x000183A5
	protected override void Awake()
	{
		base.Awake();
		this.singleDoorOpen = new bool[this.singleDoorCount];
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0001A1BE File Offset: 0x000183BE
	public override void OnOwnerChange(Player newOwner, Player previousOwner)
	{
		base.OnOwnerChange(newOwner, previousOwner);
		Player localPlayer = PhotonNetwork.LocalPlayer;
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0001A1D0 File Offset: 0x000183D0
	public override void WriteDataFusion()
	{
		this.NetData = new GhostLabData((int)this.doorState, this.singleDoorOpen);
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0001A1EC File Offset: 0x000183EC
	public override void ReadDataFusion()
	{
		this.doorState = (GhostLab.EntranceDoorsState)this.NetData.DoorState;
		for (int i = 0; i < this.singleDoorCount; i++)
		{
			this.singleDoorOpen[i] = this.NetData.OpenDoors[i];
		}
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0001A244 File Offset: 0x00018444
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!base.IsMine && !info.Sender.IsMasterClient)
		{
			return;
		}
		stream.SendNext(this.doorState);
		for (int i = 0; i < this.singleDoorOpen.Length; i++)
		{
			stream.SendNext(this.singleDoorOpen[i]);
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0001A2A0 File Offset: 0x000184A0
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!base.IsMine && !info.Sender.IsMasterClient)
		{
			return;
		}
		this.doorState = (GhostLab.EntranceDoorsState)stream.ReceiveNext();
		for (int i = 0; i < this.singleDoorOpen.Length; i++)
		{
			this.singleDoorOpen[i] = (bool)stream.ReceiveNext();
		}
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0001A2FC File Offset: 0x000184FC
	public void UpdateEntranceDoorsState(GhostLab.EntranceDoorsState newState)
	{
		if (!NetworkSystem.Instance.InRoom || NetworkSystem.Instance.IsMasterClient)
		{
			this.doorState = newState;
			return;
		}
		if (NetworkSystem.Instance.InRoom && !NetworkSystem.Instance.IsMasterClient)
		{
			base.SendRPC("RemoteEntranceDoorState", RpcTarget.MasterClient, new object[]
			{
				newState
			});
		}
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0001A35C File Offset: 0x0001855C
	public void UpdateSingleDoorState(int singleDoorIndex)
	{
		if (!NetworkSystem.Instance.InRoom || NetworkSystem.Instance.IsMasterClient)
		{
			this.singleDoorOpen[singleDoorIndex] = !this.singleDoorOpen[singleDoorIndex];
			return;
		}
		if (NetworkSystem.Instance.InRoom && !NetworkSystem.Instance.IsMasterClient)
		{
			base.SendRPC("RemoteSingleDoorState", RpcTarget.MasterClient, new object[]
			{
				singleDoorIndex
			});
		}
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0001A3C8 File Offset: 0x000185C8
	[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
	public unsafe void RPC_RemoteEntranceDoorState(GhostLab.EntranceDoorsState newState, RpcInfo info = default(RpcInfo))
	{
		if (!this.InvokeRpc)
		{
			NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
			if (base.Runner.Stage != SimulationStages.Resimulate)
			{
				int localAuthorityMask = base.Object.GetLocalAuthorityMask();
				if ((localAuthorityMask & 7) != 0)
				{
					if ((localAuthorityMask & 1) != 1)
					{
						if (base.Runner.HasAnyActiveConnections())
						{
							int num = 8;
							num += 4;
							SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, num);
							byte* data = SimulationMessage.GetData(ptr);
							int num2 = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 1), data);
							*(GhostLab.EntranceDoorsState*)(data + num2) = newState;
							num2 += 4;
							ptr->Offset = num2 * 8;
							base.Runner.SendRpc(ptr);
						}
						if ((localAuthorityMask & 1) == 0)
						{
							return;
						}
					}
					info = RpcInfo.FromLocal(base.Runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
					goto IL_12;
				}
				NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void GhostLabReliableState::RPC_RemoteEntranceDoorState(GhostLab/EntranceDoorsState,Fusion.RpcInfo)", base.Object, 7);
			}
			return;
		}
		this.InvokeRpc = false;
		IL_12:
		GorillaNot.IncrementRPCCall(info, "RPC_RemoteEntranceDoorState");
		if (!base.IsMine)
		{
			return;
		}
		this.doorState = newState;
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0001A52C File Offset: 0x0001872C
	[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
	public unsafe void RPC_RemoteSingleDoorState(int doorIndex, RpcInfo info = default(RpcInfo))
	{
		if (!this.InvokeRpc)
		{
			NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
			if (base.Runner.Stage != SimulationStages.Resimulate)
			{
				int localAuthorityMask = base.Object.GetLocalAuthorityMask();
				if ((localAuthorityMask & 7) != 0)
				{
					if ((localAuthorityMask & 1) != 1)
					{
						if (base.Runner.HasAnyActiveConnections())
						{
							int num = 8;
							num += 4;
							SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, num);
							byte* data = SimulationMessage.GetData(ptr);
							int num2 = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 2), data);
							*(int*)(data + num2) = doorIndex;
							num2 += 4;
							ptr->Offset = num2 * 8;
							base.Runner.SendRpc(ptr);
						}
						if ((localAuthorityMask & 1) == 0)
						{
							return;
						}
					}
					info = RpcInfo.FromLocal(base.Runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
					goto IL_12;
				}
				NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void GhostLabReliableState::RPC_RemoteSingleDoorState(System.Int32,Fusion.RpcInfo)", base.Object, 7);
			}
			return;
		}
		this.InvokeRpc = false;
		IL_12:
		GorillaNot.IncrementRPCCall(info, "RPC_RemoteSingleDoorState");
		if (!base.IsMine)
		{
			return;
		}
		if (doorIndex >= this.singleDoorCount)
		{
			return;
		}
		this.singleDoorOpen[doorIndex] = !this.singleDoorOpen[doorIndex];
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0001A6A1 File Offset: 0x000188A1
	[PunRPC]
	public void RemoteEntranceDoorState(GhostLab.EntranceDoorsState newState, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RemoteEntranceDoorState");
		if (!base.IsMine)
		{
			return;
		}
		this.doorState = newState;
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0001A6BE File Offset: 0x000188BE
	[PunRPC]
	public void RemoteSingleDoorState(int doorIndex, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RemoteSingleDoorState");
		if (!base.IsMine)
		{
			return;
		}
		if (doorIndex >= this.singleDoorCount)
		{
			return;
		}
		this.singleDoorOpen[doorIndex] = !this.singleDoorOpen[doorIndex];
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0001A6F1 File Offset: 0x000188F1
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.NetData = this._NetData;
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0001A709 File Offset: 0x00018909
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._NetData = this.NetData;
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0001A720 File Offset: 0x00018920
	[NetworkRpcWeavedInvoker(1, 7, 1)]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_RemoteEntranceDoorState@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		GhostLab.EntranceDoorsState entranceDoorsState = *(GhostLab.EntranceDoorsState*)(data + num);
		num += 4;
		GhostLab.EntranceDoorsState newState = entranceDoorsState;
		RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
		behaviour.InvokeRpc = true;
		((GhostLabReliableState)behaviour).RPC_RemoteEntranceDoorState(newState, info);
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0001A794 File Offset: 0x00018994
	[NetworkRpcWeavedInvoker(2, 7, 1)]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_RemoteSingleDoorState@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		int num2 = *(int*)(data + num);
		num += 4;
		int doorIndex = num2;
		RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
		behaviour.InvokeRpc = true;
		((GhostLabReliableState)behaviour).RPC_RemoteSingleDoorState(doorIndex, info);
	}

	// Token: 0x0400051B RID: 1307
	public GhostLab.EntranceDoorsState doorState;

	// Token: 0x0400051C RID: 1308
	public int singleDoorCount;

	// Token: 0x0400051D RID: 1309
	public bool[] singleDoorOpen;

	// Token: 0x0400051E RID: 1310
	[WeaverGenerated]
	[DefaultForProperty("NetData", 0, 11)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private GhostLabData _NetData;
}
