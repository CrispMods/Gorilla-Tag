using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;

// Token: 0x0200053E RID: 1342
public class PhotonPrefabPool : MonoBehaviour, IPunPrefabPool, ITickSystemPre
{
	// Token: 0x1700034B RID: 843
	// (get) Token: 0x0600208B RID: 8331 RVA: 0x000452C0 File Offset: 0x000434C0
	// (set) Token: 0x0600208C RID: 8332 RVA: 0x000452C8 File Offset: 0x000434C8
	bool ITickSystemPre.PreTickRunning { get; set; }

	// Token: 0x0600208D RID: 8333 RVA: 0x000452D1 File Offset: 0x000434D1
	private void Awake()
	{
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x000F13A4 File Offset: 0x000EF5A4
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

	// Token: 0x0600208F RID: 8335 RVA: 0x000452F3 File Offset: 0x000434F3
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

	// Token: 0x06002090 RID: 8336 RVA: 0x000F1430 File Offset: 0x000EF630
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

	// Token: 0x06002091 RID: 8337 RVA: 0x000F1480 File Offset: 0x000EF680
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

	// Token: 0x06002092 RID: 8338 RVA: 0x000F154C File Offset: 0x000EF74C
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

	// Token: 0x06002093 RID: 8339 RVA: 0x000F1610 File Offset: 0x000EF810
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

	// Token: 0x06002094 RID: 8340 RVA: 0x000F1680 File Offset: 0x000EF880
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

	// Token: 0x04002481 RID: 9345
	[SerializeField]
	private PrefabType[] networkPrefabsData;

	// Token: 0x04002482 RID: 9346
	public Dictionary<string, GameObject> networkPrefabs = new Dictionary<string, GameObject>();

	// Token: 0x04002483 RID: 9347
	private Queue<GameObject> objectsWaiting = new Queue<GameObject>(20);

	// Token: 0x04002484 RID: 9348
	private Queue<GameObject> queueBeingProcssed = new Queue<GameObject>(20);

	// Token: 0x04002485 RID: 9349
	private HashSet<GameObject> objectsQueued = new HashSet<GameObject>();

	// Token: 0x04002486 RID: 9350
	private HashSet<GameObject> netInstantiedObjects = new HashSet<GameObject>();

	// Token: 0x04002487 RID: 9351
	private List<PhotonView> tempViews = new List<PhotonView>(5);

	// Token: 0x04002488 RID: 9352
	private List<GameObject> m_invalidCreatePool = new List<GameObject>(100);

	// Token: 0x04002489 RID: 9353
	private HashSet<GameObject> m_m_invalidCreatePoolLookup = new HashSet<GameObject>(100);

	// Token: 0x0400248A RID: 9354
	private bool waiting;
}
