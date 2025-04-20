using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020005BD RID: 1469
[NetworkBehaviourWeaved(5)]
public class HalloweenGhostChaser : NetworkComponent
{
	// Token: 0x06002481 RID: 9345 RVA: 0x00048C39 File Offset: 0x00046E39
	protected override void Awake()
	{
		base.Awake();
		this.spawnIndex = 0;
		this.targetPlayer = null;
		this.currentState = HalloweenGhostChaser.ChaseState.Dormant;
		this.grabTime = -this.minGrabCooldown;
		this.possibleTarget = new List<NetPlayer>();
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x00048C6E File Offset: 0x00046E6E
	private new void Start()
	{
		NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
	}

	// Token: 0x06002483 RID: 9347 RVA: 0x00102550 File Offset: 0x00100750
	private void InitializeGhost()
	{
		if (NetworkSystem.Instance.InRoom && base.IsMine)
		{
			this.lastHeadAngleTime = 0f;
			this.nextHeadAngleTime = this.lastHeadAngleTime + UnityEngine.Random.value * this.maxTimeToNextHeadAngle;
			this.nextTimeToChasePlayer = Time.time + UnityEngine.Random.Range(this.minGrabCooldown, this.maxNextTimeToChasePlayer);
			this.ghostBody.transform.localPosition = Vector3.zero;
			base.transform.eulerAngles = Vector3.zero;
			this.lastSpeedIncreased = 0f;
			this.currentSpeed = 0f;
		}
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x001025F0 File Offset: 0x001007F0
	private void LateUpdate()
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			this.currentState = HalloweenGhostChaser.ChaseState.Dormant;
			this.UpdateState();
			return;
		}
		if (base.IsMine)
		{
			HalloweenGhostChaser.ChaseState chaseState = this.currentState;
			switch (chaseState)
			{
			case HalloweenGhostChaser.ChaseState.Dormant:
				if (Time.time >= this.nextTimeToChasePlayer)
				{
					this.currentState = HalloweenGhostChaser.ChaseState.InitialRise;
				}
				if (Time.time >= this.lastSummonCheck + this.summoningDuration)
				{
					this.lastSummonCheck = Time.time;
					this.possibleTarget.Clear();
					int num = 0;
					int i = 0;
					while (i < this.spawnTransforms.Length)
					{
						int num2 = 0;
						for (int j = 0; j < GorillaParent.instance.vrrigs.Count; j++)
						{
							if ((GorillaParent.instance.vrrigs[j].transform.position - this.spawnTransforms[i].position).magnitude < this.summonDistance)
							{
								this.possibleTarget.Add(GorillaParent.instance.vrrigs[j].creator);
								num2++;
								if (num2 >= this.summonCount)
								{
									break;
								}
							}
						}
						if (num2 >= this.summonCount)
						{
							if (!this.wasSurroundedLastCheck)
							{
								this.wasSurroundedLastCheck = true;
								break;
							}
							this.wasSurroundedLastCheck = false;
							this.isSummoned = true;
							this.currentState = HalloweenGhostChaser.ChaseState.Gong;
							break;
						}
						else
						{
							num++;
							i++;
						}
					}
					if (num == this.spawnTransforms.Length)
					{
						this.wasSurroundedLastCheck = false;
					}
				}
				break;
			case HalloweenGhostChaser.ChaseState.InitialRise:
				if (Time.time > this.timeRiseStarted + this.totalTimeToRise)
				{
					this.currentState = HalloweenGhostChaser.ChaseState.Chasing;
				}
				break;
			case (HalloweenGhostChaser.ChaseState)3:
				break;
			case HalloweenGhostChaser.ChaseState.Gong:
				if (Time.time > this.timeGongStarted + this.gongDuration)
				{
					this.currentState = HalloweenGhostChaser.ChaseState.InitialRise;
				}
				break;
			default:
				if (chaseState != HalloweenGhostChaser.ChaseState.Chasing)
				{
					if (chaseState == HalloweenGhostChaser.ChaseState.Grabbing)
					{
						if (Time.time > this.grabTime + this.grabDuration)
						{
							this.currentState = HalloweenGhostChaser.ChaseState.Dormant;
						}
					}
				}
				else
				{
					if (this.followTarget == null || this.targetPlayer == null)
					{
						this.ChooseRandomTarget();
					}
					if (!(this.followTarget == null) && (this.followTarget.position - this.ghostBody.transform.position).magnitude < this.catchDistance)
					{
						this.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
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

	// Token: 0x06002485 RID: 9349 RVA: 0x00102888 File Offset: 0x00100A88
	public void UpdateState()
	{
		HalloweenGhostChaser.ChaseState chaseState = this.currentState;
		switch (chaseState)
		{
		case HalloweenGhostChaser.ChaseState.Dormant:
			this.isSummoned = false;
			if (this.ghostMaterial.color == this.summonedColor)
			{
				this.ghostMaterial.color = this.defaultColor;
				return;
			}
			break;
		case HalloweenGhostChaser.ChaseState.InitialRise:
			if (NetworkSystem.Instance.InRoom)
			{
				if (base.IsMine)
				{
					this.RiseHost();
				}
				this.MoveHead();
				return;
			}
			break;
		case (HalloweenGhostChaser.ChaseState)3:
		case HalloweenGhostChaser.ChaseState.Gong:
			break;
		default:
			if (chaseState != HalloweenGhostChaser.ChaseState.Chasing)
			{
				if (chaseState != HalloweenGhostChaser.ChaseState.Grabbing)
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
					this.MoveHead();
				}
			}
			else if (NetworkSystem.Instance.InRoom)
			{
				if (base.IsMine)
				{
					this.ChaseHost();
				}
				this.MoveBodyShared();
				this.MoveHead();
				return;
			}
			break;
		}
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x0010296C File Offset: 0x00100B6C
	private void OnChangeState(HalloweenGhostChaser.ChaseState newState)
	{
		switch (newState)
		{
		case HalloweenGhostChaser.ChaseState.Dormant:
			if (this.ghostBody.activeSelf)
			{
				this.ghostBody.SetActive(false);
			}
			if (base.IsMine)
			{
				this.targetPlayer = null;
				this.InitializeGhost();
			}
			else
			{
				this.nextTimeToChasePlayer = Time.time + UnityEngine.Random.Range(this.minGrabCooldown, this.maxNextTimeToChasePlayer);
			}
			this.SetInitialRotations();
			return;
		case HalloweenGhostChaser.ChaseState.InitialRise:
			this.timeRiseStarted = Time.time;
			if (!this.ghostBody.activeSelf)
			{
				this.ghostBody.SetActive(true);
			}
			if (base.IsMine)
			{
				if (!this.isSummoned)
				{
					this.currentSpeed = 0f;
					this.ChooseRandomTarget();
					this.SetInitialSpawnPoint();
				}
				else
				{
					this.currentSpeed = 3f;
				}
			}
			if (this.isSummoned)
			{
				this.laugh.volume = 0.25f;
				this.laugh.GTPlayOneShot(this.deepLaugh, 1f);
				this.ghostMaterial.color = this.summonedColor;
			}
			else
			{
				this.laugh.volume = 0.25f;
				this.laugh.GTPlay();
				this.ghostMaterial.color = this.defaultColor;
			}
			this.SetInitialRotations();
			return;
		case (HalloweenGhostChaser.ChaseState)3:
			break;
		case HalloweenGhostChaser.ChaseState.Gong:
			if (!this.ghostBody.activeSelf)
			{
				this.ghostBody.SetActive(true);
			}
			if (base.IsMine)
			{
				this.ChooseRandomTarget();
				this.SetInitialSpawnPoint();
				base.transform.position = this.spawnTransforms[this.spawnIndex].position;
			}
			this.timeGongStarted = Time.time;
			this.laugh.volume = 1f;
			this.laugh.GTPlayOneShot(this.gong, 1f);
			this.isSummoned = true;
			return;
		default:
			if (newState != HalloweenGhostChaser.ChaseState.Chasing)
			{
				if (newState != HalloweenGhostChaser.ChaseState.Grabbing)
				{
					return;
				}
				if (!this.ghostBody.activeSelf)
				{
					this.ghostBody.SetActive(true);
				}
				this.grabTime = Time.time;
				if (this.isSummoned)
				{
					this.laugh.volume = 0.25f;
					this.laugh.GTPlayOneShot(this.deepLaugh, 1f);
				}
				else
				{
					this.laugh.volume = 0.25f;
					this.laugh.GTPlay();
				}
				this.leftArm.localEulerAngles = this.leftArmGrabbingLocal;
				this.rightArm.localEulerAngles = this.rightArmGrabbingLocal;
				this.leftHand.localEulerAngles = this.leftHandGrabbingLocal;
				this.rightHand.localEulerAngles = this.rightHandGrabbingLocal;
				this.ghostBody.transform.localPosition = this.ghostOffsetGrabbingLocal;
				this.ghostBody.transform.localEulerAngles = this.ghostGrabbingEulerRotation;
				VRRig vrrig = GorillaGameManager.StaticFindRigForPlayer(this.targetPlayer);
				if (vrrig != null)
				{
					this.followTarget = vrrig.transform;
					return;
				}
			}
			else
			{
				if (!this.ghostBody.activeSelf)
				{
					this.ghostBody.SetActive(true);
				}
				this.ResetPath();
			}
			break;
		}
	}

	// Token: 0x06002487 RID: 9351 RVA: 0x00102C64 File Offset: 0x00100E64
	private void SetInitialSpawnPoint()
	{
		float num = 1000f;
		this.spawnIndex = 0;
		if (this.followTarget == null)
		{
			return;
		}
		for (int i = 0; i < this.spawnTransforms.Length; i++)
		{
			float magnitude = (this.followTarget.position - this.spawnTransformOffsets[i].position).magnitude;
			if (magnitude < num)
			{
				num = magnitude;
				this.spawnIndex = i;
			}
		}
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x00102CD4 File Offset: 0x00100ED4
	private void ChooseRandomTarget()
	{
		int num = -1;
		if (this.possibleTarget.Count >= this.summonCount)
		{
			int randomTarget = UnityEngine.Random.Range(0, this.possibleTarget.Count);
			num = GorillaParent.instance.vrrigs.FindIndex((VRRig x) => x.creator != null && x.creator == this.possibleTarget[randomTarget]);
			this.currentSpeed = 3f;
		}
		if (num == -1)
		{
			num = UnityEngine.Random.Range(0, GorillaParent.instance.vrrigs.Count);
		}
		this.possibleTarget.Clear();
		if (num < GorillaParent.instance.vrrigs.Count)
		{
			this.targetPlayer = GorillaParent.instance.vrrigs[num].creator;
			this.followTarget = GorillaParent.instance.vrrigs[num].head.rigTarget;
			NavMeshHit navMeshHit;
			this.targetIsOnNavMesh = NavMesh.SamplePosition(this.followTarget.position, out navMeshHit, 5f, 1);
			return;
		}
		this.targetPlayer = null;
		this.followTarget = null;
	}

	// Token: 0x06002489 RID: 9353 RVA: 0x00102DEC File Offset: 0x00100FEC
	private void SetInitialRotations()
	{
		this.leftArm.localEulerAngles = Vector3.zero;
		this.rightArm.localEulerAngles = Vector3.zero;
		this.leftHand.localEulerAngles = this.leftHandStartingLocal;
		this.rightHand.localEulerAngles = this.rightHandStartingLocal;
		this.ghostBody.transform.localPosition = Vector3.zero;
		this.ghostBody.transform.localEulerAngles = this.ghostStartingEulerRotation;
	}

	// Token: 0x0600248A RID: 9354 RVA: 0x00102E68 File Offset: 0x00101068
	private void MoveHead()
	{
		if (Time.time > this.nextHeadAngleTime)
		{
			this.skullTransform.localEulerAngles = this.headEulerAngles[UnityEngine.Random.Range(0, this.headEulerAngles.Length)];
			this.lastHeadAngleTime = Time.time;
			this.nextHeadAngleTime = this.lastHeadAngleTime + Mathf.Max(UnityEngine.Random.value * this.maxTimeToNextHeadAngle, 0.05f);
		}
	}

	// Token: 0x0600248B RID: 9355 RVA: 0x00102ED4 File Offset: 0x001010D4
	private void RiseHost()
	{
		if (Time.time < this.timeRiseStarted + this.totalTimeToRise)
		{
			if (this.spawnIndex == -1)
			{
				this.spawnIndex = 0;
			}
			base.transform.position = this.spawnTransforms[this.spawnIndex].position + Vector3.up * (Time.time - this.timeRiseStarted) / this.totalTimeToRise * this.riseDistance;
			base.transform.rotation = this.spawnTransforms[this.spawnIndex].rotation;
		}
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x00102F70 File Offset: 0x00101170
	private void RiseGrabbedLocalPlayer()
	{
		if (Time.time > this.grabTime + this.minGrabCooldown)
		{
			this.grabTime = Time.time;
			GorillaTagger.Instance.ApplyStatusEffect(GorillaTagger.StatusEffect.Frozen, GorillaTagger.Instance.tagCooldown);
			GorillaTagger.Instance.StartVibration(true, this.hapticStrength, this.hapticDuration);
			GorillaTagger.Instance.StartVibration(false, this.hapticStrength, this.hapticDuration);
		}
		if (Time.time < this.grabTime + this.grabDuration)
		{
			GorillaTagger.Instance.rigidbody.velocity = Vector3.up * this.grabSpeed;
			EquipmentInteractor.instance.ForceStopClimbing();
		}
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x00103020 File Offset: 0x00101220
	public void UpdateFollowPath(Vector3 destination, float currentSpeed)
	{
		if (this.path == null)
		{
			this.GetNewPath(destination);
		}
		this.points[this.points.Count - 1] = destination;
		Vector3 vector = this.points[this.currentTargetIdx];
		base.transform.position = Vector3.MoveTowards(base.transform.position, vector, currentSpeed * Time.deltaTime);
		Vector3 eulerAngles = Quaternion.LookRotation(vector - base.transform.position).eulerAngles;
		if (Mathf.Abs(eulerAngles.x) > 45f)
		{
			eulerAngles.x = 0f;
		}
		base.transform.rotation = Quaternion.Euler(eulerAngles);
		if (this.currentTargetIdx + 1 < this.points.Count && (base.transform.position - vector).sqrMagnitude < 0.1f)
		{
			if (this.nextPathTimestamp <= Time.time)
			{
				this.GetNewPath(destination);
				return;
			}
			this.currentTargetIdx++;
		}
	}

	// Token: 0x0600248E RID: 9358 RVA: 0x00103130 File Offset: 0x00101330
	private void GetNewPath(Vector3 destination)
	{
		this.path = new NavMeshPath();
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(base.transform.position, out navMeshHit, 5f, 1);
		NavMeshHit navMeshHit2;
		this.targetIsOnNavMesh = NavMesh.SamplePosition(destination, out navMeshHit2, 5f, 1);
		NavMesh.CalculatePath(navMeshHit.position, navMeshHit2.position, -1, this.path);
		this.points = new List<Vector3>();
		foreach (Vector3 a in this.path.corners)
		{
			this.points.Add(a + Vector3.up * this.heightAboveNavmesh);
		}
		this.points.Add(destination);
		this.currentTargetIdx = 0;
		this.nextPathTimestamp = Time.time + 2f;
	}

	// Token: 0x0600248F RID: 9359 RVA: 0x00048CA0 File Offset: 0x00046EA0
	public void ResetPath()
	{
		this.path = null;
	}

	// Token: 0x06002490 RID: 9360 RVA: 0x00103204 File Offset: 0x00101404
	private void ChaseHost()
	{
		if (this.followTarget != null)
		{
			if (Time.time > this.lastSpeedIncreased + this.velocityIncreaseTime)
			{
				this.lastSpeedIncreased = Time.time;
				this.currentSpeed += this.velocityStep;
			}
			if (this.targetIsOnNavMesh)
			{
				this.UpdateFollowPath(this.followTarget.position, this.currentSpeed);
				return;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.followTarget.position, this.currentSpeed * Time.deltaTime);
			base.transform.rotation = Quaternion.LookRotation(this.followTarget.position - base.transform.position, Vector3.up);
		}
	}

	// Token: 0x06002491 RID: 9361 RVA: 0x001032D8 File Offset: 0x001014D8
	private void MoveBodyShared()
	{
		this.noisyOffset = new Vector3(Mathf.PerlinNoise(Time.time, 0f) - 0.5f, Mathf.PerlinNoise(Time.time, 10f) - 0.5f, Mathf.PerlinNoise(Time.time, 20f) - 0.5f);
		this.childGhost.localPosition = this.noisyOffset;
		this.leftArm.localEulerAngles = this.noisyOffset * 20f;
		this.rightArm.localEulerAngles = this.noisyOffset * -20f;
	}

	// Token: 0x06002492 RID: 9362 RVA: 0x00048CA9 File Offset: 0x00046EA9
	private void GrabBodyShared()
	{
		if (this.followTarget != null)
		{
			base.transform.rotation = this.followTarget.rotation;
			base.transform.position = this.followTarget.position;
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06002493 RID: 9363 RVA: 0x00048CE5 File Offset: 0x00046EE5
	// (set) Token: 0x06002494 RID: 9364 RVA: 0x00048D0F File Offset: 0x00046F0F
	[Networked]
	[NetworkedWeaved(0, 5)]
	public unsafe HalloweenGhostChaser.GhostData Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing HalloweenGhostChaser.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(HalloweenGhostChaser.GhostData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing HalloweenGhostChaser.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(HalloweenGhostChaser.GhostData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06002495 RID: 9365 RVA: 0x00103378 File Offset: 0x00101578
	public override void WriteDataFusion()
	{
		HalloweenGhostChaser.GhostData data = default(HalloweenGhostChaser.GhostData);
		NetPlayer netPlayer = this.targetPlayer;
		data.TargetActorNumber = ((netPlayer != null) ? netPlayer.ActorNumber : -1);
		data.CurrentState = (int)this.currentState;
		data.SpawnIndex = this.spawnIndex;
		data.CurrentSpeed = this.currentSpeed;
		data.IsSummoned = this.isSummoned;
		this.Data = data;
	}

	// Token: 0x06002496 RID: 9366 RVA: 0x001033E8 File Offset: 0x001015E8
	public override void ReadDataFusion()
	{
		int targetActorNumber = this.Data.TargetActorNumber;
		this.targetPlayer = NetworkSystem.Instance.GetPlayer(targetActorNumber);
		this.currentState = (HalloweenGhostChaser.ChaseState)this.Data.CurrentState;
		this.spawnIndex = this.Data.SpawnIndex;
		float f = this.Data.CurrentSpeed;
		this.isSummoned = this.Data.IsSummoned;
		if (float.IsFinite(f))
		{
			this.currentSpeed = f;
		}
	}

	// Token: 0x06002497 RID: 9367 RVA: 0x00103468 File Offset: 0x00101668
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (NetworkSystem.Instance.GetPlayer(info.Sender) != NetworkSystem.Instance.MasterClient)
		{
			return;
		}
		if (this.targetPlayer == null)
		{
			stream.SendNext(-1);
		}
		else
		{
			stream.SendNext(this.targetPlayer.ActorNumber);
		}
		stream.SendNext(this.currentState);
		stream.SendNext(this.spawnIndex);
		stream.SendNext(this.currentSpeed);
		stream.SendNext(this.isSummoned);
	}

	// Token: 0x06002498 RID: 9368 RVA: 0x00103504 File Offset: 0x00101704
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (NetworkSystem.Instance.GetPlayer(info.Sender) != NetworkSystem.Instance.MasterClient)
		{
			return;
		}
		int playerID = (int)stream.ReceiveNext();
		this.targetPlayer = NetworkSystem.Instance.GetPlayer(playerID);
		this.currentState = (HalloweenGhostChaser.ChaseState)stream.ReceiveNext();
		this.spawnIndex = (int)stream.ReceiveNext();
		float f = (float)stream.ReceiveNext();
		this.isSummoned = (bool)stream.ReceiveNext();
		if (float.IsFinite(f))
		{
			this.currentSpeed = f;
		}
	}

	// Token: 0x06002499 RID: 9369 RVA: 0x00048D3A File Offset: 0x00046F3A
	public override void OnOwnerChange(Player newOwner, Player previousOwner)
	{
		base.OnOwnerChange(newOwner, previousOwner);
		if (newOwner == PhotonNetwork.LocalPlayer)
		{
			this.OnChangeState(this.currentState);
		}
	}

	// Token: 0x0600249A RID: 9370 RVA: 0x00048D58 File Offset: 0x00046F58
	public void OnJoinedRoom()
	{
		Debug.Log("Here");
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.InitializeGhost();
			return;
		}
		this.nextTimeToChasePlayer = Time.time + UnityEngine.Random.Range(this.minGrabCooldown, this.maxNextTimeToChasePlayer);
	}

	// Token: 0x0600249C RID: 9372 RVA: 0x00048D94 File Offset: 0x00046F94
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x0600249D RID: 9373 RVA: 0x00048DAC File Offset: 0x00046FAC
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04002862 RID: 10338
	public float heightAboveNavmesh = 0.5f;

	// Token: 0x04002863 RID: 10339
	public Transform followTarget;

	// Token: 0x04002864 RID: 10340
	public Transform childGhost;

	// Token: 0x04002865 RID: 10341
	public float velocityStep = 1f;

	// Token: 0x04002866 RID: 10342
	public float currentSpeed;

	// Token: 0x04002867 RID: 10343
	public float velocityIncreaseTime = 20f;

	// Token: 0x04002868 RID: 10344
	public float riseDistance = 2f;

	// Token: 0x04002869 RID: 10345
	public float summonDistance = 5f;

	// Token: 0x0400286A RID: 10346
	public float timeEncircled;

	// Token: 0x0400286B RID: 10347
	public float lastSummonCheck;

	// Token: 0x0400286C RID: 10348
	public float timeGongStarted;

	// Token: 0x0400286D RID: 10349
	public float summoningDuration = 30f;

	// Token: 0x0400286E RID: 10350
	public float summoningCheckCountdown = 5f;

	// Token: 0x0400286F RID: 10351
	public float gongDuration = 5f;

	// Token: 0x04002870 RID: 10352
	public int summonCount = 5;

	// Token: 0x04002871 RID: 10353
	public bool wasSurroundedLastCheck;

	// Token: 0x04002872 RID: 10354
	public AudioSource laugh;

	// Token: 0x04002873 RID: 10355
	public List<NetPlayer> possibleTarget;

	// Token: 0x04002874 RID: 10356
	public AudioClip defaultLaugh;

	// Token: 0x04002875 RID: 10357
	public AudioClip deepLaugh;

	// Token: 0x04002876 RID: 10358
	public AudioClip gong;

	// Token: 0x04002877 RID: 10359
	public Vector3 noisyOffset;

	// Token: 0x04002878 RID: 10360
	public Vector3 leftArmGrabbingLocal;

	// Token: 0x04002879 RID: 10361
	public Vector3 rightArmGrabbingLocal;

	// Token: 0x0400287A RID: 10362
	public Vector3 leftHandGrabbingLocal;

	// Token: 0x0400287B RID: 10363
	public Vector3 rightHandGrabbingLocal;

	// Token: 0x0400287C RID: 10364
	public Vector3 leftHandStartingLocal;

	// Token: 0x0400287D RID: 10365
	public Vector3 rightHandStartingLocal;

	// Token: 0x0400287E RID: 10366
	public Vector3 ghostOffsetGrabbingLocal;

	// Token: 0x0400287F RID: 10367
	public Vector3 ghostStartingEulerRotation;

	// Token: 0x04002880 RID: 10368
	public Vector3 ghostGrabbingEulerRotation;

	// Token: 0x04002881 RID: 10369
	public float maxTimeToNextHeadAngle;

	// Token: 0x04002882 RID: 10370
	public float lastHeadAngleTime;

	// Token: 0x04002883 RID: 10371
	public float nextHeadAngleTime;

	// Token: 0x04002884 RID: 10372
	public float nextTimeToChasePlayer;

	// Token: 0x04002885 RID: 10373
	public float maxNextTimeToChasePlayer;

	// Token: 0x04002886 RID: 10374
	public float timeRiseStarted;

	// Token: 0x04002887 RID: 10375
	public float totalTimeToRise;

	// Token: 0x04002888 RID: 10376
	public float catchDistance;

	// Token: 0x04002889 RID: 10377
	public float grabTime;

	// Token: 0x0400288A RID: 10378
	public float grabDuration;

	// Token: 0x0400288B RID: 10379
	public float grabSpeed = 1f;

	// Token: 0x0400288C RID: 10380
	public float minGrabCooldown;

	// Token: 0x0400288D RID: 10381
	public float lastSpeedIncreased;

	// Token: 0x0400288E RID: 10382
	public Vector3[] headEulerAngles;

	// Token: 0x0400288F RID: 10383
	public Transform skullTransform;

	// Token: 0x04002890 RID: 10384
	public Transform leftArm;

	// Token: 0x04002891 RID: 10385
	public Transform rightArm;

	// Token: 0x04002892 RID: 10386
	public Transform leftHand;

	// Token: 0x04002893 RID: 10387
	public Transform rightHand;

	// Token: 0x04002894 RID: 10388
	public Transform[] spawnTransforms;

	// Token: 0x04002895 RID: 10389
	public Transform[] spawnTransformOffsets;

	// Token: 0x04002896 RID: 10390
	public NetPlayer targetPlayer;

	// Token: 0x04002897 RID: 10391
	public GameObject ghostBody;

	// Token: 0x04002898 RID: 10392
	public HalloweenGhostChaser.ChaseState currentState;

	// Token: 0x04002899 RID: 10393
	public HalloweenGhostChaser.ChaseState lastState;

	// Token: 0x0400289A RID: 10394
	public int spawnIndex;

	// Token: 0x0400289B RID: 10395
	public NetPlayer grabbedPlayer;

	// Token: 0x0400289C RID: 10396
	public Material ghostMaterial;

	// Token: 0x0400289D RID: 10397
	public Color defaultColor;

	// Token: 0x0400289E RID: 10398
	public Color summonedColor;

	// Token: 0x0400289F RID: 10399
	public bool isSummoned;

	// Token: 0x040028A0 RID: 10400
	private bool targetIsOnNavMesh;

	// Token: 0x040028A1 RID: 10401
	private const float navMeshSampleRange = 5f;

	// Token: 0x040028A2 RID: 10402
	[Tooltip("Haptic vibration when chased by lucy")]
	public float hapticStrength = 1f;

	// Token: 0x040028A3 RID: 10403
	public float hapticDuration = 1.5f;

	// Token: 0x040028A4 RID: 10404
	private NavMeshPath path;

	// Token: 0x040028A5 RID: 10405
	public List<Vector3> points;

	// Token: 0x040028A6 RID: 10406
	public int currentTargetIdx;

	// Token: 0x040028A7 RID: 10407
	private float nextPathTimestamp;

	// Token: 0x040028A8 RID: 10408
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 5)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private HalloweenGhostChaser.GhostData _Data;

	// Token: 0x020005BE RID: 1470
	public enum ChaseState
	{
		// Token: 0x040028AA RID: 10410
		Dormant = 1,
		// Token: 0x040028AB RID: 10411
		InitialRise,
		// Token: 0x040028AC RID: 10412
		Gong = 4,
		// Token: 0x040028AD RID: 10413
		Chasing = 8,
		// Token: 0x040028AE RID: 10414
		Grabbing = 16
	}

	// Token: 0x020005BF RID: 1471
	[NetworkStructWeaved(5)]
	[StructLayout(LayoutKind.Explicit, Size = 20)]
	public struct GhostData : INetworkStruct
	{
		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x0600249E RID: 9374 RVA: 0x00048DC0 File Offset: 0x00046FC0
		// (set) Token: 0x0600249F RID: 9375 RVA: 0x00048DCE File Offset: 0x00046FCE
		[Networked]
		public unsafe float CurrentSpeed
		{
			readonly get
			{
				return *(float*)Native.ReferenceToPointer<FixedStorage@1>(ref this._CurrentSpeed);
			}
			set
			{
				*(float*)Native.ReferenceToPointer<FixedStorage@1>(ref this._CurrentSpeed) = value;
			}
		}

		// Token: 0x040028AF RID: 10415
		[FieldOffset(0)]
		public int TargetActorNumber;

		// Token: 0x040028B0 RID: 10416
		[FieldOffset(4)]
		public int CurrentState;

		// Token: 0x040028B1 RID: 10417
		[FieldOffset(8)]
		public int SpawnIndex;

		// Token: 0x040028B2 RID: 10418
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate@ReaderWriter@System_Single), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(12)]
		private FixedStorage@1 _CurrentSpeed;

		// Token: 0x040028B3 RID: 10419
		[FieldOffset(16)]
		public NetworkBool IsSummoned;
	}
}
