﻿using System;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200045F RID: 1119
[NetworkBehaviourWeaved(31)]
public class BattleGameModeData : FusionGameModeData
{
	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06001B6F RID: 7023 RVA: 0x00086FB7 File Offset: 0x000851B7
	// (set) Token: 0x06001B70 RID: 7024 RVA: 0x00086FE1 File Offset: 0x000851E1
	[Networked]
	[NetworkedWeaved(0, 31)]
	private unsafe PaintbrawlData PaintbrawlData
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing BattleGameModeData.PaintbrawlData. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(PaintbrawlData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing BattleGameModeData.PaintbrawlData. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(PaintbrawlData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06001B71 RID: 7025 RVA: 0x0008700C File Offset: 0x0008520C
	// (set) Token: 0x06001B72 RID: 7026 RVA: 0x00087019 File Offset: 0x00085219
	public override object Data
	{
		get
		{
			return this.PaintbrawlData;
		}
		set
		{
			this.PaintbrawlData = (PaintbrawlData)value;
		}
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x00087027 File Offset: 0x00085227
	public override void Spawned()
	{
		this.serializer = base.GetComponent<GameModeSerializer>();
		this.battleTarget = (GorillaPaintbrawlManager)this.serializer.GameModeInstance;
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x0008704C File Offset: 0x0008524C
	[Rpc]
	public unsafe void RPC_ReportSlinshotHit(int taggedPlayerID, Vector3 hitLocation, int projectileCount, RpcInfo rpcInfo = default(RpcInfo))
	{
		if (!this.InvokeRpc)
		{
			NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
			if (base.Runner.Stage != SimulationStages.Resimulate)
			{
				int localAuthorityMask = base.Object.GetLocalAuthorityMask();
				if ((localAuthorityMask & 7) == 0)
				{
					NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void BattleGameModeData::RPC_ReportSlinshotHit(System.Int32,UnityEngine.Vector3,System.Int32,Fusion.RpcInfo)", base.Object, 7);
				}
				else
				{
					if (base.Runner.HasAnyActiveConnections())
					{
						int num = 8;
						num += 4;
						num += 12;
						num += 4;
						SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, num);
						byte* data = SimulationMessage.GetData(ptr);
						int num2 = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 1), data);
						*(int*)(data + num2) = taggedPlayerID;
						num2 += 4;
						*(Vector3*)(data + num2) = hitLocation;
						num2 += 12;
						*(int*)(data + num2) = projectileCount;
						num2 += 4;
						ptr->Offset = num2 * 8;
						base.Runner.SendRpc(ptr);
					}
					if ((localAuthorityMask & 7) != 0)
					{
						rpcInfo = RpcInfo.FromLocal(base.Runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
						goto IL_12;
					}
				}
			}
			return;
		}
		this.InvokeRpc = false;
		IL_12:
		PhotonMessageInfoWrapped photonMessageInfoWrapped = new PhotonMessageInfoWrapped(rpcInfo);
		GorillaNot.IncrementRPCCall(photonMessageInfoWrapped, "RPC_ReportSlinshotHit");
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(taggedPlayerID);
		this.battleTarget.ReportSlingshotHit(player, hitLocation, projectileCount, photonMessageInfoWrapped);
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x00087213 File Offset: 0x00085413
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.PaintbrawlData = this._PaintbrawlData;
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x0008722B File Offset: 0x0008542B
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._PaintbrawlData = this.PaintbrawlData;
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x00087240 File Offset: 0x00085440
	[NetworkRpcWeavedInvoker(1, 7, 7)]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_ReportSlinshotHit@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		int num2 = *(int*)(data + num);
		num += 4;
		int taggedPlayerID = num2;
		Vector3 vector = *(Vector3*)(data + num);
		num += 12;
		Vector3 hitLocation = vector;
		int num3 = *(int*)(data + num);
		num += 4;
		int projectileCount = num3;
		RpcInfo rpcInfo = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
		behaviour.InvokeRpc = true;
		((BattleGameModeData)behaviour).RPC_ReportSlinshotHit(taggedPlayerID, hitLocation, projectileCount, rpcInfo);
	}

	// Token: 0x04001E77 RID: 7799
	[WeaverGenerated]
	[DefaultForProperty("PaintbrawlData", 0, 31)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private PaintbrawlData _PaintbrawlData;

	// Token: 0x04001E78 RID: 7800
	private GorillaPaintbrawlManager battleTarget;

	// Token: 0x04001E79 RID: 7801
	private GameModeSerializer serializer;
}
