using System;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class ParticleEffectsPoolStatic<T> : ParticleEffectsPool where T : ParticleEffectsPool
{
	// Token: 0x170000EA RID: 234
	// (get) Token: 0x0600094A RID: 2378 RVA: 0x00035983 File Offset: 0x00033B83
	public static T Instance
	{
		get
		{
			return ParticleEffectsPoolStatic<T>.gInstance;
		}
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0003598A File Offset: 0x00033B8A
	protected override void OnPoolAwake()
	{
		if (ParticleEffectsPoolStatic<T>.gInstance && ParticleEffectsPoolStatic<T>.gInstance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		ParticleEffectsPoolStatic<T>.gInstance = (this as T);
	}

	// Token: 0x04000B40 RID: 2880
	protected static T gInstance;
}
