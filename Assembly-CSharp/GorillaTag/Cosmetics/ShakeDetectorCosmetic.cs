using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C85 RID: 3205
	public class ShakeDetectorCosmetic : MonoBehaviour
	{
		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x0600500B RID: 20491 RVA: 0x000644A1 File Offset: 0x000626A1
		// (set) Token: 0x0600500C RID: 20492 RVA: 0x000644A9 File Offset: 0x000626A9
		public Vector3 HandVelocity { get; private set; }

		// Token: 0x0600500D RID: 20493 RVA: 0x000644B2 File Offset: 0x000626B2
		private void Awake()
		{
			this.HandVelocity = Vector3.zero;
			this.shakeEndTime = 0f;
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x001BA724 File Offset: 0x001B8924
		private void UpdateShakeVelocity()
		{
			if (!this.parentTransferrable)
			{
				return;
			}
			if (!this.parentTransferrable.InHand())
			{
				this.HandVelocity = Vector3.zero;
				return;
			}
			if (!this.parentTransferrable.IsMyItem())
			{
				return;
			}
			this.isLeftHand = this.parentTransferrable.InLeftHand();
			this.HandVelocity = (this.isLeftHand ? GTPlayer.Instance.leftInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false) : GTPlayer.Instance.rightInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false));
			this.HandVelocity = Vector3.ClampMagnitude(this.HandVelocity, this.maxHandVelocity);
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x001BA7CC File Offset: 0x001B89CC
		public void Update()
		{
			this.UpdateShakeVelocity();
			if (Time.time - this.shakeEndTime > this.cooldown && !this.isShaking && this.HandVelocity.magnitude >= this.shakeStartVelocityThreshold)
			{
				UnityEvent<bool, float> unityEvent = this.onShakeStartLocal;
				if (unityEvent != null)
				{
					unityEvent.Invoke(this.isLeftHand, this.HandVelocity.magnitude);
				}
				this.isShaking = true;
			}
			if (this.isShaking && this.HandVelocity.magnitude < this.shakeEndVelocityThreshold)
			{
				UnityEvent<bool, float> unityEvent2 = this.onShakeEndLocal;
				if (unityEvent2 != null)
				{
					unityEvent2.Invoke(this.isLeftHand, this.HandVelocity.magnitude);
				}
				this.isShaking = false;
				this.shakeEndTime = Time.time;
			}
		}

		// Token: 0x04005351 RID: 21329
		[SerializeField]
		private TransferrableObject parentTransferrable;

		// Token: 0x04005352 RID: 21330
		[Tooltip("for velocity equal or above this, we fire a Shake Start event")]
		[SerializeField]
		private float shakeStartVelocityThreshold;

		// Token: 0x04005353 RID: 21331
		[Tooltip("for velocity under this, we fire a Shake End event")]
		[SerializeField]
		private float shakeEndVelocityThreshold;

		// Token: 0x04005354 RID: 21332
		[Tooltip("cooldown starts when shaking ends")]
		[SerializeField]
		private float cooldown;

		// Token: 0x04005355 RID: 21333
		[Tooltip("Use for clamping hand velocity value")]
		[SerializeField]
		private float maxHandVelocity = 20f;

		// Token: 0x04005356 RID: 21334
		[FormerlySerializedAs("onShakeStart")]
		public UnityEvent<bool, float> onShakeStartLocal;

		// Token: 0x04005357 RID: 21335
		[FormerlySerializedAs("onShakeEnd")]
		public UnityEvent<bool, float> onShakeEndLocal;

		// Token: 0x04005359 RID: 21337
		private bool isShaking;

		// Token: 0x0400535A RID: 21338
		private float shakeEndTime;

		// Token: 0x0400535B RID: 21339
		private bool isLeftHand;
	}
}
