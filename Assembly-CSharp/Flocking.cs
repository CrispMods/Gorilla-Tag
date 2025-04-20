using System;
using GorillaExtensions;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x0200052B RID: 1323
public class Flocking : MonoBehaviour
{
	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06002011 RID: 8209 RVA: 0x00045CDB File Offset: 0x00043EDB
	// (set) Token: 0x06002012 RID: 8210 RVA: 0x00045CE3 File Offset: 0x00043EE3
	public FlockingManager.FishArea FishArea { get; set; }

	// Token: 0x06002013 RID: 8211 RVA: 0x00045CEC File Offset: 0x00043EEC
	private void Awake()
	{
		this.manager = base.GetComponentInParent<FlockingManager>();
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x00045CFA File Offset: 0x00043EFA
	private void Start()
	{
		this.speed = UnityEngine.Random.Range(this.minSpeed, this.maxSpeed);
		this.fishState = Flocking.FishState.patrol;
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x000F1898 File Offset: 0x000EFA98
	private void OnDisable()
	{
		FlockingManager flockingManager = this.manager;
		flockingManager.onFoodDetected = (UnityAction<FlockingManager.FishFood>)Delegate.Remove(flockingManager.onFoodDetected, new UnityAction<FlockingManager.FishFood>(this.HandleOnFoodDetected));
		FlockingManager flockingManager2 = this.manager;
		flockingManager2.onFoodDestroyed = (UnityAction<BoxCollider>)Delegate.Remove(flockingManager2.onFoodDestroyed, new UnityAction<BoxCollider>(this.HandleOnFoodDestroyed));
		FlockingUpdateManager.UnregisterFlocking(this);
	}

	// Token: 0x06002016 RID: 8214 RVA: 0x000F18FC File Offset: 0x000EFAFC
	public void InvokeUpdate()
	{
		if (this.manager == null)
		{
			this.manager = base.GetComponentInParent<FlockingManager>();
		}
		this.AvoidPlayerHands();
		this.MaybeTurn();
		switch (this.fishState)
		{
		case Flocking.FishState.flock:
			this.Flock(this.FishArea.nextWaypoint);
			this.SwitchState(Flocking.FishState.patrol);
			break;
		case Flocking.FishState.patrol:
			if (UnityEngine.Random.Range(0, 10) < 2)
			{
				this.SwitchState(Flocking.FishState.flock);
			}
			break;
		case Flocking.FishState.followFood:
			if (this.isTurning)
			{
				return;
			}
			if (this.isRealFood)
			{
				if ((double)Vector3.Distance(base.transform.position, this.projectileGameObject.transform.position) > this.FollowFoodStopDistance)
				{
					this.FollowFood();
				}
				else
				{
					this.followingFood = false;
					this.Flock(this.projectileGameObject.transform.position);
					this.feedingTimeStarted += Time.deltaTime;
					if (this.feedingTimeStarted > this.eatFoodDuration)
					{
						this.SwitchState(Flocking.FishState.patrol);
					}
				}
			}
			else if (Vector3.Distance(base.transform.position, this.projectileGameObject.transform.position) > this.FollowFakeFoodStopDistance)
			{
				this.FollowFood();
			}
			else
			{
				this.followingFood = false;
				this.SwitchState(Flocking.FishState.patrol);
			}
			break;
		}
		if (!this.followingFood)
		{
			base.transform.Translate(0f, 0f, this.speed * Time.deltaTime);
		}
		this.pos = base.transform.position;
		this.rot = base.transform.rotation;
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x000F1A98 File Offset: 0x000EFC98
	private void MaybeTurn()
	{
		if (!this.manager.IsInside(base.transform.position, this.FishArea))
		{
			this.Turn(this.FishArea.colliderCenter);
			if (Vector3.Angle(this.FishArea.colliderCenter - base.transform.position, Vector3.forward) > 5f)
			{
				this.isTurning = true;
				return;
			}
		}
		else
		{
			this.isTurning = false;
		}
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x000F1B10 File Offset: 0x000EFD10
	private void Turn(Vector3 towardPoint)
	{
		this.isTurning = true;
		Quaternion to = Quaternion.LookRotation(towardPoint - base.transform.position);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.rotationSpeed * Time.deltaTime);
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x00045D1A File Offset: 0x00043F1A
	private void SwitchState(Flocking.FishState state)
	{
		this.fishState = state;
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x000F1B64 File Offset: 0x000EFD64
	private void Flock(Vector3 nextGoal)
	{
		Vector3 a = Vector3.zero;
		Vector3 vector = Vector3.zero;
		float num = 1f;
		int num2 = 0;
		foreach (Flocking flocking in this.FishArea.fishList)
		{
			if (flocking.gameObject != base.gameObject)
			{
				float num3 = Vector3.Distance(flocking.transform.position, base.transform.position);
				if (num3 <= this.maxNeighbourDistance)
				{
					a += flocking.transform.position;
					num2++;
					if (num3 < this.flockingAvoidanceDistance)
					{
						vector += base.transform.position - flocking.transform.position;
					}
					num += flocking.speed;
				}
			}
		}
		if (num2 > 0)
		{
			this.fishState = Flocking.FishState.flock;
			a = a / (float)num2 + (nextGoal - base.transform.position);
			this.speed = num / (float)num2;
			this.speed = Mathf.Clamp(this.speed, this.minSpeed, this.maxSpeed);
			Vector3 vector2 = a + vector - base.transform.position;
			if (vector2 != Vector3.zero)
			{
				Quaternion to = Quaternion.LookRotation(vector2);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.rotationSpeed * Time.deltaTime);
			}
		}
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x000F1D08 File Offset: 0x000EFF08
	private void HandleOnFoodDetected(FlockingManager.FishFood fishFood)
	{
		bool flag = false;
		foreach (BoxCollider y in this.FishArea.colliders)
		{
			if (fishFood.collider == y)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		this.SwitchState(Flocking.FishState.followFood);
		this.feedingTimeStarted = 0f;
		this.projectileGameObject = fishFood.slingshotProjectile.gameObject;
		this.isRealFood = fishFood.isRealFood;
	}

	// Token: 0x0600201C RID: 8220 RVA: 0x000F1D78 File Offset: 0x000EFF78
	private void HandleOnFoodDestroyed(BoxCollider collider)
	{
		bool flag = false;
		foreach (BoxCollider y in this.FishArea.colliders)
		{
			if (collider == y)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		this.SwitchState(Flocking.FishState.patrol);
		this.projectileGameObject = null;
		this.followingFood = false;
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000F1DCC File Offset: 0x000EFFCC
	private void FollowFood()
	{
		this.followingFood = true;
		Quaternion to = Quaternion.LookRotation(this.projectileGameObject.transform.position - base.transform.position);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.rotationSpeed * Time.deltaTime);
		base.transform.position = Vector3.MoveTowards(base.transform.position, this.projectileGameObject.transform.position, this.speed * this.followFoodSpeedMult * Time.deltaTime);
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x000F1E6C File Offset: 0x000F006C
	private void AvoidPlayerHands()
	{
		foreach (GameObject gameObject in FlockingManager.avoidPoints)
		{
			Vector3 position = gameObject.transform.position;
			if ((base.transform.position - position).IsShorterThan(this.avointPointRadius))
			{
				Vector3 randomPointInsideCollider = this.manager.GetRandomPointInsideCollider(this.FishArea);
				this.Turn(randomPointInsideCollider);
				this.speed = this.avoidHandSpeed;
			}
		}
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x000F1F04 File Offset: 0x000F0104
	internal void SetSyncPosRot(Vector3 syncPos, Quaternion syncRot)
	{
		if (this.manager == null)
		{
			this.manager = base.GetComponentInParent<FlockingManager>();
		}
		if (this.FishArea == null)
		{
			Debug.LogError("FISH AREA NULL");
		}
		if (syncRot.IsValid())
		{
			this.rot = syncRot;
		}
		float num = 10000f;
		if (syncPos.IsValid(num))
		{
			this.pos = this.manager.RestrictPointToArea(syncPos, this.FishArea);
		}
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x000F1F78 File Offset: 0x000F0178
	private void OnEnable()
	{
		if (this.manager == null)
		{
			this.manager = base.GetComponentInParent<FlockingManager>();
		}
		FlockingManager flockingManager = this.manager;
		flockingManager.onFoodDetected = (UnityAction<FlockingManager.FishFood>)Delegate.Combine(flockingManager.onFoodDetected, new UnityAction<FlockingManager.FishFood>(this.HandleOnFoodDetected));
		FlockingManager flockingManager2 = this.manager;
		flockingManager2.onFoodDestroyed = (UnityAction<BoxCollider>)Delegate.Combine(flockingManager2.onFoodDestroyed, new UnityAction<BoxCollider>(this.HandleOnFoodDestroyed));
		FlockingUpdateManager.RegisterFlocking(this);
	}

	// Token: 0x0400240F RID: 9231
	[Tooltip("Speed is randomly generated from min and max speed")]
	public float minSpeed = 2f;

	// Token: 0x04002410 RID: 9232
	public float maxSpeed = 4f;

	// Token: 0x04002411 RID: 9233
	public float rotationSpeed = 360f;

	// Token: 0x04002412 RID: 9234
	[Tooltip("Maximum distance to the neighbours to form a flocking group")]
	public float maxNeighbourDistance = 4f;

	// Token: 0x04002413 RID: 9235
	public float eatFoodDuration = 10f;

	// Token: 0x04002414 RID: 9236
	[Tooltip("How fast should it follow the food? This value multiplies by the current speed")]
	public float followFoodSpeedMult = 3f;

	// Token: 0x04002415 RID: 9237
	[Tooltip("How fast should it run away from players hand?")]
	public float avoidHandSpeed = 1.2f;

	// Token: 0x04002416 RID: 9238
	[FormerlySerializedAs("avoidanceDistance")]
	[Tooltip("When flocking they will avoid each other if the distance between them is less than this value")]
	public float flockingAvoidanceDistance = 2f;

	// Token: 0x04002417 RID: 9239
	[Tooltip("Follow the fish food until they are this far from it")]
	[FormerlySerializedAs("distanceToFollowFood")]
	public double FollowFoodStopDistance = 0.20000000298023224;

	// Token: 0x04002418 RID: 9240
	[Tooltip("Follow any fake fish food until they are this far from it")]
	[FormerlySerializedAs("distanceToFollowFakeFood")]
	public float FollowFakeFoodStopDistance = 2f;

	// Token: 0x04002419 RID: 9241
	private float speed;

	// Token: 0x0400241A RID: 9242
	private Vector3 averageHeading;

	// Token: 0x0400241B RID: 9243
	private Vector3 averagePosition;

	// Token: 0x0400241C RID: 9244
	private float feedingTimeStarted;

	// Token: 0x0400241D RID: 9245
	private GameObject projectileGameObject;

	// Token: 0x0400241E RID: 9246
	private bool followingFood;

	// Token: 0x0400241F RID: 9247
	private FlockingManager manager;

	// Token: 0x04002420 RID: 9248
	private GameObjectManagerWithId _fishSceneGameObjectsManager;

	// Token: 0x04002421 RID: 9249
	private UnityEvent<string, Transform> sendIdEvent;

	// Token: 0x04002422 RID: 9250
	private Flocking.FishState fishState;

	// Token: 0x04002423 RID: 9251
	[HideInInspector]
	public Vector3 pos;

	// Token: 0x04002424 RID: 9252
	[HideInInspector]
	public Quaternion rot;

	// Token: 0x04002425 RID: 9253
	private float velocity;

	// Token: 0x04002426 RID: 9254
	private bool isTurning;

	// Token: 0x04002427 RID: 9255
	private bool isRealFood;

	// Token: 0x04002428 RID: 9256
	public float avointPointRadius = 0.5f;

	// Token: 0x04002429 RID: 9257
	private float cacheSpeed;

	// Token: 0x0200052C RID: 1324
	public enum FishState
	{
		// Token: 0x0400242C RID: 9260
		flock,
		// Token: 0x0400242D RID: 9261
		patrol,
		// Token: 0x0400242E RID: 9262
		followFood
	}
}
