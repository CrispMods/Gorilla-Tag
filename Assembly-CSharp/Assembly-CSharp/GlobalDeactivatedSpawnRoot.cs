using System;
using UnityEngine;

// Token: 0x02000187 RID: 391
public static class GlobalDeactivatedSpawnRoot
{
	// Token: 0x060009C3 RID: 2499 RVA: 0x00036E80 File Offset: 0x00035080
	public static Transform GetOrCreate()
	{
		if (!GlobalDeactivatedSpawnRoot._xform)
		{
			GlobalDeactivatedSpawnRoot._xform = new GameObject("GlobalDeactivatedSpawnRoot").transform;
			GlobalDeactivatedSpawnRoot._xform.gameObject.SetActive(false);
			Object.DontDestroyOnLoad(GlobalDeactivatedSpawnRoot._xform.gameObject);
		}
		GlobalDeactivatedSpawnRoot._xform.gameObject.SetActive(false);
		return GlobalDeactivatedSpawnRoot._xform;
	}

	// Token: 0x04000BD9 RID: 3033
	private static Transform _xform;
}
