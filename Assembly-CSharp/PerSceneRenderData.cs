using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class PerSceneRenderData : MonoBehaviour
{
	// Token: 0x06000AC2 RID: 2754 RVA: 0x0003A45C File Offset: 0x0003865C
	private void RefreshRenderer()
	{
		int sceneIndex = this.sceneIndex;
		new List<Renderer>();
		foreach (Renderer renderer in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
		{
			if (renderer.gameObject.scene.buildIndex == sceneIndex)
			{
				this.representativeRenderer = renderer;
				return;
			}
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0003A4B0 File Offset: 0x000386B0
	public string sceneName
	{
		get
		{
			return base.gameObject.scene.name;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0003A4D0 File Offset: 0x000386D0
	public int sceneIndex
	{
		get
		{
			return base.gameObject.scene.buildIndex;
		}
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0003A4F0 File Offset: 0x000386F0
	private void Awake()
	{
		for (int i = 0; i < this.mRendererIndex; i++)
		{
			this.mRenderers[i] = this.gO[i].GetComponent<MeshRenderer>();
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x0003A523 File Offset: 0x00038723
	private void OnEnable()
	{
		BetterDayNightManager.Register(this);
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x0003A52B File Offset: 0x0003872B
	private void OnDisable()
	{
		BetterDayNightManager.Unregister(this);
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x0003A534 File Offset: 0x00038734
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

	// Token: 0x06000AC9 RID: 2761 RVA: 0x0003A580 File Offset: 0x00038780
	public bool CheckShouldRepopulate()
	{
		return this.representativeRenderer.lightmapIndex != this.lastLightmapIndex;
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0003A598 File Offset: 0x00038798
	public bool IsLoadingLightmaps
	{
		get
		{
			return this.resourceRequests.Count != 0;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0003A5A8 File Offset: 0x000387A8
	public int LoadingLightmapsCount
	{
		get
		{
			return this.resourceRequests.Count;
		}
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x0003A5B8 File Offset: 0x000387B8
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

	// Token: 0x06000ACD RID: 2765 RVA: 0x0003A66C File Offset: 0x0003886C
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

	// Token: 0x06000ACE RID: 2766 RVA: 0x0003A724 File Offset: 0x00038924
	public void ReleaseLightmap(string oldTimeOfDay)
	{
		Texture2D assetToUnload;
		if (this.lightmapsCache.Remove(oldTimeOfDay, out assetToUnload))
		{
			Resources.UnloadAsset(assetToUnload);
		}
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0003A748 File Offset: 0x00038948
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

	// Token: 0x06000AD0 RID: 2768 RVA: 0x0003A85C File Offset: 0x00038A5C
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

	// Token: 0x06000AD1 RID: 2769 RVA: 0x0003A8B4 File Offset: 0x00038AB4
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

	// Token: 0x06000AD2 RID: 2770 RVA: 0x0003A910 File Offset: 0x00038B10
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

	// Token: 0x06000AD3 RID: 2771 RVA: 0x0003A9AC File Offset: 0x00038BAC
	public static void g_StartAllScenesPopulateLightmaps(string fromLightmapName, string toLightmapName)
	{
		PerSceneRenderData._g_allScenesPopulateLightmaps_renderDatasHashSet.Clear();
		PerSceneRenderData[] array = Object.FindObjectsByType<PerSceneRenderData>(FindObjectsSortMode.None);
		PerSceneRenderData._g_allScenesPopulateLightmaps_renderDatasHashSet.UnionWith(array);
		foreach (PerSceneRenderData perSceneRenderData in array)
		{
			perSceneRenderData.StartPopulateLightmaps(fromLightmapName, toLightmapName);
			perSceneRenderData.OnPopulateToAndFromLightmapsCompleted = (Action<PerSceneRenderData>)Delegate.Combine(perSceneRenderData.OnPopulateToAndFromLightmapsCompleted, new Action<PerSceneRenderData>(PerSceneRenderData._g_AllScenesPopulateLightmaps_OnOneCompleted));
		}
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x0003AA14 File Offset: 0x00038C14
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

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0003AA59 File Offset: 0x00038C59
	public static int g_AllScenesPopulatingLightmapsLoadCount
	{
		get
		{
			return PerSceneRenderData._g_allScenesPopulateLightmaps_renderDatasHashSet.Count;
		}
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0003AA68 File Offset: 0x00038C68
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

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0003AAC4 File Offset: 0x00038CC4
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

	// Token: 0x04000D33 RID: 3379
	public Renderer representativeRenderer;

	// Token: 0x04000D34 RID: 3380
	public string lightmapsResourcePath;

	// Token: 0x04000D35 RID: 3381
	public Texture2D singleLightmap;

	// Token: 0x04000D36 RID: 3382
	private int lastLightmapIndex = -1;

	// Token: 0x04000D37 RID: 3383
	public GameObject[] gO = new GameObject[5000];

	// Token: 0x04000D38 RID: 3384
	public MeshRenderer[] mRenderers = new MeshRenderer[5000];

	// Token: 0x04000D39 RID: 3385
	public int mRendererIndex;

	// Token: 0x04000D3A RID: 3386
	private readonly Dictionary<string, ResourceRequest> resourceRequests = new Dictionary<string, ResourceRequest>(8);

	// Token: 0x04000D3B RID: 3387
	private readonly Dictionary<string, Texture2D> lightmapsCache = new Dictionary<string, Texture2D>(8);

	// Token: 0x04000D3C RID: 3388
	private Dictionary<string, List<Action<Texture2D>>> _momentName_to_callbacks = new Dictionary<string, List<Action<Texture2D>>>(8);

	// Token: 0x04000D3D RID: 3389
	private static readonly HashSet<PerSceneRenderData> _g_allScenesPopulateLightmaps_renderDatasHashSet = new HashSet<PerSceneRenderData>(32);

	// Token: 0x04000D3E RID: 3390
	public static Action g_OnAllScenesPopulateLightmapsCompleted;

	// Token: 0x04000D3F RID: 3391
	private string _populateLightmaps_fromMomentName;

	// Token: 0x04000D40 RID: 3392
	private string _populateLightmaps_toMomentName;

	// Token: 0x04000D41 RID: 3393
	private Texture2D _populateLightmaps_fromMomentLightmap;

	// Token: 0x04000D42 RID: 3394
	private Texture2D _populateLightmaps_toMomentLightmap;

	// Token: 0x04000D43 RID: 3395
	public Action<PerSceneRenderData> OnPopulateToAndFromLightmapsCompleted;
}
