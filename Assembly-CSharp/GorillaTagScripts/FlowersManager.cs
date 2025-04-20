using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009D9 RID: 2521
	[NetworkBehaviourWeaved(13)]
	public class FlowersManager : NetworkComponent
	{
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06003E9F RID: 16031 RVA: 0x00058CD9 File Offset: 0x00056ED9
		// (set) Token: 0x06003EA0 RID: 16032 RVA: 0x00058CE0 File Offset: 0x00056EE0
		public static FlowersManager Instance { get; private set; }

		// Token: 0x06003EA1 RID: 16033 RVA: 0x0016477C File Offset: 0x0016297C
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

		// Token: 0x06003EA2 RID: 16034 RVA: 0x001648A0 File Offset: 0x00162AA0
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

		// Token: 0x06003EA3 RID: 16035 RVA: 0x00164934 File Offset: 0x00162B34
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

		// Token: 0x06003EA4 RID: 16036 RVA: 0x00058CE8 File Offset: 0x00056EE8
		private void ProjectileHitReceiver(SlingshotProjectile projectile, Collider collider)
		{
			if (!projectile.CompareTag("WaterBalloonProjectile"))
			{
				return;
			}
			this.WaterFlowers(collider);
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x001649A8 File Offset: 0x00162BA8
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

		// Token: 0x06003EA6 RID: 16038 RVA: 0x00164A24 File Offset: 0x00162C24
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

		// Token: 0x06003EA7 RID: 16039 RVA: 0x00164AD0 File Offset: 0x00162CD0
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

		// Token: 0x06003EA8 RID: 16040 RVA: 0x00164B7C File Offset: 0x00162D7C
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

		// Token: 0x06003EA9 RID: 16041 RVA: 0x00164BFC File Offset: 0x00162DFC
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

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06003EAA RID: 16042 RVA: 0x00058CFF File Offset: 0x00056EFF
		// (set) Token: 0x06003EAB RID: 16043 RVA: 0x00058D29 File Offset: 0x00056F29
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

		// Token: 0x06003EAC RID: 16044 RVA: 0x00058D54 File Offset: 0x00056F54
		public override void WriteDataFusion()
		{
			if (base.HasStateAuthority)
			{
				this.Data = new FlowersDataStruct(this.allFlowers);
			}
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x00164C70 File Offset: 0x00162E70
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

		// Token: 0x06003EAE RID: 16046 RVA: 0x00164D08 File Offset: 0x00162F08
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

		// Token: 0x06003EB0 RID: 16048 RVA: 0x00058D9F File Offset: 0x00056F9F
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x00058DB7 File Offset: 0x00056FB7
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04003FC4 RID: 16324
		public List<FlowersManager.FlowersInZone> sections;

		// Token: 0x04003FC5 RID: 16325
		public int flowersToCheck = 1;

		// Token: 0x04003FC6 RID: 16326
		public int flowerCheckIndex;

		// Token: 0x04003FC7 RID: 16327
		private readonly List<Flower> allFlowers = new List<Flower>();

		// Token: 0x04003FC8 RID: 16328
		private SlingshotProjectileHitNotifier[] hitNotifiers;

		// Token: 0x04003FC9 RID: 16329
		private readonly Dictionary<GameObject, List<Flower>> sectionToFlowersDict = new Dictionary<GameObject, List<Flower>>();

		// Token: 0x04003FCA RID: 16330
		private readonly Dictionary<GameObject, GTZone> sectionToZonesDict = new Dictionary<GameObject, GTZone>();

		// Token: 0x04003FCB RID: 16331
		private bool hasBeenSerialized;

		// Token: 0x04003FCC RID: 16332
		[WeaverGenerated]
		[DefaultForProperty("Data", 0, 13)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private FlowersDataStruct _Data;

		// Token: 0x020009DA RID: 2522
		[Serializable]
		public class FlowersInZone
		{
			// Token: 0x04003FCD RID: 16333
			public GTZone zone;

			// Token: 0x04003FCE RID: 16334
			public List<GameObject> sections;
		}
	}
}
