using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200038F RID: 911
public class SlingshotProjectileManager : MonoBehaviour
{
	// Token: 0x06001564 RID: 5476 RVA: 0x000685DF File Offset: 0x000667DF
	protected void Awake()
	{
		if (SlingshotProjectileManager.hasInstance && SlingshotProjectileManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		SlingshotProjectileManager.SetInstance(this);
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x00068602 File Offset: 0x00066802
	public static void CreateManager()
	{
		SlingshotProjectileManager.SetInstance(new GameObject("SlingshotProjectileManager").AddComponent<SlingshotProjectileManager>());
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x00068618 File Offset: 0x00066818
	private static void SetInstance(SlingshotProjectileManager manager)
	{
		SlingshotProjectileManager.instance = manager;
		SlingshotProjectileManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x00068633 File Offset: 0x00066833
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

	// Token: 0x06001568 RID: 5480 RVA: 0x00068659 File Offset: 0x00066859
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

	// Token: 0x06001569 RID: 5481 RVA: 0x00068680 File Offset: 0x00066880
	public void Update()
	{
		for (int i = 0; i < SlingshotProjectileManager.allsP.Count; i++)
		{
			SlingshotProjectileManager.allsP[i].InvokeUpdate();
		}
	}

	// Token: 0x040017AD RID: 6061
	public static SlingshotProjectileManager instance;

	// Token: 0x040017AE RID: 6062
	public static bool hasInstance = false;

	// Token: 0x040017AF RID: 6063
	public static List<SlingshotProjectile> allsP = new List<SlingshotProjectile>();
}
