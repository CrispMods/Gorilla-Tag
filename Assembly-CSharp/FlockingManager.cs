using System;
using System.Collections.Generic;
using Fusion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200052E RID: 1326
[NetworkBehaviourWeaved(337)]
public class FlockingManager : NetworkComponent
{
	// Token: 0x0600202A RID: 8234 RVA: 0x000F20B8 File Offset: 0x000F02B8
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

	// Token: 0x0600202B RID: 8235 RVA: 0x00033269 File Offset: 0x00031469
	private new void Start()
	{
		NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
	}

	// Token: 0x0600202C RID: 8236 RVA: 0x000F2210 File Offset: 0x000F0410
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

	// Token: 0x0600202D RID: 8237 RVA: 0x000F22B8 File Offset: 0x000F04B8
	private void Update()
	{
		if (UnityEngine.Random.Range(0, 10000) < 50)
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

	// Token: 0x0600202E RID: 8238 RVA: 0x000F2384 File Offset: 0x000F0584
	public Vector3 GetRandomPointInsideCollider(FlockingManager.FishArea fishArea)
	{
		int num = UnityEngine.Random.Range(0, fishArea.colliders.Length);
		BoxCollider boxCollider = fishArea.colliders[num];
		Vector3 vector = boxCollider.size / 2f;
		Vector3 position = new Vector3(UnityEngine.Random.Range(-vector.x, vector.x), UnityEngine.Random.Range(-vector.y, vector.y), UnityEngine.Random.Range(-vector.z, vector.z));
		return boxCollider.transform.TransformPoint(position);
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x000F2404 File Offset: 0x000F0604
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

	// Token: 0x06002030 RID: 8240 RVA: 0x000F24A0 File Offset: 0x000F06A0
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

	// Token: 0x06002031 RID: 8241 RVA: 0x000F2650 File Offset: 0x000F0850
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

	// Token: 0x06002032 RID: 8242 RVA: 0x00045DE3 File Offset: 0x00043FE3
	private void ProjectileHitExit(SlingshotProjectile projectile, Collider collider2)
	{
		UnityAction<BoxCollider> unityAction = this.onFoodDestroyed;
		if (unityAction == null)
		{
			return;
		}
		unityAction(collider2 as BoxCollider);
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06002033 RID: 8243 RVA: 0x00045DFB File Offset: 0x00043FFB
	// (set) Token: 0x06002034 RID: 8244 RVA: 0x00045E25 File Offset: 0x00044025
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

	// Token: 0x06002035 RID: 8245 RVA: 0x00045E50 File Offset: 0x00044050
	public override void WriteDataFusion()
	{
		this.Data = new FlockingData(this.allFish);
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x000F269C File Offset: 0x000F089C
	public override void ReadDataFusion()
	{
		for (int i = 0; i < this.Data.count; i++)
		{
			Vector3 syncPos = this.Data.Positions[i];
			Quaternion syncRot = this.Data.Rotations[i];
			this.allFish[i].SetSyncPosRot(syncPos, syncRot);
		}
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x00045E63 File Offset: 0x00044063
	public static void RegisterAvoidPoint(GameObject obj)
	{
		FlockingManager.avoidPoints.Add(obj);
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x00045E70 File Offset: 0x00044070
	public static void UnregisterAvoidPoint(GameObject obj)
	{
		FlockingManager.avoidPoints.Remove(obj);
	}

	// Token: 0x0600203D RID: 8253 RVA: 0x00045EBE File Offset: 0x000440BE
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x0600203E RID: 8254 RVA: 0x00045ED6 File Offset: 0x000440D6
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04002432 RID: 9266
	public List<GameObject> fishAreaContainer;

	// Token: 0x04002433 RID: 9267
	public string foodProjectileTag = "WaterBalloonProjectile";

	// Token: 0x04002434 RID: 9268
	private Dictionary<string, Vector3> areaToWaypointDict = new Dictionary<string, Vector3>();

	// Token: 0x04002435 RID: 9269
	private List<FlockingManager.FishArea> fishAreaList = new List<FlockingManager.FishArea>();

	// Token: 0x04002436 RID: 9270
	private List<Flocking> allFish = new List<Flocking>();

	// Token: 0x04002437 RID: 9271
	public UnityAction<FlockingManager.FishFood> onFoodDetected;

	// Token: 0x04002438 RID: 9272
	public UnityAction<BoxCollider> onFoodDestroyed;

	// Token: 0x04002439 RID: 9273
	private bool hasBeenSerialized;

	// Token: 0x0400243A RID: 9274
	public static readonly List<GameObject> avoidPoints = new List<GameObject>();

	// Token: 0x0400243B RID: 9275
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 337)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private FlockingData _Data;

	// Token: 0x0200052F RID: 1327
	public class FishArea
	{
		// Token: 0x0400243C RID: 9276
		public string id;

		// Token: 0x0400243D RID: 9277
		public List<Flocking> fishList = new List<Flocking>();

		// Token: 0x0400243E RID: 9278
		public Vector3 colliderCenter;

		// Token: 0x0400243F RID: 9279
		public BoxCollider[] colliders;

		// Token: 0x04002440 RID: 9280
		public Vector3 nextWaypoint = Vector3.zero;

		// Token: 0x04002441 RID: 9281
		public ZoneBasedObject zoneBasedObject;
	}

	// Token: 0x02000530 RID: 1328
	public class FishFood
	{
		// Token: 0x04002442 RID: 9282
		public BoxCollider collider;

		// Token: 0x04002443 RID: 9283
		public bool isRealFood;

		// Token: 0x04002444 RID: 9284
		public SlingshotProjectile slingshotProjectile;
	}
}
