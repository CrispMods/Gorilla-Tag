using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class PerSceneRenderData : MonoBehaviour
{
	// Token: 0x06000B0E RID: 2830 RVA: 0x00099974 File Offset: 0x00097B74
	private void RefreshRenderer()
	{
		int sceneIndex = this.sceneIndex;
		new List<Renderer>();
		foreach (Renderer renderer in UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
		{
			if (renderer.gameObject.scene.buildIndex == sceneIndex)
			{
				this.representativeRenderer = renderer;
				return;
			}
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000B0F RID: 2831 RVA: 0x000999C8 File Offset: 0x00097BC8
	public string sceneName
	{
		get
		{
			return base.gameObject.scene.name;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000B10 RID: 2832 RVA: 0x000999E8 File Offset: 0x00097BE8
	public int sceneIndex
	{
		get
		{
			return base.gameObject.scene.buildIndex;
		}
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x00099A08 File Offset: 0x00097C08
	private void Awake()
	{
		for (int i = 0; i < this.mRendererIndex; i++)
		{
			this.mRenderers[i] = this.gO[i].GetComponent<MeshRenderer>();
		}
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x00037CB8 File Offset: 0x00035EB8
	private void OnEnable()
	{
		BetterDayNightManager.Register(this);
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x00037CC0 File Offset: 0x00035EC0
	private void OnDisable()
	{
		BetterDayNightManager.Unregister(this);
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x00099A3C File Offset: 0x00097C3C
	public void AddMeshToList(GameObject _gO, MeshRenderer mR)
	{
		try
		{
			if (mR.lightmapIndex != -1)
			{
				this.gO[this.mRendererIndex] = _gO;
				this.mRendererIndex++;
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x00037CC8 File Offset: 0x00035EC8
	public bool CheckShouldRepopulate()
	{
		return this.representativeRenderer.lightmapIndex != this.lastLightmapIndex;
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00037CE0 File Offset: 0x00035EE0
	public bool IsLoadingLightmaps
	{
		get
		{
			return this.resourceRequests.Count != 0;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00037CF0 File Offset: 0x00035EF0
	public int LoadingLightmapsCount
	{
		get
		{
			return this.resourceRequests.Count;
		}
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x00099A88 File Offset: 0x00097C88
	private Texture2D GetLightmap(string timeOfDay)
	{
		if (this.singleLightmap != null)
		{
			return this.singleLightmap;
		}
		Texture2D result;
		if (!this.lightmapsCache.TryGetValue(timeOfDay, out result))
		{
			ResourceRequest request;
			if (this.resourceRequests.TryGetValue(timeOfDay, out request))
			{
				return null;
			}
			request = Resources.LoadAsync<Texture2D>(Path.Combine(this.lightmapsResourcePath, timeOfDay));
			this.resourceRequests.Add(timeOfDay, request);
			request.completed += delegate(AsyncOperation ao)
			{
				if (this == null)
				{
					return;
				}
				this.lightmapsCache.Add(timeOfDay, (Texture2D)request.asset);
				this.resourceRequests.Remove(timeOfDay);
				if (BetterDayNightManager.instance != null)
				{
					BetterDayNightManager.instance.RequestRepopulateLightmaps();
				}
			};
		}
		return result;
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x00099B3C File Offset: 0x00097D3C
	public void PopulateLightmaps(string fromTimeOfDay, string toTimeOfDay, LightmapData[] lightmaps)
	{
		LightmapData lightmapData = new LightmapData();
		lightmapData.lightmapColor = this.GetLightmap(fromTimeOfDay);
		lightmapData.lightmapDir = this.GetLightmap(toTimeOfDay);
		if (lightmapData.lightmapColor != null && lightmapData.lightmapDir != null && this.representativeRenderer.lightmapIndex < lightmaps.Length)
		{
			lightmaps[this.representativeRenderer.lightmapIndex] = lightmapData;
		}
		this.lastLightmapIndex = this.representativeRenderer.lightmapIndex;
		for (int i = 0; i < this.mRendererIndex; i++)
		{
			if (i < this.mRenderers.Length && this.mRenderers[i] != null)
			{
				this.mRenderers[i].lightmapIndex = this.lastLightmapIndex;
			}
		}
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x00099BF4 File Offset: 0x00097DF4
	public void ReleaseLightmap(string oldTimeOfDay)
	{
		Texture2D assetToUnload;
		if (this.lightmapsCache.Remove(oldTimeOfDay, out assetToUnload))
		{
			Resources.UnloadAsset(assetToUnload);
		}
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x00099C18 File Offset: 0x00097E18
	private void TryGetLightmapOrAsyncLoad(string momentName, Action<Texture2D> callback)
	{
		if (this.singleLightmap != null)
		{
			callback(this.singleLightmap);
		}
		Texture2D obj;
		if (this.lightmapsCache.TryGetValue(momentName, out obj))
		{
			callback(obj);
		}
		List<Action<Texture2D>> callbacks;
		if (!this._momentName_to_callbacks.TryGetValue(momentName, out callbacks))
		{
			callbacks = new List<Action<Texture2D>>(8);
			this._momentName_to_callbacks[momentName] = callbacks;
		}
		if (!callbacks.Contains(callback))
		{
			callbacks.Add(callback);
		}
		ResourceRequest request;
		if (this.resourceRequests.TryGetValue(momentName, out request))
		{
			return;
		}
		request = Resources.LoadAsync<Texture2D>(Path.Combine(this.lightmapsResourcePath, momentName));
		this.resourceRequests.Add(momentName, request);
		request.completed += delegate(AsyncOperation ao)
		{
			if (this == null || ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			Texture2D texture2D = (Texture2D)request.asset;
			this.lightmapsCache.Add(momentName, texture2D);
			this.resourceRequests.Remove(momentName);
			foreach (Action<Texture2D> action in callbacks)
			{
				if (action != null)
				{
					action(texture2D);
				}
			}
			callbacks.Clear();
		};
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00099D2C File Offset: 0x00097F2C
	public bool IsLightmapWithNameLoaded(string lightmapName)
	{
		if (this.singleLightmap != null)
		{
			return true;
		}
		string text;
		string text2;
		this.GetFromAndToLightmapNames(out text, out text2);
		return !string.IsNullOrEmpty(lightmapName) && ((!string.IsNullOrEmpty(text) && text == lightmapName) || (!string.IsNullOrEmpty(text2) && text2 == lightmapName));
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00099D84 File Offset: 0x00097F84
	public bool IsLightmapsWithNamesLoaded(string fromLightmapName, string toLightmapName)
	{
		if (this.singleLightmap != null)
		{
			return true;
		}
		string text;
		string text2;
		this.GetFromAndToLightmapNames(out text, out text2);
		return !string.IsNullOrEmpty(fromLightmapName) && !string.IsNullOrEmpty(toLightmapName) && !string.IsNullOrEmpty(text) && text == fromLightmapName && !string.IsNullOrEmpty(text2) && text2 == toLightmapName;
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x00099DE0 File Offset: 0x00097FE0
	public void GetFromAndToLightmapNames(out string fromLightmapName, out string toLightmapName)
	{
		if (this.singleLightmap != null)
		{
			fromLightmapName = null;
			toLightmapName = null;
			return;
		}
		LightmapData[] lightmaps = LightmapSettings.lightmaps;
		if (this.representativeRenderer.lightmapIndex < 0 || this.representativeRenderer.lightmapIndex >= lightmaps.Length)
		{
			fromLightmapName = null;
			toLightmapName = null;
			return;
		}
		Texture2D lightmapColor = lightmaps[this.representativeRenderer.lightmapIndex].lightmapColor;
		Texture2D lightmapDir = lightmaps[this.representativeRenderer.lightmapIndex].lightmapDir;
		fromLightmapName = ((lightmapColor != null) ? lightmapColor.name : null);
		toLightmapName = ((lightmapDir != null) ? lightmapDir.name : null);
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x00099E7C File Offset: 0x0009807C
	public static void g_StartAllScenesPopulateLightmaps(string fromLightmapName, string toLightmapName)
	{
		PerSceneRenderData._g_allScenesPopulateLightmaps_renderDatasHashSet.Clear();
		PerSceneRenderData[] array = UnityEngine.Object.FindObjectsByType<PerSceneRenderData>(FindObjectsSortMode.None);
		PerSceneRenderData._g_allScenesPopulateLightmaps_renderDatasHashSet.UnionWith(array);
		foreach (PerSceneRenderData perSceneRenderData in array)
		{
			perSceneRenderData.StartPopulateLightmaps(fromLightmapName, toLightmapName);
			perSceneRenderData.OnPopulateToAndFromLightmapsCompleted = (Action<PerSceneRenderData>)Delegate.Combine(perSceneRenderData.OnPopulateToAndFromLightmapsCompleted, new Action<PerSceneRenderData>(PerSceneRenderData._g_AllScenesPopulateLightmaps_OnOneCompleted));
		}
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x00099EE4 File Offset: 0x000980E4
	private static void _g_AllScenesPopulateLightmaps_OnOneCompleted(PerSceneRenderData perSceneRenderData)
	{
		int count = PerSceneRenderData._g_allScenesPopulateLightmaps_renderDatasHashSet.Count;
		PerSceneRenderData._g_allScenesPopulateLightmaps_renderDatasHashSet.Remove(perSceneRenderData);
		int count2 = PerSceneRenderData._g_allScenesPopulateLightmaps_renderDatasHashSet.Count;
		if (count2 == 0 && count2 != count)
		{
			Action action = PerSceneRenderData.g_OnAllScenesPopulateLightmapsCompleted;
			if (action == null)
			{
				return;
			}
			action();
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00037CFD File Offset: 0x00035EFD
	public static int g_AllScenesPopulatingLightmapsLoadCount
	{
		get
		{
			return PerSceneRenderData._g_allScenesPopulateLightmaps_renderDatasHashSet.Count;
		}
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x00099F2C File Offset: 0x0009812C
	public void StartPopulateLightmaps(string fromMomentName, string toMomentName)
	{
		PerSceneRenderData._g_allScenesPopulateLightmaps_renderDatasHashSet.Clear();
		this._populateLightmaps_fromMomentLightmap = null;
		this._populateLightmaps_toMomentLightmap = null;
		this._populateLightmaps_fromMomentName = fromMomentName;
		this._populateLightmaps_toMomentName = toMomentName;
		this.TryGetLightmapOrAsyncLoad(fromMomentName, new Action<Texture2D>(this._PopulateLightmaps_OnLoadLightmap));
		this.TryGetLightmapOrAsyncLoad(toMomentName, new Action<Texture2D>(this._PopulateLightmaps_OnLoadLightmap));
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x00099F88 File Offset: 0x00098188
	private void _PopulateLightmaps_OnLoadLightmap(Texture2D lightmapTex)
	{
		if (this == null || ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		if (this._populateLightmaps_fromMomentName != lightmapTex.name)
		{
			this._populateLightmaps_fromMomentLightmap = lightmapTex;
		}
		if (this._populateLightmaps_toMomentName != lightmapTex.name)
		{
			this._populateLightmaps_toMomentLightmap = lightmapTex;
		}
		if (this._populateLightmaps_fromMomentLightmap != null && this._populateLightmaps_toMomentLightmap != null)
		{
			LightmapData[] lightmaps = LightmapSettings.lightmaps;
			LightmapData lightmapData = new LightmapData
			{
				lightmapColor = this._populateLightmaps_fromMomentLightmap,
				lightmapDir = this._populateLightmaps_toMomentLightmap
			};
			if (this.representativeRenderer.lightmapIndex >= 0 && this.representativeRenderer.lightmapIndex < lightmaps.Length)
			{
				lightmaps[this.representativeRenderer.lightmapIndex] = lightmapData;
			}
			LightmapSettings.lightmaps = lightmaps;
			this.lastLightmapIndex = this.representativeRenderer.lightmapIndex;
			for (int i = 0; i < this.mRendererIndex; i++)
			{
				if (i < this.mRenderers.Length && this.mRenderers[i] != null)
				{
					this.mRenderers[i].lightmapIndex = this.lastLightmapIndex;
				}
			}
			Action<PerSceneRenderData> onPopulateToAndFromLightmapsCompleted = this.OnPopulateToAndFromLightmapsCompleted;
			if (onPopulateToAndFromLightmapsCompleted == null)
			{
				return;
			}
			onPopulateToAndFromLightmapsCompleted(this);
		}
	}

	// Token: 0x04000D79 RID: 3449
	public Renderer representativeRenderer;

	// Token: 0x04000D7A RID: 3450
	public string lightmapsResourcePath;

	// Token: 0x04000D7B RID: 3451
	public Texture2D singleLightmap;

	// Token: 0x04000D7C RID: 3452
	private int lastLightmapIndex = -1;

	// Token: 0x04000D7D RID: 3453
	public GameObject[] gO = new GameObject[5000];

	// Token: 0x04000D7E RID: 3454
	public MeshRenderer[] mRenderers = new MeshRenderer[5000];

	// Token: 0x04000D7F RID: 3455
	public int mRendererIndex;

	// Token: 0x04000D80 RID: 3456
	private readonly Dictionary<string, ResourceRequest> resourceRequests = new Dictionary<string, ResourceRequest>(8);

	// Token: 0x04000D81 RID: 3457
	private readonly Dictionary<string, Texture2D> lightmapsCache = new Dictionary<string, Texture2D>(8);

	// Token: 0x04000D82 RID: 3458
	private Dictionary<string, List<Action<Texture2D>>> _momentName_to_callbacks = new Dictionary<string, List<Action<Texture2D>>>(8);

	// Token: 0x04000D83 RID: 3459
	private static readonly HashSet<PerSceneRenderData> _g_allScenesPopulateLightmaps_renderDatasHashSet = new HashSet<PerSceneRenderData>(32);

	// Token: 0x04000D84 RID: 3460
	public static Action g_OnAllScenesPopulateLightmapsCompleted;

	// Token: 0x04000D85 RID: 3461
	private string _populateLightmaps_fromMomentName;

	// Token: 0x04000D86 RID: 3462
	private string _populateLightmaps_toMomentName;

	// Token: 0x04000D87 RID: 3463
	private Texture2D _populateLightmaps_fromMomentLightmap;

	// Token: 0x04000D88 RID: 3464
	private Texture2D _populateLightmaps_toMomentLightmap;

	// Token: 0x04000D89 RID: 3465
	public Action<PerSceneRenderData> OnPopulateToAndFromLightmapsCompleted;
}
