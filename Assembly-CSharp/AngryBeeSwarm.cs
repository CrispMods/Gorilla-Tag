using System;
using System.Collections.Generic;
using Fusion;
using GorillaTag.Rendering;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

// Token: 0x020000E7 RID: 231
[NetworkBehaviourWeaved(3)]
public class AngryBeeSwarm : NetworkComponent
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060005E7 RID: 1511 RVA: 0x00034668 File Offset: 0x00032868
	public bool isDormant
	{
		get
		{
			return this.currentState == AngryBeeSwarm.ChaseState.Dormant;
		}
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x00083DE0 File Offset: 0x00081FE0
	protected override void Awake()
	{
		base.Awake();
		AngryBeeSwarm.instance = this;
		this.targetPlayer = null;
		this.currentState = AngryBeeSwarm.ChaseState.Dormant;
		this.grabTimestamp = -this.minGrabCooldown;
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x00083E34 File Offset: 0x00082034
	private void InitializeSwarm()
	{
		if (NetworkSystem.Instance.InRoom && base.IsMine)
		{
			this.beeAnimator.transform.localPosition = Vector3.zero;
			this.lastSpeedIncreased = 0f;
			this.currentSpeed = 0f;
		}
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x00083E80 File Offset: 0x00082080
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

	// Token: 0x060005EB RID: 1515 RVA: 0x00084064 File Offset: 0x00082264
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

	// Token: 0x060005EC RID: 1516 RVA: 0x00034673 File Offset: 0x00032873
	public void Emerge(Vector3 fromPosition, Vector3 toPosition)
	{
		base.transform.position = fromPosition;
		this.emergeFromPosition = fromPosition;
		this.emergeToPosition = toPosition;
		this.currentState = AngryBeeSwarm.ChaseState.InitialEmerge;
		this.emergeStartedTimestamp = Time.time;
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x000840F4 File Offset: 0x000822F4
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

	// Token: 0x060005EE RID: 1518 RVA: 0x0008429C File Offset: 0x0008249C
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

	// Token: 0x060005EF RID: 1519 RVA: 0x000346A1 File Offset: 0x000328A1
	private void SetInitialRotations()
	{
		this.beeAnimator.transform.localPosition = Vector3.zero;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x00084408 File Offset: 0x00082608
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

	// Token: 0x060005F1 RID: 1521 RVA: 0x00084480 File Offset: 0x00082680
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

	// Token: 0x060005F2 RID: 1522 RVA: 0x00084530 File Offset: 0x00082730
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

	// Token: 0x060005F3 RID: 1523 RVA: 0x00084640 File Offset: 0x00082840
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

	// Token: 0x060005F4 RID: 1524 RVA: 0x000346B8 File Offset: 0x000328B8
	public void ResetPath()
	{
		this.path = null;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x00084714 File Offset: 0x00082914
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

	// Token: 0x060005F6 RID: 1526 RVA: 0x000847CC File Offset: 0x000829CC
	private void MoveBodyShared()
	{
		this.noisyOffset = new Vector3(Mathf.PerlinNoise(Time.time, 0f) - 0.5f, Mathf.PerlinNoise(Time.time, 10f) - 0.5f, Mathf.PerlinNoise(Time.time, 20f) - 0.5f);
		this.beeAnimator.transform.localPosition = this.noisyOffset;
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x000346C1 File Offset: 0x000328C1
	private void GrabBodyShared()
	{
		if (this.followTarget != null)
		{
			base.transform.rotation = this.followTarget.rotation;
			base.transform.position = this.followTarget.position;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060005F8 RID: 1528 RVA: 0x000346FD File Offset: 0x000328FD
	// (set) Token: 0x060005F9 RID: 1529 RVA: 0x00034727 File Offset: 0x00032927
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

	// Token: 0x060005FA RID: 1530 RVA: 0x00034752 File Offset: 0x00032952
	public override void WriteDataFusion()
	{
		this.Data = new BeeSwarmData(this.targetPlayer.ActorNumber, (int)this.currentState, this.currentSpeed);
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0008483C File Offset: 0x00082A3C
	public override void ReadDataFusion()
	{
		this.targetPlayer = NetworkSystem.Instance.GetPlayer(this.Data.TargetActorNumber);
		this.currentState = (AngryBeeSwarm.ChaseState)this.Data.CurrentState;
		if (float.IsFinite(this.Data.CurrentSpeed))
		{
			this.currentSpeed = this.Data.CurrentSpeed;
		}
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x000848A4 File Offset: 0x00082AA4
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

	// Token: 0x060005FD RID: 1533 RVA: 0x000848F8 File Offset: 0x00082AF8
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

	// Token: 0x060005FE RID: 1534 RVA: 0x00034776 File Offset: 0x00032976
	public override void OnOwnerChange(Player newOwner, Player previousOwner)
	{
		base.OnOwnerChange(newOwner, previousOwner);
		if (newOwner == PhotonNetwork.LocalPlayer)
		{
			this.OnChangeState(this.currentState);
		}
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x00034794 File Offset: 0x00032994
	public void OnJoinedRoom()
	{
		Debug.Log("Here");
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.InitializeSwarm();
		}
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x000347B2 File Offset: 0x000329B2
	private void TestEmerge()
	{
		this.Emerge(this.testEmergeFrom.transform.position, this.testEmergeTo.transform.position);
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x000347DA File Offset: 0x000329DA
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x000347F2 File Offset: 0x000329F2
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x040006F5 RID: 1781
	public static AngryBeeSwarm instance;

	// Token: 0x040006F6 RID: 1782
	public float heightAboveNavmesh = 0.5f;

	// Token: 0x040006F7 RID: 1783
	public Transform followTarget;

	// Token: 0x040006F8 RID: 1784
	[SerializeField]
	private float velocityStep = 1f;

	// Token: 0x040006F9 RID: 1785
	private float currentSpeed;

	// Token: 0x040006FA RID: 1786
	[SerializeField]
	private float velocityIncreaseInterval = 20f;

	// Token: 0x040006FB RID: 1787
	public Vector3 noisyOffset;

	// Token: 0x040006FC RID: 1788
	public Vector3 ghostOffsetGrabbingLocal;

	// Token: 0x040006FD RID: 1789
	private float emergeStartedTimestamp;

	// Token: 0x040006FE RID: 1790
	private float grabTimestamp;

	// Token: 0x040006FF RID: 1791
	private float lastSpeedIncreased;

	// Token: 0x04000700 RID: 1792
	[SerializeField]
	private float totalTimeToEmerge;

	// Token: 0x04000701 RID: 1793
	[SerializeField]
	private float catchDistance;

	// Token: 0x04000702 RID: 1794
	[SerializeField]
	private float grabDuration;

	// Token: 0x04000703 RID: 1795
	[SerializeField]
	private float grabSpeed = 1f;

	// Token: 0x04000704 RID: 1796
	[SerializeField]
	private float minGrabCooldown;

	// Token: 0x04000705 RID: 1797
	[SerializeField]
	private float initialRangeLimit;

	// Token: 0x04000706 RID: 1798
	[SerializeField]
	private float finalRangeLimit;

	// Token: 0x04000707 RID: 1799
	[SerializeField]
	private float rangeLimitBlendDuration;

	// Token: 0x04000708 RID: 1800
	[SerializeField]
	private float boredAfterDuration;

	// Token: 0x04000709 RID: 1801
	public NetPlayer targetPlayer;

	// Token: 0x0400070A RID: 1802
	public AngryBeeAnimator beeAnimator;

	// Token: 0x0400070B RID: 1803
	public AngryBeeSwarm.ChaseState currentState;

	// Token: 0x0400070C RID: 1804
	public AngryBeeSwarm.ChaseState lastState;

	// Token: 0x0400070D RID: 1805
	public NetPlayer grabbedPlayer;

	// Token: 0x0400070E RID: 1806
	private bool targetIsOnNavMesh;

	// Token: 0x0400070F RID: 1807
	private const float navMeshSampleRange = 5f;

	// Token: 0x04000710 RID: 1808
	[Tooltip("Haptic vibration when chased by lucy")]
	public float hapticStrength = 1f;

	// Token: 0x04000711 RID: 1809
	public float hapticDuration = 1.5f;

	// Token: 0x04000712 RID: 1810
	public float MinHeightAboveWater = 0.5f;

	// Token: 0x04000713 RID: 1811
	public float PlayerMinHeightAboveWater = 0.5f;

	// Token: 0x04000714 RID: 1812
	public float RefreshClosestPlayerInterval = 1f;

	// Token: 0x04000715 RID: 1813
	private float NextRefreshClosestPlayerTimestamp = 1f;

	// Token: 0x04000716 RID: 1814
	private float BoredToDeathAtTimestamp = -1f;

	// Token: 0x04000717 RID: 1815
	[SerializeField]
	private Transform testEmergeFrom;

	// Token: 0x04000718 RID: 1816
	[SerializeField]
	private Transform testEmergeTo;

	// Token: 0x04000719 RID: 1817
	private Vector3 emergeFromPosition;

	// Token: 0x0400071A RID: 1818
	private Vector3 emergeToPosition;

	// Token: 0x0400071B RID: 1819
	private NavMeshPath path;

	// Token: 0x0400071C RID: 1820
	public List<Vector3> pathPoints;

	// Token: 0x0400071D RID: 1821
	public int currentPathPointIdx;

	// Token: 0x0400071E RID: 1822
	private float nextPathTimestamp;

	// Token: 0x0400071F RID: 1823
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 3)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private BeeSwarmData _Data;

	// Token: 0x020000E8 RID: 232
	public enum ChaseState
	{
		// Token: 0x04000721 RID: 1825
		Dormant = 1,
		// Token: 0x04000722 RID: 1826
		InitialEmerge,
		// Token: 0x04000723 RID: 1827
		Chasing = 4,
		// Token: 0x04000724 RID: 1828
		Grabbing = 8
	}
}
