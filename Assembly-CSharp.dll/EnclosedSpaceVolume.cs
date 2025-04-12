using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class EnclosedSpaceVolume : GorillaTriggerBox
{
	// Token: 0x06001F8D RID: 8077 RVA: 0x000446F8 File Offset: 0x000428F8
	private void Awake()
	{
		this.audioSourceInside.volume = this.quietVolume;
		this.audioSourceOutside.volume = this.loudVolume;
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x0004471C File Offset: 0x0004291C
	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody.GetComponentInParent<GTPlayer>() != null)
		{
			this.audioSourceInside.volume = this.loudVolume;
			this.audioSourceOutside.volume = this.quietVolume;
		}
	}

	// Token: 0x06001F8F RID: 8079 RVA: 0x00044753 File Offset: 0x00042953
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
