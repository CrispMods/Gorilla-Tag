using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005E8 RID: 1512
public class SizeManagerManager : MonoBehaviour
{
	// Token: 0x06002594 RID: 9620 RVA: 0x0004882D File Offset: 0x00046A2D
	protected void Awake()
	{
		if (SizeManagerManager.hasInstance && SizeManagerManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		SizeManagerManager.SetInstance(this);
	}

	// Token: 0x06002595 RID: 9621 RVA: 0x00048850 File Offset: 0x00046A50
	public static void CreateManager()
	{
		SizeManagerManager.SetInstance(new GameObject("SizeManagerManager").AddComponent<SizeManagerManager>());
	}

	// Token: 0x06002596 RID: 9622 RVA: 0x00048866 File Offset: 0x00046A66
	private static void SetInstance(SizeManagerManager manager)
	{
		SizeManagerManager.instance = manager;
		SizeManagerManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06002597 RID: 9623 RVA: 0x00048881 File Offset: 0x00046A81
	public static void RegisterSM(SizeManager sM)
	{
		if (!SizeManagerManager.hasInstance)
		{
			SizeManagerManager.CreateManager();
		}
		if (!SizeManagerManager.allSM.Contains(sM))
		{
			SizeManagerManager.allSM.Add(sM);
		}
	}

	// Token: 0x06002598 RID: 9624 RVA: 0x000488A7 File Offset: 0x00046AA7
	public static void UnregisterSM(SizeManager sM)
	{
		if (!SizeManagerManager.hasInstance)
		{
			SizeManagerManager.CreateManager();
		}
		if (SizeManagerManager.allSM.Contains(sM))
		{
			SizeManagerManager.allSM.Remove(sM);
		}
	}

	// Token: 0x06002599 RID: 9625 RVA: 0x00104730 File Offset: 0x00102930
	public void FixedUpdate()
	{
		for (int i = 0; i < SizeManagerManager.allSM.Count; i++)
		{
			SizeManagerManager.allSM[i].InvokeFixedUpdate();
		}
	}

	// Token: 0x040029CF RID: 10703
	[OnEnterPlay_SetNull]
	public static SizeManagerManager instance;

	// Token: 0x040029D0 RID: 10704
	[OnEnterPlay_Set(false)]
	public static bool hasInstance = false;

	// Token: 0x040029D1 RID: 10705
	[OnEnterPlay_Clear]
	public static List<SizeManager> allSM = new List<SizeManager>();
}
