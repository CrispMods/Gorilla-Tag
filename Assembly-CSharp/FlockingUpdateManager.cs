using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public class FlockingUpdateManager : MonoBehaviour
{
	// Token: 0x06001FC9 RID: 8137 RVA: 0x000A0804 File Offset: 0x0009EA04
	protected void Awake()
	{
		if (FlockingUpdateManager.hasInstance && FlockingUpdateManager.instance != null && FlockingUpdateManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		FlockingUpdateManager.SetInstance(this);
	}

	// Token: 0x06001FCA RID: 8138 RVA: 0x000A0834 File Offset: 0x0009EA34
	public static void CreateManager()
	{
		FlockingUpdateManager.SetInstance(new GameObject("FlockingUpdateManager").AddComponent<FlockingUpdateManager>());
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x000A084A File Offset: 0x0009EA4A
	private static void SetInstance(FlockingUpdateManager manager)
	{
		FlockingUpdateManager.instance = manager;
		FlockingUpdateManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06001FCC RID: 8140 RVA: 0x000A0865 File Offset: 0x0009EA65
	public static void RegisterFlocking(Flocking flocking)
	{
		if (!FlockingUpdateManager.hasInstance)
		{
			FlockingUpdateManager.CreateManager();
		}
		if (!FlockingUpdateManager.allFlockings.Contains(flocking))
		{
			FlockingUpdateManager.allFlockings.Add(flocking);
		}
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x000A088B File Offset: 0x0009EA8B
	public static void UnregisterFlocking(Flocking flocking)
	{
		if (!FlockingUpdateManager.hasInstance)
		{
			FlockingUpdateManager.CreateManager();
		}
		if (FlockingUpdateManager.allFlockings.Contains(flocking))
		{
			FlockingUpdateManager.allFlockings.Remove(flocking);
		}
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x000A08B4 File Offset: 0x0009EAB4
	public void Update()
	{
		for (int i = 0; i < FlockingUpdateManager.allFlockings.Count; i++)
		{
			FlockingUpdateManager.allFlockings[i].InvokeUpdate();
		}
	}

	// Token: 0x040023DC RID: 9180
	public static FlockingUpdateManager instance;

	// Token: 0x040023DD RID: 9181
	public static bool hasInstance = false;

	// Token: 0x040023DE RID: 9182
	public static List<Flocking> allFlockings = new List<Flocking>();
}
