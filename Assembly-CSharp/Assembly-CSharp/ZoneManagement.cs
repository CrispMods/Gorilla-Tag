using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000213 RID: 531
public class ZoneManagement : MonoBehaviour
{
	// Token: 0x14000021 RID: 33
	// (add) Token: 0x06000C4E RID: 3150 RVA: 0x00041CA4 File Offset: 0x0003FEA4
	// (remove) Token: 0x06000C4F RID: 3151 RVA: 0x00041CD8 File Offset: 0x0003FED8
	public static event ZoneManagement.ZoneChangeEvent OnZoneChange;

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000C50 RID: 3152 RVA: 0x00041D0B File Offset: 0x0003FF0B
	// (set) Token: 0x06000C51 RID: 3153 RVA: 0x00041D13 File Offset: 0x0003FF13
	public bool hasInstance { get; private set; }

	// Token: 0x06000C52 RID: 3154 RVA: 0x00041D1C File Offset: 0x0003FF1C
	private void Awake()
	{
		if (ZoneManagement.instance == null)
		{
			this.Initialize();
			return;
		}
		if (ZoneManagement.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00041D4A File Offset: 0x0003FF4A
	public static void SetActiveZone(GTZone zone)
	{
		ZoneManagement.SetActiveZones(new GTZone[]
		{
			zone
		});
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00041D5C File Offset: 0x0003FF5C
	public static void SetActiveZones(GTZone[] zones)
	{
		if (ZoneManagement.instance == null)
		{
			ZoneManagement.FindInstance();
		}
		if (zones == null || zones.Length == 0)
		{
			return;
		}
		ZoneManagement.instance.SetZones(zones);
		Action action = ZoneManagement.instance.onZoneChanged;
		if (action != null)
		{
			action();
		}
		if (ZoneManagement.OnZoneChange != null)
		{
			ZoneManagement.OnZoneChange(ZoneManagement.instance.zones);
		}
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00041DC0 File Offset: 0x0003FFC0
	public static bool IsInZone(GTZone zone)
	{
		if (ZoneManagement.instance == null)
		{
			ZoneManagement.FindInstance();
		}
		ZoneData zoneData = ZoneManagement.instance.GetZoneData(zone);
		return zoneData != null && zoneData.active;
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x00041DF6 File Offset: 0x0003FFF6
	public GameObject GetPrimaryGameObject(GTZone zone)
	{
		return this.GetZoneData(zone).rootGameObjects[0];
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00041E06 File Offset: 0x00040006
	public static void AddSceneToForceStayLoaded(string sceneName)
	{
		if (ZoneManagement.instance == null)
		{
			ZoneManagement.FindInstance();
		}
		ZoneManagement.instance.sceneForceStayLoaded.Add(sceneName);
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x00041E2B File Offset: 0x0004002B
	public static void RemoveSceneFromForceStayLoaded(string sceneName)
	{
		if (ZoneManagement.instance == null)
		{
			ZoneManagement.FindInstance();
		}
		ZoneManagement.instance.sceneForceStayLoaded.Remove(sceneName);
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00041E50 File Offset: 0x00040050
	public static void FindInstance()
	{
		ZoneManagement zoneManagement = Object.FindObjectOfType<ZoneManagement>();
		if (zoneManagement == null)
		{
			throw new NullReferenceException("Unable to find ZoneManagement object in scene.");
		}
		Debug.LogWarning("ZoneManagement accessed before MonoBehaviour awake function called; consider delaying zone management functions to avoid FindObject lookup.");
		zoneManagement.Initialize();
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x00041E7C File Offset: 0x0004007C
	public bool IsSceneLoaded(GTZone gtZone)
	{
		foreach (ZoneData zoneData in this.zones)
		{
			if (zoneData.zone == gtZone && this.scenesLoaded.Contains(zoneData.sceneName))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00041EC4 File Offset: 0x000400C4
	public bool IsZoneActive(GTZone zone)
	{
		ZoneData zoneData = this.GetZoneData(zone);
		return zoneData != null && zoneData.active;
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x00041EE4 File Offset: 0x000400E4
	public HashSet<string> GetAllLoadedScenes()
	{
		return this.scenesLoaded;
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x00041EEC File Offset: 0x000400EC
	public bool IsSceneLoaded(string sceneName)
	{
		return this.scenesLoaded.Contains(sceneName);
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00041EFC File Offset: 0x000400FC
	private void Initialize()
	{
		ZoneManagement.instance = this;
		this.hasInstance = true;
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		List<GameObject> list = new List<GameObject>(8);
		for (int i = 0; i < this.zones.Length; i++)
		{
			list.Clear();
			ZoneData zoneData = this.zones[i];
			if (zoneData != null && zoneData.rootGameObjects != null)
			{
				hashSet.UnionWith(zoneData.rootGameObjects);
				for (int j = 0; j < zoneData.rootGameObjects.Length; j++)
				{
					GameObject gameObject = zoneData.rootGameObjects[j];
					if (!(gameObject == null))
					{
						list.Add(gameObject);
					}
				}
				hashSet.UnionWith(list);
			}
		}
		this.allObjects = hashSet.ToArray<GameObject>();
		this.objectActivationState = new bool[this.allObjects.Length];
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x00041FB8 File Offset: 0x000401B8
	private void SetZones(GTZone[] newActiveZones)
	{
		for (int i = 0; i < this.objectActivationState.Length; i++)
		{
			this.objectActivationState[i] = false;
		}
		this.activeZones.Clear();
		for (int j = 0; j < newActiveZones.Length; j++)
		{
			this.activeZones.Add(newActiveZones[j]);
		}
		this.scenesRequested.Clear();
		this.scenesRequested.Add("GorillaTag");
		float num = 0f;
		for (int k = 0; k < this.zones.Length; k++)
		{
			ZoneData zoneData = this.zones[k];
			if (zoneData == null || zoneData.rootGameObjects == null || !newActiveZones.Contains(zoneData.zone))
			{
				zoneData.active = false;
			}
			else
			{
				zoneData.active = true;
				num = Mathf.Max(num, zoneData.CameraFarClipPlane);
				if (!string.IsNullOrEmpty(zoneData.sceneName))
				{
					this.scenesRequested.Add(zoneData.sceneName);
				}
				foreach (GameObject x in zoneData.rootGameObjects)
				{
					if (!(x == null))
					{
						for (int m = 0; m < this.allObjects.Length; m++)
						{
							if (x == this.allObjects[m])
							{
								this.objectActivationState[m] = true;
								break;
							}
						}
					}
				}
			}
		}
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
		}
		this.mainCamera.farClipPlane = num;
		int loadedSceneCount = SceneManager.loadedSceneCount;
		for (int n = 0; n < loadedSceneCount; n++)
		{
			this.scenesLoaded.Add(SceneManager.GetSceneAt(n).name);
		}
		foreach (string text in this.scenesRequested)
		{
			if (this.scenesLoaded.Add(text))
			{
				AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(text, LoadSceneMode.Additive);
				this._scenes_to_loadOps[text] = asyncOperation;
				asyncOperation.completed += this.HandleOnSceneLoadCompleted;
			}
		}
		this.scenesToUnload.Clear();
		foreach (string item in this.scenesLoaded)
		{
			if (!this.scenesRequested.Contains(item) && !this.sceneForceStayLoaded.Contains(item))
			{
				this.scenesToUnload.Add(item);
			}
		}
		foreach (string text2 in this.scenesToUnload)
		{
			this.scenesLoaded.Remove(text2);
			AsyncOperation value = SceneManager.UnloadSceneAsync(text2);
			this._scenes_to_unloadOps[text2] = value;
		}
		for (int num2 = 0; num2 < this.objectActivationState.Length; num2++)
		{
			if (!(this.allObjects[num2] == null))
			{
				this.allObjects[num2].SetActive(this.objectActivationState[num2]);
			}
		}
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x000422FC File Offset: 0x000404FC
	private void HandleOnSceneLoadCompleted(AsyncOperation thisLoadOp)
	{
		using (Dictionary<string, AsyncOperation>.ValueCollection.Enumerator enumerator = this._scenes_to_loadOps.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.isDone)
				{
					return;
				}
			}
		}
		using (Dictionary<string, AsyncOperation>.ValueCollection.Enumerator enumerator = this._scenes_to_unloadOps.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.isDone)
				{
					return;
				}
			}
		}
		Action onSceneLoadsCompleted = this.OnSceneLoadsCompleted;
		if (onSceneLoadsCompleted == null)
		{
			return;
		}
		onSceneLoadsCompleted();
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x000423B0 File Offset: 0x000405B0
	private ZoneData GetZoneData(GTZone zone)
	{
		for (int i = 0; i < this.zones.Length; i++)
		{
			if (this.zones[i].zone == zone)
			{
				return this.zones[i];
			}
		}
		return null;
	}

	// Token: 0x04000FB6 RID: 4022
	public static ZoneManagement instance;

	// Token: 0x04000FB8 RID: 4024
	[SerializeField]
	private ZoneData[] zones;

	// Token: 0x04000FB9 RID: 4025
	private GameObject[] allObjects;

	// Token: 0x04000FBA RID: 4026
	private bool[] objectActivationState;

	// Token: 0x04000FBB RID: 4027
	public Action onZoneChanged;

	// Token: 0x04000FBC RID: 4028
	public Action OnSceneLoadsCompleted;

	// Token: 0x04000FBD RID: 4029
	public List<GTZone> activeZones = new List<GTZone>(20);

	// Token: 0x04000FBE RID: 4030
	private HashSet<string> scenesLoaded = new HashSet<string>();

	// Token: 0x04000FBF RID: 4031
	private HashSet<string> scenesRequested = new HashSet<string>();

	// Token: 0x04000FC0 RID: 4032
	private HashSet<string> sceneForceStayLoaded = new HashSet<string>(8);

	// Token: 0x04000FC1 RID: 4033
	private List<string> scenesToUnload = new List<string>();

	// Token: 0x04000FC2 RID: 4034
	private Dictionary<string, AsyncOperation> _scenes_to_loadOps = new Dictionary<string, AsyncOperation>(32);

	// Token: 0x04000FC3 RID: 4035
	private Dictionary<string, AsyncOperation> _scenes_to_unloadOps = new Dictionary<string, AsyncOperation>(32);

	// Token: 0x04000FC4 RID: 4036
	private Camera mainCamera;

	// Token: 0x02000214 RID: 532
	// (Invoke) Token: 0x06000C64 RID: 3172
	public delegate void ZoneChangeEvent(ZoneData[] zones);
}
