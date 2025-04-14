using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GorillaLocomotion;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000544 RID: 1348
internal class VRRigCache : MonoBehaviour
{
	// Token: 0x1700035E RID: 862
	// (get) Token: 0x060020CB RID: 8395 RVA: 0x000A4074 File Offset: 0x000A2274
	// (set) Token: 0x060020CC RID: 8396 RVA: 0x000A407B File Offset: 0x000A227B
	public static VRRigCache Instance { get; private set; }

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x060020CD RID: 8397 RVA: 0x000A4083 File Offset: 0x000A2283
	public Transform NetworkParent
	{
		get
		{
			return this.networkParent;
		}
	}

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x060020CE RID: 8398 RVA: 0x000A408B File Offset: 0x000A228B
	// (set) Token: 0x060020CF RID: 8399 RVA: 0x000A4092 File Offset: 0x000A2292
	public static bool isInitialized { get; private set; }

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x060020D0 RID: 8400 RVA: 0x000A409C File Offset: 0x000A229C
	// (remove) Token: 0x060020D1 RID: 8401 RVA: 0x000A40D0 File Offset: 0x000A22D0
	public static event Action OnPostInitialize;

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x060020D2 RID: 8402 RVA: 0x000A4104 File Offset: 0x000A2304
	// (remove) Token: 0x060020D3 RID: 8403 RVA: 0x000A4138 File Offset: 0x000A2338
	public static event Action OnPostSpawnRig;

	// Token: 0x1400004C RID: 76
	// (add) Token: 0x060020D4 RID: 8404 RVA: 0x000A416C File Offset: 0x000A236C
	// (remove) Token: 0x060020D5 RID: 8405 RVA: 0x000A41A0 File Offset: 0x000A23A0
	public static event Action<RigContainer> OnRigActivated;

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x060020D6 RID: 8406 RVA: 0x000A41D4 File Offset: 0x000A23D4
	// (remove) Token: 0x060020D7 RID: 8407 RVA: 0x000A4208 File Offset: 0x000A2408
	public static event Action<RigContainer> OnRigDeactivated;

	// Token: 0x060020D8 RID: 8408 RVA: 0x000A423B File Offset: 0x000A243B
	private void Start()
	{
		this.InitializeVRRigCache();
		RigContainer rigContainer = this.localRig;
		if (rigContainer == null)
		{
			return;
		}
		VRRig rig = rigContainer.Rig;
		if (rig == null)
		{
			return;
		}
		GorillaBodyRenderer bodyRenderer = rig.bodyRenderer;
		if (bodyRenderer == null)
		{
			return;
		}
		bodyRenderer.SetupAsLocalPlayerBody();
	}

	// Token: 0x060020D9 RID: 8409 RVA: 0x000A4268 File Offset: 0x000A2468
	public void InitializeVRRigCache()
	{
		if (VRRigCache.isInitialized || ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		if (VRRigCache.Instance != null && VRRigCache.Instance != this)
		{
			Object.Destroy(this);
			return;
		}
		VRRigCache.Instance = this;
		if (this.rigParent == null)
		{
			this.rigParent = base.transform;
		}
		if (this.networkParent == null)
		{
			this.networkParent = base.transform;
		}
		for (int i = 0; i < this.rigAmount; i++)
		{
			RigContainer rigContainer = this.SpawnRig();
			VRRigCache.freeRigs.Enqueue(rigContainer);
			rigContainer.Rig.BuildInitialize();
			rigContainer.Rig.transform.parent = null;
		}
		VRRigCache.isInitialized = true;
		Action onPostInitialize = VRRigCache.OnPostInitialize;
		if (onPostInitialize == null)
		{
			return;
		}
		onPostInitialize();
	}

	// Token: 0x060020DA RID: 8410 RVA: 0x000A4333 File Offset: 0x000A2533
	private void OnDestroy()
	{
		if (VRRigCache.Instance == this)
		{
			VRRigCache.Instance = null;
		}
	}

	// Token: 0x060020DB RID: 8411 RVA: 0x000A4348 File Offset: 0x000A2548
	private RigContainer SpawnRig()
	{
		if (this.rigTemplate.activeSelf)
		{
			this.rigTemplate.SetActive(false);
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.rigTemplate, this.rigParent, false);
		Action onPostSpawnRig = VRRigCache.OnPostSpawnRig;
		if (onPostSpawnRig != null)
		{
			onPostSpawnRig();
		}
		if (gameObject == null)
		{
			return null;
		}
		return gameObject.GetComponent<RigContainer>();
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x000A439B File Offset: 0x000A259B
	internal bool TryGetVrrig(Player targetPlayer, out RigContainer playerRig)
	{
		return this.TryGetVrrig(NetworkSystem.Instance.GetPlayer(targetPlayer.ActorNumber), out playerRig);
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x000A43B4 File Offset: 0x000A25B4
	internal bool TryGetVrrig(NetPlayer targetPlayer, out RigContainer playerRig)
	{
		playerRig = null;
		if (ApplicationQuittingState.IsQuitting)
		{
			return false;
		}
		if (targetPlayer == null || targetPlayer.IsNull)
		{
			this.LogError("VrRigCache - target player is null");
			return false;
		}
		if (targetPlayer.IsLocal)
		{
			playerRig = this.localRig;
			return true;
		}
		if (!targetPlayer.InRoom)
		{
			this.LogWarning("player is not in room?? " + targetPlayer.UserId);
			return false;
		}
		if (VRRigCache.rigsInUse.ContainsKey(targetPlayer))
		{
			playerRig = VRRigCache.rigsInUse[targetPlayer];
		}
		else
		{
			if (VRRigCache.freeRigs.Count <= 0)
			{
				this.LogWarning("all rigs are in use");
				return false;
			}
			playerRig = VRRigCache.freeRigs.Dequeue();
			playerRig.Creator = targetPlayer;
			VRRigCache.rigsInUse.Add(targetPlayer, playerRig);
			playerRig.gameObject.SetActive(true);
			Action<RigContainer> onRigActivated = VRRigCache.OnRigActivated;
			if (onRigActivated != null)
			{
				onRigActivated(playerRig);
			}
		}
		return true;
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000A4490 File Offset: 0x000A2690
	private void AddRigToGorillaParent(NetPlayer player, VRRig vrrig)
	{
		GorillaParent instance = GorillaParent.instance;
		if (instance == null)
		{
			return;
		}
		if (!instance.vrrigs.Contains(vrrig))
		{
			instance.vrrigs.Add(vrrig);
		}
		if (!instance.vrrigDict.ContainsKey(player))
		{
			instance.vrrigDict.Add(player, vrrig);
			return;
		}
		instance.vrrigDict[player] = vrrig;
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x000A44F4 File Offset: 0x000A26F4
	public void OnPlayerEnteredRoom(NetPlayer newPlayer)
	{
		if (newPlayer.ActorNumber == -1)
		{
			Debug.LogError("LocalPlayer returned, vrrig no correctly initialised");
		}
		RigContainer rigContainer;
		if (this.TryGetVrrig(newPlayer, out rigContainer))
		{
			this.AddRigToGorillaParent(newPlayer, rigContainer.Rig);
		}
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x000A452C File Offset: 0x000A272C
	public void OnJoinedRoom()
	{
		foreach (NetPlayer netPlayer in NetworkSystem.Instance.AllNetPlayers)
		{
			RigContainer rigContainer;
			if (this.TryGetVrrig(netPlayer, out rigContainer))
			{
				this.AddRigToGorillaParent(netPlayer, rigContainer.Rig);
			}
		}
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x000A4570 File Offset: 0x000A2770
	private void RemoveRigFromGorillaParent(NetPlayer player, VRRig vrrig)
	{
		GorillaParent instance = GorillaParent.instance;
		if (instance == null)
		{
			return;
		}
		if (instance.vrrigs.Contains(vrrig))
		{
			instance.vrrigs.Remove(vrrig);
		}
		if (instance.vrrigDict.ContainsKey(player))
		{
			instance.vrrigDict.Remove(player);
		}
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x000A45C8 File Offset: 0x000A27C8
	public void OnPlayerLeftRoom(NetPlayer leavingPlayer)
	{
		if (leavingPlayer.IsNull)
		{
			Debug.LogError("Leaving players NetPlayer is Null");
			this.CheckForMissingPlayer();
		}
		RigContainer rigContainer;
		if (!VRRigCache.rigsInUse.TryGetValue(leavingPlayer, out rigContainer))
		{
			this.LogError("failed to find player's vrrig who left " + leavingPlayer.UserId);
			return;
		}
		rigContainer.gameObject.Disable();
		VRRigCache.freeRigs.Enqueue(rigContainer);
		VRRigCache.rigsInUse.Remove(leavingPlayer);
		this.RemoveRigFromGorillaParent(leavingPlayer, rigContainer.Rig);
		Action<RigContainer> onRigDeactivated = VRRigCache.OnRigDeactivated;
		if (onRigDeactivated == null)
		{
			return;
		}
		onRigDeactivated(rigContainer);
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000A4654 File Offset: 0x000A2854
	private void CheckForMissingPlayer()
	{
		foreach (KeyValuePair<NetPlayer, RigContainer> keyValuePair in VRRigCache.rigsInUse)
		{
			if (keyValuePair.Key == null || keyValuePair.Value == null)
			{
				Debug.LogError("Somehow null reference in rigsInUse");
			}
			else if (!keyValuePair.Key.InRoom)
			{
				keyValuePair.Value.gameObject.Disable();
				VRRigCache.freeRigs.Enqueue(keyValuePair.Value);
				VRRigCache.rigsInUse.Remove(keyValuePair.Key);
				this.RemoveRigFromGorillaParent(keyValuePair.Key, keyValuePair.Value.Rig);
				Action<RigContainer> onRigDeactivated = VRRigCache.OnRigDeactivated;
				if (onRigDeactivated != null)
				{
					onRigDeactivated(keyValuePair.Value);
				}
			}
		}
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x000A473C File Offset: 0x000A293C
	public void OnLeftRoom()
	{
		foreach (NetPlayer netPlayer in VRRigCache.rigsInUse.Keys.ToArray<NetPlayer>())
		{
			RigContainer rigContainer = VRRigCache.rigsInUse[netPlayer];
			if (!(rigContainer == null))
			{
				VRRig rig = VRRigCache.rigsInUse[netPlayer].Rig;
				rigContainer.gameObject.Disable();
				VRRigCache.rigsInUse.Remove(netPlayer);
				this.RemoveRigFromGorillaParent(netPlayer, rig);
				VRRigCache.freeRigs.Enqueue(rigContainer);
				Action<RigContainer> onRigDeactivated = VRRigCache.OnRigDeactivated;
				if (onRigDeactivated != null)
				{
					onRigDeactivated(rigContainer);
				}
			}
		}
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x000A47D0 File Offset: 0x000A29D0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal VRRig[] GetAllRigs()
	{
		VRRig[] array = new VRRig[VRRigCache.rigsInUse.Count + VRRigCache.freeRigs.Count];
		int num = 0;
		foreach (RigContainer rigContainer in VRRigCache.rigsInUse.Values)
		{
			array[num] = rigContainer.Rig;
			num++;
		}
		foreach (RigContainer rigContainer2 in VRRigCache.freeRigs)
		{
			array[num] = rigContainer2.Rig;
			num++;
		}
		return array;
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x000A4898 File Offset: 0x000A2A98
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal int GetAllRigsHash()
	{
		int num = 0;
		foreach (RigContainer rigContainer in VRRigCache.rigsInUse.Values)
		{
			num += rigContainer.GetInstanceID();
		}
		foreach (RigContainer rigContainer2 in VRRigCache.freeRigs)
		{
			num += rigContainer2.GetInstanceID();
		}
		return num;
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x000A493C File Offset: 0x000A2B3C
	internal void InstantiateNetworkObject()
	{
		GameObject gameObject;
		VRRigCache.Instance.GetComponent<PhotonPrefabPool>().networkPrefabs.TryGetValue("Player Network Controller", out gameObject);
		if (gameObject == null)
		{
			Debug.LogError("OnJoinedRoom: Unable to find player prefab to spawn");
			return;
		}
		GameObject gameObject2 = GTPlayer.Instance.gameObject;
		Color playerColor = this.localRig.Rig.playerColor;
		VRRigCache.rigRGBData[0] = playerColor.r;
		VRRigCache.rigRGBData[1] = playerColor.g;
		VRRigCache.rigRGBData[2] = playerColor.b;
		NetworkSystem.Instance.NetInstantiate(gameObject, gameObject2.transform.position, gameObject2.transform.rotation, false, 0, VRRigCache.rigRGBData, null);
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x000023F4 File Offset: 0x000005F4
	private void LogInfo(string log)
	{
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x000023F4 File Offset: 0x000005F4
	private void LogWarning(string log)
	{
	}

	// Token: 0x060020EA RID: 8426 RVA: 0x000023F4 File Offset: 0x000005F4
	private void LogError(string log)
	{
	}

	// Token: 0x040024A8 RID: 9384
	public RigContainer localRig;

	// Token: 0x040024A9 RID: 9385
	[SerializeField]
	private Transform rigParent;

	// Token: 0x040024AA RID: 9386
	[SerializeField]
	private Transform networkParent;

	// Token: 0x040024AB RID: 9387
	[SerializeField]
	private GameObject rigTemplate;

	// Token: 0x040024AC RID: 9388
	[SerializeField]
	private int rigAmount = 9;

	// Token: 0x040024AD RID: 9389
	[OnEnterPlay_Clear]
	private static Queue<RigContainer> freeRigs = new Queue<RigContainer>(10);

	// Token: 0x040024AE RID: 9390
	[OnEnterPlay_Clear]
	private static Dictionary<NetPlayer, RigContainer> rigsInUse = new Dictionary<NetPlayer, RigContainer>(10);

	// Token: 0x040024B4 RID: 9396
	private static object[] rigRGBData = new object[]
	{
		0f,
		0f,
		0f
	};
}
