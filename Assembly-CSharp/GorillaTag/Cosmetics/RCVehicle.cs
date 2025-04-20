using System;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C55 RID: 3157
	public class RCVehicle : MonoBehaviour, ISpawnable
	{
		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06004EE7 RID: 20199 RVA: 0x000638C2 File Offset: 0x00061AC2
		public bool HasLocalAuthority
		{
			get
			{
				return !PhotonNetwork.InRoom || (this.networkSync != null && this.networkSync.photonView.IsMine);
			}
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x001B4B3C File Offset: 0x001B2D3C
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

		// Token: 0x06004EE9 RID: 20201 RVA: 0x001B4BA0 File Offset: 0x001B2DA0
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

		// Token: 0x06004EEA RID: 20202 RVA: 0x000638ED File Offset: 0x00061AED
		public virtual void EndConnection()
		{
			this.connectedRemote = null;
			this.activeInput = default(RCRemoteHoldable.RCInput);
			this.disconnectionTime = Time.time;
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x001B4C04 File Offset: 0x001B2E04
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

		// Token: 0x06004EEC RID: 20204 RVA: 0x001B4CDC File Offset: 0x001B2EDC
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

		// Token: 0x06004EED RID: 20205 RVA: 0x001B4D4C File Offset: 0x001B2F4C
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

		// Token: 0x06004EEE RID: 20206 RVA: 0x0006390D File Offset: 0x00061B0D
		protected virtual void AuthorityBeginCrash()
		{
			this.localState = RCVehicle.State.Crashed;
			if (this.networkSync != null)
			{
				this.networkSync.syncedState.state = (byte)this.localState;
			}
			this.stateStartTime = Time.time;
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x001B4DA8 File Offset: 0x001B2FA8
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

		// Token: 0x06004EF0 RID: 20208 RVA: 0x00063946 File Offset: 0x00061B46
		protected virtual void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x00030607 File Offset: 0x0002E807
		protected virtual void OnEnable()
		{
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06004EF2 RID: 20210 RVA: 0x00063954 File Offset: 0x00061B54
		// (set) Token: 0x06004EF3 RID: 20211 RVA: 0x0006395C File Offset: 0x00061B5C
		bool ISpawnable.IsSpawned { get; set; }

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06004EF4 RID: 20212 RVA: 0x00063965 File Offset: 0x00061B65
		// (set) Token: 0x06004EF5 RID: 20213 RVA: 0x0006396D File Offset: 0x00061B6D
		ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

		// Token: 0x06004EF6 RID: 20214 RVA: 0x001B4DFC File Offset: 0x001B2FFC
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

		// Token: 0x06004EF7 RID: 20215 RVA: 0x00030607 File Offset: 0x0002E807
		void ISpawnable.OnDespawn()
		{
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x00063976 File Offset: 0x00061B76
		protected virtual void OnDisable()
		{
			this.localState = RCVehicle.State.Disabled;
			this.localStatePrev = RCVehicle.State.Disabled;
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x001B4EB8 File Offset: 0x001B30B8
		public void ApplyRemoteControlInput(RCRemoteHoldable.RCInput rcInput)
		{
			this.activeInput.joystick.y = Mathf.Sign(rcInput.joystick.y) * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(this.joystickDeadzone, 1f, Mathf.Abs(rcInput.joystick.y)));
			this.activeInput.joystick.x = Mathf.Sign(rcInput.joystick.x) * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(this.joystickDeadzone, 1f, Mathf.Abs(rcInput.joystick.x)));
			this.activeInput.trigger = Mathf.Clamp(rcInput.trigger, -1f, 1f);
			this.activeInput.buttons = rcInput.buttons;
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x001B4F98 File Offset: 0x001B3198
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

		// Token: 0x06004EFB RID: 20219 RVA: 0x001B4FD8 File Offset: 0x001B31D8
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

		// Token: 0x06004EFC RID: 20220 RVA: 0x001B5138 File Offset: 0x001B3338
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

		// Token: 0x06004EFD RID: 20221 RVA: 0x00030607 File Offset: 0x0002E807
		protected virtual void SharedUpdate(float dt)
		{
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x001B52C0 File Offset: 0x001B34C0
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

		// Token: 0x06004EFF RID: 20223 RVA: 0x00050BA5 File Offset: 0x0004EDA5
		protected float NormalizeAngle180(float angle)
		{
			angle = (angle + 180f) % 360f;
			if (angle < 0f)
			{
				angle += 360f;
			}
			return angle - 180f;
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x001B5338 File Offset: 0x001B3538
		protected static void AddScaledGravityCompensationForce(Rigidbody rb, float scaleFactor, float gravityCompensation)
		{
			Vector3 gravity = Physics.gravity;
			Vector3 vector = -gravity * gravityCompensation;
			Vector3 vector2 = gravity + vector;
			Vector3 b = vector2 * scaleFactor - vector2;
			rb.AddForce(vector + b, ForceMode.Acceleration);
		}

		// Token: 0x04005192 RID: 20882
		[SerializeField]
		private Transform leftDockParent;

		// Token: 0x04005193 RID: 20883
		[SerializeField]
		private Transform rightDockParent;

		// Token: 0x04005194 RID: 20884
		[SerializeField]
		private float maxRange = 100f;

		// Token: 0x04005195 RID: 20885
		[SerializeField]
		private float maxDisconnectionTime = 10f;

		// Token: 0x04005196 RID: 20886
		[SerializeField]
		private float crashRespawnDelay = 3f;

		// Token: 0x04005197 RID: 20887
		[SerializeField]
		private bool crashOnHit;

		// Token: 0x04005198 RID: 20888
		[SerializeField]
		private float crashOnHitSpeedThreshold = 5f;

		// Token: 0x04005199 RID: 20889
		[SerializeField]
		[Range(0f, 1f)]
		private float hitVelocityTransfer = 0.5f;

		// Token: 0x0400519A RID: 20890
		[SerializeField]
		[Range(0f, 1f)]
		private float projectileVelocityTransfer = 0.1f;

		// Token: 0x0400519B RID: 20891
		[SerializeField]
		private float hitMaxHitSpeed = 4f;

		// Token: 0x0400519C RID: 20892
		[SerializeField]
		[Range(0f, 1f)]
		private float joystickDeadzone = 0.1f;

		// Token: 0x0400519D RID: 20893
		protected RCVehicle.State localState;

		// Token: 0x0400519E RID: 20894
		protected RCVehicle.State localStatePrev;

		// Token: 0x0400519F RID: 20895
		protected float stateStartTime;

		// Token: 0x040051A0 RID: 20896
		protected RCRemoteHoldable connectedRemote;

		// Token: 0x040051A1 RID: 20897
		protected RCCosmeticNetworkSync networkSync;

		// Token: 0x040051A2 RID: 20898
		protected bool hasNetworkSync;

		// Token: 0x040051A3 RID: 20899
		protected RCRemoteHoldable.RCInput activeInput;

		// Token: 0x040051A4 RID: 20900
		protected Rigidbody rb;

		// Token: 0x040051A5 RID: 20901
		private bool waitingForTriggerRelease;

		// Token: 0x040051A6 RID: 20902
		private float disconnectionTime;

		// Token: 0x040051A7 RID: 20903
		private bool useLeftDock;

		// Token: 0x040051A8 RID: 20904
		private BoneOffset dockLeftOffset = new BoneOffset(GTHardCodedBones.EBone.forearm_L, new Vector3(-0.062f, 0.283f, -0.136f), new Vector3(275f, 0f, 25f));

		// Token: 0x040051A9 RID: 20905
		private BoneOffset dockRightOffset = new BoneOffset(GTHardCodedBones.EBone.forearm_R, new Vector3(0.069f, 0.265f, -0.128f), new Vector3(275f, 0f, 335f));

		// Token: 0x040051AA RID: 20906
		private float networkSyncFollowRateExp = 2f;

		// Token: 0x040051AB RID: 20907
		private Transform[] _vrRigBones;

		// Token: 0x02000C56 RID: 3158
		protected enum State
		{
			// Token: 0x040051AF RID: 20911
			Disabled,
			// Token: 0x040051B0 RID: 20912
			DockedLeft,
			// Token: 0x040051B1 RID: 20913
			DockedRight,
			// Token: 0x040051B2 RID: 20914
			Mobilized,
			// Token: 0x040051B3 RID: 20915
			Crashed
		}
	}
}
