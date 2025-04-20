using System;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class AudioFader : MonoBehaviour
{
	// Token: 0x06000888 RID: 2184 RVA: 0x0003602B File Offset: 0x0003422B
	private void Start()
	{
		this.fadeInSpeed = this.maxVolume / this.fadeInDuration;
		this.fadeOutSpeed = this.maxVolume / this.fadeOutDuration;
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x0008E558 File Offset: 0x0008C758
	public void FadeIn()
	{
		this.targetVolume = this.maxVolume;
		if (this.fadeInDuration > 0f)
		{
			base.enabled = true;
			this.currentFadeSpeed = this.fadeInSpeed;
		}
		else
		{
			this.currentVolume = this.maxVolume;
		}
		this.audioToFade.volume = this.currentVolume;
		if (!this.audioToFade.isPlaying)
		{
			this.audioToFade.Play();
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x0008E5C8 File Offset: 0x0008C7C8
	public void FadeOut()
	{
		this.targetVolume = 0f;
		if (this.fadeOutDuration > 0f)
		{
			base.enabled = true;
			this.currentFadeSpeed = this.fadeOutSpeed;
		}
		else
		{
			this.currentVolume = 0f;
			if (this.audioToFade.isPlaying)
			{
				this.audioToFade.Stop();
			}
		}
		if (this.outro != null && this.currentVolume > 0f)
		{
			this.outro.volume = this.currentVolume;
			this.outro.Play();
		}
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0008E65C File Offset: 0x0008C85C
	private void Update()
	{
		this.currentVolume = Mathf.MoveTowards(this.currentVolume, this.targetVolume, this.currentFadeSpeed * Time.deltaTime);
		this.audioToFade.volume = this.currentVolume;
		if (this.currentVolume == this.targetVolume)
		{
			base.enabled = false;
			if (this.currentVolume == 0f && this.audioToFade.isPlaying)
			{
				this.audioToFade.Stop();
			}
		}
	}

	// Token: 0x040009EF RID: 2543
	[SerializeField]
	private AudioSource audioToFade;

	// Token: 0x040009F0 RID: 2544
	[SerializeField]
	private AudioSource outro;

	// Token: 0x040009F1 RID: 2545
	[SerializeField]
	private float fadeInDuration = 0.3f;

	// Token: 0x040009F2 RID: 2546
	[SerializeField]
	private float fadeOutDuration = 0.3f;

	// Token: 0x040009F3 RID: 2547
	[SerializeField]
	private float maxVolume = 1f;

	// Token: 0x040009F4 RID: 2548
	private float currentVolume;

	// Token: 0x040009F5 RID: 2549
	private float targetVolume;

	// Token: 0x040009F6 RID: 2550
	private float currentFadeSpeed;

	// Token: 0x040009F7 RID: 2551
	private float fadeInSpeed;

	// Token: 0x040009F8 RID: 2552
	private float fadeOutSpeed;
}
