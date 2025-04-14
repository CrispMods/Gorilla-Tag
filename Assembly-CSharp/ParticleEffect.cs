using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffect : MonoBehaviour
{
	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x0600092D RID: 2349 RVA: 0x0003188E File Offset: 0x0002FA8E
	public long effectID
	{
		get
		{
			return this._effectID;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x0600092E RID: 2350 RVA: 0x00031896 File Offset: 0x0002FA96
	public bool isPlaying
	{
		get
		{
			return this.system && this.system.isPlaying;
		}
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x000318B2 File Offset: 0x0002FAB2
	public virtual void Play()
	{
		base.gameObject.SetActive(true);
		this.system.Play(true);
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x000318CC File Offset: 0x0002FACC
	public virtual void Stop()
	{
		this.system.Stop(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x000318E6 File Offset: 0x0002FAE6
	private void OnParticleSystemStopped()
	{
		base.gameObject.SetActive(false);
		if (this.pool)
		{
			this.pool.Return(this);
		}
	}

	// Token: 0x04000B31 RID: 2865
	public ParticleSystem system;

	// Token: 0x04000B32 RID: 2866
	[SerializeField]
	private long _effectID;

	// Token: 0x04000B33 RID: 2867
	public ParticleEffectsPool pool;

	// Token: 0x04000B34 RID: 2868
	[NonSerialized]
	public int poolIndex = -1;
}
