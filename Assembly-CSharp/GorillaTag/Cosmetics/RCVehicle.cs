using System;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C27 RID: 3111
	public class RCVehicle : MonoBehaviour, ISpawnable
	{
		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06004D96 RID: 19862 RVA: 0x0017C524 File Offset: 0x0017A724
		public bool HasLocalAuthority
		{
			get
			{
				return !PhotonNetwork.InRoom || (this.networkSync != null && this.networkSync.photonView.IsMine);
			}
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x0017C550 File Offset: 0x0017A750
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

		// Token: 0x06004D98 RID: 19864 RVA: 0x0017C5B4 File Offset: 0x0017A7B4
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

		// Token: 0x06004D99 RID: 19865 RVA: 0x0017C615 File Offset: 0x0017A815
		public virtual void EndConnection()
		{
			this.connectedRemote = null;
			this.activeInput = default(RCRemoteHoldable.RCInput);
			this.disconnectionTime = Time.time;
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x0017C638 File Offset: 0x0017A838
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

		// Token: 0x06004D9B RID: 19867 RVA: 0x0017C710 File Offset: 0x0017A910
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

		// Token: 0x06004D9C RID: 19868 RVA: 0x0017C780 File Offset: 0x0017A980
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

		// Token: 0x06004D9D RID: 19869 RVA: 0x0017C7DC File Offset: 0x0017A9DC
		protected virtual void AuthorityBeginCrash()
		{
			this.localState = RCVehicle.State.Crashed;
			if (this.networkSync != null)
			{
				this.networkSync.syncedState.state = (byte)this.localState;
			}
			this.stateStartTime = Time.time;
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x0017C818 File Offset: 0x0017AA18
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

		// Token: 0x06004D9F RID: 19871 RVA: 0x0017C86A File Offset: 0x0017AA6A
		protected virtual void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x000023F4 File Offset: 0x000005F4
		protected virtual void OnEnable()
		{
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06004DA1 RID: 19873 RVA: 0x0017C878 File Offset: 0x0017AA78
		// (set) Token: 0x06004DA2 RID: 19874 RVA: 0x0017C880 File Offset: 0x0017AA80
		bool ISpawnable.IsSpawned { get; set; }

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06004DA3 RID: 19875 RVA: 0x0017C889 File Offset: 0x0017AA89
		// (set) Token: 0x06004DA4 RID: 19876 RVA: 0x0017C891 File Offset: 0x0017AA91
		ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

		// Token: 0x06004DA5 RID: 19877 RVA: 0x0017C89C File Offset: 0x0017AA9C
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

		// Token: 0x06004DA6 RID: 19878 RVA: 0x000023F4 File Offset: 0x000005F4
		void ISpawnable.OnDespawn()
		{
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x0017C957 File Offset: 0x0017AB57
		protected virtual void OnDisable()
		{
			this.localState = RCVehicle.State.Disabled;
			this.localStatePrev = RCVehicle.State.Disabled;
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x0017C968 File Offset: 0x0017AB68
		public void ApplyRemoteControlInput(RCRemoteHoldable.RCInput rcInput)
		{
			this.activeInput.joystick.y = Mathf.Sign(rcInput.joystick.y) * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(this.joystickDeadzone, 1f, Mathf.Abs(rcInput.joystick.y)));
			this.activeInput.joystick.x = Mathf.Sign(rcInput.joystick.x) * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(this.joystickDeadzone, 1f, Mathf.Abs(rcInput.joystick.x)));
			this.activeInput.trigger = Mathf.Clamp(rcInput.trigger, -1f, 1f);
			this.activeInput.buttons = rcInput.buttons;
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x0017CA48 File Offset: 0x0017AC48
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

		// Token: 0x06004DAA RID: 19882 RVA: 0x0017CA88 File Offset: 0x0017AC88
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

		// Token: 0x06004DAB RID: 19883 RVA: 0x0017CBE8 File Offset: 0x0017ADE8
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

		// Token: 0x06004DAC RID: 19884 RVA: 0x000023F4 File Offset: 0x000005F4
		protected virtual void SharedUpdate(float dt)
		{
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x0017CD70 File Offset: 0x0017AF70
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

		// Token: 0x06004DAE RID: 19886 RVA: 0x000EC73D File Offset: 0x000EA93D
		protected float NormalizeAngle180(float angle)
		{
			angle = (angle + 180f) % 360f;
			if (angle < 0f)
			{
				angle += 360f;
			}
			return angle - 180f;
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x0017CDE8 File Offset: 0x0017AFE8
		protected static void AddScaledGravityCompensationForce(Rigidbody rb, float scaleFactor, float gravityCompensation)
		{
			Vector3 gravity = Physics.gravity;
			Vector3 vector = -gravity * gravityCompensation;
			Vector3 vector2 = gravity + vector;
			Vector3 b = vector2 * scaleFactor - vector2;
			rb.AddForce(vector + b, ForceMode.Acceleration);
		}

		// Token: 0x0400509C RID: 20636
		[SerializeField]
		private Transform leftDockParent;

		// Token: 0x0400509D RID: 20637
		[SerializeField]
		private Transform rightDockParent;

		// Token: 0x0400509E RID: 20638
		[SerializeField]
		private float maxRange = 100f;

		// Token: 0x0400509F RID: 20639
		[SerializeField]
		private float maxDisconnectionTime = 10f;

		// Token: 0x040050A0 RID: 20640
		[SerializeField]
		private float crashRespawnDelay = 3f;

		// Token: 0x040050A1 RID: 20641
		[SerializeField]
		private bool crashOnHit;

		// Token: 0x040050A2 RID: 20642
		[SerializeField]
		private float crashOnHitSpeedThreshold = 5f;

		// Token: 0x040050A3 RID: 20643
		[SerializeField]
		[Range(0f, 1f)]
		private float hitVelocityTransfer = 0.5f;

		// Token: 0x040050A4 RID: 20644
		[SerializeField]
		[Range(0f, 1f)]
		private float projectileVelocityTransfer = 0.1f;

		// Token: 0x040050A5 RID: 20645
		[SerializeField]
		private float hitMaxHitSpeed = 4f;

		// Token: 0x040050A6 RID: 20646
		[SerializeField]
		[Range(0f, 1f)]
		private float joystickDeadzone = 0.1f;

		// Token: 0x040050A7 RID: 20647
		protected RCVehicle.State localState;

		// Token: 0x040050A8 RID: 20648
		protected RCVehicle.State localStatePrev;

		// Token: 0x040050A9 RID: 20649
		protected float stateStartTime;

		// Token: 0x040050AA RID: 20650
		protected RCRemoteHoldable connectedRemote;

		// Token: 0x040050AB RID: 20651
		protected RCCosmeticNetworkSync networkSync;

		// Token: 0x040050AC RID: 20652
		protected bool hasNetworkSync;

		// Token: 0x040050AD RID: 20653
		protected RCRemoteHoldable.RCInput activeInput;

		// Token: 0x040050AE RID: 20654
		protected Rigidbody rb;

		// Token: 0x040050AF RID: 20655
		private bool waitingForTriggerRelease;

		// Token: 0x040050B0 RID: 20656
		private float disconnectionTime;

		// Token: 0x040050B1 RID: 20657
		private bool useLeftDock;

		// Token: 0x040050B2 RID: 20658
		private BoneOffset dockLeftOffset = new BoneOffset(GTHardCodedBones.EBone.forearm_L, new Vector3(-0.062f, 0.283f, -0.136f), new Vector3(275f, 0f, 25f));

		// Token: 0x040050B3 RID: 20659
		private BoneOffset dockRightOffset = new BoneOffset(GTHardCodedBones.EBone.forearm_R, new Vector3(0.069f, 0.265f, -0.128f), new Vector3(275f, 0f, 335f));

		// Token: 0x040050B4 RID: 20660
		private float networkSyncFollowRateExp = 2f;

		// Token: 0x040050B5 RID: 20661
		private Transform[] _vrRigBones;

		// Token: 0x02000C28 RID: 3112
		protected enum State
		{
			// Token: 0x040050B9 RID: 20665
			Disabled,
			// Token: 0x040050BA RID: 20666
			DockedLeft,
			// Token: 0x040050BB RID: 20667
			DockedRight,
			// Token: 0x040050BC RID: 20668
			Mobilized,
			// Token: 0x040050BD RID: 20669
			Crashed
		}
	}
}
