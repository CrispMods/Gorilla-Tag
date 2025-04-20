using System;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C81 RID: 3201
	public class PlayHapticsCosmetic : MonoBehaviour
	{
		// Token: 0x06004FF4 RID: 20468 RVA: 0x000643A7 File Offset: 0x000625A7
		public void PlayHaptics(bool isLeftHand, float value)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x000643A7 File Offset: 0x000625A7
		public void PlayHaptics(bool isLeftHand, Collider other)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x000643A7 File Offset: 0x000625A7
		public void PlayHaptics(bool isLeftHand, Collision other)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.hapticStrength, this.hapticDuration);
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x001BA2C8 File Offset: 0x001B84C8
		public void PlayHapticsByButtonValue(bool isLeftHand, float strength)
		{
			float amplitude = Mathf.InverseLerp(this.minHapticStrengthThreshold, this.maxHapticStrengthThreshold, strength);
			GorillaTagger.Instance.StartVibration(isLeftHand, amplitude, this.hapticDuration);
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x001BA2FC File Offset: 0x001B84FC
		public void PlayHapticsByVelocity(bool isLeftHand, float velocity)
		{
			float num = (isLeftHand ? GTPlayer.Instance.leftInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false) : GTPlayer.Instance.rightInteractPointVelocityTracker.GetAverageVelocity(true, 0.15f, false)).magnitude;
			num = Mathf.InverseLerp(this.minHapticStrengthThreshold, this.maxHapticStrengthThreshold, num);
			GorillaTagger.Instance.StartVibration(isLeftHand, num, this.hapticDuration);
		}

		// Token: 0x0400532E RID: 21294
		[SerializeField]
		private float hapticDuration;

		// Token: 0x0400532F RID: 21295
		[SerializeField]
		private float hapticStrength;

		// Token: 0x04005330 RID: 21296
		[SerializeField]
		private float minHapticStrengthThreshold;

		// Token: 0x04005331 RID: 21297
		[SerializeField]
		private float maxHapticStrengthThreshold;
	}
}
