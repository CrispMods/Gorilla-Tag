using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x020003AD RID: 941
public class WorldShareableItemManager : MonoBehaviour
{
	// Token: 0x06001609 RID: 5641 RVA: 0x0003EDB1 File Offset: 0x0003CFB1
	protected void Awake()
	{
		if (WorldShareableItemManager.hasInstance && WorldShareableItemManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		WorldShareableItemManager.SetInstance(this);
	}

	// Token: 0x0600160A RID: 5642 RVA: 0x0003EDD4 File Offset: 0x0003CFD4
	protected void OnDestroy()
	{
		if (WorldShareableItemManager.instance == this)
		{
			WorldShareableItemManager.hasInstance = false;
			WorldShareableItemManager.instance = null;
		}
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x000C1974 File Offset: 0x000BFB74
	protected void Update()
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		for (int i = 0; i < WorldShareableItemManager.worldShareableItems.Count; i++)
		{
			if (WorldShareableItemManager.worldShareableItems[i] != null)
			{
				WorldShareableItemManager.worldShareableItems[i].TriggeredUpdate();
			}
		}
	}

	// Token: 0x0600160C RID: 5644 RVA: 0x0003EDEF File Offset: 0x0003CFEF
	public static void CreateManager()
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		WorldShareableItemManager.SetInstance(new GameObject("WorldShareableItemManager").AddComponent<WorldShareableItemManager>());
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x0003EE0D File Offset: 0x0003D00D
	private static void SetInstance(WorldShareableItemManager manager)
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		WorldShareableItemManager.instance = manager;
		WorldShareableItemManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x0003EE30 File Offset: 0x0003D030
	public static void Register(WorldShareableItem worldShareableItem)
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		if (!WorldShareableItemManager.hasInstance)
		{
			WorldShareableItemManager.CreateManager();
		}
		if (!WorldShareableItemManager.worldShareableItems.Contains(worldShareableItem))
		{
			WorldShareableItemManager.worldShareableItems.Add(worldShareableItem);
		}
	}

	// Token: 0x0600160F RID: 5647 RVA: 0x0003EE5E File Offset: 0x0003D05E
	public static void Unregister(WorldShareableItem worldShareableItem)
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		if (!WorldShareableItemManager.hasInstance)
		{
			WorldShareableItemManager.CreateManager();
		}
		if (WorldShareableItemManager.worldShareableItems.Contains(worldShareableItem))
		{
			WorldShareableItemManager.worldShareableItems.Remove(worldShareableItem);
		}
	}

	// Token: 0x04001838 RID: 6200
	public static WorldShareableItemManager instance;

	// Token: 0x04001839 RID: 6201
	public static bool hasInstance = false;

	// Token: 0x0400183A RID: 6202
	public static readonly List<WorldShareableItem> worldShareableItems = new List<WorldShareableItem>(1024);
}
