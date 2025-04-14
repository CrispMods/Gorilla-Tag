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

// Token: 0x02000603 RID: 1539
public class ModIOMapLoader : MonoBehaviour
{
	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x0600263B RID: 9787 RVA: 0x000BC5AF File Offset: 0x000BA7AF
	// (set) Token: 0x0600263A RID: 9786 RVA: 0x000BC598 File Offset: 0x000BA798
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

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x0600263C RID: 9788 RVA: 0x000BC5B6 File Offset: 0x000BA7B6
	public static long LoadedMapModId
	{
		get
		{
			return ModIOMapLoader.loadedMapModId;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x0600263D RID: 9789 RVA: 0x000BC5BD File Offset: 0x000BA7BD
	public static MapDescriptor LoadedMapDescriptor
	{
		get
		{
			return ModIOMapLoader.loadedMapDescriptor;
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x0600263E RID: 9790 RVA: 0x000BC5C4 File Offset: 0x000BA7C4
	public static long LoadingMapModId
	{
		get
		{
			return ModIOMapLoader.attemptedLoadID;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x0600263F RID: 9791 RVA: 0x000BC5CB File Offset: 0x000BA7CB
	public static bool IsLoading
	{
		get
		{
			return ModIOMapLoader.isLoading;
		}
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x000BC5D2 File Offset: 0x000BA7D2
	public static bool IsCustomScene(string sceneName)
	{
		return ModIOMapLoader.loadedSceneNames.Contains(sceneName);
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x000BC5DF File Offset: 0x000BA7DF
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
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x000BC61C File Offset: 0x000BA81C
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

	// Token: 0x06002643 RID: 9795 RVA: 0x000BC710 File Offset: 0x000BA910
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

	// Token: 0x06002644 RID: 9796 RVA: 0x000BC78A File Offset: 0x000BA98A
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

	// Token: 0x06002645 RID: 9797 RVA: 0x000BC7BC File Offset: 0x000BA9BC
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

	// Token: 0x06002646 RID: 9798 RVA: 0x000BC7D2 File Offset: 0x000BA9D2
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

	// Token: 0x06002647 RID: 9799 RVA: 0x000BC7E1 File Offset: 0x000BA9E1
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

	// Token: 0x06002648 RID: 9800 RVA: 0x000BC7F0 File Offset: 0x000BA9F0
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

	// Token: 0x06002649 RID: 9801 RVA: 0x000BC80D File Offset: 0x000BAA0D
	private static void RequestAbortModLoad(Action callback = null)
	{
		ModIOMapLoader.abortModLoadCallback = callback;
		ModIOMapLoader.shouldAbortSceneLoad = true;
		ModIOMapLoader.shouldUnloadMod = true;
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x000BC821 File Offset: 0x000BAA21
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

	// Token: 0x0600264B RID: 9803 RVA: 0x000BC830 File Offset: 0x000BAA30
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
		GameObject[] array = Object.FindObjectsOfType<GameObject>();
		MapDescriptor[] array2 = Object.FindObjectsOfType<MapDescriptor>();
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

	// Token: 0x0600264C RID: 9804 RVA: 0x000BC850 File Offset: 0x000BAA50
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

	// Token: 0x0600264D RID: 9805 RVA: 0x000BC8E7 File Offset: 0x000BAAE7
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

	// Token: 0x0600264E RID: 9806 RVA: 0x000BC8EF File Offset: 0x000BAAEF
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

	// Token: 0x0600264F RID: 9807 RVA: 0x000BC8F7 File Offset: 0x000BAAF7
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

	// Token: 0x06002650 RID: 9808 RVA: 0x000BC90D File Offset: 0x000BAB0D
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

	// Token: 0x06002651 RID: 9809 RVA: 0x000BC918 File Offset: 0x000BAB18
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

	// Token: 0x06002652 RID: 9810 RVA: 0x000BC998 File Offset: 0x000BAB98
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

	// Token: 0x06002653 RID: 9811 RVA: 0x000BCA14 File Offset: 0x000BAC14
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
				Object.DestroyImmediate(gameObject, true);
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
				Object.DestroyImmediate(gameObject, true);
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002654 RID: 9812 RVA: 0x000BCAD4 File Offset: 0x000BACD4
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

	// Token: 0x06002655 RID: 9813 RVA: 0x000BCAEA File Offset: 0x000BACEA
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

	// Token: 0x06002656 RID: 9814 RVA: 0x000BCB00 File Offset: 0x000BAD00
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
			Object.Destroy(component);
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

	// Token: 0x06002657 RID: 9815 RVA: 0x000BCBE8 File Offset: 0x000BADE8
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
			Object.Destroy(component);
		}
		TagZoneSettings component2 = gameObject.GetComponent<TagZoneSettings>();
		if (component2 != null)
		{
			TagZone tagZone = gameObject.AddComponent<TagZone>();
			if (tagZone != null)
			{
				tagZone.CopyTriggerSettings(component2);
			}
			Object.Destroy(component2);
		}
		TeleporterSettings component3 = gameObject.GetComponent<TeleporterSettings>();
		if (component3 != null)
		{
			Teleporter teleporter = gameObject.AddComponent<Teleporter>();
			if (teleporter != null)
			{
				teleporter.CopyTriggerSettings(component3);
			}
			Object.Destroy(component3);
		}
		ObjectActivationTriggerSettings component4 = gameObject.GetComponent<ObjectActivationTriggerSettings>();
		if (component4 != null)
		{
			ObjectActivationTrigger objectActivationTrigger = gameObject.AddComponent<ObjectActivationTrigger>();
			if (objectActivationTrigger != null)
			{
				objectActivationTrigger.CopyTriggerSettings(component4);
			}
			Object.Destroy(component4);
		}
		LoadZoneSettings component5 = gameObject.GetComponent<LoadZoneSettings>();
		if (component5 != null)
		{
			CustomMapLoadingZone customMapLoadingZone = gameObject.AddComponent<CustomMapLoadingZone>();
			if (customMapLoadingZone != null)
			{
				customMapLoadingZone.SetupLoadingZone(component5, ModIOMapLoader.assetBundleSceneFilePaths);
			}
			Object.Destroy(component5);
		}
	}

	// Token: 0x06002658 RID: 9816 RVA: 0x000BCCF0 File Offset: 0x000BAEF0
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
			GameObject gameObject2 = Object.Instantiate<GameObject>(ModIOMapLoader.instance.gliderWindVolume, gameObject.transform.position, gameObject.transform.rotation);
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
			GameObject gameObject3 = Object.Instantiate<GameObject>(ModIOMapLoader.instance.waterVolume, gameObject.transform.position, gameObject.transform.rotation);
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
			GameObject gameObject4 = Object.Instantiate<GameObject>(ModIOMapLoader.instance.forceVolume, gameObject.transform.position, gameObject.transform.rotation);
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

	// Token: 0x06002659 RID: 9817 RVA: 0x000BCFE2 File Offset: 0x000BB1E2
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

	// Token: 0x0600265A RID: 9818 RVA: 0x000BD018 File Offset: 0x000BB218
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

	// Token: 0x0600265B RID: 9819 RVA: 0x000BD0B9 File Offset: 0x000BB2B9
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

	// Token: 0x0600265C RID: 9820 RVA: 0x000BD0DC File Offset: 0x000BB2DC
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

	// Token: 0x0600265D RID: 9821 RVA: 0x000BD164 File Offset: 0x000BB364
	private static string GetSceneNameFromFilePath(string filePath)
	{
		string[] array = filePath.Split("/", StringSplitOptions.None);
		return array[array.Length - 1].Split(".", StringSplitOptions.None)[0];
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x000BD188 File Offset: 0x000BB388
	public static MapPackageInfo GetPackageInfo(string packageInfoFilePath)
	{
		MapPackageInfo result;
		using (StreamReader streamReader = new StreamReader(File.OpenRead(packageInfoFilePath), Encoding.Default))
		{
			result = JsonConvert.DeserializeObject<MapPackageInfo>(streamReader.ReadToEnd());
		}
		return result;
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x000BD1D0 File Offset: 0x000BB3D0
	public static Transform GetCustomMapsDefaultSpawnLocation()
	{
		if (ModIOMapLoader.hasInstance)
		{
			return ModIOMapLoader.instance.CustomMapsDefaultSpawnLocation;
		}
		return null;
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x000BD1E7 File Offset: 0x000BB3E7
	public static bool IsModLoaded(long mapModId = 0L)
	{
		if (mapModId != 0L)
		{
			return !ModIOMapLoader.IsLoading && ModIOMapLoader.LoadedMapModId == mapModId;
		}
		return !ModIOMapLoader.IsLoading && ModIOMapLoader.LoadedMapModId != 0L;
	}

	// Token: 0x04002A30 RID: 10800
	[OnEnterPlay_SetNull]
	private static volatile ModIOMapLoader instance;

	// Token: 0x04002A31 RID: 10801
	[OnEnterPlay_Set(false)]
	private static bool hasInstance;

	// Token: 0x04002A32 RID: 10802
	public Transform CustomMapsDefaultSpawnLocation;

	// Token: 0x04002A33 RID: 10803
	public ModIOCustomMapAccessDoor accessDoor;

	// Token: 0x04002A34 RID: 10804
	public GameObject networkTrigger;

	// Token: 0x04002A35 RID: 10805
	[SerializeField]
	private BetterDayNightManager dayNightManager;

	// Token: 0x04002A36 RID: 10806
	[SerializeField]
	private GameObject placeholderParent;

	// Token: 0x04002A37 RID: 10807
	[SerializeField]
	private GliderHoldable[] leafGliders;

	// Token: 0x04002A38 RID: 10808
	[SerializeField]
	private GameObject leafGlider;

	// Token: 0x04002A39 RID: 10809
	[SerializeField]
	private GameObject gliderWindVolume;

	// Token: 0x04002A3A RID: 10810
	[SerializeField]
	private GameObject waterVolume;

	// Token: 0x04002A3B RID: 10811
	[SerializeField]
	private GameObject forceVolume;

	// Token: 0x04002A3C RID: 10812
	private static readonly List<int> APPROVED_LAYERS = new List<int>
	{
		0,
		1,
		2,
		4,
		5
	};

	// Token: 0x04002A3D RID: 10813
	private static bool isLoading;

	// Token: 0x04002A3E RID: 10814
	private static bool isUnloading;

	// Token: 0x04002A3F RID: 10815
	private static bool runningAsyncLoad = false;

	// Token: 0x04002A40 RID: 10816
	private static long attemptedLoadID = 0L;

	// Token: 0x04002A41 RID: 10817
	private static string attemptedSceneToLoad;

	// Token: 0x04002A42 RID: 10818
	private static bool shouldUnloadMod = false;

	// Token: 0x04002A43 RID: 10819
	private static AssetBundle mapBundle;

	// Token: 0x04002A44 RID: 10820
	private static MapDescriptor loadedMapDescriptor;

	// Token: 0x04002A45 RID: 10821
	private static string initialSceneName = string.Empty;

	// Token: 0x04002A46 RID: 10822
	private static string loadedMapLevelName;

	// Token: 0x04002A47 RID: 10823
	private static long loadedMapModId;

	// Token: 0x04002A48 RID: 10824
	private static Action<MapLoadStatus, int, string> modLoadProgressCallback;

	// Token: 0x04002A49 RID: 10825
	private static Action<bool> modLoadedCallback;

	// Token: 0x04002A4A RID: 10826
	private static Coroutine sceneLoadingCoroutine;

	// Token: 0x04002A4B RID: 10827
	private static Action<string> sceneLoadedCallback;

	// Token: 0x04002A4C RID: 10828
	private static Action<string> sceneUnloadedCallback;

	// Token: 0x04002A4D RID: 10829
	private static string[] assetBundleSceneFilePaths;

	// Token: 0x04002A4E RID: 10830
	private static List<string> loadedSceneFilePaths = new List<string>();

	// Token: 0x04002A4F RID: 10831
	private static List<string> loadedSceneNames = new List<string>();

	// Token: 0x04002A50 RID: 10832
	private static int leafGliderIndex;

	// Token: 0x04002A51 RID: 10833
	private static bool shouldAbortSceneLoad = false;

	// Token: 0x04002A52 RID: 10834
	private static Action abortModLoadCallback;

	// Token: 0x04002A53 RID: 10835
	private static Action unloadModCallback;

	// Token: 0x04002A54 RID: 10836
	private static string cachedExceptionMessage = "";

	// Token: 0x04002A55 RID: 10837
	private static LightmapData[] lightmaps;

	// Token: 0x04002A56 RID: 10838
	private static List<Texture2D> lightmapsToKeep = new List<Texture2D>();

	// Token: 0x04002A57 RID: 10839
	private static List<GameObject> placeholderReplacements = new List<GameObject>();

	// Token: 0x04002A58 RID: 10840
	private string dontDestroyOnLoadSceneName = "";

	// Token: 0x04002A59 RID: 10841
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

	// Token: 0x04002A5A RID: 10842
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
