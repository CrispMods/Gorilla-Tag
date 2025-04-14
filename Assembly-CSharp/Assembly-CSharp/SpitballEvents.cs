using System;
using UnityEngine;

// Token: 0x02000445 RID: 1093
public class SpitballEvents : SubEmitterListener
{
	// Token: 0x06001AE9 RID: 6889 RVA: 0x00084989 File Offset: 0x00082B89
	protected override void OnSubEmit()
	{
		base.OnSubEmit();
		if (this._audioSource && this._sfxHit)
		{
			this._audioSource.GTPlayOneShot(this._sfxHit, 1f);
		}
	}

	// Token: 0x04001DC6 RID: 7622
	[SerializeField]
	private AudioSource _audioSource;

	// Token: 0x04001DC7 RID: 7623
	[SerializeField]
	private AudioClip _sfxHit;
}
