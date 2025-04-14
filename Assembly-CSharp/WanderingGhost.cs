using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000699 RID: 1689
[NetworkBehaviourWeaved(1)]
public class WanderingGhost : NetworkComponent
{
	// Token: 0x060029EF RID: 10735 RVA: 0x000D0500 File Offset: 0x000CE700
	protected override void Start()
	{
		base.Start();
		this.waypointRegions = this.waypointsContainer.GetComponentsInChildren<ZoneBasedObject>();
		this.idlePassedTime = 0f;
		ThrowableSetDressing[] array = this.allFlowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].anchor.position = this.flowerDisabledPosition;
		}
		base.Invoke("DelayedStart", 0.5f);
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x000D0567 File Offset: 0x000CE767
	private void DelayedStart()
	{
		this.PickNextWaypoint();
		base.transform.position = this.currentWaypoint._transform.position;
		this.PickNextWaypoint();
		this.ChangeState(WanderingGhost.ghostState.patrol);
	}

	// Token: 0x060029F1 RID: 10737 RVA: 0x000D0598 File Offset: 0x000CE798
	private void LateUpdate()
	{
		this.UpdateState();
		this.hoverVelocity -= this.mrenderer.transform.localPosition * this.hoverRectifyForce * Time.deltaTime;
		this.hoverVelocity += Random.insideUnitSphere * this.hoverRandomForce * Time.deltaTime;
		this.hoverVelocity = Vector3.MoveTowards(this.hoverVelocity, Vector3.zero, this.hoverDrag * Time.deltaTime);
		this.mrenderer.transform.localPosition += this.hoverVelocity * Time.deltaTime;
	}

	// Token: 0x060029F2 RID: 10738 RVA: 0x000D065C File Offset: 0x000CE85C
	private void PickNextWaypoint()
	{
		if (this.waypoints.Count == 0 || this.lastWaypointRegion == null || !this.lastWaypointRegion.IsLocalPlayerInZone())
		{
			ZoneBasedObject zoneBasedObject = ZoneBasedObject.SelectRandomEligible(this.waypointRegions, this.debugForceWaypointRegion);
			if (zoneBasedObject == null)
			{
				zoneBasedObject = this.lastWaypointRegion;
			}
			if (zoneBasedObject == null)
			{
				return;
			}
			this.lastWaypointRegion = zoneBasedObject;
			this.waypoints.Clear();
			foreach (object obj in zoneBasedObject.transform)
			{
				Transform transform = (Transform)obj;
				this.waypoints.Add(new WanderingGhost.Waypoint(transform.name.Contains("_v_"), transform));
			}
		}
		int index = Random.Range(0, this.waypoints.Count);
		this.currentWaypoint = this.waypoints[index];
		this.waypoints.RemoveAt(index);
	}

	// Token: 0x060029F3 RID: 10739 RVA: 0x000D076C File Offset: 0x000CE96C
	private void Patrol()
	{
		this.idlePassedTime = 0f;
		this.mrenderer.sharedMaterial = this.scryableMaterial;
		Transform transform = this.currentWaypoint._transform;
		base.transform.position = Vector3.MoveTowards(base.transform.position, transform.position, this.patrolSpeed * Time.deltaTime);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(transform.position - base.transform.position), 360f * Time.deltaTime);
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x000D0810 File Offset: 0x000CEA10
	private bool MaybeHideGhost()
	{
		int num = Physics.OverlapSphereNonAlloc(base.transform.position, this.sphereColliderRadius, this.hitColliders);
		for (int i = 0; i < num; i++)
		{
			if (this.hitColliders[i].gameObject.IsOnLayer(UnityLayer.GorillaHand) || this.hitColliders[i].gameObject.IsOnLayer(UnityLayer.GorillaBodyCollider))
			{
				this.ChangeState(WanderingGhost.ghostState.patrol);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060029F5 RID: 10741 RVA: 0x000D087C File Offset: 0x000CEA7C
	private void ChangeState(WanderingGhost.ghostState newState)
	{
		this.currentState = newState;
		this.mrenderer.sharedMaterial = ((newState == WanderingGhost.ghostState.idle) ? this.visibleMaterial : this.scryableMaterial);
		if (newState == WanderingGhost.ghostState.patrol)
		{
			this.audioSource.GTStop();
			this.audioSource.volume = this.patrolVolume;
			this.audioSource.clip = this.patrolAudio;
			this.audioSource.GTPlay();
			return;
		}
		if (newState != WanderingGhost.ghostState.idle)
		{
			return;
		}
		this.audioSource.GTStop();
		this.audioSource.volume = this.idleVolume;
		this.audioSource.GTPlayOneShot(this.appearAudio.GetRandomItem<AudioClip>(), 1f);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.SpawnFlowerNearby();
		}
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x000D0938 File Offset: 0x000CEB38
	private void UpdateState()
	{
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		WanderingGhost.ghostState ghostState = this.currentState;
		if (ghostState != WanderingGhost.ghostState.patrol)
		{
			if (ghostState != WanderingGhost.ghostState.idle)
			{
				return;
			}
			this.idlePassedTime += Time.deltaTime;
			if (this.idlePassedTime >= this.idleStayDuration || this.MaybeHideGhost())
			{
				this.PickNextWaypoint();
				this.ChangeState(WanderingGhost.ghostState.patrol);
			}
		}
		else
		{
			if (this.currentWaypoint._transform == null)
			{
				this.PickNextWaypoint();
				return;
			}
			this.Patrol();
			if (Vector3.Distance(base.transform.position, this.currentWaypoint._transform.position) < 0.2f)
			{
				if (this.currentWaypoint._visible)
				{
					this.ChangeState(WanderingGhost.ghostState.idle);
					return;
				}
				this.PickNextWaypoint();
				return;
			}
		}
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x000D09FC File Offset: 0x000CEBFC
	private void HauntObjects()
	{
		Collider[] array = new Collider[20];
		int num = Physics.OverlapSphereNonAlloc(base.transform.position, this.sphereColliderRadius, array);
		for (int i = 0; i < num; i++)
		{
			if (array[i].CompareTag("HauntedObject"))
			{
				UnityAction<GameObject> triggerHauntedObjects = this.TriggerHauntedObjects;
				if (triggerHauntedObjects != null)
				{
					triggerHauntedObjects(array[i].gameObject);
				}
			}
		}
	}

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x060029F8 RID: 10744 RVA: 0x000D0A5D File Offset: 0x000CEC5D
	// (set) Token: 0x060029F9 RID: 10745 RVA: 0x000D0A87 File Offset: 0x000CEC87
	[Networked]
	[NetworkedWeaved(0, 1)]
	private unsafe WanderingGhost.ghostState Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing WanderingGhost.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return (WanderingGhost.ghostState)this.Ptr[0];
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing WanderingGhost.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			this.Ptr[0] = (int)value;
		}
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x000D0AB2 File Offset: 0x000CECB2
	public override void WriteDataFusion()
	{
		this.Data = this.currentState;
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x000D0AC0 File Offset: 0x000CECC0
	public override void ReadDataFusion()
	{
		this.ReadDataShared(this.Data);
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x000D0ACE File Offset: 0x000CECCE
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		stream.SendNext(this.currentState);
	}

	// Token: 0x060029FD RID: 10749 RVA: 0x000D0AF0 File Offset: 0x000CECF0
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		WanderingGhost.ghostState state = (WanderingGhost.ghostState)stream.ReceiveNext();
		this.ReadDataShared(state);
	}

	// Token: 0x060029FE RID: 10750 RVA: 0x000D0B1E File Offset: 0x000CED1E
	private void ReadDataShared(WanderingGhost.ghostState state)
	{
		WanderingGhost.ghostState ghostState = this.currentState;
		this.currentState = state;
		if (ghostState != this.currentState)
		{
			this.ChangeState(this.currentState);
		}
	}

	// Token: 0x060029FF RID: 10751 RVA: 0x000D0B41 File Offset: 0x000CED41
	public override void OnOwnerChange(Player newOwner, Player previousOwner)
	{
		base.OnOwnerChange(newOwner, previousOwner);
		if (newOwner == PhotonNetwork.LocalPlayer)
		{
			this.ChangeState(this.currentState);
		}
	}

	// Token: 0x06002A00 RID: 10752 RVA: 0x000D0B60 File Offset: 0x000CED60
	private void SpawnFlowerNearby()
	{
		Vector3 position = base.transform.position + Vector3.down * 0.25f;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(base.transform.position + Random.insideUnitCircle.x0y() * this.flowerSpawnRadius, Vector3.down), out raycastHit, 3f, this.flowerGroundMask))
		{
			position = raycastHit.point;
		}
		ThrowableSetDressing throwableSetDressing = null;
		int num = 0;
		foreach (ThrowableSetDressing throwableSetDressing2 in this.allFlowers)
		{
			if (!throwableSetDressing2.InHand())
			{
				num++;
				if (Random.Range(0, num) == 0)
				{
					throwableSetDressing = throwableSetDressing2;
				}
			}
		}
		if (throwableSetDressing != null)
		{
			if (!throwableSetDressing.IsLocalOwnedWorldShareable)
			{
				throwableSetDressing.WorldShareableRequestOwnership();
			}
			throwableSetDressing.SetWillTeleport();
			throwableSetDressing.transform.position = position;
			throwableSetDressing.StartRespawnTimer(this.flowerSpawnDuration);
		}
	}

	// Token: 0x06002A02 RID: 10754 RVA: 0x000D0CA0 File Offset: 0x000CEEA0
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06002A03 RID: 10755 RVA: 0x000D0CB8 File Offset: 0x000CEEB8
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04002F66 RID: 12134
	public float patrolSpeed = 3f;

	// Token: 0x04002F67 RID: 12135
	public float idleStayDuration = 5f;

	// Token: 0x04002F68 RID: 12136
	public float sphereColliderRadius = 2f;

	// Token: 0x04002F69 RID: 12137
	public ThrowableSetDressing[] allFlowers;

	// Token: 0x04002F6A RID: 12138
	public Vector3 flowerDisabledPosition;

	// Token: 0x04002F6B RID: 12139
	public float flowerSpawnRadius;

	// Token: 0x04002F6C RID: 12140
	public float flowerSpawnDuration;

	// Token: 0x04002F6D RID: 12141
	public LayerMask flowerGroundMask;

	// Token: 0x04002F6E RID: 12142
	public MeshRenderer mrenderer;

	// Token: 0x04002F6F RID: 12143
	public Material visibleMaterial;

	// Token: 0x04002F70 RID: 12144
	public Material scryableMaterial;

	// Token: 0x04002F71 RID: 12145
	public GameObject waypointsContainer;

	// Token: 0x04002F72 RID: 12146
	private ZoneBasedObject[] waypointRegions;

	// Token: 0x04002F73 RID: 12147
	private ZoneBasedObject lastWaypointRegion;

	// Token: 0x04002F74 RID: 12148
	private List<WanderingGhost.Waypoint> waypoints = new List<WanderingGhost.Waypoint>();

	// Token: 0x04002F75 RID: 12149
	private WanderingGhost.Waypoint currentWaypoint;

	// Token: 0x04002F76 RID: 12150
	public string debugForceWaypointRegion;

	// Token: 0x04002F77 RID: 12151
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04002F78 RID: 12152
	public AudioClip[] appearAudio;

	// Token: 0x04002F79 RID: 12153
	public float idleVolume;

	// Token: 0x04002F7A RID: 12154
	public AudioClip patrolAudio;

	// Token: 0x04002F7B RID: 12155
	public float patrolVolume;

	// Token: 0x04002F7C RID: 12156
	private WanderingGhost.ghostState currentState;

	// Token: 0x04002F7D RID: 12157
	private float idlePassedTime;

	// Token: 0x04002F7E RID: 12158
	public UnityAction<GameObject> TriggerHauntedObjects;

	// Token: 0x04002F7F RID: 12159
	private Vector3 hoverVelocity;

	// Token: 0x04002F80 RID: 12160
	public float hoverRectifyForce;

	// Token: 0x04002F81 RID: 12161
	public float hoverRandomForce;

	// Token: 0x04002F82 RID: 12162
	public float hoverDrag;

	// Token: 0x04002F83 RID: 12163
	private const int maxColliders = 10;

	// Token: 0x04002F84 RID: 12164
	private Collider[] hitColliders = new Collider[10];

	// Token: 0x04002F85 RID: 12165
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private WanderingGhost.ghostState _Data;

	// Token: 0x0200069A RID: 1690
	[Serializable]
	public struct Waypoint
	{
		// Token: 0x06002A04 RID: 10756 RVA: 0x000D0CCC File Offset: 0x000CEECC
		public Waypoint(bool visible, Transform tr)
		{
			this._visible = visible;
			this._transform = tr;
		}

		// Token: 0x04002F86 RID: 12166
		[Tooltip("The ghost will be visible when its reached to this waypoint")]
		public bool _visible;

		// Token: 0x04002F87 RID: 12167
		public Transform _transform;
	}

	// Token: 0x0200069B RID: 1691
	private enum ghostState
	{
		// Token: 0x04002F89 RID: 12169
		patrol,
		// Token: 0x04002F8A RID: 12170
		idle
	}
}
