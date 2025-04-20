using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000224 RID: 548
public static class SceneIndexExtensions
{
	// Token: 0x06000CB6 RID: 3254 RVA: 0x00038E4D File Offset: 0x0003704D
	public static SceneIndex GetSceneIndex(this Scene scene)
	{
		return (SceneIndex)scene.buildIndex;
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x000A0490 File Offset: 0x0009E690
	public static SceneIndex GetSceneIndex(this GameObject obj)
	{
		return (SceneIndex)obj.scene.buildIndex;
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x000A04AC File Offset: 0x0009E6AC
	public static SceneIndex GetSceneIndex(this Component cmp)
	{
		return (SceneIndex)cmp.gameObject.scene.buildIndex;
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x000A04CC File Offset: 0x0009E6CC
	public static void AddCallbackOnSceneLoad(this SceneIndex scene, Action callback)
	{
		if (SceneIndexExtensions.onSceneLoadCallbacks == null)
		{
			SceneIndexExtensions.onSceneLoadCallbacks = new List<Action>[15];
			for (int i = 0; i < SceneIndexExtensions.onSceneLoadCallbacks.Length; i++)
			{
				SceneIndexExtensions.onSceneLoadCallbacks[i] = new List<Action>();
			}
			SceneManager.sceneLoaded += SceneIndexExtensions.OnSceneLoad;
		}
		SceneIndexExtensions.onSceneLoadCallbacks[(int)scene].Add(callback);
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x00038E56 File Offset: 0x00037056
	public static void RemoveCallbackOnSceneLoad(this SceneIndex scene, Action callback)
	{
		SceneIndexExtensions.onSceneLoadCallbacks[(int)scene].Remove(callback);
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x000A0528 File Offset: 0x0009E728
	public static void OnSceneLoad(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex != -1)
		{
			foreach (Action action in SceneIndexExtensions.onSceneLoadCallbacks[scene.buildIndex])
			{
				action();
			}
		}
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x000A058C File Offset: 0x0009E78C
	public static void AddCallbackOnSceneUnload(this SceneIndex scene, Action callback)
	{
		if (SceneIndexExtensions.onSceneUnloadCallbacks == null)
		{
			SceneIndexExtensions.onSceneUnloadCallbacks = new List<Action>[15];
			for (int i = 0; i < SceneIndexExtensions.onSceneUnloadCallbacks.Length; i++)
			{
				SceneIndexExtensions.onSceneUnloadCallbacks[i] = new List<Action>();
			}
			SceneManager.sceneUnloaded += SceneIndexExtensions.OnSceneUnload;
		}
		SceneIndexExtensions.onSceneUnloadCallbacks[(int)scene].Add(callback);
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x00038E66 File Offset: 0x00037066
	public static void RemoveCallbackOnSceneUnload(this SceneIndex scene, Action callback)
	{
		SceneIndexExtensions.onSceneUnloadCallbacks[(int)scene].Remove(callback);
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x000A05E8 File Offset: 0x0009E7E8
	public static void OnSceneUnload(Scene scene)
	{
		if (scene.buildIndex != -1)
		{
			foreach (Action action in SceneIndexExtensions.onSceneUnloadCallbacks[scene.buildIndex])
			{
				action();
			}
		}
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x00038E76 File Offset: 0x00037076
	[OnEnterPlay_Run]
	private static void Reset()
	{
		if (SceneIndexExtensions.onSceneLoadCallbacks != null)
		{
			SceneIndexExtensions.onSceneLoadCallbacks = null;
			SceneManager.sceneLoaded -= SceneIndexExtensions.OnSceneLoad;
		}
		if (SceneIndexExtensions.onSceneUnloadCallbacks != null)
		{
			SceneIndexExtensions.onSceneUnloadCallbacks = null;
			SceneManager.sceneUnloaded -= SceneIndexExtensions.OnSceneUnload;
		}
	}

	// Token: 0x04001021 RID: 4129
	private const int SceneIndex_COUNT = 15;

	// Token: 0x04001022 RID: 4130
	[OnEnterPlay_SetNull]
	private static List<Action>[] onSceneLoadCallbacks;

	// Token: 0x04001023 RID: 4131
	[OnEnterPlay_SetNull]
	private static List<Action>[] onSceneUnloadCallbacks;
}
