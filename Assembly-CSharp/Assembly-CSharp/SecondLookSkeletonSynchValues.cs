using System;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020000B6 RID: 182
[NetworkBehaviourWeaved(11)]
public class SecondLookSkeletonSynchValues : NetworkComponent
{
	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001C2A2 File Offset: 0x0001A4A2
	// (set) Token: 0x060004B1 RID: 1201 RVA: 0x0001C2CC File Offset: 0x0001A4CC
	[Networked]
	[NetworkedWeaved(0, 11)]
	public unsafe SkeletonNetData NetData
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing SecondLookSkeletonSynchValues.NetData. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(SkeletonNetData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing SecondLookSkeletonSynchValues.NetData. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(SkeletonNetData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0001C2F7 File Offset: 0x0001A4F7
	protected override void OnOwnerSwitched(NetPlayer newOwningPlayer)
	{
		base.OnOwnerSwitched(newOwningPlayer);
		if (newOwningPlayer.IsLocal)
		{
			this.mySkeleton.SetNodes();
			if (this.mySkeleton.currentState != this.currentState)
			{
				this.mySkeleton.ChangeState(this.currentState);
			}
		}
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x0001C337 File Offset: 0x0001A537
	public override void WriteDataFusion()
	{
		this.NetData = new SkeletonNetData((int)this.currentState, this.position, this.rotation, this.currentNode, this.nextNode, this.angerPoint);
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x0001C368 File Offset: 0x0001A568
	public override void ReadDataFusion()
	{
		this.currentState = (SecondLookSkeleton.GhostState)this.NetData.CurrentState;
		Vector3 vector = this.NetData.Position;
		ref this.position.SetValueSafe(vector);
		Quaternion quaternion = this.NetData.Rotation;
		ref this.rotation.SetValueSafe(quaternion);
		this.currentNode = this.NetData.CurrentNode;
		this.nextNode = this.NetData.NextNode;
		this.angerPoint = this.NetData.AngerPoint;
		if (this.mySkeleton.tapped && this.currentState != this.mySkeleton.currentState)
		{
			this.mySkeleton.ChangeState(this.currentState);
		}
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0001C430 File Offset: 0x0001A630
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!base.IsMine && !info.Sender.IsMasterClient)
		{
			return;
		}
		stream.SendNext(this.mySkeleton.currentState);
		stream.SendNext(this.mySkeleton.spookyGhost.transform.position);
		stream.SendNext(this.mySkeleton.spookyGhost.transform.rotation);
		stream.SendNext(this.currentNode);
		stream.SendNext(this.nextNode);
		stream.SendNext(this.angerPoint);
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0001C4DC File Offset: 0x0001A6DC
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!base.IsMine && !info.Sender.IsMasterClient)
		{
			return;
		}
		this.currentState = (SecondLookSkeleton.GhostState)stream.ReceiveNext();
		Vector3 vector = (Vector3)stream.ReceiveNext();
		ref this.position.SetValueSafe(vector);
		Quaternion quaternion = (Quaternion)stream.ReceiveNext();
		ref this.rotation.SetValueSafe(quaternion);
		this.currentNode = (int)stream.ReceiveNext();
		this.nextNode = (int)stream.ReceiveNext();
		this.angerPoint = (int)stream.ReceiveNext();
		if (this.mySkeleton.tapped && this.currentState != this.mySkeleton.currentState)
		{
			this.mySkeleton.ChangeState(this.currentState);
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x0001C5A8 File Offset: 0x0001A7A8
	[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
	public unsafe void RPC_RemoteActiveGhost(RpcInfo info = default(RpcInfo))
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
							int capacityInBytes = 8;
							SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, capacityInBytes);
							byte* data = SimulationMessage.GetData(ptr);
							int num = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 1), data);
							ptr->Offset = num * 8;
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
				NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void SecondLookSkeletonSynchValues::RPC_RemoteActiveGhost(Fusion.RpcInfo)", base.Object, 7);
			}
			return;
		}
		this.InvokeRpc = false;
		IL_12:
		GorillaNot.IncrementRPCCall(info, "RPC_RemoteActiveGhost");
		if (!base.IsMine)
		{
			return;
		}
		this.mySkeleton.RemoteActivateGhost();
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x0001C6EC File Offset: 0x0001A8EC
	[Rpc(RpcSources.All, RpcTargets.All)]
	public unsafe void RPC_RemotePlayerSeen(RpcInfo info = default(RpcInfo))
	{
		if (!this.InvokeRpc)
		{
			NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
			if (base.Runner.Stage != SimulationStages.Resimulate)
			{
				int localAuthorityMask = base.Object.GetLocalAuthorityMask();
				if ((localAuthorityMask & 7) == 0)
				{
					NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void SecondLookSkeletonSynchValues::RPC_RemotePlayerSeen(Fusion.RpcInfo)", base.Object, 7);
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
		GorillaNot.IncrementRPCCall(info, "RPC_RemotePlayerSeen");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Source);
		if (!this.mySkeleton.playersSeen.Contains(player))
		{
			this.mySkeleton.RemotePlayerSeen(player);
		}
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0001C840 File Offset: 0x0001AA40
	[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
	public unsafe void RPC_RemotePlayerCaught(RpcInfo info = default(RpcInfo))
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
							int capacityInBytes = 8;
							SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, capacityInBytes);
							byte* data = SimulationMessage.GetData(ptr);
							int num = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 3), data);
							ptr->Offset = num * 8;
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
				NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void SecondLookSkeletonSynchValues::RPC_RemotePlayerCaught(Fusion.RpcInfo)", base.Object, 7);
			}
			return;
		}
		this.InvokeRpc = false;
		IL_12:
		GorillaNot.IncrementRPCCall(info, "RPC_RemotePlayerCaught");
		if (!base.IsMine)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Source);
		if (this.mySkeleton.currentState == SecondLookSkeleton.GhostState.Chasing)
		{
			this.mySkeleton.RemotePlayerCaught(player);
		}
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0001C9A1 File Offset: 0x0001ABA1
	[PunRPC]
	public void RemoteActivateGhost(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RemoteActivateGhost");
		if (!base.IsMine)
		{
			return;
		}
		this.mySkeleton.RemoteActivateGhost();
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0001C9C4 File Offset: 0x0001ABC4
	[PunRPC]
	public void RemotePlayerSeen(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RemotePlayerSeen");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		if (!this.mySkeleton.playersSeen.Contains(player))
		{
			this.mySkeleton.RemotePlayerSeen(player);
		}
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0001CA0C File Offset: 0x0001AC0C
	[PunRPC]
	public void RemotePlayerCaught(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RemotePlayerCaught");
		if (!base.IsMine)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		if (this.mySkeleton.currentState == SecondLookSkeleton.GhostState.Chasing)
		{
			this.mySkeleton.RemotePlayerCaught(player);
		}
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0001CA58 File Offset: 0x0001AC58
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.NetData = this._NetData;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0001CA70 File Offset: 0x0001AC70
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._NetData = this.NetData;
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0001CA84 File Offset: 0x0001AC84
	[NetworkRpcWeavedInvoker(1, 7, 1)]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_RemoteActiveGhost@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
		behaviour.InvokeRpc = true;
		((SecondLookSkeletonSynchValues)behaviour).RPC_RemoteActiveGhost(info);
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x0001CAD8 File Offset: 0x0001ACD8
	[NetworkRpcWeavedInvoker(2, 7, 7)]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_RemotePlayerSeen@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
		behaviour.InvokeRpc = true;
		((SecondLookSkeletonSynchValues)behaviour).RPC_RemotePlayerSeen(info);
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x0001CB2C File Offset: 0x0001AD2C
	[NetworkRpcWeavedInvoker(3, 7, 1)]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_RemotePlayerCaught@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
		behaviour.InvokeRpc = true;
		((SecondLookSkeletonSynchValues)behaviour).RPC_RemotePlayerCaught(info);
	}

	// Token: 0x0400056C RID: 1388
	public SecondLookSkeleton.GhostState currentState;

	// Token: 0x0400056D RID: 1389
	public Vector3 position;

	// Token: 0x0400056E RID: 1390
	public Quaternion rotation;

	// Token: 0x0400056F RID: 1391
	public SecondLookSkeleton mySkeleton;

	// Token: 0x04000570 RID: 1392
	public int currentNode;

	// Token: 0x04000571 RID: 1393
	public int nextNode;

	// Token: 0x04000572 RID: 1394
	public int angerPoint;

	// Token: 0x04000573 RID: 1395
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("NetData", 0, 11)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private SkeletonNetData _NetData;
}
