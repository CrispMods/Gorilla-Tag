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
	// (get) Token: 0x06002088 RID: 8328 RVA: 0x000A3206 File Offset: 0x000A1406
	// (set) Token: 0x06002089 RID: 8329 RVA: 0x000A320E File Offset: 0x000A140E
	bool ITickSystemPre.PreTickRunning { get; set; }

	// Token: 0x0600208A RID: 8330 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Awake()
	{
	}

	// Token: 0x0600208B RID: 8331 RVA: 0x000A3218 File Offset: 0x000A1418
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

	// Token: 0x0600208C RID: 8332 RVA: 0x000A32A4 File Offset: 0x000A14A4
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

	// Token: 0x0600208D RID: 8333 RVA: 0x000A32D0 File Offset: 0x000A14D0
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
		GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, position, rotation);
		this.netInstantiedObjects.Add(gameObject2);
		if (activeSelf)
		{
			gameObject.SetActive(true);
		}
		return gameObject2;
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x000A3320 File Offset: 0x000A1520
	void IPunPrefabPool.Destroy(GameObject gameObject)
	{
		if (gameObject.IsNull())
		{
			return;
		}
		if (this.netInstantiedObjects.Contains(gameObject))
		{
			this.netInstantiedObjects.Remove(gameObject);
			Object.Destroy(gameObject);
			return;
		}
		PhotonView photonView;
		if (!gameObject.TryGetComponent<PhotonView>(out photonView) || photonView.isRuntimeInstantiated)
		{
			Object.Destroy(gameObject);
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
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x000A33B0 File Offset: 0x000A15B0
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

	// Token: 0x06002090 RID: 8336 RVA: 0x000A3474 File Offset: 0x000A1674
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

	// Token: 0x04002480 RID: 9344
	[SerializeField]
	private PrefabType[] networkPrefabsData;

	// Token: 0x04002481 RID: 9345
	public Dictionary<string, GameObject> networkPrefabs = new Dictionary<string, GameObject>();

	// Token: 0x04002482 RID: 9346
	private Queue<GameObject> objectsWaiting = new Queue<GameObject>(20);

	// Token: 0x04002483 RID: 9347
	private Queue<GameObject> queueBeingProcssed = new Queue<GameObject>(20);

	// Token: 0x04002484 RID: 9348
	private HashSet<GameObject> objectsQueued = new HashSet<GameObject>();

	// Token: 0x04002485 RID: 9349
	private HashSet<GameObject> netInstantiedObjects = new HashSet<GameObject>();

	// Token: 0x04002486 RID: 9350
	private List<PhotonView> tempViews = new List<PhotonView>(5);

	// Token: 0x04002487 RID: 9351
	private bool waiting;
}
