using System;
using UnityEngine;

// Token: 0x0200082A RID: 2090
[RequireComponent(typeof(AudioSource))]
public class AudioLooper : MonoBehaviour
{
	// Token: 0x0600331B RID: 13083 RVA: 0x000F43DB File Offset: 0x000F25DB
	protected virtual void Awake()
	{
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x000F43EC File Offset: 0x000F25EC
	private void Update()
	{
		if (!this.audioSource.isPlaying)
		{
			if (this.audioSource.clip == this.loopClip && this.interjectionClips.Length != 0 && Random.value < this.interjectionLikelyhood)
			{
				this.audioSource.clip = this.interjectionClips[Random.Range(0, this.interjectionClips.Length)];
			}
			else
			{
				this.audioSource.clip = this.loopClip;
			}
			this.audioSource.GTPlay();
		}
	}

	// Token: 0x0400367F RID: 13951
	private AudioSource audioSource;

	// Token: 0x04003680 RID: 13952
	[SerializeField]
	private AudioClip loopClip;

	// Token: 0x04003681 RID: 13953
	[SerializeField]
	private AudioClip[] interjectionClips;

	// Token: 0x04003682 RID: 13954
	[SerializeField]
	private float interjectionLikelyhood = 0.5f;
}
