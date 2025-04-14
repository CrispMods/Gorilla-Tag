using System;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class ParticleEffectsPoolStatic<T> : ParticleEffectsPool where T : ParticleEffectsPool
{
	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000948 RID: 2376 RVA: 0x00031BFF File Offset: 0x0002FDFF
	public static T Instance
	{
		get
		{
			return ParticleEffectsPoolStatic<T>.gInstance;
		}
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x00031C06 File Offset: 0x0002FE06
	protected override void OnPoolAwake()
	{
		if (ParticleEffectsPoolStatic<T>.gInstance && ParticleEffectsPoolStatic<T>.gInstance != this)
		{
			Object.Destroy(this);
			return;
		}
		ParticleEffectsPoolStatic<T>.gInstance = (this as T);
	}

	// Token: 0x04000B3F RID: 2879
	protected static T gInstance;
}
