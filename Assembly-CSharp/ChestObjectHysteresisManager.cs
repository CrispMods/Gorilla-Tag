using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D7 RID: 983
[DefaultExecutionOrder(2000)]
public class ChestObjectHysteresisManager : MonoBehaviour
{
	// Token: 0x060017C0 RID: 6080 RVA: 0x00073C5E File Offset: 0x00071E5E
	protected void Awake()
	{
		if (ChestObjectHysteresisManager.hasInstance && ChestObjectHysteresisManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		ChestObjectHysteresisManager.SetInstance(this);
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x00073C81 File Offset: 0x00071E81
	public static void CreateManager()
	{
		ChestObjectHysteresisManager.SetInstance(new GameObject("ChestObjectHysteresisManager").AddComponent<ChestObjectHysteresisManager>());
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x00073C97 File Offset: 0x00071E97
	private static void SetInstance(ChestObjectHysteresisManager manager)
	{
		ChestObjectHysteresisManager.instance = manager;
		ChestObjectHysteresisManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x00073CB2 File Offset: 0x00071EB2
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

	// Token: 0x060017C4 RID: 6084 RVA: 0x00073CD8 File Offset: 0x00071ED8
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

	// Token: 0x060017C5 RID: 6085 RVA: 0x00073D00 File Offset: 0x00071F00
	public void Update()
	{
		for (int i = 0; i < ChestObjectHysteresisManager.allChests.Count; i++)
		{
			ChestObjectHysteresisManager.allChests[i].InvokeUpdate();
		}
	}

	// Token: 0x04001A5E RID: 6750
	public static ChestObjectHysteresisManager instance;

	// Token: 0x04001A5F RID: 6751
	public static bool hasInstance = false;

	// Token: 0x04001A60 RID: 6752
	public static List<ChestObjectHysteresis> allChests = new List<ChestObjectHysteresis>();
}
