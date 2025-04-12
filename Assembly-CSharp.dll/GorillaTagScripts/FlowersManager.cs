﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B6 RID: 2486
	[NetworkBehaviourWeaved(13)]
	public class FlowersManager : NetworkComponent
	{
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06003D93 RID: 15763 RVA: 0x00057442 File Offset: 0x00055642
		// (set) Token: 0x06003D94 RID: 15764 RVA: 0x00057449 File Offset: 0x00055649
		public static FlowersManager Instance { get; private set; }

		// Token: 0x06003D95 RID: 15765 RVA: 0x0015E794 File Offset: 0x0015C994
		protected override void Awake()
		{
			base.Awake();
			FlowersManager.Instance = this;
			this.hitNotifiers = base.GetComponentsInChildren<SlingshotProjectileHitNotifier>();
			foreach (SlingshotProjectileHitNotifier slingshotProjectileHitNotifier in this.hitNotifiers)
			{
				if (slingshotProjectileHitNotifier != null)
				{
					slingshotProjectileHitNotifier.OnProjectileTriggerEnter += this.ProjectileHitReceiver;
				}
				else
				{
					Debug.LogError("Needs SlingshotProjectileHitNotifier added to this GameObject children");
				}
			}
			foreach (FlowersManager.FlowersInZone flowersInZone in this.sections)
			{
				foreach (GameObject gameObject in flowersInZone.sections)
				{
					this.sectionToZonesDict[gameObject] = flowersInZone.zone;
					Flower[] componentsInChildren = gameObject.GetComponentsInChildren<Flower>();
					this.allFlowers.AddRange(componentsInChildren);
					this.sectionToFlowersDict[gameObject] = componentsInChildren.ToList<Flower>();
				}
			}
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x0015E8B8 File Offset: 0x0015CAB8
		private new void Start()
		{
			NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.HandleOnZoneChanged));
			if (base.IsMine)
			{
				foreach (Flower flower in this.allFlowers)
				{
					flower.UpdateFlowerState(Flower.FlowerState.Healthy, false, false);
				}
			}
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x0015E94C File Offset: 0x0015CB4C
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			foreach (SlingshotProjectileHitNotifier slingshotProjectileHitNotifier in this.hitNotifiers)
			{
				if (slingshotProjectileHitNotifier != null)
				{
					slingshotProjectileHitNotifier.OnProjectileTriggerEnter -= this.ProjectileHitReceiver;
				}
			}
			FlowersManager.Instance = null;
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.HandleOnZoneChanged));
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x00057451 File Offset: 0x00055651
		private void ProjectileHitReceiver(SlingshotProjectile projectile, Collider collider)
		{
			if (!projectile.CompareTag("WaterBalloonProjectile"))
			{
				return;
			}
			this.WaterFlowers(collider);
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x0015E9C0 File Offset: 0x0015CBC0
		private void WaterFlowers(Collider collider)
		{
			if (!base.IsMine)
			{
				return;
			}
			GameObject gameObject = collider.gameObject;
			if (gameObject == null)
			{
				Debug.LogError("Could not find any flowers section");
				return;
			}
			foreach (Flower flower in this.sectionToFlowersDict[gameObject])
			{
				flower.WaterFlower(true);
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x0015EA3C File Offset: 0x0015CC3C
		private void HandleOnZoneChanged()
		{
			foreach (KeyValuePair<GameObject, GTZone> keyValuePair in this.sectionToZonesDict)
			{
				bool enable = ZoneManagement.instance.IsZoneActive(keyValuePair.Value);
				foreach (Flower flower in this.sectionToFlowersDict[keyValuePair.Key])
				{
					flower.UpdateVisuals(enable);
				}
			}
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x0015EAE8 File Offset: 0x0015CCE8
		public int GetHealthyFlowersInZoneCount(GTZone zone)
		{
			int num = 0;
			foreach (KeyValuePair<GameObject, GTZone> keyValuePair in this.sectionToZonesDict)
			{
				if (keyValuePair.Value == zone)
				{
					using (List<Flower>.Enumerator enumerator2 = this.sectionToFlowersDict[keyValuePair.Key].GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.GetCurrentState() == Flower.FlowerState.Healthy)
							{
								num++;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x0015EB94 File Offset: 0x0015CD94
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			stream.SendNext(this.allFlowers.Count);
			for (int i = 0; i < this.allFlowers.Count; i++)
			{
				stream.SendNext(this.allFlowers[i].IsWatered);
				stream.SendNext(this.allFlowers[i].GetCurrentState());
			}
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x0015EC14 File Offset: 0x0015CE14
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			int num = (int)stream.ReceiveNext();
			for (int i = 0; i < num; i++)
			{
				bool isWatered = (bool)stream.ReceiveNext();
				Flower.FlowerState currentState = this.allFlowers[i].GetCurrentState();
				Flower.FlowerState flowerState = (Flower.FlowerState)stream.ReceiveNext();
				if (currentState != flowerState)
				{
					this.allFlowers[i].UpdateFlowerState(flowerState, isWatered, true);
				}
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06003D9E RID: 15774 RVA: 0x00057468 File Offset: 0x00055668
		// (set) Token: 0x06003D9F RID: 15775 RVA: 0x00057492 File Offset: 0x00055692
		[Networked]
		[NetworkedWeaved(0, 13)]
		private unsafe FlowersDataStruct Data
		{
			get
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FlowersManager.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(FlowersDataStruct*)(this.Ptr + 0);
			}
			set
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FlowersManager.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(FlowersDataStruct*)(this.Ptr + 0) = value;
			}
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x000574BD File Offset: 0x000556BD
		public override void WriteDataFusion()
		{
			if (base.HasStateAuthority)
			{
				this.Data = new FlowersDataStruct(this.allFlowers);
			}
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x0015EC88 File Offset: 0x0015CE88
		public override void ReadDataFusion()
		{
			if (this.Data.FlowerCount > 0)
			{
				for (int i = 0; i < this.Data.FlowerCount; i++)
				{
					bool isWatered = this.Data.FlowerWateredData[i] == 1;
					Flower.FlowerState currentState = this.allFlowers[i].GetCurrentState();
					Flower.FlowerState flowerState = (Flower.FlowerState)this.Data.FlowerStateData[i];
					if (currentState != flowerState)
					{
						this.allFlowers[i].UpdateFlowerState(flowerState, isWatered, true);
					}
				}
			}
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x0015ED20 File Offset: 0x0015CF20
		private void Update()
		{
			int num = this.flowerCheckIndex + 1;
			while (num < this.allFlowers.Count && num < this.flowerCheckIndex + this.flowersToCheck)
			{
				this.allFlowers[num].AnimCatch();
				num++;
			}
			this.flowerCheckIndex = ((this.flowerCheckIndex + this.flowersToCheck >= this.allFlowers.Count) ? 0 : (this.flowerCheckIndex + this.flowersToCheck));
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x00057508 File Offset: 0x00055708
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x00057520 File Offset: 0x00055720
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04003EFC RID: 16124
		public List<FlowersManager.FlowersInZone> sections;

		// Token: 0x04003EFD RID: 16125
		public int flowersToCheck = 1;

		// Token: 0x04003EFE RID: 16126
		public int flowerCheckIndex;

		// Token: 0x04003EFF RID: 16127
		private readonly List<Flower> allFlowers = new List<Flower>();

		// Token: 0x04003F00 RID: 16128
		private SlingshotProjectileHitNotifier[] hitNotifiers;

		// Token: 0x04003F01 RID: 16129
		private readonly Dictionary<GameObject, List<Flower>> sectionToFlowersDict = new Dictionary<GameObject, List<Flower>>();

		// Token: 0x04003F02 RID: 16130
		private readonly Dictionary<GameObject, GTZone> sectionToZonesDict = new Dictionary<GameObject, GTZone>();

		// Token: 0x04003F03 RID: 16131
		private bool hasBeenSerialized;

		// Token: 0x04003F04 RID: 16132
		[WeaverGenerated]
		[DefaultForProperty("Data", 0, 13)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private FlowersDataStruct _Data;

		// Token: 0x020009B7 RID: 2487
		[Serializable]
		public class FlowersInZone
		{
			// Token: 0x04003F05 RID: 16133
			public GTZone zone;

			// Token: 0x04003F06 RID: 16134
			public List<GameObject> sections;
		}
	}
}
