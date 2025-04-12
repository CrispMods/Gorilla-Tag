using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffect : MonoBehaviour
{
	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x000357C7 File Offset: 0x000339C7
	public long effectID
	{
		get
		{
			return this._effectID;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000930 RID: 2352 RVA: 0x000357CF File Offset: 0x000339CF
	public bool isPlaying
	{
		get
		{
			return this.system && this.system.isPlaying;
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x000357EB File Offset: 0x000339EB
	public virtual void Play()
	{
		base.gameObject.SetActive(true);
		this.system.Play(true);
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x00035805 File Offset: 0x00033A05
	public virtual void Stop()
	{
		this.system.Stop(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0003581F File Offset: 0x00033A1F
	private void OnParticleSystemStopped()
	{
		base.gameObject.SetActive(false);
		if (this.pool)
		{
			this.pool.Return(this);
		}
	}

	// Token: 0x04000B32 RID: 2866
	public ParticleSystem system;

	// Token: 0x04000B33 RID: 2867
	[SerializeField]
	private long _effectID;

	// Token: 0x04000B34 RID: 2868
	public ParticleEffectsPool pool;

	// Token: 0x04000B35 RID: 2869
	[NonSerialized]
	public int poolIndex = -1;
}
