using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C58 RID: 3160
	[RequireComponent(typeof(TransferrableObject))]
	public class SmoothScaleModifierCosmetic : MonoBehaviour
	{
		// Token: 0x06004EBD RID: 20157 RVA: 0x00062AB8 File Offset: 0x00060CB8
		private void Awake()
		{
			this.transferrableObject = base.GetComponentInParent<TransferrableObject>();
			this.initialScale = this.objectPrefab.transform.localScale;
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x00062ADC File Offset: 0x00060CDC
		private void OnEnable()
		{
			this.UpdateState(SmoothScaleModifierCosmetic.State.Reset);
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x001B27B0 File Offset: 0x001B09B0
		private void Update()
		{
			if (this.transferrableObject && !this.transferrableObject.InHand())
			{
				if (this.audioSource && this.audioSource.isPlaying)
				{
					this.audioSource.GTStop();
				}
				return;
			}
			switch (this.currentState)
			{
			case SmoothScaleModifierCosmetic.State.None:
				if (this.audioSource && this.normalSizeAudio && !this.audioSource.isPlaying)
				{
					this.audioSource.clip = this.normalSizeAudio;
					this.audioSource.volume = this.normalSizeAudioVolume;
					this.audioSource.GTPlay();
				}
				break;
			case SmoothScaleModifierCosmetic.State.Reset:
				this.SmoothScale(this.objectPrefab.transform.localScale, this.initialScale);
				if (Vector3.Distance(this.objectPrefab.transform.localScale, this.initialScale) < 0.01f)
				{
					this.objectPrefab.transform.localScale = this.initialScale;
					this.UpdateState(SmoothScaleModifierCosmetic.State.None);
					return;
				}
				break;
			case SmoothScaleModifierCosmetic.State.Scaling:
				this.SmoothScale(this.objectPrefab.transform.localScale, this.targetScale);
				if (Vector3.Distance(this.objectPrefab.transform.localScale, this.targetScale) < 0.01f)
				{
					this.objectPrefab.transform.localScale = this.targetScale;
					this.UpdateState(SmoothScaleModifierCosmetic.State.Scaled);
					return;
				}
				break;
			case SmoothScaleModifierCosmetic.State.Scaled:
				if (this.audioSource && this.scaledAudio && !this.audioSource.isPlaying)
				{
					this.audioSource.clip = this.scaledAudio;
					this.audioSource.volume = this.scaleAudioVolume;
					this.audioSource.GTPlay();
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x00062AE5 File Offset: 0x00060CE5
		private void SmoothScale(Vector3 initial, Vector3 target)
		{
			this.objectPrefab.transform.localScale = Vector3.MoveTowards(initial, target, this.speed * Time.deltaTime);
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x001B298C File Offset: 0x001B0B8C
		private void ApplyScaling(IFingerFlexListener.ComponentActivator activator)
		{
			if (this.audioSource)
			{
				this.audioSource.GTStop();
			}
			if (this.scaleOn == activator)
			{
				if (this.currentState != SmoothScaleModifierCosmetic.State.Scaled)
				{
					this.UpdateState(SmoothScaleModifierCosmetic.State.Scaling);
					return;
				}
			}
			else if (this.resetOn == activator && this.currentState != SmoothScaleModifierCosmetic.State.Reset)
			{
				this.UpdateState(SmoothScaleModifierCosmetic.State.Reset);
			}
		}

		// Token: 0x06004EC2 RID: 20162 RVA: 0x00062B0A File Offset: 0x00060D0A
		private void UpdateState(SmoothScaleModifierCosmetic.State newState)
		{
			this.currentState = newState;
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x00062B13 File Offset: 0x00060D13
		public void OnButtonPressed()
		{
			this.ApplyScaling(IFingerFlexListener.ComponentActivator.FingerFlexed);
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x00062B1C File Offset: 0x00060D1C
		public void OnButtonReleased()
		{
			this.ApplyScaling(IFingerFlexListener.ComponentActivator.FingerReleased);
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x00062B25 File Offset: 0x00060D25
		public void OnButtonPressStayed()
		{
			this.ApplyScaling(IFingerFlexListener.ComponentActivator.FingerStayed);
		}

		// Token: 0x04005262 RID: 21090
		[SerializeField]
		private GameObject objectPrefab;

		// Token: 0x04005263 RID: 21091
		[SerializeField]
		private Vector3 targetScale = new Vector3(2f, 2f, 2f);

		// Token: 0x04005264 RID: 21092
		[SerializeField]
		private float speed = 2f;

		// Token: 0x04005265 RID: 21093
		[SerializeField]
		private IFingerFlexListener.ComponentActivator scaleOn;

		// Token: 0x04005266 RID: 21094
		[SerializeField]
		private IFingerFlexListener.ComponentActivator resetOn;

		// Token: 0x04005267 RID: 21095
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005268 RID: 21096
		[SerializeField]
		private AudioClip scaledAudio;

		// Token: 0x04005269 RID: 21097
		[SerializeField]
		private float scaleAudioVolume = 0.1f;

		// Token: 0x0400526A RID: 21098
		[SerializeField]
		private AudioClip normalSizeAudio;

		// Token: 0x0400526B RID: 21099
		[SerializeField]
		private float normalSizeAudioVolume = 0.1f;

		// Token: 0x0400526C RID: 21100
		private SmoothScaleModifierCosmetic.State currentState;

		// Token: 0x0400526D RID: 21101
		private Vector3 initialScale;

		// Token: 0x0400526E RID: 21102
		private TransferrableObject transferrableObject;

		// Token: 0x02000C59 RID: 3161
		private enum State
		{
			// Token: 0x04005270 RID: 21104
			None,
			// Token: 0x04005271 RID: 21105
			Reset,
			// Token: 0x04005272 RID: 21106
			Scaling,
			// Token: 0x04005273 RID: 21107
			Scaled
		}
	}
}
