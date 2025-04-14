using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020005B0 RID: 1456
[NetworkBehaviourWeaved(5)]
public class HalloweenGhostChaser : NetworkComponent
{
	// Token: 0x06002429 RID: 9257 RVA: 0x000B3EBF File Offset: 0x000B20BF
	protected override void Awake()
	{
		base.Awake();
		this.spawnIndex = 0;
		this.targetPlayer = null;
		this.currentState = HalloweenGhostChaser.ChaseState.Dormant;
		this.grabTime = -this.minGrabCooldown;
		this.possibleTarget = new List<NetPlayer>();
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x000B3EF4 File Offset: 0x000B20F4
	private new void Start()
	{
		NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
	}

	// Token: 0x0600242B RID: 9259 RVA: 0x000B3F28 File Offset: 0x000B2128
	private void InitializeGhost()
	{
		if (NetworkSystem.Instance.InRoom && base.IsMine)
		{
			this.lastHeadAngleTime = 0f;
			this.nextHeadAngleTime = this.lastHeadAngleTime + Random.value * this.maxTimeToNextHeadAngle;
			this.nextTimeToChasePlayer = Time.time + Random.Range(this.minGrabCooldown, this.maxNextTimeToChasePlayer);
			this.ghostBody.transform.localPosition = Vector3.zero;
			base.transform.eulerAngles = Vector3.zero;
			this.lastSpeedIncreased = 0f;
			this.currentSpeed = 0f;
		}
	}

	// Token: 0x0600242C RID: 9260 RVA: 0x000B3FC8 File Offset: 0x000B21C8
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

	// Token: 0x0600242D RID: 9261 RVA: 0x000B4260 File Offset: 0x000B2460
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

	// Token: 0x0600242E RID: 9262 RVA: 0x000B4344 File Offset: 0x000B2544
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
				this.nextTimeToChasePlayer = Time.time + Random.Range(this.minGrabCooldown, this.maxNextTimeToChasePlayer);
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

	// Token: 0x0600242F RID: 9263 RVA: 0x000B463C File Offset: 0x000B283C
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

	// Token: 0x06002430 RID: 9264 RVA: 0x000B46AC File Offset: 0x000B28AC
	private void ChooseRandomTarget()
	{
		int num = -1;
		if (this.possibleTarget.Count >= this.summonCount)
		{
			int randomTarget = Random.Range(0, this.possibleTarget.Count);
			num = GorillaParent.instance.vrrigs.FindIndex((VRRig x) => x.creator != null && x.creator == this.possibleTarget[randomTarget]);
			this.currentSpeed = 3f;
		}
		if (num == -1)
		{
			num = Random.Range(0, GorillaParent.instance.vrrigs.Count);
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

	// Token: 0x06002431 RID: 9265 RVA: 0x000B47C4 File Offset: 0x000B29C4
	private void SetInitialRotations()
	{
		this.leftArm.localEulerAngles = Vector3.zero;
		this.rightArm.localEulerAngles = Vector3.zero;
		this.leftHand.localEulerAngles = this.leftHandStartingLocal;
		this.rightHand.localEulerAngles = this.rightHandStartingLocal;
		this.ghostBody.transform.localPosition = Vector3.zero;
		this.ghostBody.transform.localEulerAngles = this.ghostStartingEulerRotation;
	}

	// Token: 0x06002432 RID: 9266 RVA: 0x000B4840 File Offset: 0x000B2A40
	private void MoveHead()
	{
		if (Time.time > this.nextHeadAngleTime)
		{
			this.skullTransform.localEulerAngles = this.headEulerAngles[Random.Range(0, this.headEulerAngles.Length)];
			this.lastHeadAngleTime = Time.time;
			this.nextHeadAngleTime = this.lastHeadAngleTime + Mathf.Max(Random.value * this.maxTimeToNextHeadAngle, 0.05f);
		}
	}

	// Token: 0x06002433 RID: 9267 RVA: 0x000B48AC File Offset: 0x000B2AAC
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

	// Token: 0x06002434 RID: 9268 RVA: 0x000B4948 File Offset: 0x000B2B48
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

	// Token: 0x06002435 RID: 9269 RVA: 0x000B49F8 File Offset: 0x000B2BF8
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

	// Token: 0x06002436 RID: 9270 RVA: 0x000B4B08 File Offset: 0x000B2D08
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

	// Token: 0x06002437 RID: 9271 RVA: 0x000B4BDC File Offset: 0x000B2DDC
	public void ResetPath()
	{
		this.path = null;
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x000B4BE8 File Offset: 0x000B2DE8
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

	// Token: 0x06002439 RID: 9273 RVA: 0x000B4CBC File Offset: 0x000B2EBC
	private void MoveBodyShared()
	{
		this.noisyOffset = new Vector3(Mathf.PerlinNoise(Time.time, 0f) - 0.5f, Mathf.PerlinNoise(Time.time, 10f) - 0.5f, Mathf.PerlinNoise(Time.time, 20f) - 0.5f);
		this.childGhost.localPosition = this.noisyOffset;
		this.leftArm.localEulerAngles = this.noisyOffset * 20f;
		this.rightArm.localEulerAngles = this.noisyOffset * -20f;
	}

	// Token: 0x0600243A RID: 9274 RVA: 0x000B4D5A File Offset: 0x000B2F5A
	private void GrabBodyShared()
	{
		if (this.followTarget != null)
		{
			base.transform.rotation = this.followTarget.rotation;
			base.transform.position = this.followTarget.position;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x0600243B RID: 9275 RVA: 0x000B4D96 File Offset: 0x000B2F96
	// (set) Token: 0x0600243C RID: 9276 RVA: 0x000B4DC0 File Offset: 0x000B2FC0
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

	// Token: 0x0600243D RID: 9277 RVA: 0x000B4DEC File Offset: 0x000B2FEC
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

	// Token: 0x0600243E RID: 9278 RVA: 0x000B4E5C File Offset: 0x000B305C
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

	// Token: 0x0600243F RID: 9279 RVA: 0x000B4EDC File Offset: 0x000B30DC
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

	// Token: 0x06002440 RID: 9280 RVA: 0x000B4F78 File Offset: 0x000B3178
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

	// Token: 0x06002441 RID: 9281 RVA: 0x000B500D File Offset: 0x000B320D
	public override void OnOwnerChange(Player newOwner, Player previousOwner)
	{
		base.OnOwnerChange(newOwner, previousOwner);
		if (newOwner == PhotonNetwork.LocalPlayer)
		{
			this.OnChangeState(this.currentState);
		}
	}

	// Token: 0x06002442 RID: 9282 RVA: 0x000B502B File Offset: 0x000B322B
	public void OnJoinedRoom()
	{
		Debug.Log("Here");
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.InitializeGhost();
			return;
		}
		this.nextTimeToChasePlayer = Time.time + Random.Range(this.minGrabCooldown, this.maxNextTimeToChasePlayer);
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x000B50FB File Offset: 0x000B32FB
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06002445 RID: 9285 RVA: 0x000B5113 File Offset: 0x000B3313
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x0400280C RID: 10252
	public float heightAboveNavmesh = 0.5f;

	// Token: 0x0400280D RID: 10253
	public Transform followTarget;

	// Token: 0x0400280E RID: 10254
	public Transform childGhost;

	// Token: 0x0400280F RID: 10255
	public float velocityStep = 1f;

	// Token: 0x04002810 RID: 10256
	public float currentSpeed;

	// Token: 0x04002811 RID: 10257
	public float velocityIncreaseTime = 20f;

	// Token: 0x04002812 RID: 10258
	public float riseDistance = 2f;

	// Token: 0x04002813 RID: 10259
	public float summonDistance = 5f;

	// Token: 0x04002814 RID: 10260
	public float timeEncircled;

	// Token: 0x04002815 RID: 10261
	public float lastSummonCheck;

	// Token: 0x04002816 RID: 10262
	public float timeGongStarted;

	// Token: 0x04002817 RID: 10263
	public float summoningDuration = 30f;

	// Token: 0x04002818 RID: 10264
	public float summoningCheckCountdown = 5f;

	// Token: 0x04002819 RID: 10265
	public float gongDuration = 5f;

	// Token: 0x0400281A RID: 10266
	public int summonCount = 5;

	// Token: 0x0400281B RID: 10267
	public bool wasSurroundedLastCheck;

	// Token: 0x0400281C RID: 10268
	public AudioSource laugh;

	// Token: 0x0400281D RID: 10269
	public List<NetPlayer> possibleTarget;

	// Token: 0x0400281E RID: 10270
	public AudioClip defaultLaugh;

	// Token: 0x0400281F RID: 10271
	public AudioClip deepLaugh;

	// Token: 0x04002820 RID: 10272
	public AudioClip gong;

	// Token: 0x04002821 RID: 10273
	public Vector3 noisyOffset;

	// Token: 0x04002822 RID: 10274
	public Vector3 leftArmGrabbingLocal;

	// Token: 0x04002823 RID: 10275
	public Vector3 rightArmGrabbingLocal;

	// Token: 0x04002824 RID: 10276
	public Vector3 leftHandGrabbingLocal;

	// Token: 0x04002825 RID: 10277
	public Vector3 rightHandGrabbingLocal;

	// Token: 0x04002826 RID: 10278
	public Vector3 leftHandStartingLocal;

	// Token: 0x04002827 RID: 10279
	public Vector3 rightHandStartingLocal;

	// Token: 0x04002828 RID: 10280
	public Vector3 ghostOffsetGrabbingLocal;

	// Token: 0x04002829 RID: 10281
	public Vector3 ghostStartingEulerRotation;

	// Token: 0x0400282A RID: 10282
	public Vector3 ghostGrabbingEulerRotation;

	// Token: 0x0400282B RID: 10283
	public float maxTimeToNextHeadAngle;

	// Token: 0x0400282C RID: 10284
	public float lastHeadAngleTime;

	// Token: 0x0400282D RID: 10285
	public float nextHeadAngleTime;

	// Token: 0x0400282E RID: 10286
	public float nextTimeToChasePlayer;

	// Token: 0x0400282F RID: 10287
	public float maxNextTimeToChasePlayer;

	// Token: 0x04002830 RID: 10288
	public float timeRiseStarted;

	// Token: 0x04002831 RID: 10289
	public float totalTimeToRise;

	// Token: 0x04002832 RID: 10290
	public float catchDistance;

	// Token: 0x04002833 RID: 10291
	public float grabTime;

	// Token: 0x04002834 RID: 10292
	public float grabDuration;

	// Token: 0x04002835 RID: 10293
	public float grabSpeed = 1f;

	// Token: 0x04002836 RID: 10294
	public float minGrabCooldown;

	// Token: 0x04002837 RID: 10295
	public float lastSpeedIncreased;

	// Token: 0x04002838 RID: 10296
	public Vector3[] headEulerAngles;

	// Token: 0x04002839 RID: 10297
	public Transform skullTransform;

	// Token: 0x0400283A RID: 10298
	public Transform leftArm;

	// Token: 0x0400283B RID: 10299
	public Transform rightArm;

	// Token: 0x0400283C RID: 10300
	public Transform leftHand;

	// Token: 0x0400283D RID: 10301
	public Transform rightHand;

	// Token: 0x0400283E RID: 10302
	public Transform[] spawnTransforms;

	// Token: 0x0400283F RID: 10303
	public Transform[] spawnTransformOffsets;

	// Token: 0x04002840 RID: 10304
	public NetPlayer targetPlayer;

	// Token: 0x04002841 RID: 10305
	public GameObject ghostBody;

	// Token: 0x04002842 RID: 10306
	public HalloweenGhostChaser.ChaseState currentState;

	// Token: 0x04002843 RID: 10307
	public HalloweenGhostChaser.ChaseState lastState;

	// Token: 0x04002844 RID: 10308
	public int spawnIndex;

	// Token: 0x04002845 RID: 10309
	public NetPlayer grabbedPlayer;

	// Token: 0x04002846 RID: 10310
	public Material ghostMaterial;

	// Token: 0x04002847 RID: 10311
	public Color defaultColor;

	// Token: 0x04002848 RID: 10312
	public Color summonedColor;

	// Token: 0x04002849 RID: 10313
	public bool isSummoned;

	// Token: 0x0400284A RID: 10314
	private bool targetIsOnNavMesh;

	// Token: 0x0400284B RID: 10315
	private const float navMeshSampleRange = 5f;

	// Token: 0x0400284C RID: 10316
	[Tooltip("Haptic vibration when chased by lucy")]
	public float hapticStrength = 1f;

	// Token: 0x0400284D RID: 10317
	public float hapticDuration = 1.5f;

	// Token: 0x0400284E RID: 10318
	private NavMeshPath path;

	// Token: 0x0400284F RID: 10319
	public List<Vector3> points;

	// Token: 0x04002850 RID: 10320
	public int currentTargetIdx;

	// Token: 0x04002851 RID: 10321
	private float nextPathTimestamp;

	// Token: 0x04002852 RID: 10322
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 5)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private HalloweenGhostChaser.GhostData _Data;

	// Token: 0x020005B1 RID: 1457
	public enum ChaseState
	{
		// Token: 0x04002854 RID: 10324
		Dormant = 1,
		// Token: 0x04002855 RID: 10325
		InitialRise,
		// Token: 0x04002856 RID: 10326
		Gong = 4,
		// Token: 0x04002857 RID: 10327
		Chasing = 8,
		// Token: 0x04002858 RID: 10328
		Grabbing = 16
	}

	// Token: 0x020005B2 RID: 1458
	[NetworkStructWeaved(5)]
	[StructLayout(LayoutKind.Explicit, Size = 20)]
	public struct GhostData : INetworkStruct
	{
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06002446 RID: 9286 RVA: 0x000B5127 File Offset: 0x000B3327
		// (set) Token: 0x06002447 RID: 9287 RVA: 0x000B5135 File Offset: 0x000B3335
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

		// Token: 0x04002859 RID: 10329
		[FieldOffset(0)]
		public int TargetActorNumber;

		// Token: 0x0400285A RID: 10330
		[FieldOffset(4)]
		public int CurrentState;

		// Token: 0x0400285B RID: 10331
		[FieldOffset(8)]
		public int SpawnIndex;

		// Token: 0x0400285C RID: 10332
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate@ReaderWriter@System_Single), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(12)]
		private FixedStorage@1 _CurrentSpeed;

		// Token: 0x0400285D RID: 10333
		[FieldOffset(16)]
		public NetworkBool IsSummoned;
	}
}
