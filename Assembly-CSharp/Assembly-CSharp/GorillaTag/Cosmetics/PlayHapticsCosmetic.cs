using System;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C53 RID: 3155
	public class PlayHapticsCosmetic : MonoBehaviour
	{
		// Token: 0x06004EA0 RID: 20128 RVA: 0x00182503 File Offset: 0x00180703
		public void PlayHaptics(bool isLeftHand, float value)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x00182503 File Offset: 0x00180703
		public void PlayHaptics(bool isLeftHand, Collider other)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x00182503 File Offset: 0x00180703
		public void PlayHaptics(bool isLeftHand, Collision other)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06004EA3 RID: 20131 RVA: 0x0018251C File Offset: 0x0018071C
		public void PlayHapticsByButtonValue(bool isLeftHand, float strength)
		{
			float amplitude = Mathf.InverseLerp(this.minHapticStrengthThreshold, this.maxHapticStrengthThreshold, strength);
			GorillaTagger.Instance.StartVibration(isLeftHand, amplitude, this.hapticDuration);
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x00182550 File Offset: 0x00180750
		public void PlayHapticsByVelocity(bool isLeftHand, float velocity)
		{
			float num = (isLeftHand ? GTPlayer.Instance.leftInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false) : GTPlayer.Instance.rightInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false)).magnitude;
			num = Mathf.InverseLerp(this.minHapticStrengthThreshold, this.maxHapticStrengthThreshold, num);
			GorillaTagger.Instance.StartVibration(isLeftHand, num, this.hapticDuration);
		}

		// Token: 0x04005234 RID: 21044
		[SerializeField]
		private float hapticDuration;

		// Token: 0x04005235 RID: 21045
		[SerializeField]
		private float hapticStrength;

		// Token: 0x04005236 RID: 21046
		[SerializeField]
		private float minHapticStrengthThreshold;

		// Token: 0x04005237 RID: 21047
		[SerializeField]
		private float maxHapticStrengthThreshold;
	}
}
