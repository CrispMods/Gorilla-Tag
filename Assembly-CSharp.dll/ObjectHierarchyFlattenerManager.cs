using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class ObjectHierarchyFlattenerManager : MonoBehaviour
{
	// Token: 0x06000AB1 RID: 2737 RVA: 0x000368D7 File Offset: 0x00034AD7
	protected void Awake()
	{
		if (ObjectHierarchyFlattenerManager.hasInstance && ObjectHierarchyFlattenerManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		ObjectHierarchyFlattenerManager.SetInstance(this);
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x000368FA File Offset: 0x00034AFA
	public static void CreateManager()
	{
		ObjectHierarchyFlattenerManager.SetInstance(new GameObject("ObjectHierarchyFlattenerManager").AddComponent<ObjectHierarchyFlattenerManager>());
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x00036910 File Offset: 0x00034B10
	private static void SetInstance(ObjectHierarchyFlattenerManager manager)
	{
		ObjectHierarchyFlattenerManager.instance = manager;
		ObjectHierarchyFlattenerManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x0003692B File Offset: 0x00034B2B
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

	// Token: 0x06000AB5 RID: 2741 RVA: 0x00036951 File Offset: 0x00034B51
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

	// Token: 0x06000AB6 RID: 2742 RVA: 0x00096F34 File Offset: 0x00095134
	public void LateUpdate()
	{
		for (int i = 0; i < ObjectHierarchyFlattenerManager.alloHF.Count; i++)
		{
			ObjectHierarchyFlattenerManager.alloHF[i].InvokeLateUpdate();
		}
	}

	// Token: 0x04000D28 RID: 3368
	public static ObjectHierarchyFlattenerManager instance;

	// Token: 0x04000D29 RID: 3369
	[OnEnterPlay_Set(false)]
	public static bool hasInstance = false;

	// Token: 0x04000D2A RID: 3370
	public static List<ObjectHierarchyFlattener> alloHF = new List<ObjectHierarchyFlattener>();
}
