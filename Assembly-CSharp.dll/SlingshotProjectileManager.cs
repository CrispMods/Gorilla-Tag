using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200038F RID: 911
public class SlingshotProjectileManager : MonoBehaviour
{
	// Token: 0x06001567 RID: 5479 RVA: 0x0003D749 File Offset: 0x0003B949
	protected void Awake()
	{
		if (SlingshotProjectileManager.hasInstance && SlingshotProjectileManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		SlingshotProjectileManager.SetInstance(this);
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x0003D76C File Offset: 0x0003B96C
	public static void CreateManager()
	{
		SlingshotProjectileManager.SetInstance(new GameObject("SlingshotProjectileManager").AddComponent<SlingshotProjectileManager>());
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x0003D782 File Offset: 0x0003B982
	private static void SetInstance(SlingshotProjectileManager manager)
	{
		SlingshotProjectileManager.instance = manager;
		SlingshotProjectileManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x0003D79D File Offset: 0x0003B99D
	public static void RegisterSP(SlingshotProjectile sP)
	{
		if (!SlingshotProjectileManager.hasInstance)
		{
			SlingshotProjectileManager.CreateManager();
		}
		if (!SlingshotProjectileManager.allsP.Contains(sP))
		{
			SlingshotProjectileManager.allsP.Add(sP);
		}
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x0003D7C3 File Offset: 0x0003B9C3
	public static void UnregisterSP(SlingshotProjectile sP)
	{
		if (!SlingshotProjectileManager.hasInstance)
		{
			SlingshotProjectileManager.CreateManager();
		}
		if (SlingshotProjectileManager.allsP.Contains(sP))
		{
			SlingshotProjectileManager.allsP.Remove(sP);
		}
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x000BE444 File Offset: 0x000BC644
	public void Update()
	{
		for (int i = 0; i < SlingshotProjectileManager.allsP.Count; i++)
		{
			SlingshotProjectileManager.allsP[i].InvokeUpdate();
		}
	}

	// Token: 0x040017AE RID: 6062
	public static SlingshotProjectileManager instance;

	// Token: 0x040017AF RID: 6063
	public static bool hasInstance = false;

	// Token: 0x040017B0 RID: 6064
	public static List<SlingshotProjectile> allsP = new List<SlingshotProjectile>();
}
