using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200069A RID: 1690
[NetworkBehaviourWeaved(1)]
public class WanderingGhost : NetworkComponent
{
	// Token: 0x060029F7 RID: 10743 RVA: 0x000D0980 File Offset: 0x000CEB80
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

	// Token: 0x060029F8 RID: 10744 RVA: 0x000D09E7 File Offset: 0x000CEBE7
	private void DelayedStart()
	{
		this.PickNextWaypoint();
		base.transform.position = this.currentWaypoint._transform.position;
		this.PickNextWaypoint();
		this.ChangeState(WanderingGhost.ghostState.patrol);
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x000D0A18 File Offset: 0x000CEC18
	private void LateUpdate()
	{
		this.UpdateState();
		this.hoverVelocity -= this.mrenderer.transform.localPosition * this.hoverRectifyForce * Time.deltaTime;
		this.hoverVelocity += Random.insideUnitSphere * this.hoverRandomForce * Time.deltaTime;
		this.hoverVelocity = Vector3.MoveTowards(this.hoverVelocity, Vector3.zero, this.hoverDrag * Time.deltaTime);
		this.mrenderer.transform.localPosition += this.hoverVelocity * Time.deltaTime;
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x000D0ADC File Offset: 0x000CECDC
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

	// Token: 0x060029FB RID: 10747 RVA: 0x000D0BEC File Offset: 0x000CEDEC
	private void Patrol()
	{
		this.idlePassedTime = 0f;
		this.mrenderer.sharedMaterial = this.scryableMaterial;
		Transform transform = this.currentWaypoint._transform;
		base.transform.position = Vector3.MoveTowards(base.transform.position, transform.position, this.patrolSpeed * Time.deltaTime);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(transform.position - base.transform.position), 360f * Time.deltaTime);
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x000D0C90 File Offset: 0x000CEE90
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

	// Token: 0x060029FD RID: 10749 RVA: 0x000D0CFC File Offset: 0x000CEEFC
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

	// Token: 0x060029FE RID: 10750 RVA: 0x000D0DB8 File Offset: 0x000CEFB8
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

	// Token: 0x060029FF RID: 10751 RVA: 0x000D0E7C File Offset: 0x000CF07C
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

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06002A00 RID: 10752 RVA: 0x000D0EDD File Offset: 0x000CF0DD
	// (set) Token: 0x06002A01 RID: 10753 RVA: 0x000D0F07 File Offset: 0x000CF107
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

	// Token: 0x06002A02 RID: 10754 RVA: 0x000D0F32 File Offset: 0x000CF132
	public override void WriteDataFusion()
	{
		this.Data = this.currentState;
	}

	// Token: 0x06002A03 RID: 10755 RVA: 0x000D0F40 File Offset: 0x000CF140
	public override void ReadDataFusion()
	{
		this.ReadDataShared(this.Data);
	}

	// Token: 0x06002A04 RID: 10756 RVA: 0x000D0F4E File Offset: 0x000CF14E
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		stream.SendNext(this.currentState);
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x000D0F70 File Offset: 0x000CF170
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		WanderingGhost.ghostState state = (WanderingGhost.ghostState)stream.ReceiveNext();
		this.ReadDataShared(state);
	}

	// Token: 0x06002A06 RID: 10758 RVA: 0x000D0F9E File Offset: 0x000CF19E
	private void ReadDataShared(WanderingGhost.ghostState state)
	{
		WanderingGhost.ghostState ghostState = this.currentState;
		this.currentState = state;
		if (ghostState != this.currentState)
		{
			this.ChangeState(this.currentState);
		}
	}

	// Token: 0x06002A07 RID: 10759 RVA: 0x000D0FC1 File Offset: 0x000CF1C1
	public override void OnOwnerChange(Player newOwner, Player previousOwner)
	{
		base.OnOwnerChange(newOwner, previousOwner);
		if (newOwner == PhotonNetwork.LocalPlayer)
		{
			this.ChangeState(this.currentState);
		}
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x000D0FE0 File Offset: 0x000CF1E0
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

	// Token: 0x06002A0A RID: 10762 RVA: 0x000D1120 File Offset: 0x000CF320
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06002A0B RID: 10763 RVA: 0x000D1138 File Offset: 0x000CF338
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04002F6C RID: 12140
	public float patrolSpeed = 3f;

	// Token: 0x04002F6D RID: 12141
	public float idleStayDuration = 5f;

	// Token: 0x04002F6E RID: 12142
	public float sphereColliderRadius = 2f;

	// Token: 0x04002F6F RID: 12143
	public ThrowableSetDressing[] allFlowers;

	// Token: 0x04002F70 RID: 12144
	public Vector3 flowerDisabledPosition;

	// Token: 0x04002F71 RID: 12145
	public float flowerSpawnRadius;

	// Token: 0x04002F72 RID: 12146
	public float flowerSpawnDuration;

	// Token: 0x04002F73 RID: 12147
	public LayerMask flowerGroundMask;

	// Token: 0x04002F74 RID: 12148
	public MeshRenderer mrenderer;

	// Token: 0x04002F75 RID: 12149
	public Material visibleMaterial;

	// Token: 0x04002F76 RID: 12150
	public Material scryableMaterial;

	// Token: 0x04002F77 RID: 12151
	public GameObject waypointsContainer;

	// Token: 0x04002F78 RID: 12152
	private ZoneBasedObject[] waypointRegions;

	// Token: 0x04002F79 RID: 12153
	private ZoneBasedObject lastWaypointRegion;

	// Token: 0x04002F7A RID: 12154
	private List<WanderingGhost.Waypoint> waypoints = new List<WanderingGhost.Waypoint>();

	// Token: 0x04002F7B RID: 12155
	private WanderingGhost.Waypoint currentWaypoint;

	// Token: 0x04002F7C RID: 12156
	public string debugForceWaypointRegion;

	// Token: 0x04002F7D RID: 12157
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04002F7E RID: 12158
	public AudioClip[] appearAudio;

	// Token: 0x04002F7F RID: 12159
	public float idleVolume;

	// Token: 0x04002F80 RID: 12160
	public AudioClip patrolAudio;

	// Token: 0x04002F81 RID: 12161
	public float patrolVolume;

	// Token: 0x04002F82 RID: 12162
	private WanderingGhost.ghostState currentState;

	// Token: 0x04002F83 RID: 12163
	private float idlePassedTime;

	// Token: 0x04002F84 RID: 12164
	public UnityAction<GameObject> TriggerHauntedObjects;

	// Token: 0x04002F85 RID: 12165
	private Vector3 hoverVelocity;

	// Token: 0x04002F86 RID: 12166
	public float hoverRectifyForce;

	// Token: 0x04002F87 RID: 12167
	public float hoverRandomForce;

	// Token: 0x04002F88 RID: 12168
	public float hoverDrag;

	// Token: 0x04002F89 RID: 12169
	private const int maxColliders = 10;

	// Token: 0x04002F8A RID: 12170
	private Collider[] hitColliders = new Collider[10];

	// Token: 0x04002F8B RID: 12171
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private WanderingGhost.ghostState _Data;

	// Token: 0x0200069B RID: 1691
	[Serializable]
	public struct Waypoint
	{
		// Token: 0x06002A0C RID: 10764 RVA: 0x000D114C File Offset: 0x000CF34C
		public Waypoint(bool visible, Transform tr)
		{
			this._visible = visible;
			this._transform = tr;
		}

		// Token: 0x04002F8C RID: 12172
		[Tooltip("The ghost will be visible when its reached to this waypoint")]
		public bool _visible;

		// Token: 0x04002F8D RID: 12173
		public Transform _transform;
	}

	// Token: 0x0200069C RID: 1692
	private enum ghostState
	{
		// Token: 0x04002F8F RID: 12175
		patrol,
		// Token: 0x04002F90 RID: 12176
		idle
	}
}
