using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GorillaExtensions;
using GorillaNetworking;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

// Token: 0x020007F6 RID: 2038
internal class PlayerCosmeticsSystem : MonoBehaviour, ITickSystemPre
{
	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06003235 RID: 12853 RVA: 0x000F0E28 File Offset: 0x000EF028
	// (set) Token: 0x06003236 RID: 12854 RVA: 0x000F0E30 File Offset: 0x000EF030
	bool ITickSystemPre.PreTickRunning { get; set; }

	// Token: 0x06003237 RID: 12855 RVA: 0x000F0E3C File Offset: 0x000EF03C
	private void Awake()
	{
		if (PlayerCosmeticsSystem.instance == null)
		{
			PlayerCosmeticsSystem.instance = this;
			base.transform.SetParent(null, true);
			Object.DontDestroyOnLoad(this);
			this.inventory = new List<string>();
			this.inventory.Add("Inventory");
			NetworkSystem.Instance.OnRaiseEvent += this.OnNetEvent;
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x06003238 RID: 12856 RVA: 0x000F0EA9 File Offset: 0x000EF0A9
	private void Start()
	{
		this.playerLookUpCooldown = Mathf.Max(this.playerLookUpCooldown, 3f);
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x000F0EC1 File Offset: 0x000EF0C1
	private void OnDestroy()
	{
		if (PlayerCosmeticsSystem.instance == this)
		{
			PlayerCosmeticsSystem.instance = null;
		}
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x000F0ED6 File Offset: 0x000EF0D6
	private void LookUpPlayerCosmetics(bool wait = false)
	{
		if (!this.isLookingUp)
		{
			TickSystem<object>.AddPreTickCallback(this);
			if (wait)
			{
				this.startSearchingTime = Time.time;
				return;
			}
			this.startSearchingTime = float.MinValue;
		}
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x000F0F00 File Offset: 0x000EF100
	public void PreTick()
	{
		if (PlayerCosmeticsSystem.playersToLookUp.Count < 1)
		{
			TickSystem<object>.RemovePreTickCallback(this);
			this.startSearchingTime = float.MinValue;
			this.isLookingUp = false;
			return;
		}
		this.isLookingUp = true;
		if (this.startSearchingTime + this.playerLookUpCooldown > Time.time)
		{
			return;
		}
		if (GorillaServer.Instance.NewCosmeticsPath())
		{
			this.NewCosmeticsPath();
			return;
		}
		PlayerCosmeticsSystem.playerIDsList.Clear();
		while (PlayerCosmeticsSystem.playersToLookUp.Count > 0)
		{
			NetPlayer netPlayer = PlayerCosmeticsSystem.playersToLookUp.Dequeue();
			string item = netPlayer.ActorNumber.ToString();
			if (netPlayer.InRoom() && !PlayerCosmeticsSystem.playerIDsList.Contains(item))
			{
				if (PlayerCosmeticsSystem.playerIDsList.Count == 0)
				{
					int actorNumber = netPlayer.ActorNumber;
				}
				PlayerCosmeticsSystem.playerIDsList.Add(item);
				PlayerCosmeticsSystem.playersWaiting.AddSortedUnique(netPlayer.ActorNumber);
			}
		}
		if (PlayerCosmeticsSystem.playerIDsList.Count > 0)
		{
			Debug.Log("group id to look up: " + NetworkSystem.Instance.RoomName + Regex.Replace(NetworkSystem.Instance.CurrentRegion, "[^a-zA-Z0-9]", "").ToUpper());
			PlayFab.ClientModels.GetSharedGroupDataRequest getSharedGroupDataRequest = new PlayFab.ClientModels.GetSharedGroupDataRequest();
			getSharedGroupDataRequest.Keys = PlayerCosmeticsSystem.playerIDsList;
			getSharedGroupDataRequest.SharedGroupId = NetworkSystem.Instance.RoomName + Regex.Replace(NetworkSystem.Instance.CurrentRegion, "[^a-zA-Z0-9]", "").ToUpper();
			PlayFabClientAPI.GetSharedGroupData(getSharedGroupDataRequest, new Action<GetSharedGroupDataResult>(this.OnGetsharedGroupData), delegate(PlayFabError error)
			{
				Debug.Log("Got error retrieving user data:");
				Debug.Log(error.GenerateErrorReport());
				if (error.Error == PlayFabErrorCode.NotAuthenticated)
				{
					PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					return;
				}
				if (error.Error == PlayFabErrorCode.AccountBanned)
				{
					GorillaGameManager.ForceStopGame_DisconnectAndDestroy();
				}
			}, null, null);
		}
		this.isLookingUp = false;
	}

	// Token: 0x0600323C RID: 12860 RVA: 0x000F109F File Offset: 0x000EF29F
	private void NewCosmeticsPath()
	{
		if (this.isLookingUpNew)
		{
			return;
		}
		base.StartCoroutine(this.NewCosmeticsPathCoroutine());
	}

	// Token: 0x0600323D RID: 12861 RVA: 0x000F10B7 File Offset: 0x000EF2B7
	private IEnumerator NewCosmeticsPathCoroutine()
	{
		this.isLookingUpNew = true;
		NetPlayer player = null;
		PlayerCosmeticsSystem.playerIDsList.Clear();
		PlayerCosmeticsSystem.playerActorNumberList.Clear();
		while (PlayerCosmeticsSystem.playersToLookUp.Count > 0)
		{
			player = PlayerCosmeticsSystem.playersToLookUp.Dequeue();
			string item = player.ActorNumber.ToString();
			if (player.InRoom() && !PlayerCosmeticsSystem.playerIDsList.Contains(item))
			{
				PlayerCosmeticsSystem.playerIDsList.Add(player.UserId);
				PlayerCosmeticsSystem.playerActorNumberList.Add(player.ActorNumber);
			}
		}
		int num;
		for (int i = 0; i < PlayerCosmeticsSystem.playerIDsList.Count; i = num + 1)
		{
			int j = i;
			PlayFab.ClientModels.GetSharedGroupDataRequest getSharedGroupDataRequest = new PlayFab.ClientModels.GetSharedGroupDataRequest();
			getSharedGroupDataRequest.Keys = this.inventory;
			getSharedGroupDataRequest.SharedGroupId = PlayerCosmeticsSystem.playerIDsList[j] + "Inventory";
			PlayFabClientAPI.GetSharedGroupData(getSharedGroupDataRequest, delegate(GetSharedGroupDataResult result)
			{
				if (!NetworkSystem.Instance.InRoom)
				{
					PlayerCosmeticsSystem.playersWaiting.Clear();
					return;
				}
				foreach (KeyValuePair<string, PlayFab.ClientModels.SharedGroupDataRecord> keyValuePair in result.Data)
				{
					if (!(keyValuePair.Key != "Inventory") && Utils.PlayerInRoom(PlayerCosmeticsSystem.playerActorNumberList[j]))
					{
						this.tempCosmetics = keyValuePair.Value.Value;
						IUserCosmeticsCallback userCosmeticsCallback;
						if (!PlayerCosmeticsSystem.userCosmeticCallback.TryGetValue(PlayerCosmeticsSystem.playerActorNumberList[j], out userCosmeticsCallback))
						{
							PlayerCosmeticsSystem.userCosmeticsWaiting[PlayerCosmeticsSystem.playerActorNumberList[j]] = this.tempCosmetics;
						}
						else
						{
							userCosmeticsCallback.PendingUpdate = false;
							if (!userCosmeticsCallback.OnGetUserCosmetics(this.tempCosmetics))
							{
								PlayerCosmeticsSystem.playersToLookUp.Enqueue(player);
								userCosmeticsCallback.PendingUpdate = true;
							}
						}
					}
				}
			}, delegate(PlayFabError error)
			{
				if (error.Error == PlayFabErrorCode.NotAuthenticated)
				{
					PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					return;
				}
				if (error.Error == PlayFabErrorCode.AccountBanned)
				{
					GorillaGameManager.ForceStopGame_DisconnectAndDestroy();
				}
			}, null, null);
			yield return new WaitForSeconds(this.getSharedGroupDataCooldown);
			num = i;
		}
		this.isLookingUpNew = false;
		yield break;
	}

	// Token: 0x0600323E RID: 12862 RVA: 0x000F10C8 File Offset: 0x000EF2C8
	private void UpdatePlayersWaitingAndDoLookup(bool retrying)
	{
		if (PlayerCosmeticsSystem.playersWaiting.Count > 0)
		{
			Debug.Log("didn't recieve player cosmetics");
			for (int i = PlayerCosmeticsSystem.playersWaiting.Count - 1; i >= 0; i--)
			{
				int num = PlayerCosmeticsSystem.playersWaiting[i];
				if (!Utils.PlayerInRoom(num))
				{
					PlayerCosmeticsSystem.playersWaiting.RemoveAt(i);
				}
				else
				{
					Debug.Log("retrying to get player cosmetics for `actorNumber` " + num.ToString());
					PlayerCosmeticsSystem.playersToLookUp.Enqueue(NetworkSystem.Instance.GetPlayer(num));
					retrying = true;
				}
			}
		}
		if (retrying)
		{
			this.LookUpPlayerCosmetics(true);
		}
	}

	// Token: 0x0600323F RID: 12863 RVA: 0x000F115C File Offset: 0x000EF35C
	private void OnGetsharedGroupData(GetSharedGroupDataResult result)
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			PlayerCosmeticsSystem.playersWaiting.Clear();
			return;
		}
		bool retrying = false;
		foreach (KeyValuePair<string, PlayFab.ClientModels.SharedGroupDataRecord> keyValuePair in result.Data)
		{
			this.playerTemp = null;
			int num;
			if (int.TryParse(keyValuePair.Key, out num))
			{
				if (!Utils.PlayerInRoom(num))
				{
					PlayerCosmeticsSystem.playersWaiting.Remove(num);
				}
				else
				{
					PlayerCosmeticsSystem.playersWaiting.Remove(num);
					this.playerTemp = NetworkSystem.Instance.GetPlayer(num);
					this.tempCosmetics = keyValuePair.Value.Value;
					IUserCosmeticsCallback userCosmeticsCallback;
					if (!PlayerCosmeticsSystem.userCosmeticCallback.TryGetValue(num, out userCosmeticsCallback))
					{
						PlayerCosmeticsSystem.userCosmeticsWaiting[num] = this.tempCosmetics;
					}
					else
					{
						userCosmeticsCallback.PendingUpdate = false;
						if (!userCosmeticsCallback.OnGetUserCosmetics(this.tempCosmetics))
						{
							Debug.Log("retrying cosmetics for " + this.playerTemp.ToStringFull());
							PlayerCosmeticsSystem.playersToLookUp.Enqueue(this.playerTemp);
							retrying = true;
							userCosmeticsCallback.PendingUpdate = true;
						}
					}
				}
			}
		}
		this.UpdatePlayersWaitingAndDoLookup(retrying);
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x000F12A0 File Offset: 0x000EF4A0
	private void OnNetEvent(byte code, object data, int source)
	{
		if (code != 199 || source < 0)
		{
			return;
		}
		Debug.Log("OnNetEvent in Cosmetics called!!");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(source);
		GorillaNot.IncrementRPCCall(new PhotonMessageInfoWrapped(source, NetworkSystem.Instance.ServerTimestamp), "UpdatePlayerCosmetics");
		PlayerCosmeticsSystem.UpdatePlayerCosmetics(player);
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06003241 RID: 12865 RVA: 0x000F12EE File Offset: 0x000EF4EE
	private static bool nullInstance
	{
		get
		{
			return PlayerCosmeticsSystem.instance == null || !PlayerCosmeticsSystem.instance;
		}
	}

	// Token: 0x06003242 RID: 12866 RVA: 0x000F1308 File Offset: 0x000EF508
	public static void RegisterCosmeticCallback(int playerID, IUserCosmeticsCallback callback)
	{
		PlayerCosmeticsSystem.userCosmeticCallback[playerID] = callback;
		string cosmetics;
		if (PlayerCosmeticsSystem.userCosmeticsWaiting.TryGetValue(playerID, out cosmetics))
		{
			callback.PendingUpdate = false;
			callback.OnGetUserCosmetics(cosmetics);
			PlayerCosmeticsSystem.userCosmeticsWaiting.Remove(playerID);
		}
	}

	// Token: 0x06003243 RID: 12867 RVA: 0x000F134B File Offset: 0x000EF54B
	public static void RemoveCosmeticCallback(int playerID)
	{
		if (PlayerCosmeticsSystem.userCosmeticCallback.ContainsKey(playerID))
		{
			PlayerCosmeticsSystem.userCosmeticCallback.Remove(playerID);
		}
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x000F1368 File Offset: 0x000EF568
	public static void UpdatePlayerCosmetics(NetPlayer player)
	{
		if (player == null || player.IsLocal)
		{
			return;
		}
		PlayerCosmeticsSystem.playersToLookUp.Enqueue(player);
		IUserCosmeticsCallback userCosmeticsCallback;
		if (PlayerCosmeticsSystem.userCosmeticCallback.TryGetValue(player.ActorNumber, out userCosmeticsCallback))
		{
			userCosmeticsCallback.PendingUpdate = true;
		}
		if (!PlayerCosmeticsSystem.nullInstance)
		{
			PlayerCosmeticsSystem.instance.LookUpPlayerCosmetics(true);
		}
	}

	// Token: 0x06003245 RID: 12869 RVA: 0x000F13BC File Offset: 0x000EF5BC
	public static void UpdatePlayerCosmetics(List<NetPlayer> players)
	{
		foreach (NetPlayer netPlayer in players)
		{
			if (netPlayer != null && !netPlayer.IsLocal)
			{
				PlayerCosmeticsSystem.playersToLookUp.Enqueue(netPlayer);
				IUserCosmeticsCallback userCosmeticsCallback;
				if (PlayerCosmeticsSystem.userCosmeticCallback.TryGetValue(netPlayer.ActorNumber, out userCosmeticsCallback))
				{
					userCosmeticsCallback.PendingUpdate = true;
				}
			}
		}
		if (!PlayerCosmeticsSystem.nullInstance)
		{
			PlayerCosmeticsSystem.instance.LookUpPlayerCosmetics(false);
		}
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x000F1448 File Offset: 0x000EF648
	public static void SetRigTryOn(bool inTryon, RigContainer rigRefg)
	{
		VRRig rig = rigRefg.Rig;
		rig.inTryOnRoom = inTryon;
		if (inTryon)
		{
			if (PlayerCosmeticsSystem.sinceLastTryOnEvent.HasElapsed(0.5f, true))
			{
				GorillaTelemetry.PostShopEvent(rig, GTShopEventType.item_try_on, rig.tryOnSet.items);
			}
		}
		else if (rig.isOfflineVRRig)
		{
			rig.tryOnSet.ClearSet(CosmeticsController.instance.nullItem);
			CosmeticsController.instance.ClearCheckout(false);
			CosmeticsController.instance.UpdateShoppingCart();
			CosmeticsController.instance.UpdateWornCosmetics(true);
			rig.myBodyDockPositions.RefreshTransferrableItems();
			return;
		}
		rig.LocalUpdateCosmeticsWithTryon(rig.cosmeticSet, rig.tryOnSet);
		rig.myBodyDockPositions.RefreshTransferrableItems();
	}

	// Token: 0x06003247 RID: 12871 RVA: 0x000F14FA File Offset: 0x000EF6FA
	public static void StaticReset()
	{
		PlayerCosmeticsSystem.playersToLookUp.Clear();
		PlayerCosmeticsSystem.userCosmeticCallback.Clear();
		PlayerCosmeticsSystem.userCosmeticsWaiting.Clear();
		PlayerCosmeticsSystem.playerIDsList.Clear();
		PlayerCosmeticsSystem.playersWaiting.Clear();
	}

	// Token: 0x040035B5 RID: 13749
	public float playerLookUpCooldown = 3f;

	// Token: 0x040035B6 RID: 13750
	public float getSharedGroupDataCooldown = 0.1f;

	// Token: 0x040035B7 RID: 13751
	private float startSearchingTime = float.MinValue;

	// Token: 0x040035B8 RID: 13752
	private bool isLookingUp;

	// Token: 0x040035B9 RID: 13753
	private bool isLookingUpNew;

	// Token: 0x040035BA RID: 13754
	private string tempCosmetics;

	// Token: 0x040035BB RID: 13755
	private NetPlayer playerTemp;

	// Token: 0x040035BC RID: 13756
	private RigContainer tempRC;

	// Token: 0x040035BD RID: 13757
	private List<string> inventory;

	// Token: 0x040035BE RID: 13758
	private static PlayerCosmeticsSystem instance;

	// Token: 0x040035BF RID: 13759
	private static Queue<NetPlayer> playersToLookUp = new Queue<NetPlayer>(10);

	// Token: 0x040035C0 RID: 13760
	private static Dictionary<int, IUserCosmeticsCallback> userCosmeticCallback = new Dictionary<int, IUserCosmeticsCallback>(10);

	// Token: 0x040035C1 RID: 13761
	private static Dictionary<int, string> userCosmeticsWaiting = new Dictionary<int, string>(5);

	// Token: 0x040035C2 RID: 13762
	private static List<string> playerIDsList = new List<string>(10);

	// Token: 0x040035C3 RID: 13763
	private static List<int> playerActorNumberList = new List<int>(10);

	// Token: 0x040035C4 RID: 13764
	private static List<int> playersWaiting = new List<int>();

	// Token: 0x040035C5 RID: 13765
	private static TimeSince sinceLastTryOnEvent = 0f;
}
