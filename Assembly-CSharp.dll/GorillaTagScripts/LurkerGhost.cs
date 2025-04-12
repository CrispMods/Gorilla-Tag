using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009CA RID: 2506
	[NetworkBehaviourWeaved(6)]
	public class LurkerGhost : NetworkComponent
	{
		// Token: 0x06003E64 RID: 15972 RVA: 0x00057DCE File Offset: 0x00055FCE
		protected override void Awake()
		{
			base.Awake();
			this.possibleTargets = new List<NetPlayer>();
			this.targetPlayer = null;
			this.targetTransform = null;
			this.targetVRRig = null;
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x00057DF6 File Offset: 0x00055FF6
		protected override void Start()
		{
			base.Start();
			this.waypointRegions = this.waypointsContainer.GetComponentsInChildren<ZoneBasedObject>();
			this.PickNextWaypoint();
			this.ChangeState(LurkerGhost.ghostState.patrol);
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x00057E1C File Offset: 0x0005601C
		private void LateUpdate()
		{
			this.UpdateState();
			this.UpdateGhostVisibility();
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x001627EC File Offset: 0x001609EC
		private void PickNextWaypoint()
		{
			if (this.waypoints.Count == 0 || this.lastWaypointRegion == null || !this.lastWaypointRegion.IsLocalPlayerInZone())
			{
				ZoneBasedObject zoneBasedObject = ZoneBasedObject.SelectRandomEligible(this.waypointRegions, "");
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
					Transform item = (Transform)obj;
					this.waypoints.Add(item);
				}
			}
			int index = UnityEngine.Random.Range(0, this.waypoints.Count);
			this.currentWaypoint = this.waypoints[index];
			this.targetRotation = Quaternion.LookRotation(this.currentWaypoint.position - base.transform.position);
			this.waypoints.RemoveAt(index);
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x0016290C File Offset: 0x00160B0C
		private void Patrol()
		{
			Transform transform = this.currentWaypoint;
			if (transform != null)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, transform.position, this.patrolSpeed * Time.deltaTime);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.targetRotation, 360f * Time.deltaTime);
			}
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x00162984 File Offset: 0x00160B84
		private void PlaySound(AudioClip clip, bool loop)
		{
			if (this.audioSource && this.audioSource.isPlaying)
			{
				this.audioSource.GTStop();
			}
			if (this.audioSource && clip != null)
			{
				this.audioSource.clip = clip;
				this.audioSource.loop = loop;
				this.audioSource.GTPlay();
			}
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x001629F0 File Offset: 0x00160BF0
		private bool PickPlayer(float maxDistance)
		{
			if (base.IsMine)
			{
				this.possibleTargets.Clear();
				for (int i = 0; i < GorillaParent.instance.vrrigs.Count; i++)
				{
					if ((GorillaParent.instance.vrrigs[i].transform.position - base.transform.position).magnitude < maxDistance && GorillaParent.instance.vrrigs[i].creator != this.targetPlayer)
					{
						this.possibleTargets.Add(GorillaParent.instance.vrrigs[i].creator);
					}
				}
				this.targetPlayer = null;
				this.targetTransform = null;
				this.targetVRRig = null;
				if (this.possibleTargets.Count > 0)
				{
					int index = UnityEngine.Random.Range(0, this.possibleTargets.Count);
					this.PickPlayer(this.possibleTargets[index]);
				}
			}
			else
			{
				this.targetPlayer = null;
				this.targetTransform = null;
				this.targetVRRig = null;
			}
			return this.targetPlayer != null && this.targetTransform != null;
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x00162B20 File Offset: 0x00160D20
		private void PickPlayer(NetPlayer player)
		{
			int num = GorillaParent.instance.vrrigs.FindIndex((VRRig x) => x.creator != null && x.creator == player);
			if (num > -1 && num < GorillaParent.instance.vrrigs.Count)
			{
				this.targetPlayer = GorillaParent.instance.vrrigs[num].creator;
				this.targetTransform = GorillaParent.instance.vrrigs[num].head.rigTarget;
				this.targetVRRig = GorillaParent.instance.vrrigs[num];
			}
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x00162BC8 File Offset: 0x00160DC8
		private void SeekPlayer()
		{
			if (this.targetTransform == null)
			{
				this.ChangeState(LurkerGhost.ghostState.patrol);
				return;
			}
			this.targetPosition = this.targetTransform.position + this.targetTransform.forward.x0z() * this.seekAheadDistance;
			this.targetRotation = Quaternion.LookRotation(this.targetTransform.position - base.transform.position);
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetPosition, this.seekSpeed * Time.deltaTime);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.targetRotation, 720f * Time.deltaTime);
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x00162C9C File Offset: 0x00160E9C
		private void ChargeAtPlayer()
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetPosition, this.chargeSpeed * Time.deltaTime);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.targetRotation, 720f * Time.deltaTime);
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x00162D04 File Offset: 0x00160F04
		private void UpdateGhostVisibility()
		{
			switch (this.currentState)
			{
			case LurkerGhost.ghostState.patrol:
				this.meshRenderer.sharedMaterial = this.scryableMaterial;
				this.bonesMeshRenderer.sharedMaterial = this.scryableMaterialBones;
				return;
			case LurkerGhost.ghostState.seek:
			case LurkerGhost.ghostState.charge:
				if (this.targetPlayer == NetworkSystem.Instance.LocalPlayer || this.passingPlayer == NetworkSystem.Instance.LocalPlayer)
				{
					this.meshRenderer.sharedMaterial = this.visibleMaterial;
					this.bonesMeshRenderer.sharedMaterial = this.visibleMaterialBones;
					return;
				}
				this.meshRenderer.sharedMaterial = this.scryableMaterial;
				this.bonesMeshRenderer.sharedMaterial = this.scryableMaterialBones;
				return;
			case LurkerGhost.ghostState.possess:
				if (this.targetPlayer == NetworkSystem.Instance.LocalPlayer || this.passingPlayer == NetworkSystem.Instance.LocalPlayer)
				{
					this.meshRenderer.sharedMaterial = this.visibleMaterial;
					this.bonesMeshRenderer.sharedMaterial = this.visibleMaterialBones;
					return;
				}
				this.meshRenderer.sharedMaterial = this.scryableMaterial;
				this.bonesMeshRenderer.sharedMaterial = this.scryableMaterialBones;
				return;
			default:
				return;
			}
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x00162E28 File Offset: 0x00161028
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

		// Token: 0x06003E70 RID: 15984 RVA: 0x00162E8C File Offset: 0x0016108C
		private void ChangeState(LurkerGhost.ghostState newState)
		{
			this.currentState = newState;
			VRRig vrrig = null;
			switch (this.currentState)
			{
			case LurkerGhost.ghostState.patrol:
				this.PlaySound(this.patrolAudio, true);
				this.passingPlayer = null;
				this.cooldownTimeRemaining = UnityEngine.Random.Range(this.cooldownDuration, this.maxCooldownDuration);
				this.currentRepeatHuntTimes = 0;
				break;
			case LurkerGhost.ghostState.charge:
				this.PlaySound(this.huntAudio, false);
				this.targetPosition = this.targetTransform.position;
				this.targetRotation = Quaternion.LookRotation(this.targetTransform.position - base.transform.position);
				break;
			case LurkerGhost.ghostState.possess:
				if (this.targetPlayer == NetworkSystem.Instance.LocalPlayer)
				{
					this.PlaySound(this.possessedAudio, true);
					GorillaTagger.Instance.StartVibration(true, this.hapticStrength, this.hapticDuration);
					GorillaTagger.Instance.StartVibration(false, this.hapticStrength, this.hapticDuration);
				}
				vrrig = GorillaGameManager.StaticFindRigForPlayer(this.targetPlayer);
				break;
			}
			Shader.SetGlobalFloat(this._BlackAndWhite, (float)((newState == LurkerGhost.ghostState.possess && this.targetPlayer == NetworkSystem.Instance.LocalPlayer) ? 1 : 0));
			if (vrrig != this.lastHauntedVRRig && this.lastHauntedVRRig != null)
			{
				this.lastHauntedVRRig.IsHaunted = false;
			}
			if (vrrig != null)
			{
				vrrig.IsHaunted = true;
			}
			this.lastHauntedVRRig = vrrig;
			this.UpdateGhostVisibility();
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x00057E2A File Offset: 0x0005602A
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			Shader.SetGlobalFloat(this._BlackAndWhite, 0f);
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x0016300C File Offset: 0x0016120C
		private void UpdateState()
		{
			switch (this.currentState)
			{
			case LurkerGhost.ghostState.patrol:
				this.Patrol();
				if (base.IsMine)
				{
					if (this.currentWaypoint == null || Vector3.Distance(base.transform.position, this.currentWaypoint.position) < 0.2f)
					{
						this.PickNextWaypoint();
					}
					this.cooldownTimeRemaining -= Time.deltaTime;
					if (this.cooldownTimeRemaining <= 0f)
					{
						this.cooldownTimeRemaining = 0f;
						if (this.PickPlayer(this.maxHuntDistance))
						{
							this.ChangeState(LurkerGhost.ghostState.seek);
							return;
						}
					}
				}
				break;
			case LurkerGhost.ghostState.seek:
				this.SeekPlayer();
				if (base.IsMine && (this.targetPosition - base.transform.position).sqrMagnitude < this.seekCloseEnoughDistance * this.seekCloseEnoughDistance)
				{
					this.ChangeState(LurkerGhost.ghostState.charge);
					return;
				}
				break;
			case LurkerGhost.ghostState.charge:
				this.ChargeAtPlayer();
				if (base.IsMine && (this.targetPosition - base.transform.position).sqrMagnitude < 0.25f)
				{
					if ((this.targetTransform.position - this.targetPosition).magnitude < this.minCatchDistance)
					{
						this.ChangeState(LurkerGhost.ghostState.possess);
						return;
					}
					this.huntedPassedTime = 0f;
					this.ChangeState(LurkerGhost.ghostState.patrol);
					return;
				}
				break;
			case LurkerGhost.ghostState.possess:
				if (this.targetTransform != null)
				{
					float num = this.SpookyMagicNumbers.x + MathF.Abs(MathF.Sin(Time.time * this.SpookyMagicNumbers.y));
					float num2 = this.HauntedMagicNumbers.x * MathF.Sin(Time.time * this.HauntedMagicNumbers.y) + this.HauntedMagicNumbers.z * MathF.Sin(Time.time * this.HauntedMagicNumbers.w);
					float y = 0.5f + 0.5f * MathF.Sin(Time.time * this.SpookyMagicNumbers.z);
					Vector3 target = this.targetTransform.position + new Vector3(num * (float)Math.Sin((double)num2), y, num * (float)Math.Cos((double)num2));
					base.transform.position = Vector3.MoveTowards(base.transform.position, target, this.chargeSpeed);
					base.transform.rotation = Quaternion.LookRotation(base.transform.position - this.targetTransform.position);
				}
				if (base.IsMine)
				{
					this.huntedPassedTime += Time.deltaTime;
					if (this.huntedPassedTime >= this.PossessionDuration)
					{
						this.huntedPassedTime = 0f;
						if (this.hauntNeighbors && this.currentRepeatHuntTimes < this.maxRepeatHuntTimes && this.PickPlayer(this.maxRepeatHuntDistance))
						{
							this.currentRepeatHuntTimes++;
							this.ChangeState(LurkerGhost.ghostState.seek);
							return;
						}
						this.ChangeState(LurkerGhost.ghostState.patrol);
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06003E73 RID: 15987 RVA: 0x00057E47 File Offset: 0x00056047
		// (set) Token: 0x06003E74 RID: 15988 RVA: 0x00057E71 File Offset: 0x00056071
		[Networked]
		[NetworkedWeaved(0, 6)]
		private unsafe LurkerGhost.LurkerGhostData Data
		{
			get
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LurkerGhost.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(LurkerGhost.LurkerGhostData*)(this.Ptr + 0);
			}
			set
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LurkerGhost.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(LurkerGhost.LurkerGhostData*)(this.Ptr + 0) = value;
			}
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x00057E9C File Offset: 0x0005609C
		public override void WriteDataFusion()
		{
			this.Data = new LurkerGhost.LurkerGhostData(this.currentState, this.currentIndex, this.targetPlayer.ActorNumber, this.targetPosition);
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x0016331C File Offset: 0x0016151C
		public override void ReadDataFusion()
		{
			this.ReadDataShared(this.Data.CurrentState, this.Data.CurrentIndex, this.Data.TargetActor, this.Data.TargetPos);
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x00163368 File Offset: 0x00161568
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			stream.SendNext(this.currentState);
			stream.SendNext(this.currentIndex);
			if (this.targetPlayer != null)
			{
				stream.SendNext(this.targetPlayer.ActorNumber);
			}
			else
			{
				stream.SendNext(-1);
			}
			stream.SendNext(this.targetPosition);
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x001633E4 File Offset: 0x001615E4
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != PhotonNetwork.MasterClient)
			{
				return;
			}
			LurkerGhost.ghostState state = (LurkerGhost.ghostState)stream.ReceiveNext();
			int index = (int)stream.ReceiveNext();
			int targetActorNumber = (int)stream.ReceiveNext();
			Vector3 targetPos = (Vector3)stream.ReceiveNext();
			this.ReadDataShared(state, index, targetActorNumber, targetPos);
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x0016343C File Offset: 0x0016163C
		private void ReadDataShared(LurkerGhost.ghostState state, int index, int targetActorNumber, Vector3 targetPos)
		{
			LurkerGhost.ghostState ghostState = this.currentState;
			this.currentState = state;
			this.currentIndex = index;
			NetPlayer netPlayer = this.targetPlayer;
			this.targetPlayer = NetworkSystem.Instance.GetPlayer(targetActorNumber);
			this.targetPosition = targetPos;
			float num = 10000f;
			if (!this.targetPosition.IsValid(num))
			{
				RigContainer rigContainer;
				if (VRRigCache.Instance.TryGetVrrig(this.targetPlayer, out rigContainer))
				{
					this.targetPosition = (this.targetPlayer.IsLocal ? rigContainer.Rig.transform.position : rigContainer.Rig.syncPos);
				}
				else
				{
					this.targetPosition = base.transform.position;
				}
			}
			if (this.targetPlayer != netPlayer)
			{
				this.PickPlayer(this.targetPlayer);
			}
			if (ghostState != this.currentState || this.targetPlayer != netPlayer)
			{
				this.ChangeState(this.currentState);
			}
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x00057EC6 File Offset: 0x000560C6
		public override void OnOwnerChange(Player newOwner, Player previousOwner)
		{
			base.OnOwnerChange(newOwner, previousOwner);
			if (newOwner == PhotonNetwork.LocalPlayer)
			{
				this.ChangeState(this.currentState);
			}
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x00057EE4 File Offset: 0x000560E4
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x00057EFC File Offset: 0x000560FC
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04003F99 RID: 16281
		public float patrolSpeed = 3f;

		// Token: 0x04003F9A RID: 16282
		public float seekSpeed = 6f;

		// Token: 0x04003F9B RID: 16283
		public float chargeSpeed = 6f;

		// Token: 0x04003F9C RID: 16284
		[Tooltip("Cooldown until the next time the ghost needs to hunt a new player")]
		public float cooldownDuration = 10f;

		// Token: 0x04003F9D RID: 16285
		[Tooltip("Max Cooldown (randomized)")]
		public float maxCooldownDuration = 10f;

		// Token: 0x04003F9E RID: 16286
		[Tooltip("How long the possession effects should last")]
		public float PossessionDuration = 15f;

		// Token: 0x04003F9F RID: 16287
		[Tooltip("Hunted objects within this radius will get triggered ")]
		public float sphereColliderRadius = 2f;

		// Token: 0x04003FA0 RID: 16288
		[Tooltip("Maximum distance to the possible player to get hunted")]
		public float maxHuntDistance = 20f;

		// Token: 0x04003FA1 RID: 16289
		[Tooltip("Minimum distance from the player to start the possession effects")]
		public float minCatchDistance = 2f;

		// Token: 0x04003FA2 RID: 16290
		[Tooltip("Maximum distance to the possible player to get repeat hunted")]
		public float maxRepeatHuntDistance = 5f;

		// Token: 0x04003FA3 RID: 16291
		[Tooltip("Maximum times the lurker can haunt a nearby player before going back on cooldown")]
		public int maxRepeatHuntTimes = 3;

		// Token: 0x04003FA4 RID: 16292
		[Tooltip("Time in seconds before a haunted player can pass the lurker to another player by tagging")]
		public float tagCoolDown = 2f;

		// Token: 0x04003FA5 RID: 16293
		[Tooltip("UP & DOWN, IN & OUT")]
		public Vector3 SpookyMagicNumbers = new Vector3(1f, 1f, 1f);

		// Token: 0x04003FA6 RID: 16294
		[Tooltip("SPIN, SPIN, SPIN, SPIN")]
		public Vector4 HauntedMagicNumbers = new Vector4(1f, 2f, 3f, 1f);

		// Token: 0x04003FA7 RID: 16295
		[Tooltip("Haptic vibration when haunted by the ghost")]
		public float hapticStrength = 1f;

		// Token: 0x04003FA8 RID: 16296
		public float hapticDuration = 1.5f;

		// Token: 0x04003FA9 RID: 16297
		public GameObject waypointsContainer;

		// Token: 0x04003FAA RID: 16298
		private ZoneBasedObject[] waypointRegions;

		// Token: 0x04003FAB RID: 16299
		private ZoneBasedObject lastWaypointRegion;

		// Token: 0x04003FAC RID: 16300
		private List<Transform> waypoints = new List<Transform>();

		// Token: 0x04003FAD RID: 16301
		private Transform currentWaypoint;

		// Token: 0x04003FAE RID: 16302
		public Material visibleMaterial;

		// Token: 0x04003FAF RID: 16303
		public Material scryableMaterial;

		// Token: 0x04003FB0 RID: 16304
		public Material visibleMaterialBones;

		// Token: 0x04003FB1 RID: 16305
		public Material scryableMaterialBones;

		// Token: 0x04003FB2 RID: 16306
		public MeshRenderer meshRenderer;

		// Token: 0x04003FB3 RID: 16307
		public MeshRenderer bonesMeshRenderer;

		// Token: 0x04003FB4 RID: 16308
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003FB5 RID: 16309
		public AudioClip patrolAudio;

		// Token: 0x04003FB6 RID: 16310
		public AudioClip huntAudio;

		// Token: 0x04003FB7 RID: 16311
		public AudioClip possessedAudio;

		// Token: 0x04003FB8 RID: 16312
		public ThrowableSetDressing scryingGlass;

		// Token: 0x04003FB9 RID: 16313
		public float scryingAngerAngle;

		// Token: 0x04003FBA RID: 16314
		public float scryingAngerDelay;

		// Token: 0x04003FBB RID: 16315
		public float seekAheadDistance;

		// Token: 0x04003FBC RID: 16316
		public float seekCloseEnoughDistance;

		// Token: 0x04003FBD RID: 16317
		private float scryingAngerAfterTimestamp;

		// Token: 0x04003FBE RID: 16318
		private int currentRepeatHuntTimes;

		// Token: 0x04003FBF RID: 16319
		public UnityAction<GameObject> TriggerHauntedObjects;

		// Token: 0x04003FC0 RID: 16320
		private int currentIndex;

		// Token: 0x04003FC1 RID: 16321
		private LurkerGhost.ghostState currentState;

		// Token: 0x04003FC2 RID: 16322
		private float cooldownTimeRemaining;

		// Token: 0x04003FC3 RID: 16323
		private List<NetPlayer> possibleTargets;

		// Token: 0x04003FC4 RID: 16324
		private NetPlayer targetPlayer;

		// Token: 0x04003FC5 RID: 16325
		private Transform targetTransform;

		// Token: 0x04003FC6 RID: 16326
		private float huntedPassedTime;

		// Token: 0x04003FC7 RID: 16327
		private Vector3 targetPosition;

		// Token: 0x04003FC8 RID: 16328
		private Quaternion targetRotation;

		// Token: 0x04003FC9 RID: 16329
		private VRRig targetVRRig;

		// Token: 0x04003FCA RID: 16330
		private ShaderHashId _BlackAndWhite = "_BlackAndWhite";

		// Token: 0x04003FCB RID: 16331
		private VRRig lastHauntedVRRig;

		// Token: 0x04003FCC RID: 16332
		private float nextTagTime;

		// Token: 0x04003FCD RID: 16333
		private NetPlayer passingPlayer;

		// Token: 0x04003FCE RID: 16334
		[SerializeField]
		private bool hauntNeighbors = true;

		// Token: 0x04003FCF RID: 16335
		[WeaverGenerated]
		[DefaultForProperty("Data", 0, 6)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private LurkerGhost.LurkerGhostData _Data;

		// Token: 0x020009CB RID: 2507
		private enum ghostState
		{
			// Token: 0x04003FD1 RID: 16337
			patrol,
			// Token: 0x04003FD2 RID: 16338
			seek,
			// Token: 0x04003FD3 RID: 16339
			charge,
			// Token: 0x04003FD4 RID: 16340
			possess
		}

		// Token: 0x020009CC RID: 2508
		[NetworkStructWeaved(6)]
		[StructLayout(LayoutKind.Explicit, Size = 24)]
		private struct LurkerGhostData : INetworkStruct
		{
			// Token: 0x17000651 RID: 1617
			// (get) Token: 0x06003E7E RID: 15998 RVA: 0x00057F10 File Offset: 0x00056110
			// (set) Token: 0x06003E7F RID: 15999 RVA: 0x00057F18 File Offset: 0x00056118
			public LurkerGhost.ghostState CurrentState { readonly get; set; }

			// Token: 0x17000652 RID: 1618
			// (get) Token: 0x06003E80 RID: 16000 RVA: 0x00057F21 File Offset: 0x00056121
			// (set) Token: 0x06003E81 RID: 16001 RVA: 0x00057F29 File Offset: 0x00056129
			public int CurrentIndex { readonly get; set; }

			// Token: 0x17000653 RID: 1619
			// (get) Token: 0x06003E82 RID: 16002 RVA: 0x00057F32 File Offset: 0x00056132
			// (set) Token: 0x06003E83 RID: 16003 RVA: 0x00057F3A File Offset: 0x0005613A
			public int TargetActor { readonly get; set; }

			// Token: 0x17000654 RID: 1620
			// (get) Token: 0x06003E84 RID: 16004 RVA: 0x00057F43 File Offset: 0x00056143
			// (set) Token: 0x06003E85 RID: 16005 RVA: 0x00057F55 File Offset: 0x00056155
			[Networked]
			public unsafe Vector3 TargetPos
			{
				readonly get
				{
					return *(Vector3*)Native.ReferenceToPointer<FixedStorage@3>(ref this._TargetPos);
				}
				set
				{
					*(Vector3*)Native.ReferenceToPointer<FixedStorage@3>(ref this._TargetPos) = value;
				}
			}

			// Token: 0x06003E86 RID: 16006 RVA: 0x00057F68 File Offset: 0x00056168
			public LurkerGhostData(LurkerGhost.ghostState state, int index, int actor, Vector3 pos)
			{
				this.CurrentState = state;
				this.CurrentIndex = index;
				this.TargetActor = actor;
				this.TargetPos = pos;
			}

			// Token: 0x04003FD8 RID: 16344
			[FixedBufferProperty(typeof(Vector3), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3), 0, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(12)]
			private FixedStorage@3 _TargetPos;
		}
	}
}
