using System;
using UnityEngine;

// Token: 0x0200017B RID: 379
[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffect : MonoBehaviour
{
	// Token: 0x170000ED RID: 237
	// (get) Token: 0x0600097A RID: 2426 RVA: 0x00036A92 File Offset: 0x00034C92
	public long effectID
	{
		get
		{
			return this._effectID;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x0600097B RID: 2427 RVA: 0x00036A9A File Offset: 0x00034C9A
	public bool isPlaying
	{
		get
		{
			return this.system && this.system.isPlaying;
		}
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00036AB6 File Offset: 0x00034CB6
	public virtual void Play()
	{
		base.gameObject.SetActive(true);
		this.system.Play(true);
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00036AD0 File Offset: 0x00034CD0
	public virtual void Stop()
	{
		this.system.Stop(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x00036AEA File Offset: 0x00034CEA
	private void OnParticleSystemStopped()
	{
		base.gameObject.SetActive(false);
		if (this.pool)
		{
			this.pool.Return(this);
		}
	}

	// Token: 0x04000B78 RID: 2936
	public ParticleSystem system;

	// Token: 0x04000B79 RID: 2937
	[SerializeField]
	private long _effectID;

	// Token: 0x04000B7A RID: 2938
	public ParticleEffectsPool pool;

	// Token: 0x04000B7B RID: 2939
	[NonSerialized]
	public int poolIndex = -1;
}
