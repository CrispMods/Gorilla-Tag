using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class ObjectHierarchyFlattenerManager : MonoBehaviour
{
	// Token: 0x06000AFB RID: 2811 RVA: 0x00037B97 File Offset: 0x00035D97
	protected void Awake()
	{
		if (ObjectHierarchyFlattenerManager.hasInstance && ObjectHierarchyFlattenerManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		ObjectHierarchyFlattenerManager.SetInstance(this);
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x00037BBA File Offset: 0x00035DBA
	public static void CreateManager()
	{
		ObjectHierarchyFlattenerManager.SetInstance(new GameObject("ObjectHierarchyFlattenerManager").AddComponent<ObjectHierarchyFlattenerManager>());
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x00037BD0 File Offset: 0x00035DD0
	private static void SetInstance(ObjectHierarchyFlattenerManager manager)
	{
		ObjectHierarchyFlattenerManager.instance = manager;
		ObjectHierarchyFlattenerManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x00037BEB File Offset: 0x00035DEB
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

	// Token: 0x06000AFF RID: 2815 RVA: 0x00037C11 File Offset: 0x00035E11
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

	// Token: 0x06000B00 RID: 2816 RVA: 0x00099828 File Offset: 0x00097A28
	public void LateUpdate()
	{
		for (int i = 0; i < ObjectHierarchyFlattenerManager.alloHF.Count; i++)
		{
			ObjectHierarchyFlattenerManager.alloHF[i].InvokeLateUpdate();
		}
	}

	// Token: 0x04000D6D RID: 3437
	public static ObjectHierarchyFlattenerManager instance;

	// Token: 0x04000D6E RID: 3438
	[OnEnterPlay_Set(false)]
	public static bool hasInstance = false;

	// Token: 0x04000D6F RID: 3439
	public static List<ObjectHierarchyFlattener> alloHF = new List<ObjectHierarchyFlattener>();
}
