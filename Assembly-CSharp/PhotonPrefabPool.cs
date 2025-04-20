using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class PhotonPrefabPool : MonoBehaviour, IPunPrefabPool, ITickSystemPre
{
	// Token: 0x17000352 RID: 850
	// (get) Token: 0x060020E1 RID: 8417 RVA: 0x0004665F File Offset: 0x0004485F
	// (set) Token: 0x060020E2 RID: 8418 RVA: 0x00046667 File Offset: 0x00044867
	bool ITickSystemPre.PreTickRunning { get; set; }

	// Token: 0x060020E3 RID: 8419 RVA: 0x00046670 File Offset: 0x00044870
	private void Awake()
	{
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x000F4128 File Offset: 0x000F2328
	private void Start()
	{
		PhotonNetwork.PrefabPool = this;
		NetworkSystem.Instance.AddRemoteVoiceAddedCallback(new Action<RemoteVoiceLink>(this.CheckVOIPSettings));
		for (int i = 0; i < this.networkPrefabsData.Length; i++)
		{
			PrefabType prefabType = this.networkPrefabsData[i];
			if (prefabType.prefab)
			{
				if (string.IsNullOrEmpty(prefabType.prefabName))
				{
					prefabType.prefabName = prefabType.prefab.name;
				}
				this.networkPrefabs.Add(prefabType.prefabName, prefabType.prefab);
			}
		}
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x00046692 File Offset: 0x00044892
	public void AddPrefab(GameObject go, string name)
	{
		if (go == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(name))
		{
			name = go.name;
		}
		this.networkPrefabs.Add(name, go);
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x000F41B4 File Offset: 0x000F23B4
	GameObject IPunPrefabPool.Instantiate(string prefabId, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject;
		if (!this.networkPrefabs.TryGetValue(prefabId, out gameObject))
		{
			return null;
		}
		bool activeSelf = gameObject.activeSelf;
		if (activeSelf)
		{
			gameObject.SetActive(false);
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, position, rotation);
		this.netInstantiedObjects.Add(gameObject2);
		if (activeSelf)
		{
			gameObject.SetActive(true);
		}
		return gameObject2;
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x000F4204 File Offset: 0x000F2404
	void IPunPrefabPool.Destroy(GameObject gameObject)
	{
		if (gameObject.IsNull())
		{
			return;
		}
		if (this.netInstantiedObjects.Contains(gameObject))
		{
			PhotonViewCache photonViewCache;
			if (this.m_invalidCreatePool.Count < 200 && gameObject.TryGetComponent<PhotonViewCache>(out photonViewCache) && !photonViewCache.Initialized)
			{
				if (this.m_m_invalidCreatePoolLookup.Add(gameObject))
				{
					this.m_invalidCreatePool.Add(gameObject);
				}
				return;
			}
			this.netInstantiedObjects.Remove(gameObject);
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		else
		{
			PhotonView photonView;
			if (!gameObject.TryGetComponent<PhotonView>(out photonView) || photonView.isRuntimeInstantiated)
			{
				UnityEngine.Object.Destroy(gameObject);
				return;
			}
			if (!this.objectsQueued.Contains(gameObject))
			{
				this.objectsWaiting.Enqueue(gameObject);
				this.objectsQueued.Add(gameObject);
			}
			if (!this.waiting)
			{
				this.waiting = true;
				TickSystem<object>.AddPreTickCallback(this);
			}
			return;
		}
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x000F42D0 File Offset: 0x000F24D0
	void ITickSystemPre.PreTick()
	{
		if (this.waiting)
		{
			this.waiting = false;
			return;
		}
		Queue<GameObject> queue = this.queueBeingProcssed;
		Queue<GameObject> queue2 = this.objectsWaiting;
		this.objectsWaiting = queue;
		this.queueBeingProcssed = queue2;
		while (this.queueBeingProcssed.Count > 0)
		{
			GameObject gameObject = this.queueBeingProcssed.Dequeue();
			this.objectsQueued.Remove(gameObject);
			if (!gameObject.IsNull())
			{
				gameObject.SetActive(true);
				gameObject.GetComponents<PhotonView>(this.tempViews);
				for (int i = 0; i < this.tempViews.Count; i++)
				{
					PhotonNetwork.RegisterPhotonView(this.tempViews[i]);
				}
			}
		}
		if (this.objectsQueued.Count < 1)
		{
			TickSystem<object>.RemovePreTickCallback(this);
			return;
		}
		this.waiting = true;
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x000F4394 File Offset: 0x000F2594
	private void OnLeftRoom()
	{
		foreach (GameObject gameObject in this.m_invalidCreatePool)
		{
			if (!gameObject.IsNull())
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.m_invalidCreatePool.Clear();
		this.m_m_invalidCreatePoolLookup.Clear();
	}

	// Token: 0x060020EA RID: 8426 RVA: 0x000F4404 File Offset: 0x000F2604
	private void CheckVOIPSettings(RemoteVoiceLink voiceLink)
	{
		try
		{
			NetPlayer netPlayer = null;
			if (voiceLink.Info.UserData != null)
			{
				int num;
				if (int.TryParse(voiceLink.Info.UserData.ToString(), out num))
				{
					netPlayer = NetworkSystem.Instance.GetPlayer(num / PhotonNetwork.MAX_VIEW_IDS);
				}
			}
			else
			{
				netPlayer = NetworkSystem.Instance.GetPlayer(voiceLink.PlayerId);
			}
			if (netPlayer != null)
			{
				RigContainer rigContainer;
				if ((voiceLink.Info.Bitrate > 20000 || voiceLink.Info.SamplingRate > 16000) && VRRigCache.Instance.TryGetVrrig(netPlayer, out rigContainer))
				{
					rigContainer.ForceMute = true;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
		}
	}

	// Token: 0x040024D3 RID: 9427
	[SerializeField]
	private PrefabType[] networkPrefabsData;

	// Token: 0x040024D4 RID: 9428
	public Dictionary<string, GameObject> networkPrefabs = new Dictionary<string, GameObject>();

	// Token: 0x040024D5 RID: 9429
	private Queue<GameObject> objectsWaiting = new Queue<GameObject>(20);

	// Token: 0x040024D6 RID: 9430
	private Queue<GameObject> queueBeingProcssed = new Queue<GameObject>(20);

	// Token: 0x040024D7 RID: 9431
	private HashSet<GameObject> objectsQueued = new HashSet<GameObject>();

	// Token: 0x040024D8 RID: 9432
	private HashSet<GameObject> netInstantiedObjects = new HashSet<GameObject>();

	// Token: 0x040024D9 RID: 9433
	private List<PhotonView> tempViews = new List<PhotonView>(5);

	// Token: 0x040024DA RID: 9434
	private List<GameObject> m_invalidCreatePool = new List<GameObject>(100);

	// Token: 0x040024DB RID: 9435
	private HashSet<GameObject> m_m_invalidCreatePoolLookup = new HashSet<GameObject>(100);

	// Token: 0x040024DC RID: 9436
	private bool waiting;
}
