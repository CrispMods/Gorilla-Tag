using System;
using UnityEngine;

// Token: 0x02000844 RID: 2116
[RequireComponent(typeof(AudioSource))]
public class AudioLooper : MonoBehaviour
{
	// Token: 0x060033D6 RID: 13270 RVA: 0x00052134 File Offset: 0x00050334
	protected virtual void Awake()
	{
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x060033D7 RID: 13271 RVA: 0x0013C1EC File Offset: 0x0013A3EC
	private void Update()
	{
		if (!this.audioSource.isPlaying)
		{
			if (this.audioSource.clip == this.loopClip && this.interjectionClips.Length != 0 && UnityEngine.Random.value < this.interjectionLikelyhood)
			{
				this.audioSource.clip = this.interjectionClips[UnityEngine.Random.Range(0, this.interjectionClips.Length)];
			}
			else
			{
				this.audioSource.clip = this.loopClip;
			}
			this.audioSource.GTPlay();
		}
	}

	// Token: 0x0400373B RID: 14139
	private AudioSource audioSource;

	// Token: 0x0400373C RID: 14140
	[SerializeField]
	private AudioClip loopClip;

	// Token: 0x0400373D RID: 14141
	[SerializeField]
	private AudioClip[] interjectionClips;

	// Token: 0x0400373E RID: 14142
	[SerializeField]
	private float interjectionLikelyhood = 0.5f;
}
