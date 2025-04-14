using System;
using UnityEngine;

// Token: 0x02000187 RID: 391
public static class GlobalDeactivatedSpawnRoot
{
	// Token: 0x060009C1 RID: 2497 RVA: 0x00036B5C File Offset: 0x00034D5C
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

	// Token: 0x04000BD8 RID: 3032
	private static Transform _xform;
}
