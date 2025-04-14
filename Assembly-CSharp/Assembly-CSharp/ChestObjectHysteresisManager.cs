using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D7 RID: 983
[DefaultExecutionOrder(2000)]
public class ChestObjectHysteresisManager : MonoBehaviour
{
	// Token: 0x060017C3 RID: 6083 RVA: 0x00073FE2 File Offset: 0x000721E2
	protected void Awake()
	{
		if (ChestObjectHysteresisManager.hasInstance && ChestObjectHysteresisManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		ChestObjectHysteresisManager.SetInstance(this);
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x00074005 File Offset: 0x00072205
	public static void CreateManager()
	{
		ChestObjectHysteresisManager.SetInstance(new GameObject("ChestObjectHysteresisManager").AddComponent<ChestObjectHysteresisManager>());
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x0007401B File Offset: 0x0007221B
	private static void SetInstance(ChestObjectHysteresisManager manager)
	{
		ChestObjectHysteresisManager.instance = manager;
		ChestObjectHysteresisManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x00074036 File Offset: 0x00072236
	public static void RegisterCH(ChestObjectHysteresis cOH)
	{
		if (!ChestObjectHysteresisManager.hasInstance)
		{
			ChestObjectHysteresisManager.CreateManager();
		}
		if (!ChestObjectHysteresisManager.allChests.Contains(cOH))
		{
			ChestObjectHysteresisManager.allChests.Add(cOH);
		}
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x0007405C File Offset: 0x0007225C
	public static void UnregisterCH(ChestObjectHysteresis cOH)
	{
		if (!ChestObjectHysteresisManager.hasInstance)
		{
			ChestObjectHysteresisManager.CreateManager();
		}
		if (ChestObjectHysteresisManager.allChests.Contains(cOH))
		{
			ChestObjectHysteresisManager.allChests.Remove(cOH);
		}
	}

	// Token: 0x060017C8 RID: 6088 RVA: 0x00074084 File Offset: 0x00072284
	public void Update()
	{
		for (int i = 0; i < ChestObjectHysteresisManager.allChests.Count; i++)
		{
			ChestObjectHysteresisManager.allChests[i].InvokeUpdate();
		}
	}

	// Token: 0x04001A5F RID: 6751
	public static ChestObjectHysteresisManager instance;

	// Token: 0x04001A60 RID: 6752
	public static bool hasInstance = false;

	// Token: 0x04001A61 RID: 6753
	public static List<ChestObjectHysteresis> allChests = new List<ChestObjectHysteresis>();
}
