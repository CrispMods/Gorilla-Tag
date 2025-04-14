using System;
using UnityEngine;

namespace DefaultNamespace
{
	// Token: 0x0200096D RID: 2413
	[RequireComponent(typeof(SoundBankPlayer))]
	public class SoundBankPlayerCosmetic : MonoBehaviour
	{
		// Token: 0x06003AD8 RID: 15064 RVA: 0x0010E9F0 File Offset: 0x0010CBF0
		private void Awake()
		{
			this.playAudioLoop = false;
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x0010E9FC File Offset: 0x0010CBFC
		public void Update()
		{
			if (!this.playAudioLoop)
			{
				return;
			}
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null && !this.soundBankPlayer.audioSource.isPlaying)
			{
				this.soundBankPlayer.Play();
			}
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x0010EA64 File Offset: 0x0010CC64
		public void PlayAudio()
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				this.soundBankPlayer.Play();
			}
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x0010EAB0 File Offset: 0x0010CCB0
		public void PlayAudioLoop()
		{
			this.playAudioLoop = true;
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x0010EABC File Offset: 0x0010CCBC
		public void PlayAudioNonInterrupting()
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				if (this.soundBankPlayer.audioSource.isPlaying)
				{
					return;
				}
				this.soundBankPlayer.Play();
			}
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x0010EB1C File Offset: 0x0010CD1C
		public void PlayAudioWithTunableVolume(bool leftHand, float fingerValue)
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				float volume = Mathf.Clamp01(fingerValue);
				this.soundBankPlayer.audioSource.volume = volume;
				this.soundBankPlayer.Play();
			}
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x0010EB80 File Offset: 0x0010CD80
		public void StopAudio()
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				this.soundBankPlayer.audioSource.Stop();
			}
			this.playAudioLoop = false;
		}

		// Token: 0x04003BDF RID: 15327
		[SerializeField]
		private SoundBankPlayer soundBankPlayer;

		// Token: 0x04003BE0 RID: 15328
		private bool playAudioLoop;
	}
}
