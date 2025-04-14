using System;
using UnityEngine;

namespace DefaultNamespace
{
	// Token: 0x02000970 RID: 2416
	[RequireComponent(typeof(SoundBankPlayer))]
	public class SoundBankPlayerCosmetic : MonoBehaviour
	{
		// Token: 0x06003AE4 RID: 15076 RVA: 0x0010EFB8 File Offset: 0x0010D1B8
		private void Awake()
		{
			this.playAudioLoop = false;
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0010EFC4 File Offset: 0x0010D1C4
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

		// Token: 0x06003AE6 RID: 15078 RVA: 0x0010F02C File Offset: 0x0010D22C
		public void PlayAudio()
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				this.soundBankPlayer.Play();
			}
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x0010F078 File Offset: 0x0010D278
		public void PlayAudioLoop()
		{
			this.playAudioLoop = true;
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x0010F084 File Offset: 0x0010D284
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

		// Token: 0x06003AE9 RID: 15081 RVA: 0x0010F0E4 File Offset: 0x0010D2E4
		public void PlayAudioWithTunableVolume(bool leftHand, float fingerValue)
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				float volume = Mathf.Clamp01(fingerValue);
				this.soundBankPlayer.audioSource.volume = volume;
				this.soundBankPlayer.Play();
			}
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x0010F148 File Offset: 0x0010D348
		public void StopAudio()
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				this.soundBankPlayer.audioSource.Stop();
			}
			this.playAudioLoop = false;
		}

		// Token: 0x04003BF1 RID: 15345
		[SerializeField]
		private SoundBankPlayer soundBankPlayer;

		// Token: 0x04003BF2 RID: 15346
		private bool playAudioLoop;
	}
}
