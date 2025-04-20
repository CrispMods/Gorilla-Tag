using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005F5 RID: 1525
public class SizeManagerManager : MonoBehaviour
{
	// Token: 0x060025EE RID: 9710 RVA: 0x00049C04 File Offset: 0x00047E04
	protected void Awake()
	{
		if (SizeManagerManager.hasInstance && SizeManagerManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		SizeManagerManager.SetInstance(this);
	}

	// Token: 0x060025EF RID: 9711 RVA: 0x00049C27 File Offset: 0x00047E27
	public static void CreateManager()
	{
		SizeManagerManager.SetInstance(new GameObject("SizeManagerManager").AddComponent<SizeManagerManager>());
	}

	// Token: 0x060025F0 RID: 9712 RVA: 0x00049C3D File Offset: 0x00047E3D
	private static void SetInstance(SizeManagerManager manager)
	{
		SizeManagerManager.instance = manager;
		SizeManagerManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060025F1 RID: 9713 RVA: 0x00049C58 File Offset: 0x00047E58
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

	// Token: 0x060025F2 RID: 9714 RVA: 0x00049C7E File Offset: 0x00047E7E
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

	// Token: 0x060025F3 RID: 9715 RVA: 0x0010766C File Offset: 0x0010586C
	public void FixedUpdate()
	{
		for (int i = 0; i < SizeManagerManager.allSM.Count; i++)
		{
			SizeManagerManager.allSM[i].InvokeFixedUpdate();
		}
	}

	// Token: 0x04002A28 RID: 10792
	[OnEnterPlay_SetNull]
	public static SizeManagerManager instance;

	// Token: 0x04002A29 RID: 10793
	[OnEnterPlay_Set(false)]
	public static bool hasInstance = false;

	// Token: 0x04002A2A RID: 10794
	[OnEnterPlay_Clear]
	public static List<SizeManager> allSM = new List<SizeManager>();
}
