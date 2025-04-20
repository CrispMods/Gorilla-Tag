using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C8E RID: 3214
	[RequireComponent(typeof(TransferrableObject))]
	public class VenusFlyTrapHoldable : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06005040 RID: 20544 RVA: 0x00064721 File Offset: 0x00062921
		// (set) Token: 0x06005041 RID: 20545 RVA: 0x00064729 File Offset: 0x00062929
		public bool TickRunning { get; set; }

		// Token: 0x06005042 RID: 20546 RVA: 0x00064732 File Offset: 0x00062932
		private void Awake()
		{
			this.transferrableObject = base.GetComponent<TransferrableObject>();
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x001BB404 File Offset: 0x001B9604
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

		// Token: 0x06005044 RID: 20548 RVA: 0x001BB51C File Offset: 0x001B971C
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

		// Token: 0x06005045 RID: 20549 RVA: 0x001BB588 File Offset: 0x001B9788
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

		// Token: 0x06005046 RID: 20550 RVA: 0x001BB698 File Offset: 0x001B9898
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

		// Token: 0x06005047 RID: 20551 RVA: 0x00064740 File Offset: 0x00062940
		private void UpdateState(VenusFlyTrapHoldable.VenusState newState)
		{
			this.state = newState;
			if (this.state == VenusFlyTrapHoldable.VenusState.Closed)
			{
				this.closedStartedTime = Time.time;
			}
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x001BB884 File Offset: 0x001B9A84
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

		// Token: 0x06005049 RID: 20553 RVA: 0x0006475C File Offset: 0x0006295C
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

		// Token: 0x0600504A RID: 20554 RVA: 0x00064788 File Offset: 0x00062988
		private void OnTriggerLocal()
		{
			this.UpdateState(VenusFlyTrapHoldable.VenusState.Closing);
			if (this.audioSource && this.closingAudio != null)
			{
				this.audioSource.GTPlayOneShot(this.closingAudio, 1f);
			}
		}

		// Token: 0x0400539F RID: 21407
		[SerializeField]
		private GameObject lipA;

		// Token: 0x040053A0 RID: 21408
		[SerializeField]
		private GameObject lipB;

		// Token: 0x040053A1 RID: 21409
		[SerializeField]
		private Vector3 targetRotationA;

		// Token: 0x040053A2 RID: 21410
		[SerializeField]
		private Vector3 targetRotationB;

		// Token: 0x040053A3 RID: 21411
		[SerializeField]
		private float closedDuration = 3f;

		// Token: 0x040053A4 RID: 21412
		[SerializeField]
		private float speed = 2f;

		// Token: 0x040053A5 RID: 21413
		[SerializeField]
		private UnityLayer layers;

		// Token: 0x040053A6 RID: 21414
		[SerializeField]
		private TriggerEventNotifier triggerEventNotifier;

		// Token: 0x040053A7 RID: 21415
		[SerializeField]
		private float hapticStrength = 0.5f;

		// Token: 0x040053A8 RID: 21416
		[SerializeField]
		private float hapticDuration = 0.1f;

		// Token: 0x040053A9 RID: 21417
		[SerializeField]
		private GameObject bug;

		// Token: 0x040053AA RID: 21418
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040053AB RID: 21419
		[SerializeField]
		private AudioClip closingAudio;

		// Token: 0x040053AC RID: 21420
		[SerializeField]
		private AudioClip openingAudio;

		// Token: 0x040053AD RID: 21421
		[SerializeField]
		private AudioClip flyLoopingAudio;

		// Token: 0x040053AE RID: 21422
		private CallLimiter callLimiter = new CallLimiter(10, 2f, 0.5f);

		// Token: 0x040053AF RID: 21423
		private float closedStartedTime;

		// Token: 0x040053B0 RID: 21424
		private VenusFlyTrapHoldable.VenusState state;

		// Token: 0x040053B1 RID: 21425
		private Quaternion localRotA;

		// Token: 0x040053B2 RID: 21426
		private Quaternion localRotB;

		// Token: 0x040053B3 RID: 21427
		private RubberDuckEvents _events;

		// Token: 0x040053B4 RID: 21428
		private TransferrableObject transferrableObject;

		// Token: 0x02000C8F RID: 3215
		private enum VenusState
		{
			// Token: 0x040053B7 RID: 21431
			Closed,
			// Token: 0x040053B8 RID: 21432
			Open,
			// Token: 0x040053B9 RID: 21433
			Closing,
			// Token: 0x040053BA RID: 21434
			Opening
		}
	}
}
