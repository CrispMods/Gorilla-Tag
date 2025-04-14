using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class EnclosedSpaceVolume : GorillaTriggerBox
{
	// Token: 0x06001F8D RID: 8077 RVA: 0x0009EFFA File Offset: 0x0009D1FA
	private void Awake()
	{
		this.audioSourceInside.volume = this.quietVolume;
		this.audioSourceOutside.volume = this.loudVolume;
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x0009F01E File Offset: 0x0009D21E
	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody.GetComponentInParent<GTPlayer>() != null)
		{
			this.audioSourceInside.volume = this.loudVolume;
			this.audioSourceOutside.volume = this.quietVolume;
		}
	}

	// Token: 0x06001F8F RID: 8079 RVA: 0x0009F055 File Offset: 0x0009D255
	private void OnTriggerExit(Collider other)
	{
		if (other.attachedRigidbody.GetComponentInParent<GTPlayer>() != null)
		{
			this.audioSourceInside.volume = this.quietVolume;
			this.audioSourceOutside.volume = this.loudVolume;
		}
	}

	// Token: 0x04002366 RID: 9062
	public AudioSource audioSourceInside;

	// Token: 0x04002367 RID: 9063
	public AudioSource audioSourceOutside;

	// Token: 0x04002368 RID: 9064
	public float loudVolume;

	// Token: 0x04002369 RID: 9065
	public float quietVolume;
}
