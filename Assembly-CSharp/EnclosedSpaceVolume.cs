using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000523 RID: 1315
public class EnclosedSpaceVolume : GorillaTriggerBox
{
	// Token: 0x06001FE3 RID: 8163 RVA: 0x00045A97 File Offset: 0x00043C97
	private void Awake()
	{
		this.audioSourceInside.volume = this.quietVolume;
		this.audioSourceOutside.volume = this.loudVolume;
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x00045ABB File Offset: 0x00043CBB
	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody.GetComponentInParent<GTPlayer>() != null)
		{
			this.audioSourceInside.volume = this.loudVolume;
			this.audioSourceOutside.volume = this.quietVolume;
		}
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x00045AF2 File Offset: 0x00043CF2
	private void OnTriggerExit(Collider other)
	{
		if (other.attachedRigidbody.GetComponentInParent<GTPlayer>() != null)
		{
			this.audioSourceInside.volume = this.quietVolume;
			this.audioSourceOutside.volume = this.loudVolume;
		}
	}

	// Token: 0x040023B8 RID: 9144
	public AudioSource audioSourceInside;

	// Token: 0x040023B9 RID: 9145
	public AudioSource audioSourceOutside;

	// Token: 0x040023BA RID: 9146
	public float loudVolume;

	// Token: 0x040023BB RID: 9147
	public float quietVolume;
}
