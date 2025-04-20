using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E2 RID: 994
[DefaultExecutionOrder(2000)]
public class ChestObjectHysteresisManager : MonoBehaviour
{
	// Token: 0x0600180D RID: 6157 RVA: 0x00040491 File Offset: 0x0003E691
	protected void Awake()
	{
		if (ChestObjectHysteresisManager.hasInstance && ChestObjectHysteresisManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		ChestObjectHysteresisManager.SetInstance(this);
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x000404B4 File Offset: 0x0003E6B4
	public static void CreateManager()
	{
		ChestObjectHysteresisManager.SetInstance(new GameObject("ChestObjectHysteresisManager").AddComponent<ChestObjectHysteresisManager>());
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x000404CA File Offset: 0x0003E6CA
	private static void SetInstance(ChestObjectHysteresisManager manager)
	{
		ChestObjectHysteresisManager.instance = manager;
		ChestObjectHysteresisManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x000404E5 File Offset: 0x0003E6E5
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

	// Token: 0x06001811 RID: 6161 RVA: 0x0004050B File Offset: 0x0003E70B
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

	// Token: 0x06001812 RID: 6162 RVA: 0x000CA868 File Offset: 0x000C8A68
	public void Update()
	{
		for (int i = 0; i < ChestObjectHysteresisManager.allChests.Count; i++)
		{
			ChestObjectHysteresisManager.allChests[i].InvokeUpdate();
		}
	}

	// Token: 0x04001AA7 RID: 6823
	public static ChestObjectHysteresisManager instance;

	// Token: 0x04001AA8 RID: 6824
	public static bool hasInstance = false;

	// Token: 0x04001AA9 RID: 6825
	public static List<ChestObjectHysteresis> allChests = new List<ChestObjectHysteresis>();
}
