using System;
using UnityEngine;

namespace DefaultNamespace
{
	// Token: 0x02000993 RID: 2451
	[RequireComponent(typeof(SoundBankPlayer))]
	public class SoundBankPlayerCosmetic : MonoBehaviour
	{
		// Token: 0x06003BF0 RID: 15344 RVA: 0x0005723D File Offset: 0x0005543D
		private void Awake()
		{
			this.playAudioLoop = false;
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x00152260 File Offset: 0x00150460
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

		// Token: 0x06003BF2 RID: 15346 RVA: 0x001522C8 File Offset: 0x001504C8
		public void PlayAudio()
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				this.soundBankPlayer.Play();
			}
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x00057246 File Offset: 0x00055446
		public void PlayAudioLoop()
		{
			this.playAudioLoop = true;
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x00152314 File Offset: 0x00150514
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

		// Token: 0x06003BF5 RID: 15349 RVA: 0x00152374 File Offset: 0x00150574
		public void PlayAudioWithTunableVolume(bool leftHand, float fingerValue)
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				float volume = Mathf.Clamp01(fingerValue);
				this.soundBankPlayer.audioSource.volume = volume;
				this.soundBankPlayer.Play();
			}
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x001523D8 File Offset: 0x001505D8
		public void StopAudio()
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				this.soundBankPlayer.audioSource.Stop();
			}
			this.playAudioLoop = false;
		}

		// Token: 0x04003CB9 RID: 15545
		[SerializeField]
		private SoundBankPlayer soundBankPlayer;

		// Token: 0x04003CBA RID: 15546
		private bool playAudioLoop;
	}
}
