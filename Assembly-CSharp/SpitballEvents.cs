using System;
using UnityEngine;

// Token: 0x02000445 RID: 1093
public class SpitballEvents : SubEmitterListener
{
	// Token: 0x06001AE6 RID: 6886 RVA: 0x00084605 File Offset: 0x00082805
	protected override void OnSubEmit()
	{
		base.OnSubEmit();
		if (this._audioSource && this._sfxHit)
		{
			this._audioSource.GTPlayOneShot(this._sfxHit, 1f);
		}
	}

	// Token: 0x04001DC5 RID: 7621
	[SerializeField]
	private AudioSource _audioSource;

	// Token: 0x04001DC6 RID: 7622
	[SerializeField]
	private AudioClip _sfxHit;
}
