using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C57 RID: 3159
	public class ShakeDetectorCosmetic : MonoBehaviour
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06004EB7 RID: 20151 RVA: 0x00182A58 File Offset: 0x00180C58
		// (set) Token: 0x06004EB8 RID: 20152 RVA: 0x00182A60 File Offset: 0x00180C60
		public Vector3 HandVelocity { get; private set; }

		// Token: 0x06004EB9 RID: 20153 RVA: 0x00182A69 File Offset: 0x00180C69
		private void Awake()
		{
			this.HandVelocity = Vector3.zero;
			this.shakeEndTime = 0f;
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x00182A84 File Offset: 0x00180C84
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

		// Token: 0x06004EBB RID: 20155 RVA: 0x00182B2C File Offset: 0x00180D2C
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

		// Token: 0x04005257 RID: 21079
		[SerializeField]
		private TransferrableObject parentTransferrable;

		// Token: 0x04005258 RID: 21080
		[Tooltip("for velocity equal or above this, we fire a Shake Start event")]
		[SerializeField]
		private float shakeStartVelocityThreshold;

		// Token: 0x04005259 RID: 21081
		[Tooltip("for velocity under this, we fire a Shake End event")]
		[SerializeField]
		private float shakeEndVelocityThreshold;

		// Token: 0x0400525A RID: 21082
		[Tooltip("cooldown starts when shaking ends")]
		[SerializeField]
		private float cooldown;

		// Token: 0x0400525B RID: 21083
		[Tooltip("Use for clamping hand velocity value")]
		[SerializeField]
		private float maxHandVelocity = 20f;

		// Token: 0x0400525C RID: 21084
		[FormerlySerializedAs("onShakeStart")]
		public UnityEvent<bool, float> onShakeStartLocal;

		// Token: 0x0400525D RID: 21085
		[FormerlySerializedAs("onShakeEnd")]
		public UnityEvent<bool, float> onShakeEndLocal;

		// Token: 0x0400525F RID: 21087
		private bool isShaking;

		// Token: 0x04005260 RID: 21088
		private float shakeEndTime;

		// Token: 0x04005261 RID: 21089
		private bool isLeftHand;
	}
}
