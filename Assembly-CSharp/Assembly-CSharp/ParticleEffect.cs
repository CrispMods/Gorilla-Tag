using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffect : MonoBehaviour
{
	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x00031BB2 File Offset: 0x0002FDB2
	public long effectID
	{
		get
		{
			return this._effectID;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000930 RID: 2352 RVA: 0x00031BBA File Offset: 0x0002FDBA
	public bool isPlaying
	{
		get
		{
			return this.system && this.system.isPlaying;
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x00031BD6 File Offset: 0x0002FDD6
	public virtual void Play()
	{
		base.gameObject.SetActive(true);
		this.system.Play(true);
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x00031BF0 File Offset: 0x0002FDF0
	public virtual void Stop()
	{
		this.system.Stop(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00031C0A File Offset: 0x0002FE0A
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
