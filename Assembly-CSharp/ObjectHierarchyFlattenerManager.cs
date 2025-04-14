using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class ObjectHierarchyFlattenerManager : MonoBehaviour
{
	// Token: 0x06000AAF RID: 2735 RVA: 0x0003A1EE File Offset: 0x000383EE
	protected void Awake()
	{
		if (ObjectHierarchyFlattenerManager.hasInstance && ObjectHierarchyFlattenerManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		ObjectHierarchyFlattenerManager.SetInstance(this);
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x0003A211 File Offset: 0x00038411
	public static void CreateManager()
	{
		ObjectHierarchyFlattenerManager.SetInstance(new GameObject("ObjectHierarchyFlattenerManager").AddComponent<ObjectHierarchyFlattenerManager>());
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x0003A227 File Offset: 0x00038427
	private static void SetInstance(ObjectHierarchyFlattenerManager manager)
	{
		ObjectHierarchyFlattenerManager.instance = manager;
		ObjectHierarchyFlattenerManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x0003A242 File Offset: 0x00038442
	public static void RegisterOHF(ObjectHierarchyFlattener rbWI)
	{
		if (!ObjectHierarchyFlattenerManager.hasInstance)
		{
			ObjectHierarchyFlattenerManager.CreateManager();
		}
		if (!ObjectHierarchyFlattenerManager.alloHF.Contains(rbWI))
		{
			ObjectHierarchyFlattenerManager.alloHF.Add(rbWI);
		}
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x0003A268 File Offset: 0x00038468
	public static void UnregisterOHF(ObjectHierarchyFlattener rbWI)
	{
		if (!ObjectHierarchyFlattenerManager.hasInstance)
		{
			ObjectHierarchyFlattenerManager.CreateManager();
		}
		if (ObjectHierarchyFlattenerManager.alloHF.Contains(rbWI))
		{
			ObjectHierarchyFlattenerManager.alloHF.Remove(rbWI);
		}
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x0003A290 File Offset: 0x00038490
	public void LateUpdate()
	{
		for (int i = 0; i < ObjectHierarchyFlattenerManager.alloHF.Count; i++)
		{
			ObjectHierarchyFlattenerManager.alloHF[i].InvokeLateUpdate();
		}
	}

	// Token: 0x04000D27 RID: 3367
	public static ObjectHierarchyFlattenerManager instance;

	// Token: 0x04000D28 RID: 3368
	[OnEnterPlay_Set(false)]
	public static bool hasInstance = false;

	// Token: 0x04000D29 RID: 3369
	public static List<ObjectHierarchyFlattener> alloHF = new List<ObjectHierarchyFlattener>();
}
