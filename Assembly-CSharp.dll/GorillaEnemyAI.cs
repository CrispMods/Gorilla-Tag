﻿using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200068B RID: 1675
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class GorillaEnemyAI : MonoBehaviourPun, IPunObservable, IInRoomCallbacks
{
	// Token: 0x060029AC RID: 10668 RVA: 0x00116960 File Offset: 0x00114B60
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.r = base.GetComponent<Rigidbody>();
		this.r.useGravity = true;
		if (!base.photonView.IsMine)
		{
			this.agent.enabled = false;
			this.r.isKinematic = true;
		}
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x001169B8 File Offset: 0x00114BB8
	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.eulerAngles);
			return;
		}
		this.targetPosition = (Vector3)stream.ReceiveNext();
		this.targetRotation = (Vector3)stream.ReceiveNext();
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x00116A1C File Offset: 0x00114C1C
	private void Update()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.FindClosestPlayer();
			if (this.playerTransform != null)
			{
				this.agent.destination = this.playerTransform.position;
			}
			base.transform.LookAt(new Vector3(this.playerTransform.transform.position.x, base.transform.position.y, this.playerTransform.position.z));
			this.r.velocity *= 0.99f;
			return;
		}
		base.transform.position = Vector3.Lerp(base.transform.position, this.targetPosition, this.lerpValue);
		base.transform.eulerAngles = Vector3.Lerp(base.transform.eulerAngles, this.targetRotation, this.lerpValue);
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x00116B0C File Offset: 0x00114D0C
	private void FindClosestPlayer()
	{
		VRRig[] array = UnityEngine.Object.FindObjectsOfType<VRRig>();
		VRRig vrrig = null;
		float num = 100000f;
		foreach (VRRig vrrig2 in array)
		{
			Vector3 vector = vrrig2.transform.position - base.transform.position;
			if (vector.magnitude < num)
			{
				vrrig = vrrig2;
				num = vector.magnitude;
			}
		}
		this.playerTransform = vrrig.transform;
	}

	// Token: 0x060029B0 RID: 10672 RVA: 0x0004B65B File Offset: 0x0004985B
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 19)
		{
			PhotonNetwork.Destroy(base.photonView);
		}
	}

	// Token: 0x060029B1 RID: 10673 RVA: 0x0004B677 File Offset: 0x00049877
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.agent.enabled = true;
			this.r.isKinematic = false;
		}
	}

	// Token: 0x060029B2 RID: 10674 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x060029B3 RID: 10675 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer)
	{
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x0002F75F File Offset: 0x0002D95F
	void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
	}

	// Token: 0x04002EFF RID: 12031
	public Transform playerTransform;

	// Token: 0x04002F00 RID: 12032
	private NavMeshAgent agent;

	// Token: 0x04002F01 RID: 12033
	private Rigidbody r;

	// Token: 0x04002F02 RID: 12034
	private Vector3 targetPosition;

	// Token: 0x04002F03 RID: 12035
	private Vector3 targetRotation;

	// Token: 0x04002F04 RID: 12036
	public float lerpValue;
}
