using System;
using System.Collections.Generic;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020000B2 RID: 178
public class SecondLookSkeleton : MonoBehaviour
{
	// Token: 0x0600047F RID: 1151 RVA: 0x0007B7C4 File Offset: 0x000799C4
	private void Start()
	{
		this.playersSeen = new List<NetPlayer>();
		this.synchValues = base.GetComponent<SecondLookSkeletonSynchValues>();
		this.playerTransform = Camera.main.transform;
		this.tapped = !this.requireTappingToActivate;
		this.localCaught = false;
		this.audioSource = base.GetComponentInChildren<AudioSource>();
		this.spookyGhost.SetActive(false);
		this.angerPointIndex = UnityEngine.Random.Range(0, this.angerPoint.Length);
		this.angerPointChangedTime = Time.time;
		this.synchValues.angerPoint = this.angerPointIndex;
		this.spookyGhost.transform.position = this.angerPoint[this.synchValues.angerPoint].position;
		this.spookyGhost.transform.rotation = this.angerPoint[this.synchValues.angerPoint].rotation;
		this.ChangeState(SecondLookSkeleton.GhostState.Unactivated);
		this.rHits = new RaycastHit[20];
		this.lookedAway = false;
		this.firstLookActivated = false;
		this.animator.Play("ArmsOut");
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00032686 File Offset: 0x00030886
	private void Update()
	{
		this.ProcessGhostState();
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0007B8DC File Offset: 0x00079ADC
	public void ChangeState(SecondLookSkeleton.GhostState newState)
	{
		if (newState == this.currentState)
		{
			return;
		}
		switch (newState)
		{
		case SecondLookSkeleton.GhostState.Unactivated:
			this.spookyGhost.gameObject.SetActive(false);
			this.audioSource.GTStop();
			this.audioSource.loop = false;
			if (this.IsMine())
			{
				this.synchValues.angerPoint = UnityEngine.Random.Range(0, this.angerPoint.Length);
				this.angerPointIndex = this.synchValues.angerPoint;
				this.angerPointChangedTime = Time.time;
				this.spookyGhost.transform.position = this.angerPoint[this.angerPointIndex].position;
				this.spookyGhost.transform.rotation = this.angerPoint[this.angerPointIndex].rotation;
			}
			this.currentState = SecondLookSkeleton.GhostState.Unactivated;
			return;
		case SecondLookSkeleton.GhostState.Activated:
			this.currentState = SecondLookSkeleton.GhostState.Activated;
			if (this.tapped)
			{
				GTAudioSourceExtensions.GTPlayClipAtPoint(this.initialScream, this.audioSource.transform.position, 1f);
				if (this.spookyText != null)
				{
					this.spookyText.SetActive(true);
				}
				this.spookyGhost.SetActive(true);
			}
			this.animator.Play("ArmsOut");
			this.spookyGhost.transform.rotation = Quaternion.LookRotation(this.playerTransform.position - this.spookyGhost.transform.position, Vector3.up);
			if (this.IsMine())
			{
				this.timeFirstAppeared = Time.time;
				return;
			}
			break;
		case SecondLookSkeleton.GhostState.Patrolling:
			this.playersSeen.Clear();
			if (this.tapped)
			{
				this.spookyGhost.SetActive(true);
				this.animator.Play("CrawlPatrol");
				this.audioSource.loop = true;
				this.audioSource.clip = this.patrolLoop;
				this.audioSource.GTPlay();
			}
			if (this.IsMine())
			{
				this.currentNode = this.pathPoints[UnityEngine.Random.Range(0, this.pathPoints.Length)];
				this.nextNode = this.currentNode.connectedNodes[UnityEngine.Random.Range(0, this.currentNode.connectedNodes.Length)];
				this.SyncNodes();
				this.spookyGhost.transform.position = this.currentNode.transform.position;
			}
			this.currentState = SecondLookSkeleton.GhostState.Patrolling;
			return;
		case SecondLookSkeleton.GhostState.Chasing:
			this.currentState = SecondLookSkeleton.GhostState.Chasing;
			this.resetChaseHistory.Clear();
			this.animator.Play("CrawlChase");
			this.localThrown = false;
			this.localCaught = false;
			if (this.tapped)
			{
				this.audioSource.clip = this.chaseLoop;
				this.audioSource.loop = true;
				this.audioSource.GTPlay();
				return;
			}
			break;
		case SecondLookSkeleton.GhostState.CaughtPlayer:
			this.currentState = SecondLookSkeleton.GhostState.CaughtPlayer;
			this.heightOffset.localPosition = Vector3.zero;
			if (this.tapped)
			{
				this.audioSource.GTPlayOneShot(this.grabbedSound, 1f);
				this.audioSource.loop = true;
				this.audioSource.clip = this.carryingLoop;
				this.audioSource.GTPlay();
				this.animator.Play("ArmsOut");
			}
			if (!this.IsMine())
			{
				this.SetNodes();
				return;
			}
			break;
		case SecondLookSkeleton.GhostState.PlayerThrown:
			this.currentState = SecondLookSkeleton.GhostState.PlayerThrown;
			this.timeThrown = Time.time;
			this.localThrown = false;
			break;
		case SecondLookSkeleton.GhostState.Reset:
			break;
		default:
			return;
		}
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0007BC44 File Offset: 0x00079E44
	private void ProcessGhostState()
	{
		if (this.IsMine())
		{
			switch (this.currentState)
			{
			case SecondLookSkeleton.GhostState.Unactivated:
				if (this.changeAngerPointOnTimeInterval && Time.time - this.angerPointChangedTime > this.changeAngerPointTimeMinutes * 60f)
				{
					this.synchValues.angerPoint = UnityEngine.Random.Range(0, this.angerPoint.Length);
					this.angerPointIndex = this.synchValues.angerPoint;
					this.angerPointChangedTime = Time.time;
				}
				this.spookyGhost.transform.position = this.angerPoint[this.angerPointIndex].position;
				this.spookyGhost.transform.rotation = this.angerPoint[this.angerPointIndex].rotation;
				this.CheckActivateGhost();
				return;
			case SecondLookSkeleton.GhostState.Activated:
				if (Time.time > this.timeFirstAppeared + this.timeToFirstDisappear)
				{
					this.ChangeState(SecondLookSkeleton.GhostState.Patrolling);
					return;
				}
				break;
			case SecondLookSkeleton.GhostState.Patrolling:
				if (!this.CheckPlayerSeen() && this.playersSeen.Count == 0)
				{
					this.PatrolMove();
					return;
				}
				this.StartChasing();
				return;
			case SecondLookSkeleton.GhostState.Chasing:
				if (!this.CheckPlayerSeen() || !this.CanGrab())
				{
					this.ChaseMove();
					return;
				}
				this.GrabPlayer();
				return;
			case SecondLookSkeleton.GhostState.CaughtPlayer:
				this.CaughtPlayerUpdate();
				return;
			case SecondLookSkeleton.GhostState.PlayerThrown:
				if (Time.time > this.timeThrown + this.timeThrownCooldown)
				{
					this.ChangeState(SecondLookSkeleton.GhostState.Unactivated);
				}
				break;
			case SecondLookSkeleton.GhostState.Reset:
				break;
			default:
				return;
			}
			return;
		}
		this.SetTappedState();
		switch (this.currentState)
		{
		case SecondLookSkeleton.GhostState.Unactivated:
			this.SetNodes();
			this.spookyGhost.transform.position = this.angerPoint[this.angerPointIndex].position;
			this.spookyGhost.transform.rotation = this.angerPoint[this.angerPointIndex].rotation;
			this.CheckActivateGhost();
			return;
		case SecondLookSkeleton.GhostState.Activated:
			this.FollowPosition();
			return;
		case SecondLookSkeleton.GhostState.Patrolling:
			this.FollowPosition();
			this.CheckPlayerSeen();
			return;
		case SecondLookSkeleton.GhostState.Chasing:
			if (this.CheckPlayerSeen() && this.CanGrab())
			{
				this.GrabPlayer();
			}
			this.FollowPosition();
			return;
		case SecondLookSkeleton.GhostState.CaughtPlayer:
		case SecondLookSkeleton.GhostState.PlayerThrown:
			this.CaughtPlayerUpdate();
			break;
		case SecondLookSkeleton.GhostState.Reset:
			break;
		default:
			return;
		}
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0007BE64 File Offset: 0x0007A064
	private void CaughtPlayerUpdate()
	{
		if (this.localThrown)
		{
			return;
		}
		if (this.GhostAtExit())
		{
			if (this.localCaught)
			{
				this.ChuckPlayer();
			}
			if (this.IsMine())
			{
				this.DeactivateGhost();
			}
			return;
		}
		this.CaughtMove();
		if (this.localCaught)
		{
			this.FloatPlayer();
			return;
		}
		if (this.CheckPlayerSeen() && this.CanGrab())
		{
			this.localCaught = true;
		}
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0007BECC File Offset: 0x0007A0CC
	private void SetTappedState()
	{
		if (!this.tapped)
		{
			return;
		}
		if (this.spookyText != null && !this.spookyText.activeSelf)
		{
			this.spookyText.SetActive(true);
		}
		if (this.spookyGhost.activeSelf && this.currentState != SecondLookSkeleton.GhostState.Unactivated)
		{
			return;
		}
		this.spookyGhost.SetActive(true);
		switch (this.currentState)
		{
		case SecondLookSkeleton.GhostState.Unactivated:
			this.spookyGhost.SetActive(false);
			return;
		case SecondLookSkeleton.GhostState.Activated:
			this.animator.Play("ArmsOut");
			return;
		case SecondLookSkeleton.GhostState.Patrolling:
			this.animator.Play("CrawlPatrol");
			this.audioSource.loop = true;
			this.audioSource.clip = this.patrolLoop;
			this.audioSource.GTPlay();
			return;
		case SecondLookSkeleton.GhostState.Chasing:
			this.audioSource.clip = this.chaseLoop;
			this.audioSource.loop = true;
			this.audioSource.GTPlay();
			this.animator.Play("CrawlChase");
			this.spookyGhost.SetActive(true);
			return;
		case SecondLookSkeleton.GhostState.CaughtPlayer:
			this.audioSource.GTPlayOneShot(this.grabbedSound, 1f);
			this.audioSource.loop = true;
			this.audioSource.clip = this.carryingLoop;
			this.audioSource.GTPlay();
			this.animator.Play("ArmsOut");
			break;
		case SecondLookSkeleton.GhostState.PlayerThrown:
			this.animator.Play("ArmsOut");
			return;
		case SecondLookSkeleton.GhostState.Reset:
			break;
		default:
			return;
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0007C050 File Offset: 0x0007A250
	private void FollowPosition()
	{
		this.spookyGhost.transform.position = Vector3.Lerp(this.spookyGhost.transform.position, this.synchValues.position, 0.66f);
		this.spookyGhost.transform.rotation = Quaternion.Lerp(this.spookyGhost.transform.rotation, this.synchValues.rotation, 0.66f);
		if (this.currentState == SecondLookSkeleton.GhostState.Patrolling || this.currentState == SecondLookSkeleton.GhostState.Chasing)
		{
			this.SetHeightOffset();
			return;
		}
		this.heightOffset.localPosition = Vector3.zero;
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x0007C0F0 File Offset: 0x0007A2F0
	private void CheckActivateGhost()
	{
		if (!this.tapped || this.currentState != SecondLookSkeleton.GhostState.Unactivated || this.playerTransform == null)
		{
			return;
		}
		this.currentlyLooking = this.IsCurrentlyLooking();
		if (this.requireSecondLookToActivate)
		{
			if (!this.firstLookActivated && this.currentlyLooking)
			{
				this.firstLookActivated = this.currentlyLooking;
				return;
			}
			if (this.firstLookActivated && !this.currentlyLooking)
			{
				this.lookedAway = true;
				return;
			}
			if (this.firstLookActivated && this.lookedAway && this.currentlyLooking)
			{
				this.firstLookActivated = false;
				this.lookedAway = false;
				this.ActivateGhost();
				return;
			}
		}
		else if (this.currentlyLooking)
		{
			this.ActivateGhost();
		}
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0003268E File Offset: 0x0003088E
	private bool CanSeePlayer()
	{
		return this.CanSeePlayerWithResults(out this.closest);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0007C1A0 File Offset: 0x0007A3A0
	private bool CanSeePlayerWithResults(out RaycastHit closest)
	{
		Vector3 vector = this.playerTransform.position - this.lookSource.position;
		int num = Physics.RaycastNonAlloc(this.lookSource.position, vector.normalized, this.rHits, this.maxSeeDistance, this.mask, QueryTriggerInteraction.Ignore);
		closest = this.rHits[0];
		if (num == 0)
		{
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			if (closest.distance > this.rHits[i].distance)
			{
				closest = this.rHits[i];
			}
		}
		return (this.playerMask & 1 << closest.collider.gameObject.layer) != 0;
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0003269C File Offset: 0x0003089C
	private void ActivateGhost()
	{
		if (this.IsMine())
		{
			this.ChangeState(SecondLookSkeleton.GhostState.Activated);
			return;
		}
		this.synchValues.SendRPC("RemoteActivateGhost", RpcTarget.MasterClient, Array.Empty<object>());
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x000326C4 File Offset: 0x000308C4
	private void StartChasing()
	{
		if (!this.IsMine())
		{
			return;
		}
		this.ChangeState(SecondLookSkeleton.GhostState.Chasing);
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0007C26C File Offset: 0x0007A46C
	private bool CheckPlayerSeen()
	{
		if (!this.tapped)
		{
			return false;
		}
		if (this.playersSeen.Contains(NetworkSystem.Instance.LocalPlayer))
		{
			return true;
		}
		if (!this.CanSeePlayer())
		{
			return false;
		}
		if (NetworkSystem.Instance.InRoom)
		{
			this.synchValues.SendRPC("RemotePlayerSeen", RpcTarget.Others, Array.Empty<object>());
		}
		this.playersSeen.Add(NetworkSystem.Instance.LocalPlayer);
		return true;
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x000326D6 File Offset: 0x000308D6
	public void RemoteActivateGhost()
	{
		if (this.IsMine() && this.currentState == SecondLookSkeleton.GhostState.Unactivated)
		{
			this.ActivateGhost();
		}
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x000326EE File Offset: 0x000308EE
	public void RemotePlayerSeen(NetPlayer player)
	{
		if (this.IsMine() && !this.playersSeen.Contains(player))
		{
			this.playersSeen.Add(player);
		}
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0007C2E0 File Offset: 0x0007A4E0
	public void RemotePlayerCaught(NetPlayer player)
	{
		if (this.IsMine() && this.currentState == SecondLookSkeleton.GhostState.Chasing)
		{
			RigContainer x;
			VRRigCache.Instance.TryGetVrrig(player, out x);
			if (x != null && this.playersSeen.Contains(player))
			{
				this.ChangeState(SecondLookSkeleton.GhostState.CaughtPlayer);
			}
		}
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x0007C32C File Offset: 0x0007A52C
	private bool IsCurrentlyLooking()
	{
		return Vector3.Dot(this.playerTransform.forward, -this.spookyGhost.transform.forward) > 0f && (this.spookyGhost.transform.position - this.playerTransform.position).magnitude < this.ghostActivationDistance && this.CanSeePlayer();
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00032712 File Offset: 0x00030912
	private void PatrolMove()
	{
		this.GhostMove(this.nextNode.transform, this.patrolSpeed);
		this.SetHeightOffset();
		this.CheckReachedNextNode(false, false);
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x0007C3A0 File Offset: 0x0007A5A0
	private void CheckReachedNextNode(bool forChuck, bool forChase)
	{
		if ((this.nextNode.transform.position - this.spookyGhost.transform.position).magnitude < this.reachNodeDist)
		{
			if (this.nextNode.connectedNodes.Length == 1)
			{
				this.currentNode = this.nextNode;
				this.nextNode = this.nextNode.connectedNodes[0];
				this.SyncNodes();
				return;
			}
			if (forChuck)
			{
				float distanceToExitNode = this.nextNode.distanceToExitNode;
				SkeletonPathingNode skeletonPathingNode = this.nextNode.connectedNodes[0];
				for (int i = 0; i < this.nextNode.connectedNodes.Length; i++)
				{
					if (this.nextNode.connectedNodes[i].distanceToExitNode <= distanceToExitNode)
					{
						skeletonPathingNode = this.nextNode.connectedNodes[i];
						distanceToExitNode = skeletonPathingNode.distanceToExitNode;
					}
				}
				this.currentNode = this.nextNode;
				this.nextNode = skeletonPathingNode;
				this.SyncNodes();
				return;
			}
			if (forChase)
			{
				float num = float.MaxValue;
				float num2 = num;
				RigContainer rigContainer = GorillaTagger.Instance.offlineVRRig.rigContainer;
				RigContainer rigContainer2 = rigContainer;
				for (int j = 0; j < this.playersSeen.Count; j++)
				{
					VRRigCache.Instance.TryGetVrrig(this.playersSeen[j], out rigContainer);
					if (!(rigContainer == null))
					{
						num = (rigContainer.transform.position - this.nextNode.transform.position).sqrMagnitude;
						if (num < num2)
						{
							rigContainer2 = rigContainer;
							num2 = num;
						}
					}
				}
				Vector3 vector = rigContainer2.transform.position - this.nextNode.transform.position;
				SkeletonPathingNode skeletonPathingNode2 = this.nextNode.connectedNodes[0];
				num2 = 0f;
				for (int k = 0; k < this.nextNode.connectedNodes.Length; k++)
				{
					Vector3 vector2 = this.nextNode.connectedNodes[k].transform.position - this.nextNode.transform.position;
					num = Mathf.Sign(Vector3.Dot(vector, vector2)) * Vector3.Project(vector, vector2).sqrMagnitude;
					if (num >= num2)
					{
						skeletonPathingNode2 = this.nextNode.connectedNodes[k];
						num2 = num;
					}
				}
				this.currentNode = this.nextNode;
				this.nextNode = skeletonPathingNode2;
				this.SyncNodes();
				this.resetChaseHistory.Add(this.nextNode);
				if (this.resetChaseHistory.Count > 8)
				{
					this.resetChaseHistory.RemoveAt(0);
				}
				if (this.resetChaseHistory.Count >= 8 && this.resetChaseHistory[0] == this.resetChaseHistory[2] == this.resetChaseHistory[4] == this.resetChaseHistory[6] && this.resetChaseHistory[1] == this.resetChaseHistory[3] == this.resetChaseHistory[5] == this.resetChaseHistory[7])
				{
					this.resetChaseHistory.Clear();
					this.ChangeState(SecondLookSkeleton.GhostState.Patrolling);
				}
				return;
			}
			SkeletonPathingNode skeletonPathingNode3 = this.nextNode.connectedNodes[UnityEngine.Random.Range(0, this.nextNode.connectedNodes.Length)];
			for (int l = 0; l < 10; l++)
			{
				skeletonPathingNode3 = this.nextNode.connectedNodes[UnityEngine.Random.Range(0, this.nextNode.connectedNodes.Length)];
				if (!skeletonPathingNode3.ejectionPoint && skeletonPathingNode3 != this.currentNode)
				{
					break;
				}
			}
			this.currentNode = this.nextNode;
			this.nextNode = skeletonPathingNode3;
			this.SyncNodes();
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00032739 File Offset: 0x00030939
	private void ChaseMove()
	{
		this.GhostMove(this.nextNode.transform, this.chaseSpeed);
		this.SetHeightOffset();
		this.CheckReachedNextNode(false, true);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00032760 File Offset: 0x00030960
	private void CaughtMove()
	{
		this.GhostMove(this.nextNode.transform, this.caughtSpeed);
		this.CheckReachedNextNode(true, false);
		this.SyncNodes();
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0007C76C File Offset: 0x0007A96C
	private void SyncNodes()
	{
		this.synchValues.currentNode = this.pathPoints.IndexOfRef(this.currentNode);
		this.synchValues.nextNode = this.pathPoints.IndexOfRef(this.nextNode);
		this.synchValues.angerPoint = this.angerPointIndex;
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0007C7C4 File Offset: 0x0007A9C4
	public void SetNodes()
	{
		if (this.synchValues.currentNode > this.pathPoints.Length || this.synchValues.currentNode < 0)
		{
			return;
		}
		this.currentNode = this.pathPoints[this.synchValues.currentNode];
		this.nextNode = this.pathPoints[this.synchValues.nextNode];
		this.angerPointIndex = this.synchValues.angerPoint;
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x0007C838 File Offset: 0x0007AA38
	private bool GhostAtExit()
	{
		return this.currentNode.distanceToExitNode == 0f && (this.spookyGhost.transform.position - this.currentNode.transform.position).magnitude < this.reachNodeDist;
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0007C890 File Offset: 0x0007AA90
	private void GhostMove(Transform target, float speed)
	{
		this.spookyGhost.transform.rotation = Quaternion.RotateTowards(this.spookyGhost.transform.rotation, Quaternion.LookRotation(target.position - this.spookyGhost.transform.position, Vector3.up), this.maxRotSpeed * Time.deltaTime);
		this.spookyGhost.transform.position += (target.position - this.spookyGhost.transform.position).normalized * speed * Time.deltaTime;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00032787 File Offset: 0x00030987
	private void DeactivateGhost()
	{
		this.ChangeState(SecondLookSkeleton.GhostState.PlayerThrown);
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0007C944 File Offset: 0x0007AB44
	private bool CanGrab()
	{
		return (this.spookyGhost.transform.position - this.playerTransform.position).magnitude < this.catchDistance;
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00032790 File Offset: 0x00030990
	private void GrabPlayer()
	{
		if (this.IsMine())
		{
			if (this.currentState == SecondLookSkeleton.GhostState.Chasing)
			{
				this.ChangeState(SecondLookSkeleton.GhostState.CaughtPlayer);
			}
			this.localCaught = true;
		}
		this.synchValues.SendRPC("RemotePlayerCaught", RpcTarget.MasterClient, Array.Empty<object>());
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x0007C984 File Offset: 0x0007AB84
	private void FloatPlayer()
	{
		RaycastHit raycastHit;
		if (this.CanSeePlayerWithResults(out raycastHit))
		{
			GorillaTagger.Instance.rigidbody.MovePosition(Vector3.MoveTowards(GorillaTagger.Instance.rigidbody.position, this.spookyGhost.transform.position + this.spookyGhost.transform.rotation * this.offsetGrabPosition, this.caughtSpeed * 10f * Time.deltaTime));
		}
		else
		{
			Vector3 vector = raycastHit.point - this.playerTransform.position;
			vector += GTPlayer.Instance.headCollider.radius * 1.05f * vector.normalized;
			GorillaTagger.Instance.transform.parent.position += vector;
			GTPlayer.Instance.InitializeValues();
		}
		GorillaTagger.Instance.rigidbody.velocity = Vector3.zero;
		EquipmentInteractor.instance.ForceStopClimbing();
		GorillaTagger.Instance.ApplyStatusEffect(GorillaTagger.StatusEffect.Frozen, 0.25f);
		GorillaTagger.Instance.StartVibration(true, this.hapticStrength / 4f, Time.deltaTime);
		GorillaTagger.Instance.StartVibration(false, this.hapticStrength / 4f, Time.deltaTime);
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0007CAD4 File Offset: 0x0007ACD4
	private void ChuckPlayer()
	{
		this.localCaught = false;
		this.localThrown = true;
		Vector3 vector = this.currentNode.transform.position - this.currentNode.connectedNodes[0].transform.position;
		GorillaTagger instance = GorillaTagger.Instance;
		Rigidbody rigidbody = (instance != null) ? instance.rigidbody : null;
		GTAudioSourceExtensions.GTPlayClipAtPoint(this.throwSound, this.audioSource.transform.position, 0.25f);
		this.audioSource.GTStop();
		this.audioSource.loop = false;
		if (rigidbody != null)
		{
			rigidbody.velocity = vector.normalized * this.throwForce;
		}
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0007CB88 File Offset: 0x0007AD88
	private void SetHeightOffset()
	{
		int num = Physics.RaycastNonAlloc(this.spookyGhost.transform.position + Vector3.up * this.bodyHeightOffset, Vector3.down, this.rHits, this.maxSeeDistance, this.mask, QueryTriggerInteraction.Ignore);
		if (num == 0)
		{
			this.heightOffset.localPosition = Vector3.zero;
			return;
		}
		RaycastHit raycastHit = this.rHits[0];
		for (int i = 0; i < num; i++)
		{
			if (raycastHit.distance < this.rHits[i].distance)
			{
				raycastHit = this.rHits[i];
			}
		}
		this.heightOffset.localPosition = new Vector3(0f, -raycastHit.distance, 0f);
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x000327C7 File Offset: 0x000309C7
	private bool IsMine()
	{
		return !NetworkSystem.Instance.InRoom || this.synchValues.IsMine;
	}

	// Token: 0x04000521 RID: 1313
	public Transform[] angerPoint;

	// Token: 0x04000522 RID: 1314
	public int angerPointIndex;

	// Token: 0x04000523 RID: 1315
	public SkeletonPathingNode[] pathPoints;

	// Token: 0x04000524 RID: 1316
	public SkeletonPathingNode[] exitPoints;

	// Token: 0x04000525 RID: 1317
	public Transform heightOffset;

	// Token: 0x04000526 RID: 1318
	public bool requireSecondLookToActivate;

	// Token: 0x04000527 RID: 1319
	public bool requireTappingToActivate;

	// Token: 0x04000528 RID: 1320
	public bool changeAngerPointOnTimeInterval;

	// Token: 0x04000529 RID: 1321
	public float changeAngerPointTimeMinutes = 3f;

	// Token: 0x0400052A RID: 1322
	private bool firstLookActivated;

	// Token: 0x0400052B RID: 1323
	private bool lookedAway;

	// Token: 0x0400052C RID: 1324
	private bool currentlyLooking;

	// Token: 0x0400052D RID: 1325
	public float ghostActivationDistance;

	// Token: 0x0400052E RID: 1326
	public GameObject spookyGhost;

	// Token: 0x0400052F RID: 1327
	public float timeFirstAppeared;

	// Token: 0x04000530 RID: 1328
	public float timeToFirstDisappear;

	// Token: 0x04000531 RID: 1329
	public SecondLookSkeleton.GhostState currentState;

	// Token: 0x04000532 RID: 1330
	public GameObject spookyText;

	// Token: 0x04000533 RID: 1331
	public float patrolSpeed;

	// Token: 0x04000534 RID: 1332
	public float chaseSpeed;

	// Token: 0x04000535 RID: 1333
	public float caughtSpeed;

	// Token: 0x04000536 RID: 1334
	public SkeletonPathingNode firstNode;

	// Token: 0x04000537 RID: 1335
	public SkeletonPathingNode currentNode;

	// Token: 0x04000538 RID: 1336
	public SkeletonPathingNode nextNode;

	// Token: 0x04000539 RID: 1337
	public Transform lookSource;

	// Token: 0x0400053A RID: 1338
	private Transform playerTransform;

	// Token: 0x0400053B RID: 1339
	public float reachNodeDist;

	// Token: 0x0400053C RID: 1340
	public float maxRotSpeed;

	// Token: 0x0400053D RID: 1341
	public float hapticStrength;

	// Token: 0x0400053E RID: 1342
	public float hapticDuration;

	// Token: 0x0400053F RID: 1343
	public Vector3 offsetGrabPosition;

	// Token: 0x04000540 RID: 1344
	public float throwForce;

	// Token: 0x04000541 RID: 1345
	public Animator animator;

	// Token: 0x04000542 RID: 1346
	public float bodyHeightOffset;

	// Token: 0x04000543 RID: 1347
	private float timeThrown;

	// Token: 0x04000544 RID: 1348
	public float timeThrownCooldown = 1f;

	// Token: 0x04000545 RID: 1349
	public float catchDistance;

	// Token: 0x04000546 RID: 1350
	public float maxSeeDistance;

	// Token: 0x04000547 RID: 1351
	private RaycastHit[] rHits;

	// Token: 0x04000548 RID: 1352
	public LayerMask mask;

	// Token: 0x04000549 RID: 1353
	public LayerMask playerMask;

	// Token: 0x0400054A RID: 1354
	public AudioSource audioSource;

	// Token: 0x0400054B RID: 1355
	public AudioClip initialScream;

	// Token: 0x0400054C RID: 1356
	public AudioClip patrolLoop;

	// Token: 0x0400054D RID: 1357
	public AudioClip chaseLoop;

	// Token: 0x0400054E RID: 1358
	public AudioClip grabbedSound;

	// Token: 0x0400054F RID: 1359
	public AudioClip carryingLoop;

	// Token: 0x04000550 RID: 1360
	public AudioClip throwSound;

	// Token: 0x04000551 RID: 1361
	public List<SkeletonPathingNode> resetChaseHistory = new List<SkeletonPathingNode>();

	// Token: 0x04000552 RID: 1362
	private SecondLookSkeletonSynchValues synchValues;

	// Token: 0x04000553 RID: 1363
	private bool localCaught;

	// Token: 0x04000554 RID: 1364
	private bool localThrown;

	// Token: 0x04000555 RID: 1365
	public List<NetPlayer> playersSeen;

	// Token: 0x04000556 RID: 1366
	public bool tapped;

	// Token: 0x04000557 RID: 1367
	private RaycastHit closest;

	// Token: 0x04000558 RID: 1368
	private float angerPointChangedTime;

	// Token: 0x020000B3 RID: 179
	public enum GhostState
	{
		// Token: 0x0400055A RID: 1370
		Unactivated,
		// Token: 0x0400055B RID: 1371
		Activated,
		// Token: 0x0400055C RID: 1372
		Patrolling,
		// Token: 0x0400055D RID: 1373
		Chasing,
		// Token: 0x0400055E RID: 1374
		CaughtPlayer,
		// Token: 0x0400055F RID: 1375
		PlayerThrown,
		// Token: 0x04000560 RID: 1376
		Reset
	}
}
