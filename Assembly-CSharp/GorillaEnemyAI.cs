using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000654 RID: 1620
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class GorillaEnemyAI : MonoBehaviourPun, IPunObservable, IInRoomCallbacks
{
	// Token: 0x06002823 RID: 10275 RVA: 0x0010FD68 File Offset: 0x0010DF68
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

	// Token: 0x06002824 RID: 10276 RVA: 0x0010FDC0 File Offset: 0x0010DFC0
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

	// Token: 0x06002825 RID: 10277 RVA: 0x0010FE24 File Offset: 0x0010E024
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

	// Token: 0x06002826 RID: 10278 RVA: 0x0010FF14 File Offset: 0x0010E114
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

	// Token: 0x06002827 RID: 10279 RVA: 0x0004B560 File Offset: 0x00049760
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 19)
		{
			PhotonNetwork.Destroy(base.photonView);
		}
	}

	// Token: 0x06002828 RID: 10280 RVA: 0x0004B57C File Offset: 0x0004977C
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			this.agent.enabled = true;
			this.r.isKinematic = false;
		}
	}

	// Token: 0x06002829 RID: 10281 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x0600282A RID: 10282 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer)
	{
	}

	// Token: 0x0600282B RID: 10283 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x0600282C RID: 10284 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
	}

	// Token: 0x04002D69 RID: 11625
	public Transform playerTransform;

	// Token: 0x04002D6A RID: 11626
	private NavMeshAgent agent;

	// Token: 0x04002D6B RID: 11627
	private Rigidbody r;

	// Token: 0x04002D6C RID: 11628
	private Vector3 targetPosition;

	// Token: 0x04002D6D RID: 11629
	private Vector3 targetRotation;

	// Token: 0x04002D6E RID: 11630
	public float lerpValue;
}
