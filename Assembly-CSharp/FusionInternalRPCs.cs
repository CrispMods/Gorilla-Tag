using System;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200025B RID: 603
public class FusionInternalRPCs : SimulationBehaviour
{
	// Token: 0x06000E01 RID: 3585 RVA: 0x00047196 File Offset: 0x00045396
	private void Awake()
	{
		FusionInternalRPCs.netSys = (NetworkSystem.Instance as NetworkSystemFusion);
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x000471A8 File Offset: 0x000453A8
	[Rpc(RpcSources.All, RpcTargets.All)]
	public unsafe static void RPC_SendPlayerSyncProp(NetworkRunner runner, [RpcTarget] PlayerRef player, PlayerRef playerData, string propKey, string propValue)
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
			if (runner.Stage != SimulationStages.Resimulate)
			{
				RpcTargetStatus rpcTargetStatus = runner.GetRpcTargetStatus(player);
				if (rpcTargetStatus == RpcTargetStatus.Unreachable)
				{
					NetworkBehaviourUtils.NotifyRpcTargetUnreachable(player, "System.Void FusionInternalRPCs::RPC_SendPlayerSyncProp(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.PlayerRef,System.String,System.String)");
				}
				else
				{
					if (rpcTargetStatus == RpcTargetStatus.Self)
					{
						goto IL_10;
					}
					int num = 8;
					num += 4;
					num += (ReadWriteUtilsForWeaver.GetByteCountUtf8NoHash(propKey) + 3 & -4);
					num += (ReadWriteUtilsForWeaver.GetByteCountUtf8NoHash(propValue) + 3 & -4);
					SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
					byte* data = SimulationMessage.GetData(ptr);
					int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void FusionInternalRPCs::RPC_SendPlayerSyncProp(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.PlayerRef,System.String,System.String)")), data);
					*(PlayerRef*)(data + num2) = playerData;
					num2 += 4;
					num2 = (ReadWriteUtilsForWeaver.WriteStringUtf8NoHash((void*)(data + num2), propKey) + 3 & -4) + num2;
					num2 = (ReadWriteUtilsForWeaver.WriteStringUtf8NoHash((void*)(data + num2), propValue) + 3 & -4) + num2;
					ptr->Offset = num2 * 8;
					ptr->SetTarget(player);
					ptr->SetStatic();
					runner.SendRpc(ptr);
				}
			}
			return;
		}
		IL_10:
		Debug.Log("RPC Setting player prop: " + propKey + " - " + propValue);
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x00047344 File Offset: 0x00045544
	[NetworkRpcStaticWeavedInvoker("System.Void FusionInternalRPCs::RPC_SendPlayerSyncProp(Fusion.NetworkRunner,Fusion.PlayerRef,Fusion.PlayerRef,System.String,System.String)")]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_SendPlayerSyncProp@Invoker(NetworkRunner runner, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		PlayerRef target = message->Target;
		PlayerRef playerRef = *(PlayerRef*)(data + num);
		num += 4;
		PlayerRef playerData = playerRef;
		string propKey;
		num = (ReadWriteUtilsForWeaver.ReadStringUtf8NoHash((void*)(data + num), out propKey) + 3 & -4) + num;
		string propValue;
		num = (ReadWriteUtilsForWeaver.ReadStringUtf8NoHash((void*)(data + num), out propValue) + 3 & -4) + num;
		NetworkBehaviourUtils.InvokeRpc = true;
		FusionInternalRPCs.RPC_SendPlayerSyncProp(runner, target, playerData, propKey, propValue);
	}

	// Token: 0x040010E8 RID: 4328
	private static NetworkSystemFusion netSys;
}
