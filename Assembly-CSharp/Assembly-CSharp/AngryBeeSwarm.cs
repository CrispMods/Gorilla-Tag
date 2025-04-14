using System;
using System.Collections.Generic;
using Fusion;
using GorillaTag.Rendering;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

// Token: 0x020000DD RID: 221
[NetworkBehaviourWeaved(3)]
public class AngryBeeSwarm : NetworkComponent
{
	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060005A8 RID: 1448 RVA: 0x000215D0 File Offset: 0x0001F7D0
	public bool isDormant
	{
		get
		{
			return this.currentState == AngryBeeSwarm.ChaseState.Dormant;
		}
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x000215DC File Offset: 0x0001F7DC
	protected override void Awake()
	{
		base.Awake();
		AngryBeeSwarm.instance = this;
		this.targetPlayer = null;
		this.currentState = AngryBeeSwarm.ChaseState.Dormant;
		this.grabTimestamp = -this.minGrabCooldown;
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00021630 File Offset: 0x0001F830
	private void InitializeSwarm()
	{
		if (NetworkSystem.Instance.InRoom && base.IsMine)
		{
			this.beeAnimator.transform.localPosition = Vector3.zero;
			this.lastSpeedIncreased = 0f;
			this.currentSpeed = 0f;
		}
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0002167C File Offset: 0x0001F87C
	private void LateUpdate()
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			this.currentState = AngryBeeSwarm.ChaseState.Dormant;
			this.UpdateState();
			return;
		}
		if (base.IsMine)
		{
			AngryBeeSwarm.ChaseState chaseState = this.currentState;
			switch (chaseState)
			{
			case AngryBeeSwarm.ChaseState.Dormant:
				if (Application.isEditor && Keyboard.current[Key.Space].wasPressedThisFrame)
				{
					this.currentState = AngryBeeSwarm.ChaseState.InitialEmerge;
				}
				break;
			case AngryBeeSwarm.ChaseState.InitialEmerge:
				if (Time.time > this.emergeStartedTimestamp + this.totalTimeToEmerge)
				{
					this.currentState = AngryBeeSwarm.ChaseState.Chasing;
				}
				break;
			case (AngryBeeSwarm.ChaseState)3:
				break;
			case AngryBeeSwarm.ChaseState.Chasing:
				if (this.followTarget == null || this.targetPlayer == null || Time.time > this.NextRefreshClosestPlayerTimestamp)
				{
					this.ChooseClosestTarget();
					if (this.followTarget != null)
					{
						this.BoredToDeathAtTimestamp = -1f;
					}
					else if (this.BoredToDeathAtTimestamp < 0f)
					{
						this.BoredToDeathAtTimestamp = Time.time + this.boredAfterDuration;
					}
				}
				if (this.BoredToDeathAtTimestamp >= 0f && Time.time > this.BoredToDeathAtTimestamp)
				{
					this.currentState = AngryBeeSwarm.ChaseState.Dormant;
				}
				else if (!(this.followTarget == null) && (this.followTarget.position - this.beeAnimator.transform.position).magnitude < this.catchDistance)
				{
					float num = ZoneShaderSettings.GetWaterY() + this.PlayerMinHeightAboveWater;
					if (this.followTarget.position.y > num)
					{
						this.currentState = AngryBeeSwarm.ChaseState.Grabbing;
					}
				}
				break;
			default:
				if (chaseState == AngryBeeSwarm.ChaseState.Grabbing)
				{
					if (Time.time > this.grabTimestamp + this.grabDuration)
					{
						this.currentState = AngryBeeSwarm.ChaseState.Dormant;
					}
				}
				break;
			}
		}
		if (this.lastState != this.currentState)
		{
			this.OnChangeState(this.currentState);
			this.lastState = this.currentState;
		}
		this.UpdateState();
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00021860 File Offset: 0x0001FA60
	public void UpdateState()
	{
		AngryBeeSwarm.ChaseState chaseState = this.currentState;
		switch (chaseState)
		{
		case AngryBeeSwarm.ChaseState.Dormant:
		case (AngryBeeSwarm.ChaseState)3:
			break;
		case AngryBeeSwarm.ChaseState.InitialEmerge:
			if (NetworkSystem.Instance.InRoom)
			{
				this.SwarmEmergeUpdateShared();
				return;
			}
			break;
		case AngryBeeSwarm.ChaseState.Chasing:
			if (NetworkSystem.Instance.InRoom)
			{
				if (base.IsMine)
				{
					this.ChaseHost();
				}
				this.MoveBodyShared();
				return;
			}
			break;
		default:
			if (chaseState != AngryBeeSwarm.ChaseState.Grabbing)
			{
				return;
			}
			if (NetworkSystem.Instance.InRoom)
			{
				if (this.targetPlayer == NetworkSystem.Instance.LocalPlayer)
				{
					this.RiseGrabbedLocalPlayer();
				}
				this.GrabBodyShared();
			}
			break;
		}
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x000218EF File Offset: 0x0001FAEF
	public void Emerge(Vector3 fromPosition, Vector3 toPosition)
	{
		base.transform.position = fromPosition;
		this.emergeFromPosition = fromPosition;
		this.emergeToPosition = toPosition;
		this.currentState = AngryBeeSwarm.ChaseState.InitialEmerge;
		this.emergeStartedTimestamp = Time.time;
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00021920 File Offset: 0x0001FB20
	private void OnChangeState(AngryBeeSwarm.ChaseState newState)
	{
		switch (newState)
		{
		case AngryBeeSwarm.ChaseState.Dormant:
			if (this.beeAnimator.gameObject.activeSelf)
			{
				this.beeAnimator.gameObject.SetActive(false);
			}
			if (base.IsMine)
			{
				this.targetPlayer = null;
				base.transform.position = new Vector3(0f, -9999f, 0f);
				this.InitializeSwarm();
			}
			this.SetInitialRotations();
			return;
		case AngryBeeSwarm.ChaseState.InitialEmerge:
			this.emergeStartedTimestamp = Time.time;
			if (!this.beeAnimator.gameObject.activeSelf)
			{
				this.beeAnimator.gameObject.SetActive(true);
			}
			this.beeAnimator.SetEmergeFraction(0f);
			if (base.IsMine)
			{
				this.currentSpeed = 0f;
				this.ChooseClosestTarget();
			}
			this.SetInitialRotations();
			return;
		case (AngryBeeSwarm.ChaseState)3:
			break;
		case AngryBeeSwarm.ChaseState.Chasing:
			if (!this.beeAnimator.gameObject.activeSelf)
			{
				this.beeAnimator.gameObject.SetActive(true);
			}
			this.beeAnimator.SetEmergeFraction(1f);
			this.ResetPath();
			this.NextRefreshClosestPlayerTimestamp = Time.time + this.RefreshClosestPlayerInterval;
			this.BoredToDeathAtTimestamp = -1f;
			return;
		default:
		{
			if (newState != AngryBeeSwarm.ChaseState.Grabbing)
			{
				return;
			}
			if (!this.beeAnimator.gameObject.activeSelf)
			{
				this.beeAnimator.gameObject.SetActive(true);
			}
			this.grabTimestamp = Time.time;
			this.beeAnimator.transform.localPosition = this.ghostOffsetGrabbingLocal;
			VRRig vrrig = GorillaGameManager.StaticFindRigForPlayer(this.targetPlayer);
			if (vrrig != null)
			{
				this.followTarget = vrrig.transform;
			}
			break;
		}
		}
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00021AC8 File Offset: 0x0001FCC8
	private void ChooseClosestTarget()
	{
		float num = Mathf.Lerp(this.initialRangeLimit, this.finalRangeLimit, (Time.time + this.totalTimeToEmerge - this.emergeStartedTimestamp) / this.rangeLimitBlendDuration);
		float num2 = num * num;
		VRRig vrrig = null;
		float num3 = ZoneShaderSettings.GetWaterY() + this.PlayerMinHeightAboveWater;
		foreach (VRRig vrrig2 in GorillaParent.instance.vrrigs)
		{
			if (vrrig2.head != null && !(vrrig2.head.rigTarget == null) && vrrig2.head.rigTarget.position.y > num3)
			{
				float sqrMagnitude = (base.transform.position - vrrig2.head.rigTarget.transform.position).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					num2 = sqrMagnitude;
					vrrig = vrrig2;
				}
			}
		}
		if (vrrig != null)
		{
			this.targetPlayer = vrrig.creator;
			this.followTarget = vrrig.head.rigTarget;
			NavMeshHit navMeshHit;
			this.targetIsOnNavMesh = NavMesh.SamplePosition(this.followTarget.position, out navMeshHit, 5f, 1);
		}
		else
		{
			this.targetPlayer = null;
			this.followTarget = null;
		}
		this.NextRefreshClosestPlayerTimestamp = Time.time + this.RefreshClosestPlayerInterval;
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00021C34 File Offset: 0x0001FE34
	private void SetInitialRotations()
	{
		this.beeAnimator.transform.localPosition = Vector3.zero;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00021C4C File Offset: 0x0001FE4C
	private void SwarmEmergeUpdateShared()
	{
		if (Time.time < this.emergeStartedTimestamp + this.totalTimeToEmerge)
		{
			float emergeFraction = (Time.time - this.emergeStartedTimestamp) / this.totalTimeToEmerge;
			if (base.IsMine)
			{
				base.transform.position = Vector3.Lerp(this.emergeFromPosition, this.emergeToPosition, (Time.time - this.emergeStartedTimestamp) / this.totalTimeToEmerge);
			}
			this.beeAnimator.SetEmergeFraction(emergeFraction);
		}
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00021CC4 File Offset: 0x0001FEC4
	private void RiseGrabbedLocalPlayer()
	{
		if (Time.time > this.grabTimestamp + this.minGrabCooldown)
		{
			this.grabTimestamp = Time.time;
			GorillaTagger.Instance.ApplyStatusEffect(GorillaTagger.StatusEffect.Frozen, GorillaTagger.Instance.tagCooldown);
			GorillaTagger.Instance.StartVibration(true, this.hapticStrength, this.hapticDuration);
			GorillaTagger.Instance.StartVibration(false, this.hapticStrength, this.hapticDuration);
		}
		if (Time.time < this.grabTimestamp + this.grabDuration)
		{
			GorillaTagger.Instance.rigidbody.velocity = Vector3.up * this.grabSpeed;
			EquipmentInteractor.instance.ForceStopClimbing();
		}
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00021D74 File Offset: 0x0001FF74
	public void UpdateFollowPath(Vector3 destination, float currentSpeed)
	{
		if (this.path == null)
		{
			this.GetNewPath(destination);
		}
		this.pathPoints[this.pathPoints.Count - 1] = destination;
		Vector3 vector = this.pathPoints[this.currentPathPointIdx];
		base.transform.position = Vector3.MoveTowards(base.transform.position, vector, currentSpeed * Time.deltaTime);
		Vector3 eulerAngles = Quaternion.LookRotation(vector - base.transform.position).eulerAngles;
		if (Mathf.Abs(eulerAngles.x) > 45f)
		{
			eulerAngles.x = 0f;
		}
		base.transform.rotation = Quaternion.Euler(eulerAngles);
		if (this.currentPathPointIdx + 1 < this.pathPoints.Count && (base.transform.position - vector).sqrMagnitude < 0.1f)
		{
			if (this.nextPathTimestamp <= Time.time)
			{
				this.GetNewPath(destination);
				return;
			}
			this.currentPathPointIdx++;
		}
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x00021E84 File Offset: 0x00020084
	private void GetNewPath(Vector3 destination)
	{
		this.path = new NavMeshPath();
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(base.transform.position, out navMeshHit, 5f, 1);
		NavMeshHit navMeshHit2;
		this.targetIsOnNavMesh = NavMesh.SamplePosition(destination, out navMeshHit2, 5f, 1);
		NavMesh.CalculatePath(navMeshHit.position, navMeshHit2.position, -1, this.path);
		this.pathPoints = new List<Vector3>();
		foreach (Vector3 a in this.path.corners)
		{
			this.pathPoints.Add(a + Vector3.up * this.heightAboveNavmesh);
		}
		this.pathPoints.Add(destination);
		this.currentPathPointIdx = 0;
		this.nextPathTimestamp = Time.time + 2f;
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x00021F58 File Offset: 0x00020158
	public void ResetPath()
	{
		this.path = null;
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x00021F64 File Offset: 0x00020164
	private void ChaseHost()
	{
		if (this.followTarget != null)
		{
			if (Time.time > this.lastSpeedIncreased + this.velocityIncreaseInterval)
			{
				this.lastSpeedIncreased = Time.time;
				this.currentSpeed += this.velocityStep;
			}
			float num = ZoneShaderSettings.GetWaterY() + this.MinHeightAboveWater;
			Vector3 position = this.followTarget.position;
			if (position.y < num)
			{
				position.y = num;
			}
			if (this.targetIsOnNavMesh)
			{
				this.UpdateFollowPath(position, this.currentSpeed);
				return;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, position, this.currentSpeed * Time.deltaTime);
		}
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x0002201C File Offset: 0x0002021C
	private void MoveBodyShared()
	{
		this.noisyOffset = new Vector3(Mathf.PerlinNoise(Time.time, 0f) - 0.5f, Mathf.PerlinNoise(Time.time, 10f) - 0.5f, Mathf.PerlinNoise(Time.time, 20f) - 0.5f);
		this.beeAnimator.transform.localPosition = this.noisyOffset;
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00022089 File Offset: 0x00020289
	private void GrabBodyShared()
	{
		if (this.followTarget != null)
		{
			base.transform.rotation = this.followTarget.rotation;
			base.transform.position = this.followTarget.position;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060005B9 RID: 1465 RVA: 0x000220C5 File Offset: 0x000202C5
	// (set) Token: 0x060005BA RID: 1466 RVA: 0x000220EF File Offset: 0x000202EF
	[Networked]
	[NetworkedWeaved(0, 3)]
	public unsafe BeeSwarmData Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing AngryBeeSwarm.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(BeeSwarmData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing AngryBeeSwarm.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(BeeSwarmData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0002211A File Offset: 0x0002031A
	public override void WriteDataFusion()
	{
		this.Data = new BeeSwarmData(this.targetPlayer.ActorNumber, (int)this.currentState, this.currentSpeed);
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00022140 File Offset: 0x00020340
	public override void ReadDataFusion()
	{
		this.targetPlayer = NetworkSystem.Instance.GetPlayer(this.Data.TargetActorNumber);
		this.currentState = (AngryBeeSwarm.ChaseState)this.Data.CurrentState;
		if (float.IsFinite(this.Data.CurrentSpeed))
		{
			this.currentSpeed = this.Data.CurrentSpeed;
		}
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x000221A8 File Offset: 0x000203A8
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		stream.SendNext(this.targetPlayer.ActorNumber);
		stream.SendNext(this.currentState);
		stream.SendNext(this.currentSpeed);
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x000221FC File Offset: 0x000203FC
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		int playerID = (int)stream.ReceiveNext();
		this.targetPlayer = NetworkSystem.Instance.GetPlayer(playerID);
		this.currentState = (AngryBeeSwarm.ChaseState)stream.ReceiveNext();
		float f = (float)stream.ReceiveNext();
		if (float.IsFinite(f))
		{
			this.currentSpeed = f;
		}
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x00022260 File Offset: 0x00020460
	public override void OnOwnerChange(Player newOwner, Player previousOwner)
	{
		base.OnOwnerChange(newOwner, previousOwner);
		if (newOwner == PhotonNetwork.LocalPlayer)
		{
			this.OnChangeState(this.currentState);
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x0002227E File Offset: 0x0002047E
	public void OnJoinedRoom()
	{
		Debug.Log("Here");
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.InitializeSwarm();
		}
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x0002229C File Offset: 0x0002049C
	private void TestEmerge()
	{
		this.Emerge(this.testEmergeFrom.transform.position, this.testEmergeTo.transform.position);
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00022350 File Offset: 0x00020550
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00022368 File Offset: 0x00020568
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x040006B5 RID: 1717
	public static AngryBeeSwarm instance;

	// Token: 0x040006B6 RID: 1718
	public float heightAboveNavmesh = 0.5f;

	// Token: 0x040006B7 RID: 1719
	public Transform followTarget;

	// Token: 0x040006B8 RID: 1720
	[SerializeField]
	private float velocityStep = 1f;

	// Token: 0x040006B9 RID: 1721
	private float currentSpeed;

	// Token: 0x040006BA RID: 1722
	[SerializeField]
	private float velocityIncreaseInterval = 20f;

	// Token: 0x040006BB RID: 1723
	public Vector3 noisyOffset;

	// Token: 0x040006BC RID: 1724
	public Vector3 ghostOffsetGrabbingLocal;

	// Token: 0x040006BD RID: 1725
	private float emergeStartedTimestamp;

	// Token: 0x040006BE RID: 1726
	private float grabTimestamp;

	// Token: 0x040006BF RID: 1727
	private float lastSpeedIncreased;

	// Token: 0x040006C0 RID: 1728
	[SerializeField]
	private float totalTimeToEmerge;

	// Token: 0x040006C1 RID: 1729
	[SerializeField]
	private float catchDistance;

	// Token: 0x040006C2 RID: 1730
	[SerializeField]
	private float grabDuration;

	// Token: 0x040006C3 RID: 1731
	[SerializeField]
	private float grabSpeed = 1f;

	// Token: 0x040006C4 RID: 1732
	[SerializeField]
	private float minGrabCooldown;

	// Token: 0x040006C5 RID: 1733
	[SerializeField]
	private float initialRangeLimit;

	// Token: 0x040006C6 RID: 1734
	[SerializeField]
	private float finalRangeLimit;

	// Token: 0x040006C7 RID: 1735
	[SerializeField]
	private float rangeLimitBlendDuration;

	// Token: 0x040006C8 RID: 1736
	[SerializeField]
	private float boredAfterDuration;

	// Token: 0x040006C9 RID: 1737
	public NetPlayer targetPlayer;

	// Token: 0x040006CA RID: 1738
	public AngryBeeAnimator beeAnimator;

	// Token: 0x040006CB RID: 1739
	public AngryBeeSwarm.ChaseState currentState;

	// Token: 0x040006CC RID: 1740
	public AngryBeeSwarm.ChaseState lastState;

	// Token: 0x040006CD RID: 1741
	public NetPlayer grabbedPlayer;

	// Token: 0x040006CE RID: 1742
	private bool targetIsOnNavMesh;

	// Token: 0x040006CF RID: 1743
	private const float navMeshSampleRange = 5f;

	// Token: 0x040006D0 RID: 1744
	[Tooltip("Haptic vibration when chased by lucy")]
	public float hapticStrength = 1f;

	// Token: 0x040006D1 RID: 1745
	public float hapticDuration = 1.5f;

	// Token: 0x040006D2 RID: 1746
	public float MinHeightAboveWater = 0.5f;

	// Token: 0x040006D3 RID: 1747
	public float PlayerMinHeightAboveWater = 0.5f;

	// Token: 0x040006D4 RID: 1748
	public float RefreshClosestPlayerInterval = 1f;

	// Token: 0x040006D5 RID: 1749
	private float NextRefreshClosestPlayerTimestamp = 1f;

	// Token: 0x040006D6 RID: 1750
	private float BoredToDeathAtTimestamp = -1f;

	// Token: 0x040006D7 RID: 1751
	[SerializeField]
	private Transform testEmergeFrom;

	// Token: 0x040006D8 RID: 1752
	[SerializeField]
	private Transform testEmergeTo;

	// Token: 0x040006D9 RID: 1753
	private Vector3 emergeFromPosition;

	// Token: 0x040006DA RID: 1754
	private Vector3 emergeToPosition;

	// Token: 0x040006DB RID: 1755
	private NavMeshPath path;

	// Token: 0x040006DC RID: 1756
	public List<Vector3> pathPoints;

	// Token: 0x040006DD RID: 1757
	public int currentPathPointIdx;

	// Token: 0x040006DE RID: 1758
	private float nextPathTimestamp;

	// Token: 0x040006DF RID: 1759
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 3)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private BeeSwarmData _Data;

	// Token: 0x020000DE RID: 222
	public enum ChaseState
	{
		// Token: 0x040006E1 RID: 1761
		Dormant = 1,
		// Token: 0x040006E2 RID: 1762
		InitialEmerge,
		// Token: 0x040006E3 RID: 1763
		Chasing = 4,
		// Token: 0x040006E4 RID: 1764
		Grabbing = 8
	}
}
