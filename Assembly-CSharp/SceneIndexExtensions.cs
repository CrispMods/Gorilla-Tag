using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000219 RID: 537
public static class SceneIndexExtensions
{
	// Token: 0x06000C6B RID: 3179 RVA: 0x00042163 File Offset: 0x00040363
	public static SceneIndex GetSceneIndex(this Scene scene)
	{
		return (SceneIndex)scene.buildIndex;
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x0004216C File Offset: 0x0004036C
	public static SceneIndex GetSceneIndex(this GameObject obj)
	{
		return (SceneIndex)obj.scene.buildIndex;
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00042188 File Offset: 0x00040388
	public static SceneIndex GetSceneIndex(this Component cmp)
	{
		return (SceneIndex)cmp.gameObject.scene.buildIndex;
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x000421A8 File Offset: 0x000403A8
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

	// Token: 0x06000C6F RID: 3183 RVA: 0x00042204 File Offset: 0x00040404
	public static void RemoveCallbackOnSceneLoad(this SceneIndex scene, Action callback)
	{
		SceneIndexExtensions.onSceneLoadCallbacks[(int)scene].Remove(callback);
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00042214 File Offset: 0x00040414
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

	// Token: 0x06000C71 RID: 3185 RVA: 0x00042278 File Offset: 0x00040478
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

	// Token: 0x06000C72 RID: 3186 RVA: 0x000422D4 File Offset: 0x000404D4
	public static void RemoveCallbackOnSceneUnload(this SceneIndex scene, Action callback)
	{
		SceneIndexExtensions.onSceneUnloadCallbacks[(int)scene].Remove(callback);
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x000422E4 File Offset: 0x000404E4
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

	// Token: 0x06000C74 RID: 3188 RVA: 0x00042348 File Offset: 0x00040548
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

	// Token: 0x04000FDB RID: 4059
	private const int SceneIndex_COUNT = 15;

	// Token: 0x04000FDC RID: 4060
	[OnEnterPlay_SetNull]
	private static List<Action>[] onSceneLoadCallbacks;

	// Token: 0x04000FDD RID: 4061
	[OnEnterPlay_SetNull]
	private static List<Action>[] onSceneUnloadCallbacks;
}
