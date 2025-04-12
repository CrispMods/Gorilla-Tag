using System;
using UnityEngine;

namespace DefaultNamespace
{
	// Token: 0x02000970 RID: 2416
	[RequireComponent(typeof(SoundBankPlayer))]
	public class SoundBankPlayerCosmetic : MonoBehaviour
	{
		// Token: 0x06003AE4 RID: 15076 RVA: 0x000559A6 File Offset: 0x00053BA6
		private void Awake()
		{
			this.playAudioLoop = false;
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0014C278 File Offset: 0x0014A478
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

		// Token: 0x06003AE6 RID: 15078 RVA: 0x0014C2E0 File Offset: 0x0014A4E0
		public void PlayAudio()
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				this.soundBankPlayer.Play();
			}
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x000559AF File Offset: 0x00053BAF
		public void PlayAudioLoop()
		{
			this.playAudioLoop = true;
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x0014C32C File Offset: 0x0014A52C
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

		// Token: 0x06003AE9 RID: 15081 RVA: 0x0014C38C File Offset: 0x0014A58C
		public void PlayAudioWithTunableVolume(bool leftHand, float fingerValue)
		{
			if (this.soundBankPlayer != null && this.soundBankPlayer.audioSource != null && this.soundBankPlayer.soundBank != null)
			{
				float volume = Mathf.Clamp01(fingerValue);
				this.soundBankPlayer.audioSource.volume = volume;
				this.soundBankPlayer.Play();
			}
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x0014C3F0 File Offset: 0x0014A5F0
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
