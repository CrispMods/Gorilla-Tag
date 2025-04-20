using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C86 RID: 3206
	[RequireComponent(typeof(TransferrableObject))]
	public class SmoothScaleModifierCosmetic : MonoBehaviour
	{
		// Token: 0x06005011 RID: 20497 RVA: 0x000644DD File Offset: 0x000626DD
		private void Awake()
		{
			this.transferrableObject = base.GetComponentInParent<TransferrableObject>();
			this.initialScale = this.objectPrefab.transform.localScale;
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x00064501 File Offset: 0x00062701
		private void OnEnable()
		{
			this.UpdateState(SmoothScaleModifierCosmetic.State.Reset);
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x001BA894 File Offset: 0x001B8A94
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

		// Token: 0x06005014 RID: 20500 RVA: 0x0006450A File Offset: 0x0006270A
		private void SmoothScale(Vector3 initial, Vector3 target)
		{
			this.objectPrefab.transform.localScale = Vector3.MoveTowards(initial, target, this.speed * Time.deltaTime);
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x001BAA70 File Offset: 0x001B8C70
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

		// Token: 0x06005016 RID: 20502 RVA: 0x0006452F File Offset: 0x0006272F
		private void UpdateState(SmoothScaleModifierCosmetic.State newState)
		{
			this.currentState = newState;
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x00064538 File Offset: 0x00062738
		public void OnButtonPressed()
		{
			this.ApplyScaling(IFingerFlexListener.ComponentActivator.FingerFlexed);
		}

		// Token: 0x06005018 RID: 20504 RVA: 0x00064541 File Offset: 0x00062741
		public void OnButtonReleased()
		{
			this.ApplyScaling(IFingerFlexListener.ComponentActivator.FingerReleased);
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x0006454A File Offset: 0x0006274A
		public void OnButtonPressStayed()
		{
			this.ApplyScaling(IFingerFlexListener.ComponentActivator.FingerStayed);
		}

		// Token: 0x0400535C RID: 21340
		[SerializeField]
		private GameObject objectPrefab;

		// Token: 0x0400535D RID: 21341
		[SerializeField]
		private Vector3 targetScale = new Vector3(2f, 2f, 2f);

		// Token: 0x0400535E RID: 21342
		[SerializeField]
		private float speed = 2f;

		// Token: 0x0400535F RID: 21343
		[SerializeField]
		private IFingerFlexListener.ComponentActivator scaleOn;

		// Token: 0x04005360 RID: 21344
		[SerializeField]
		private IFingerFlexListener.ComponentActivator resetOn;

		// Token: 0x04005361 RID: 21345
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005362 RID: 21346
		[SerializeField]
		private AudioClip scaledAudio;

		// Token: 0x04005363 RID: 21347
		[SerializeField]
		private float scaleAudioVolume = 0.1f;

		// Token: 0x04005364 RID: 21348
		[SerializeField]
		private AudioClip normalSizeAudio;

		// Token: 0x04005365 RID: 21349
		[SerializeField]
		private float normalSizeAudioVolume = 0.1f;

		// Token: 0x04005366 RID: 21350
		private SmoothScaleModifierCosmetic.State currentState;

		// Token: 0x04005367 RID: 21351
		private Vector3 initialScale;

		// Token: 0x04005368 RID: 21352
		private TransferrableObject transferrableObject;

		// Token: 0x02000C87 RID: 3207
		private enum State
		{
			// Token: 0x0400536A RID: 21354
			None,
			// Token: 0x0400536B RID: 21355
			Reset,
			// Token: 0x0400536C RID: 21356
			Scaling,
			// Token: 0x0400536D RID: 21357
			Scaled
		}
	}
}
