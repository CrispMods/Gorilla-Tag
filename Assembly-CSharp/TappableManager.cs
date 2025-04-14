using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fusion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005EA RID: 1514
public class TappableManager : NetworkSceneObject
{
	// Token: 0x060025A5 RID: 9637 RVA: 0x000B9D8C File Offset: 0x000B7F8C
	private void Awake()
	{
		if (TappableManager.gManager != null && TappableManager.gManager != this)
		{
			GTDev.LogWarning<string>("Instance of TappableManager already exists. Destroying.", null);
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (TappableManager.gManager == null)
		{
			TappableManager.gManager = this;
		}
		if (TappableManager.gRegistry.Count == 0)
		{
			return;
		}
		Tappable[] array = TappableManager.gRegistry.ToArray<Tappable>();
		for (int i = 0; i < array.Length; i++)
		{
			if (!(array[i] == null))
			{
				this.RegisterInstance(array[i]);
			}
		}
		TappableManager.gRegistry.Clear();
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x000B9E1C File Offset: 0x000B801C
	private void RegisterInstance(Tappable t)
	{
		if (t == null)
		{
			GTDev.LogError<string>("Tappable is null.", null);
			return;
		}
		t.manager = this;
		if (this.idSet.Add(t.tappableId))
		{
			this.tappables.Add(t);
		}
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x000B9E59 File Offset: 0x000B8059
	private void UnregisterInstance(Tappable t)
	{
		if (t == null)
		{
			return;
		}
		if (!this.idSet.Remove(t.tappableId))
		{
			return;
		}
		this.tappables.Remove(t);
		t.manager = null;
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x000B9E8D File Offset: 0x000B808D
	public static void Register(Tappable t)
	{
		if (TappableManager.gManager != null)
		{
			TappableManager.gManager.RegisterInstance(t);
			return;
		}
		TappableManager.gRegistry.Add(t);
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x000B9EB4 File Offset: 0x000B80B4
	public static void Unregister(Tappable t)
	{
		if (TappableManager.gManager != null)
		{
			TappableManager.gManager.UnregisterInstance(t);
			return;
		}
		TappableManager.gRegistry.Remove(t);
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x000B9EDC File Offset: 0x000B80DC
	[Conditional("QATESTING")]
	public void DebugTestTap()
	{
		if (this.tappables.Count > 0)
		{
			int index = Random.Range(0, this.tappables.Count);
			Debug.Log("Send TestTap to tappable index: " + index.ToString() + "/" + this.tappables.Count.ToString());
			this.tappables[index].OnTap(10f);
			return;
		}
		Debug.Log("TappableManager: tappables array is empty.");
	}

	// Token: 0x060025AB RID: 9643 RVA: 0x000B9F58 File Offset: 0x000B8158
	[PunRPC]
	public void SendOnTapRPC(int key, float tapStrength, PhotonMessageInfo info)
	{
		this.SendOnTapShared(key, tapStrength, new PhotonMessageInfoWrapped(info));
	}

	// Token: 0x060025AC RID: 9644 RVA: 0x000B9F68 File Offset: 0x000B8168
	[Rpc]
	public unsafe static void RPC_SendOnTap(NetworkRunner runner, int key, float tapStrength, RpcInfo info = default(RpcInfo))
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
				num += 4;
				SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
				byte* data = SimulationMessage.GetData(ptr);
				int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void TappableManager::RPC_SendOnTap(Fusion.NetworkRunner,System.Int32,System.Single,Fusion.RpcInfo)")), data);
				*(int*)(data + num2) = key;
				num2 += 4;
				*(float*)(data + num2) = tapStrength;
				num2 += 4;
				ptr->Offset = num2 * 8;
				ptr->SetStatic();
				runner.SendRpc(ptr);
			}
			info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
		}
		TappableManager.gManager.SendOnTapShared(key, tapStrength, new PhotonMessageInfoWrapped(info));
	}

	// Token: 0x060025AD RID: 9645 RVA: 0x000BA084 File Offset: 0x000B8284
	private void SendOnTapShared(int key, float tapStrength, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "SendOnTapShared");
		if (key == 0 || !float.IsFinite(tapStrength))
		{
			return;
		}
		tapStrength = Mathf.Clamp(tapStrength, 0f, 1f);
		for (int i = 0; i < this.tappables.Count; i++)
		{
			Tappable tappable = this.tappables[i];
			if (tappable.tappableId == key)
			{
				tappable.OnTapLocal(tapStrength, Time.time, info);
			}
		}
	}

	// Token: 0x060025AE RID: 9646 RVA: 0x000BA0F3 File Offset: 0x000B82F3
	[PunRPC]
	public void SendOnGrabRPC(int key, PhotonMessageInfo info)
	{
		this.SendOnGrabShared(key, new PhotonMessageInfoWrapped(info));
	}

	// Token: 0x060025AF RID: 9647 RVA: 0x000BA104 File Offset: 0x000B8304
	[Rpc]
	public unsafe static void RPC_SendOnGrab(NetworkRunner runner, int key, RpcInfo info = default(RpcInfo))
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
				SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
				byte* data = SimulationMessage.GetData(ptr);
				int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void TappableManager::RPC_SendOnGrab(Fusion.NetworkRunner,System.Int32,Fusion.RpcInfo)")), data);
				*(int*)(data + num2) = key;
				num2 += 4;
				ptr->Offset = num2 * 8;
				ptr->SetStatic();
				runner.SendRpc(ptr);
			}
			info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
		}
		TappableManager.gManager.SendOnGrabShared(key, new PhotonMessageInfoWrapped(info));
	}

	// Token: 0x060025B0 RID: 9648 RVA: 0x000BA1FC File Offset: 0x000B83FC
	private void SendOnGrabShared(int key, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "SendOnGrabShared");
		if (key == 0)
		{
			return;
		}
		for (int i = 0; i < this.tappables.Count; i++)
		{
			Tappable tappable = this.tappables[i];
			if (tappable.tappableId == key)
			{
				tappable.OnGrabLocal(Time.time, info);
			}
		}
	}

	// Token: 0x060025B1 RID: 9649 RVA: 0x000BA250 File Offset: 0x000B8450
	[PunRPC]
	public void SendOnReleaseRPC(int key, PhotonMessageInfo info)
	{
		this.SendOnReleaseShared(key, new PhotonMessageInfoWrapped(info));
	}

	// Token: 0x060025B2 RID: 9650 RVA: 0x000BA260 File Offset: 0x000B8460
	[Rpc]
	public unsafe static void RPC_SendOnRelease(NetworkRunner runner, int key, RpcInfo info = default(RpcInfo))
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
				SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
				byte* data = SimulationMessage.GetData(ptr);
				int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void TappableManager::RPC_SendOnRelease(Fusion.NetworkRunner,System.Int32,Fusion.RpcInfo)")), data);
				*(int*)(data + num2) = key;
				num2 += 4;
				ptr->Offset = num2 * 8;
				ptr->SetStatic();
				runner.SendRpc(ptr);
			}
			info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
		}
		TappableManager.gManager.SendOnReleaseShared(key, new PhotonMessageInfoWrapped(info));
	}

	// Token: 0x060025B3 RID: 9651 RVA: 0x000BA358 File Offset: 0x000B8558
	public void SendOnReleaseShared(int key, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "SendOnReleaseShared");
		if (key == 0)
		{
			return;
		}
		for (int i = 0; i < this.tappables.Count; i++)
		{
			Tappable tappable = this.tappables[i];
			if (tappable.tappableId == key)
			{
				tappable.OnReleaseLocal(Time.time, info);
			}
		}
	}

	// Token: 0x060025B6 RID: 9654 RVA: 0x000BA3D8 File Offset: 0x000B85D8
	[NetworkRpcStaticWeavedInvoker("System.Void TappableManager::RPC_SendOnTap(Fusion.NetworkRunner,System.Int32,System.Single,Fusion.RpcInfo)")]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_SendOnTap@Invoker(NetworkRunner runner, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		int num2 = *(int*)(data + num);
		num += 4;
		int key = num2;
		float num3 = *(float*)(data + num);
		num += 4;
		float tapStrength = num3;
		RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
		NetworkBehaviourUtils.InvokeRpc = true;
		TappableManager.RPC_SendOnTap(runner, key, tapStrength, info);
	}

	// Token: 0x060025B7 RID: 9655 RVA: 0x000BA460 File Offset: 0x000B8660
	[NetworkRpcStaticWeavedInvoker("System.Void TappableManager::RPC_SendOnGrab(Fusion.NetworkRunner,System.Int32,Fusion.RpcInfo)")]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_SendOnGrab@Invoker(NetworkRunner runner, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		int num2 = *(int*)(data + num);
		num += 4;
		int key = num2;
		RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
		NetworkBehaviourUtils.InvokeRpc = true;
		TappableManager.RPC_SendOnGrab(runner, key, info);
	}

	// Token: 0x060025B8 RID: 9656 RVA: 0x000BA4CC File Offset: 0x000B86CC
	[NetworkRpcStaticWeavedInvoker("System.Void TappableManager::RPC_SendOnRelease(Fusion.NetworkRunner,System.Int32,Fusion.RpcInfo)")]
	[Preserve]
	[WeaverGenerated]
	protected unsafe static void RPC_SendOnRelease@Invoker(NetworkRunner runner, SimulationMessage* message)
	{
		byte* data = SimulationMessage.GetData(message);
		int num = RpcHeader.ReadSize(data) + 3 & -4;
		int num2 = *(int*)(data + num);
		num += 4;
		int key = num2;
		RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
		NetworkBehaviourUtils.InvokeRpc = true;
		TappableManager.RPC_SendOnRelease(runner, key, info);
	}

	// Token: 0x040029D5 RID: 10709
	private static TappableManager gManager;

	// Token: 0x040029D6 RID: 10710
	[SerializeField]
	private List<Tappable> tappables = new List<Tappable>();

	// Token: 0x040029D7 RID: 10711
	private HashSet<int> idSet = new HashSet<int>();

	// Token: 0x040029D8 RID: 10712
	private static HashSet<Tappable> gRegistry = new HashSet<Tappable>();
}
