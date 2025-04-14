using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200025D RID: 605
[NetworkBehaviourWeaved(0)]
public class FusionPlayerProperties : NetworkBehaviour
{
	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06000E17 RID: 3607 RVA: 0x000476B0 File Offset: 0x000458B0
	[Capacity(10)]
	private NetworkDictionary<PlayerRef, FusionPlayerProperties.PlayerInfo> netPlayerAttributes
	{
		get
		{
			return default(NetworkDictionary<PlayerRef, FusionPlayerProperties.PlayerInfo>);
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x000476C8 File Offset: 0x000458C8
	public FusionPlayerProperties.PlayerInfo PlayerProperties
	{
		get
		{
			return this.netPlayerAttributes[base.Runner.LocalPlayer];
		}
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x000476EE File Offset: 0x000458EE
	private void OnAttributesChanged()
	{
		FusionPlayerProperties.PlayerAttributeOnChanged playerAttributeOnChanged = this.playerAttributeOnChanged;
		if (playerAttributeOnChanged == null)
		{
			return;
		}
		playerAttributeOnChanged();
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x00047700 File Offset: 0x00045900
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

	// Token: 0x06000E1B RID: 3611 RVA: 0x000478FD File Offset: 0x00045AFD
	public override void Spawned()
	{
		Debug.Log("Player props SPAWNED!");
		if (base.Runner.Mode == SimulationModes.Client)
		{
			Debug.Log("SET Player Properties manager!");
		}
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x00047924 File Offset: 0x00045B24
	public string GetDisplayName(PlayerRef player)
	{
		return this.netPlayerAttributes[player].NickName.Value;
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x00047950 File Offset: 0x00045B50
	public string GetLocalDisplayName()
	{
		return this.netPlayerAttributes[base.Runner.LocalPlayer].NickName.Value;
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x00047988 File Offset: 0x00045B88
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

	// Token: 0x06000E1F RID: 3615 RVA: 0x000479D0 File Offset: 0x00045BD0
	public bool PlayerHasEntry(PlayerRef player)
	{
		return this.netPlayerAttributes.ContainsKey(player);
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x000479EC File Offset: 0x00045BEC
	public void RemovePlayerEntry(PlayerRef player)
	{
		if (base.Object.HasStateAuthority)
		{
			string value = this.netPlayerAttributes[player].NickName.Value;
			this.netPlayerAttributes.Remove(player);
			Debug.Log("Removed " + value + "player properties as they just left.");
		}
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x00047A54 File Offset: 0x00045C54
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

	// Token: 0x040010EC RID: 4332
	public FusionPlayerProperties.PlayerAttributeOnChanged playerAttributeOnChanged;

	// Token: 0x0200025E RID: 606
	[NetworkStructWeaved(240)]
	[StructLayout(LayoutKind.Explicit, Size = 960)]
	public struct PlayerInfo : INetworkStruct
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x00047ACB File Offset: 0x00045CCB
		// (set) Token: 0x06000E26 RID: 3622 RVA: 0x00047ADD File Offset: 0x00045CDD
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

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x00047AF0 File Offset: 0x00045CF0
		[Networked]
		public unsafe NetworkDictionary<NetworkString<_32>, NetworkString<_32>> properties
		{
			get
			{
				return new NetworkDictionary<NetworkString<_32>, NetworkString<_32>>((int*)Native.ReferenceToPointer<FixedStorage@207>(ref this._properties), 3, ReaderWriter@Fusion_NetworkString.GetInstance(), ReaderWriter@Fusion_NetworkString.GetInstance());
			}
		}

		// Token: 0x040010ED RID: 4333
		[FixedBufferProperty(typeof(NetworkString<_32>), typeof(UnityValueSurrogate@ReaderWriter@Fusion_NetworkString), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@33 _NickName;

		// Token: 0x040010EE RID: 4334
		[FixedBufferProperty(typeof(NetworkDictionary<NetworkString<_32>, NetworkString<_32>>), typeof(UnityDictionarySurrogate@ReaderWriter@Fusion_NetworkString`1<Fusion__32>@ReaderWriter@Fusion_NetworkString), 3, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(132)]
		private FixedStorage@207 _properties;
	}

	// Token: 0x0200025F RID: 607
	// (Invoke) Token: 0x06000E29 RID: 3625
	public delegate void PlayerAttributeOnChanged();
}
