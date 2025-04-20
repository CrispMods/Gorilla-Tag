using System;
using System.Collections.Generic;
using GorillaLocomotion;
using JetBrains.Annotations;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000A4 RID: 164
[RequireComponent(typeof(NetworkView))]
public class MonkeyeAI : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x0600041C RID: 1052 RVA: 0x0007AF0C File Offset: 0x0007910C
	private string UserIdFromRig(VRRig rig)
	{
		if (rig == null)
		{
			return "";
		}
		if (!NetworkSystem.Instance.InRoom)
		{
			if (rig == GorillaTagger.Instance.offlineVRRig)
			{
				return "-1";
			}
			Debug.Log("Not in a room but not targeting offline rig");
			return null;
		}
		else
		{
			if (rig == GorillaTagger.Instance.offlineVRRig)
			{
				return NetworkSystem.Instance.LocalPlayer.UserId;
			}
			if (rig.creator == null)
			{
				return "";
			}
			return rig.creator.UserId;
		}
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0007AF94 File Offset: 0x00079194
	private VRRig GetRig(string userId)
	{
		if (userId == "")
		{
			return null;
		}
		if (NetworkSystem.Instance.InRoom || !(userId != "-1"))
		{
			foreach (VRRig vrrig in this.GetValidChoosableRigs())
			{
				if (!(vrrig == null))
				{
					NetPlayer creator = vrrig.creator;
					if (creator != null && userId == creator.UserId)
					{
						return vrrig;
					}
				}
			}
			return null;
		}
		if (userId == "-1 " && GorillaTagger.Instance != null)
		{
			return GorillaTagger.Instance.offlineVRRig;
		}
		return null;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0007B058 File Offset: 0x00079258
	private float Distance2D(Vector3 a, Vector3 b)
	{
		Vector2 a2 = new Vector2(a.x, a.z);
		Vector2 b2 = new Vector2(b.x, b.z);
		return Vector2.Distance(a2, b2);
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x0007B090 File Offset: 0x00079290
	private Transform PickRandomPatrolPoint()
	{
		int num;
		do
		{
			num = UnityEngine.Random.Range(0, this.patrolPts.Count);
		}
		while (num == this.patrolIdx);
		this.patrolIdx = num;
		return this.patrolPts[num];
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0007B0D0 File Offset: 0x000792D0
	private void PickNewPath(bool pathFinished = false)
	{
		if (this.calculatingPath)
		{
			return;
		}
		this.currentWaypoint = 0;
		switch (this.replState.state)
		{
		case MonkeyeAI_ReplState.EStates.Patrolling:
			if (this.patrolCount == this.maxPatrols)
			{
				this.SetState(MonkeyeAI_ReplState.EStates.Patrolling);
				this.targetPosition = this.PickRandomPatrolPoint().position;
				this.patrolCount = 0;
			}
			else
			{
				this.targetPosition = this.PickRandomPatrolPoint().position;
				this.patrolCount++;
			}
			break;
		case MonkeyeAI_ReplState.EStates.Chasing:
			if (!this.lockedOn)
			{
				Vector3 position = base.transform.position;
				VRRig vrrig;
				if (this.ClosestPlayer(position, out vrrig) && vrrig != this.targetRig)
				{
					this.SetTargetPlayer(vrrig);
				}
			}
			if (this.targetRig == null)
			{
				this.SetState(MonkeyeAI_ReplState.EStates.Patrolling);
				this.targetPosition = this.sleepPt.position;
			}
			else
			{
				this.targetPosition = this.targetRig.transform.position;
			}
			break;
		case MonkeyeAI_ReplState.EStates.ReturnToSleepPt:
			this.targetPosition = this.sleepPt.position;
			break;
		}
		this.calculatingPath = true;
		this.seeker.StartPath(base.transform.position, this.targetPosition, new OnPathDelegate(this.OnPathComplete));
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0007B220 File Offset: 0x00079420
	private void Awake()
	{
		this.lazerFx = base.GetComponent<Monkeye_LazerFX>();
		this.animController = base.GetComponent<Animator>();
		this.layerBase = this.animController.GetLayerIndex("Base_Layer");
		this.layerForward = this.animController.GetLayerIndex("MoveFwdAddPose");
		this.layerLeft = this.animController.GetLayerIndex("TurnLAddPose");
		this.layerRight = this.animController.GetLayerIndex("TurnRAddPose");
		this.seeker = base.GetComponent<Seeker>();
		this.renderer = this.portalFx.GetComponent<Renderer>();
		this.portalMatPropBlock = new MaterialPropertyBlock();
		this.monkEyeMatPropBlock = new MaterialPropertyBlock();
		this.layerMask = (UnityLayer.Default.ToLayerMask() | UnityLayer.GorillaObject.ToLayerMask());
		this.SetDefaultAttackState();
		this.SetState(MonkeyeAI_ReplState.EStates.Sleeping);
		this.replStateRequestableOwnershipGaurd = this.replState.GetComponent<RequestableOwnershipGuard>();
		this.myRequestableOwnershipGaurd = base.GetComponent<RequestableOwnershipGuard>();
		if (this.monkEyeColor.a != 0f || this.monkEyeEyeColorNormal.a != 0f)
		{
			if (this.monkEyeColor.a != 0f)
			{
				this.monkEyeMatPropBlock.SetVector(MonkeyeAI.ColorShaderProp, this.monkEyeColor);
			}
			if (this.monkEyeEyeColorNormal.a != 0f)
			{
				this.monkEyeMatPropBlock.SetVector(MonkeyeAI.EyeColorShaderProp, this.monkEyeEyeColorNormal);
			}
			this.skinnedMeshRenderer.SetPropertyBlock(this.monkEyeMatPropBlock);
		}
		base.InvokeRepeating("AntiOverlapAssurance", 0.2f, 0.5f);
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x00033269 File Offset: 0x00031469
	private void Start()
	{
		NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0007B3B8 File Offset: 0x000795B8
	private void OnPathComplete(Path path_)
	{
		this.path = path_;
		this.currentWaypoint = 0;
		if (this.path.vectorPath.Count < 1)
		{
			base.transform.position = this.sleepPt.position;
			base.transform.rotation = this.sleepPt.rotation;
			this.path = null;
		}
		this.calculatingPath = false;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0007B420 File Offset: 0x00079620
	private void FollowPath()
	{
		if (this.path == null || this.currentWaypoint >= this.path.vectorPath.Count || this.currentWaypoint < 0)
		{
			this.PickNewPath(false);
			if (this.path == null)
			{
				return;
			}
		}
		if (this.Distance2D(base.transform.position, this.path.vectorPath[this.currentWaypoint]) < 0.01f)
		{
			if (this.currentWaypoint + 1 == this.path.vectorPath.Count)
			{
				this.PickNewPath(true);
				return;
			}
			this.currentWaypoint++;
		}
		Vector3 normalized = (this.path.vectorPath[this.currentWaypoint] - base.transform.position).normalized;
		normalized.y = 0f;
		if (this.animController.GetCurrentAnimatorStateInfo(0).IsName("Move"))
		{
			Vector3 a = normalized * this.speed;
			base.transform.position += a * this.deltaTime;
		}
		Mathf.Clamp01(Vector3.Dot(base.transform.forward, normalized) / 1.5707964f);
		if (Mathf.Sign(Vector3.Cross(base.transform.forward, normalized).y) > 0f)
		{
			this.animController.SetLayerWeight(this.layerRight, 0f);
		}
		else
		{
			this.animController.SetLayerWeight(this.layerLeft, 0f);
		}
		this.animController.SetLayerWeight(this.layerForward, 0f);
		Vector3 forward = Vector3.RotateTowards(base.transform.forward, normalized, this.rotationSpeed * this.deltaTime, 0f);
		base.transform.rotation = Quaternion.LookRotation(forward);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0007B604 File Offset: 0x00079804
	private bool PlayerNear(VRRig rig, float dist, out float playerDist)
	{
		if (rig == null)
		{
			playerDist = float.PositiveInfinity;
			return false;
		}
		playerDist = this.Distance2D(rig.transform.position, base.transform.position);
		return playerDist < dist && Physics.RaycastNonAlloc(new Ray(base.transform.position, rig.transform.position - base.transform.position), this.rayResults, playerDist, this.layerMask) <= 0;
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0007B694 File Offset: 0x00079894
	private void Sleeping()
	{
		this.audioSource.volume = Mathf.Min(this.sleepLoopVolume, this.audioSource.volume + this.deltaTime / this.sleepDuration);
		if (this.audioSource.volume == this.sleepLoopVolume)
		{
			this.SetState(MonkeyeAI_ReplState.EStates.Patrolling);
			this.PickNewPath(false);
		}
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0007B6F4 File Offset: 0x000798F4
	private bool ClosestPlayer(in Vector3 myPos, out VRRig outRig)
	{
		float num = float.MaxValue;
		outRig = null;
		foreach (VRRig vrrig in this.GetValidChoosableRigs())
		{
			float num2 = 0f;
			if (this.PlayerNear(vrrig, this.chaseDistance, out num2) && num2 < num)
			{
				num = num2;
				outRig = vrrig;
			}
		}
		return num != float.MaxValue;
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x0007B774 File Offset: 0x00079974
	private bool CheckForChase()
	{
		foreach (VRRig vrrig in this.GetValidChoosableRigs())
		{
			float num = 0f;
			if (this.PlayerNear(vrrig, this.wakeDistance, out num))
			{
				this.SetTargetPlayer(vrrig);
				this.SetState(MonkeyeAI_ReplState.EStates.Chasing);
				this.PickNewPath(false);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x0003327B File Offset: 0x0003147B
	public void SetChasePlayer(VRRig rig)
	{
		if (!this.GetValidChoosableRigs().Contains(rig))
		{
			return;
		}
		this.SetTargetPlayer(rig);
		this.lockedOn = true;
		this.SetState(MonkeyeAI_ReplState.EStates.Chasing);
		this.PickNewPath(false);
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x000332A8 File Offset: 0x000314A8
	public void SetSleep()
	{
		if (this.replState.state == MonkeyeAI_ReplState.EStates.Patrolling || this.replState.state == MonkeyeAI_ReplState.EStates.Chasing)
		{
			this.SetState(MonkeyeAI_ReplState.EStates.Sleeping);
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0007B7F4 File Offset: 0x000799F4
	private void Patrolling()
	{
		this.audioSource.volume = Mathf.Min(this.patrolLoopVolume, this.audioSource.volume + this.deltaTime / this.patrolLoopFadeInTime);
		if (this.path == null)
		{
			this.PickNewPath(false);
		}
		if (this.audioSource.volume == this.patrolLoopVolume)
		{
			this.CheckForChase();
		}
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x0007B85C File Offset: 0x00079A5C
	private void Chasing()
	{
		this.audioSource.volume = Mathf.Min(this.chaseLoopVolume, this.audioSource.volume + this.deltaTime / this.chaseLoopFadeInTime);
		this.PickNewPath(false);
		if (this.targetRig == null)
		{
			this.SetState(MonkeyeAI_ReplState.EStates.Patrolling);
			return;
		}
		if (this.Distance2D(base.transform.position, this.targetRig.transform.position) < this.attackDistance)
		{
			this.SetState(MonkeyeAI_ReplState.EStates.BeginAttack);
			return;
		}
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x0007B8E8 File Offset: 0x00079AE8
	private void ReturnToSleepPt()
	{
		if (this.path == null)
		{
			this.PickNewPath(false);
		}
		if (this.CheckForChase())
		{
			this.SetState(MonkeyeAI_ReplState.EStates.Chasing);
			return;
		}
		if (this.Distance2D(base.transform.position, this.sleepPt.position) < 0.01f)
		{
			this.SetState(MonkeyeAI_ReplState.EStates.Sleeping);
		}
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0007B940 File Offset: 0x00079B40
	private void UpdateClientState()
	{
		if (this.wasConnectedToRoom && !NetworkSystem.Instance.InRoom)
		{
			this.SetDefaultState();
			return;
		}
		if (ColliderEnabledManager.instance != null && !this.replState.floorEnabled)
		{
			if (!NetworkSystem.Instance.InRoom)
			{
				if (this.replState.userId == "-1")
				{
					ColliderEnabledManager.instance.DisableFloorForFrame();
				}
			}
			else if (this.replState.userId == NetworkSystem.Instance.LocalPlayer.UserId)
			{
				ColliderEnabledManager.instance.DisableFloorForFrame();
			}
		}
		if (this.portalFx.activeSelf != this.replState.portalEnabled)
		{
			this.portalFx.SetActive(this.replState.portalEnabled);
		}
		this.portalFx.transform.position = new Vector3(this.replState.attackPos.x, this.portalFx.transform.position.y, this.replState.attackPos.z);
		this.replState.timer -= this.deltaTime;
		if (this.replState.timer < 0f)
		{
			this.replState.timer = 0f;
		}
		VRRig rig = this.GetRig(this.replState.userId);
		if (this.replState.state >= MonkeyeAI_ReplState.EStates.BeginAttack)
		{
			if (rig == null)
			{
				this.lazerFx.DisableLazer();
			}
			else if (this.replState.state < MonkeyeAI_ReplState.EStates.DropPlayer)
			{
				this.lazerFx.EnableLazer(this.eyeBones, rig);
			}
			else
			{
				this.lazerFx.DisableLazer();
			}
		}
		else
		{
			this.lazerFx.DisableLazer();
		}
		if (this.replState.portalEnabled)
		{
			this.portalColor.a = this.replState.alpha;
			this.portalMatPropBlock.SetVector(MonkeyeAI.tintColorShaderProp, this.portalColor);
			this.renderer.SetPropertyBlock(this.portalMatPropBlock);
		}
		if (GorillaTagger.Instance.offlineVRRig == rig && this.replState.freezePlayer)
		{
			GTPlayer.Instance.SetMaximumSlipThisFrame();
			Rigidbody rigidbody = GorillaTagger.Instance.rigidbody;
			Vector3 vector = rigidbody.velocity;
			rigidbody.velocity = new Vector3(vector.x * this.deltaTime * 4f, Mathf.Min(vector.y, 0f), vector.x * this.deltaTime * 4f);
		}
		if (!this.replState.IsMine)
		{
			this.SetClientState(this.replState.state);
		}
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x000332CD File Offset: 0x000314CD
	private void SetDefaultState()
	{
		this.SetState(MonkeyeAI_ReplState.EStates.Sleeping);
		this.SetDefaultAttackState();
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x0007BBE8 File Offset: 0x00079DE8
	private void SetDefaultAttackState()
	{
		this.replState.floorEnabled = true;
		this.replState.timer = 0f;
		this.replState.userId = "";
		this.replState.attackPos = base.transform.position;
		this.replState.portalEnabled = false;
		this.replState.freezePlayer = false;
		this.replState.alpha = 0f;
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x000332DC File Offset: 0x000314DC
	private void ExitAttackState()
	{
		this.SetDefaultAttackState();
		this.SetState(MonkeyeAI_ReplState.EStates.Patrolling);
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0007BC60 File Offset: 0x00079E60
	private void BeginAttack()
	{
		this.path = null;
		this.replState.freezePlayer = true;
		if (this.replState.timer <= 0f)
		{
			if (this.audioSource.enabled)
			{
				this.audioSource.GTPlayOneShot(this.attackSound, this.attackVolume);
			}
			this.replState.timer = this.openFloorTime;
			this.replState.portalEnabled = true;
			this.SetState(MonkeyeAI_ReplState.EStates.OpenFloor);
		}
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x0007BCDC File Offset: 0x00079EDC
	private void OpenFloor()
	{
		this.replState.alpha = Mathf.Lerp(0f, 1f, 1f - Mathf.Clamp01(this.replState.timer / this.openFloorTime));
		if (this.replState.timer <= 0f)
		{
			this.replState.timer = this.dropPlayerTime;
			this.replState.floorEnabled = false;
			this.SetState(MonkeyeAI_ReplState.EStates.DropPlayer);
		}
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x000332EB File Offset: 0x000314EB
	private void DropPlayer()
	{
		if (this.replState.timer <= 0f)
		{
			this.replState.timer = this.dropPlayerTime;
			this.replState.floorEnabled = true;
			this.SetState(MonkeyeAI_ReplState.EStates.CloseFloor);
		}
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x00033323 File Offset: 0x00031523
	private void CloseFloor()
	{
		if (this.replState.timer <= 0f)
		{
			this.ExitAttackState();
		}
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x0007BD58 File Offset: 0x00079F58
	private void ValidateChasingRig()
	{
		if (this.targetRig == null)
		{
			this.SetTargetPlayer(null);
			return;
		}
		bool flag = false;
		foreach (VRRig vrrig in this.GetValidChoosableRigs())
		{
			if (vrrig == this.targetRig)
			{
				flag = true;
				this.SetTargetPlayer(vrrig);
				break;
			}
		}
		if (!flag)
		{
			this.SetTargetPlayer(null);
		}
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0007BDE0 File Offset: 0x00079FE0
	public void SetState(MonkeyeAI_ReplState.EStates state_)
	{
		if (this.replState.IsMine)
		{
			this.replState.state = state_;
		}
		this.animController.SetInteger(MonkeyeAI.animStateID, (int)this.replState.state);
		switch (this.replState.state)
		{
		case MonkeyeAI_ReplState.EStates.Sleeping:
			this.setEyeColor(this.monkEyeEyeColorNormal);
			this.lockedOn = false;
			this.audioSource.clip = this.sleepLoopSound;
			this.audioSource.volume = 0f;
			if (this.audioSource.enabled)
			{
				this.audioSource.GTPlay();
				return;
			}
			break;
		case MonkeyeAI_ReplState.EStates.Patrolling:
			this.setEyeColor(this.monkEyeEyeColorNormal);
			this.lockedOn = false;
			this.audioSource.clip = this.patrolLoopSound;
			this.audioSource.loop = true;
			this.audioSource.volume = 0f;
			if (this.audioSource.enabled)
			{
				this.audioSource.GTPlay();
			}
			this.patrolCount = 0;
			return;
		case MonkeyeAI_ReplState.EStates.Chasing:
			this.setEyeColor(this.monkEyeEyeColorNormal);
			this.audioSource.loop = true;
			this.audioSource.volume = 0f;
			this.audioSource.clip = this.chaseLoopSound;
			if (this.audioSource.enabled)
			{
				this.audioSource.GTPlay();
				return;
			}
			break;
		case MonkeyeAI_ReplState.EStates.ReturnToSleepPt:
		case MonkeyeAI_ReplState.EStates.GoToSleep:
			break;
		case MonkeyeAI_ReplState.EStates.BeginAttack:
			this.setEyeColor(this.monkEyeEyeColorAttacking);
			if (this.replState.IsMine)
			{
				this.replState.attackPos = ((this.targetRig != null) ? this.targetRig.transform.position : base.transform.position);
				this.replState.timer = this.beginAttackTime;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x0007BFB0 File Offset: 0x0007A1B0
	public void SetClientState(MonkeyeAI_ReplState.EStates state_)
	{
		this.animController.SetInteger(MonkeyeAI.animStateID, (int)this.replState.state);
		if (this.previousState != this.replState.state)
		{
			this.previousState = this.replState.state;
			switch (this.replState.state)
			{
			case MonkeyeAI_ReplState.EStates.Sleeping:
				this.setEyeColor(this.monkEyeEyeColorNormal);
				this.lockedOn = false;
				this.audioSource.clip = this.sleepLoopSound;
				this.audioSource.volume = Mathf.Min(this.sleepLoopVolume, this.audioSource.volume + this.deltaTime / this.sleepDuration);
				if (this.audioSource.enabled)
				{
					this.audioSource.GTPlay();
				}
				break;
			case MonkeyeAI_ReplState.EStates.Patrolling:
				this.setEyeColor(this.monkEyeEyeColorNormal);
				this.lockedOn = false;
				this.audioSource.clip = this.patrolLoopSound;
				this.audioSource.loop = true;
				this.audioSource.volume = Mathf.Min(this.patrolLoopVolume, this.audioSource.volume + this.deltaTime / this.patrolLoopFadeInTime);
				if (this.audioSource.enabled)
				{
					this.audioSource.GTPlay();
				}
				this.patrolCount = 0;
				break;
			case MonkeyeAI_ReplState.EStates.Chasing:
				this.setEyeColor(this.monkEyeEyeColorNormal);
				this.audioSource.loop = true;
				this.audioSource.volume = Mathf.Min(this.chaseLoopVolume, this.audioSource.volume + this.deltaTime / this.chaseLoopFadeInTime);
				this.audioSource.clip = this.chaseLoopSound;
				if (this.audioSource.enabled)
				{
					this.audioSource.GTPlay();
				}
				break;
			case MonkeyeAI_ReplState.EStates.BeginAttack:
				this.setEyeColor(this.monkEyeEyeColorAttacking);
				break;
			}
		}
		switch (this.replState.state)
		{
		case MonkeyeAI_ReplState.EStates.Sleeping:
			this.audioSource.volume = Mathf.Min(this.sleepLoopVolume, this.audioSource.volume + this.deltaTime / this.sleepDuration);
			return;
		case MonkeyeAI_ReplState.EStates.Patrolling:
			this.audioSource.volume = Mathf.Min(this.patrolLoopVolume, this.audioSource.volume + this.deltaTime / this.patrolLoopFadeInTime);
			return;
		case MonkeyeAI_ReplState.EStates.Chasing:
			this.audioSource.volume = Mathf.Min(this.chaseLoopVolume, this.audioSource.volume + this.deltaTime / this.chaseLoopFadeInTime);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0003333D File Offset: 0x0003153D
	private void setEyeColor(Color c)
	{
		if (c.a != 0f)
		{
			this.monkEyeMatPropBlock.SetVector(MonkeyeAI.EyeColorShaderProp, c);
			this.skinnedMeshRenderer.SetPropertyBlock(this.monkEyeMatPropBlock);
		}
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x0007C250 File Offset: 0x0007A450
	public List<VRRig> GetValidChoosableRigs()
	{
		this.validRigs.Clear();
		foreach (VRRig vrrig in this.playerCollection.containedRigs)
		{
			if ((NetworkSystem.Instance.InRoom || vrrig.isOfflineVRRig) && !(vrrig == null))
			{
				this.validRigs.Add(vrrig);
			}
		}
		return this.validRigs;
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x0007C2DC File Offset: 0x0007A4DC
	public void SliceUpdate()
	{
		this.wasConnectedToRoom = NetworkSystem.Instance.InRoom;
		this.deltaTime = Time.time - this.lastTime;
		this.lastTime = Time.time;
		this.UpdateClientState();
		if (NetworkSystem.Instance.InRoom && !this.replState.IsMine)
		{
			this.path = null;
			return;
		}
		if (!this.playerCollection.gameObject.activeInHierarchy)
		{
			NetPlayer netPlayer = null;
			float num = float.PositiveInfinity;
			foreach (VRRig vrrig in this.playersInRoomCollection.containedRigs)
			{
				if (!(vrrig == null))
				{
					float num2 = Vector3.Distance(base.transform.position, vrrig.transform.position);
					if (num2 < num)
					{
						netPlayer = vrrig.creator;
						num = num2;
					}
				}
			}
			if (num > 6f)
			{
				return;
			}
			this.path = null;
			if (netPlayer == null)
			{
				return;
			}
			this.replStateRequestableOwnershipGaurd.TransferOwnership(netPlayer, "");
			this.myRequestableOwnershipGaurd.TransferOwnership(netPlayer, "");
			return;
		}
		else
		{
			this.ValidateChasingRig();
			switch (this.replState.state)
			{
			case MonkeyeAI_ReplState.EStates.Sleeping:
				this.Sleeping();
				break;
			case MonkeyeAI_ReplState.EStates.Patrolling:
				this.Patrolling();
				break;
			case MonkeyeAI_ReplState.EStates.Chasing:
				this.Chasing();
				break;
			case MonkeyeAI_ReplState.EStates.ReturnToSleepPt:
				this.ReturnToSleepPt();
				break;
			case MonkeyeAI_ReplState.EStates.BeginAttack:
				this.BeginAttack();
				break;
			case MonkeyeAI_ReplState.EStates.OpenFloor:
				this.OpenFloor();
				break;
			case MonkeyeAI_ReplState.EStates.DropPlayer:
				this.DropPlayer();
				break;
			case MonkeyeAI_ReplState.EStates.CloseFloor:
				this.CloseFloor();
				break;
			}
			if (this.path == null)
			{
				return;
			}
			this.FollowPath();
			this.velocity = base.transform.position - this.prevPosition;
			this.prevPosition = base.transform.position;
			return;
		}
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00032C89 File Offset: 0x00030E89
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x0007C4C8 File Offset: 0x0007A6C8
	private void AntiOverlapAssurance()
	{
		try
		{
			if ((!NetworkSystem.Instance.InRoom || this.replState.IsMine) && this.playerCollection.gameObject.activeInHierarchy)
			{
				foreach (MonkeyeAI monkeyeAI in this.playerCollection.monkeyeAis)
				{
					if (!(monkeyeAI == this) && Vector3.Distance(base.transform.position, monkeyeAI.transform.position) < this.overlapRadius && (double)Vector3.Dot(base.transform.forward, monkeyeAI.transform.forward) > 0.2)
					{
						MonkeyeAI_ReplState.EStates state = this.replState.state;
						if (state != MonkeyeAI_ReplState.EStates.Patrolling)
						{
							if (state == MonkeyeAI_ReplState.EStates.Chasing)
							{
								if (monkeyeAI.replState.state == MonkeyeAI_ReplState.EStates.Chasing)
								{
									this.SetState(MonkeyeAI_ReplState.EStates.Patrolling);
								}
							}
						}
						else
						{
							this.PickNewPath(false);
						}
					}
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x0007C5F0 File Offset: 0x0007A7F0
	private void SetTargetPlayer([CanBeNull] VRRig rig)
	{
		if (rig == null)
		{
			this.replState.userId = "";
			this.replState.freezePlayer = false;
			this.replState.floorEnabled = true;
			this.replState.portalEnabled = false;
			this.targetRig = null;
			return;
		}
		this.replState.userId = this.UserIdFromRig(rig);
		this.targetRig = rig;
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x0400049E RID: 1182
	public List<Transform> patrolPts;

	// Token: 0x0400049F RID: 1183
	public Transform sleepPt;

	// Token: 0x040004A0 RID: 1184
	private int patrolIdx = -1;

	// Token: 0x040004A1 RID: 1185
	private int patrolCount;

	// Token: 0x040004A2 RID: 1186
	private Vector3 targetPosition;

	// Token: 0x040004A3 RID: 1187
	private MaterialPropertyBlock portalMatPropBlock;

	// Token: 0x040004A4 RID: 1188
	private MaterialPropertyBlock monkEyeMatPropBlock;

	// Token: 0x040004A5 RID: 1189
	private Renderer renderer;

	// Token: 0x040004A6 RID: 1190
	private AIDestinationSetter aiDest;

	// Token: 0x040004A7 RID: 1191
	private AIPath aiPath;

	// Token: 0x040004A8 RID: 1192
	private AILerp aiLerp;

	// Token: 0x040004A9 RID: 1193
	private Seeker seeker;

	// Token: 0x040004AA RID: 1194
	private Path path;

	// Token: 0x040004AB RID: 1195
	private int currentWaypoint;

	// Token: 0x040004AC RID: 1196
	private bool calculatingPath;

	// Token: 0x040004AD RID: 1197
	private Monkeye_LazerFX lazerFx;

	// Token: 0x040004AE RID: 1198
	private Animator animController;

	// Token: 0x040004AF RID: 1199
	private RaycastHit[] rayResults = new RaycastHit[1];

	// Token: 0x040004B0 RID: 1200
	private LayerMask layerMask;

	// Token: 0x040004B1 RID: 1201
	private bool wasConnectedToRoom;

	// Token: 0x040004B2 RID: 1202
	public SkinnedMeshRenderer skinnedMeshRenderer;

	// Token: 0x040004B3 RID: 1203
	public MazePlayerCollection playerCollection;

	// Token: 0x040004B4 RID: 1204
	public PlayerCollection playersInRoomCollection;

	// Token: 0x040004B5 RID: 1205
	private List<VRRig> validRigs = new List<VRRig>();

	// Token: 0x040004B6 RID: 1206
	public GameObject portalFx;

	// Token: 0x040004B7 RID: 1207
	public Transform[] eyeBones;

	// Token: 0x040004B8 RID: 1208
	public float speed = 0.1f;

	// Token: 0x040004B9 RID: 1209
	public float rotationSpeed = 1f;

	// Token: 0x040004BA RID: 1210
	public float wakeDistance = 1f;

	// Token: 0x040004BB RID: 1211
	public float chaseDistance = 3f;

	// Token: 0x040004BC RID: 1212
	public float sleepDuration = 3f;

	// Token: 0x040004BD RID: 1213
	public float attackDistance = 0.1f;

	// Token: 0x040004BE RID: 1214
	public float beginAttackTime = 1f;

	// Token: 0x040004BF RID: 1215
	public float openFloorTime = 3f;

	// Token: 0x040004C0 RID: 1216
	public float dropPlayerTime = 1f;

	// Token: 0x040004C1 RID: 1217
	public float closeFloorTime = 1f;

	// Token: 0x040004C2 RID: 1218
	public Color portalColor;

	// Token: 0x040004C3 RID: 1219
	public Color gorillaPortalColor;

	// Token: 0x040004C4 RID: 1220
	public Color monkEyeColor;

	// Token: 0x040004C5 RID: 1221
	public Color monkEyeEyeColorNormal;

	// Token: 0x040004C6 RID: 1222
	public Color monkEyeEyeColorAttacking;

	// Token: 0x040004C7 RID: 1223
	public int maxPatrols = 4;

	// Token: 0x040004C8 RID: 1224
	private VRRig targetRig;

	// Token: 0x040004C9 RID: 1225
	private float deltaTime;

	// Token: 0x040004CA RID: 1226
	private float lastTime;

	// Token: 0x040004CB RID: 1227
	public MonkeyeAI_ReplState replState;

	// Token: 0x040004CC RID: 1228
	private MonkeyeAI_ReplState.EStates previousState;

	// Token: 0x040004CD RID: 1229
	private RequestableOwnershipGuard replStateRequestableOwnershipGaurd;

	// Token: 0x040004CE RID: 1230
	private RequestableOwnershipGuard myRequestableOwnershipGaurd;

	// Token: 0x040004CF RID: 1231
	private int layerBase;

	// Token: 0x040004D0 RID: 1232
	private int layerForward = 1;

	// Token: 0x040004D1 RID: 1233
	private int layerLeft = 2;

	// Token: 0x040004D2 RID: 1234
	private int layerRight = 3;

	// Token: 0x040004D3 RID: 1235
	private static readonly int EmissionColorShaderProp = Shader.PropertyToID("_EmissionColor");

	// Token: 0x040004D4 RID: 1236
	private static readonly int ColorShaderProp = Shader.PropertyToID("_BaseColor");

	// Token: 0x040004D5 RID: 1237
	private static readonly int EyeColorShaderProp = Shader.PropertyToID("_GChannelColor");

	// Token: 0x040004D6 RID: 1238
	private static readonly int tintColorShaderProp = Shader.PropertyToID("_TintColor");

	// Token: 0x040004D7 RID: 1239
	private static readonly int animStateID = Animator.StringToHash("state");

	// Token: 0x040004D8 RID: 1240
	private Vector3 prevPosition;

	// Token: 0x040004D9 RID: 1241
	private Vector3 velocity;

	// Token: 0x040004DA RID: 1242
	public AudioSource audioSource;

	// Token: 0x040004DB RID: 1243
	public AudioClip sleepLoopSound;

	// Token: 0x040004DC RID: 1244
	public float sleepLoopVolume = 0.5f;

	// Token: 0x040004DD RID: 1245
	[FormerlySerializedAs("moveLoopSound")]
	public AudioClip patrolLoopSound;

	// Token: 0x040004DE RID: 1246
	public float patrolLoopVolume = 0.5f;

	// Token: 0x040004DF RID: 1247
	public float patrolLoopFadeInTime = 1f;

	// Token: 0x040004E0 RID: 1248
	public AudioClip chaseLoopSound;

	// Token: 0x040004E1 RID: 1249
	public float chaseLoopVolume = 0.5f;

	// Token: 0x040004E2 RID: 1250
	public float chaseLoopFadeInTime = 0.05f;

	// Token: 0x040004E3 RID: 1251
	public AudioClip attackSound;

	// Token: 0x040004E4 RID: 1252
	public float attackVolume = 0.5f;

	// Token: 0x040004E5 RID: 1253
	public float overlapRadius;

	// Token: 0x040004E6 RID: 1254
	private bool lockedOn;
}
