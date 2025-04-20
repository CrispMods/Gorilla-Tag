using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GorillaLocomotion;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000552 RID: 1362
internal class VRRigCache : MonoBehaviour
{
	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06002129 RID: 8489 RVA: 0x00046916 File Offset: 0x00044B16
	// (set) Token: 0x0600212A RID: 8490 RVA: 0x0004691D File Offset: 0x00044B1D
	public static VRRigCache Instance { get; private set; }

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x0600212B RID: 8491 RVA: 0x00046925 File Offset: 0x00044B25
	public Transform NetworkParent
	{
		get
		{
			return this.networkParent;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x0600212C RID: 8492 RVA: 0x0004692D File Offset: 0x00044B2D
	// (set) Token: 0x0600212D RID: 8493 RVA: 0x00046934 File Offset: 0x00044B34
	public static bool isInitialized { get; private set; }

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x0600212E RID: 8494 RVA: 0x000F4DD4 File Offset: 0x000F2FD4
	// (remove) Token: 0x0600212F RID: 8495 RVA: 0x000F4E08 File Offset: 0x000F3008
	public static event Action OnPostInitialize;

	// Token: 0x1400004C RID: 76
	// (add) Token: 0x06002130 RID: 8496 RVA: 0x000F4E3C File Offset: 0x000F303C
	// (remove) Token: 0x06002131 RID: 8497 RVA: 0x000F4E70 File Offset: 0x000F3070
	public static event Action OnPostSpawnRig;

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x06002132 RID: 8498 RVA: 0x000F4EA4 File Offset: 0x000F30A4
	// (remove) Token: 0x06002133 RID: 8499 RVA: 0x000F4ED8 File Offset: 0x000F30D8
	public static event Action<RigContainer> OnRigActivated;

	// Token: 0x1400004E RID: 78
	// (add) Token: 0x06002134 RID: 8500 RVA: 0x000F4F0C File Offset: 0x000F310C
	// (remove) Token: 0x06002135 RID: 8501 RVA: 0x000F4F40 File Offset: 0x000F3140
	public static event Action<RigContainer> OnRigDeactivated;

	// Token: 0x06002136 RID: 8502 RVA: 0x0004693C File Offset: 0x00044B3C
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

	// Token: 0x06002137 RID: 8503 RVA: 0x000F4F74 File Offset: 0x000F3174
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

	// Token: 0x06002138 RID: 8504 RVA: 0x00046968 File Offset: 0x00044B68
	private void OnDestroy()
	{
		if (VRRigCache.Instance == this)
		{
			VRRigCache.Instance = null;
		}
		VRRigCache.isInitialized = false;
	}

	// Token: 0x06002139 RID: 8505 RVA: 0x000F5040 File Offset: 0x000F3240
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

	// Token: 0x0600213A RID: 8506 RVA: 0x00046983 File Offset: 0x00044B83
	internal bool TryGetVrrig(Player targetPlayer, out RigContainer playerRig)
	{
		return this.TryGetVrrig(NetworkSystem.Instance.GetPlayer(targetPlayer.ActorNumber), out playerRig);
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000F5094 File Offset: 0x000F3294
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

	// Token: 0x0600213C RID: 8508 RVA: 0x000F5170 File Offset: 0x000F3370
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

	// Token: 0x0600213D RID: 8509 RVA: 0x000F51D4 File Offset: 0x000F33D4
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

	// Token: 0x0600213E RID: 8510 RVA: 0x000F520C File Offset: 0x000F340C
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

	// Token: 0x0600213F RID: 8511 RVA: 0x000F5250 File Offset: 0x000F3450
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

	// Token: 0x06002140 RID: 8512 RVA: 0x000F52A8 File Offset: 0x000F34A8
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

	// Token: 0x06002141 RID: 8513 RVA: 0x000F5334 File Offset: 0x000F3534
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

	// Token: 0x06002142 RID: 8514 RVA: 0x000F541C File Offset: 0x000F361C
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

	// Token: 0x06002143 RID: 8515 RVA: 0x000F54B0 File Offset: 0x000F36B0
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

	// Token: 0x06002144 RID: 8516 RVA: 0x000F5578 File Offset: 0x000F3778
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

	// Token: 0x06002145 RID: 8517 RVA: 0x000F561C File Offset: 0x000F381C
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
		NetworkSystem.Instance.NetInstantiate(gameObject, Vector3.negativeInfinity, gameObject2.transform.rotation, false, 0, VRRigCache.rigRGBData, null);
	}

	// Token: 0x06002146 RID: 8518 RVA: 0x00030607 File Offset: 0x0002E807
	private void LogInfo(string log)
	{
	}

	// Token: 0x06002147 RID: 8519 RVA: 0x00030607 File Offset: 0x0002E807
	private void LogWarning(string log)
	{
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x00030607 File Offset: 0x0002E807
	private void LogError(string log)
	{
	}

	// Token: 0x04002500 RID: 9472
	public RigContainer localRig;

	// Token: 0x04002501 RID: 9473
	[SerializeField]
	private Transform rigParent;

	// Token: 0x04002502 RID: 9474
	[SerializeField]
	private Transform networkParent;

	// Token: 0x04002503 RID: 9475
	[SerializeField]
	private GameObject rigTemplate;

	// Token: 0x04002504 RID: 9476
	[SerializeField]
	private int rigAmount = 9;

	// Token: 0x04002505 RID: 9477
	[OnEnterPlay_Clear]
	private static Queue<RigContainer> freeRigs = new Queue<RigContainer>(10);

	// Token: 0x04002506 RID: 9478
	[OnEnterPlay_Clear]
	private static Dictionary<NetPlayer, RigContainer> rigsInUse = new Dictionary<NetPlayer, RigContainer>(10);

	// Token: 0x0400250C RID: 9484
	private static object[] rigRGBData = new object[]
	{
		0f,
		0f,
		0f
	};
}
