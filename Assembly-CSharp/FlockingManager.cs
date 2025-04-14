using System;
using System.Collections.Generic;
using Fusion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000521 RID: 1313
[NetworkBehaviourWeaved(337)]
public class FlockingManager : NetworkComponent
{
	// Token: 0x06001FD1 RID: 8145 RVA: 0x000A08F8 File Offset: 0x0009EAF8
	protected override void Awake()
	{
		base.Awake();
		foreach (GameObject gameObject in this.fishAreaContainer)
		{
			Flocking[] componentsInChildren = gameObject.GetComponentsInChildren<Flocking>(false);
			FlockingManager.FishArea fishArea = new FlockingManager.FishArea();
			fishArea.id = gameObject.name;
			fishArea.colliders = gameObject.GetComponentsInChildren<BoxCollider>();
			fishArea.colliderCenter = fishArea.colliders[0].bounds.center;
			fishArea.fishList.AddRange(componentsInChildren);
			fishArea.zoneBasedObject = gameObject.GetComponent<ZoneBasedObject>();
			this.areaToWaypointDict[fishArea.id] = Vector3.zero;
			Flocking[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].FishArea = fishArea;
			}
			this.fishAreaList.Add(fishArea);
			this.allFish.AddRange(fishArea.fishList);
			SlingshotProjectileHitNotifier component = gameObject.GetComponent<SlingshotProjectileHitNotifier>();
			if (component != null)
			{
				component.OnProjectileTriggerEnter += this.ProjectileHitReceiver;
				component.OnProjectileTriggerExit += this.ProjectileHitExit;
			}
			else
			{
				Debug.LogError("Needs SlingshotProjectileHitNotifier added to each fish area");
			}
		}
	}

	// Token: 0x06001FD2 RID: 8146 RVA: 0x0001758B File Offset: 0x0001578B
	private new void Start()
	{
		NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x000A0A50 File Offset: 0x0009EC50
	private void OnDestroy()
	{
		NetworkBehaviourUtils.InternalOnDestroy(this);
		this.fishAreaList.Clear();
		this.areaToWaypointDict.Clear();
		this.allFish.Clear();
		foreach (GameObject gameObject in this.fishAreaContainer)
		{
			SlingshotProjectileHitNotifier component = gameObject.GetComponent<SlingshotProjectileHitNotifier>();
			if (component != null)
			{
				component.OnProjectileTriggerExit -= this.ProjectileHitExit;
				component.OnProjectileTriggerEnter -= this.ProjectileHitReceiver;
			}
		}
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x000A0AF8 File Offset: 0x0009ECF8
	private void Update()
	{
		if (Random.Range(0, 10000) < 50)
		{
			foreach (FlockingManager.FishArea fishArea in this.fishAreaList)
			{
				if (fishArea.zoneBasedObject != null)
				{
					fishArea.zoneBasedObject.gameObject.SetActive(fishArea.zoneBasedObject.IsLocalPlayerInZone());
				}
				fishArea.nextWaypoint = this.GetRandomPointInsideCollider(fishArea);
				this.areaToWaypointDict[fishArea.id] = fishArea.nextWaypoint;
				Debug.DrawLine(fishArea.nextWaypoint, Vector3.forward * 5f, Color.magenta);
			}
		}
	}

	// Token: 0x06001FD5 RID: 8149 RVA: 0x000A0BC4 File Offset: 0x0009EDC4
	public Vector3 GetRandomPointInsideCollider(FlockingManager.FishArea fishArea)
	{
		int num = Random.Range(0, fishArea.colliders.Length);
		BoxCollider boxCollider = fishArea.colliders[num];
		Vector3 vector = boxCollider.size / 2f;
		Vector3 position = new Vector3(Random.Range(-vector.x, vector.x), Random.Range(-vector.y, vector.y), Random.Range(-vector.z, vector.z));
		return boxCollider.transform.TransformPoint(position);
	}

	// Token: 0x06001FD6 RID: 8150 RVA: 0x000A0C44 File Offset: 0x0009EE44
	public bool IsInside(Vector3 point, FlockingManager.FishArea fish)
	{
		foreach (BoxCollider boxCollider in fish.colliders)
		{
			Vector3 center = boxCollider.center;
			Vector3 vector = boxCollider.transform.InverseTransformPoint(point);
			vector -= center;
			Vector3 size = boxCollider.size;
			if (Mathf.Abs(vector.x) < size.x / 2f && Mathf.Abs(vector.y) < size.y / 2f && Mathf.Abs(vector.z) < size.z / 2f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001FD7 RID: 8151 RVA: 0x000A0CE0 File Offset: 0x0009EEE0
	public Vector3 RestrictPointToArea(Vector3 point, FlockingManager.FishArea fish)
	{
		Vector3 result = default(Vector3);
		float num = float.MaxValue;
		foreach (BoxCollider boxCollider in fish.colliders)
		{
			Vector3 center = boxCollider.center;
			Vector3 vector = boxCollider.transform.InverseTransformPoint(point);
			Vector3 vector2 = vector - center;
			Vector3 size = boxCollider.size;
			float num2 = size.x / 2f;
			float num3 = size.y / 2f;
			float num4 = size.z / 2f;
			if (Mathf.Abs(vector2.x) < num2 && Mathf.Abs(vector2.y) < num3 && Mathf.Abs(vector2.z) < num4)
			{
				return point;
			}
			Vector3 vector3 = new Vector3(center.x - num2, center.y - num3, center.z - num4);
			Vector3 vector4 = new Vector3(center.x + num2, center.y + num3, center.z + num4);
			Vector3 vector5 = new Vector3(Mathf.Clamp(vector.x, vector3.x, vector4.x), Mathf.Clamp(vector.y, vector3.y, vector4.y), Mathf.Clamp(vector.z, vector3.z, vector4.z));
			float num5 = Vector3.Distance(vector, vector5);
			if (num5 < num)
			{
				num = num5;
				if (num5 > 1f)
				{
					Vector3 a = Vector3.Normalize(vector - vector5);
					result = boxCollider.transform.TransformPoint(vector5 + a * 1f);
				}
				else
				{
					result = point;
				}
			}
		}
		return result;
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x000A0E90 File Offset: 0x0009F090
	private void ProjectileHitReceiver(SlingshotProjectile projectile, Collider collider1)
	{
		bool isRealFood = projectile.CompareTag(this.foodProjectileTag);
		FlockingManager.FishFood arg = new FlockingManager.FishFood
		{
			collider = (collider1 as BoxCollider),
			isRealFood = isRealFood,
			slingshotProjectile = projectile
		};
		UnityAction<FlockingManager.FishFood> unityAction = this.onFoodDetected;
		if (unityAction == null)
		{
			return;
		}
		unityAction(arg);
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x000A0EDB File Offset: 0x0009F0DB
	private void ProjectileHitExit(SlingshotProjectile projectile, Collider collider2)
	{
		UnityAction<BoxCollider> unityAction = this.onFoodDestroyed;
		if (unityAction == null)
		{
			return;
		}
		unityAction(collider2 as BoxCollider);
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06001FDA RID: 8154 RVA: 0x000A0EF3 File Offset: 0x0009F0F3
	// (set) Token: 0x06001FDB RID: 8155 RVA: 0x000A0F1D File Offset: 0x0009F11D
	[Networked]
	[NetworkedWeaved(0, 337)]
	public unsafe FlockingData Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing FlockingManager.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(FlockingData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing FlockingManager.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(FlockingData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x000A0F48 File Offset: 0x0009F148
	public override void WriteDataFusion()
	{
		this.Data = new FlockingData(this.allFish);
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x000A0F5C File Offset: 0x0009F15C
	public override void ReadDataFusion()
	{
		for (int i = 0; i < this.Data.count; i++)
		{
			Vector3 syncPos = this.Data.Positions[i];
			Quaternion syncRot = this.Data.Rotations[i];
			this.allFish[i].SetSyncPosRot(syncPos, syncRot);
		}
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x000A0FC7 File Offset: 0x0009F1C7
	public static void RegisterAvoidPoint(GameObject obj)
	{
		FlockingManager.avoidPoints.Add(obj);
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000A0FD4 File Offset: 0x0009F1D4
	public static void UnregisterAvoidPoint(GameObject obj)
	{
		FlockingManager.avoidPoints.Remove(obj);
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000A1022 File Offset: 0x0009F222
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x000A103A File Offset: 0x0009F23A
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x040023DF RID: 9183
	public List<GameObject> fishAreaContainer;

	// Token: 0x040023E0 RID: 9184
	public string foodProjectileTag = "WaterBalloonProjectile";

	// Token: 0x040023E1 RID: 9185
	private Dictionary<string, Vector3> areaToWaypointDict = new Dictionary<string, Vector3>();

	// Token: 0x040023E2 RID: 9186
	private List<FlockingManager.FishArea> fishAreaList = new List<FlockingManager.FishArea>();

	// Token: 0x040023E3 RID: 9187
	private List<Flocking> allFish = new List<Flocking>();

	// Token: 0x040023E4 RID: 9188
	public UnityAction<FlockingManager.FishFood> onFoodDetected;

	// Token: 0x040023E5 RID: 9189
	public UnityAction<BoxCollider> onFoodDestroyed;

	// Token: 0x040023E6 RID: 9190
	private bool hasBeenSerialized;

	// Token: 0x040023E7 RID: 9191
	public static readonly List<GameObject> avoidPoints = new List<GameObject>();

	// Token: 0x040023E8 RID: 9192
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 337)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private FlockingData _Data;

	// Token: 0x02000522 RID: 1314
	public class FishArea
	{
		// Token: 0x040023E9 RID: 9193
		public string id;

		// Token: 0x040023EA RID: 9194
		public List<Flocking> fishList = new List<Flocking>();

		// Token: 0x040023EB RID: 9195
		public Vector3 colliderCenter;

		// Token: 0x040023EC RID: 9196
		public BoxCollider[] colliders;

		// Token: 0x040023ED RID: 9197
		public Vector3 nextWaypoint = Vector3.zero;

		// Token: 0x040023EE RID: 9198
		public ZoneBasedObject zoneBasedObject;
	}

	// Token: 0x02000523 RID: 1315
	public class FishFood
	{
		// Token: 0x040023EF RID: 9199
		public BoxCollider collider;

		// Token: 0x040023F0 RID: 9200
		public bool isRealFood;

		// Token: 0x040023F1 RID: 9201
		public SlingshotProjectile slingshotProjectile;
	}
}
