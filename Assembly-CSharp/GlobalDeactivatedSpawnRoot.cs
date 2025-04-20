using System;
using UnityEngine;

// Token: 0x02000192 RID: 402
public static class GlobalDeactivatedSpawnRoot
{
	// Token: 0x06000A0D RID: 2573 RVA: 0x00096C3C File Offset: 0x00094E3C
	public static Transform GetOrCreate()
	{
		if (!GlobalDeactivatedSpawnRoot._xform)
		{
			GlobalDeactivatedSpawnRoot._xform = new GameObject("GlobalDeactivatedSpawnRoot").transform;
			GlobalDeactivatedSpawnRoot._xform.gameObject.SetActive(false);
			UnityEngine.Object.DontDestroyOnLoad(GlobalDeactivatedSpawnRoot._xform.gameObject);
		}
		GlobalDeactivatedSpawnRoot._xform.gameObject.SetActive(false);
		return GlobalDeactivatedSpawnRoot._xform;
	}

	// Token: 0x04000C1E RID: 3102
	private static Transform _xform;
}
