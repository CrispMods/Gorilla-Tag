using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C55 RID: 3157
	[RequireComponent(typeof(TransferrableObject))]
	public class SmoothScaleModifierCosmetic : MonoBehaviour
	{
		// Token: 0x06004EB1 RID: 20145 RVA: 0x0018263D File Offset: 0x0018083D
		private void Awake()
		{
			this.transferrableObject = base.GetComponentInParent<TransferrableObject>();
			this.initialScale = this.objectPrefab.transform.localScale;
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x00182661 File Offset: 0x00180861
		private void OnEnable()
		{
			this.UpdateState(SmoothScaleModifierCosmetic.State.Reset);
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x0018266C File Offset: 0x0018086C
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

		// Token: 0x06004EB4 RID: 20148 RVA: 0x00182847 File Offset: 0x00180A47
		private void SmoothScale(Vector3 initial, Vector3 target)
		{
			this.objectPrefab.transform.localScale = Vector3.MoveTowards(initial, target, this.speed * Time.deltaTime);
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x0018286C File Offset: 0x00180A6C
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

		// Token: 0x06004EB6 RID: 20150 RVA: 0x001828C4 File Offset: 0x00180AC4
		private void UpdateState(SmoothScaleModifierCosmetic.State newState)
		{
			this.currentState = newState;
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x001828CD File Offset: 0x00180ACD
		public void OnButtonPressed()
		{
			this.ApplyScaling(IFingerFlexListener.ComponentActivator.FingerFlexed);
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x001828D6 File Offset: 0x00180AD6
		public void OnButtonReleased()
		{
			this.ApplyScaling(IFingerFlexListener.ComponentActivator.FingerReleased);
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x001828DF File Offset: 0x00180ADF
		public void OnButtonPressStayed()
		{
			this.ApplyScaling(IFingerFlexListener.ComponentActivator.FingerStayed);
		}

		// Token: 0x04005250 RID: 21072
		[SerializeField]
		private GameObject objectPrefab;

		// Token: 0x04005251 RID: 21073
		[SerializeField]
		private Vector3 targetScale = new Vector3(2f, 2f, 2f);

		// Token: 0x04005252 RID: 21074
		[SerializeField]
		private float speed = 2f;

		// Token: 0x04005253 RID: 21075
		[SerializeField]
		private IFingerFlexListener.ComponentActivator scaleOn;

		// Token: 0x04005254 RID: 21076
		[SerializeField]
		private IFingerFlexListener.ComponentActivator resetOn;

		// Token: 0x04005255 RID: 21077
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005256 RID: 21078
		[SerializeField]
		private AudioClip scaledAudio;

		// Token: 0x04005257 RID: 21079
		[SerializeField]
		private float scaleAudioVolume = 0.1f;

		// Token: 0x04005258 RID: 21080
		[SerializeField]
		private AudioClip normalSizeAudio;

		// Token: 0x04005259 RID: 21081
		[SerializeField]
		private float normalSizeAudioVolume = 0.1f;

		// Token: 0x0400525A RID: 21082
		private SmoothScaleModifierCosmetic.State currentState;

		// Token: 0x0400525B RID: 21083
		private Vector3 initialScale;

		// Token: 0x0400525C RID: 21084
		private TransferrableObject transferrableObject;

		// Token: 0x02000C56 RID: 3158
		private enum State
		{
			// Token: 0x0400525E RID: 21086
			None,
			// Token: 0x0400525F RID: 21087
			Reset,
			// Token: 0x04005260 RID: 21088
			Scaling,
			// Token: 0x04005261 RID: 21089
			Scaled
		}
	}
}
