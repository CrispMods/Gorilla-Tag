using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006AE RID: 1710
[NetworkBehaviourWeaved(1)]
public class WanderingGhost : NetworkComponent
{
	// Token: 0x06002A85 RID: 10885 RVA: 0x0011CD28 File Offset: 0x0011AF28
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

	// Token: 0x06002A86 RID: 10886 RVA: 0x0004CBF7 File Offset: 0x0004ADF7
	private void DelayedStart()
	{
		this.PickNextWaypoint();
		base.transform.position = this.currentWaypoint._transform.position;
		this.PickNextWaypoint();
		this.ChangeState(WanderingGhost.ghostState.patrol);
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x0011CD90 File Offset: 0x0011AF90
	private void LateUpdate()
	{
		this.UpdateState();
		this.hoverVelocity -= this.mrenderer.transform.localPosition * this.hoverRectifyForce * Time.deltaTime;
		this.hoverVelocity += UnityEngine.Random.insideUnitSphere * this.hoverRandomForce * Time.deltaTime;
		this.hoverVelocity = Vector3.MoveTowards(this.hoverVelocity, Vector3.zero, this.hoverDrag * Time.deltaTime);
		this.mrenderer.transform.localPosition += this.hoverVelocity * Time.deltaTime;
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x0011CE54 File Offset: 0x0011B054
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
		int index = UnityEngine.Random.Range(0, this.waypoints.Count);
		this.currentWaypoint = this.waypoints[index];
		this.waypoints.RemoveAt(index);
	}

	// Token: 0x06002A89 RID: 10889 RVA: 0x0011CF64 File Offset: 0x0011B164
	private void Patrol()
	{
		this.idlePassedTime = 0f;
		this.mrenderer.sharedMaterial = this.scryableMaterial;
		Transform transform = this.currentWaypoint._transform;
		base.transform.position = Vector3.MoveTowards(base.transform.position, transform.position, this.patrolSpeed * Time.deltaTime);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(transform.position - base.transform.position), 360f * Time.deltaTime);
	}

	// Token: 0x06002A8A RID: 10890 RVA: 0x0011D008 File Offset: 0x0011B208
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

	// Token: 0x06002A8B RID: 10891 RVA: 0x0011D074 File Offset: 0x0011B274
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

	// Token: 0x06002A8C RID: 10892 RVA: 0x0011D130 File Offset: 0x0011B330
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

	// Token: 0x06002A8D RID: 10893 RVA: 0x0011D1F4 File Offset: 0x0011B3F4
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

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x06002A8E RID: 10894 RVA: 0x0004CC27 File Offset: 0x0004AE27
	// (set) Token: 0x06002A8F RID: 10895 RVA: 0x0004CC51 File Offset: 0x0004AE51
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

	// Token: 0x06002A90 RID: 10896 RVA: 0x0004CC7C File Offset: 0x0004AE7C
	public override void WriteDataFusion()
	{
		this.Data = this.currentState;
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x0004CC8A File Offset: 0x0004AE8A
	public override void ReadDataFusion()
	{
		this.ReadDataShared(this.Data);
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x0004CC98 File Offset: 0x0004AE98
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		stream.SendNext(this.currentState);
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x0011D258 File Offset: 0x0011B458
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		WanderingGhost.ghostState state = (WanderingGhost.ghostState)stream.ReceiveNext();
		this.ReadDataShared(state);
	}

	// Token: 0x06002A94 RID: 10900 RVA: 0x0004CCB9 File Offset: 0x0004AEB9
	private void ReadDataShared(WanderingGhost.ghostState state)
	{
		WanderingGhost.ghostState ghostState = this.currentState;
		this.currentState = state;
		if (ghostState != this.currentState)
		{
			this.ChangeState(this.currentState);
		}
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x0004CCDC File Offset: 0x0004AEDC
	public override void OnOwnerChange(Player newOwner, Player previousOwner)
	{
		base.OnOwnerChange(newOwner, previousOwner);
		if (newOwner == PhotonNetwork.LocalPlayer)
		{
			this.ChangeState(this.currentState);
		}
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x0011D288 File Offset: 0x0011B488
	private void SpawnFlowerNearby()
	{
		Vector3 position = base.transform.position + Vector3.down * 0.25f;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(base.transform.position + UnityEngine.Random.insideUnitCircle.x0y() * this.flowerSpawnRadius, Vector3.down), out raycastHit, 3f, this.flowerGroundMask))
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
				if (UnityEngine.Random.Range(0, num) == 0)
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

	// Token: 0x06002A98 RID: 10904 RVA: 0x0004CCFA File Offset: 0x0004AEFA
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x0004CD12 File Offset: 0x0004AF12
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04003003 RID: 12291
	public float patrolSpeed = 3f;

	// Token: 0x04003004 RID: 12292
	public float idleStayDuration = 5f;

	// Token: 0x04003005 RID: 12293
	public float sphereColliderRadius = 2f;

	// Token: 0x04003006 RID: 12294
	public ThrowableSetDressing[] allFlowers;

	// Token: 0x04003007 RID: 12295
	public Vector3 flowerDisabledPosition;

	// Token: 0x04003008 RID: 12296
	public float flowerSpawnRadius;

	// Token: 0x04003009 RID: 12297
	public float flowerSpawnDuration;

	// Token: 0x0400300A RID: 12298
	public LayerMask flowerGroundMask;

	// Token: 0x0400300B RID: 12299
	public MeshRenderer mrenderer;

	// Token: 0x0400300C RID: 12300
	public Material visibleMaterial;

	// Token: 0x0400300D RID: 12301
	public Material scryableMaterial;

	// Token: 0x0400300E RID: 12302
	public GameObject waypointsContainer;

	// Token: 0x0400300F RID: 12303
	private ZoneBasedObject[] waypointRegions;

	// Token: 0x04003010 RID: 12304
	private ZoneBasedObject lastWaypointRegion;

	// Token: 0x04003011 RID: 12305
	private List<WanderingGhost.Waypoint> waypoints = new List<WanderingGhost.Waypoint>();

	// Token: 0x04003012 RID: 12306
	private WanderingGhost.Waypoint currentWaypoint;

	// Token: 0x04003013 RID: 12307
	public string debugForceWaypointRegion;

	// Token: 0x04003014 RID: 12308
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04003015 RID: 12309
	public AudioClip[] appearAudio;

	// Token: 0x04003016 RID: 12310
	public float idleVolume;

	// Token: 0x04003017 RID: 12311
	public AudioClip patrolAudio;

	// Token: 0x04003018 RID: 12312
	public float patrolVolume;

	// Token: 0x04003019 RID: 12313
	private WanderingGhost.ghostState currentState;

	// Token: 0x0400301A RID: 12314
	private float idlePassedTime;

	// Token: 0x0400301B RID: 12315
	public UnityAction<GameObject> TriggerHauntedObjects;

	// Token: 0x0400301C RID: 12316
	private Vector3 hoverVelocity;

	// Token: 0x0400301D RID: 12317
	public float hoverRectifyForce;

	// Token: 0x0400301E RID: 12318
	public float hoverRandomForce;

	// Token: 0x0400301F RID: 12319
	public float hoverDrag;

	// Token: 0x04003020 RID: 12320
	private const int maxColliders = 10;

	// Token: 0x04003021 RID: 12321
	private Collider[] hitColliders = new Collider[10];

	// Token: 0x04003022 RID: 12322
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private WanderingGhost.ghostState _Data;

	// Token: 0x020006AF RID: 1711
	[Serializable]
	public struct Waypoint
	{
		// Token: 0x06002A9A RID: 10906 RVA: 0x0004CD26 File Offset: 0x0004AF26
		public Waypoint(bool visible, Transform tr)
		{
			this._visible = visible;
			this._transform = tr;
		}

		// Token: 0x04003023 RID: 12323
		[Tooltip("The ghost will be visible when its reached to this waypoint")]
		public bool _visible;

		// Token: 0x04003024 RID: 12324
		public Transform _transform;
	}

	// Token: 0x020006B0 RID: 1712
	private enum ghostState
	{
		// Token: 0x04003026 RID: 12326
		patrol,
		// Token: 0x04003027 RID: 12327
		idle
	}
}
