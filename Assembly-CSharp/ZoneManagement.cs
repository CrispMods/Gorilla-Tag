using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200021E RID: 542
public class ZoneManagement : MonoBehaviour
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06000C97 RID: 3223 RVA: 0x0009FDAC File Offset: 0x0009DFAC
	// (remove) Token: 0x06000C98 RID: 3224 RVA: 0x0009FDE0 File Offset: 0x0009DFE0
	public static event ZoneManagement.ZoneChangeEvent OnZoneChange;

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000C99 RID: 3225 RVA: 0x00038D2F File Offset: 0x00036F2F
	// (set) Token: 0x06000C9A RID: 3226 RVA: 0x00038D37 File Offset: 0x00036F37
	public bool hasInstance { get; private set; }

	// Token: 0x06000C9B RID: 3227 RVA: 0x00038D40 File Offset: 0x00036F40
	private void Awake()
	{
		if (ZoneManagement.instance == null)
		{
			this.Initialize();
			return;
		}
		if (ZoneManagement.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00038D6E File Offset: 0x00036F6E
	public static void SetActiveZone(GTZone zone)
	{
		ZoneManagement.SetActiveZones(new GTZone[]
		{
			zone
		});
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x0009FE14 File Offset: 0x0009E014
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

	// Token: 0x06000C9E RID: 3230 RVA: 0x0009FE78 File Offset: 0x0009E078
	public static bool IsInZone(GTZone zone)
	{
		if (ZoneManagement.instance == null)
		{
			ZoneManagement.FindInstance();
		}
		ZoneData zoneData = ZoneManagement.instance.GetZoneData(zone);
		return zoneData != null && zoneData.active;
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x00038D7F File Offset: 0x00036F7F
	public GameObject GetPrimaryGameObject(GTZone zone)
	{
		return this.GetZoneData(zone).rootGameObjects[0];
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x00038D8F File Offset: 0x00036F8F
	public static void AddSceneToForceStayLoaded(string sceneName)
	{
		if (ZoneManagement.instance == null)
		{
			ZoneManagement.FindInstance();
		}
		ZoneManagement.instance.sceneForceStayLoaded.Add(sceneName);
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x00038DB4 File Offset: 0x00036FB4
	public static void RemoveSceneFromForceStayLoaded(string sceneName)
	{
		if (ZoneManagement.instance == null)
		{
			ZoneManagement.FindInstance();
		}
		ZoneManagement.instance.sceneForceStayLoaded.Remove(sceneName);
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x00038DD9 File Offset: 0x00036FD9
	public static void FindInstance()
	{
		ZoneManagement zoneManagement = UnityEngine.Object.FindObjectOfType<ZoneManagement>();
		if (zoneManagement == null)
		{
			throw new NullReferenceException("Unable to find ZoneManagement object in scene.");
		}
		Debug.LogWarning("ZoneManagement accessed before MonoBehaviour awake function called; consider delaying zone management functions to avoid FindObject lookup.");
		zoneManagement.Initialize();
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x0009FEB0 File Offset: 0x0009E0B0
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

	// Token: 0x06000CA4 RID: 3236 RVA: 0x0009FEF8 File Offset: 0x0009E0F8
	public bool IsZoneActive(GTZone zone)
	{
		ZoneData zoneData = this.GetZoneData(zone);
		return zoneData != null && zoneData.active;
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x00038E03 File Offset: 0x00037003
	public HashSet<string> GetAllLoadedScenes()
	{
		return this.scenesLoaded;
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x00038E0B File Offset: 0x0003700B
	public bool IsSceneLoaded(string sceneName)
	{
		return this.scenesLoaded.Contains(sceneName);
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x0009FF18 File Offset: 0x0009E118
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

	// Token: 0x06000CA8 RID: 3240 RVA: 0x0009FFD4 File Offset: 0x0009E1D4
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

	// Token: 0x06000CA9 RID: 3241 RVA: 0x000A0318 File Offset: 0x0009E518
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

	// Token: 0x06000CAA RID: 3242 RVA: 0x000A03CC File Offset: 0x0009E5CC
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

	// Token: 0x04000FFB RID: 4091
	public static ZoneManagement instance;

	// Token: 0x04000FFD RID: 4093
	[SerializeField]
	private ZoneData[] zones;

	// Token: 0x04000FFE RID: 4094
	private GameObject[] allObjects;

	// Token: 0x04000FFF RID: 4095
	private bool[] objectActivationState;

	// Token: 0x04001000 RID: 4096
	public Action onZoneChanged;

	// Token: 0x04001001 RID: 4097
	public Action OnSceneLoadsCompleted;

	// Token: 0x04001002 RID: 4098
	public List<GTZone> activeZones = new List<GTZone>(20);

	// Token: 0x04001003 RID: 4099
	private HashSet<string> scenesLoaded = new HashSet<string>();

	// Token: 0x04001004 RID: 4100
	private HashSet<string> scenesRequested = new HashSet<string>();

	// Token: 0x04001005 RID: 4101
	private HashSet<string> sceneForceStayLoaded = new HashSet<string>(8);

	// Token: 0x04001006 RID: 4102
	private List<string> scenesToUnload = new List<string>();

	// Token: 0x04001007 RID: 4103
	private Dictionary<string, AsyncOperation> _scenes_to_loadOps = new Dictionary<string, AsyncOperation>(32);

	// Token: 0x04001008 RID: 4104
	private Dictionary<string, AsyncOperation> _scenes_to_unloadOps = new Dictionary<string, AsyncOperation>(32);

	// Token: 0x04001009 RID: 4105
	private Camera mainCamera;

	// Token: 0x0200021F RID: 543
	// (Invoke) Token: 0x06000CAD RID: 3245
	public delegate void ZoneChangeEvent(ZoneData[] zones);
}
