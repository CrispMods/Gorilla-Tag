using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public class FlockingUpdateManager : MonoBehaviour
{
	// Token: 0x06001FCC RID: 8140 RVA: 0x000A0B88 File Offset: 0x0009ED88
	protected void Awake()
	{
		if (FlockingUpdateManager.hasInstance && FlockingUpdateManager.instance != null && FlockingUpdateManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		FlockingUpdateManager.SetInstance(this);
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x000A0BB8 File Offset: 0x0009EDB8
	public static void CreateManager()
	{
		FlockingUpdateManager.SetInstance(new GameObject("FlockingUpdateManager").AddComponent<FlockingUpdateManager>());
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x000A0BCE File Offset: 0x0009EDCE
	private static void SetInstance(FlockingUpdateManager manager)
	{
		FlockingUpdateManager.instance = manager;
		FlockingUpdateManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x000A0BE9 File Offset: 0x0009EDE9
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

	// Token: 0x06001FD0 RID: 8144 RVA: 0x000A0C0F File Offset: 0x0009EE0F
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

	// Token: 0x06001FD1 RID: 8145 RVA: 0x000A0C38 File Offset: 0x0009EE38
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
