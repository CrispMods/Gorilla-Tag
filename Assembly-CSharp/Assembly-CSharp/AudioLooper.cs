using System;
using UnityEngine;

// Token: 0x0200082D RID: 2093
[RequireComponent(typeof(AudioSource))]
public class AudioLooper : MonoBehaviour
{
	// Token: 0x06003327 RID: 13095 RVA: 0x000F49A3 File Offset: 0x000F2BA3
	protected virtual void Awake()
	{
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x06003328 RID: 13096 RVA: 0x000F49B4 File Offset: 0x000F2BB4
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

	// Token: 0x04003691 RID: 13969
	private AudioSource audioSource;

	// Token: 0x04003692 RID: 13970
	[SerializeField]
	private AudioClip loopClip;

	// Token: 0x04003693 RID: 13971
	[SerializeField]
	private AudioClip[] interjectionClips;

	// Token: 0x04003694 RID: 13972
	[SerializeField]
	private float interjectionLikelyhood = 0.5f;
}
