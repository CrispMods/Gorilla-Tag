using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;

namespace GorillaTagScripts
{
	// Token: 0x020009D5 RID: 2517
	[NetworkBehaviourWeaved(1)]
	public class DecorativeItemsManager : NetworkComponent
	{
		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06003E76 RID: 15990 RVA: 0x00058AB0 File Offset: 0x00056CB0
		public static DecorativeItemsManager Instance
		{
			get
			{
				return DecorativeItemsManager._instance;
			}
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x00163CA4 File Offset: 0x00161EA4
		protected override void Awake()
		{
			base.Awake();
			if (DecorativeItemsManager._instance != null && DecorativeItemsManager._instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				DecorativeItemsManager._instance = this;
			}
			this.currentIndex = -1;
			this.shouldRunUpdate = true;
			this.zone = base.GetComponent<ZoneBasedObject>();
			foreach (DecorativeItem decorativeItem in this.decorativeItemsContainer.GetComponentsInChildren<DecorativeItem>(false))
			{
				if (decorativeItem)
				{
					this.itemsList.Add(decorativeItem);
					DecorativeItem decorativeItem2 = decorativeItem;
					decorativeItem2.respawnItem = (UnityAction<DecorativeItem>)Delegate.Combine(decorativeItem2.respawnItem, new UnityAction<DecorativeItem>(this.OnRequestToRespawn));
				}
			}
			foreach (AttachPoint attachPoint in this.respawnableHooksContainer.GetComponentsInChildren<AttachPoint>(false))
			{
				if (attachPoint)
				{
					this.respawnableHooks.Add(attachPoint);
				}
			}
			this.allHooks.AddRange(this.respawnableHooks);
			foreach (GameObject gameObject in this.nonRespawnableHooksContainer)
			{
				foreach (AttachPoint attachPoint2 in gameObject.GetComponentsInChildren<AttachPoint>(false))
				{
					if (attachPoint2)
					{
						this.allHooks.Add(attachPoint2);
					}
				}
			}
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x00163E0C File Offset: 0x0016200C
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			foreach (DecorativeItem decorativeItem in this.itemsList)
			{
				decorativeItem.respawnItem = (UnityAction<DecorativeItem>)Delegate.Remove(decorativeItem.respawnItem, new UnityAction<DecorativeItem>(this.OnRequestToRespawn));
			}
			this.itemsList.Clear();
			this.respawnableHooks.Clear();
			if (DecorativeItemsManager._instance == this)
			{
				DecorativeItemsManager._instance = null;
			}
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x00163EA8 File Offset: 0x001620A8
		private void Update()
		{
			if (!PhotonNetwork.InRoom)
			{
				return;
			}
			if (this.wasInZone != this.zone.IsLocalPlayerInZone())
			{
				this.shouldRunUpdate = true;
			}
			if (!this.shouldRunUpdate)
			{
				return;
			}
			if (base.IsMine)
			{
				if (this.wasInZone != this.zone.IsLocalPlayerInZone())
				{
					foreach (AttachPoint attachPoint in this.allHooks)
					{
						attachPoint.SetIsHook(false);
					}
					for (int i = 0; i < this.itemsList.Count; i++)
					{
						this.itemsList[i].itemState = TransferrableObject.ItemStates.State2;
						this.SpawnItem(i);
					}
					this.shouldRunUpdate = false;
				}
				this.wasInZone = this.zone.IsLocalPlayerInZone();
				this.SpawnItem(this.UpdateListPerFrame());
			}
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x00163F98 File Offset: 0x00162198
		private void SpawnItem(int index)
		{
			if (!NetworkSystem.Instance.InRoom)
			{
				return;
			}
			if (index < 0 || index >= this.itemsList.Count)
			{
				return;
			}
			if (this.respawnableHooks == null)
			{
				return;
			}
			if (this.itemsList == null)
			{
				return;
			}
			if (this.itemsList.Count > this.respawnableHooks.Count)
			{
				Debug.LogError("Trying to snap more decorative items than allowed! Some items will be left un-hooked!");
				return;
			}
			Transform transform = this.RandomSpawn();
			if (transform == null)
			{
				return;
			}
			Vector3 position = transform.position;
			Quaternion rotation = transform.rotation;
			DecorativeItem decorativeItem = this.itemsList[index];
			decorativeItem.WorldShareableRequestOwnership();
			decorativeItem.Respawn(position, rotation);
			base.SendRPC("RespawnItemRPC", RpcTarget.Others, new object[]
			{
				index,
				position,
				rotation
			});
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x00058AB7 File Offset: 0x00056CB7
		[PunRPC]
		private void RespawnItemRPC(int index, Vector3 _transformPos, Quaternion _transformRot, PhotonMessageInfo info)
		{
			this.RespawnItemShared(index, _transformPos, _transformRot, info);
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x00164060 File Offset: 0x00162260
		[Rpc]
		private unsafe void RPC_RespawnItem(int index, Vector3 _transformPos, Quaternion _transformRot, RpcInfo info = default(RpcInfo))
		{
			if (!this.InvokeRpc)
			{
				NetworkBehaviourUtils.ThrowIfBehaviourNotInitialized(this);
				if (base.Runner.Stage != SimulationStages.Resimulate)
				{
					int localAuthorityMask = base.Object.GetLocalAuthorityMask();
					if ((localAuthorityMask & 7) == 0)
					{
						NetworkBehaviourUtils.NotifyLocalSimulationNotAllowedToSendRpc("System.Void GorillaTagScripts.DecorativeItemsManager::RPC_RespawnItem(System.Int32,UnityEngine.Vector3,UnityEngine.Quaternion,Fusion.RpcInfo)", base.Object, 7);
					}
					else
					{
						if (base.Runner.HasAnyActiveConnections())
						{
							int num = 8;
							num += 4;
							num += 12;
							num += 16;
							SimulationMessage* ptr = SimulationMessage.Allocate(base.Runner.Simulation, num);
							byte* data = SimulationMessage.GetData(ptr);
							int num2 = RpcHeader.Write(RpcHeader.Create(base.Object.Id, this.ObjectIndex, 1), data);
							*(int*)(data + num2) = index;
							num2 += 4;
							*(Vector3*)(data + num2) = _transformPos;
							num2 += 12;
							*(Quaternion*)(data + num2) = _transformRot;
							num2 += 16;
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
			this.RespawnItemShared(index, _transformPos, _transformRot, info);
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x00164200 File Offset: 0x00162400
		protected void RespawnItemShared(int index, Vector3 _transformPos, Quaternion _transformRot, PhotonMessageInfoWrapped info)
		{
			if (index >= 0 && index <= this.itemsList.Count - 1)
			{
				float num = 10000f;
				if (_transformPos.IsValid(num) && _transformRot.IsValid() && info.Sender == NetworkSystem.Instance.MasterClient)
				{
					GorillaNot.IncrementRPCCall(info, "RespawnItemRPC");
					this.itemsList[index].Respawn(_transformPos, _transformRot);
					return;
				}
			}
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x00164270 File Offset: 0x00162470
		private Transform RandomSpawn()
		{
			this.lastIndex = this.currentIndex;
			bool flag = false;
			bool flag2 = this.zone.IsLocalPlayerInZone();
			int index = UnityEngine.Random.Range(0, this.respawnableHooks.Count);
			while (!flag)
			{
				index = UnityEngine.Random.Range(0, this.respawnableHooks.Count);
				if (!this.respawnableHooks[index].inForest == flag2)
				{
					flag = true;
				}
			}
			if (!this.respawnableHooks[index].IsHooked())
			{
				this.currentIndex = index;
			}
			else
			{
				this.currentIndex = -1;
			}
			if (this.currentIndex != this.lastIndex && this.currentIndex > -1)
			{
				return this.respawnableHooks[this.currentIndex].attachPoint;
			}
			this.currentIndex = -1;
			return null;
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x00058AC9 File Offset: 0x00056CC9
		private int UpdateListPerFrame()
		{
			this.arrayIndex++;
			if (this.arrayIndex >= this.itemsList.Count || this.arrayIndex < 0)
			{
				this.shouldRunUpdate = false;
				return -1;
			}
			return this.arrayIndex;
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x00164334 File Offset: 0x00162534
		private void OnRequestToRespawn(DecorativeItem item)
		{
			if (base.IsMine)
			{
				if (item == null)
				{
					return;
				}
				int index = this.itemsList.IndexOf(item);
				this.SpawnItem(index);
			}
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x00164368 File Offset: 0x00162568
		public AttachPoint getCurrentAttachPointByPosition(Vector3 _attachPoint)
		{
			foreach (AttachPoint attachPoint in this.allHooks)
			{
				if (attachPoint.attachPoint.position == _attachPoint)
				{
					return attachPoint;
				}
			}
			return null;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06003E82 RID: 16002 RVA: 0x00058B04 File Offset: 0x00056D04
		// (set) Token: 0x06003E83 RID: 16003 RVA: 0x00058B2A File Offset: 0x00056D2A
		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe int Data
		{
			get
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing DecorativeItemsManager.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				return this.Ptr[0];
			}
			set
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing DecorativeItemsManager.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				this.Ptr[0] = value;
			}
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x00058B51 File Offset: 0x00056D51
		public override void WriteDataFusion()
		{
			this.Data = this.currentIndex;
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x00058B5F File Offset: 0x00056D5F
		public override void ReadDataFusion()
		{
			this.currentIndex = this.Data;
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x00058B6D File Offset: 0x00056D6D
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			stream.SendNext(this.currentIndex);
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x00058B8E File Offset: 0x00056D8E
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			this.currentIndex = (int)stream.ReceiveNext();
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x00058BEA File Offset: 0x00056DEA
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x00058C02 File Offset: 0x00056E02
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x001643D0 File Offset: 0x001625D0
		[NetworkRpcWeavedInvoker(1, 7, 7)]
		[Preserve]
		[WeaverGenerated]
		protected unsafe static void RPC_RespawnItem@Invoker(NetworkBehaviour behaviour, SimulationMessage* message)
		{
			byte* data = SimulationMessage.GetData(message);
			int num = RpcHeader.ReadSize(data) + 3 & -4;
			int num2 = *(int*)(data + num);
			num += 4;
			int index = num2;
			Vector3 vector = *(Vector3*)(data + num);
			num += 12;
			Vector3 transformPos = vector;
			Quaternion quaternion = *(Quaternion*)(data + num);
			num += 16;
			Quaternion transformRot = quaternion;
			RpcInfo info = RpcInfo.FromMessage(behaviour.Runner, message, RpcHostMode.SourceIsServer);
			behaviour.InvokeRpc = true;
			((DecorativeItemsManager)behaviour).RPC_RespawnItem(index, transformPos, transformRot, info);
		}

		// Token: 0x04003F9E RID: 16286
		public GameObject decorativeItemsContainer;

		// Token: 0x04003F9F RID: 16287
		public GameObject respawnableHooksContainer;

		// Token: 0x04003FA0 RID: 16288
		public List<GameObject> nonRespawnableHooksContainer = new List<GameObject>();

		// Token: 0x04003FA1 RID: 16289
		private readonly List<DecorativeItem> itemsList = new List<DecorativeItem>();

		// Token: 0x04003FA2 RID: 16290
		private readonly List<AttachPoint> respawnableHooks = new List<AttachPoint>();

		// Token: 0x04003FA3 RID: 16291
		private readonly List<AttachPoint> allHooks = new List<AttachPoint>();

		// Token: 0x04003FA4 RID: 16292
		private int lastIndex;

		// Token: 0x04003FA5 RID: 16293
		private int currentIndex;

		// Token: 0x04003FA6 RID: 16294
		private int arrayIndex = -1;

		// Token: 0x04003FA7 RID: 16295
		private bool shouldRunUpdate;

		// Token: 0x04003FA8 RID: 16296
		private ZoneBasedObject zone;

		// Token: 0x04003FA9 RID: 16297
		private bool wasInZone;

		// Token: 0x04003FAA RID: 16298
		[OnEnterPlay_SetNull]
		private static DecorativeItemsManager _instance;

		// Token: 0x04003FAB RID: 16299
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Data;
	}
}
