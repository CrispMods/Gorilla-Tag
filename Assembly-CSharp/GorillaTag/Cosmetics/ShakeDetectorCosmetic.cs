using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C54 RID: 3156
	public class ShakeDetectorCosmetic : MonoBehaviour
	{
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x00182490 File Offset: 0x00180690
		// (set) Token: 0x06004EAC RID: 20140 RVA: 0x00182498 File Offset: 0x00180698
		public Vector3 HandVelocity { get; private set; }

		// Token: 0x06004EAD RID: 20141 RVA: 0x001824A1 File Offset: 0x001806A1
		private void Awake()
		{
			this.HandVelocity = Vector3.zero;
			this.shakeEndTime = 0f;
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x001824BC File Offset: 0x001806BC
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

		// Token: 0x06004EAF RID: 20143 RVA: 0x00182564 File Offset: 0x00180764
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

		// Token: 0x04005245 RID: 21061
		[SerializeField]
		private TransferrableObject parentTransferrable;

		// Token: 0x04005246 RID: 21062
		[Tooltip("for velocity equal or above this, we fire a Shake Start event")]
		[SerializeField]
		private float shakeStartVelocityThreshold;

		// Token: 0x04005247 RID: 21063
		[Tooltip("for velocity under this, we fire a Shake End event")]
		[SerializeField]
		private float shakeEndVelocityThreshold;

		// Token: 0x04005248 RID: 21064
		[Tooltip("cooldown starts when shaking ends")]
		[SerializeField]
		private float cooldown;

		// Token: 0x04005249 RID: 21065
		[Tooltip("Use for clamping hand velocity value")]
		[SerializeField]
		private float maxHandVelocity = 20f;

		// Token: 0x0400524A RID: 21066
		[FormerlySerializedAs("onShakeStart")]
		public UnityEvent<bool, float> onShakeStartLocal;

		// Token: 0x0400524B RID: 21067
		[FormerlySerializedAs("onShakeEnd")]
		public UnityEvent<bool, float> onShakeEndLocal;

		// Token: 0x0400524D RID: 21069
		private bool isShaking;

		// Token: 0x0400524E RID: 21070
		private float shakeEndTime;

		// Token: 0x0400524F RID: 21071
		private bool isLeftHand;
	}
}
