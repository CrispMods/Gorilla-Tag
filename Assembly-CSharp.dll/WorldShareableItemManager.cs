using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x020003A2 RID: 930
public class WorldShareableItemManager : MonoBehaviour
{
	// Token: 0x060015C0 RID: 5568 RVA: 0x0003DAF1 File Offset: 0x0003BCF1
	protected void Awake()
	{
		if (WorldShareableItemManager.hasInstance && WorldShareableItemManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		WorldShareableItemManager.SetInstance(this);
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x0003DB14 File Offset: 0x0003BD14
	protected void OnDestroy()
	{
		if (WorldShareableItemManager.instance == this)
		{
			WorldShareableItemManager.hasInstance = false;
			WorldShareableItemManager.instance = null;
		}
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x000BF14C File Offset: 0x000BD34C
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

	// Token: 0x060015C3 RID: 5571 RVA: 0x0003DB2F File Offset: 0x0003BD2F
	public static void CreateManager()
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		WorldShareableItemManager.SetInstance(new GameObject("WorldShareableItemManager").AddComponent<WorldShareableItemManager>());
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x0003DB4D File Offset: 0x0003BD4D
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

	// Token: 0x060015C5 RID: 5573 RVA: 0x0003DB70 File Offset: 0x0003BD70
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

	// Token: 0x060015C6 RID: 5574 RVA: 0x0003DB9E File Offset: 0x0003BD9E
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

	// Token: 0x040017F2 RID: 6130
	public static WorldShareableItemManager instance;

	// Token: 0x040017F3 RID: 6131
	public static bool hasInstance = false;

	// Token: 0x040017F4 RID: 6132
	public static readonly List<WorldShareableItem> worldShareableItems = new List<WorldShareableItem>(1024);
}
