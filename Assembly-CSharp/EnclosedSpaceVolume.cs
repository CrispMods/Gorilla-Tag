using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class EnclosedSpaceVolume : GorillaTriggerBox
{
	// Token: 0x06001F8A RID: 8074 RVA: 0x0009EC76 File Offset: 0x0009CE76
	private void Awake()
	{
		this.audioSourceInside.volume = this.quietVolume;
		this.audioSourceOutside.volume = this.loudVolume;
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x0009EC9A File Offset: 0x0009CE9A
	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody.GetComponentInParent<GTPlayer>() != null)
		{
			this.audioSourceInside.volume = this.loudVolume;
			this.audioSourceOutside.volume = this.quietVolume;
		}
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x0009ECD1 File Offset: 0x0009CED1
	private void OnTriggerExit(Collider other)
	{
		if (other.attachedRigidbody.GetComponentInParent<GTPlayer>() != null)
		{
			this.audioSourceInside.volume = this.quietVolume;
			this.audioSourceOutside.volume = this.loudVolume;
		}
	}

	// Token: 0x04002365 RID: 9061
	public AudioSource audioSourceInside;

	// Token: 0x04002366 RID: 9062
	public AudioSource audioSourceOutside;

	// Token: 0x04002367 RID: 9063
	public float loudVolume;

	// Token: 0x04002368 RID: 9064
	public float quietVolume;
}
