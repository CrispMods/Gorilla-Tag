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
	// Token: 0x020009AF RID: 2479
	[NetworkBehaviourWeaved(1)]
	public class DecorativeItemsManager : NetworkComponent
	{
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06003D5E RID: 15710 RVA: 0x00121CCC File Offset: 0x0011FECC
		public static DecorativeItemsManager Instance
		{
			get
			{
				return DecorativeItemsManager._instance;
			}
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x00121CD4 File Offset: 0x0011FED4
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

		// Token: 0x06003D60 RID: 15712 RVA: 0x00121E3C File Offset: 0x0012003C
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

		// Token: 0x06003D61 RID: 15713 RVA: 0x00121ED8 File Offset: 0x001200D8
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

		// Token: 0x06003D62 RID: 15714 RVA: 0x00121FC8 File Offset: 0x001201C8
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

		// Token: 0x06003D63 RID: 15715 RVA: 0x0012208F File Offset: 0x0012028F
		[PunRPC]
		private void RespawnItemRPC(int index, Vector3 _transformPos, Quaternion _transformRot, PhotonMessageInfo info)
		{
			this.RespawnItemShared(index, _transformPos, _transformRot, info);
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x001220A4 File Offset: 0x001202A4
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

		// Token: 0x06003D65 RID: 15717 RVA: 0x00122244 File Offset: 0x00120444
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

		// Token: 0x06003D66 RID: 15718 RVA: 0x001222B4 File Offset: 0x001204B4
		private Transform RandomSpawn()
		{
			this.lastIndex = this.currentIndex;
			bool flag = false;
			bool flag2 = this.zone.IsLocalPlayerInZone();
			int index = Random.Range(0, this.respawnableHooks.Count);
			while (!flag)
			{
				index = Random.Range(0, this.respawnableHooks.Count);
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

		// Token: 0x06003D67 RID: 15719 RVA: 0x00122376 File Offset: 0x00120576
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

		// Token: 0x06003D68 RID: 15720 RVA: 0x001223B4 File Offset: 0x001205B4
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

		// Token: 0x06003D69 RID: 15721 RVA: 0x001223E8 File Offset: 0x001205E8
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

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06003D6A RID: 15722 RVA: 0x00122450 File Offset: 0x00120650
		// (set) Token: 0x06003D6B RID: 15723 RVA: 0x00122476 File Offset: 0x00120676
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

		// Token: 0x06003D6C RID: 15724 RVA: 0x0012249D File Offset: 0x0012069D
		public override void WriteDataFusion()
		{
			this.Data = this.currentIndex;
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x001224AB File Offset: 0x001206AB
		public override void ReadDataFusion()
		{
			this.currentIndex = this.Data;
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x001224B9 File Offset: 0x001206B9
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			stream.SendNext(this.currentIndex);
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x001224DA File Offset: 0x001206DA
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			this.currentIndex = (int)stream.ReceiveNext();
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x00122536 File Offset: 0x00120736
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x0012254E File Offset: 0x0012074E
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x00122564 File Offset: 0x00120764
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

		// Token: 0x04003EC4 RID: 16068
		public GameObject decorativeItemsContainer;

		// Token: 0x04003EC5 RID: 16069
		public GameObject respawnableHooksContainer;

		// Token: 0x04003EC6 RID: 16070
		public List<GameObject> nonRespawnableHooksContainer = new List<GameObject>();

		// Token: 0x04003EC7 RID: 16071
		private readonly List<DecorativeItem> itemsList = new List<DecorativeItem>();

		// Token: 0x04003EC8 RID: 16072
		private readonly List<AttachPoint> respawnableHooks = new List<AttachPoint>();

		// Token: 0x04003EC9 RID: 16073
		private readonly List<AttachPoint> allHooks = new List<AttachPoint>();

		// Token: 0x04003ECA RID: 16074
		private int lastIndex;

		// Token: 0x04003ECB RID: 16075
		private int currentIndex;

		// Token: 0x04003ECC RID: 16076
		private int arrayIndex = -1;

		// Token: 0x04003ECD RID: 16077
		private bool shouldRunUpdate;

		// Token: 0x04003ECE RID: 16078
		private ZoneBasedObject zone;

		// Token: 0x04003ECF RID: 16079
		private bool wasInZone;

		// Token: 0x04003ED0 RID: 16080
		[OnEnterPlay_SetNull]
		private static DecorativeItemsManager _instance;

		// Token: 0x04003ED1 RID: 16081
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Data", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Data;
	}
}
