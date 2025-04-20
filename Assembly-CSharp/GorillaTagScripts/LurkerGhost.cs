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
	// Token: 0x020009ED RID: 2541
	[NetworkBehaviourWeaved(6)]
	public class LurkerGhost : NetworkComponent
	{
		// Token: 0x06003F70 RID: 16240 RVA: 0x00059665 File Offset: 0x00057865
		protected override void Awake()
		{
			base.Awake();
			this.possibleTargets = new List<NetPlayer>();
			this.targetPlayer = null;
			this.targetTransform = null;
			this.targetVRRig = null;
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x0005968D File Offset: 0x0005788D
		protected override void Start()
		{
			base.Start();
			this.waypointRegions = this.waypointsContainer.GetComponentsInChildren<ZoneBasedObject>();
			this.PickNextWaypoint();
			this.ChangeState(LurkerGhost.ghostState.patrol);
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x000596B3 File Offset: 0x000578B3
		private void LateUpdate()
		{
			this.UpdateState();
			this.UpdateGhostVisibility();
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x00168810 File Offset: 0x00166A10
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

		// Token: 0x06003F74 RID: 16244 RVA: 0x00168930 File Offset: 0x00166B30
		private void Patrol()
		{
			Transform transform = this.currentWaypoint;
			if (transform != null)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, transform.position, this.patrolSpeed * Time.deltaTime);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.targetRotation, 360f * Time.deltaTime);
			}
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x001689A8 File Offset: 0x00166BA8
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

		// Token: 0x06003F76 RID: 16246 RVA: 0x00168A14 File Offset: 0x00166C14
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

		// Token: 0x06003F77 RID: 16247 RVA: 0x00168B44 File Offset: 0x00166D44
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

		// Token: 0x06003F78 RID: 16248 RVA: 0x00168BEC File Offset: 0x00166DEC
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

		// Token: 0x06003F79 RID: 16249 RVA: 0x00168CC0 File Offset: 0x00166EC0
		private void ChargeAtPlayer()
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetPosition, this.chargeSpeed * Time.deltaTime);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.targetRotation, 720f * Time.deltaTime);
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x00168D28 File Offset: 0x00166F28
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

		// Token: 0x06003F7B RID: 16251 RVA: 0x00168E4C File Offset: 0x0016704C
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

		// Token: 0x06003F7C RID: 16252 RVA: 0x00168EB0 File Offset: 0x001670B0
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

		// Token: 0x06003F7D RID: 16253 RVA: 0x000596C1 File Offset: 0x000578C1
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			Shader.SetGlobalFloat(this._BlackAndWhite, 0f);
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x00169030 File Offset: 0x00167230
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

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06003F7F RID: 16255 RVA: 0x000596DE File Offset: 0x000578DE
		// (set) Token: 0x06003F80 RID: 16256 RVA: 0x00059708 File Offset: 0x00057908
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

		// Token: 0x06003F81 RID: 16257 RVA: 0x00059733 File Offset: 0x00057933
		public override void WriteDataFusion()
		{
			this.Data = new LurkerGhost.LurkerGhostData(this.currentState, this.currentIndex, this.targetPlayer.ActorNumber, this.targetPosition);
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x00169340 File Offset: 0x00167540
		public override void ReadDataFusion()
		{
			this.ReadDataShared(this.Data.CurrentState, this.Data.CurrentIndex, this.Data.TargetActor, this.Data.TargetPos);
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x0016938C File Offset: 0x0016758C
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

		// Token: 0x06003F84 RID: 16260 RVA: 0x00169408 File Offset: 0x00167608
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

		// Token: 0x06003F85 RID: 16261 RVA: 0x00169460 File Offset: 0x00167660
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

		// Token: 0x06003F86 RID: 16262 RVA: 0x0005975D File Offset: 0x0005795D
		public override void OnOwnerChange(Player newOwner, Player previousOwner)
		{
			base.OnOwnerChange(newOwner, previousOwner);
			if (newOwner == PhotonNetwork.LocalPlayer)
			{
				this.ChangeState(this.currentState);
			}
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x0005977B File Offset: 0x0005797B
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x00059793 File Offset: 0x00057993
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04004061 RID: 16481
		public float patrolSpeed = 3f;

		// Token: 0x04004062 RID: 16482
		public float seekSpeed = 6f;

		// Token: 0x04004063 RID: 16483
		public float chargeSpeed = 6f;

		// Token: 0x04004064 RID: 16484
		[Tooltip("Cooldown until the next time the ghost needs to hunt a new player")]
		public float cooldownDuration = 10f;

		// Token: 0x04004065 RID: 16485
		[Tooltip("Max Cooldown (randomized)")]
		public float maxCooldownDuration = 10f;

		// Token: 0x04004066 RID: 16486
		[Tooltip("How long the possession effects should last")]
		public float PossessionDuration = 15f;

		// Token: 0x04004067 RID: 16487
		[Tooltip("Hunted objects within this radius will get triggered ")]
		public float sphereColliderRadius = 2f;

		// Token: 0x04004068 RID: 16488
		[Tooltip("Maximum distance to the possible player to get hunted")]
		public float maxHuntDistance = 20f;

		// Token: 0x04004069 RID: 16489
		[Tooltip("Minimum distance from the player to start the possession effects")]
		public float minCatchDistance = 2f;

		// Token: 0x0400406A RID: 16490
		[Tooltip("Maximum distance to the possible player to get repeat hunted")]
		public float maxRepeatHuntDistance = 5f;

		// Token: 0x0400406B RID: 16491
		[Tooltip("Maximum times the lurker can haunt a nearby player before going back on cooldown")]
		public int maxRepeatHuntTimes = 3;

		// Token: 0x0400406C RID: 16492
		[Tooltip("Time in seconds before a haunted player can pass the lurker to another player by tagging")]
		public float tagCoolDown = 2f;

		// Token: 0x0400406D RID: 16493
		[Tooltip("UP & DOWN, IN & OUT")]
		public Vector3 SpookyMagicNumbers = new Vector3(1f, 1f, 1f);

		// Token: 0x0400406E RID: 16494
		[Tooltip("SPIN, SPIN, SPIN, SPIN")]
		public Vector4 HauntedMagicNumbers = new Vector4(1f, 2f, 3f, 1f);

		// Token: 0x0400406F RID: 16495
		[Tooltip("Haptic vibration when haunted by the ghost")]
		public float hapticStrength = 1f;

		// Token: 0x04004070 RID: 16496
		public float hapticDuration = 1.5f;

		// Token: 0x04004071 RID: 16497
		public GameObject waypointsContainer;

		// Token: 0x04004072 RID: 16498
		private ZoneBasedObject[] waypointRegions;

		// Token: 0x04004073 RID: 16499
		private ZoneBasedObject lastWaypointRegion;

		// Token: 0x04004074 RID: 16500
		private List<Transform> waypoints = new List<Transform>();

		// Token: 0x04004075 RID: 16501
		private Transform currentWaypoint;

		// Token: 0x04004076 RID: 16502
		public Material visibleMaterial;

		// Token: 0x04004077 RID: 16503
		public Material scryableMaterial;

		// Token: 0x04004078 RID: 16504
		public Material visibleMaterialBones;

		// Token: 0x04004079 RID: 16505
		public Material scryableMaterialBones;

		// Token: 0x0400407A RID: 16506
		public MeshRenderer meshRenderer;

		// Token: 0x0400407B RID: 16507
		public MeshRenderer bonesMeshRenderer;

		// Token: 0x0400407C RID: 16508
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400407D RID: 16509
		public AudioClip patrolAudio;

		// Token: 0x0400407E RID: 16510
		public AudioClip huntAudio;

		// Token: 0x0400407F RID: 16511
		public AudioClip possessedAudio;

		// Token: 0x04004080 RID: 16512
		public ThrowableSetDressing scryingGlass;

		// Token: 0x04004081 RID: 16513
		public float scryingAngerAngle;

		// Token: 0x04004082 RID: 16514
		public float scryingAngerDelay;

		// Token: 0x04004083 RID: 16515
		public float seekAheadDistance;

		// Token: 0x04004084 RID: 16516
		public float seekCloseEnoughDistance;

		// Token: 0x04004085 RID: 16517
		private float scryingAngerAfterTimestamp;

		// Token: 0x04004086 RID: 16518
		private int currentRepeatHuntTimes;

		// Token: 0x04004087 RID: 16519
		public UnityAction<GameObject> TriggerHauntedObjects;

		// Token: 0x04004088 RID: 16520
		private int currentIndex;

		// Token: 0x04004089 RID: 16521
		private LurkerGhost.ghostState currentState;

		// Token: 0x0400408A RID: 16522
		private float cooldownTimeRemaining;

		// Token: 0x0400408B RID: 16523
		private List<NetPlayer> possibleTargets;

		// Token: 0x0400408C RID: 16524
		private NetPlayer targetPlayer;

		// Token: 0x0400408D RID: 16525
		private Transform targetTransform;

		// Token: 0x0400408E RID: 16526
		private float huntedPassedTime;

		// Token: 0x0400408F RID: 16527
		private Vector3 targetPosition;

		// Token: 0x04004090 RID: 16528
		private Quaternion targetRotation;

		// Token: 0x04004091 RID: 16529
		private VRRig targetVRRig;

		// Token: 0x04004092 RID: 16530
		private ShaderHashId _BlackAndWhite = "_BlackAndWhite";

		// Token: 0x04004093 RID: 16531
		private VRRig lastHauntedVRRig;

		// Token: 0x04004094 RID: 16532
		private float nextTagTime;

		// Token: 0x04004095 RID: 16533
		private NetPlayer passingPlayer;

		// Token: 0x04004096 RID: 16534
		[SerializeField]
		private bool hauntNeighbors = true;

		// Token: 0x04004097 RID: 16535
		[WeaverGenerated]
		[DefaultForProperty("Data", 0, 6)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private LurkerGhost.LurkerGhostData _Data;

		// Token: 0x020009EE RID: 2542
		private enum ghostState
		{
			// Token: 0x04004099 RID: 16537
			patrol,
			// Token: 0x0400409A RID: 16538
			seek,
			// Token: 0x0400409B RID: 16539
			charge,
			// Token: 0x0400409C RID: 16540
			possess
		}

		// Token: 0x020009EF RID: 2543
		[NetworkStructWeaved(6)]
		[StructLayout(LayoutKind.Explicit, Size = 24)]
		private struct LurkerGhostData : INetworkStruct
		{
			// Token: 0x17000668 RID: 1640
			// (get) Token: 0x06003F8A RID: 16266 RVA: 0x000597A7 File Offset: 0x000579A7
			// (set) Token: 0x06003F8B RID: 16267 RVA: 0x000597AF File Offset: 0x000579AF
			public LurkerGhost.ghostState CurrentState { readonly get; set; }

			// Token: 0x17000669 RID: 1641
			// (get) Token: 0x06003F8C RID: 16268 RVA: 0x000597B8 File Offset: 0x000579B8
			// (set) Token: 0x06003F8D RID: 16269 RVA: 0x000597C0 File Offset: 0x000579C0
			public int CurrentIndex { readonly get; set; }

			// Token: 0x1700066A RID: 1642
			// (get) Token: 0x06003F8E RID: 16270 RVA: 0x000597C9 File Offset: 0x000579C9
			// (set) Token: 0x06003F8F RID: 16271 RVA: 0x000597D1 File Offset: 0x000579D1
			public int TargetActor { readonly get; set; }

			// Token: 0x1700066B RID: 1643
			// (get) Token: 0x06003F90 RID: 16272 RVA: 0x000597DA File Offset: 0x000579DA
			// (set) Token: 0x06003F91 RID: 16273 RVA: 0x000597EC File Offset: 0x000579EC
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

			// Token: 0x06003F92 RID: 16274 RVA: 0x000597FF File Offset: 0x000579FF
			public LurkerGhostData(LurkerGhost.ghostState state, int index, int actor, Vector3 pos)
			{
				this.CurrentState = state;
				this.CurrentIndex = index;
				this.TargetActor = actor;
				this.TargetPos = pos;
			}

			// Token: 0x040040A0 RID: 16544
			[FixedBufferProperty(typeof(Vector3), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3), 0, order = -2147483647)]
			[WeaverGenerated]
			[SerializeField]
			[FieldOffset(12)]
			private FixedStorage@3 _TargetPos;
		}
	}
}
