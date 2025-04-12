using System;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class AudioFader : MonoBehaviour
{
	// Token: 0x06000846 RID: 2118 RVA: 0x00034DB5 File Offset: 0x00032FB5
	private void Start()
	{
		this.fadeInSpeed = this.maxVolume / this.fadeInDuration;
		this.fadeOutSpeed = this.maxVolume / this.fadeOutDuration;
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x0008BBD0 File Offset: 0x00089DD0
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

	// Token: 0x06000848 RID: 2120 RVA: 0x0008BC40 File Offset: 0x00089E40
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

	// Token: 0x06000849 RID: 2121 RVA: 0x0008BCD4 File Offset: 0x00089ED4
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

	// Token: 0x040009AD RID: 2477
	[SerializeField]
	private AudioSource audioToFade;

	// Token: 0x040009AE RID: 2478
	[SerializeField]
	private AudioSource outro;

	// Token: 0x040009AF RID: 2479
	[SerializeField]
	private float fadeInDuration = 0.3f;

	// Token: 0x040009B0 RID: 2480
	[SerializeField]
	private float fadeOutDuration = 0.3f;

	// Token: 0x040009B1 RID: 2481
	[SerializeField]
	private float maxVolume = 1f;

	// Token: 0x040009B2 RID: 2482
	private float currentVolume;

	// Token: 0x040009B3 RID: 2483
	private float targetVolume;

	// Token: 0x040009B4 RID: 2484
	private float currentFadeSpeed;

	// Token: 0x040009B5 RID: 2485
	private float fadeInSpeed;

	// Token: 0x040009B6 RID: 2486
	private float fadeOutSpeed;
}
