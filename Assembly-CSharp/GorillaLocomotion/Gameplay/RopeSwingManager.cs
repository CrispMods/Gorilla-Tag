using System;
using System.Collections.Generic;
using Fusion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Scripting;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B89 RID: 2953
	public class RopeSwingManager : NetworkSceneObject
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06004A0A RID: 18954 RVA: 0x0006030E File Offset: 0x0005E50E
		// (set) Token: 0x06004A0B RID: 18955 RVA: 0x00060315 File Offset: 0x0005E515
		public static RopeSwingManager instance { get; private set; }

		// Token: 0x06004A0C RID: 18956 RVA: 0x0019C2A4 File Offset: 0x0019A4A4
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

		// Token: 0x06004A0D RID: 18957 RVA: 0x0006031D File Offset: 0x0005E51D
		private void RegisterInstance(GorillaRopeSwing t)
		{
			this.ropes.Add(t.ropeId, t);
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x00060331 File Offset: 0x0005E531
		private void UnregisterInstance(GorillaRopeSwing t)
		{
			this.ropes.Remove(t.ropeId);
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x00060345 File Offset: 0x0005E545
		public static void Register(GorillaRopeSwing t)
		{
			RopeSwingManager.instance.RegisterInstance(t);
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x00060352 File Offset: 0x0005E552
		public static void Unregister(GorillaRopeSwing t)
		{
			RopeSwingManager.instance.UnregisterInstance(t);
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x0019C2F0 File Offset: 0x0019A4F0
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

		// Token: 0x06004A12 RID: 18962 RVA: 0x0006035F File Offset: 0x0005E55F
		public bool TryGetRope(int ropeId, out GorillaRopeSwing result)
		{
			return this.ropes.TryGetValue(ropeId, out result);
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x0019C35C File Offset: 0x0019A55C
		[PunRPC]
		public void SetVelocity(int ropeId, int boneIndex, Vector3 velocity, bool wholeRope, PhotonMessageInfo info)
		{
			PhotonMessageInfoWrapped info2 = new PhotonMessageInfoWrapped(info);
			this.SetVelocityShared(ropeId, boneIndex, velocity, wholeRope, info2);
			Utils.Log("Receiving RPC for ropes");
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x0019C388 File Offset: 0x0019A588
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

		// Token: 0x06004A15 RID: 18965 RVA: 0x0019C500 File Offset: 0x0019A700
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

		// Token: 0x06004A17 RID: 18967 RVA: 0x0019C544 File Offset: 0x0019A744
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

		// Token: 0x04004C60 RID: 19552
		private Dictionary<int, GorillaRopeSwing> ropes = new Dictionary<int, GorillaRopeSwing>();
	}
}
