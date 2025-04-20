using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000264 RID: 612
public class FusionCallbackHandler : SimulationBehaviour, INetworkRunnerCallbacks
{
	// Token: 0x06000E2C RID: 3628 RVA: 0x0003A277 File Offset: 0x00038477
	public void Setup(NetworkSystemFusion parentController)
	{
		this.parent = parentController;
		this.parent.runner.AddCallbacks(new INetworkRunnerCallbacks[]
		{
			this
		});
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x0003A29A File Offset: 0x0003849A
	private void OnDestroy()
	{
		NetworkBehaviourUtils.InternalOnDestroy(this);
		this.RemoveCallbacks();
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x000A379C File Offset: 0x000A199C
	private void RemoveCallbacks()
	{
		FusionCallbackHandler.<RemoveCallbacks>d__3 <RemoveCallbacks>d__;
		<RemoveCallbacks>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<RemoveCallbacks>d__.<>4__this = this;
		<RemoveCallbacks>d__.<>1__state = -1;
		<RemoveCallbacks>d__.<>t__builder.Start<FusionCallbackHandler.<RemoveCallbacks>d__3>(ref <RemoveCallbacks>d__);
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x0003A2A8 File Offset: 0x000384A8
	public void OnConnectedToServer(NetworkRunner runner)
	{
		this.parent.OnJoinedSession();
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0003A2B5 File Offset: 0x000384B5
	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{
		this.parent.OnJoinFailed(reason);
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x000A37D4 File Offset: 0x000A19D4
	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
	{
		this.parent.CustomAuthenticationResponse(data);
		Debug.Log("Received custom auth response:");
		foreach (KeyValuePair<string, object> keyValuePair in data)
		{
			Debug.Log(keyValuePair.Key + ":" + (keyValuePair.Value as string));
		}
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x0003A2C3 File Offset: 0x000384C3
	public void OnDisconnectedFromServer(NetworkRunner runner)
	{
		this.parent.OnDisconnectedFromSession();
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x0003A2D0 File Offset: 0x000384D0
	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{
		this.parent.MigrateHost(runner, hostMigrationToken);
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x000A3854 File Offset: 0x000A1A54
	public void OnInput(NetworkRunner runner, NetworkInput input)
	{
		NetworkedInput input2 = NetInput.GetInput();
		input.Set<NetworkedInput>(input2);
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0003A2DF File Offset: 0x000384DF
	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
		this.parent.OnFusionPlayerJoined(player);
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0003A2ED File Offset: 0x000384ED
	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
		this.parent.OnFusionPlayerLeft(player);
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
	{
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnSceneLoadDone(NetworkRunner runner)
	{
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnSceneLoadStart(NetworkRunner runner)
	{
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x0003A2FB File Offset: 0x000384FB
	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
		this.parent.OnRunnerShutDown();
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
	{
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x000A3870 File Offset: 0x000A1A70
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

	// Token: 0x06000E40 RID: 3648 RVA: 0x000A3A74 File Offset: 0x000A1C74
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

	// Token: 0x06000E41 RID: 3649 RVA: 0x000A3C80 File Offset: 0x000A1E80
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

	// Token: 0x06000E42 RID: 3650 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
	{
	}

	// Token: 0x06000E45 RID: 3653 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
	{
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
	{
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x000A3CEC File Offset: 0x000A1EEC
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

	// Token: 0x06000E49 RID: 3657 RVA: 0x000A3E08 File Offset: 0x000A2008
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

	// Token: 0x04001129 RID: 4393
	private NetworkSystemFusion parent;
}
