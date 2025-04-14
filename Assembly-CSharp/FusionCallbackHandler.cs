using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000259 RID: 601
public class FusionCallbackHandler : SimulationBehaviour, INetworkRunnerCallbacks
{
	// Token: 0x06000DE1 RID: 3553 RVA: 0x00046890 File Offset: 0x00044A90
	public void Setup(NetworkSystemFusion parentController)
	{
		this.parent = parentController;
		this.parent.runner.AddCallbacks(new INetworkRunnerCallbacks[]
		{
			this
		});
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x000468B3 File Offset: 0x00044AB3
	private void OnDestroy()
	{
		NetworkBehaviourUtils.InternalOnDestroy(this);
		this.RemoveCallbacks();
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x000468C4 File Offset: 0x00044AC4
	private void RemoveCallbacks()
	{
		FusionCallbackHandler.<RemoveCallbacks>d__3 <RemoveCallbacks>d__;
		<RemoveCallbacks>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<RemoveCallbacks>d__.<>4__this = this;
		<RemoveCallbacks>d__.<>1__state = -1;
		<RemoveCallbacks>d__.<>t__builder.Start<FusionCallbackHandler.<RemoveCallbacks>d__3>(ref <RemoveCallbacks>d__);
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x000468FB File Offset: 0x00044AFB
	public void OnConnectedToServer(NetworkRunner runner)
	{
		this.parent.OnJoinedSession();
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x00046908 File Offset: 0x00044B08
	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{
		this.parent.OnJoinFailed(reason);
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x00046918 File Offset: 0x00044B18
	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
	{
		this.parent.CustomAuthenticationResponse(data);
		Debug.Log("Received custom auth response:");
		foreach (KeyValuePair<string, object> keyValuePair in data)
		{
			Debug.Log(keyValuePair.Key + ":" + (keyValuePair.Value as string));
		}
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x00046998 File Offset: 0x00044B98
	public void OnDisconnectedFromServer(NetworkRunner runner)
	{
		this.parent.OnDisconnectedFromSession();
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x000469A5 File Offset: 0x00044BA5
	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{
		this.parent.MigrateHost(runner, hostMigrationToken);
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x000469B4 File Offset: 0x00044BB4
	public void OnInput(NetworkRunner runner, NetworkInput input)
	{
		NetworkedInput input2 = NetInput.GetInput();
		input.Set<NetworkedInput>(input2);
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x000469D0 File Offset: 0x00044BD0
	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
		this.parent.OnFusionPlayerJoined(player);
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x000469DE File Offset: 0x00044BDE
	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
		this.parent.OnFusionPlayerLeft(player);
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
	{
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnSceneLoadDone(NetworkRunner runner)
	{
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnSceneLoadStart(NetworkRunner runner)
	{
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x000469EC File Offset: 0x00044BEC
	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
		this.parent.OnRunnerShutDown();
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
	{
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x000469FC File Offset: 0x00044BFC
	[Rpc(Channel = RpcChannel.Reliable)]
	public unsafe static void RPC_OnEventRaisedReliable(NetworkRunner runner, byte eventCode, byte[] byteData, bool hasOps, byte[] netOptsData, RpcInfo info = default(RpcInfo))
	{
		if (NetworkBehaviourUtils.InvokeRpc)
		{
			NetworkBehaviourUtils.InvokeRpc = false;
		}
		else
		{
			if (runner == null)
			{
				throw new ArgumentNullException("runner");
			}
			if (runner.Stage == SimulationStages.Resimulate)
			{
				return;
			}
			if (runner.HasAnyActiveConnections())
			{
				int num = 8;
				num += 4;
				num += (byteData.Length * 1 + 4 + 3 & -4);
				num += 4;
				num += (netOptsData.Length * 1 + 4 + 3 & -4);
				SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
				byte* data = SimulationMessage.GetData(ptr);
				int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void FusionCallbackHandler::RPC_OnEventRaisedReliable(Fusion.NetworkRunner,System.Byte,System.Byte[],System.Boolean,System.Byte[],Fusion.RpcInfo)")), data);
				data[num2] = eventCode;
				num2 += (1 + 3 & -4);
				*(int*)(data + num2) = byteData.Length;
				num2 += 4;
				num2 = (Native.CopyFromArray<byte>((void*)(data + num2), byteData) + 3 & -4) + num2;
				ReadWriteUtilsForWeaver.WriteBoolean((int*)(data + num2), hasOps);
				num2 += 4;
				*(int*)(data + num2) = netOptsData.Length;
				num2 += 4;
				num2 = (Native.CopyFromArray<byte>((void*)(data + num2), netOptsData) + 3 & -4) + num2;
				ptr->Offset = num2 * 8;
				ptr->SetStatic();
				runner.SendRpc(ptr);
			}
			info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
		}
		object data2 = byteData.ByteDeserialize();
		NetEventOptions opts = null;
		if (hasOps)
		{
			opts = (NetEventOptions)netOptsData.ByteDeserialize();
		}
		if (!FusionCallbackHandler.CanRecieveEvent(runner, opts, info))
		{
			return;
		}
		NetworkSystem.Instance.RaiseEvent(eventCode, data2, info.Source.PlayerId);
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x00046C00 File Offset: 0x00044E00
	[Rpc(Channel = RpcChannel.Unreliable)]
	public unsafe static void RPC_OnEventRaisedUnreliable(NetworkRunner runner, byte eventCode, byte[] byteData, bool hasOps, byte[] netOptsData, RpcInfo info = default(RpcInfo))
	{
		if (NetworkBehaviourUtils.InvokeRpc)
		{
			NetworkBehaviourUtils.InvokeRpc = false;
		}
		else
		{
			if (runner == null)
			{
				throw new ArgumentNullException("runner");
			}
			if (runner.Stage == SimulationStages.Resimulate)
			{
				return;
			}
			if (runner.HasAnyActiveConnections())
			{
				int num = 8;
				num += 4;
				num += (byteData.Length * 1 + 4 + 3 & -4);
				num += 4;
				num += (netOptsData.Length * 1 + 4 + 3 & -4);
				SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
				byte* data = SimulationMessage.GetData(ptr);
				int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void FusionCallbackHandler::RPC_OnEventRaisedUnreliable(Fusion.NetworkRunner,System.Byte,System.Byte[],System.Boolean,System.Byte[],Fusion.RpcInfo)")), data);
				data[num2] = eventCode;
				num2 += (1 + 3 & -4);
				*(int*)(data + num2) = byteData.Length;
				num2 += 4;
				num2 = (Native.CopyFromArray<byte>((void*)(data + num2), byteData) + 3 & -4) + num2;
				ReadWriteUtilsForWeaver.WriteBoolean((int*)(data + num2), hasOps);
				num2 += 4;
				*(int*)(data + num2) = netOptsData.Length;
				num2 += 4;
				num2 = (Native.CopyFromArray<byte>((void*)(data + num2), netOptsData) + 3 & -4) + num2;
				ptr->Offset = num2 * 8;
				ptr->SetUnreliable();
				ptr->SetStatic();
				runner.SendRpc(ptr);
			}
			info = RpcInfo.FromLocal(runner, RpcChannel.Unreliable, RpcHostMode.SourceIsServer);
		}
		object data2 = byteData.ByteDeserialize();
		NetEventOptions opts = null;
		if (hasOps)
		{
			opts = (NetEventOptions)netOptsData.ByteDeserialize();
		}
		if (!FusionCallbackHandler.CanRecieveEvent(runner, opts, info))
		{
			return;
		}
		NetworkSystem.Instance.RaiseEvent(eventCode, data2, info.Source.PlayerId);
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x00046E0C File Offset: 0x0004500C
	private static bool CanRecieveEvent(NetworkRunner runner, NetEventOptions opts, RpcInfo info)
	{
		if (opts != null)
		{
			if (opts.Reciever != NetEventOptions.RecieverTarget.all)
			{
				if (opts.Reciever == NetEventOptions.RecieverTarget.master && !NetworkSystem.Instance.IsMasterClient)
				{
					return false;
				}
				if (info.Source == runner.LocalPlayer)
				{
					return false;
				}
			}
			if (opts.TargetActors != null && !opts.TargetActors.Contains(runner.LocalPlayer.PlayerId))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
	{
	}

	// Token: 0x06000DFA RID: 3578 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
	{
	}

	// Token: 0x06000DFB RID: 3579 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
	{
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x00046E80 File Offset: 0x00045080
	[NetworkRpcStaticWeavedInvoker("System.Void FusionCallbackHandler::RPC_OnEventRaisedReliable(Fusion.NetworkRunner,System.Byte,System.Byte[],System.Boolean,System.Byte[],Fusion.RpcInfo)")]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_OnEventRaisedReliable@Invoker(NetworkRunner runner, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		byte b = data[num];
		num += (1 + 3 & -4);
		byte eventCode = b;
		byte[] array = new byte[*(int*)(data + num)];
		num += 4;
		num = (Native.CopyToArray<byte>(array, (void*)(data + num)) + 3 & -4) + num;
		bool flag = ReadWriteUtilsForWeaver.ReadBoolean((int*)(data + num));
		num += 4;
		bool hasOps = flag;
		byte[] array2 = new byte[*(int*)(data + num)];
		num += 4;
		num = (Native.CopyToArray<byte>(array2, (void*)(data + num)) + 3 & -4) + num;
		RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
		NetworkBehaviourUtils.InvokeRpc = true;
		FusionCallbackHandler.RPC_OnEventRaisedReliable(runner, eventCode, array, hasOps, array2, info);
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x00046F9C File Offset: 0x0004519C
	[NetworkRpcStaticWeavedInvoker("System.Void FusionCallbackHandler::RPC_OnEventRaisedUnreliable(Fusion.NetworkRunner,System.Byte,System.Byte[],System.Boolean,System.Byte[],Fusion.RpcInfo)")]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_OnEventRaisedUnreliable@Invoker(NetworkRunner runner, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		byte b = data[num];
		num += (1 + 3 & -4);
		byte eventCode = b;
		byte[] array = new byte[*(int*)(data + num)];
		num += 4;
		num = (Native.CopyToArray<byte>(array, (void*)(data + num)) + 3 & -4) + num;
		bool flag = ReadWriteUtilsForWeaver.ReadBoolean((int*)(data + num));
		num += 4;
		bool hasOps = flag;
		byte[] array2 = new byte[*(int*)(data + num)];
		num += 4;
		num = (Native.CopyToArray<byte>(array2, (void*)(data + num)) + 3 & -4) + num;
		RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
		NetworkBehaviourUtils.InvokeRpc = true;
		FusionCallbackHandler.RPC_OnEventRaisedUnreliable(runner, eventCode, array, hasOps, array2, info);
	}

	// Token: 0x040010E3 RID: 4323
	private NetworkSystemFusion parent;
}
