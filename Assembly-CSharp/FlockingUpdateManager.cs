using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200052D RID: 1325
public class FlockingUpdateManager : MonoBehaviour
{
	// Token: 0x06002022 RID: 8226 RVA: 0x00045D23 File Offset: 0x00043F23
	protected void Awake()
	{
		if (FlockingUpdateManager.hasInstance && FlockingUpdateManager.instance != null && FlockingUpdateManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		FlockingUpdateManager.SetInstance(this);
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x00045D53 File Offset: 0x00043F53
	public static void CreateManager()
	{
		FlockingUpdateManager.SetInstance(new GameObject("FlockingUpdateManager").AddComponent<FlockingUpdateManager>());
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x00045D69 File Offset: 0x00043F69
	private static void SetInstance(FlockingUpdateManager manager)
	{
		FlockingUpdateManager.instance = manager;
		FlockingUpdateManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x00045D84 File Offset: 0x00043F84
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

	// Token: 0x06002026 RID: 8230 RVA: 0x00045DAA File Offset: 0x00043FAA
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

	// Token: 0x06002027 RID: 8231 RVA: 0x000F2084 File Offset: 0x000F0284
	public void Update()
	{
		for (int i = 0; i < FlockingUpdateManager.allFlockings.Count; i++)
		{
			FlockingUpdateManager.allFlockings[i].InvokeUpdate();
		}
	}

	// Token: 0x0400242F RID: 9263
	public static FlockingUpdateManager instance;

	// Token: 0x04002430 RID: 9264
	public static bool hasInstance = false;

	// Token: 0x04002431 RID: 9265
	public static List<Flocking> allFlockings = new List<Flocking>();
}
