using System;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class ParticleEffectsPoolStatic<T> : ParticleEffectsPool where T : ParticleEffectsPool
{
	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000995 RID: 2453 RVA: 0x00036C4E File Offset: 0x00034E4E
	public static T Instance
	{
		get
		{
			return ParticleEffectsPoolStatic<T>.gInstance;
		}
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x00036C55 File Offset: 0x00034E55
	protected override void OnPoolAwake()
	{
		if (ParticleEffectsPoolStatic<T>.gInstance && ParticleEffectsPoolStatic<T>.gInstance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		ParticleEffectsPoolStatic<T>.gInstance = (this as T);
	}

	// Token: 0x04000B86 RID: 2950
	protected static T gInstance;
}
