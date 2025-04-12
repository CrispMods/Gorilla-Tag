using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public class FlockingUpdateManager : MonoBehaviour
{
	// Token: 0x06001FCC RID: 8140 RVA: 0x00044984 File Offset: 0x00042B84
	protected void Awake()
	{
		if (FlockingUpdateManager.hasInstance && FlockingUpdateManager.instance != null && FlockingUpdateManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		FlockingUpdateManager.SetInstance(this);
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x000449B4 File Offset: 0x00042BB4
	public static void CreateManager()
	{
		FlockingUpdateManager.SetInstance(new GameObject("FlockingUpdateManager").AddComponent<FlockingUpdateManager>());
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x000449CA File Offset: 0x00042BCA
	private static void SetInstance(FlockingUpdateManager manager)
	{
		FlockingUpdateManager.instance = manager;
		FlockingUpdateManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x000449E5 File Offset: 0x00042BE5
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

	// Token: 0x06001FD0 RID: 8144 RVA: 0x00044A0B File Offset: 0x00042C0B
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

	// Token: 0x06001FD1 RID: 8145 RVA: 0x000EF300 File Offset: 0x000ED500
	public void Update()
	{
		for (int i = 0; i < FlockingUpdateManager.allFlockings.Count; i++)
		{
			FlockingUpdateManager.allFlockings[i].InvokeUpdate();
		}
	}

	// Token: 0x040023DD RID: 9181
	public static FlockingUpdateManager instance;

	// Token: 0x040023DE RID: 9182
	public static bool hasInstance = false;

	// Token: 0x040023DF RID: 9183
	public static List<Flocking> allFlockings = new List<Flocking>();
}
