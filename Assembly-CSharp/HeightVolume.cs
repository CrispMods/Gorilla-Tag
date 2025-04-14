using System;
using UnityEngine;

// Token: 0x020005BA RID: 1466
public class HeightVolume : MonoBehaviour
{
	// Token: 0x06002469 RID: 9321 RVA: 0x000B54B5 File Offset: 0x000B36B5
	private void Awake()
	{
		if (this.targetTransform == null)
		{
			this.targetTransform = Camera.main.transform;
		}
		this.musicSource = this.audioSource.gameObject.GetComponent<MusicSource>();
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x000B54EC File Offset: 0x000B36EC
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

	// Token: 0x04002884 RID: 10372
	public Transform heightTop;

	// Token: 0x04002885 RID: 10373
	public Transform heightBottom;

	// Token: 0x04002886 RID: 10374
	public AudioSource audioSource;

	// Token: 0x04002887 RID: 10375
	public float baseVolume;

	// Token: 0x04002888 RID: 10376
	public float minVolume;

	// Token: 0x04002889 RID: 10377
	public Transform targetTransform;

	// Token: 0x0400288A RID: 10378
	public bool invertHeightVol;

	// Token: 0x0400288B RID: 10379
	private MusicSource musicSource;
}
