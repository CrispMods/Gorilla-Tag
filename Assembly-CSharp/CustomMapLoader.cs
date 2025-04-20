using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GorillaExtensions;
using GorillaLocomotion.Swimming;
using GorillaNetworking;
using GorillaNetworking.Store;
using GorillaTag.Rendering;
using GorillaTagScripts.CustomMapSupport;
using GorillaTagScripts.ModIO;
using GorillaTagScripts.VirtualStumpCustomMaps;
using GT_CustomMapSupportRuntime;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000664 RID: 1636
public class CustomMapLoader : MonoBehaviour
{
	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x0600287A RID: 10362 RVA: 0x0004B836 File Offset: 0x00049A36
	// (set) Token: 0x06002879 RID: 10361 RVA: 0x0004B81F File Offset: 0x00049A1F
	public static string LoadedMapLevelName
	{
		get
		{
			return CustomMapLoader.loadedMapLevelName;
		}
		set
		{
			CustomMapLoader.loadedMapLevelName = value.Replace(" ", "");
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x0600287B RID: 10363 RVA: 0x0004B83D File Offset: 0x00049A3D
	public static long LoadedMapModId
	{
		get
		{
			return CustomMapLoader.loadedMapModId;
		}
	}

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x0600287C RID: 10364 RVA: 0x0004B844 File Offset: 0x00049A44
	public static MapDescriptor LoadedMapDescriptor
	{
		get
		{
			return CustomMapLoader.loadedMapDescriptor;
		}
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x0600287D RID: 10365 RVA: 0x0004B84B File Offset: 0x00049A4B
	public static long LoadingMapModId
	{
		get
		{
			return CustomMapLoader.attemptedLoadID;
		}
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x0600287E RID: 10366 RVA: 0x0004B852 File Offset: 0x00049A52
	public static bool IsLoading
	{
		get
		{
			return CustomMapLoader.isLoading;
		}
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x0004B859 File Offset: 0x00049A59
	public static bool IsCustomScene(string sceneName)
	{
		return CustomMapLoader.loadedSceneNames.Contains(sceneName);
	}

	// Token: 0x06002880 RID: 10368 RVA: 0x0004B866 File Offset: 0x00049A66
	private void Awake()
	{
		if (CustomMapLoader.instance == null)
		{
			CustomMapLoader.instance = this;
			CustomMapLoader.hasInstance = true;
			return;
		}
		if (CustomMapLoader.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002881 RID: 10369 RVA: 0x001115DC File Offset: 0x0010F7DC
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

	// Token: 0x06002882 RID: 10370 RVA: 0x001116D0 File Offset: 0x0010F8D0
	public static void LoadMap(long mapModId, string mapFilePath, Action<bool> onLoadComplete, Action<MapLoadStatus, int, string> progressCallback, Action<string> onSceneLoaded)
	{
		if (!CustomMapLoader.hasInstance)
		{
			return;
		}
		if (CustomMapLoader.isLoading)
		{
			return;
		}
		if (CustomMapLoader.isUnloading)
		{
			if (onLoadComplete != null)
			{
				onLoadComplete(false);
			}
			return;
		}
		if (CustomMapLoader.IsModLoaded(mapModId))
		{
			if (onLoadComplete != null)
			{
				onLoadComplete(true);
			}
			return;
		}
		GorillaNetworkJoinTrigger.DisableTriggerJoins();
		CustomMapLoader.modLoadProgressCallback = progressCallback;
		CustomMapLoader.modLoadedCallback = onLoadComplete;
		CustomMapLoader.sceneLoadedCallback = onSceneLoaded;
		CustomMapLoader.instance.StartCoroutine(CustomMapLoader.LoadAssetBundle(mapModId, mapFilePath, new Action<bool, bool>(CustomMapLoader.OnAssetBundleLoaded)));
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x0011174C File Offset: 0x0010F94C
	public static void ResetToInitialZone(Action<string> onSceneLoaded, Action<string> onSceneUnloaded)
	{
		int[] array = new int[]
		{
			CustomMapLoader.initialSceneIndex
		};
		List<int> list = CustomMapLoader.loadedSceneIndexes;
		list.Remove(CustomMapLoader.initialSceneIndex);
		if (CustomMapLoader.sceneLoadingCoroutine != null)
		{
			CustomMapLoader.LoadZoneRequest item = new CustomMapLoader.LoadZoneRequest
			{
				sceneIndexesToLoad = array,
				sceneIndexesToUnload = list.ToArray(),
				onSceneLoadedCallback = onSceneLoaded,
				onSceneUnloadedCallback = onSceneUnloaded
			};
			CustomMapLoader.queuedLoadZoneRequests.Add(item);
			return;
		}
		CustomMapLoader.sceneLoadedCallback = onSceneLoaded;
		CustomMapLoader.sceneUnloadedCallback = onSceneUnloaded;
		CustomMapLoader.sceneLoadingCoroutine = CustomMapLoader.instance.StartCoroutine(CustomMapLoader.LoadZoneCoroutine(array, list.ToArray()));
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x001117E8 File Offset: 0x0010F9E8
	public static void LoadZoneTriggered(int[] loadSceneIndexes, int[] unloadSceneIndexes, Action<string> onSceneLoaded, Action<string> onSceneUnloaded)
	{
		string str = "";
		for (int i = 0; i < loadSceneIndexes.Length; i++)
		{
			str += loadSceneIndexes[i].ToString();
			if (i != loadSceneIndexes.Length - 1)
			{
				str += ", ";
			}
		}
		string str2 = "";
		for (int j = 0; j < unloadSceneIndexes.Length; j++)
		{
			str2 += unloadSceneIndexes[j].ToString();
			if (j != unloadSceneIndexes.Length - 1)
			{
				str2 += ", ";
			}
		}
		if (CustomMapLoader.sceneLoadingCoroutine != null)
		{
			CustomMapLoader.LoadZoneRequest item = new CustomMapLoader.LoadZoneRequest
			{
				sceneIndexesToLoad = loadSceneIndexes,
				sceneIndexesToUnload = unloadSceneIndexes,
				onSceneLoadedCallback = onSceneLoaded,
				onSceneUnloadedCallback = onSceneUnloaded
			};
			CustomMapLoader.queuedLoadZoneRequests.Add(item);
			return;
		}
		CustomMapLoader.sceneLoadedCallback = onSceneLoaded;
		CustomMapLoader.sceneUnloadedCallback = onSceneUnloaded;
		CustomMapLoader.sceneLoadingCoroutine = CustomMapLoader.instance.StartCoroutine(CustomMapLoader.LoadZoneCoroutine(loadSceneIndexes, unloadSceneIndexes));
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x0004B8A0 File Offset: 0x00049AA0
	private static IEnumerator LoadZoneCoroutine(int[] loadScenes, int[] unloadScenes)
	{
		if (!unloadScenes.IsNullOrEmpty<int>())
		{
			yield return CustomMapLoader.UnloadScenesCoroutine(unloadScenes);
		}
		if (!loadScenes.IsNullOrEmpty<int>())
		{
			yield return CustomMapLoader.LoadScenesCoroutine(loadScenes);
		}
		if (CustomMapLoader.sceneLoadingCoroutine != null)
		{
			CustomMapLoader.instance.StopCoroutine(CustomMapLoader.sceneLoadingCoroutine);
			CustomMapLoader.sceneLoadingCoroutine = null;
		}
		if (CustomMapLoader.queuedLoadZoneRequests.Count > 0)
		{
			CustomMapLoader.LoadZoneRequest loadZoneRequest = CustomMapLoader.queuedLoadZoneRequests[0];
			CustomMapLoader.queuedLoadZoneRequests.RemoveAt(0);
			CustomMapLoader.LoadZoneTriggered(loadZoneRequest.sceneIndexesToLoad, loadZoneRequest.sceneIndexesToUnload, loadZoneRequest.onSceneLoadedCallback, loadZoneRequest.onSceneUnloadedCallback);
		}
		yield break;
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x0004B8B6 File Offset: 0x00049AB6
	private static IEnumerator LoadScenesCoroutine(int[] sceneIndexes)
	{
		int num;
		for (int i = 0; i < sceneIndexes.Length; i = num + 1)
		{
			if (!CustomMapLoader.loadedSceneFilePaths.Contains(CustomMapLoader.assetBundleSceneFilePaths[sceneIndexes[i]]))
			{
				yield return CustomMapLoader.LoadSceneFromAssetBundle(sceneIndexes[i], false, new Action<bool, bool, string>(CustomMapLoader.OnIncrementalLoadComplete));
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x0004B8C5 File Offset: 0x00049AC5
	private static IEnumerator UnloadScenesCoroutine(int[] sceneIndexes)
	{
		int num;
		for (int i = 0; i < sceneIndexes.Length; i = num + 1)
		{
			yield return CustomMapLoader.UnloadSceneCoroutine(sceneIndexes[i], null);
			num = i;
		}
		yield break;
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x0004B8D4 File Offset: 0x00049AD4
	private static IEnumerator LoadAssetBundle(long mapModID, string packageInfoFilePath, Action<bool, bool> OnLoadComplete)
	{
		if (CustomMapLoader.isLoading)
		{
			if (OnLoadComplete != null)
			{
				OnLoadComplete(false, false);
			}
			yield break;
		}
		yield return CustomMapLoader.CloseDoorAndUnloadModCoroutine();
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield return CustomMapLoader.AbortSceneLoad(-1);
			OnLoadComplete(false, true);
			yield break;
		}
		CustomMapLoader.isLoading = true;
		CustomMapLoader.attemptedLoadID = mapModID;
		Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
		if (action != null)
		{
			action(MapLoadStatus.Loading, 1, "GRABBING LIGHTMAP DATA");
		}
		CustomMapLoader.lightmaps = new LightmapData[LightmapSettings.lightmaps.Length];
		if (CustomMapLoader.lightmapsToKeep.Count > 0)
		{
			CustomMapLoader.lightmapsToKeep.Clear();
		}
		CustomMapLoader.lightmapsToKeep = new List<Texture2D>(LightmapSettings.lightmaps.Length * 2);
		for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
		{
			CustomMapLoader.lightmaps[i] = LightmapSettings.lightmaps[i];
			if (LightmapSettings.lightmaps[i].lightmapColor != null)
			{
				CustomMapLoader.lightmapsToKeep.Add(LightmapSettings.lightmaps[i].lightmapColor);
			}
			if (LightmapSettings.lightmaps[i].lightmapDir != null)
			{
				CustomMapLoader.lightmapsToKeep.Add(LightmapSettings.lightmaps[i].lightmapDir);
			}
		}
		Action<MapLoadStatus, int, string> action2 = CustomMapLoader.modLoadProgressCallback;
		if (action2 != null)
		{
			action2(MapLoadStatus.Loading, 2, "LOADING PACKAGE INFO");
		}
		MapPackageInfo packageInfo;
		try
		{
			packageInfo = CustomMapLoader.GetPackageInfo(packageInfoFilePath);
		}
		catch (Exception ex)
		{
			Action<MapLoadStatus, int, string> action3 = CustomMapLoader.modLoadProgressCallback;
			if (action3 != null)
			{
				action3(MapLoadStatus.Error, 0, ex.Message);
			}
			yield break;
		}
		if (packageInfo == null)
		{
			Action<MapLoadStatus, int, string> action4 = CustomMapLoader.modLoadProgressCallback;
			if (action4 != null)
			{
				action4(MapLoadStatus.Error, 0, "FAILED TO READ FILE AT " + packageInfoFilePath);
			}
			OnLoadComplete(false, false);
			yield break;
		}
		CustomMapLoader.initialSceneName = packageInfo.initialScene;
		Action<MapLoadStatus, int, string> action5 = CustomMapLoader.modLoadProgressCallback;
		if (action5 != null)
		{
			action5(MapLoadStatus.Loading, 3, "PACKAGE INFO LOADED");
		}
		string path = Path.GetDirectoryName(packageInfoFilePath) + "/" + packageInfo.pcFileName;
		Action<MapLoadStatus, int, string> action6 = CustomMapLoader.modLoadProgressCallback;
		if (action6 != null)
		{
			action6(MapLoadStatus.Loading, 12, "LOADING MAP ASSET BUNDLE");
		}
		AssetBundleCreateRequest loadBundleRequest = AssetBundle.LoadFromFileAsync(path);
		yield return loadBundleRequest;
		CustomMapLoader.mapBundle = loadBundleRequest.assetBundle;
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield return CustomMapLoader.AbortSceneLoad(-1);
			OnLoadComplete(false, true);
			yield break;
		}
		if (CustomMapLoader.mapBundle == null)
		{
			Action<MapLoadStatus, int, string> action7 = CustomMapLoader.modLoadProgressCallback;
			if (action7 != null)
			{
				action7(MapLoadStatus.Error, 0, "CUSTOM MAP ASSET BUNDLE FAILED TO LOAD");
			}
			OnLoadComplete(false, false);
			yield break;
		}
		if (!CustomMapLoader.mapBundle.isStreamedSceneAssetBundle)
		{
			CustomMapLoader.mapBundle.Unload(true);
			Action<MapLoadStatus, int, string> action8 = CustomMapLoader.modLoadProgressCallback;
			if (action8 != null)
			{
				action8(MapLoadStatus.Error, 0, "AssetBundle does not contain a Unity Scene file");
			}
			OnLoadComplete(false, false);
			yield break;
		}
		Action<MapLoadStatus, int, string> action9 = CustomMapLoader.modLoadProgressCallback;
		if (action9 != null)
		{
			action9(MapLoadStatus.Loading, 20, "MAP ASSET BUNDLE LOADED");
		}
		CustomMapLoader.mapBundle.GetAllAssetNames();
		CustomMapLoader.assetBundleSceneFilePaths = CustomMapLoader.mapBundle.GetAllScenePaths();
		if (CustomMapLoader.assetBundleSceneFilePaths.Length == 0)
		{
			CustomMapLoader.mapBundle.Unload(true);
			Action<MapLoadStatus, int, string> action10 = CustomMapLoader.modLoadProgressCallback;
			if (action10 != null)
			{
				action10(MapLoadStatus.Error, 0, "AssetBundle does not contain a Unity Scene file");
			}
			OnLoadComplete(false, false);
			yield break;
		}
		foreach (string text in CustomMapLoader.assetBundleSceneFilePaths)
		{
			if (text.Equals(CustomMapLoader.instance.dontDestroyOnLoadSceneName, StringComparison.OrdinalIgnoreCase))
			{
				CustomMapLoader.mapBundle.Unload(true);
				Action<MapLoadStatus, int, string> action11 = CustomMapLoader.modLoadProgressCallback;
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

	// Token: 0x06002889 RID: 10377 RVA: 0x0004B8F1 File Offset: 0x00049AF1
	private static void RequestAbortModLoad(Action callback = null)
	{
		CustomMapLoader.abortModLoadCallback = callback;
		CustomMapLoader.shouldAbortSceneLoad = true;
		CustomMapLoader.shouldUnloadMod = true;
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x0004B905 File Offset: 0x00049B05
	private static IEnumerator AbortSceneLoad(int sceneIndex)
	{
		if (sceneIndex == -1)
		{
			CustomMapLoader.shouldUnloadMod = true;
		}
		CustomMapLoader.isLoading = false;
		if (CustomMapLoader.shouldUnloadMod)
		{
			if (CustomMapLoader.sceneLoadingCoroutine != null)
			{
				CustomMapLoader.instance.StopCoroutine(CustomMapLoader.sceneLoadingCoroutine);
				CustomMapLoader.sceneLoadingCoroutine = null;
			}
			yield return CustomMapLoader.UnloadAllScenesCoroutine();
			if (CustomMapLoader.mapBundle != null)
			{
				CustomMapLoader.mapBundle.Unload(true);
			}
			CustomMapLoader.mapBundle = null;
			Action action = CustomMapLoader.abortModLoadCallback;
			if (action != null)
			{
				action();
			}
		}
		else
		{
			yield return CustomMapLoader.UnloadSceneCoroutine(sceneIndex, null);
		}
		CustomMapLoader.abortModLoadCallback = null;
		CustomMapLoader.shouldAbortSceneLoad = false;
		CustomMapLoader.shouldUnloadMod = false;
		yield break;
	}

	// Token: 0x0600288B RID: 10379 RVA: 0x001118D0 File Offset: 0x0010FAD0
	private static int GetSceneIndex(string sceneName)
	{
		int result = -1;
		if (CustomMapLoader.assetBundleSceneFilePaths.Length == 1)
		{
			return 0;
		}
		for (int i = 0; i < CustomMapLoader.assetBundleSceneFilePaths.Length; i++)
		{
			string sceneNameFromFilePath = CustomMapLoader.GetSceneNameFromFilePath(CustomMapLoader.assetBundleSceneFilePaths[i]);
			if (sceneNameFromFilePath != null && sceneNameFromFilePath.Equals(sceneName))
			{
				result = i;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x0004B914 File Offset: 0x00049B14
	private static IEnumerator LoadSceneFromAssetBundle(int sceneIndex, bool useProgressCallback, Action<bool, bool, string> OnLoadComplete)
	{
		LoadSceneParameters parameters = new LoadSceneParameters
		{
			loadSceneMode = LoadSceneMode.Additive,
			localPhysicsMode = LocalPhysicsMode.None
		};
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			OnLoadComplete(false, true, "");
			yield break;
		}
		CustomMapLoader.runningAsyncLoad = true;
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
			if (action != null)
			{
				action(MapLoadStatus.Loading, 30, "LOADING MAP SCENE");
			}
		}
		CustomMapLoader.attemptedSceneToLoad = CustomMapLoader.assetBundleSceneFilePaths[sceneIndex];
		string sceneName = CustomMapLoader.GetSceneNameFromFilePath(CustomMapLoader.attemptedSceneToLoad);
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(CustomMapLoader.attemptedSceneToLoad, parameters);
		yield return asyncOperation;
		CustomMapLoader.runningAsyncLoad = false;
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			OnLoadComplete(false, true, "");
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action2 = CustomMapLoader.modLoadProgressCallback;
			if (action2 != null)
			{
				action2(MapLoadStatus.Loading, 50, "SANITIZING MAP");
			}
		}
		GameObject[] rootGameObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();
		List<MapDescriptor> list = new List<MapDescriptor>();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			list.AddRange(rootGameObjects[i].GetComponentsInChildren<MapDescriptor>());
		}
		MapDescriptor mapDescriptor = null;
		bool flag = false;
		foreach (MapDescriptor mapDescriptor2 in list)
		{
			if (!mapDescriptor2.IsNull())
			{
				if (!mapDescriptor.IsNull())
				{
					flag = true;
					break;
				}
				mapDescriptor = mapDescriptor2;
			}
		}
		if (flag)
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			if (useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action3 = CustomMapLoader.modLoadProgressCallback;
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
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			if (useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action4 = CustomMapLoader.modLoadProgressCallback;
				if (action4 != null)
				{
					action4(MapLoadStatus.Error, 0, "MAP SCENE DOES NOT CONTAIN A MAP DESCRIPTOR");
				}
			}
			OnLoadComplete(false, false, "");
			yield break;
		}
		GameObject gameObject = mapDescriptor.gameObject;
		if (!CustomMapLoader.SanitizeObject(gameObject, gameObject))
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			if (useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action5 = CustomMapLoader.modLoadProgressCallback;
				if (action5 != null)
				{
					action5(MapLoadStatus.Error, 0, "MAP DESCRIPTOR HAS UNAPPROVED COMPONENTS ON IT");
				}
			}
			OnLoadComplete(false, false, "");
			yield break;
		}
		CustomMapLoader.totalObjectsInLoadingScene = 0;
		for (int j = 0; j < rootGameObjects.Length; j++)
		{
			CustomMapLoader.SanitizeObjectRecursive(rootGameObjects[j], gameObject);
		}
		CustomMapLoader.CheckVirtualStumpOverlap(sceneName);
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action6 = CustomMapLoader.modLoadProgressCallback;
			if (action6 != null)
			{
				action6(MapLoadStatus.Loading, 70, "MAP SCENE LOADED");
			}
		}
		CustomMapLoader.leafGliderIndex = 0;
		yield return CustomMapLoader.ProcessAndInstantiateMap(gameObject, useProgressCallback);
		yield return null;
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield return CustomMapLoader.AbortSceneLoad(sceneIndex);
			OnLoadComplete(false, true, "");
			if (CustomMapLoader.cachedExceptionMessage.Length > 0 && useProgressCallback)
			{
				Action<MapLoadStatus, int, string> action7 = CustomMapLoader.modLoadProgressCallback;
				if (action7 != null)
				{
					action7(MapLoadStatus.Error, 0, CustomMapLoader.cachedExceptionMessage);
				}
			}
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action8 = CustomMapLoader.modLoadProgressCallback;
			if (action8 != null)
			{
				action8(MapLoadStatus.Loading, 99, "FINALIZING MAP");
			}
		}
		CustomMapLoader.loadedSceneFilePaths.AddIfNew(CustomMapLoader.attemptedSceneToLoad);
		CustomMapLoader.loadedSceneNames.AddIfNew(sceneName);
		CustomMapLoader.loadedSceneIndexes.AddIfNew(sceneIndex);
		OnLoadComplete(true, false, sceneName);
		yield break;
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x00111920 File Offset: 0x0010FB20
	public static void CloseDoorAndUnloadMod(Action unloadFinishedCallback = null)
	{
		if (!CustomMapLoader.IsModLoaded(0L) && !CustomMapLoader.isLoading)
		{
			return;
		}
		CustomMapLoader.unloadModCallback = unloadFinishedCallback;
		if (CustomMapLoader.isLoading)
		{
			CustomMapLoader.RequestAbortModLoad(null);
			return;
		}
		if (CustomMapLoader.instance.accessDoor != null)
		{
			CustomMapLoader.instance.accessDoor.CloseDoor();
		}
		CustomMapLoader.shouldUnloadMod = true;
		if (CustomMapLoader.mapBundle != null)
		{
			CustomMapLoader.mapBundle.Unload(true);
		}
		CustomMapLoader.mapBundle = null;
		CustomMapTelemetry.EndMapTracking();
		CMSSerializer.ResetSyncedMapObjects();
		CustomMapLoader.instance.StartCoroutine(CustomMapLoader.UnloadAllScenesCoroutine());
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x0004B931 File Offset: 0x00049B31
	private static IEnumerator CloseDoorAndUnloadModCoroutine()
	{
		if (!CustomMapLoader.IsModLoaded(0L) || CustomMapLoader.IsLoading)
		{
			yield break;
		}
		if (CustomMapLoader.instance.accessDoor != null)
		{
			CustomMapLoader.instance.accessDoor.CloseDoor();
		}
		CustomMapLoader.shouldUnloadMod = true;
		if (CustomMapLoader.mapBundle != null)
		{
			CustomMapLoader.mapBundle.Unload(true);
		}
		CustomMapLoader.mapBundle = null;
		CustomMapTelemetry.EndMapTracking();
		CMSSerializer.ResetSyncedMapObjects();
		yield return CustomMapLoader.UnloadAllScenesCoroutine();
		yield break;
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x0004B939 File Offset: 0x00049B39
	private static IEnumerator UnloadAllScenesCoroutine()
	{
		CustomMapLoader.isLoading = false;
		CustomMapLoader.isUnloading = true;
		ZoneShaderSettings.ActivateDefaultSettings();
		CustomMapLoader.RemoveCustomMapATM();
		int num;
		for (int sceneIndex = 0; sceneIndex < CustomMapLoader.assetBundleSceneFilePaths.Length; sceneIndex = num + 1)
		{
			yield return CustomMapLoader.UnloadSceneCoroutine(sceneIndex, null);
			num = sceneIndex;
		}
		CustomMapLoader.loadedMapDescriptor = null;
		CustomMapLoader.loadedSceneFilePaths.Clear();
		CustomMapLoader.loadedSceneNames.Clear();
		CustomMapLoader.loadedSceneIndexes.Clear();
		for (int i = 0; i < CustomMapLoader.instance.leafGliders.Length; i++)
		{
			CustomMapLoader.instance.leafGliders[i].CustomMapUnload();
			CustomMapLoader.instance.leafGliders[i].enabled = false;
			CustomMapLoader.instance.leafGliders[CustomMapLoader.leafGliderIndex].transform.GetChild(0).gameObject.SetActive(false);
		}
		GorillaNetworkJoinTrigger.EnableTriggerJoins();
		LightmapSettings.lightmaps = CustomMapLoader.lightmaps;
		CustomMapLoader.UnloadLightmaps();
		Resources.UnloadUnusedAssets();
		CustomMapLoader.isUnloading = false;
		if (CustomMapLoader.shouldUnloadMod)
		{
			yield return CustomMapLoader.ResetLightmaps();
			CustomMapLoader.assetBundleSceneFilePaths = new string[]
			{
				""
			};
			CustomMapLoader.loadedMapModId = 0L;
			CustomMapLoader.initialSceneIndex = 0;
			CustomMapLoader.initialSceneName = "";
			Action action = CustomMapLoader.unloadModCallback;
			if (action != null)
			{
				action();
			}
			CustomMapLoader.unloadModCallback = null;
			CustomMapLoader.shouldUnloadMod = false;
		}
		yield break;
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x0004B941 File Offset: 0x00049B41
	private static IEnumerator UnloadSceneCoroutine(int sceneIndex, Action OnUnloadComplete = null)
	{
		if (!CustomMapLoader.hasInstance)
		{
			yield break;
		}
		if (sceneIndex < 0 || sceneIndex >= CustomMapLoader.assetBundleSceneFilePaths.Length)
		{
			Debug.LogError(string.Format("[CustomMapLoader::UnloadSceneCoroutine] SceneIndex of {0} is invalid! ", sceneIndex) + string.Format("The currently loaded AssetBundle contains {0} scenes.", CustomMapLoader.assetBundleSceneFilePaths.Length));
			yield break;
		}
		while (CustomMapLoader.runningAsyncLoad)
		{
			yield return null;
		}
		UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects;
		string scenePathWithExtension = CustomMapLoader.assetBundleSceneFilePaths[sceneIndex];
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
		Scene sceneByName = SceneManager.GetSceneByName(text);
		if (sceneByName.IsValid())
		{
			if (CustomMapLoader.customMapATM.IsNotNull() && CustomMapLoader.customMapATM.gameObject.scene.Equals(sceneByName))
			{
				CustomMapLoader.RemoveCustomMapATM();
			}
			CustomMapLoader.RemoveUnloadingHoverboardAreas(sceneByName);
			AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scenePathWithExtension, options);
			yield return asyncOperation;
			CustomMapLoader.loadedSceneFilePaths.Remove(scenePathWithExtension);
			CustomMapLoader.loadedSceneNames.Remove(sceneName);
			CustomMapLoader.loadedSceneIndexes.Remove(sceneIndex);
			Action<string> action = CustomMapLoader.sceneUnloadedCallback;
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

	// Token: 0x06002891 RID: 10385 RVA: 0x00030607 File Offset: 0x0002E807
	private static void RemoveUnloadingHoverboardAreas(Scene unloadingScene)
	{
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x0004B957 File Offset: 0x00049B57
	private static void RemoveCustomMapATM()
	{
		if (ATM_Manager.instance.IsNotNull())
		{
			ATM_Manager.instance.RemoveATM(CustomMapLoader.customMapATM);
			ATM_Manager.instance.ResetTemporaryCreatorCode();
			CustomMapLoader.customMapATM = null;
		}
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x0004B98A File Offset: 0x00049B8A
	private static IEnumerator ResetLightmaps()
	{
		CustomMapLoader.instance.dayNightManager.RequestRepopulateLightmaps();
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

	// Token: 0x06002894 RID: 10388 RVA: 0x001119B8 File Offset: 0x0010FBB8
	private static void LoadLightmaps(Texture2D[] colorMaps, Texture2D[] dirMaps)
	{
		if (colorMaps.Length == 0)
		{
			return;
		}
		CustomMapLoader.UnloadLightmaps();
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

	// Token: 0x06002895 RID: 10389 RVA: 0x00111A38 File Offset: 0x0010FC38
	private static void UnloadLightmaps()
	{
		foreach (LightmapData lightmapData in LightmapSettings.lightmaps)
		{
			if (lightmapData.lightmapColor != null && !CustomMapLoader.lightmapsToKeep.Contains(lightmapData.lightmapColor))
			{
				Resources.UnloadAsset(lightmapData.lightmapColor);
			}
			if (lightmapData.lightmapDir != null && !CustomMapLoader.lightmapsToKeep.Contains(lightmapData.lightmapDir))
			{
				Resources.UnloadAsset(lightmapData.lightmapDir);
			}
		}
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x00111AB4 File Offset: 0x0010FCB4
	private static bool SanitizeObject(GameObject gameObject, GameObject mapRoot)
	{
		if (gameObject == null)
		{
			return false;
		}
		if (!CustomMapLoader.APPROVED_LAYERS.Contains(gameObject.layer))
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
			foreach (Type right in CustomMapLoader.componentAllowlist)
			{
				if (component.GetType() == typeof(Camera))
				{
					Camera camera = (Camera)component;
					if (camera.IsNotNull() && camera.targetTexture.IsNull())
					{
						break;
					}
				}
				if (component.GetType() == right)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				foreach (string value in CustomMapLoader.componentTypeStringAllowList)
				{
					if (component.GetType().ToString().Contains(value))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				UnityEngine.Object.DestroyImmediate(gameObject, true);
				return false;
			}
		}
		if (gameObject.transform.parent.IsNull() && gameObject.transform != mapRoot.transform)
		{
			gameObject.transform.SetParent(mapRoot.transform);
		}
		return true;
	}

	// Token: 0x06002897 RID: 10391 RVA: 0x00111C40 File Offset: 0x0010FE40
	private static void SanitizeObjectRecursive(GameObject rootObject, GameObject mapRoot)
	{
		if (!CustomMapLoader.SanitizeObject(rootObject, mapRoot))
		{
			return;
		}
		CustomMapLoader.totalObjectsInLoadingScene++;
		for (int i = 0; i < rootObject.transform.childCount; i++)
		{
			GameObject gameObject = rootObject.transform.GetChild(i).gameObject;
			if (gameObject.IsNotNull())
			{
				CustomMapLoader.SanitizeObjectRecursive(gameObject, mapRoot);
			}
		}
	}

	// Token: 0x06002898 RID: 10392 RVA: 0x0004B992 File Offset: 0x00049B92
	private static IEnumerator ProcessAndInstantiateMap(GameObject map, bool useProgressCallback)
	{
		if (map.IsNull() || !CustomMapLoader.hasInstance)
		{
			yield break;
		}
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
			if (action != null)
			{
				action(MapLoadStatus.Loading, 73, "PROCESSING ROOT MAP OBJECT");
			}
		}
		CustomMapLoader.loadedMapDescriptor = map.GetComponent<MapDescriptor>();
		if (CustomMapLoader.loadedMapDescriptor.IsNull())
		{
			yield break;
		}
		CustomMapLoader.objectsProcessedForLoadingScene = 0;
		CustomMapLoader.objectsProcessedThisFrame = 0;
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action2 = CustomMapLoader.modLoadProgressCallback;
			if (action2 != null)
			{
				action2(MapLoadStatus.Loading, 75, "PROCESSING CHILD OBJECTS");
			}
		}
		CustomMapLoader.initializePhaseTwoComponents.Clear();
		yield return CustomMapLoader.ProcessChildObjects(map, useProgressCallback);
		if (CustomMapLoader.shouldAbortSceneLoad)
		{
			yield break;
		}
		CustomMapLoader.InitializeComponentsPhaseTwo();
		CustomMapLoader.placeholderReplacements.Clear();
		if (useProgressCallback)
		{
			Action<MapLoadStatus, int, string> action3 = CustomMapLoader.modLoadProgressCallback;
			if (action3 != null)
			{
				action3(MapLoadStatus.Loading, 95, "PROCESSING COMPLETE");
			}
		}
		if (CustomMapLoader.loadedMapDescriptor.IsInitialScene)
		{
			VirtualStumpReturnWatch.SetWatchProperties(CustomMapLoader.loadedMapDescriptor.GetReturnToVStumpWatchProps());
		}
		yield break;
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x0004B9A8 File Offset: 0x00049BA8
	private static IEnumerator ProcessChildObjects(GameObject parent, bool useProgressCallback)
	{
		if (parent == null || CustomMapLoader.placeholderReplacements.Contains(parent))
		{
			yield break;
		}
		int num3;
		for (int i = 0; i < parent.transform.childCount; i = num3 + 1)
		{
			Transform child = parent.transform.GetChild(i);
			if (!(child == null))
			{
				GameObject gameObject = child.gameObject;
				if (!(gameObject == null) && !CustomMapLoader.placeholderReplacements.Contains(gameObject))
				{
					try
					{
						CustomMapLoader.SetupCollisions(gameObject);
						CustomMapLoader.ReplaceDataOnlyScripts(gameObject);
						CustomMapLoader.ReplacePlaceholders(gameObject);
						CustomMapLoader.InitializeComponentsPhaseOne(gameObject);
					}
					catch (Exception ex)
					{
						CustomMapLoader.shouldAbortSceneLoad = true;
						CustomMapLoader.cachedExceptionMessage = ex.Message;
						yield break;
					}
					if (gameObject.transform.childCount > 0)
					{
						yield return CustomMapLoader.ProcessChildObjects(gameObject, useProgressCallback);
						if (CustomMapLoader.shouldAbortSceneLoad)
						{
							yield break;
						}
					}
					if (CustomMapLoader.shouldAbortSceneLoad)
					{
						yield break;
					}
					CustomMapLoader.objectsProcessedForLoadingScene++;
					CustomMapLoader.objectsProcessedThisFrame++;
					if (CustomMapLoader.objectsProcessedThisFrame >= CustomMapLoader.numObjectsToProcessPerFrame)
					{
						CustomMapLoader.objectsProcessedThisFrame = 0;
						if (useProgressCallback)
						{
							float num = (float)CustomMapLoader.objectsProcessedForLoadingScene / (float)CustomMapLoader.totalObjectsInLoadingScene;
							int num2 = Mathf.FloorToInt(20f * num);
							Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
							if (action != null)
							{
								action(MapLoadStatus.Loading, 75 + num2, "PROCESSING CHILD OBJECTS");
							}
						}
						yield return null;
					}
				}
			}
			num3 = i;
		}
		yield break;
	}

	// Token: 0x0600289A RID: 10394 RVA: 0x00111C9C File Offset: 0x0010FE9C
	private static void CheckVirtualStumpOverlap(string sceneName)
	{
		Vector3 vector = new Vector3(5.15f, 0.72f, 5.15f);
		Vector3 b = new Vector3(0f, 0.73f, 0f);
		float radius = vector.x * 0.5f + 2f;
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		gameObject.transform.position = CustomMapLoader.instance.virtualStumpMesh.transform.position + b;
		gameObject.transform.localScale = vector;
		Collider[] array = Physics.OverlapSphere(gameObject.transform.position, radius);
		if (array == null || array.Length == 0)
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
			return;
		}
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
		meshCollider.convex = true;
		foreach (Collider collider in array)
		{
			Vector3 vector2;
			float num;
			if (!(collider == null) && !(collider.gameObject == gameObject) && !(collider.gameObject.scene.name != sceneName) && Physics.ComputePenetration(meshCollider, gameObject.transform.position, gameObject.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out vector2, out num) && !collider.isTrigger)
			{
				GTDev.Log<string>("[CustomMapLoader::CheckVirtualStumpOverlap] Gameobject " + collider.name + " has a collider overlapping with the virtual stump. Collider will be removed", null);
				UnityEngine.Object.DestroyImmediate(collider);
			}
		}
		UnityEngine.Object.DestroyImmediate(gameObject);
	}

	// Token: 0x0600289B RID: 10395 RVA: 0x00111E24 File Offset: 0x00110024
	private static void SetupCollisions(GameObject gameObject)
	{
		if (gameObject == null || CustomMapLoader.placeholderReplacements.Contains(gameObject))
		{
			return;
		}
		Collider[] components = gameObject.GetComponents<Collider>();
		if (components == null)
		{
			return;
		}
		bool flag = true;
		bool flag2 = false;
		foreach (Collider collider in components)
		{
			if (!(collider == null))
			{
				if (collider.isTrigger)
				{
					flag2 = true;
					if (gameObject.layer != UnityLayer.GorillaInteractable.ToLayerIndex())
					{
						gameObject.layer = UnityLayer.GorillaTrigger.ToLayerIndex();
						break;
					}
				}
				else
				{
					flag = false;
					if (!flag2 && gameObject.layer == UnityLayer.Default.ToLayerIndex())
					{
						gameObject.layer = UnityLayer.GorillaObject.ToLayerIndex();
					}
				}
			}
		}
		if (!flag)
		{
			SurfaceOverrideSettings component = gameObject.GetComponent<SurfaceOverrideSettings>();
			GorillaSurfaceOverride gorillaSurfaceOverride = gameObject.AddComponent<GorillaSurfaceOverride>();
			if (component == null)
			{
				gorillaSurfaceOverride.overrideIndex = 0;
				return;
			}
			gorillaSurfaceOverride.overrideIndex = (int)component.soundOverride;
			gorillaSurfaceOverride.extraVelMultiplier = component.extraVelMultiplier;
			gorillaSurfaceOverride.extraVelMaxMultiplier = component.extraVelMaxMultiplier;
			gorillaSurfaceOverride.slidePercentageOverride = component.slidePercentage;
			gorillaSurfaceOverride.disablePushBackEffect = component.disablePushBackEffect;
			UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x0600289C RID: 10396 RVA: 0x00111F38 File Offset: 0x00110138
	private static void ReplaceDataOnlyScripts(GameObject gameObject)
	{
		MapBoundarySettings component = gameObject.GetComponent<MapBoundarySettings>();
		if (component != null)
		{
			CMSMapBoundary cmsmapBoundary = gameObject.AddComponent<CMSMapBoundary>();
			if (cmsmapBoundary != null)
			{
				cmsmapBoundary.CopyTriggerSettings(component);
			}
			UnityEngine.Object.Destroy(component);
		}
		TagZoneSettings component2 = gameObject.GetComponent<TagZoneSettings>();
		if (component2 != null)
		{
			CMSTagZone cmstagZone = gameObject.AddComponent<CMSTagZone>();
			if (cmstagZone != null)
			{
				cmstagZone.CopyTriggerSettings(component2);
			}
			UnityEngine.Object.Destroy(component2);
		}
		TeleporterSettings component3 = gameObject.GetComponent<TeleporterSettings>();
		if (component3 != null)
		{
			CMSTeleporter cmsteleporter = gameObject.AddComponent<CMSTeleporter>();
			if (cmsteleporter != null)
			{
				cmsteleporter.CopyTriggerSettings(component3);
			}
			UnityEngine.Object.Destroy(component3);
		}
		ObjectActivationTriggerSettings component4 = gameObject.GetComponent<ObjectActivationTriggerSettings>();
		if (component4 != null)
		{
			CMSObjectActivationTrigger cmsobjectActivationTrigger = gameObject.AddComponent<CMSObjectActivationTrigger>();
			if (cmsobjectActivationTrigger != null)
			{
				cmsobjectActivationTrigger.CopyTriggerSettings(component4);
			}
			UnityEngine.Object.Destroy(component4);
		}
		LoadZoneSettings component5 = gameObject.GetComponent<LoadZoneSettings>();
		if (component5 != null)
		{
			CMSLoadingZone cmsloadingZone = gameObject.AddComponent<CMSLoadingZone>();
			if (cmsloadingZone != null)
			{
				cmsloadingZone.SetupLoadingZone(component5, CustomMapLoader.assetBundleSceneFilePaths);
			}
			UnityEngine.Object.Destroy(component5);
		}
		CMSZoneShaderSettings component6 = gameObject.GetComponent<CMSZoneShaderSettings>();
		if (component6.IsNotNull())
		{
			ZoneShaderSettings zoneShaderSettings = gameObject.AddComponent<ZoneShaderSettings>();
			zoneShaderSettings.CopySettings(component6, false, false);
			if (component6.isDefaultValues)
			{
				CustomMapManager.SetDefaultZoneShaderSettings(zoneShaderSettings, component6.GetProperties());
			}
			CustomMapManager.AddZoneShaderSettings(zoneShaderSettings);
			UnityEngine.Object.Destroy(component6);
		}
		ZoneShaderTriggerSettings component7 = gameObject.GetComponent<ZoneShaderTriggerSettings>();
		if (component7.IsNotNull())
		{
			gameObject.AddComponent<CMSZoneShaderSettingsTrigger>().CopySettings(component7);
			UnityEngine.Object.Destroy(component7);
		}
		HandHoldSettings component8 = gameObject.GetComponent<HandHoldSettings>();
		if (component8.IsNotNull())
		{
			gameObject.AddComponent<HandHold>().CopyProperties(component8);
			UnityEngine.Object.Destroy(component8);
		}
		CustomMapEjectButtonSettings component9 = gameObject.GetComponent<CustomMapEjectButtonSettings>();
		if (component9.IsNotNull())
		{
			CustomMapEjectButton customMapEjectButton = gameObject.AddComponent<CustomMapEjectButton>();
			customMapEjectButton.gameObject.layer = UnityLayer.GorillaInteractable.ToLayerIndex();
			customMapEjectButton.CopySettings(component9);
			UnityEngine.Object.Destroy(component9);
		}
	}

	// Token: 0x0600289D RID: 10397 RVA: 0x00112108 File Offset: 0x00110308
	private static void ReplacePlaceholders(GameObject placeholderGameObject)
	{
		if (placeholderGameObject.IsNull())
		{
			return;
		}
		GTObjectPlaceholder component = placeholderGameObject.GetComponent<GTObjectPlaceholder>();
		if (component.IsNull())
		{
			return;
		}
		switch (component.PlaceholderObject)
		{
		case GTObject.LeafGlider:
			if (CustomMapLoader.leafGliderIndex < CustomMapLoader.instance.leafGliders.Length)
			{
				CustomMapLoader.instance.leafGliders[CustomMapLoader.leafGliderIndex].enabled = true;
				CustomMapLoader.instance.leafGliders[CustomMapLoader.leafGliderIndex].CustomMapLoad(component.transform, component.maxDistanceBeforeRespawn);
				CustomMapLoader.instance.leafGliders[CustomMapLoader.leafGliderIndex].transform.GetChild(0).gameObject.SetActive(true);
				CustomMapLoader.leafGliderIndex++;
				return;
			}
			break;
		case GTObject.GliderWindVolume:
		{
			List<Collider> list = new List<Collider>(component.GetComponents<Collider>());
			if (component.useDefaultPlaceholder || list.Count == 0)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CustomMapLoader.instance.gliderWindVolume, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
				if (gameObject != null)
				{
					CustomMapLoader.placeholderReplacements.Add(gameObject);
					gameObject.transform.localScale = placeholderGameObject.transform.localScale;
					placeholderGameObject.transform.localScale = Vector3.one;
					gameObject.transform.SetParent(placeholderGameObject.transform);
					GliderWindVolume component2 = gameObject.GetComponent<GliderWindVolume>();
					if (component2 == null)
					{
						return;
					}
					component2.SetProperties(component.maxSpeed, component.maxAccel, component.SpeedVSAccelCurve, component.localWindDirection);
					return;
				}
			}
			else
			{
				placeholderGameObject.layer = UnityLayer.GorillaTrigger.ToLayerIndex();
				GliderWindVolume gliderWindVolume = placeholderGameObject.AddComponent<GliderWindVolume>();
				if (gliderWindVolume.IsNotNull())
				{
					gliderWindVolume.SetProperties(component.maxSpeed, component.maxAccel, component.SpeedVSAccelCurve, component.localWindDirection);
					return;
				}
			}
			break;
		}
		case GTObject.WaterVolume:
		{
			List<Collider> list = new List<Collider>(component.GetComponents<Collider>());
			if (component.useDefaultPlaceholder || list.Count == 0)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(CustomMapLoader.instance.waterVolumePrefab, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
				if (gameObject2 != null)
				{
					CustomMapLoader.placeholderReplacements.Add(gameObject2);
					gameObject2.layer = UnityLayer.Water.ToLayerIndex();
					gameObject2.transform.localScale = placeholderGameObject.transform.localScale;
					placeholderGameObject.transform.localScale = Vector3.one;
					gameObject2.transform.SetParent(placeholderGameObject.transform);
					MeshRenderer component3 = gameObject2.GetComponent<MeshRenderer>();
					if (component3.IsNull())
					{
						return;
					}
					if (!component.useWaterMesh)
					{
						component3.enabled = false;
						return;
					}
					component3.enabled = true;
					WaterSurfaceMaterialController component4 = gameObject2.GetComponent<WaterSurfaceMaterialController>();
					if (component4.IsNull())
					{
						return;
					}
					component4.ScrollX = component.scrollTextureX;
					component4.ScrollY = component.scrollTextureY;
					component4.Scale = component.scaleTexture;
					return;
				}
			}
			else
			{
				placeholderGameObject.layer = UnityLayer.Water.ToLayerIndex();
				WaterVolume waterVolume = placeholderGameObject.AddComponent<WaterVolume>();
				if (waterVolume.IsNotNull())
				{
					WaterParameters parameters = null;
					CMSZoneShaderSettings.EZoneLiquidType liquidType = component.liquidType;
					if (liquidType != CMSZoneShaderSettings.EZoneLiquidType.Water)
					{
						if (liquidType == CMSZoneShaderSettings.EZoneLiquidType.Lava)
						{
							parameters = CustomMapLoader.instance.defaultLavaParameters;
						}
					}
					else
					{
						parameters = CustomMapLoader.instance.defaultWaterParameters;
					}
					waterVolume.SetPropertiesFromPlaceholder(component.GetWaterVolumeProperties(), list, parameters);
					waterVolume.RefreshColliders();
					return;
				}
			}
			break;
		}
		case GTObject.ForceVolume:
		{
			List<Collider> list = new List<Collider>(component.GetComponents<Collider>());
			if (component.useDefaultPlaceholder || list.Count == 0)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(CustomMapLoader.instance.forceVolumePrefab, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
				if (gameObject3.IsNotNull())
				{
					CustomMapLoader.placeholderReplacements.Add(gameObject3);
					gameObject3.transform.localScale = placeholderGameObject.transform.localScale;
					placeholderGameObject.transform.localScale = Vector3.one;
					gameObject3.transform.SetParent(placeholderGameObject.transform);
					ForceVolume component5 = gameObject3.GetComponent<ForceVolume>();
					if (component5.IsNull())
					{
						return;
					}
					component5.SetPropertiesFromPlaceholder(component.GetForceVolumeProperties(), null, null);
					return;
				}
			}
			else
			{
				ForceVolume forceVolume = placeholderGameObject.AddComponent<ForceVolume>();
				if (forceVolume.IsNotNull())
				{
					AudioSource audioSource = placeholderGameObject.GetComponent<AudioSource>();
					if (audioSource.IsNull())
					{
						audioSource = placeholderGameObject.AddComponent<AudioSource>();
						audioSource.spatialize = true;
						audioSource.playOnAwake = false;
						audioSource.priority = 128;
						audioSource.volume = 0.522f;
						audioSource.pitch = 1f;
						audioSource.panStereo = 0f;
						audioSource.spatialBlend = 1f;
						audioSource.reverbZoneMix = 1f;
						audioSource.dopplerLevel = 1f;
						audioSource.spread = 0f;
						audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
						audioSource.minDistance = 8.2f;
						audioSource.maxDistance = 43.94f;
						audioSource.enabled = true;
					}
					audioSource.outputAudioMixerGroup = CustomMapLoader.instance.masterAudioMixer;
					for (int i = list.Count - 1; i >= 0; i--)
					{
						if (i == 0)
						{
							list[i].isTrigger = true;
						}
						else
						{
							UnityEngine.Object.Destroy(list[i]);
						}
					}
					placeholderGameObject.layer = UnityLayer.GorillaBoundary.ToLayerIndex();
					forceVolume.SetPropertiesFromPlaceholder(component.GetForceVolumeProperties(), audioSource, component.GetComponent<Collider>());
					return;
				}
				Debug.LogError("[CustomMapLoader::ReplacePlaceholders] Failed to add ForceVolume component to Placeholder!");
				return;
			}
			break;
		}
		case GTObject.ATM:
		{
			if (CustomMapLoader.customMapATM.IsNotNull())
			{
				Debug.LogError("[CustomMapLoader::ReplacePlaceholders] Map already contains an ATM, maps are only allowed 1 ATM! Removing placeholder and not instantiating...");
				return;
			}
			if (CustomMapLoader.instance.atmPrefab.IsNull())
			{
				return;
			}
			GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(CustomMapLoader.instance.atmPrefab, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
			if (gameObject4.IsNotNull())
			{
				CustomMapLoader.placeholderReplacements.Add(gameObject4);
				gameObject4.transform.SetParent(placeholderGameObject.transform);
				ATM_UI componentInChildren = gameObject4.GetComponentInChildren<ATM_UI>();
				if (componentInChildren.IsNotNull() && ATM_Manager.instance.IsNotNull())
				{
					CustomMapLoader.customMapATM = componentInChildren;
					ATM_Manager.instance.AddATM(componentInChildren);
					if (!component.defaultCreatorCode.IsNullOrEmpty())
					{
						ATM_Manager.instance.SetTemporaryCreatorCode(component.defaultCreatorCode, true, null);
						return;
					}
				}
			}
			break;
		}
		case GTObject.HoverboardArea:
			if (component.AddComponent<HoverboardAreaTrigger>().IsNotNull())
			{
				component.gameObject.layer = UnityLayer.GorillaBoundary.ToLayerIndex();
				List<Collider> list = new List<Collider>(component.GetComponents<Collider>());
				if (list.Count != 0)
				{
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (j == 0)
						{
							list[j].isTrigger = true;
						}
						else
						{
							UnityEngine.Object.Destroy(list[j]);
						}
					}
					return;
				}
				BoxCollider boxCollider = component.AddComponent<BoxCollider>();
				if (boxCollider.IsNotNull())
				{
					boxCollider.isTrigger = true;
					return;
				}
			}
			break;
		case GTObject.HoverboardDispenser:
		{
			if (CustomMapLoader.instance.hoverboardDispenserPrefab.IsNull())
			{
				Debug.LogError("[CustomMapLoader::ReplacePlaceholders] hoverboardDispenserPrefab is NULL!");
				return;
			}
			GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(CustomMapLoader.instance.hoverboardDispenserPrefab, placeholderGameObject.transform.position, placeholderGameObject.transform.rotation);
			if (gameObject5.IsNotNull())
			{
				CustomMapLoader.placeholderReplacements.Add(gameObject5);
				gameObject5.transform.SetParent(placeholderGameObject.transform);
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x0600289E RID: 10398 RVA: 0x00030607 File Offset: 0x0002E807
	private static void InitializeComponentsPhaseOne(GameObject childGameObject)
	{
	}

	// Token: 0x0600289F RID: 10399 RVA: 0x0011283C File Offset: 0x00110A3C
	private static void InitializeComponentsPhaseTwo()
	{
		for (int i = 0; i < CustomMapLoader.initializePhaseTwoComponents.Count; i++)
		{
		}
		CustomMapLoader.initializePhaseTwoComponents.Clear();
	}

	// Token: 0x060028A0 RID: 10400 RVA: 0x0004B9BE File Offset: 0x00049BBE
	public static bool OpenDoorToMap()
	{
		if (!CustomMapLoader.hasInstance)
		{
			return false;
		}
		if (CustomMapLoader.instance.accessDoor != null)
		{
			CustomMapLoader.instance.accessDoor.OpenDoor();
			return true;
		}
		return false;
	}

	// Token: 0x060028A1 RID: 10401 RVA: 0x00112868 File Offset: 0x00110A68
	private static void OnAssetBundleLoaded(bool loadSucceeded, bool loadAborted)
	{
		if (loadAborted)
		{
			return;
		}
		if (loadSucceeded)
		{
			CustomMapLoader.loadedMapModId = CustomMapLoader.attemptedLoadID;
			if (CustomMapLoader.initialSceneName != string.Empty)
			{
				CustomMapLoader.initialSceneIndex = CustomMapLoader.GetSceneIndex(CustomMapLoader.initialSceneName);
			}
			if (CustomMapLoader.initialSceneIndex == -1 && CustomMapLoader.mapBundle != null)
			{
				CustomMapLoader.mapBundle.Unload(true);
				CustomMapLoader.mapBundle = null;
				Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
				if (action != null)
				{
					action(MapLoadStatus.Error, 0, "ASSET BUNDLE CONTAINS MULTIPLE SCENES, BUT NONE ARE SET AS INITIAL SCENE.");
				}
				CustomMapLoader.OnLoadComplete(false, true, "");
			}
			CustomMapLoader.instance.StartCoroutine(CustomMapLoader.LoadSceneFromAssetBundle(CustomMapLoader.initialSceneIndex, true, new Action<bool, bool, string>(CustomMapLoader.OnLoadComplete)));
		}
	}

	// Token: 0x060028A2 RID: 10402 RVA: 0x0004B9F1 File Offset: 0x00049BF1
	private static void OnIncrementalLoadComplete(bool loadSucceeded, bool loadAborted, string loadedScene)
	{
		if (loadSucceeded)
		{
			CustomMapLoader.sceneLoadedCallback(loadedScene);
			return;
		}
		CustomMapLoader.instance.StopAllCoroutines();
		CustomMapLoader.isLoading = false;
	}

	// Token: 0x060028A3 RID: 10403 RVA: 0x00112918 File Offset: 0x00110B18
	private static void OnLoadComplete(bool loadSucceeded, bool loadAborted, string loadedScene)
	{
		CustomMapLoader.isLoading = false;
		GorillaNetworkJoinTrigger.EnableTriggerJoins();
		if (loadSucceeded)
		{
			Action<MapLoadStatus, int, string> action = CustomMapLoader.modLoadProgressCallback;
			if (action != null)
			{
				action(MapLoadStatus.Loading, 100, "Load Complete");
			}
			if (CustomMapLoader.instance.networkTrigger != null)
			{
				CustomMapLoader.instance.networkTrigger.SetActive(true);
			}
		}
		else
		{
			CustomMapLoader.loadedMapDescriptor = null;
		}
		if (!loadAborted)
		{
			Action<bool> action2 = CustomMapLoader.modLoadedCallback;
			if (action2 != null)
			{
				action2(loadSucceeded);
			}
			if (loadSucceeded)
			{
				Action<string> action3 = CustomMapLoader.sceneLoadedCallback;
				if (action3 == null)
				{
					return;
				}
				action3(loadedScene);
			}
		}
	}

	// Token: 0x060028A4 RID: 10404 RVA: 0x0004BA14 File Offset: 0x00049C14
	private static string GetSceneNameFromFilePath(string filePath)
	{
		string[] array = filePath.Split("/", StringSplitOptions.None);
		return array[array.Length - 1].Split(".", StringSplitOptions.None)[0];
	}

	// Token: 0x060028A5 RID: 10405 RVA: 0x001129A0 File Offset: 0x00110BA0
	public static MapPackageInfo GetPackageInfo(string packageInfoFilePath)
	{
		MapPackageInfo result;
		using (StreamReader streamReader = new StreamReader(File.OpenRead(packageInfoFilePath), Encoding.Default))
		{
			result = JsonConvert.DeserializeObject<MapPackageInfo>(streamReader.ReadToEnd());
		}
		return result;
	}

	// Token: 0x060028A6 RID: 10406 RVA: 0x0004BA35 File Offset: 0x00049C35
	public static Transform GetCustomMapsDefaultSpawnLocation()
	{
		if (CustomMapLoader.hasInstance)
		{
			return CustomMapLoader.instance.CustomMapsDefaultSpawnLocation;
		}
		return null;
	}

	// Token: 0x060028A7 RID: 10407 RVA: 0x0004BA4C File Offset: 0x00049C4C
	public static bool IsModLoaded(long mapModId = 0L)
	{
		if (mapModId != 0L)
		{
			return !CustomMapLoader.IsLoading && CustomMapLoader.LoadedMapModId == mapModId;
		}
		return !CustomMapLoader.IsLoading && CustomMapLoader.LoadedMapModId != 0L;
	}

	// Token: 0x04002DD7 RID: 11735
	[OnEnterPlay_SetNull]
	private static volatile CustomMapLoader instance;

	// Token: 0x04002DD8 RID: 11736
	[OnEnterPlay_Set(false)]
	private static bool hasInstance;

	// Token: 0x04002DD9 RID: 11737
	public Transform CustomMapsDefaultSpawnLocation;

	// Token: 0x04002DDA RID: 11738
	public CustomMapAccessDoor accessDoor;

	// Token: 0x04002DDB RID: 11739
	public GameObject networkTrigger;

	// Token: 0x04002DDC RID: 11740
	[SerializeField]
	private BetterDayNightManager dayNightManager;

	// Token: 0x04002DDD RID: 11741
	[SerializeField]
	private GameObject placeholderParent;

	// Token: 0x04002DDE RID: 11742
	[SerializeField]
	private GliderHoldable[] leafGliders;

	// Token: 0x04002DDF RID: 11743
	[SerializeField]
	private GameObject leafGlider;

	// Token: 0x04002DE0 RID: 11744
	[SerializeField]
	private GameObject gliderWindVolume;

	// Token: 0x04002DE1 RID: 11745
	[FormerlySerializedAs("waterVolume")]
	[SerializeField]
	private GameObject waterVolumePrefab;

	// Token: 0x04002DE2 RID: 11746
	[SerializeField]
	private WaterParameters defaultWaterParameters;

	// Token: 0x04002DE3 RID: 11747
	[SerializeField]
	private WaterParameters defaultLavaParameters;

	// Token: 0x04002DE4 RID: 11748
	[FormerlySerializedAs("forceVolume")]
	[SerializeField]
	private GameObject forceVolumePrefab;

	// Token: 0x04002DE5 RID: 11749
	[SerializeField]
	private GameObject atmPrefab;

	// Token: 0x04002DE6 RID: 11750
	[SerializeField]
	private GameObject hoverboardDispenserPrefab;

	// Token: 0x04002DE7 RID: 11751
	[SerializeField]
	private GameObject zoneShaderSettingsTrigger;

	// Token: 0x04002DE8 RID: 11752
	[SerializeField]
	private AudioMixerGroup masterAudioMixer;

	// Token: 0x04002DE9 RID: 11753
	[SerializeField]
	private ZoneShaderSettings customMapZoneShaderSettings;

	// Token: 0x04002DEA RID: 11754
	[SerializeField]
	private GameObject virtualStumpMesh;

	// Token: 0x04002DEB RID: 11755
	private static readonly int numObjectsToProcessPerFrame = 5;

	// Token: 0x04002DEC RID: 11756
	private static readonly List<int> APPROVED_LAYERS = new List<int>
	{
		0,
		1,
		2,
		4,
		5,
		9,
		11,
		18,
		22,
		27,
		30
	};

	// Token: 0x04002DED RID: 11757
	private static bool isLoading;

	// Token: 0x04002DEE RID: 11758
	private static bool isUnloading;

	// Token: 0x04002DEF RID: 11759
	private static bool runningAsyncLoad = false;

	// Token: 0x04002DF0 RID: 11760
	private static long attemptedLoadID = 0L;

	// Token: 0x04002DF1 RID: 11761
	private static string attemptedSceneToLoad;

	// Token: 0x04002DF2 RID: 11762
	private static bool shouldUnloadMod = false;

	// Token: 0x04002DF3 RID: 11763
	private static AssetBundle mapBundle;

	// Token: 0x04002DF4 RID: 11764
	private static string initialSceneName = string.Empty;

	// Token: 0x04002DF5 RID: 11765
	private static int initialSceneIndex = 0;

	// Token: 0x04002DF6 RID: 11766
	private static string loadedMapLevelName;

	// Token: 0x04002DF7 RID: 11767
	private static long loadedMapModId;

	// Token: 0x04002DF8 RID: 11768
	private static MapDescriptor loadedMapDescriptor;

	// Token: 0x04002DF9 RID: 11769
	private static Action<MapLoadStatus, int, string> modLoadProgressCallback;

	// Token: 0x04002DFA RID: 11770
	private static Action<bool> modLoadedCallback;

	// Token: 0x04002DFB RID: 11771
	private static Coroutine sceneLoadingCoroutine;

	// Token: 0x04002DFC RID: 11772
	private static Action<string> sceneLoadedCallback;

	// Token: 0x04002DFD RID: 11773
	private static Action<string> sceneUnloadedCallback;

	// Token: 0x04002DFE RID: 11774
	private static List<CustomMapLoader.LoadZoneRequest> queuedLoadZoneRequests = new List<CustomMapLoader.LoadZoneRequest>();

	// Token: 0x04002DFF RID: 11775
	private static string[] assetBundleSceneFilePaths;

	// Token: 0x04002E00 RID: 11776
	private static List<string> loadedSceneFilePaths = new List<string>();

	// Token: 0x04002E01 RID: 11777
	private static List<string> loadedSceneNames = new List<string>();

	// Token: 0x04002E02 RID: 11778
	private static List<int> loadedSceneIndexes = new List<int>();

	// Token: 0x04002E03 RID: 11779
	private static int leafGliderIndex;

	// Token: 0x04002E04 RID: 11780
	private static int totalObjectsInLoadingScene = 0;

	// Token: 0x04002E05 RID: 11781
	private static int objectsProcessedForLoadingScene = 0;

	// Token: 0x04002E06 RID: 11782
	private static int objectsProcessedThisFrame = 0;

	// Token: 0x04002E07 RID: 11783
	private static List<Component> initializePhaseTwoComponents = new List<Component>();

	// Token: 0x04002E08 RID: 11784
	private static bool shouldAbortSceneLoad = false;

	// Token: 0x04002E09 RID: 11785
	private static Action abortModLoadCallback;

	// Token: 0x04002E0A RID: 11786
	private static Action unloadModCallback;

	// Token: 0x04002E0B RID: 11787
	private static string cachedExceptionMessage = "";

	// Token: 0x04002E0C RID: 11788
	private static LightmapData[] lightmaps;

	// Token: 0x04002E0D RID: 11789
	private static List<Texture2D> lightmapsToKeep = new List<Texture2D>();

	// Token: 0x04002E0E RID: 11790
	private static List<GameObject> placeholderReplacements = new List<GameObject>();

	// Token: 0x04002E0F RID: 11791
	private static ATM_UI customMapATM = null;

	// Token: 0x04002E10 RID: 11792
	private string dontDestroyOnLoadSceneName = "";

	// Token: 0x04002E11 RID: 11793
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
		typeof(ReflectionProbe),
		typeof(AudioSource),
		typeof(Animator),
		typeof(SkinnedMeshRenderer),
		typeof(TextMesh),
		typeof(ParticleSystem),
		typeof(ParticleSystemRenderer),
		typeof(RectTransform),
		typeof(SpriteRenderer),
		typeof(BillboardRenderer),
		typeof(Canvas),
		typeof(CanvasRenderer),
		typeof(CanvasScaler),
		typeof(GraphicRaycaster),
		typeof(Rigidbody),
		typeof(TrailRenderer),
		typeof(LineRenderer),
		typeof(LensFlareComponentSRP),
		typeof(Camera),
		typeof(UniversalAdditionalCameraData),
		typeof(MapDescriptor),
		typeof(AccessDoorPlaceholder),
		typeof(MapOrientationPoint),
		typeof(SurfaceOverrideSettings),
		typeof(TeleporterSettings),
		typeof(TagZoneSettings),
		typeof(MapBoundarySettings),
		typeof(ObjectActivationTriggerSettings),
		typeof(LoadZoneSettings),
		typeof(GTObjectPlaceholder),
		typeof(CMSZoneShaderSettings),
		typeof(ZoneShaderTriggerSettings),
		typeof(MultiPartFire),
		typeof(HandHoldSettings),
		typeof(CustomMapEjectButtonSettings),
		typeof(ProBuilderMesh),
		typeof(TMP_Text),
		typeof(TextMeshPro),
		typeof(TextMeshProUGUI),
		typeof(UniversalAdditionalLightData),
		typeof(BakerySkyLight),
		typeof(BakeryDirectLight),
		typeof(BakeryPointLight),
		typeof(ftLightmapsStorage)
	};

	// Token: 0x04002E12 RID: 11794
	private static readonly List<string> componentTypeStringAllowList = new List<string>
	{
		"UnityEngine.Halo"
	};

	// Token: 0x04002E13 RID: 11795
	private static Type[] badComponents = new Type[]
	{
		typeof(EventTrigger),
		typeof(UIBehaviour),
		typeof(GorillaPressableButton),
		typeof(GorillaPressableDelayButton),
		typeof(Camera),
		typeof(AudioListener),
		typeof(VideoPlayer)
	};

	// Token: 0x02000665 RID: 1637
	private struct LoadZoneRequest
	{
		// Token: 0x04002E14 RID: 11796
		public int[] sceneIndexesToLoad;

		// Token: 0x04002E15 RID: 11797
		public int[] sceneIndexesToUnload;

		// Token: 0x04002E16 RID: 11798
		public Action<string> onSceneLoadedCallback;

		// Token: 0x04002E17 RID: 11799
		public Action<string> onSceneUnloadedCallback;
	}
}
