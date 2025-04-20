using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000268 RID: 616
[NetworkBehaviourWeaved(0)]
public class FusionPlayerProperties : NetworkBehaviour
{
	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000E62 RID: 3682 RVA: 0x000A4498 File Offset: 0x000A2698
	[Capacity(10)]
	private NetworkDictionary<PlayerRef, FusionPlayerProperties.PlayerInfo> netPlayerAttributes
	{
		get
		{
			return default(NetworkDictionary<PlayerRef, FusionPlayerProperties.PlayerInfo>);
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06000E63 RID: 3683 RVA: 0x000A44B0 File Offset: 0x000A26B0
	public FusionPlayerProperties.PlayerInfo PlayerProperties
	{
		get
		{
			return this.netPlayerAttributes[base.Runner.LocalPlayer];
		}
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x0003A38F File Offset: 0x0003858F
	private void OnAttributesChanged()
	{
		FusionPlayerProperties.PlayerAttributeOnChanged playerAttributeOnChanged = this.playerAttributeOnChanged;
		if (playerAttributeOnChanged == null)
		{
			return;
		}
		playerAttributeOnChanged();
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x000A44D8 File Offset: 0x000A26D8
	[Rpc(RpcSources.All, RpcTargets.All, InvokeLocal = true, TickAligned = true)]
	public unsafe void RPC_UpdatePlayerAttributes(FusionPlayerProperties.PlayerInfo newInfo, RpcInfo info = default(RpcInfo))
	{
		if (!this.InvokeRpc)
		{
			NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
			if (base.Runner.Stage != SimulationStages.Resimulate)
			{
				int localAuthorityMask = base.Object.GetLocalAuthorityMask();
				if ((localAuthorityMask & 7) == 0)
				{
					NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void FusionPlayerProperties::RPC_UpdatePlayerAttributes(FusionPlayerProperties/PlayerInfo,Fusion.RpcInfo)", base.Object, 7);
				}
				else
				{
					if (base.Runner.HasAnyActiveConnections())
					{
						int num = 8;
						num += 960;
						SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, num);
						byte* data = SimulationMessage.GetData(ptr);
						int num2 = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 1), data);
						*(FusionPlayerProperties.PlayerInfo*)(data + num2) = newInfo;
						num2 += 960;
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
		Debug.Log("Update Player attributes triggered");
		PlayerRef source = info.Source;
		if (this.netPlayerAttributes.ContainsKey(source))
		{
			Debug.Log("Current nickname is " + this.netPlayerAttributes[source].NickName.ToString());
			Debug.Log("Sent nickname is " + newInfo.NickName.ToString());
			if (this.netPlayerAttributes[source].Equals(newInfo))
			{
				Debug.Log("Info is already correct for this user. Shouldnt have received an RPC in this case.");
				return;
			}
		}
		this.netPlayerAttributes.Set(source, newInfo);
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x0003A3A1 File Offset: 0x000385A1
	public override void Spawned()
	{
		Debug.Log("Player props SPAWNED!");
		if (base.Runner.Mode == SimulationModes.Client)
		{
			Debug.Log("SET Player Properties manager!");
		}
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x000A46D8 File Offset: 0x000A28D8
	public string GetDisplayName(PlayerRef player)
	{
		return this.netPlayerAttributes[player].NickName.Value;
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x000A4704 File Offset: 0x000A2904
	public string GetLocalDisplayName()
	{
		return this.netPlayerAttributes[base.Runner.LocalPlayer].NickName.Value;
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x000A473C File Offset: 0x000A293C
	public bool GetProperty(PlayerRef player, string propertyName, out string propertyValue)
	{
		NetworkString<_32> networkString;
		if (this.netPlayerAttributes[player].properties.TryGet(propertyName, out networkString))
		{
			propertyValue = networkString.Value;
			return true;
		}
		propertyValue = null;
		return false;
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x000A4784 File Offset: 0x000A2984
	public bool PlayerHasEntry(PlayerRef player)
	{
		return this.netPlayerAttributes.ContainsKey(player);
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x000A47A0 File Offset: 0x000A29A0
	public void RemovePlayerEntry(PlayerRef player)
	{
		if (base.Object.HasStateAuthority)
		{
			string value = this.netPlayerAttributes[player].NickName.Value;
			this.netPlayerAttributes.Remove(player);
			Debug.Log("Removed " + value + "player properties as they just left.");
		}
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x00030607 File Offset: 0x0002E807
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x00030607 File Offset: 0x0002E807
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x000A4800 File Offset: 0x000A2A00
	[NetworkRpcWeavedInvoker(1, 7, 7)]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_UpdatePlayerAttributes@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		FusionPlayerProperties.PlayerInfo playerInfo = *(FusionPlayerProperties.PlayerInfo*)(data + num);
		num += 960;
		FusionPlayerProperties.PlayerInfo newInfo = playerInfo;
		RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
		behaviour.InvokeRpc = true;
		((FusionPlayerProperties)behaviour).RPC_UpdatePlayerAttributes(newInfo, info);
	}

	// Token: 0x04001132 RID: 4402
	public FusionPlayerProperties.PlayerAttributeOnChanged playerAttributeOnChanged;

	// Token: 0x02000269 RID: 617
	[NetworkStructWeaved(240)]
	[StructLayout(LayoutKind.Explicit, Size = 960)]
	public struct PlayerInfo : INetworkStruct
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0003A3CD File Offset: 0x000385CD
		// (set) Token: 0x06000E71 RID: 3697 RVA: 0x0003A3DF File Offset: 0x000385DF
		[Networked]
		public unsafe NetworkString<_32> NickName
		{
			readonly get
			{
				return *(NetworkString<_32>*)Native.ReferenceToPointer<FixedStorage@33>(ref this._NickName);
			}
			set
			{
				*(NetworkString<_32>*)Native.ReferenceToPointer<FixedStorage@33>(ref this._NickName) = value;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0003A3F2 File Offset: 0x000385F2
		[Networked]
		public unsafe NetworkDictionary<NetworkString<_32>, NetworkString<_32>> properties
		{
			get
			{
				return new NetworkDictionary<NetworkString<_32>, NetworkString<_32>>((int*)Native.ReferenceToPointer<FixedStorage@207>(ref this._properties), 3, ReaderWriter@Fusion_NetworkString.GetInstance(), ReaderWriter@Fusion_NetworkString.GetInstance());
			}
		}

		// Token: 0x04001133 RID: 4403
		[FixedBufferProperty(typeof(NetworkString<_32>), typeof(UnityValueSurrogate@ReaderWriter@Fusion_NetworkString), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@33 _NickName;

		// Token: 0x04001134 RID: 4404
		[FixedBufferProperty(typeof(NetworkDictionary<NetworkString<_32>, NetworkString<_32>>), typeof(UnityDictionarySurrogate@ReaderWriter@Fusion_NetworkString`1<Fusion__32>@ReaderWriter@Fusion_NetworkString), 3, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(132)]
		private FixedStorage@207 _properties;
	}

	// Token: 0x0200026A RID: 618
	// (Invoke) Token: 0x06000E74 RID: 3700
	public delegate void PlayerAttributeOnChanged();
}
