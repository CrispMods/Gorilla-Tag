using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GorillaExtensions;
using GorillaNetworking;
using GorillaTagScripts.CustomMapSupport;
using GorillaTagScripts.ModIO;
using GT_CustomMapSupportRuntime;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

// Token: 0x02000604 RID: 1540
public class ModIOMapLoader : MonoBehaviour
{
	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06002643 RID: 9795 RVA: 0x00049112 File Offset: 0x00047312
	// (set) Token: 0x06002642 RID: 9794 RVA: 0x000490FB File Offset: 0x000472FB
	public static string LoadedMapLevelName
	{
		get
		{
			return ModIOMapLoader.loadedMapLevelName;
		}
		set
		{
			ModIOMapLoader.loadedMapLevelName = value.Replace(" ", "");
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06002644 RID: 9796 RVA: 0x00049119 File Offset: 0x00047319
	public static long LoadedMapModId
	{
		get
		{
			return ModIOMapLoader.loadedMapModId;
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06002645 RID: 9797 RVA: 0x00049120 File Offset: 0x00047320
	public static MapDescriptor LoadedMapDescriptor
	{
		get
		{
			return ModIOMapLoader.loadedMapDescriptor;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06002646 RID: 9798 RVA: 0x00049127 File Offset: 0x00047327
	public static long LoadingMapModId
	{
		get
		{
			return ModIOMapLoader.attemptedLoadID;
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06002647 RID: 9799 RVA: 0x0004912E File Offset: 0x0004732E
	public static bool IsLoading
	{
		get
		{
			return ModIOMapLoader.isLoading;
		}
	}

	// Token: 0x06002648 RID: 9800 RVA: 0x00049135 File Offset: 0x00047335
	public static bool IsCustomScene(string sceneName)
	{
		return ModIOMapLoader.loadedSceneNames.Contains(sceneName);
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x00049142 File Offset: 0x00047342
	private void Awake()
	{
		if (ModIOMapLoader.instance == null)
		{
			ModIOMapLoader.instance = this;
			ModIOMapLoader.hasInstance = true;
			return;
		}
		if (ModIOMapLoader.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x001069E0 File Offset: 0x00104BE0
	private void Start()
	{
		byte[] bytes = new byte[]
		{
			Convert.ToByte(68),
			Convert.ToByte(111),
			Convert.ToByte(110),
			Convert.ToByte(116),
			Convert.ToByte(68),
			Convert.ToByte(101),
			Convert.ToByte(115),
			Convert.ToByte(116),
			Convert.ToByte(114),
			Convert.ToByte(111),
			Convert.ToByte(121),
			Convert.ToByte(79),
			Convert.ToByte(110),
			Convert.ToByte(76),
			Convert.ToByte(111),
			Convert.ToByte(97),
			Convert.ToByte(100)
		};
		this.dontDestroyOnLoadSceneName = Encoding.ASCII.GetString(bytes);
		if (this.networkTrigger != null)
		{
			this.networkTrigger.SetActive(false);
		}
	}

	// Token: 0x0600264B RID: 9803 RVA: 0x00106AD4 File Offset: 0x00104CD4
	public static void LoadMod(long mapModId, string mapFilePath, Action<bool> onLoadComplete, Action<MapLoadStatus, int, string> progressCallback, Action<string> onSceneLoaded)
	{
		if (!ModIOMapLoader.hasInstance)
		{
			return;
		}
		if (ModIOMapLoader.isLoading)
		{
			return;
		}
		if (ModIOMapLoader.isUnloading)
		{
			if (onLoadComplete != null)
			{
				onLoadComplete(false);
			}
			return;
		}
		if (ModIOMapLoader.IsModLoaded(mapModId))
		{
			if (onLoadComplete != null)
			{
				onLoadComplete(true);
			}
			return;
		}
		GorillaNetworkJoinTrigger.DisableTriggerJoins();
		ModIOMapLoader.modLoadProgressCallback = progressCallback;
		ModIOMapLoader.modLoadedCallback = onLoadComplete;
		ModIOMapLoader.sceneLoadedCallback = onSceneLoaded;
		ModIOMapLoader.instance.StartCoroutine(ModIOMapLoader.LoadAssetBundle(mapModId, mapFilePath, new Action<bool, bool>(ModIOMapLoader.OnAssetBundleLoaded)));
	}

	// Token: 0x0600264C RID: 9804 RVA: 0x0004917C File Offset: 0x0004737C
	public static void LoadZoneTriggered(int[] loadSceneIndexes, int[] unloadSceneIndexes, Texture2D[] lightmapsColor, Texture2D[] lightmapsDir, Action<string> onSceneLoaded, Action<string> onSceneUnloaded)
	{
		if (ModIOMapLoader.sceneLoadingCoroutine != null)
		{
			return;
		}
		ModIOMapLoader.sceneLoadedCallback = onSceneLoaded;
		ModIOMapLoader.sceneUnloadedCallback = onSceneUnloaded;
		ModIOMapLoader.sceneLoadingCoroutine = ModIOMapLoader.instance.StartCoroutine(ModIOMapLoader.LoadZoneCoroutine(loadSceneIndexes, unloadSceneIndexes, lightmapsColor, lightmapsDir));
	}

	// Token: 0x0600264D RID: 9805 RVA: 0x000491AE File Offset: 0x000473AE
	private static IEnumerator LoadZoneCoroutine(int[] loadScenes, int[] unloadScenes, Texture2D[] lightmapsColor, Texture2D[] lightmapsDir)
	{
		if (!loadScenes.IsNullOrEmpty<int>())
		{
			yield return ModIOMapLoader.LoadScenesCoroutine(loadScenes);
		}
		if (!unloadScenes.IsNullOrEmpty<int>())
		{
			yield return ModIOMapLoader.UnloadScenesCoroutine(unloadScenes);
		}
		if (ModIOMapLoader.sceneLoadingCoroutine != null)
		{
			ModIOMapLoader.instance.StopCoroutine(ModIOMapLoader.sceneLoadingCoroutine);
			ModIOMapLoader.sceneLoadingCoroutine = null;
		}
		yield break;
	}

	// Token: 0x0600264E RID: 9806 RVA: 0x000491C4 File Offset: 0x000473C4
	private static IEnumerator LoadScenesCoroutine(int[] sceneIndexes)
	{
		int num;
		for (int i = 0; i < sceneIndexes.Length; i = num + 1)
		{
			if (!ModIOMapLoader.loadedSceneFilePaths.Contains(ModIOMapLoader.assetBundleSceneFilePaths[sceneIndexes[i]]))
			{
				yield return ModIOMapLoader.LoadSceneFromAssetBundle(sceneIndexes[i], false, new Action<bool, bool, string>(ModIOMapLoader.OnIncrementalLoadComplete));
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x0600264F RID: 9807 RVA: 0x000491D3 File Offset: 0x000473D3
	private static IEnumerator UnloadScenesCoroutine(int[] sceneIndexes)
	{
		int num;
		for (int i = 0; i < sceneIndexes.Length; i = num + 1)
		{
			yield return ModIOMapLoader.UnloadSceneCoroutine(sceneIndexes[i], null);
			num = i;
		}
		yield break;
	}

	// Token: 0x06002650 RID: 9808 RVA: 0x000491E2 File Offset: 0x000473E2
	private static IEnumerator LoadAssetBundle(long mapModID, string packageInfoFilePath, Action<bool, bool> OnLoadComplete)
	{
		if (ModIOMapLoader.isLoading)
		{
			if (OnLoadComplete != null)
			{
				OnLoadComplete(false, false);
			}
			yield break;
		}
		yield return ModIOMapLoader.CloseDoorAndUnloadModCoroutine();
		if (ModIOMapLoader.shouldAbortSceneLoad)
		{
			OnLoadComplete(false, true);
			yield break;
		}
		ModIOMapLoader.isLoading = true;
		ModIOMapLoader.attemptedLoadID = mapModID;
		Action<MapLoadStatus, int, string> action = ModIOMapLoader.modLoadProgressCallback;
		if (action != null)
		{
			action(MapLoadStatus.Loading, 1, "GRABBING LIGHTMAP DATA");
		}
		ModIOMapLoader.lightmaps = new LightmapData[LightmapSettings.lightmaps.Length];
		if (ModIOMapLoader.lightmapsToKeep.Count > 0)
		{
			ModIOMapLoader.lightmapsToKeep.Clear();
		}
		ModIOMapLoader.lightmapsToKeep = new List<Texture2D>(LightmapSettings.lightmaps.Length * 2);
		for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
		{
			ModIOMapLoader.lightmaps[i] = LightmapSettings.lightmaps[i];
			if (LightmapSettings.lightmaps[i].lightmapColor != null)
			{
				ModIOMapLoader.lightmapsToKeep.Add(LightmapSettings.lightmaps[i].lightmapColor);
			}
			if (LightmapSettings.lightmaps[i].lightmapDir != null)
			{
				ModIOMapLoader.lightmapsToKeep.Add(LightmapSettings.lightmaps[i].lightmapDir);
			}
		}
		Action<MapLoadStatus, int, string> action2 = ModIOMapLoader.modLoadProgressCallback;
		if (action2 != null)
		{
			action2(MapLoadStatus.Loading, 2, "LOADING PACKAGE INFO");
		}
		MapPackageInfo packageInfo;
		try
		{
			packageInfo = ModIOMapLoader.GetPackageInfo(packageInfoFilePath);
		}
		catch (Exception ex)
		{
			Action<MapLoadStatus, int, string> action3 = ModIOMapLoader.modLoadProgressCallback;
			if (action3 != null)
			{
				action3(MapLoadStatus.Error, 0, ex.Message);
			}
			yield break;
		}
		if (packageInfo == null)
		{
			Action<MapLoadStatus, int, string> action4 = ModIOMapLoader.modLoadProgressCallback;
			if (action4 != null)
			{
				action4(MapLoadStatus.Error, 0, "FAILED TO READ FILE AT " + packageInfoFilePath);
			}
			OnLoadComplete(false, false);
			yield break;
		}
		ModIOMapLoader.initialSceneName = packageInfo.initialScene;
		Action<MapLoadStatus, int, string> action5 = ModIOMapLoader.modLoadProgressCallback;
		if (action5 != null)
		{
			action5(MapLoadStatus.Loading, 3, "PACKAGE INFO LOADED");
		}
		string path = Path.GetDirectoryName(packageInfoFilePath) + "/" + packageInfo.pcFileName;
		Action<MapLoadStatus, int, string> action6 = ModIOMapLoader.modLoadProgressCallback;
		if (action6 != null)
		{
			action6(MapLoadStatus.Loading, 12, "LOADING MAP ASSET BUNDLE");
		}
		AssetBundleCreateRequest loadBundleRequest = AssetBundle.LoadFromFileAsync(path);
		yield return loadBundleRequest;
		ModIOMapLoader.mapBundle = loadBundleRequest.assetBundle;
		if (ModIOMapLoader.shouldAbortSceneLoad)
		{
			if (ModIOMapLoader.mapBundle != null)
			{
				ModIOMapLoader.mapBundle.Unload(true);
			}
			ModIOMapLoader.mapBundle = null;
			OnLoadComplete(false, true);
			yield break;
		}
		if (ModIOMapLoader.mapBundle == null)
		{
			Action<MapLoadStatus, int, string> action7 = ModIOMapLoader.modLoadProgressCallback;
			if (action7 != null)
			{
				action7(MapLoadStatus.Error, 0, "CUSTOM MAP ASSET BUNDLE FAILED TO LOAD");
			}
			OnLoadComplete(false, false);
			yield break;
		}
		if (!ModIOMapLoader.mapBundle.isStreamedSceneAssetBundle)
		{
			ModIOMapLoader.mapBundle.Unload(true);
			Action<MapLoadStatus, int, string> action8 = ModIOMapLoader.modLoadProgressCallback;
			if (action8 != null)
			{
				action8(MapLoadStatus.Error, 0, "AssetBundle does not contain a Unity Scene file");
			}
			OnLoadComplete(false, false);
			yield break;
		}
		Action<MapLoadStatus, int, string> action9 = ModIOMapLoader.modLoadProgressCallback;
		if (action9 != null)
		{
			action9(MapLoadStatus.Loading, 20, "MAP ASSET BUNDLE LOADED");
		}
		ModIOMapLoader.mapBundle.GetAllAssetNames();
		ModIOMapLoader.assetBundleSceneFilePaths = ModIOMapLoader.mapBundle.GetAllScenePaths();
		if (ModIOMapLoader.assetBundleSceneFilePaths.Length == 0)
		{
			ModIOMapLoader.mapBundle.Unload(true);
			Action<MapLoadStatus, int, string> action10 = ModIOMapLoader.modLoadProgressCallback;
			if (action10 != null)
			{
				action10(MapLoadStatus.Error, 0, "AssetBundle does not contain a Unity Scene file");
			}
			OnLoadComplete(false, false);
			yield break;
		}
		foreach (string text in ModIOMapLoader.assetBundleSceneFilePaths)
		{
			if (text.Equals(ModIOMapLoader.instance.dontDestroyOnLoadSceneName, StringComparison.OrdinalIgnoreCase))
			{
				ModIOMapLoader.mapBundle.Unload(true);
				Action<MapLoadStatus, int, string> action11 = ModIOMapLoader.modLoadProgressCallback;
				if (action11 != null)
				{
					action11(MapLoadStatus.Error, 0, "Map name is " + text + " this is an invalid name");
				}
				OnLoadComplete(false, false);
				yield break;
			}
		}
		OnLoadComplete(true, false);
		yield break;
	}

	// Token: 0x06002651 RID: 9809 RVA: 0x000491FF File Offset: 0x000473FF
	private static void RequestAbortModLoad(Action callback = null)
	{
		ModIOMapLoader.abortModLoadCallback = callback;
		ModIOMapLoader.shouldAbortSceneLoad = true;
		ModIOMapLoader.shouldUnloadMod = true;
	}

	// Token: 0x06002652 RID: 9810 RVA: 0x00049213 File Offset: 0x00047413
	private static IEnumerator AbortSceneLoad(int sceneIndex)
	{
		ModIOMapLoader.isLoading = false;
		if (ModIOMapLoader.shouldUnloadMod)
		{
			if (ModIOMapLoader.sceneLoadingCoroutine != null)
			{
				ModIOMapLoader.instance.StopCoroutine(ModIOMapLoader.sceneLoadingCoroutine);
				ModIOMapLoader.sceneLoadingCoroutine = null;
			}
			yield return ModIOMapLoader.UnloadAllScenesCoroutine();
			if (ModIOMapLoader.mapBundle != null)
			{
				ModIOMapLoader.mapBundle.Unload(true);
			}
			ModIOMapLoader.mapBundle = null;
			Action action = ModIOMapLoader.abortModLoadCallback;
			if (action != null)
			{
				action();
			}
		}
		else
		{
			yield return ModIOMapLoader.UnloadSceneCoroutine(sceneIndex, null);
		}
		ModIOMapLoader.abortModLoadCallback = null;
		ModIOMapLoader.shouldAbortSceneLoad = false;
		ModIOMapLoader.shouldUnloadMod = false;
		yield break;
	}

	// Token: 0x06002653 RID: 9811 RVA: 0x00049222 File Offset: 0x00047422
	private static IEnumerator LoadSceneFromAssetBundle(int sceneIndex, bool useProgressCallback, Action<bool, bool, string> OnLoadComplete)
	{
		LoadSceneParameters parameters = new LoadSceneParameters
		{
			loadSceneMode = LoadSceneMode.Additive,
			localPhysicsMode = LocalPhysicsMode.None
		};
		if (ModIOMapLoader.shouldAbortSceneLoad)
		{
			yield return ModIOMapLoader.AbortSceneLoad(sceneIndex);
			OnLoadComplete(false, true, "");
			yield break;
		}
		ModIOMapLoader.runningAsyncLoad = true;
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action = ModIOMapLoader.modLoadProgressCallback;
			if (action != null)
			{
				action(MapLoadStatus.Loading, 30, "LOADING MAP SCENE");
			}
		}
		ModIOMapLoader.attemptedSceneToLoad = ModIOMapLoader.assetBundleSceneFilePaths[sceneIndex];
		string sceneName = ModIOMapLoader.GetSceneNameFromFilePath(ModIOMapLoader.attemptedSceneToLoad);
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(ModIOMapLoader.attemptedSceneToLoad, parameters);
		yield return asyncOperation;
		ModIOMapLoader.runningAsyncLoad = false;
		if (ModIOMapLoader.shouldAbortSceneLoad)
		{
			yield return ModIOMapLoader.AbortSceneLoad(sceneIndex);
			OnLoadComplete(false, true, "");
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action2 = ModIOMapLoader.modLoadProgressCallback;
			if (action2 != null)
			{
				action2(MapLoadStatus.Loading, 50, "SANITIZING MAP");
			}
		}
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
		MapDescriptor[] array2 = UnityEngine.Object.FindObjectsOfType<MapDescriptor>();
		MapDescriptor mapDescriptor = null;
		bool flag = false;
		foreach (MapDescriptor mapDescriptor2 in array2)
		{
			if (mapDescriptor2.gameObject.scene.name == sceneName)
			{
				if (!(mapDescriptor == null))
				{
					flag = true;
					break;
				}
				mapDescriptor = mapDescriptor2;
			}
		}
		if (flag)
		{
			yield return ModIOMapLoader.AbortSceneLoad(sceneIndex);
			if (useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action3 = ModIOMapLoader.modLoadProgressCallback;
				if (action3 != null)
				{
					action3(MapLoadStatus.Error, 0, "MAP CONTAINS MULTIPLE MAP DESCRIPTOR OBJECTS");
				}
			}
			OnLoadComplete(false, false, "");
			yield break;
		}
		if (mapDescriptor.IsNull())
		{
			yield return ModIOMapLoader.AbortSceneLoad(sceneIndex);
			if (useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action4 = ModIOMapLoader.modLoadProgressCallback;
				if (action4 != null)
				{
					action4(MapLoadStatus.Error, 0, "MAP SCENE DOES NOT CONTAIN A MAP DESCRIPTOR");
				}
			}
			OnLoadComplete(false, false, "");
			yield break;
		}
		GameObject gameObject = mapDescriptor.gameObject;
		if (!ModIOMapLoader.SanitizeObject(gameObject))
		{
			yield return ModIOMapLoader.AbortSceneLoad(sceneIndex);
			if (useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action5 = ModIOMapLoader.modLoadProgressCallback;
				if (action5 != null)
				{
					action5(MapLoadStatus.Error, 0, "MAP DESCRIPTOR HAS UNAPPROVED COMPONENTS ON IT");
				}
			}
			OnLoadComplete(false, false, "");
			yield break;
		}
		foreach (GameObject gameObject2 in array)
		{
			if (!gameObject2.IsNull())
			{
				bool flag2 = false;
				foreach (string filePath in ModIOMapLoader.loadedSceneFilePaths)
				{
					if (gameObject2.scene.name == ModIOMapLoader.GetSceneNameFromFilePath(filePath))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && gameObject2.scene.name != SceneManager.GetActiveScene().name && !gameObject2.scene.name.Equals(ModIOMapLoader.instance.dontDestroyOnLoadSceneName, StringComparison.OrdinalIgnoreCase) && ModIOMapLoader.SanitizeObject(gameObject2) && gameObject2.transform.parent.IsNull() && gameObject2.transform != gameObject.transform)
				{
					gameObject2.transform.SetParent(gameObject.transform);
				}
			}
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action6 = ModIOMapLoader.modLoadProgressCallback;
			if (action6 != null)
			{
				action6(MapLoadStatus.Loading, 70, "MAP SCENE LOADED");
			}
		}
		ModIOMapLoader.leafGliderIndex = 0;
		yield return ModIOMapLoader.ProcessAndInstantiateMap(gameObject, useProgressCallback);
		yield return null;
		if (ModIOMapLoader.shouldAbortSceneLoad)
		{
			yield return ModIOMapLoader.AbortSceneLoad(sceneIndex);
			OnLoadComplete(false, true, "");
			if (ModIOMapLoader.cachedExceptionMessage.Length > 0 && useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action7 = ModIOMapLoader.modLoadProgressCallback;
				if (action7 != null)
				{
					action7(MapLoadStatus.Error, 0, ModIOMapLoader.cachedExceptionMessage);
				}
			}
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action8 = ModIOMapLoader.modLoadProgressCallback;
			if (action8 != null)
			{
				action8(MapLoadStatus.Loading, 99, "FINALIZING MAP");
			}
		}
		if (!ModIOMapLoader.loadedSceneFilePaths.Contains(ModIOMapLoader.attemptedSceneToLoad))
		{
			ModIOMapLoader.loadedSceneFilePaths.Add(ModIOMapLoader.attemptedSceneToLoad);
			ModIOMapLoader.loadedSceneNames.Add(sceneName);
		}
		OnLoadComplete(true, false, sceneName);
		yield break;
	}

	// Token: 0x06002654 RID: 9812 RVA: 0x00106B50 File Offset: 0x00104D50
	public static void CloseDoorAndUnloadMod(Action unloadFinishedCallback = null)
	{
		if (!ModIOMapLoader.IsModLoaded(0L) && !ModIOMapLoader.isLoading)
		{
			return;
		}
		ModIOMapLoader.unloadModCallback = unloadFinishedCallback;
		if (ModIOMapLoader.isLoading)
		{
			ModIOMapLoader.RequestAbortModLoad(null);
			return;
		}
		if (ModIOMapLoader.instance.accessDoor != null)
		{
			ModIOMapLoader.instance.accessDoor.CloseDoor();
		}
		ModIOMapLoader.shouldUnloadMod = true;
		if (ModIOMapLoader.mapBundle != null)
		{
			ModIOMapLoader.mapBundle.Unload(true);
		}
		ModIOMapLoader.mapBundle = null;
		ModIOTelemetry.EndMapTracking();
		CustomMapSerializer.ResetSyncedMapObjects();
		ModIOMapLoader.instance.StartCoroutine(ModIOMapLoader.UnloadAllScenesCoroutine());
	}

	// Token: 0x06002655 RID: 9813 RVA: 0x0004923F File Offset: 0x0004743F
	private static IEnumerator CloseDoorAndUnloadModCoroutine()
	{
		if (!ModIOMapLoader.IsModLoaded(0L) || ModIOMapLoader.IsLoading)
		{
			yield break;
		}
		if (ModIOMapLoader.instance.accessDoor != null)
		{
			ModIOMapLoader.instance.accessDoor.CloseDoor();
		}
		ModIOMapLoader.shouldUnloadMod = true;
		if (ModIOMapLoader.mapBundle != null)
		{
			ModIOMapLoader.mapBundle.Unload(true);
		}
		ModIOMapLoader.mapBundle = null;
		ModIOTelemetry.EndMapTracking();
		CustomMapSerializer.ResetSyncedMapObjects();
		yield return ModIOMapLoader.UnloadAllScenesCoroutine();
		yield break;
	}

	// Token: 0x06002656 RID: 9814 RVA: 0x00049247 File Offset: 0x00047447
	private static IEnumerator UnloadAllScenesCoroutine()
	{
		ModIOMapLoader.isLoading = false;
		ModIOMapLoader.isUnloading = true;
		int num;
		for (int sceneIndex = 0; sceneIndex < ModIOMapLoader.assetBundleSceneFilePaths.Length; sceneIndex = num + 1)
		{
			yield return ModIOMapLoader.UnloadSceneCoroutine(sceneIndex, null);
			num = sceneIndex;
		}
		ModIOMapLoader.loadedMapDescriptor = null;
		ModIOMapLoader.loadedSceneFilePaths.Clear();
		ModIOMapLoader.loadedSceneNames.Clear();
		for (int i = 0; i < ModIOMapLoader.instance.leafGliders.Length; i++)
		{
			ModIOMapLoader.instance.leafGliders[i].CustomMapUnload();
			ModIOMapLoader.instance.leafGliders[i].enabled = false;
			ModIOMapLoader.instance.leafGliders[ModIOMapLoader.leafGliderIndex].transform.GetChild(0).gameObject.SetActive(false);
		}
		GorillaNetworkJoinTrigger.EnableTriggerJoins();
		LightmapSettings.lightmaps = ModIOMapLoader.lightmaps;
		ModIOMapLoader.UnloadLightmaps();
		Resources.UnloadUnusedAssets();
		ModIOMapLoader.isUnloading = false;
		if (ModIOMapLoader.shouldUnloadMod)
		{
			yield return ModIOMapLoader.ResetLightmaps();
			ModIOMapLoader.assetBundleSceneFilePaths = new string[]
			{
				""
			};
			ModIOMapLoader.loadedMapModId = 0L;
			Action action = ModIOMapLoader.unloadModCallback;
			if (action != null)
			{
				action();
			}
			ModIOMapLoader.unloadModCallback = null;
			ModIOMapLoader.shouldUnloadMod = false;
		}
		yield break;
	}

	// Token: 0x06002657 RID: 9815 RVA: 0x0004924F File Offset: 0x0004744F
	private static IEnumerator UnloadSceneCoroutine(int sceneIndex, Action OnUnloadComplete = null)
	{
		if (!ModIOMapLoader.hasInstance)
		{
			yield break;
		}
		while (ModIOMapLoader.runningAsyncLoad)
		{
			yield return null;
		}
		UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects;
		string scenePathWithExtension = ModIOMapLoader.assetBundleSceneFilePaths[sceneIndex];
		string[] array = scenePathWithExtension.Split(".", StringSplitOptions.None);
		string text = "";
		string sceneName = "";
		if (!array.IsNullOrEmpty<string>())
		{
			text = array[0];
			if (text.Length > 0)
			{
				sceneName = Path.GetFileName(text);
			}
		}
		if (SceneManager.GetSceneByName(text).IsValid())
		{
			AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scenePathWithExtension, options);
			yield return asyncOperation;
			ModIOMapLoader.loadedSceneFilePaths.Remove(scenePathWithExtension);
			ModIOMapLoader.loadedSceneNames.Remove(sceneName);
			Action<string> action = ModIOMapLoader.sceneUnloadedCallback;
			if (action != null)
			{
				action(sceneName);
			}
			if (OnUnloadComplete != null)
			{
				OnUnloadComplete();
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06002658 RID: 9816 RVA: 0x00049265 File Offset: 0x00047465
	private static IEnumerator ResetLightmaps()
	{
		ModIOMapLoader.instance.dayNightManager.RequestRepopulateLightmaps();
		LoadSceneParameters parameters = new LoadSceneParameters
		{
			loadSceneMode = LoadSceneMode.Additive,
			localPhysicsMode = LocalPhysicsMode.None
		};
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(10, parameters);
		yield return asyncOperation;
		asyncOperation = SceneManager.UnloadSceneAsync(10);
		yield return asyncOperation;
		yield break;
	}

	// Token: 0x06002659 RID: 9817 RVA: 0x00106BE8 File Offset: 0x00104DE8
	private static void LoadLightmaps(Texture2D[] colorMaps, Texture2D[] dirMaps)
	{
		if (colorMaps.Length == 0)
		{
			return;
		}
		ModIOMapLoader.UnloadLightmaps();
		List<LightmapData> list = new List<LightmapData>(LightmapSettings.lightmaps);
		for (int i = 0; i < colorMaps.Length; i++)
		{
			bool flag = false;
			LightmapData lightmapData = new LightmapData();
			if (colorMaps[i] != null)
			{
				lightmapData.lightmapColor = colorMaps[i];
				flag = true;
				if (i < dirMaps.Length && dirMaps[i] != null)
				{
					lightmapData.lightmapDir = dirMaps[i];
				}
			}
			if (flag)
			{
				list.Add(lightmapData);
			}
		}
		LightmapSettings.lightmaps = list.ToArray();
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x00106C68 File Offset: 0x00104E68
	private static void UnloadLightmaps()
	{
		foreach (LightmapData lightmapData in LightmapSettings.lightmaps)
		{
			if (lightmapData.lightmapColor != null && !ModIOMapLoader.lightmapsToKeep.Contains(lightmapData.lightmapColor))
			{
				Resources.UnloadAsset(lightmapData.lightmapColor);
			}
			if (lightmapData.lightmapDir != null && !ModIOMapLoader.lightmapsToKeep.Contains(lightmapData.lightmapDir))
			{
				Resources.UnloadAsset(lightmapData.lightmapDir);
			}
		}
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x00106CE4 File Offset: 0x00104EE4
	private static bool SanitizeObject(GameObject gameObject)
	{
		if (gameObject == null)
		{
			return false;
		}
		if (!ModIOMapLoader.APPROVED_LAYERS.Contains(gameObject.layer))
		{
			gameObject.layer = 0;
		}
		foreach (Component component in gameObject.GetComponents<Component>())
		{
			if (component == null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject, true);
				return false;
			}
			bool flag = true;
			foreach (Type right in ModIOMapLoader.componentAllowlist)
			{
				if (component.GetType() == right)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				UnityEngine.Object.DestroyImmediate(gameObject, true);
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600265C RID: 9820 RVA: 0x0004926D File Offset: 0x0004746D
	private static IEnumerator ProcessAndInstantiateMap(GameObject map, bool useProgressCallback)
	{
		if (map.IsNull() || !ModIOMapLoader.hasInstance)
		{
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action = ModIOMapLoader.modLoadProgressCallback;
			if (action != null)
			{
				action(MapLoadStatus.Loading, 73, "PROCESSING ROOT MAP OBJECT");
			}
		}
		ModIOMapLoader.loadedMapDescriptor = map.GetComponent<MapDescriptor>();
		if (ModIOMapLoader.loadedMapDescriptor.IsNull())
		{
			yield break;
		}
		yield return ModIOMapLoader.ProcessChildObjects(map, useProgressCallback);
		if (ModIOMapLoader.shouldAbortSceneLoad)
		{
			yield break;
		}
		ModIOMapLoader.placeholderReplacements.Clear();
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action2 = ModIOMapLoader.modLoadProgressCallback;
			if (action2 != null)
			{
				action2(MapLoadStatus.Loading, 95, "PROCESSING COMPLETE");
			}
		}
		yield break;
	}

	// Token: 0x0600265D RID: 9821 RVA: 0x00049283 File Offset: 0x00047483
	private static IEnumerator ProcessChildObjects(GameObject parent, bool useProgressCallback)
	{
		if (parent == null)
		{
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action = ModIOMapLoader.modLoadProgressCallback;
			if (action != null)
			{
				action(MapLoadStatus.Loading, 76, "PROCESSING CHILD OBJECTS");
			}
		}
		int num;
		for (int i = 0; i < parent.transform.childCount; i = num + 1)
		{
			Transform child = parent.transform.GetChild(i);
			if (!(child == null))
			{
				GameObject gameObject = child.gameObject;
				if (!(gameObject == null))
				{
					try
					{
						ModIOMapLoader.SetupCollisions(gameObject);
						ModIOMapLoader.ReplaceDataOnlyScripts(gameObject);
						ModIOMapLoader.ReplacePlaceholders(gameObject);
					}
					catch (Exception ex)
					{
						ModIOMapLoader.shouldAbortSceneLoad = true;
						ModIOMapLoader.cachedExceptionMessage = ex.Message;
						yield break;
					}
					if (gameObject.transform.childCount > 0)
					{
						yield return ModIOMapLoader.ProcessChildObjects(gameObject, useProgressCallback);
						if (ModIOMapLoader.shouldAbortSceneLoad)
						{
							yield break;
						}
					}
					yield return null;
					if (ModIOMapLoader.shouldAbortSceneLoad)
					{
						yield break;
					}
				}
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x00106DA4 File Offset: 0x00104FA4
	private static void SetupCollisions(GameObject gameObject)
	{
		if (gameObject == null || ModIOMapLoader.placeholderReplacements.Contains(gameObject))
		{
			return;
		}
		Collider[] components = gameObject.GetComponents<Collider>();
		if (components == null)
		{
			return;
		}
		SurfaceOverrideSettings component = gameObject.GetComponent<SurfaceOverrideSettings>();
		GorillaSurfaceOverride gorillaSurfaceOverride = gameObject.AddComponent<GorillaSurfaceOverride>();
		if (component == null)
		{
			gorillaSurfaceOverride.overrideIndex = 0;
		}
		else
		{
			gorillaSurfaceOverride.overrideIndex = (int)component.soundOverride;
			gorillaSurfaceOverride.extraVelMultiplier = component.extraVelMultiplier;
			gorillaSurfaceOverride.extraVelMaxMultiplier = component.extraVelMaxMultiplier;
			gorillaSurfaceOverride.slidePercentageOverride = component.slidePercentage;
			gorillaSurfaceOverride.disablePushBackEffect = component.disablePushBackEffect;
			UnityEngine.Object.Destroy(component);
		}
		foreach (Collider collider in components)
		{
			if (!(collider == null))
			{
				if (collider.isTrigger)
				{
					gameObject.layer = UnityLayer.GorillaTrigger.ToLayerIndex();
					return;
				}
				if (gameObject.layer == UnityLayer.Default.ToLayerIndex())
				{
					gameObject.layer = UnityLayer.GorillaObject.ToLayerIndex();
				}
			}
		}
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x00106E8C File Offset: 0x0010508C
	private static void ReplaceDataOnlyScripts(GameObject gameObject)
	{
		MapBoundarySettings component = gameObject.GetComponent<MapBoundarySettings>();
		if (component != null)
		{
			MapBoundary mapBoundary = gameObject.AddComponent<MapBoundary>();
			if (mapBoundary != null)
			{
				mapBoundary.CopyTriggerSettings(component);
			}
			UnityEngine.Object.Destroy(component);
		}
		TagZoneSettings component2 = gameObject.GetComponent<TagZoneSettings>();
		if (component2 != null)
		{
			TagZone tagZone = gameObject.AddComponent<TagZone>();
			if (tagZone != null)
			{
				tagZone.CopyTriggerSettings(component2);
			}
			UnityEngine.Object.Destroy(component2);
		}
		TeleporterSettings component3 = gameObject.GetComponent<TeleporterSettings>();
		if (component3 != null)
		{
			Teleporter teleporter = gameObject.AddComponent<Teleporter>();
			if (teleporter != null)
			{
				teleporter.CopyTriggerSettings(component3);
			}
			UnityEngine.Object.Destroy(component3);
		}
		ObjectActivationTriggerSettings component4 = gameObject.GetComponent<ObjectActivationTriggerSettings>();
		if (component4 != null)
		{
			ObjectActivationTrigger objectActivationTrigger = gameObject.AddComponent<ObjectActivationTrigger>();
			if (objectActivationTrigger != null)
			{
				objectActivationTrigger.CopyTriggerSettings(component4);
			}
			UnityEngine.Object.Destroy(component4);
		}
		LoadZoneSettings component5 = gameObject.GetComponent<LoadZoneSettings>();
		if (component5 != null)
		{
			CustomMapLoadingZone customMapLoadingZone = gameObject.AddComponent<CustomMapLoadingZone>();
			if (customMapLoadingZone != null)
			{
				customMapLoadingZone.SetupLoadingZone(component5, ModIOMapLoader.assetBundleSceneFilePaths);
			}
			UnityEngine.Object.Destroy(component5);
		}
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x00106F94 File Offset: 0x00105194
	private static void ReplacePlaceholders(GameObject gameObject)
	{
		if (gameObject == null)
		{
			return;
		}
		GTObjectPlaceholder component = gameObject.GetComponent<GTObjectPlaceholder>();
		if (component == null)
		{
			return;
		}
		switch (component.PlaceholderObject)
		{
		case GTObject.LeafGlider:
			if (ModIOMapLoader.leafGliderIndex < ModIOMapLoader.instance.leafGliders.Length)
			{
				ModIOMapLoader.instance.leafGliders[ModIOMapLoader.leafGliderIndex].enabled = true;
				ModIOMapLoader.instance.leafGliders[ModIOMapLoader.leafGliderIndex].CustomMapLoad(component.transform, component.maxDistanceBeforeRespawn);
				ModIOMapLoader.instance.leafGliders[ModIOMapLoader.leafGliderIndex].transform.GetChild(0).gameObject.SetActive(true);
				ModIOMapLoader.leafGliderIndex++;
				return;
			}
			break;
		case GTObject.GliderWindVolume:
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(ModIOMapLoader.instance.gliderWindVolume, gameObject.transform.position, gameObject.transform.rotation);
			if (gameObject2 != null)
			{
				ModIOMapLoader.placeholderReplacements.Add(gameObject2);
				gameObject2.transform.localScale = gameObject.transform.localScale;
				gameObject.transform.localScale = Vector3.one;
				gameObject2.transform.parent = gameObject.transform;
				GliderWindVolume component2 = gameObject2.GetComponent<GliderWindVolume>();
				if (component2 == null)
				{
					return;
				}
				component2.SetProperties(component.maxSpeed, component.maxAccel, component.SpeedVSAccelCurve, component.localWindDirection);
				return;
			}
			break;
		}
		case GTObject.WaterVolume:
		{
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(ModIOMapLoader.instance.waterVolume, gameObject.transform.position, gameObject.transform.rotation);
			if (gameObject3 != null)
			{
				ModIOMapLoader.placeholderReplacements.Add(gameObject3);
				gameObject3.layer = LayerMask.NameToLayer("Water");
				gameObject3.transform.localScale = gameObject.transform.localScale;
				gameObject.transform.localScale = Vector3.one;
				gameObject3.transform.parent = gameObject.transform;
				MeshRenderer component3 = gameObject3.GetComponent<MeshRenderer>();
				if (component3 == null)
				{
					return;
				}
				if (component.useWaterMesh)
				{
					component3.enabled = true;
					WaterSurfaceMaterialController component4 = gameObject3.GetComponent<WaterSurfaceMaterialController>();
					if (component4 == null)
					{
						return;
					}
					component4.ScrollX = component.scrollTextureX;
					component4.ScrollY = component.scrollTextureY;
					component4.Scale = component.scaleTexture;
					return;
				}
			}
			break;
		}
		case GTObject.ForceVolume:
		{
			GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(ModIOMapLoader.instance.forceVolume, gameObject.transform.position, gameObject.transform.rotation);
			if (gameObject4)
			{
				ModIOMapLoader.placeholderReplacements.Add(gameObject4);
				gameObject4.transform.localScale = gameObject.transform.localScale;
				gameObject.transform.localScale = Vector3.one;
				gameObject4.transform.parent = gameObject.transform;
				ForceVolume component5 = gameObject4.GetComponent<ForceVolume>();
				if (component5 == null)
				{
					return;
				}
				component5.SetPropertiesFromPlaceholder(component.GetForceVolumeProperties());
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06002661 RID: 9825 RVA: 0x00049299 File Offset: 0x00047499
	public static bool OpenDoorToMap()
	{
		if (!ModIOMapLoader.hasInstance)
		{
			return false;
		}
		if (ModIOMapLoader.instance.accessDoor != null)
		{
			ModIOMapLoader.instance.accessDoor.OpenDoor();
			return true;
		}
		return false;
	}

	// Token: 0x06002662 RID: 9826 RVA: 0x00107288 File Offset: 0x00105488
	private static void OnAssetBundleLoaded(bool loadSucceeded, bool loadAborted)
	{
		if (ModIOMapLoader.shouldAbortSceneLoad)
		{
			if (ModIOMapLoader.mapBundle != null)
			{
				ModIOMapLoader.mapBundle.Unload(true);
			}
			ModIOMapLoader.mapBundle = null;
			return;
		}
		if (loadSucceeded)
		{
			int sceneIndex = 0;
			ModIOMapLoader.loadedMapModId = ModIOMapLoader.attemptedLoadID;
			if (ModIOMapLoader.initialSceneName != string.Empty)
			{
				for (int i = 0; i < ModIOMapLoader.assetBundleSceneFilePaths.Length; i++)
				{
					if (string.Equals(ModIOMapLoader.initialSceneName, ModIOMapLoader.GetSceneNameFromFilePath(ModIOMapLoader.assetBundleSceneFilePaths[i])))
					{
						sceneIndex = i;
						break;
					}
				}
			}
			ModIOMapLoader.instance.StartCoroutine(ModIOMapLoader.LoadSceneFromAssetBundle(sceneIndex, true, new Action<bool, bool, string>(ModIOMapLoader.OnLoadComplete)));
		}
	}

	// Token: 0x06002663 RID: 9827 RVA: 0x000492CC File Offset: 0x000474CC
	private static void OnIncrementalLoadComplete(bool loadSucceeded, bool loadAborted, string loadedScene)
	{
		if (loadSucceeded)
		{
			ModIOMapLoader.sceneLoadedCallback(loadedScene);
			return;
		}
		ModIOMapLoader.instance.StopAllCoroutines();
		ModIOMapLoader.isLoading = false;
	}

	// Token: 0x06002664 RID: 9828 RVA: 0x0010732C File Offset: 0x0010552C
	private static void OnLoadComplete(bool loadSucceeded, bool loadAborted, string loadedScene)
	{
		ModIOMapLoader.isLoading = false;
		GorillaNetworkJoinTrigger.EnableTriggerJoins();
		if (loadSucceeded)
		{
			Action<MapLoadStatus, int, string> action = ModIOMapLoader.modLoadProgressCallback;
			if (action != null)
			{
				action(MapLoadStatus.Loading, 100, "Load Complete");
			}
			if (ModIOMapLoader.instance.networkTrigger != null)
			{
				ModIOMapLoader.instance.networkTrigger.SetActive(true);
			}
		}
		else
		{
			ModIOMapLoader.loadedMapDescriptor = null;
		}
		if (!loadAborted)
		{
			Action<bool> action2 = ModIOMapLoader.modLoadedCallback;
			if (action2 != null)
			{
				action2(loadSucceeded);
			}
			if (loadSucceeded)
			{
				Action<string> action3 = ModIOMapLoader.sceneLoadedCallback;
				if (action3 == null)
				{
					return;
				}
				action3(loadedScene);
			}
		}
	}

	// Token: 0x06002665 RID: 9829 RVA: 0x000492EF File Offset: 0x000474EF
	private static string GetSceneNameFromFilePath(string filePath)
	{
		string[] array = filePath.Split("/", StringSplitOptions.None);
		return array[array.Length - 1].Split(".", StringSplitOptions.None)[0];
	}

	// Token: 0x06002666 RID: 9830 RVA: 0x001073B4 File Offset: 0x001055B4
	public static MapPackageInfo GetPackageInfo(string packageInfoFilePath)
	{
		MapPackageInfo result;
		using (StreamReader streamReader = new StreamReader(File.OpenRead(packageInfoFilePath), Encoding.Default))
		{
			result = JsonConvert.DeserializeObject<MapPackageInfo>(streamReader.ReadToEnd());
		}
		return result;
	}

	// Token: 0x06002667 RID: 9831 RVA: 0x00049310 File Offset: 0x00047510
	public static Transform GetCustomMapsDefaultSpawnLocation()
	{
		if (ModIOMapLoader.hasInstance)
		{
			return ModIOMapLoader.instance.CustomMapsDefaultSpawnLocation;
		}
		return null;
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x00049327 File Offset: 0x00047527
	public static bool IsModLoaded(long mapModId = 0L)
	{
		if (mapModId != 0L)
		{
			return !ModIOMapLoader.IsLoading && ModIOMapLoader.LoadedMapModId == mapModId;
		}
		return !ModIOMapLoader.IsLoading && ModIOMapLoader.LoadedMapModId != 0L;
	}

	// Token: 0x04002A36 RID: 10806
	[OnEnterPlay_SetNull]
	private static volatile ModIOMapLoader instance;

	// Token: 0x04002A37 RID: 10807
	[OnEnterPlay_Set(false)]
	private static bool hasInstance;

	// Token: 0x04002A38 RID: 10808
	public Transform CustomMapsDefaultSpawnLocation;

	// Token: 0x04002A39 RID: 10809
	public ModIOCustomMapAccessDoor accessDoor;

	// Token: 0x04002A3A RID: 10810
	public GameObject networkTrigger;

	// Token: 0x04002A3B RID: 10811
	[SerializeField]
	private BetterDayNightManager dayNightManager;

	// Token: 0x04002A3C RID: 10812
	[SerializeField]
	private GameObject placeholderParent;

	// Token: 0x04002A3D RID: 10813
	[SerializeField]
	private GliderHoldable[] leafGliders;

	// Token: 0x04002A3E RID: 10814
	[SerializeField]
	private GameObject leafGlider;

	// Token: 0x04002A3F RID: 10815
	[SerializeField]
	private GameObject gliderWindVolume;

	// Token: 0x04002A40 RID: 10816
	[SerializeField]
	private GameObject waterVolume;

	// Token: 0x04002A41 RID: 10817
	[SerializeField]
	private GameObject forceVolume;

	// Token: 0x04002A42 RID: 10818
	private static readonly List<int> APPROVED_LAYERS = new List<int>
	{
		0,
		1,
		2,
		4,
		5
	};

	// Token: 0x04002A43 RID: 10819
	private static bool isLoading;

	// Token: 0x04002A44 RID: 10820
	private static bool isUnloading;

	// Token: 0x04002A45 RID: 10821
	private static bool runningAsyncLoad = false;

	// Token: 0x04002A46 RID: 10822
	private static long attemptedLoadID = 0L;

	// Token: 0x04002A47 RID: 10823
	private static string attemptedSceneToLoad;

	// Token: 0x04002A48 RID: 10824
	private static bool shouldUnloadMod = false;

	// Token: 0x04002A49 RID: 10825
	private static AssetBundle mapBundle;

	// Token: 0x04002A4A RID: 10826
	private static MapDescriptor loadedMapDescriptor;

	// Token: 0x04002A4B RID: 10827
	private static string initialSceneName = string.Empty;

	// Token: 0x04002A4C RID: 10828
	private static string loadedMapLevelName;

	// Token: 0x04002A4D RID: 10829
	private static long loadedMapModId;

	// Token: 0x04002A4E RID: 10830
	private static Action<MapLoadStatus, int, string> modLoadProgressCallback;

	// Token: 0x04002A4F RID: 10831
	private static Action<bool> modLoadedCallback;

	// Token: 0x04002A50 RID: 10832
	private static Coroutine sceneLoadingCoroutine;

	// Token: 0x04002A51 RID: 10833
	private static Action<string> sceneLoadedCallback;

	// Token: 0x04002A52 RID: 10834
	private static Action<string> sceneUnloadedCallback;

	// Token: 0x04002A53 RID: 10835
	private static string[] assetBundleSceneFilePaths;

	// Token: 0x04002A54 RID: 10836
	private static List<string> loadedSceneFilePaths = new List<string>();

	// Token: 0x04002A55 RID: 10837
	private static List<string> loadedSceneNames = new List<string>();

	// Token: 0x04002A56 RID: 10838
	private static int leafGliderIndex;

	// Token: 0x04002A57 RID: 10839
	private static bool shouldAbortSceneLoad = false;

	// Token: 0x04002A58 RID: 10840
	private static Action abortModLoadCallback;

	// Token: 0x04002A59 RID: 10841
	private static Action unloadModCallback;

	// Token: 0x04002A5A RID: 10842
	private static string cachedExceptionMessage = "";

	// Token: 0x04002A5B RID: 10843
	private static LightmapData[] lightmaps;

	// Token: 0x04002A5C RID: 10844
	private static List<Texture2D> lightmapsToKeep = new List<Texture2D>();

	// Token: 0x04002A5D RID: 10845
	private static List<GameObject> placeholderReplacements = new List<GameObject>();

	// Token: 0x04002A5E RID: 10846
	private string dontDestroyOnLoadSceneName = "";

	// Token: 0x04002A5F RID: 10847
	private static List<Type> componentAllowlist = new List<Type>
	{
		typeof(MeshRenderer),
		typeof(Transform),
		typeof(MeshFilter),
		typeof(MeshRenderer),
		typeof(Collider),
		typeof(BoxCollider),
		typeof(SphereCollider),
		typeof(CapsuleCollider),
		typeof(MeshCollider),
		typeof(Light),
		typeof(UniversalAdditionalLightData),
		typeof(ReflectionProbe),
		typeof(AudioSource),
		typeof(Animator),
		typeof(SkinnedMeshRenderer),
		typeof(ProBuilderMesh),
		typeof(TextMesh),
		typeof(TMP_Text),
		typeof(TextMeshPro),
		typeof(ParticleSystem),
		typeof(ParticleSystemRenderer),
		typeof(RectTransform),
		typeof(SpriteRenderer),
		typeof(BillboardRenderer),
		typeof(GTObjectPlaceholder),
		typeof(SurfaceOverrideSettings),
		typeof(AccessDoorPlaceholder),
		typeof(MapOrientationPoint),
		typeof(TeleporterSettings),
		typeof(TagZoneSettings),
		typeof(MapBoundarySettings),
		typeof(ObjectActivationTriggerSettings),
		typeof(LoadZoneSettings),
		typeof(MapDescriptor),
		typeof(BakerySkyLight),
		typeof(ftLightmapsStorage),
		typeof(BakeryDirectLight),
		typeof(BakeryPointLight)
	};

	// Token: 0x04002A60 RID: 10848
	private static Type[] badComponents = new Type[]
	{
		typeof(EventTrigger),
		typeof(UIBehaviour),
		typeof(GorillaPressableButton),
		typeof(GorillaPressableDelayButton),
		typeof(Camera),
		typeof(AudioListener)
	};
}
