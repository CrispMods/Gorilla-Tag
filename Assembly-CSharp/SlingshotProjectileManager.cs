using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200039A RID: 922
public class SlingshotProjectileManager : MonoBehaviour
{
	// Token: 0x060015B0 RID: 5552 RVA: 0x0003EA09 File Offset: 0x0003CC09
	protected void Awake()
	{
		if (SlingshotProjectileManager.hasInstance && SlingshotProjectileManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		SlingshotProjectileManager.SetInstance(this);
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x0003EA2C File Offset: 0x0003CC2C
	public static void CreateManager()
	{
		SlingshotProjectileManager.SetInstance(new GameObject("SlingshotProjectileManager").AddComponent<SlingshotProjectileManager>());
	}

	// Token: 0x060015B2 RID: 5554 RVA: 0x0003EA42 File Offset: 0x0003CC42
	private static void SetInstance(SlingshotProjectileManager manager)
	{
		SlingshotProjectileManager.instance = manager;
		SlingshotProjectileManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060015B3 RID: 5555 RVA: 0x0003EA5D File Offset: 0x0003CC5D
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

	// Token: 0x060015B4 RID: 5556 RVA: 0x0003EA83 File Offset: 0x0003CC83
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

	// Token: 0x060015B5 RID: 5557 RVA: 0x000C0C7C File Offset: 0x000BEE7C
	public void Update()
	{
		for (int i = 0; i < SlingshotProjectileManager.allsP.Count; i++)
		{
			SlingshotProjectileManager.allsP[i].InvokeUpdate();
		}
	}

	// Token: 0x040017F4 RID: 6132
	public static SlingshotProjectileManager instance;

	// Token: 0x040017F5 RID: 6133
	public static bool hasInstance = false;

	// Token: 0x040017F6 RID: 6134
	public static List<SlingshotProjectile> allsP = new List<SlingshotProjectile>();
}
