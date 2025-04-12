using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GorillaLocomotion;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000545 RID: 1349
internal class VRRigCache : MonoBehaviour
{
	// Token: 0x1700035F RID: 863
	// (get) Token: 0x060020D3 RID: 8403 RVA: 0x00045577 File Offset: 0x00043777
	// (set) Token: 0x060020D4 RID: 8404 RVA: 0x0004557E File Offset: 0x0004377E
	public static VRRigCache Instance { get; private set; }

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x060020D5 RID: 8405 RVA: 0x00045586 File Offset: 0x00043786
	public Transform NetworkParent
	{
		get
		{
			return this.networkParent;
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x060020D6 RID: 8406 RVA: 0x0004558E File Offset: 0x0004378E
	// (set) Token: 0x060020D7 RID: 8407 RVA: 0x00045595 File Offset: 0x00043795
	public static bool isInitialized { get; private set; }

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x060020D8 RID: 8408 RVA: 0x000F2050 File Offset: 0x000F0250
	// (remove) Token: 0x060020D9 RID: 8409 RVA: 0x000F2084 File Offset: 0x000F0284
	public static event Action OnPostInitialize;

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x060020DA RID: 8410 RVA: 0x000F20B8 File Offset: 0x000F02B8
	// (remove) Token: 0x060020DB RID: 8411 RVA: 0x000F20EC File Offset: 0x000F02EC
	public static event Action OnPostSpawnRig;

	// Token: 0x1400004C RID: 76
	// (add) Token: 0x060020DC RID: 8412 RVA: 0x000F2120 File Offset: 0x000F0320
	// (remove) Token: 0x060020DD RID: 8413 RVA: 0x000F2154 File Offset: 0x000F0354
	public static event Action<RigContainer> OnRigActivated;

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x060020DE RID: 8414 RVA: 0x000F2188 File Offset: 0x000F0388
	// (remove) Token: 0x060020DF RID: 8415 RVA: 0x000F21BC File Offset: 0x000F03BC
	public static event Action<RigContainer> OnRigDeactivated;

	// Token: 0x060020E0 RID: 8416 RVA: 0x0004559D File Offset: 0x0004379D
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

	// Token: 0x060020E1 RID: 8417 RVA: 0x000F21F0 File Offset: 0x000F03F0
	public void InitializeVRRigCache()
	{
		if (VRRigCache.isInitialized || ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		if (VRRigCache.Instance != null && VRRigCache.Instance != this)
		{
			UnityEngine.Object.Destroy(this);
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

	// Token: 0x060020E2 RID: 8418 RVA: 0x000455C9 File Offset: 0x000437C9
	private void OnDestroy()
	{
		if (VRRigCache.Instance == this)
		{
			VRRigCache.Instance = null;
		}
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000F22BC File Offset: 0x000F04BC
	private RigContainer SpawnRig()
	{
		if (this.rigTemplate.activeSelf)
		{
			this.rigTemplate.SetActive(false);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.rigTemplate, this.rigParent, false);
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

	// Token: 0x060020E4 RID: 8420 RVA: 0x000455DE File Offset: 0x000437DE
	internal bool TryGetVrrig(Player targetPlayer, out RigContainer playerRig)
	{
		return this.TryGetVrrig(NetworkSystem.Instance.GetPlayer(targetPlayer.ActorNumber), out playerRig);
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x000F2310 File Offset: 0x000F0510
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

	// Token: 0x060020E6 RID: 8422 RVA: 0x000F23EC File Offset: 0x000F05EC
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

	// Token: 0x060020E7 RID: 8423 RVA: 0x000F2450 File Offset: 0x000F0650
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

	// Token: 0x060020E8 RID: 8424 RVA: 0x000F2488 File Offset: 0x000F0688
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

	// Token: 0x060020E9 RID: 8425 RVA: 0x000F24CC File Offset: 0x000F06CC
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

	// Token: 0x060020EA RID: 8426 RVA: 0x000F2524 File Offset: 0x000F0724
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

	// Token: 0x060020EB RID: 8427 RVA: 0x000F25B0 File Offset: 0x000F07B0
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

	// Token: 0x060020EC RID: 8428 RVA: 0x000F2698 File Offset: 0x000F0898
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

	// Token: 0x060020ED RID: 8429 RVA: 0x000F272C File Offset: 0x000F092C
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

	// Token: 0x060020EE RID: 8430 RVA: 0x000F27F4 File Offset: 0x000F09F4
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

	// Token: 0x060020EF RID: 8431 RVA: 0x000F2898 File Offset: 0x000F0A98
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

	// Token: 0x060020F0 RID: 8432 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void LogInfo(string log)
	{
	}

	// Token: 0x060020F1 RID: 8433 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void LogWarning(string log)
	{
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void LogError(string log)
	{
	}

	// Token: 0x040024AE RID: 9390
	public RigContainer localRig;

	// Token: 0x040024AF RID: 9391
	[SerializeField]
	private Transform rigParent;

	// Token: 0x040024B0 RID: 9392
	[SerializeField]
	private Transform networkParent;

	// Token: 0x040024B1 RID: 9393
	[SerializeField]
	private GameObject rigTemplate;

	// Token: 0x040024B2 RID: 9394
	[SerializeField]
	private int rigAmount = 9;

	// Token: 0x040024B3 RID: 9395
	[OnEnterPlay_Clear]
	private static Queue<RigContainer> freeRigs = new Queue<RigContainer>(10);

	// Token: 0x040024B4 RID: 9396
	[OnEnterPlay_Clear]
	private static Dictionary<NetPlayer, RigContainer> rigsInUse = new Dictionary<NetPlayer, RigContainer>(10);

	// Token: 0x040024BA RID: 9402
	private static object[] rigRGBData = new object[]
	{
		0f,
		0f,
		0f
	};
}
