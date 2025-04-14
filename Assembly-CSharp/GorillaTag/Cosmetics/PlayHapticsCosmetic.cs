using System;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C50 RID: 3152
	public class PlayHapticsCosmetic : MonoBehaviour
	{
		// Token: 0x06004E94 RID: 20116 RVA: 0x00181F3B File Offset: 0x0018013B
		public void PlayHaptics(bool isLeftHand, float value)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06004E95 RID: 20117 RVA: 0x00181F3B File Offset: 0x0018013B
		public void PlayHaptics(bool isLeftHand, Collider other)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x00181F3B File Offset: 0x0018013B
		public void PlayHaptics(bool isLeftHand, Collision other)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x00181F54 File Offset: 0x00180154
		public void PlayHapticsByButtonValue(bool isLeftHand, float strength)
		{
			float amplitude = Mathf.InverseLerp(this.minHapticStrengthThreshold, this.maxHapticStrengthThreshold, strength);
			GorillaTagger.Instance.StartVibration(isLeftHand, amplitude, this.hapticDuration);
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x00181F88 File Offset: 0x00180188
		public void PlayHapticsByVelocity(bool isLeftHand, float velocity)
		{
			float num = (isLeftHand ? GTPlayer.Instance.leftInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false) : GTPlayer.Instance.rightInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false)).magnitude;
			num = Mathf.InverseLerp(this.minHapticStrengthThreshold, this.maxHapticStrengthThreshold, num);
			GorillaTagger.Instance.StartVibration(isLeftHand, num, this.hapticDuration);
		}

		// Token: 0x04005222 RID: 21026
		[SerializeField]
		private float hapticDuration;

		// Token: 0x04005223 RID: 21027
		[SerializeField]
		private float hapticStrength;

		// Token: 0x04005224 RID: 21028
		[SerializeField]
		private float minHapticStrengthThreshold;

		// Token: 0x04005225 RID: 21029
		[SerializeField]
		private float maxHapticStrengthThreshold;
	}
}
