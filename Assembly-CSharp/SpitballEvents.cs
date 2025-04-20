using System;
using UnityEngine;

// Token: 0x02000451 RID: 1105
public class SpitballEvents : SubEmitterListener
{
	// Token: 0x06001B3A RID: 6970 RVA: 0x0004280B File Offset: 0x00040A0B
	protected override void OnSubEmit()
	{
		base.OnSubEmit();
		if (this._audioSource && this._sfxHit)
		{
			this._audioSource.GTPlayOneShot(this._sfxHit, 1f);
		}
	}

	// Token: 0x04001E14 RID: 7700
	[SerializeField]
	private AudioSource _audioSource;

	// Token: 0x04001E15 RID: 7701
	[SerializeField]
	private AudioClip _sfxHit;
}
