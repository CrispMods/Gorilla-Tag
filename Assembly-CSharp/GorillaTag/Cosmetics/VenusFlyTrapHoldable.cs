using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5D RID: 3165
	[RequireComponent(typeof(TransferrableObject))]
	public class VenusFlyTrapHoldable : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06004EE0 RID: 20192 RVA: 0x001833F2 File Offset: 0x001815F2
		// (set) Token: 0x06004EE1 RID: 20193 RVA: 0x001833FA File Offset: 0x001815FA
		public bool TickRunning { get; set; }

		// Token: 0x06004EE2 RID: 20194 RVA: 0x00183403 File Offset: 0x00181603
		private void Awake()
		{
			this.transferrableObject = base.GetComponent<TransferrableObject>();
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x00183414 File Offset: 0x00181614
		private void OnEnable()
		{
			TickSystem<object>.AddCallbackTarget(this);
			this.triggerEventNotifier.TriggerEnterEvent += this.TriggerEntered;
			this.state = VenusFlyTrapHoldable.VenusState.Open;
			this.localRotA = this.lipA.transform.localRotation;
			this.localRotB = this.lipB.transform.localRotation;
			if (this._events == null)
			{
				this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
				NetPlayer netPlayer = (this.transferrableObject.myOnlineRig != null) ? this.transferrableObject.myOnlineRig.creator : ((this.transferrableObject.myRig != null) ? (this.transferrableObject.myRig.creator ?? NetworkSystem.Instance.LocalPlayer) : null);
				if (netPlayer != null)
				{
					this._events.Init(netPlayer);
				}
			}
			if (this._events != null)
			{
				this._events.Activate += this.OnTriggerEvent;
			}
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x0018352C File Offset: 0x0018172C
		private void OnDisable()
		{
			TickSystem<object>.RemoveCallbackTarget(this);
			this.triggerEventNotifier.TriggerEnterEvent -= this.TriggerEntered;
			if (this._events != null)
			{
				this._events.Activate -= this.OnTriggerEvent;
				this._events.Dispose();
				this._events = null;
			}
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x00183598 File Offset: 0x00181798
		public void Tick()
		{
			if (this.transferrableObject.InHand() && this.audioSource && !this.audioSource.isPlaying && this.flyLoopingAudio != null)
			{
				this.audioSource.clip = this.flyLoopingAudio;
				this.audioSource.GTPlay();
			}
			if (!this.transferrableObject.InHand() && this.audioSource && this.audioSource.isPlaying)
			{
				this.audioSource.GTStop();
			}
			if (this.state == VenusFlyTrapHoldable.VenusState.Open)
			{
				return;
			}
			if (this.state == VenusFlyTrapHoldable.VenusState.Closed && Time.time - this.closedStartedTime >= this.closedDuration)
			{
				this.UpdateState(VenusFlyTrapHoldable.VenusState.Opening);
				if (this.audioSource && this.openingAudio != null)
				{
					this.audioSource.GTPlayOneShot(this.openingAudio, 1f);
				}
			}
			if (this.state == VenusFlyTrapHoldable.VenusState.Closing)
			{
				this.SmoothRotation(true);
				return;
			}
			if (this.state == VenusFlyTrapHoldable.VenusState.Opening)
			{
				this.SmoothRotation(false);
			}
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x001836A8 File Offset: 0x001818A8
		private void SmoothRotation(bool isClosing)
		{
			if (isClosing)
			{
				Quaternion quaternion = Quaternion.Euler(this.targetRotationB);
				this.lipB.transform.localRotation = Quaternion.Lerp(this.lipB.transform.localRotation, quaternion, Time.deltaTime * this.speed);
				Quaternion quaternion2 = Quaternion.Euler(this.targetRotationA);
				this.lipA.transform.localRotation = Quaternion.Lerp(this.lipA.transform.localRotation, quaternion2, Time.deltaTime * this.speed);
				if (Quaternion.Angle(this.lipB.transform.localRotation, quaternion) < 1f && Quaternion.Angle(this.lipA.transform.localRotation, quaternion2) < 1f)
				{
					this.lipB.transform.localRotation = quaternion;
					this.lipA.transform.localRotation = quaternion2;
					this.UpdateState(VenusFlyTrapHoldable.VenusState.Closed);
					return;
				}
			}
			else
			{
				this.lipB.transform.localRotation = Quaternion.Lerp(this.lipB.transform.localRotation, this.localRotB, Time.deltaTime * this.speed / 2f);
				this.lipA.transform.localRotation = Quaternion.Lerp(this.lipA.transform.localRotation, this.localRotA, Time.deltaTime * this.speed / 2f);
				if (Quaternion.Angle(this.lipB.transform.localRotation, this.localRotB) < 1f && Quaternion.Angle(this.lipA.transform.localRotation, this.localRotA) < 1f)
				{
					this.lipB.transform.localRotation = this.localRotB;
					this.lipA.transform.localRotation = this.localRotA;
					this.UpdateState(VenusFlyTrapHoldable.VenusState.Open);
				}
			}
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x00183892 File Offset: 0x00181A92
		private void UpdateState(VenusFlyTrapHoldable.VenusState newState)
		{
			this.state = newState;
			if (this.state == VenusFlyTrapHoldable.VenusState.Closed)
			{
				this.closedStartedTime = Time.time;
			}
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x001838B0 File Offset: 0x00181AB0
		private void TriggerEntered(TriggerEventNotifier notifier, Collider other)
		{
			if (this.state != VenusFlyTrapHoldable.VenusState.Open)
			{
				return;
			}
			if (!other.gameObject.IsOnLayer(this.layers))
			{
				return;
			}
			if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
			{
				this._events.Activate.RaiseOthers(Array.Empty<object>());
			}
			this.OnTriggerLocal();
			GorillaTriggerColliderHandIndicator componentInChildren = other.GetComponentInChildren<GorillaTriggerColliderHandIndicator>();
			if (componentInChildren == null)
			{
				return;
			}
			GorillaTagger.Instance.StartVibration(componentInChildren.isLeftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x0018394B File Offset: 0x00181B4B
		private void OnTriggerEvent(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
		{
			if (sender != target)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "OnTriggerEvent");
			if (!this.callLimiter.CheckCallTime(Time.time))
			{
				return;
			}
			this.OnTriggerLocal();
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x00183977 File Offset: 0x00181B77
		private void OnTriggerLocal()
		{
			this.UpdateState(VenusFlyTrapHoldable.VenusState.Closing);
			if (this.audioSource && this.closingAudio != null)
			{
				this.audioSource.GTPlayOneShot(this.closingAudio, 1f);
			}
		}

		// Token: 0x04005293 RID: 21139
		[SerializeField]
		private GameObject lipA;

		// Token: 0x04005294 RID: 21140
		[SerializeField]
		private GameObject lipB;

		// Token: 0x04005295 RID: 21141
		[SerializeField]
		private Vector3 targetRotationA;

		// Token: 0x04005296 RID: 21142
		[SerializeField]
		private Vector3 targetRotationB;

		// Token: 0x04005297 RID: 21143
		[SerializeField]
		private float closedDuration = 3f;

		// Token: 0x04005298 RID: 21144
		[SerializeField]
		private float speed = 2f;

		// Token: 0x04005299 RID: 21145
		[SerializeField]
		private UnityLayer layers;

		// Token: 0x0400529A RID: 21146
		[SerializeField]
		private TriggerEventNotifier triggerEventNotifier;

		// Token: 0x0400529B RID: 21147
		[SerializeField]
		private float hapticStrength = 0.5f;

		// Token: 0x0400529C RID: 21148
		[SerializeField]
		private float hapticDuration = 0.1f;

		// Token: 0x0400529D RID: 21149
		[SerializeField]
		private GameObject bug;

		// Token: 0x0400529E RID: 21150
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400529F RID: 21151
		[SerializeField]
		private AudioClip closingAudio;

		// Token: 0x040052A0 RID: 21152
		[SerializeField]
		private AudioClip openingAudio;

		// Token: 0x040052A1 RID: 21153
		[SerializeField]
		private AudioClip flyLoopingAudio;

		// Token: 0x040052A2 RID: 21154
		private CallLimiter callLimiter = new CallLimiter(10, 2f, 0.5f);

		// Token: 0x040052A3 RID: 21155
		private float closedStartedTime;

		// Token: 0x040052A4 RID: 21156
		private VenusFlyTrapHoldable.VenusState state;

		// Token: 0x040052A5 RID: 21157
		private Quaternion localRotA;

		// Token: 0x040052A6 RID: 21158
		private Quaternion localRotB;

		// Token: 0x040052A7 RID: 21159
		private RubberDuckEvents _events;

		// Token: 0x040052A8 RID: 21160
		private TransferrableObject transferrableObject;

		// Token: 0x02000C5E RID: 3166
		private enum VenusState
		{
			// Token: 0x040052AB RID: 21163
			Closed,
			// Token: 0x040052AC RID: 21164
			Open,
			// Token: 0x040052AD RID: 21165
			Closing,
			// Token: 0x040052AE RID: 21166
			Opening
		}
	}
}
