using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200038F RID: 911
public class SlingshotProjectileManager : MonoBehaviour
{
	// Token: 0x06001567 RID: 5479 RVA: 0x00068963 File Offset: 0x00066B63
	protected void Awake()
	{
		if (SlingshotProjectileManager.hasInstance && SlingshotProjectileManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		SlingshotProjectileManager.SetInstance(this);
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x00068986 File Offset: 0x00066B86
	public static void CreateManager()
	{
		SlingshotProjectileManager.SetInstance(new GameObject("SlingshotProjectileManager").AddComponent<SlingshotProjectileManager>());
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x0006899C File Offset: 0x00066B9C
	private static void SetInstance(SlingshotProjectileManager manager)
	{
		SlingshotProjectileManager.instance = manager;
		SlingshotProjectileManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x000689B7 File Offset: 0x00066BB7
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

	// Token: 0x0600156B RID: 5483 RVA: 0x000689DD File Offset: 0x00066BDD
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

	// Token: 0x0600156C RID: 5484 RVA: 0x00068A04 File Offset: 0x00066C04
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
