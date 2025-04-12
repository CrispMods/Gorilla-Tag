using System;
using GorillaExtensions;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x0200051E RID: 1310
public class Flocking : MonoBehaviour
{
	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06001FBB RID: 8123 RVA: 0x0004493C File Offset: 0x00042B3C
	// (set) Token: 0x06001FBC RID: 8124 RVA: 0x00044944 File Offset: 0x00042B44
	public FlockingManager.FishArea FishArea { get; set; }

	// Token: 0x06001FBD RID: 8125 RVA: 0x0004494D File Offset: 0x00042B4D
	private void Awake()
	{
		this.manager = base.GetComponentInParent<FlockingManager>();
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x0004495B File Offset: 0x00042B5B
	private void Start()
	{
		this.speed = UnityEngine.Random.Range(this.minSpeed, this.maxSpeed);
		this.fishState = Flocking.FishState.patrol;
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x000EEB14 File Offset: 0x000ECD14
	private void OnDisable()
	{
		FlockingManager flockingManager = this.manager;
		flockingManager.onFoodDetected = (UnityAction<FlockingManager.FishFood>)Delegate.Remove(flockingManager.onFoodDetected, new UnityAction<FlockingManager.FishFood>(this.HandleOnFoodDetected));
		FlockingManager flockingManager2 = this.manager;
		flockingManager2.onFoodDestroyed = (UnityAction<BoxCollider>)Delegate.Remove(flockingManager2.onFoodDestroyed, new UnityAction<BoxCollider>(this.HandleOnFoodDestroyed));
		FlockingUpdateManager.UnregisterFlocking(this);
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000EEB78 File Offset: 0x000ECD78
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

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000EED14 File Offset: 0x000ECF14
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

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000EED8C File Offset: 0x000ECF8C
	private void Turn(Vector3 towardPoint)
	{
		this.isTurning = true;
		Quaternion to = Quaternion.LookRotation(towardPoint - base.transform.position);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.rotationSpeed * Time.deltaTime);
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x0004497B File Offset: 0x00042B7B
	private void SwitchState(Flocking.FishState state)
	{
		this.fishState = state;
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x000EEDE0 File Offset: 0x000ECFE0
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

	// Token: 0x06001FC5 RID: 8133 RVA: 0x000EEF84 File Offset: 0x000ED184
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

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000EEFF4 File Offset: 0x000ED1F4
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

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000EF048 File Offset: 0x000ED248
	private void FollowFood()
	{
		this.followingFood = true;
		Quaternion to = Quaternion.LookRotation(this.projectileGameObject.transform.position - base.transform.position);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.rotationSpeed * Time.deltaTime);
		base.transform.position = Vector3.MoveTowards(base.transform.position, this.projectileGameObject.transform.position, this.speed * this.followFoodSpeedMult * Time.deltaTime);
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x000EF0E8 File Offset: 0x000ED2E8
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

	// Token: 0x06001FC9 RID: 8137 RVA: 0x000EF180 File Offset: 0x000ED380
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

	// Token: 0x06001FCA RID: 8138 RVA: 0x000EF1F4 File Offset: 0x000ED3F4
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

	// Token: 0x040023BD RID: 9149
	[Tooltip("Speed is randomly generated from min and max speed")]
	public float minSpeed = 2f;

	// Token: 0x040023BE RID: 9150
	public float maxSpeed = 4f;

	// Token: 0x040023BF RID: 9151
	public float rotationSpeed = 360f;

	// Token: 0x040023C0 RID: 9152
	[Tooltip("Maximum distance to the neighbours to form a flocking group")]
	public float maxNeighbourDistance = 4f;

	// Token: 0x040023C1 RID: 9153
	public float eatFoodDuration = 10f;

	// Token: 0x040023C2 RID: 9154
	[Tooltip("How fast should it follow the food? This value multiplies by the current speed")]
	public float followFoodSpeedMult = 3f;

	// Token: 0x040023C3 RID: 9155
	[Tooltip("How fast should it run away from players hand?")]
	public float avoidHandSpeed = 1.2f;

	// Token: 0x040023C4 RID: 9156
	[FormerlySerializedAs("avoidanceDistance")]
	[Tooltip("When flocking they will avoid each other if the distance between them is less than this value")]
	public float flockingAvoidanceDistance = 2f;

	// Token: 0x040023C5 RID: 9157
	[Tooltip("Follow the fish food until they are this far from it")]
	[FormerlySerializedAs("distanceToFollowFood")]
	public double FollowFoodStopDistance = 0.20000000298023224;

	// Token: 0x040023C6 RID: 9158
	[Tooltip("Follow any fake fish food until they are this far from it")]
	[FormerlySerializedAs("distanceToFollowFakeFood")]
	public float FollowFakeFoodStopDistance = 2f;

	// Token: 0x040023C7 RID: 9159
	private float speed;

	// Token: 0x040023C8 RID: 9160
	private Vector3 averageHeading;

	// Token: 0x040023C9 RID: 9161
	private Vector3 averagePosition;

	// Token: 0x040023CA RID: 9162
	private float feedingTimeStarted;

	// Token: 0x040023CB RID: 9163
	private GameObject projectileGameObject;

	// Token: 0x040023CC RID: 9164
	private bool followingFood;

	// Token: 0x040023CD RID: 9165
	private FlockingManager manager;

	// Token: 0x040023CE RID: 9166
	private GameObjectManagerWithId _fishSceneGameObjectsManager;

	// Token: 0x040023CF RID: 9167
	private UnityEvent<string, Transform> sendIdEvent;

	// Token: 0x040023D0 RID: 9168
	private Flocking.FishState fishState;

	// Token: 0x040023D1 RID: 9169
	[HideInInspector]
	public Vector3 pos;

	// Token: 0x040023D2 RID: 9170
	[HideInInspector]
	public Quaternion rot;

	// Token: 0x040023D3 RID: 9171
	private float velocity;

	// Token: 0x040023D4 RID: 9172
	private bool isTurning;

	// Token: 0x040023D5 RID: 9173
	private bool isRealFood;

	// Token: 0x040023D6 RID: 9174
	public float avointPointRadius = 0.5f;

	// Token: 0x040023D7 RID: 9175
	private float cacheSpeed;

	// Token: 0x0200051F RID: 1311
	public enum FishState
	{
		// Token: 0x040023DA RID: 9178
		flock,
		// Token: 0x040023DB RID: 9179
		patrol,
		// Token: 0x040023DC RID: 9180
		followFood
	}
}
