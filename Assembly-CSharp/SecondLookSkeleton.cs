using System;
using System.Collections.Generic;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class SecondLookSkeleton : MonoBehaviour
{
	// Token: 0x060004B9 RID: 1209 RVA: 0x0007E020 File Offset: 0x0007C220
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

	// Token: 0x060004BA RID: 1210 RVA: 0x0003388D File Offset: 0x00031A8D
	private void Update()
	{
		this.ProcessGhostState();
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0007E138 File Offset: 0x0007C338
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

	// Token: 0x060004BC RID: 1212 RVA: 0x0007E4A0 File Offset: 0x0007C6A0
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

	// Token: 0x060004BD RID: 1213 RVA: 0x0007E6C0 File Offset: 0x0007C8C0
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

	// Token: 0x060004BE RID: 1214 RVA: 0x0007E728 File Offset: 0x0007C928
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

	// Token: 0x060004BF RID: 1215 RVA: 0x0007E8AC File Offset: 0x0007CAAC
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

	// Token: 0x060004C0 RID: 1216 RVA: 0x0007E94C File Offset: 0x0007CB4C
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

	// Token: 0x060004C1 RID: 1217 RVA: 0x00033895 File Offset: 0x00031A95
	private bool CanSeePlayer()
	{
		return this.CanSeePlayerWithResults(out this.closest);
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x0007E9FC File Offset: 0x0007CBFC
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

	// Token: 0x060004C3 RID: 1219 RVA: 0x000338A3 File Offset: 0x00031AA3
	private void ActivateGhost()
	{
		if (this.IsMine())
		{
			this.ChangeState(SecondLookSkeleton.GhostState.Activated);
			return;
		}
		this.synchValues.SendRPC("RemoteActivateGhost", RpcTarget.MasterClient, Array.Empty<object>());
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x000338CB File Offset: 0x00031ACB
	private void StartChasing()
	{
		if (!this.IsMine())
		{
			return;
		}
		this.ChangeState(SecondLookSkeleton.GhostState.Chasing);
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0007EAC8 File Offset: 0x0007CCC8
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

	// Token: 0x060004C6 RID: 1222 RVA: 0x000338DD File Offset: 0x00031ADD
	public void RemoteActivateGhost()
	{
		if (this.IsMine() && this.currentState == SecondLookSkeleton.GhostState.Unactivated)
		{
			this.ActivateGhost();
		}
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x000338F5 File Offset: 0x00031AF5
	public void RemotePlayerSeen(NetPlayer player)
	{
		if (this.IsMine() && !this.playersSeen.Contains(player))
		{
			this.playersSeen.Add(player);
		}
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0007EB3C File Offset: 0x0007CD3C
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

	// Token: 0x060004C9 RID: 1225 RVA: 0x0007EB88 File Offset: 0x0007CD88
	private bool IsCurrentlyLooking()
	{
		return Vector3.Dot(this.playerTransform.forward, -this.spookyGhost.transform.forward) > 0f && (this.spookyGhost.transform.position - this.playerTransform.position).magnitude < this.ghostActivationDistance && this.CanSeePlayer();
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00033919 File Offset: 0x00031B19
	private void PatrolMove()
	{
		this.GhostMove(this.nextNode.transform, this.patrolSpeed);
		this.SetHeightOffset();
		this.CheckReachedNextNode(false, false);
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0007EBFC File Offset: 0x0007CDFC
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

	// Token: 0x060004CC RID: 1228 RVA: 0x00033940 File Offset: 0x00031B40
	private void ChaseMove()
	{
		this.GhostMove(this.nextNode.transform, this.chaseSpeed);
		this.SetHeightOffset();
		this.CheckReachedNextNode(false, true);
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x00033967 File Offset: 0x00031B67
	private void CaughtMove()
	{
		this.GhostMove(this.nextNode.transform, this.caughtSpeed);
		this.CheckReachedNextNode(true, false);
		this.SyncNodes();
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0007EFC8 File Offset: 0x0007D1C8
	private void SyncNodes()
	{
		this.synchValues.currentNode = this.pathPoints.IndexOfRef(this.currentNode);
		this.synchValues.nextNode = this.pathPoints.IndexOfRef(this.nextNode);
		this.synchValues.angerPoint = this.angerPointIndex;
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0007F020 File Offset: 0x0007D220
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

	// Token: 0x060004D0 RID: 1232 RVA: 0x0007F094 File Offset: 0x0007D294
	private bool GhostAtExit()
	{
		return this.currentNode.distanceToExitNode == 0f && (this.spookyGhost.transform.position - this.currentNode.transform.position).magnitude < this.reachNodeDist;
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0007F0EC File Offset: 0x0007D2EC
	private void GhostMove(Transform target, float speed)
	{
		this.spookyGhost.transform.rotation = Quaternion.RotateTowards(this.spookyGhost.transform.rotation, Quaternion.LookRotation(target.position - this.spookyGhost.transform.position, Vector3.up), this.maxRotSpeed * Time.deltaTime);
		this.spookyGhost.transform.position += (target.position - this.spookyGhost.transform.position).normalized * speed * Time.deltaTime;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x0003398E File Offset: 0x00031B8E
	private void DeactivateGhost()
	{
		this.ChangeState(SecondLookSkeleton.GhostState.PlayerThrown);
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0007F1A0 File Offset: 0x0007D3A0
	private bool CanGrab()
	{
		return (this.spookyGhost.transform.position - this.playerTransform.position).magnitude < this.catchDistance;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00033997 File Offset: 0x00031B97
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

	// Token: 0x060004D5 RID: 1237 RVA: 0x0007F1E0 File Offset: 0x0007D3E0
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

	// Token: 0x060004D6 RID: 1238 RVA: 0x0007F330 File Offset: 0x0007D530
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

	// Token: 0x060004D7 RID: 1239 RVA: 0x0007F3E4 File Offset: 0x0007D5E4
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

	// Token: 0x060004D8 RID: 1240 RVA: 0x000339CE File Offset: 0x00031BCE
	private bool IsMine()
	{
		return !NetworkSystem.Instance.InRoom || this.synchValues.IsMine;
	}

	// Token: 0x04000560 RID: 1376
	public Transform[] angerPoint;

	// Token: 0x04000561 RID: 1377
	public int angerPointIndex;

	// Token: 0x04000562 RID: 1378
	public SkeletonPathingNode[] pathPoints;

	// Token: 0x04000563 RID: 1379
	public SkeletonPathingNode[] exitPoints;

	// Token: 0x04000564 RID: 1380
	public Transform heightOffset;

	// Token: 0x04000565 RID: 1381
	public bool requireSecondLookToActivate;

	// Token: 0x04000566 RID: 1382
	public bool requireTappingToActivate;

	// Token: 0x04000567 RID: 1383
	public bool changeAngerPointOnTimeInterval;

	// Token: 0x04000568 RID: 1384
	public float changeAngerPointTimeMinutes = 3f;

	// Token: 0x04000569 RID: 1385
	private bool firstLookActivated;

	// Token: 0x0400056A RID: 1386
	private bool lookedAway;

	// Token: 0x0400056B RID: 1387
	private bool currentlyLooking;

	// Token: 0x0400056C RID: 1388
	public float ghostActivationDistance;

	// Token: 0x0400056D RID: 1389
	public GameObject spookyGhost;

	// Token: 0x0400056E RID: 1390
	public float timeFirstAppeared;

	// Token: 0x0400056F RID: 1391
	public float timeToFirstDisappear;

	// Token: 0x04000570 RID: 1392
	public SecondLookSkeleton.GhostState currentState;

	// Token: 0x04000571 RID: 1393
	public GameObject spookyText;

	// Token: 0x04000572 RID: 1394
	public float patrolSpeed;

	// Token: 0x04000573 RID: 1395
	public float chaseSpeed;

	// Token: 0x04000574 RID: 1396
	public float caughtSpeed;

	// Token: 0x04000575 RID: 1397
	public SkeletonPathingNode firstNode;

	// Token: 0x04000576 RID: 1398
	public SkeletonPathingNode currentNode;

	// Token: 0x04000577 RID: 1399
	public SkeletonPathingNode nextNode;

	// Token: 0x04000578 RID: 1400
	public Transform lookSource;

	// Token: 0x04000579 RID: 1401
	private Transform playerTransform;

	// Token: 0x0400057A RID: 1402
	public float reachNodeDist;

	// Token: 0x0400057B RID: 1403
	public float maxRotSpeed;

	// Token: 0x0400057C RID: 1404
	public float hapticStrength;

	// Token: 0x0400057D RID: 1405
	public float hapticDuration;

	// Token: 0x0400057E RID: 1406
	public Vector3 offsetGrabPosition;

	// Token: 0x0400057F RID: 1407
	public float throwForce;

	// Token: 0x04000580 RID: 1408
	public Animator animator;

	// Token: 0x04000581 RID: 1409
	public float bodyHeightOffset;

	// Token: 0x04000582 RID: 1410
	private float timeThrown;

	// Token: 0x04000583 RID: 1411
	public float timeThrownCooldown = 1f;

	// Token: 0x04000584 RID: 1412
	public float catchDistance;

	// Token: 0x04000585 RID: 1413
	public float maxSeeDistance;

	// Token: 0x04000586 RID: 1414
	private RaycastHit[] rHits;

	// Token: 0x04000587 RID: 1415
	public LayerMask mask;

	// Token: 0x04000588 RID: 1416
	public LayerMask playerMask;

	// Token: 0x04000589 RID: 1417
	public AudioSource audioSource;

	// Token: 0x0400058A RID: 1418
	public AudioClip initialScream;

	// Token: 0x0400058B RID: 1419
	public AudioClip patrolLoop;

	// Token: 0x0400058C RID: 1420
	public AudioClip chaseLoop;

	// Token: 0x0400058D RID: 1421
	public AudioClip grabbedSound;

	// Token: 0x0400058E RID: 1422
	public AudioClip carryingLoop;

	// Token: 0x0400058F RID: 1423
	public AudioClip throwSound;

	// Token: 0x04000590 RID: 1424
	public List<SkeletonPathingNode> resetChaseHistory = new List<SkeletonPathingNode>();

	// Token: 0x04000591 RID: 1425
	private SecondLookSkeletonSynchValues synchValues;

	// Token: 0x04000592 RID: 1426
	private bool localCaught;

	// Token: 0x04000593 RID: 1427
	private bool localThrown;

	// Token: 0x04000594 RID: 1428
	public List<NetPlayer> playersSeen;

	// Token: 0x04000595 RID: 1429
	public bool tapped;

	// Token: 0x04000596 RID: 1430
	private RaycastHit closest;

	// Token: 0x04000597 RID: 1431
	private float angerPointChangedTime;

	// Token: 0x020000BD RID: 189
	public enum GhostState
	{
		// Token: 0x04000599 RID: 1433
		Unactivated,
		// Token: 0x0400059A RID: 1434
		Activated,
		// Token: 0x0400059B RID: 1435
		Patrolling,
		// Token: 0x0400059C RID: 1436
		Chasing,
		// Token: 0x0400059D RID: 1437
		CaughtPlayer,
		// Token: 0x0400059E RID: 1438
		PlayerThrown,
		// Token: 0x0400059F RID: 1439
		Reset
	}
}
