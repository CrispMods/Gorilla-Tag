using System;
using System.Collections.Generic;
using Fusion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Scripting;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B5C RID: 2908
	public class RopeSwingManager : NetworkSceneObject
	{
		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x060048BF RID: 18623 RVA: 0x00160CEF File Offset: 0x0015EEEF
		// (set) Token: 0x060048C0 RID: 18624 RVA: 0x00160CF6 File Offset: 0x0015EEF6
		public static RopeSwingManager instance { get; private set; }

		// Token: 0x060048C1 RID: 18625 RVA: 0x00160D00 File Offset: 0x0015EF00
		private void Awake()
		{
			if (RopeSwingManager.instance != null && RopeSwingManager.instance != this)
			{
				GTDev.LogWarning<string>("Instance of RopeSwingManager already exists. Destroying.", null);
				UnityEngine.Object.Destroy(this);
				return;
			}
			if (RopeSwingManager.instance == null)
			{
				RopeSwingManager.instance = this;
			}
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x00160D4C File Offset: 0x0015EF4C
		private void RegisterInstance(GorillaRopeSwing t)
		{
			this.ropes.Add(t.ropeId, t);
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x00160D60 File Offset: 0x0015EF60
		private void UnregisterInstance(GorillaRopeSwing t)
		{
			this.ropes.Remove(t.ropeId);
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x00160D74 File Offset: 0x0015EF74
		public static void Register(GorillaRopeSwing t)
		{
			RopeSwingManager.instance.RegisterInstance(t);
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x00160D81 File Offset: 0x0015EF81
		public static void Unregister(GorillaRopeSwing t)
		{
			RopeSwingManager.instance.UnregisterInstance(t);
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x00160D90 File Offset: 0x0015EF90
		public void SendSetVelocity_RPC(int ropeId, int boneIndex, Vector3 velocity, bool wholeRope)
		{
			if (NetworkSystem.Instance.InRoom)
			{
				this.photonView.RPC("SetVelocity", RpcTarget.All, new object[]
				{
					ropeId,
					boneIndex,
					velocity,
					wholeRope
				});
				return;
			}
			this.SetVelocityShared(ropeId, boneIndex, velocity, wholeRope, default(PhotonMessageInfoWrapped));
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x00160DFA File Offset: 0x0015EFFA
		public bool TryGetRope(int ropeId, out GorillaRopeSwing result)
		{
			return this.ropes.TryGetValue(ropeId, out result);
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x00160E0C File Offset: 0x0015F00C
		[PunRPC]
		public void SetVelocity(int ropeId, int boneIndex, Vector3 velocity, bool wholeRope, PhotonMessageInfo info)
		{
			PhotonMessageInfoWrapped info2 = new PhotonMessageInfoWrapped(info);
			this.SetVelocityShared(ropeId, boneIndex, velocity, wholeRope, info2);
			Utils.Log("Receiving RPC for ropes");
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x00160E38 File Offset: 0x0015F038
		[Rpc]
		public unsafe static void RPC_SetVelocity(NetworkRunner runner, int ropeId, int boneIndex, Vector3 velocity, bool wholeRope, RpcInfo info = default(RpcInfo))
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
					num += 12;
					num += 4;
					SimulationMessage* ptr = SimulationMessage.Allocate(runner.Simulation, num);
					byte* data = SimulationMessage.GetData(ptr);
					int num2 = RpcHeader.Write(RpcHeader.Create(NetworkBehaviourUtils.GetRpcStaticIndexOrThrow("System.Void GorillaLocomotion.Gameplay.RopeSwingManager::RPC_SetVelocity(Fusion.NetworkRunner,System.Int32,System.Int32,UnityEngine.Vector3,System.Boolean,Fusion.RpcInfo)")), data);
					*(int*)(data + num2) = ropeId;
					num2 += 4;
					*(int*)(data + num2) = boneIndex;
					num2 += 4;
					*(Vector3*)(data + num2) = velocity;
					num2 += 12;
					ReadWriteUtilsForWeaver.WriteBoolean((int*)(data + num2), wholeRope);
					num2 += 4;
					ptr->Offset = num2 * 8;
					ptr->SetStatic();
					runner.SendRpc(ptr);
				}
				info = RpcInfo.FromLocal(runner, RpcChannel.Reliable, RpcHostMode.SourceIsServer);
			}
			PhotonMessageInfoWrapped info2 = new PhotonMessageInfoWrapped(info);
			RopeSwingManager.instance.SetVelocityShared(ropeId, boneIndex, velocity, wholeRope, info2);
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x00160FB0 File Offset: 0x0015F1B0
		private void SetVelocityShared(int ropeId, int boneIndex, Vector3 velocity, bool wholeRope, PhotonMessageInfoWrapped info)
		{
			if (info.Sender != null)
			{
				GorillaNot.IncrementRPCCall(info, "SetVelocityShared");
			}
			GorillaRopeSwing gorillaRopeSwing;
			if (this.TryGetRope(ropeId, out gorillaRopeSwing) && gorillaRopeSwing != null)
			{
				gorillaRopeSwing.SetVelocity(boneIndex, velocity, wholeRope, info);
			}
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x00161008 File Offset: 0x0015F208
		[NetworkRpcStaticWeavedInvoker("System.Void GorillaLocomotion.Gameplay.RopeSwingManager::RPC_SetVelocity(Fusion.NetworkRunner,System.Int32,System.Int32,UnityEngine.Vector3,System.Boolean,Fusion.RpcInfo)")]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_SetVelocity@Invoker(NetworkRunner runner, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			int num2 = *(int*)(data + num);
			num += 4;
			int ropeId = num2;
			int num3 = *(int*)(data + num);
			num += 4;
			int boneIndex = num3;
			Vector3 vector = *(Vector3*)(data + num);
			num += 12;
			Vector3 velocity = vector;
			bool flag = ReadWriteUtilsForWeaver.ReadBoolean((int*)(data + num));
			num += 4;
			bool wholeRope = flag;
			RpcInfo info = RpcInfo.FromMessage(runner, message, RpcHostMode.SourceIsServer);
			NetworkBehaviourUtils.InvokeRpc = true;
			RopeSwingManager.RPC_SetVelocity(runner, ropeId, boneIndex, velocity, wholeRope, info);
		}

		// Token: 0x04004B6A RID: 19306
		private Dictionary<int, GorillaRopeSwing> ropes = new Dictionary<int, GorillaRopeSwing>();
	}
}
