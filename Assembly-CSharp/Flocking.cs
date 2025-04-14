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
	// (get) Token: 0x06001FB8 RID: 8120 RVA: 0x0009FFD1 File Offset: 0x0009E1D1
	// (set) Token: 0x06001FB9 RID: 8121 RVA: 0x0009FFD9 File Offset: 0x0009E1D9
	public FlockingManager.FishArea FishArea { get; set; }

	// Token: 0x06001FBA RID: 8122 RVA: 0x0009FFE2 File Offset: 0x0009E1E2
	private void Awake()
	{
		this.manager = base.GetComponentInParent<FlockingManager>();
	}

	// Token: 0x06001FBB RID: 8123 RVA: 0x0009FFF0 File Offset: 0x0009E1F0
	private void Start()
	{
		this.speed = Random.Range(this.minSpeed, this.maxSpeed);
		this.fishState = Flocking.FishState.patrol;
	}

	// Token: 0x06001FBC RID: 8124 RVA: 0x000A0010 File Offset: 0x0009E210
	private void OnDisable()
	{
		FlockingManager flockingManager = this.manager;
		flockingManager.onFoodDetected = (UnityAction<FlockingManager.FishFood>)Delegate.Remove(flockingManager.onFoodDetected, new UnityAction<FlockingManager.FishFood>(this.HandleOnFoodDetected));
		FlockingManager flockingManager2 = this.manager;
		flockingManager2.onFoodDestroyed = (UnityAction<BoxCollider>)Delegate.Remove(flockingManager2.onFoodDestroyed, new UnityAction<BoxCollider>(this.HandleOnFoodDestroyed));
		FlockingUpdateManager.UnregisterFlocking(this);
	}

	// Token: 0x06001FBD RID: 8125 RVA: 0x000A0074 File Offset: 0x0009E274
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
			if (Random.Range(0, 10) < 2)
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

	// Token: 0x06001FBE RID: 8126 RVA: 0x000A0210 File Offset: 0x0009E410
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

	// Token: 0x06001FBF RID: 8127 RVA: 0x000A0288 File Offset: 0x0009E488
	private void Turn(Vector3 towardPoint)
	{
		this.isTurning = true;
		Quaternion to = Quaternion.LookRotation(towardPoint - base.transform.position);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.rotationSpeed * Time.deltaTime);
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000A02DB File Offset: 0x0009E4DB
	private void SwitchState(Flocking.FishState state)
	{
		this.fishState = state;
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000A02E4 File Offset: 0x0009E4E4
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

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000A0488 File Offset: 0x0009E688
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

	// Token: 0x06001FC3 RID: 8131 RVA: 0x000A04F8 File Offset: 0x0009E6F8
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

	// Token: 0x06001FC4 RID: 8132 RVA: 0x000A054C File Offset: 0x0009E74C
	private void FollowFood()
	{
		this.followingFood = true;
		Quaternion to = Quaternion.LookRotation(this.projectileGameObject.transform.position - base.transform.position);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.rotationSpeed * Time.deltaTime);
		base.transform.position = Vector3.MoveTowards(base.transform.position, this.projectileGameObject.transform.position, this.speed * this.followFoodSpeedMult * Time.deltaTime);
	}

	// Token: 0x06001FC5 RID: 8133 RVA: 0x000A05EC File Offset: 0x0009E7EC
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

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000A0684 File Offset: 0x0009E884
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

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000A06F8 File Offset: 0x0009E8F8
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

	// Token: 0x040023BC RID: 9148
	[Tooltip("Speed is randomly generated from min and max speed")]
	public float minSpeed = 2f;

	// Token: 0x040023BD RID: 9149
	public float maxSpeed = 4f;

	// Token: 0x040023BE RID: 9150
	public float rotationSpeed = 360f;

	// Token: 0x040023BF RID: 9151
	[Tooltip("Maximum distance to the neighbours to form a flocking group")]
	public float maxNeighbourDistance = 4f;

	// Token: 0x040023C0 RID: 9152
	public float eatFoodDuration = 10f;

	// Token: 0x040023C1 RID: 9153
	[Tooltip("How fast should it follow the food? This value multiplies by the current speed")]
	public float followFoodSpeedMult = 3f;

	// Token: 0x040023C2 RID: 9154
	[Tooltip("How fast should it run away from players hand?")]
	public float avoidHandSpeed = 1.2f;

	// Token: 0x040023C3 RID: 9155
	[FormerlySerializedAs("avoidanceDistance")]
	[Tooltip("When flocking they will avoid each other if the distance between them is less than this value")]
	public float flockingAvoidanceDistance = 2f;

	// Token: 0x040023C4 RID: 9156
	[Tooltip("Follow the fish food until they are this far from it")]
	[FormerlySerializedAs("distanceToFollowFood")]
	public double FollowFoodStopDistance = 0.20000000298023224;

	// Token: 0x040023C5 RID: 9157
	[Tooltip("Follow any fake fish food until they are this far from it")]
	[FormerlySerializedAs("distanceToFollowFakeFood")]
	public float FollowFakeFoodStopDistance = 2f;

	// Token: 0x040023C6 RID: 9158
	private float speed;

	// Token: 0x040023C7 RID: 9159
	private Vector3 averageHeading;

	// Token: 0x040023C8 RID: 9160
	private Vector3 averagePosition;

	// Token: 0x040023C9 RID: 9161
	private float feedingTimeStarted;

	// Token: 0x040023CA RID: 9162
	private GameObject projectileGameObject;

	// Token: 0x040023CB RID: 9163
	private bool followingFood;

	// Token: 0x040023CC RID: 9164
	private FlockingManager manager;

	// Token: 0x040023CD RID: 9165
	private GameObjectManagerWithId _fishSceneGameObjectsManager;

	// Token: 0x040023CE RID: 9166
	private UnityEvent<string, Transform> sendIdEvent;

	// Token: 0x040023CF RID: 9167
	private Flocking.FishState fishState;

	// Token: 0x040023D0 RID: 9168
	[HideInInspector]
	public Vector3 pos;

	// Token: 0x040023D1 RID: 9169
	[HideInInspector]
	public Quaternion rot;

	// Token: 0x040023D2 RID: 9170
	private float velocity;

	// Token: 0x040023D3 RID: 9171
	private bool isTurning;

	// Token: 0x040023D4 RID: 9172
	private bool isRealFood;

	// Token: 0x040023D5 RID: 9173
	public float avointPointRadius = 0.5f;

	// Token: 0x040023D6 RID: 9174
	private float cacheSpeed;

	// Token: 0x0200051F RID: 1311
	public enum FishState
	{
		// Token: 0x040023D9 RID: 9177
		flock,
		// Token: 0x040023DA RID: 9178
		patrol,
		// Token: 0x040023DB RID: 9179
		followFood
	}
}
