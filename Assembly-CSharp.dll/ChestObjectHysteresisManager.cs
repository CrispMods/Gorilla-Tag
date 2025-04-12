using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D7 RID: 983
[DefaultExecutionOrder(2000)]
public class ChestObjectHysteresisManager : MonoBehaviour
{
	// Token: 0x060017C3 RID: 6083 RVA: 0x0003F1A7 File Offset: 0x0003D3A7
	protected void Awake()
	{
		if (ChestObjectHysteresisManager.hasInstance && ChestObjectHysteresisManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		ChestObjectHysteresisManager.SetInstance(this);
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x0003F1CA File Offset: 0x0003D3CA
	public static void CreateManager()
	{
		ChestObjectHysteresisManager.SetInstance(new GameObject("ChestObjectHysteresisManager").AddComponent<ChestObjectHysteresisManager>());
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x0003F1E0 File Offset: 0x0003D3E0
	private static void SetInstance(ChestObjectHysteresisManager manager)
	{
		ChestObjectHysteresisManager.instance = manager;
		ChestObjectHysteresisManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x0003F1FB File Offset: 0x0003D3FB
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

	// Token: 0x060017C7 RID: 6087 RVA: 0x0003F221 File Offset: 0x0003D421
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

	// Token: 0x060017C8 RID: 6088 RVA: 0x000C8040 File Offset: 0x000C6240
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
