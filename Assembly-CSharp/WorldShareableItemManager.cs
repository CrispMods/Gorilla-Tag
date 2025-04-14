using System;
using System.Collections.Generic;
using GorillaTag;
using UnityEngine;

// Token: 0x020003A2 RID: 930
public class WorldShareableItemManager : MonoBehaviour
{
	// Token: 0x060015BD RID: 5565 RVA: 0x0006968E File Offset: 0x0006788E
	protected void Awake()
	{
		if (WorldShareableItemManager.hasInstance && WorldShareableItemManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		WorldShareableItemManager.SetInstance(this);
	}

	// Token: 0x060015BE RID: 5566 RVA: 0x000696B1 File Offset: 0x000678B1
	protected void OnDestroy()
	{
		if (WorldShareableItemManager.instance == this)
		{
			WorldShareableItemManager.hasInstance = false;
			WorldShareableItemManager.instance = null;
		}
	}

	// Token: 0x060015BF RID: 5567 RVA: 0x000696CC File Offset: 0x000678CC
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

	// Token: 0x060015C0 RID: 5568 RVA: 0x00069719 File Offset: 0x00067919
	public static void CreateManager()
	{
		if (GTAppState.isQuitting)
		{
			return;
		}
		WorldShareableItemManager.SetInstance(new GameObject("WorldShareableItemManager").AddComponent<WorldShareableItemManager>());
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x00069737 File Offset: 0x00067937
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
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x0006975A File Offset: 0x0006795A
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

	// Token: 0x060015C3 RID: 5571 RVA: 0x00069788 File Offset: 0x00067988
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

	// Token: 0x040017F1 RID: 6129
	public static WorldShareableItemManager instance;

	// Token: 0x040017F2 RID: 6130
	public static bool hasInstance = false;

	// Token: 0x040017F3 RID: 6131
	public static readonly List<WorldShareableItem> worldShareableItems = new List<WorldShareableItem>(1024);
}
