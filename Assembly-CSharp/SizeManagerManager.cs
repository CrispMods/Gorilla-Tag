using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005E7 RID: 1511
public class SizeManagerManager : MonoBehaviour
{
	// Token: 0x0600258C RID: 9612 RVA: 0x000B9A0E File Offset: 0x000B7C0E
	protected void Awake()
	{
		if (SizeManagerManager.hasInstance && SizeManagerManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		SizeManagerManager.SetInstance(this);
	}

	// Token: 0x0600258D RID: 9613 RVA: 0x000B9A31 File Offset: 0x000B7C31
	public static void CreateManager()
	{
		SizeManagerManager.SetInstance(new GameObject("SizeManagerManager").AddComponent<SizeManagerManager>());
	}

	// Token: 0x0600258E RID: 9614 RVA: 0x000B9A47 File Offset: 0x000B7C47
	private static void SetInstance(SizeManagerManager manager)
	{
		SizeManagerManager.instance = manager;
		SizeManagerManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600258F RID: 9615 RVA: 0x000B9A62 File Offset: 0x000B7C62
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

	// Token: 0x06002590 RID: 9616 RVA: 0x000B9A88 File Offset: 0x000B7C88
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

	// Token: 0x06002591 RID: 9617 RVA: 0x000B9AB0 File Offset: 0x000B7CB0
	public void FixedUpdate()
	{
		for (int i = 0; i < SizeManagerManager.allSM.Count; i++)
		{
			SizeManagerManager.allSM[i].InvokeFixedUpdate();
		}
	}

	// Token: 0x040029C9 RID: 10697
	[OnEnterPlay_SetNull]
	public static SizeManagerManager instance;

	// Token: 0x040029CA RID: 10698
	[OnEnterPlay_Set(false)]
	public static bool hasInstance = false;

	// Token: 0x040029CB RID: 10699
	[OnEnterPlay_Clear]
	public static List<SizeManager> allSM = new List<SizeManager>();
}
