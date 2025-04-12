using System;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C2A RID: 3114
	public class RCVehicle : MonoBehaviour, ISpawnable
	{
		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06004DA2 RID: 19874 RVA: 0x00061F01 File Offset: 0x00060101
		public bool HasLocalAuthority
		{
			get
			{
				return !PhotonNetwork.InRoom || (this.networkSync != null && this.networkSync.photonView.IsMine);
			}
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x001AD250 File Offset: 0x001AB450
		public virtual void WakeUpRemote(RCCosmeticNetworkSync sync)
		{
			this.networkSync = sync;
			this.hasNetworkSync = (sync != null);
			if (this.HasLocalAuthority)
			{
				return;
			}
			if (!base.enabled || !base.gameObject.activeSelf)
			{
				this.localStatePrev = RCVehicle.State.Disabled;
				base.enabled = true;
				base.gameObject.SetActive(true);
				this.RemoteUpdate(Time.deltaTime);
			}
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x001AD2B4 File Offset: 0x001AB4B4
		public virtual void StartConnection(RCRemoteHoldable remote, RCCosmeticNetworkSync sync)
		{
			this.connectedRemote = remote;
			this.networkSync = sync;
			this.hasNetworkSync = (sync != null);
			base.enabled = true;
			base.gameObject.SetActive(true);
			this.useLeftDock = (remote.XRNode == XRNode.LeftHand);
			if (this.HasLocalAuthority && this.localState != RCVehicle.State.Mobilized)
			{
				this.AuthorityBeginDocked();
			}
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x00061F2C File Offset: 0x0006012C
		public virtual void EndConnection()
		{
			this.connectedRemote = null;
			this.activeInput = default(RCRemoteHoldable.RCInput);
			this.disconnectionTime = Time.time;
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x001AD318 File Offset: 0x001AB518
		protected virtual void ResetToSpawnPosition()
		{
			if (this.rb == null)
			{
				this.rb = base.GetComponent<Rigidbody>();
			}
			if (this.rb != null)
			{
				this.rb.isKinematic = true;
			}
			base.transform.parent = (this.useLeftDock ? this.leftDockParent : this.rightDockParent);
			base.transform.SetLocalPositionAndRotation(this.useLeftDock ? this.dockLeftOffset.pos : this.dockRightOffset.pos, this.useLeftDock ? this.dockLeftOffset.rot : this.dockRightOffset.rot);
			base.transform.localScale = (this.useLeftDock ? this.dockLeftOffset.scale : this.dockRightOffset.scale);
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x001AD3F0 File Offset: 0x001AB5F0
		protected virtual void AuthorityBeginDocked()
		{
			this.localState = (this.useLeftDock ? RCVehicle.State.DockedLeft : RCVehicle.State.DockedRight);
			if (this.networkSync != null)
			{
				this.networkSync.syncedState.state = (byte)this.localState;
			}
			this.stateStartTime = Time.time;
			this.waitingForTriggerRelease = true;
			this.ResetToSpawnPosition();
			if (this.connectedRemote == null)
			{
				this.SetDisabledState();
			}
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x001AD460 File Offset: 0x001AB660
		protected virtual void AuthorityBeginMobilization()
		{
			this.localState = RCVehicle.State.Mobilized;
			if (this.networkSync != null)
			{
				this.networkSync.syncedState.state = (byte)this.localState;
			}
			this.stateStartTime = Time.time;
			base.transform.parent = null;
			this.rb.isKinematic = false;
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x00061F4C File Offset: 0x0006014C
		protected virtual void AuthorityBeginCrash()
		{
			this.localState = RCVehicle.State.Crashed;
			if (this.networkSync != null)
			{
				this.networkSync.syncedState.state = (byte)this.localState;
			}
			this.stateStartTime = Time.time;
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x001AD4BC File Offset: 0x001AB6BC
		protected virtual void SetDisabledState()
		{
			this.localState = RCVehicle.State.Disabled;
			if (this.networkSync != null)
			{
				this.networkSync.syncedState.state = (byte)this.localState;
			}
			this.ResetToSpawnPosition();
			base.enabled = false;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x00061F85 File Offset: 0x00060185
		protected virtual void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x0002F75F File Offset: 0x0002D95F
		protected virtual void OnEnable()
		{
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06004DAD RID: 19885 RVA: 0x00061F93 File Offset: 0x00060193
		// (set) Token: 0x06004DAE RID: 19886 RVA: 0x00061F9B File Offset: 0x0006019B
		bool ISpawnable.IsSpawned { get; set; }

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06004DAF RID: 19887 RVA: 0x00061FA4 File Offset: 0x000601A4
		// (set) Token: 0x06004DB0 RID: 19888 RVA: 0x00061FAC File Offset: 0x000601AC
		ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

		// Token: 0x06004DB1 RID: 19889 RVA: 0x001AD510 File Offset: 0x001AB710
		void ISpawnable.OnSpawn(VRRig rig)
		{
			if (rig == null)
			{
				GTDev.LogError<string>("RCVehicle: Could not find VRRig in parents. If you are trying to make this a world item rather than a cosmetic then you'll have to refactor how it teleports back to the arms.", this, null);
				return;
			}
			string str;
			if (!GTHardCodedBones.TryGetBoneXforms(rig, out this._vrRigBones, out str))
			{
				Debug.LogError("RCVehicle: " + str, this);
				return;
			}
			if (this.leftDockParent == null && !GTHardCodedBones.TryGetBoneXform(this._vrRigBones, this.dockLeftOffset.bone, out this.leftDockParent))
			{
				GTDev.LogError<string>("RCVehicle: Could not find left dock transform.", this, null);
			}
			if (this.rightDockParent == null && !GTHardCodedBones.TryGetBoneXform(this._vrRigBones, this.dockRightOffset.bone, out this.rightDockParent))
			{
				GTDev.LogError<string>("RCVehicle: Could not find right dock transform.", this, null);
			}
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x0002F75F File Offset: 0x0002D95F
		void ISpawnable.OnDespawn()
		{
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x00061FB5 File Offset: 0x000601B5
		protected virtual void OnDisable()
		{
			this.localState = RCVehicle.State.Disabled;
			this.localStatePrev = RCVehicle.State.Disabled;
		}

		// Token: 0x06004DB4 RID: 19892 RVA: 0x001AD5CC File Offset: 0x001AB7CC
		public void ApplyRemoteControlInput(RCRemoteHoldable.RCInput rcInput)
		{
			this.activeInput.joystick.y = Mathf.Sign(rcInput.joystick.y) * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(this.joystickDeadzone, 1f, Mathf.Abs(rcInput.joystick.y)));
			this.activeInput.joystick.x = Mathf.Sign(rcInput.joystick.x) * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(this.joystickDeadzone, 1f, Mathf.Abs(rcInput.joystick.x)));
			this.activeInput.trigger = Mathf.Clamp(rcInput.trigger, -1f, 1f);
			this.activeInput.buttons = rcInput.buttons;
		}

		// Token: 0x06004DB5 RID: 19893 RVA: 0x001AD6AC File Offset: 0x001AB8AC
		private void Update()
		{
			float deltaTime = Time.deltaTime;
			if (this.HasLocalAuthority)
			{
				this.AuthorityUpdate(deltaTime);
			}
			else
			{
				this.RemoteUpdate(deltaTime);
			}
			this.SharedUpdate(deltaTime);
			this.localStatePrev = this.localState;
		}

		// Token: 0x06004DB6 RID: 19894 RVA: 0x001AD6EC File Offset: 0x001AB8EC
		protected virtual void AuthorityUpdate(float dt)
		{
			switch (this.localState)
			{
			default:
				if (this.localState != this.localStatePrev)
				{
					this.ResetToSpawnPosition();
				}
				if (this.connectedRemote == null)
				{
					this.SetDisabledState();
					return;
				}
				if (this.waitingForTriggerRelease && this.activeInput.trigger < 0.25f)
				{
					this.waitingForTriggerRelease = false;
				}
				if (!this.waitingForTriggerRelease && this.activeInput.trigger > 0.25f)
				{
					this.AuthorityBeginMobilization();
					return;
				}
				break;
			case RCVehicle.State.Mobilized:
			{
				if (this.networkSync != null)
				{
					this.networkSync.syncedState.position = base.transform.position;
					this.networkSync.syncedState.rotation = base.transform.rotation;
				}
				bool flag = (base.transform.position - this.leftDockParent.position).sqrMagnitude > this.maxRange * this.maxRange;
				bool flag2 = this.connectedRemote == null && Time.time - this.disconnectionTime > this.maxDisconnectionTime;
				if (flag || flag2)
				{
					this.AuthorityBeginCrash();
					return;
				}
				break;
			}
			case RCVehicle.State.Crashed:
				if (Time.time > this.stateStartTime + this.crashRespawnDelay)
				{
					this.AuthorityBeginDocked();
				}
				break;
			}
		}

		// Token: 0x06004DB7 RID: 19895 RVA: 0x001AD84C File Offset: 0x001ABA4C
		protected virtual void RemoteUpdate(float dt)
		{
			if (this.networkSync == null)
			{
				this.SetDisabledState();
				return;
			}
			this.localState = (RCVehicle.State)this.networkSync.syncedState.state;
			switch (this.localState)
			{
			case RCVehicle.State.Disabled:
				this.SetDisabledState();
				break;
			default:
				if (this.localStatePrev != RCVehicle.State.DockedLeft)
				{
					this.useLeftDock = true;
					this.ResetToSpawnPosition();
					return;
				}
				break;
			case RCVehicle.State.DockedRight:
				if (this.localStatePrev != RCVehicle.State.DockedRight)
				{
					this.useLeftDock = false;
					this.ResetToSpawnPosition();
					return;
				}
				break;
			case RCVehicle.State.Mobilized:
				if (this.localStatePrev != RCVehicle.State.Mobilized)
				{
					this.rb.isKinematic = true;
					base.transform.parent = null;
				}
				base.transform.position = Vector3.Lerp(this.networkSync.syncedState.position, base.transform.position, Mathf.Exp(-this.networkSyncFollowRateExp * dt));
				base.transform.rotation = Quaternion.Slerp(this.networkSync.syncedState.rotation, base.transform.rotation, Mathf.Exp(-this.networkSyncFollowRateExp * dt));
				return;
			case RCVehicle.State.Crashed:
				if (this.localStatePrev != RCVehicle.State.Crashed)
				{
					this.rb.isKinematic = false;
					base.transform.parent = null;
					if (this.localStatePrev != RCVehicle.State.Mobilized)
					{
						base.transform.position = this.networkSync.syncedState.position;
						base.transform.rotation = this.networkSync.syncedState.rotation;
						return;
					}
				}
				break;
			}
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x0002F75F File Offset: 0x0002D95F
		protected virtual void SharedUpdate(float dt)
		{
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x001AD9D4 File Offset: 0x001ABBD4
		public virtual void AuthorityApplyImpact(Vector3 hitVelocity, bool isProjectile)
		{
			if (this.HasLocalAuthority && this.localState == RCVehicle.State.Mobilized)
			{
				float d = isProjectile ? this.projectileVelocityTransfer : this.hitVelocityTransfer;
				this.rb.AddForce(Vector3.ClampMagnitude(hitVelocity * d, this.hitMaxHitSpeed), ForceMode.VelocityChange);
				if (isProjectile || (this.crashOnHit && hitVelocity.sqrMagnitude > this.crashOnHitSpeedThreshold * this.crashOnHitSpeedThreshold))
				{
					this.AuthorityBeginCrash();
				}
			}
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x0004F7A3 File Offset: 0x0004D9A3
		protected float NormalizeAngle180(float angle)
		{
			angle = (angle + 180f) % 360f;
			if (angle < 0f)
			{
				angle += 360f;
			}
			return angle - 180f;
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x001ADA4C File Offset: 0x001ABC4C
		protected static void AddScaledGravityCompensationForce(Rigidbody rb, float scaleFactor, float gravityCompensation)
		{
			Vector3 gravity = Physics.gravity;
			Vector3 vector = -gravity * gravityCompensation;
			Vector3 vector2 = gravity + vector;
			Vector3 b = vector2 * scaleFactor - vector2;
			rb.AddForce(vector + b, ForceMode.Acceleration);
		}

		// Token: 0x040050AE RID: 20654
		[SerializeField]
		private Transform leftDockParent;

		// Token: 0x040050AF RID: 20655
		[SerializeField]
		private Transform rightDockParent;

		// Token: 0x040050B0 RID: 20656
		[SerializeField]
		private float maxRange = 100f;

		// Token: 0x040050B1 RID: 20657
		[SerializeField]
		private float maxDisconnectionTime = 10f;

		// Token: 0x040050B2 RID: 20658
		[SerializeField]
		private float crashRespawnDelay = 3f;

		// Token: 0x040050B3 RID: 20659
		[SerializeField]
		private bool crashOnHit;

		// Token: 0x040050B4 RID: 20660
		[SerializeField]
		private float crashOnHitSpeedThreshold = 5f;

		// Token: 0x040050B5 RID: 20661
		[SerializeField]
		[Range(0f, 1f)]
		private float hitVelocityTransfer = 0.5f;

		// Token: 0x040050B6 RID: 20662
		[SerializeField]
		[Range(0f, 1f)]
		private float projectileVelocityTransfer = 0.1f;

		// Token: 0x040050B7 RID: 20663
		[SerializeField]
		private float hitMaxHitSpeed = 4f;

		// Token: 0x040050B8 RID: 20664
		[SerializeField]
		[Range(0f, 1f)]
		private float joystickDeadzone = 0.1f;

		// Token: 0x040050B9 RID: 20665
		protected RCVehicle.State localState;

		// Token: 0x040050BA RID: 20666
		protected RCVehicle.State localStatePrev;

		// Token: 0x040050BB RID: 20667
		protected float stateStartTime;

		// Token: 0x040050BC RID: 20668
		protected RCRemoteHoldable connectedRemote;

		// Token: 0x040050BD RID: 20669
		protected RCCosmeticNetworkSync networkSync;

		// Token: 0x040050BE RID: 20670
		protected bool hasNetworkSync;

		// Token: 0x040050BF RID: 20671
		protected RCRemoteHoldable.RCInput activeInput;

		// Token: 0x040050C0 RID: 20672
		protected Rigidbody rb;

		// Token: 0x040050C1 RID: 20673
		private bool waitingForTriggerRelease;

		// Token: 0x040050C2 RID: 20674
		private float disconnectionTime;

		// Token: 0x040050C3 RID: 20675
		private bool useLeftDock;

		// Token: 0x040050C4 RID: 20676
		private BoneOffset dockLeftOffset = new BoneOffset(GTHardCodedBones.EBone.forearm_L, new Vector3(-0.062f, 0.283f, -0.136f), new Vector3(275f, 0f, 25f));

		// Token: 0x040050C5 RID: 20677
		private BoneOffset dockRightOffset = new BoneOffset(GTHardCodedBones.EBone.forearm_R, new Vector3(0.069f, 0.265f, -0.128f), new Vector3(275f, 0f, 335f));

		// Token: 0x040050C6 RID: 20678
		private float networkSyncFollowRateExp = 2f;

		// Token: 0x040050C7 RID: 20679
		private Transform[] _vrRigBones;

		// Token: 0x02000C2B RID: 3115
		protected enum State
		{
			// Token: 0x040050CB RID: 20683
			Disabled,
			// Token: 0x040050CC RID: 20684
			DockedLeft,
			// Token: 0x040050CD RID: 20685
			DockedRight,
			// Token: 0x040050CE RID: 20686
			Mobilized,
			// Token: 0x040050CF RID: 20687
			Crashed
		}
	}
}
