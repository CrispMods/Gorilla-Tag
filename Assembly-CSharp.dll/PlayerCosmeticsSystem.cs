using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GorillaExtensions;
using GorillaNetworking;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

// Token: 0x020007F9 RID: 2041
internal class PlayerCosmeticsSystem : MonoBehaviour, ITickSystemPre
{
	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06003241 RID: 12865 RVA: 0x000505B8 File Offset: 0x0004E7B8
	// (set) Token: 0x06003242 RID: 12866 RVA: 0x000505C0 File Offset: 0x0004E7C0
	bool ITickSystemPre.PreTickRunning { get; set; }

	// Token: 0x06003243 RID: 12867 RVA: 0x00133E50 File Offset: 0x00132050
	private void Awake()
	{
		if (PlayerCosmeticsSystem.instance == null)
		{
			PlayerCosmeticsSystem.instance = this;
			base.transform.SetParent(null, true);
			UnityEngine.Object.DontDestroyOnLoad(this);
			this.inventory = new List<string>();
			this.inventory.Add("Inventory");
			NetworkSystem.Instance.OnRaiseEvent += this.OnNetEvent;
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x000505C9 File Offset: 0x0004E7C9
	private void Start()
	{
		this.playerLookUpCooldown = Mathf.Max(this.playerLookUpCooldown, 3f);
	}

	// Token: 0x06003245 RID: 12869 RVA: 0x000505E1 File Offset: 0x0004E7E1
	private void OnDestroy()
	{
		if (PlayerCosmeticsSystem.instance == this)
		{
			PlayerCosmeticsSystem.instance = null;
		}
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x000505F6 File Offset: 0x0004E7F6
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

	// Token: 0x06003247 RID: 12871 RVA: 0x00133EC0 File Offset: 0x001320C0
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

	// Token: 0x06003248 RID: 12872 RVA: 0x00050620 File Offset: 0x0004E820
	private void NewCosmeticsPath()
	{
		if (this.isLookingUpNew)
		{
			return;
		}
		base.StartCoroutine(this.NewCosmeticsPathCoroutine());
	}

	// Token: 0x06003249 RID: 12873 RVA: 0x00050638 File Offset: 0x0004E838
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

	// Token: 0x0600324A RID: 12874 RVA: 0x00134060 File Offset: 0x00132260
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

	// Token: 0x0600324B RID: 12875 RVA: 0x001340F4 File Offset: 0x001322F4
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

	// Token: 0x0600324C RID: 12876 RVA: 0x00134238 File Offset: 0x00132438
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

	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x0600324D RID: 12877 RVA: 0x00050647 File Offset: 0x0004E847
	private static bool nullInstance
	{
		get
		{
			return PlayerCosmeticsSystem.instance == null || !PlayerCosmeticsSystem.instance;
		}
	}

	// Token: 0x0600324E RID: 12878 RVA: 0x00134288 File Offset: 0x00132488
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

	// Token: 0x0600324F RID: 12879 RVA: 0x0005065F File Offset: 0x0004E85F
	public static void RemoveCosmeticCallback(int playerID)
	{
		if (PlayerCosmeticsSystem.userCosmeticCallback.ContainsKey(playerID))
		{
			PlayerCosmeticsSystem.userCosmeticCallback.Remove(playerID);
		}
	}

	// Token: 0x06003250 RID: 12880 RVA: 0x001342CC File Offset: 0x001324CC
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

	// Token: 0x06003251 RID: 12881 RVA: 0x00134320 File Offset: 0x00132520
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

	// Token: 0x06003252 RID: 12882 RVA: 0x001343AC File Offset: 0x001325AC
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

	// Token: 0x06003253 RID: 12883 RVA: 0x0005067A File Offset: 0x0004E87A
	public static void StaticReset()
	{
		PlayerCosmeticsSystem.playersToLookUp.Clear();
		PlayerCosmeticsSystem.userCosmeticCallback.Clear();
		PlayerCosmeticsSystem.userCosmeticsWaiting.Clear();
		PlayerCosmeticsSystem.playerIDsList.Clear();
		PlayerCosmeticsSystem.playersWaiting.Clear();
	}

	// Token: 0x040035C7 RID: 13767
	public float playerLookUpCooldown = 3f;

	// Token: 0x040035C8 RID: 13768
	public float getSharedGroupDataCooldown = 0.1f;

	// Token: 0x040035C9 RID: 13769
	private float startSearchingTime = float.MinValue;

	// Token: 0x040035CA RID: 13770
	private bool isLookingUp;

	// Token: 0x040035CB RID: 13771
	private bool isLookingUpNew;

	// Token: 0x040035CC RID: 13772
	private string tempCosmetics;

	// Token: 0x040035CD RID: 13773
	private NetPlayer playerTemp;

	// Token: 0x040035CE RID: 13774
	private RigContainer tempRC;

	// Token: 0x040035CF RID: 13775
	private List<string> inventory;

	// Token: 0x040035D0 RID: 13776
	private static PlayerCosmeticsSystem instance;

	// Token: 0x040035D1 RID: 13777
	private static Queue<NetPlayer> playersToLookUp = new Queue<NetPlayer>(10);

	// Token: 0x040035D2 RID: 13778
	private static Dictionary<int, IUserCosmeticsCallback> userCosmeticCallback = new Dictionary<int, IUserCosmeticsCallback>(10);

	// Token: 0x040035D3 RID: 13779
	private static Dictionary<int, string> userCosmeticsWaiting = new Dictionary<int, string>(5);

	// Token: 0x040035D4 RID: 13780
	private static List<string> playerIDsList = new List<string>(10);

	// Token: 0x040035D5 RID: 13781
	private static List<int> playerActorNumberList = new List<int>(10);

	// Token: 0x040035D6 RID: 13782
	private static List<int> playersWaiting = new List<int>();

	// Token: 0x040035D7 RID: 13783
	private static TimeSince sinceLastTryOnEvent = 0f;
}
