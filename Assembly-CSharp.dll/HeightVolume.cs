using System;
using UnityEngine;

// Token: 0x020005BB RID: 1467
public class HeightVolume : MonoBehaviour
{
	// Token: 0x06002471 RID: 9329 RVA: 0x00047B27 File Offset: 0x00045D27
	private void Awake()
	{
		if (this.targetTransform == null)
		{
			this.targetTransform = Camera.main.transform;
		}
		this.musicSource = this.audioSource.gameObject.GetComponent<MusicSource>();
	}

	// Token: 0x06002472 RID: 9330 RVA: 0x00100ED0 File Offset: 0x000FF0D0
	private void Update()
	{
		if (this.audioSource.gameObject.activeSelf && (!(this.musicSource != null) || !this.musicSource.VolumeOverridden))
		{
			if (this.targetTransform.position.y > this.heightTop.position.y)
			{
				this.audioSource.volume = ((!this.invertHeightVol) ? this.baseVolume : this.minVolume);
				return;
			}
			if (this.targetTransform.position.y < this.heightBottom.position.y)
			{
				this.audioSource.volume = ((!this.invertHeightVol) ? this.minVolume : this.baseVolume);
				return;
			}
			this.audioSource.volume = ((!this.invertHeightVol) ? ((this.targetTransform.position.y - this.heightBottom.position.y) / (this.heightTop.position.y - this.heightBottom.position.y) * (this.baseVolume - this.minVolume) + this.minVolume) : ((this.heightTop.position.y - this.targetTransform.position.y) / (this.heightTop.position.y - this.heightBottom.position.y) * (this.baseVolume - this.minVolume) + this.minVolume));
		}
	}

	// Token: 0x0400288A RID: 10378
	public Transform heightTop;

	// Token: 0x0400288B RID: 10379
	public Transform heightBottom;

	// Token: 0x0400288C RID: 10380
	public AudioSource audioSource;

	// Token: 0x0400288D RID: 10381
	public float baseVolume;

	// Token: 0x0400288E RID: 10382
	public float minVolume;

	// Token: 0x0400288F RID: 10383
	public Transform targetTransform;

	// Token: 0x04002890 RID: 10384
	public bool invertHeightVol;

	// Token: 0x04002891 RID: 10385
	private MusicSource musicSource;
}
