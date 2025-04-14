using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005E8 RID: 1512
public class SizeManagerManager : MonoBehaviour
{
	// Token: 0x06002594 RID: 9620 RVA: 0x000B9E8E File Offset: 0x000B808E
	protected void Awake()
	{
		if (SizeManagerManager.hasInstance && SizeManagerManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		SizeManagerManager.SetInstance(this);
	}

	// Token: 0x06002595 RID: 9621 RVA: 0x000B9EB1 File Offset: 0x000B80B1
	public static void CreateManager()
	{
		SizeManagerManager.SetInstance(new GameObject("SizeManagerManager").AddComponent<SizeManagerManager>());
	}

	// Token: 0x06002596 RID: 9622 RVA: 0x000B9EC7 File Offset: 0x000B80C7
	private static void SetInstance(SizeManagerManager manager)
	{
		SizeManagerManager.instance = manager;
		SizeManagerManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06002597 RID: 9623 RVA: 0x000B9EE2 File Offset: 0x000B80E2
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

	// Token: 0x06002598 RID: 9624 RVA: 0x000B9F08 File Offset: 0x000B8108
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

	// Token: 0x06002599 RID: 9625 RVA: 0x000B9F30 File Offset: 0x000B8130
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
