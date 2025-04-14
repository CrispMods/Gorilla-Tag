using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C60 RID: 3168
	[RequireComponent(typeof(TransferrableObject))]
	public class VenusFlyTrapHoldable : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06004EEC RID: 20204 RVA: 0x001839BA File Offset: 0x00181BBA
		// (set) Token: 0x06004EED RID: 20205 RVA: 0x001839C2 File Offset: 0x00181BC2
		public bool TickRunning { get; set; }

		// Token: 0x06004EEE RID: 20206 RVA: 0x001839CB File Offset: 0x00181BCB
		private void Awake()
		{
			this.transferrableObject = base.GetComponent<TransferrableObject>();
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x001839DC File Offset: 0x00181BDC
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

		// Token: 0x06004EF0 RID: 20208 RVA: 0x00183AF4 File Offset: 0x00181CF4
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

		// Token: 0x06004EF1 RID: 20209 RVA: 0x00183B60 File Offset: 0x00181D60
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

		// Token: 0x06004EF2 RID: 20210 RVA: 0x00183C70 File Offset: 0x00181E70
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

		// Token: 0x06004EF3 RID: 20211 RVA: 0x00183E5A File Offset: 0x0018205A
		private void UpdateState(VenusFlyTrapHoldable.VenusState newState)
		{
			this.state = newState;
			if (this.state == VenusFlyTrapHoldable.VenusState.Closed)
			{
				this.closedStartedTime = Time.time;
			}
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x00183E78 File Offset: 0x00182078
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

		// Token: 0x06004EF5 RID: 20213 RVA: 0x00183F13 File Offset: 0x00182113
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

		// Token: 0x06004EF6 RID: 20214 RVA: 0x00183F3F File Offset: 0x0018213F
		private void OnTriggerLocal()
		{
			this.UpdateState(VenusFlyTrapHoldable.VenusState.Closing);
			if (this.audioSource && this.closingAudio != null)
			{
				this.audioSource.GTPlayOneShot(this.closingAudio, 1f);
			}
		}

		// Token: 0x040052A5 RID: 21157
		[SerializeField]
		private GameObject lipA;

		// Token: 0x040052A6 RID: 21158
		[SerializeField]
		private GameObject lipB;

		// Token: 0x040052A7 RID: 21159
		[SerializeField]
		private Vector3 targetRotationA;

		// Token: 0x040052A8 RID: 21160
		[SerializeField]
		private Vector3 targetRotationB;

		// Token: 0x040052A9 RID: 21161
		[SerializeField]
		private float closedDuration = 3f;

		// Token: 0x040052AA RID: 21162
		[SerializeField]
		private float speed = 2f;

		// Token: 0x040052AB RID: 21163
		[SerializeField]
		private UnityLayer layers;

		// Token: 0x040052AC RID: 21164
		[SerializeField]
		private TriggerEventNotifier triggerEventNotifier;

		// Token: 0x040052AD RID: 21165
		[SerializeField]
		private float hapticStrength = 0.5f;

		// Token: 0x040052AE RID: 21166
		[SerializeField]
		private float hapticDuration = 0.1f;

		// Token: 0x040052AF RID: 21167
		[SerializeField]
		private GameObject bug;

		// Token: 0x040052B0 RID: 21168
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040052B1 RID: 21169
		[SerializeField]
		private AudioClip closingAudio;

		// Token: 0x040052B2 RID: 21170
		[SerializeField]
		private AudioClip openingAudio;

		// Token: 0x040052B3 RID: 21171
		[SerializeField]
		private AudioClip flyLoopingAudio;

		// Token: 0x040052B4 RID: 21172
		private CallLimiter callLimiter = new CallLimiter(10, 2f, 0.5f);

		// Token: 0x040052B5 RID: 21173
		private float closedStartedTime;

		// Token: 0x040052B6 RID: 21174
		private VenusFlyTrapHoldable.VenusState state;

		// Token: 0x040052B7 RID: 21175
		private Quaternion localRotA;

		// Token: 0x040052B8 RID: 21176
		private Quaternion localRotB;

		// Token: 0x040052B9 RID: 21177
		private RubberDuckEvents _events;

		// Token: 0x040052BA RID: 21178
		private TransferrableObject transferrableObject;

		// Token: 0x02000C61 RID: 3169
		private enum VenusState
		{
			// Token: 0x040052BD RID: 21181
			Closed,
			// Token: 0x040052BE RID: 21182
			Open,
			// Token: 0x040052BF RID: 21183
			Closing,
			// Token: 0x040052C0 RID: 21184
			Opening
		}
	}
}
