using System;
using UnityEngine;

// Token: 0x020005C8 RID: 1480
public class HeightVolume : MonoBehaviour
{
	// Token: 0x060024CB RID: 9419 RVA: 0x00048F30 File Offset: 0x00047130
	private void Awake()
	{
		if (this.targetTransform == null)
		{
			this.targetTransform = Camera.main.transform;
		}
		this.musicSource = this.audioSource.gameObject.GetComponent<MusicSource>();
	}

	// Token: 0x060024CC RID: 9420 RVA: 0x00103D84 File Offset: 0x00101F84
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

	// Token: 0x040028E1 RID: 10465
	public Transform heightTop;

	// Token: 0x040028E2 RID: 10466
	public Transform heightBottom;

	// Token: 0x040028E3 RID: 10467
	public AudioSource audioSource;

	// Token: 0x040028E4 RID: 10468
	public float baseVolume;

	// Token: 0x040028E5 RID: 10469
	public float minVolume;

	// Token: 0x040028E6 RID: 10470
	public Transform targetTransform;

	// Token: 0x040028E7 RID: 10471
	public bool invertHeightVol;

	// Token: 0x040028E8 RID: 10472
	private MusicSource musicSource;
}
