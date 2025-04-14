using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using GorillaNetworking;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x0200077C RID: 1916
public class FriendBackendController : MonoBehaviour
{
	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06002F01 RID: 12033 RVA: 0x000E39B8 File Offset: 0x000E1BB8
	// (remove) Token: 0x06002F02 RID: 12034 RVA: 0x000E39F0 File Offset: 0x000E1BF0
	public event Action<bool> OnGetFriendsComplete;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06002F03 RID: 12035 RVA: 0x000E3A28 File Offset: 0x000E1C28
	// (remove) Token: 0x06002F04 RID: 12036 RVA: 0x000E3A60 File Offset: 0x000E1C60
	public event Action<bool> OnSetPrivacyStateComplete;

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06002F05 RID: 12037 RVA: 0x000E3A98 File Offset: 0x000E1C98
	// (remove) Token: 0x06002F06 RID: 12038 RVA: 0x000E3AD0 File Offset: 0x000E1CD0
	public event Action<NetPlayer, bool> OnAddFriendComplete;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06002F07 RID: 12039 RVA: 0x000E3B08 File Offset: 0x000E1D08
	// (remove) Token: 0x06002F08 RID: 12040 RVA: 0x000E3B40 File Offset: 0x000E1D40
	public event Action<FriendBackendController.Friend, bool> OnRemoveFriendComplete;

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06002F09 RID: 12041 RVA: 0x000E3B75 File Offset: 0x000E1D75
	public List<FriendBackendController.Friend> FriendsList
	{
		get
		{
			return this.lastFriendsList;
		}
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06002F0A RID: 12042 RVA: 0x000E3B7D File Offset: 0x000E1D7D
	public FriendBackendController.PrivacyState MyPrivacyState
	{
		get
		{
			return this.lastPrivacyState;
		}
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x000E3B85 File Offset: 0x000E1D85
	public void GetFriends()
	{
		if (!this.getFriendsInProgress)
		{
			this.getFriendsInProgress = true;
			this.GetFriendsInternal();
		}
	}

	// Token: 0x06002F0C RID: 12044 RVA: 0x000E3B9C File Offset: 0x000E1D9C
	public void SetPrivacyState(FriendBackendController.PrivacyState state)
	{
		if (!this.setPrivacyStateInProgress)
		{
			this.setPrivacyStateInProgress = true;
			this.setPrivacyStateState = state;
			this.SetPrivacyStateInternal();
			return;
		}
		this.setPrivacyStateQueue.Enqueue(state);
	}

	// Token: 0x06002F0D RID: 12045 RVA: 0x000E3BC8 File Offset: 0x000E1DC8
	public void AddFriend(NetPlayer target)
	{
		if (target == null)
		{
			return;
		}
		int hashCode = target.UserId.GetHashCode();
		if (!this.addFriendInProgress)
		{
			this.addFriendInProgress = true;
			this.addFriendTargetIdHash = hashCode;
			this.addFriendTargetPlayer = target;
			this.AddFriendInternal();
			return;
		}
		if (hashCode != this.addFriendTargetIdHash && !this.addFriendRequestQueue.Contains(new ValueTuple<int, NetPlayer>(hashCode, target)))
		{
			this.addFriendRequestQueue.Enqueue(new ValueTuple<int, NetPlayer>(hashCode, target));
		}
	}

	// Token: 0x06002F0E RID: 12046 RVA: 0x000E3C38 File Offset: 0x000E1E38
	public void RemoveFriend(FriendBackendController.Friend target)
	{
		if (target == null)
		{
			return;
		}
		int hashCode = target.Presence.FriendLinkId.GetHashCode();
		if (!this.removeFriendInProgress)
		{
			this.removeFriendInProgress = true;
			this.removeFriendTargetIdHash = hashCode;
			this.removeFriendTarget = target;
			this.RemoveFriendInternal();
			return;
		}
		if (hashCode != this.addFriendTargetIdHash && !this.removeFriendRequestQueue.Contains(new ValueTuple<int, FriendBackendController.Friend>(hashCode, target)))
		{
			this.removeFriendRequestQueue.Enqueue(new ValueTuple<int, FriendBackendController.Friend>(hashCode, target));
		}
	}

	// Token: 0x06002F0F RID: 12047 RVA: 0x000E3CAD File Offset: 0x000E1EAD
	private void Awake()
	{
		if (FriendBackendController.Instance == null)
		{
			FriendBackendController.Instance = this;
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x06002F10 RID: 12048 RVA: 0x000E3CD0 File Offset: 0x000E1ED0
	private void GetFriendsInternal()
	{
		base.StartCoroutine(this.SendGetFriendsRequest(new FriendBackendController.GetFriendsRequest
		{
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			MothershipId = ""
		}, new Action<FriendBackendController.GetFriendsResponse>(this.GetFriendsComplete)));
	}

	// Token: 0x06002F11 RID: 12049 RVA: 0x000E3D2A File Offset: 0x000E1F2A
	private IEnumerator SendGetFriendsRequest(FriendBackendController.GetFriendsRequest data, Action<FriendBackendController.GetFriendsResponse> callback)
	{
		UnityWebRequest request = new UnityWebRequest(PlayFabAuthenticatorSettings.FriendApiBaseUrl + "/api/GetFriendsV2", "POST");
		byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		bool flag = false;
		if (request.result == UnityWebRequest.Result.Success)
		{
			FriendBackendController.GetFriendsResponse obj = JsonConvert.DeserializeObject<FriendBackendController.GetFriendsResponse>(request.downloadHandler.text);
			callback(obj);
		}
		else
		{
			long responseCode = request.responseCode;
			if (responseCode >= 500L && responseCode < 600L)
			{
				flag = true;
			}
			else if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (this.getFriendsRetryCount < this.maxRetriesOnFail)
			{
				int num = (int)Mathf.Pow(2f, (float)(this.getFriendsRetryCount + 1));
				this.getFriendsRetryCount++;
				yield return new WaitForSeconds((float)num);
				this.GetFriendsInternal();
			}
			else
			{
				GTDev.LogError<string>("Maximum GetFriends retries attempted. Please check your network connection.", null);
				this.getFriendsRetryCount = 0;
				callback(null);
			}
		}
		else
		{
			this.getFriendsInProgress = false;
		}
		yield break;
	}

	// Token: 0x06002F12 RID: 12050 RVA: 0x000E3D48 File Offset: 0x000E1F48
	private void GetFriendsComplete([CanBeNull] FriendBackendController.GetFriendsResponse response)
	{
		this.getFriendsInProgress = false;
		if (response != null)
		{
			this.lastGetFriendsResponse = response;
			if (this.lastGetFriendsResponse.Result != null)
			{
				this.lastPrivacyState = this.lastGetFriendsResponse.Result.MyPrivacyState;
				if (this.lastGetFriendsResponse.Result.Friends != null)
				{
					this.lastFriendsList.Clear();
					foreach (FriendBackendController.Friend item in this.lastGetFriendsResponse.Result.Friends)
					{
						this.lastFriendsList.Add(item);
					}
				}
			}
			Action<bool> onGetFriendsComplete = this.OnGetFriendsComplete;
			if (onGetFriendsComplete == null)
			{
				return;
			}
			onGetFriendsComplete(true);
			return;
		}
		else
		{
			Action<bool> onGetFriendsComplete2 = this.OnGetFriendsComplete;
			if (onGetFriendsComplete2 == null)
			{
				return;
			}
			onGetFriendsComplete2(false);
			return;
		}
	}

	// Token: 0x06002F13 RID: 12051 RVA: 0x000E3E24 File Offset: 0x000E2024
	private void SetPrivacyStateInternal()
	{
		base.StartCoroutine(this.SendSetPrivacyStateRequest(new FriendBackendController.SetPrivacyStateRequest
		{
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			PrivacyState = this.setPrivacyStateState.ToString()
		}, new Action<FriendBackendController.SetPrivacyStateResponse>(this.SetPrivacyStateComplete)));
	}

	// Token: 0x06002F14 RID: 12052 RVA: 0x000E3E8A File Offset: 0x000E208A
	private IEnumerator SendSetPrivacyStateRequest(FriendBackendController.SetPrivacyStateRequest data, Action<FriendBackendController.SetPrivacyStateResponse> callback)
	{
		UnityWebRequest request = new UnityWebRequest(PlayFabAuthenticatorSettings.FriendApiBaseUrl + "/api/SetPrivacyState", "POST");
		byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		bool flag = false;
		if (request.result == UnityWebRequest.Result.Success)
		{
			FriendBackendController.SetPrivacyStateResponse obj = JsonConvert.DeserializeObject<FriendBackendController.SetPrivacyStateResponse>(request.downloadHandler.text);
			callback(obj);
		}
		else
		{
			long responseCode = request.responseCode;
			if (responseCode >= 500L && responseCode < 600L)
			{
				flag = true;
			}
			else if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (this.setPrivacyStateRetryCount < this.maxRetriesOnFail)
			{
				int num = (int)Mathf.Pow(2f, (float)(this.setPrivacyStateRetryCount + 1));
				this.setPrivacyStateRetryCount++;
				yield return new WaitForSeconds((float)num);
				this.SetPrivacyStateInternal();
			}
			else
			{
				GTDev.LogError<string>("Maximum SetPrivacyState retries attempted. Please check your network connection.", null);
				this.setPrivacyStateRetryCount = 0;
				callback(null);
			}
		}
		else
		{
			this.setPrivacyStateInProgress = false;
		}
		yield break;
	}

	// Token: 0x06002F15 RID: 12053 RVA: 0x000E3EA8 File Offset: 0x000E20A8
	private void SetPrivacyStateComplete([CanBeNull] FriendBackendController.SetPrivacyStateResponse response)
	{
		this.setPrivacyStateInProgress = false;
		if (response != null)
		{
			this.lastPrivacyStateResponse = response;
			Action<bool> onSetPrivacyStateComplete = this.OnSetPrivacyStateComplete;
			if (onSetPrivacyStateComplete != null)
			{
				onSetPrivacyStateComplete(true);
			}
		}
		else
		{
			Action<bool> onSetPrivacyStateComplete2 = this.OnSetPrivacyStateComplete;
			if (onSetPrivacyStateComplete2 != null)
			{
				onSetPrivacyStateComplete2(false);
			}
		}
		if (this.setPrivacyStateQueue.Count > 0)
		{
			FriendBackendController.PrivacyState privacyState = this.setPrivacyStateQueue.Dequeue();
			this.SetPrivacyState(privacyState);
		}
	}

	// Token: 0x06002F16 RID: 12054 RVA: 0x000E3F10 File Offset: 0x000E2110
	private void AddFriendInternal()
	{
		base.StartCoroutine(this.SendAddFriendRequest(new FriendBackendController.FriendRequestRequest
		{
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			MothershipId = "",
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			MothershipToken = "",
			MyFriendLinkId = NetworkSystem.Instance.LocalPlayer.UserId,
			FriendFriendLinkId = this.addFriendTargetPlayer.UserId
		}, new Action<bool>(this.AddFriendComplete)));
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x000E3F9B File Offset: 0x000E219B
	private IEnumerator SendAddFriendRequest(FriendBackendController.FriendRequestRequest data, Action<bool> callback)
	{
		UnityWebRequest request = new UnityWebRequest(PlayFabAuthenticatorSettings.FriendApiBaseUrl + "/api/RequestFriend", "POST");
		byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		bool flag = false;
		if (request.result == UnityWebRequest.Result.Success)
		{
			callback(true);
		}
		else
		{
			if (request.responseCode == 409L)
			{
				flag = false;
			}
			long responseCode = request.responseCode;
			if (responseCode >= 500L && responseCode < 600L)
			{
				flag = true;
			}
			else if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (this.addFriendRetryCount < this.maxRetriesOnFail)
			{
				int num = (int)Mathf.Pow(2f, (float)(this.addFriendRetryCount + 1));
				this.addFriendRetryCount++;
				yield return new WaitForSeconds((float)num);
				this.AddFriendInternal();
			}
			else
			{
				GTDev.LogError<string>("Maximum AddFriend retries attempted. Please check your network connection.", null);
				this.addFriendRetryCount = 0;
				callback(false);
			}
		}
		else
		{
			this.addFriendInProgress = false;
		}
		yield break;
	}

	// Token: 0x06002F18 RID: 12056 RVA: 0x000E3FB8 File Offset: 0x000E21B8
	private void AddFriendComplete([CanBeNull] bool success)
	{
		if (success)
		{
			Action<NetPlayer, bool> onAddFriendComplete = this.OnAddFriendComplete;
			if (onAddFriendComplete != null)
			{
				onAddFriendComplete(this.addFriendTargetPlayer, true);
			}
		}
		else
		{
			Action<NetPlayer, bool> onAddFriendComplete2 = this.OnAddFriendComplete;
			if (onAddFriendComplete2 != null)
			{
				onAddFriendComplete2(this.addFriendTargetPlayer, false);
			}
		}
		this.addFriendInProgress = false;
		this.addFriendTargetIdHash = 0;
		this.addFriendTargetPlayer = null;
		if (this.addFriendRequestQueue.Count > 0)
		{
			ValueTuple<int, NetPlayer> valueTuple = this.addFriendRequestQueue.Dequeue();
			this.AddFriend(valueTuple.Item2);
		}
	}

	// Token: 0x06002F19 RID: 12057 RVA: 0x000E4038 File Offset: 0x000E2238
	private void RemoveFriendInternal()
	{
		base.StartCoroutine(this.SendRemoveFriendRequest(new FriendBackendController.RemoveFriendRequest
		{
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			MothershipId = "",
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			MyFriendLinkId = NetworkSystem.Instance.LocalPlayer.UserId,
			FriendFriendLinkId = this.removeFriendTarget.Presence.FriendLinkId
		}, new Action<bool>(this.RemoveFriendComplete)));
	}

	// Token: 0x06002F1A RID: 12058 RVA: 0x000E40BD File Offset: 0x000E22BD
	private IEnumerator SendRemoveFriendRequest(FriendBackendController.RemoveFriendRequest data, Action<bool> callback)
	{
		UnityWebRequest request = new UnityWebRequest(PlayFabAuthenticatorSettings.FriendApiBaseUrl + "/api/RemoveFriend", "POST");
		byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		bool flag = false;
		if (request.result == UnityWebRequest.Result.Success)
		{
			callback(true);
		}
		else
		{
			long responseCode = request.responseCode;
			if (responseCode >= 500L && responseCode < 600L)
			{
				flag = true;
			}
			else if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (this.removeFriendRetryCount < this.maxRetriesOnFail)
			{
				int num = (int)Mathf.Pow(2f, (float)(this.removeFriendRetryCount + 1));
				this.removeFriendRetryCount++;
				yield return new WaitForSeconds((float)num);
				this.AddFriendInternal();
			}
			else
			{
				GTDev.LogError<string>("Maximum AddFriend retries attempted. Please check your network connection.", null);
				this.removeFriendRetryCount = 0;
				callback(false);
			}
		}
		else
		{
			this.removeFriendInProgress = false;
		}
		yield break;
	}

	// Token: 0x06002F1B RID: 12059 RVA: 0x000E40DC File Offset: 0x000E22DC
	private void RemoveFriendComplete([CanBeNull] bool success)
	{
		if (success)
		{
			Action<FriendBackendController.Friend, bool> onRemoveFriendComplete = this.OnRemoveFriendComplete;
			if (onRemoveFriendComplete != null)
			{
				onRemoveFriendComplete(this.removeFriendTarget, true);
			}
		}
		else
		{
			Action<FriendBackendController.Friend, bool> onRemoveFriendComplete2 = this.OnRemoveFriendComplete;
			if (onRemoveFriendComplete2 != null)
			{
				onRemoveFriendComplete2(this.removeFriendTarget, false);
			}
		}
		this.removeFriendInProgress = false;
		this.removeFriendTargetIdHash = 0;
		this.removeFriendTarget = null;
		if (this.removeFriendRequestQueue.Count > 0)
		{
			ValueTuple<int, FriendBackendController.Friend> valueTuple = this.removeFriendRequestQueue.Dequeue();
			this.RemoveFriend(valueTuple.Item2);
		}
	}

	// Token: 0x06002F1C RID: 12060 RVA: 0x000E415C File Offset: 0x000E235C
	private void LogNetPlayersInRoom()
	{
		Debug.Log("Local Player PlayfabId: " + PlayFabAuthenticator.instance.GetPlayFabPlayerId());
		int num = 0;
		foreach (NetPlayer netPlayer in NetworkSystem.Instance.AllNetPlayers)
		{
			Debug.Log(string.Format("[{0}] Player: {1}, ActorNumber: {2}, UserID: {3}, IsMasterClient: {4}", new object[]
			{
				num,
				netPlayer.NickName,
				netPlayer.ActorNumber,
				netPlayer.UserId,
				netPlayer.IsMasterClient
			}));
			num++;
		}
	}

	// Token: 0x06002F1D RID: 12061 RVA: 0x000E41F4 File Offset: 0x000E23F4
	private void TestAddFriend()
	{
		this.OnAddFriendComplete -= this.TestAddFriendCompleteCallback;
		this.OnAddFriendComplete += this.TestAddFriendCompleteCallback;
		NetPlayer target = null;
		if (this.netPlayerIndexToAddFriend >= 0 && this.netPlayerIndexToAddFriend < NetworkSystem.Instance.AllNetPlayers.Length)
		{
			target = NetworkSystem.Instance.AllNetPlayers[this.netPlayerIndexToAddFriend];
		}
		this.AddFriend(target);
	}

	// Token: 0x06002F1E RID: 12062 RVA: 0x000E425D File Offset: 0x000E245D
	private void TestAddFriendCompleteCallback(NetPlayer player, bool success)
	{
		if (success)
		{
			Debug.Log("FriendBackend: TestAddFriendCompleteCallback returned with success = true");
			return;
		}
		Debug.Log("FriendBackend: TestAddFriendCompleteCallback returned with success = false");
	}

	// Token: 0x06002F1F RID: 12063 RVA: 0x000E4278 File Offset: 0x000E2478
	private void TestRemoveFriend()
	{
		this.OnRemoveFriendComplete -= this.TestRemoveFriendCompleteCallback;
		this.OnRemoveFriendComplete += this.TestRemoveFriendCompleteCallback;
		FriendBackendController.Friend target = null;
		if (this.friendListIndexToRemoveFriend >= 0 && this.friendListIndexToRemoveFriend < this.FriendsList.Count)
		{
			target = this.FriendsList[this.friendListIndexToRemoveFriend];
		}
		this.RemoveFriend(target);
	}

	// Token: 0x06002F20 RID: 12064 RVA: 0x000E42E0 File Offset: 0x000E24E0
	private void TestRemoveFriendCompleteCallback(FriendBackendController.Friend friend, bool success)
	{
		if (success)
		{
			Debug.Log("FriendBackend: TestRemoveFriendCompleteCallback returned with success = true");
			return;
		}
		Debug.Log("FriendBackend: TestRemoveFriendCompleteCallback returned with success = false");
	}

	// Token: 0x06002F21 RID: 12065 RVA: 0x000E42FA File Offset: 0x000E24FA
	private void TestGetFriends()
	{
		this.OnGetFriendsComplete -= this.TestGetFriendsCompleteCallback;
		this.OnGetFriendsComplete += this.TestGetFriendsCompleteCallback;
		this.GetFriends();
	}

	// Token: 0x06002F22 RID: 12066 RVA: 0x000E4328 File Offset: 0x000E2528
	private void TestGetFriendsCompleteCallback(bool success)
	{
		if (success)
		{
			Debug.Log("FriendBackend: TestGetFriendsCompleteCallback returned with success = true");
			if (this.FriendsList != null)
			{
				string text = string.Format("Friend Count: {0} Friends: \n", this.FriendsList.Count);
				for (int i = 0; i < this.FriendsList.Count; i++)
				{
					if (this.FriendsList[i] != null && this.FriendsList[i].Presence != null)
					{
						text = string.Concat(new string[]
						{
							text,
							this.FriendsList[i].Presence.UserName,
							", ",
							this.FriendsList[i].Presence.FriendLinkId,
							", ",
							this.FriendsList[i].Presence.RoomId,
							", ",
							this.FriendsList[i].Presence.Region,
							", ",
							this.FriendsList[i].Presence.Zone,
							"\n"
						});
					}
					else
					{
						text += "null friend\n";
					}
				}
				Debug.Log(text);
				return;
			}
		}
		else
		{
			Debug.Log("FriendBackend: TestGetFriendsCompleteCallback returned with success = false");
		}
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x000E4485 File Offset: 0x000E2685
	private void TestSetPrivacyState()
	{
		this.OnSetPrivacyStateComplete -= this.TestSetPrivacyStateCompleteCallback;
		this.OnSetPrivacyStateComplete += this.TestSetPrivacyStateCompleteCallback;
		this.SetPrivacyState(this.privacyStateToSet);
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x000E44B8 File Offset: 0x000E26B8
	private void TestSetPrivacyStateCompleteCallback(bool success)
	{
		if (success)
		{
			Debug.Log(string.Format("SetPrivacyState Success: Status: {0} Error: {1}", this.lastPrivacyStateResponse.StatusCode, this.lastPrivacyStateResponse.Error));
			return;
		}
		Debug.Log(string.Format("SetPrivacyState Failed: Status: {0} Error: {1}", this.lastPrivacyStateResponse.StatusCode, this.lastPrivacyStateResponse.Error));
	}

	// Token: 0x0400333C RID: 13116
	[OnEnterPlay_SetNull]
	public static volatile FriendBackendController Instance;

	// Token: 0x04003341 RID: 13121
	private int maxRetriesOnFail = 3;

	// Token: 0x04003342 RID: 13122
	private int getFriendsRetryCount;

	// Token: 0x04003343 RID: 13123
	private int setPrivacyStateRetryCount;

	// Token: 0x04003344 RID: 13124
	private int addFriendRetryCount;

	// Token: 0x04003345 RID: 13125
	private int removeFriendRetryCount;

	// Token: 0x04003346 RID: 13126
	private bool getFriendsInProgress;

	// Token: 0x04003347 RID: 13127
	private FriendBackendController.GetFriendsResponse lastGetFriendsResponse;

	// Token: 0x04003348 RID: 13128
	private List<FriendBackendController.Friend> lastFriendsList = new List<FriendBackendController.Friend>();

	// Token: 0x04003349 RID: 13129
	private bool setPrivacyStateInProgress;

	// Token: 0x0400334A RID: 13130
	private FriendBackendController.PrivacyState setPrivacyStateState;

	// Token: 0x0400334B RID: 13131
	private FriendBackendController.SetPrivacyStateResponse lastPrivacyStateResponse;

	// Token: 0x0400334C RID: 13132
	private Queue<FriendBackendController.PrivacyState> setPrivacyStateQueue = new Queue<FriendBackendController.PrivacyState>();

	// Token: 0x0400334D RID: 13133
	private FriendBackendController.PrivacyState lastPrivacyState;

	// Token: 0x0400334E RID: 13134
	private bool addFriendInProgress;

	// Token: 0x0400334F RID: 13135
	private int addFriendTargetIdHash;

	// Token: 0x04003350 RID: 13136
	private NetPlayer addFriendTargetPlayer;

	// Token: 0x04003351 RID: 13137
	private Queue<ValueTuple<int, NetPlayer>> addFriendRequestQueue = new Queue<ValueTuple<int, NetPlayer>>();

	// Token: 0x04003352 RID: 13138
	private bool removeFriendInProgress;

	// Token: 0x04003353 RID: 13139
	private int removeFriendTargetIdHash;

	// Token: 0x04003354 RID: 13140
	private FriendBackendController.Friend removeFriendTarget;

	// Token: 0x04003355 RID: 13141
	private Queue<ValueTuple<int, FriendBackendController.Friend>> removeFriendRequestQueue = new Queue<ValueTuple<int, FriendBackendController.Friend>>();

	// Token: 0x04003356 RID: 13142
	[SerializeField]
	private int netPlayerIndexToAddFriend;

	// Token: 0x04003357 RID: 13143
	[SerializeField]
	private int friendListIndexToRemoveFriend;

	// Token: 0x04003358 RID: 13144
	[SerializeField]
	private FriendBackendController.PrivacyState privacyStateToSet;

	// Token: 0x0200077D RID: 1917
	public class Friend
	{
		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06002F26 RID: 12070 RVA: 0x000E4558 File Offset: 0x000E2758
		// (set) Token: 0x06002F27 RID: 12071 RVA: 0x000E4560 File Offset: 0x000E2760
		public FriendBackendController.FriendPresence Presence { get; set; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06002F28 RID: 12072 RVA: 0x000E4569 File Offset: 0x000E2769
		// (set) Token: 0x06002F29 RID: 12073 RVA: 0x000E4571 File Offset: 0x000E2771
		public DateTime Created { get; set; }
	}

	// Token: 0x0200077E RID: 1918
	public class FriendPresence
	{
		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06002F2B RID: 12075 RVA: 0x000E457A File Offset: 0x000E277A
		// (set) Token: 0x06002F2C RID: 12076 RVA: 0x000E4582 File Offset: 0x000E2782
		public string FriendLinkId { get; set; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06002F2D RID: 12077 RVA: 0x000E458B File Offset: 0x000E278B
		// (set) Token: 0x06002F2E RID: 12078 RVA: 0x000E4593 File Offset: 0x000E2793
		public string UserName { get; set; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06002F2F RID: 12079 RVA: 0x000E459C File Offset: 0x000E279C
		// (set) Token: 0x06002F30 RID: 12080 RVA: 0x000E45A4 File Offset: 0x000E27A4
		public string RoomId { get; set; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06002F31 RID: 12081 RVA: 0x000E45AD File Offset: 0x000E27AD
		// (set) Token: 0x06002F32 RID: 12082 RVA: 0x000E45B5 File Offset: 0x000E27B5
		public string Zone { get; set; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06002F33 RID: 12083 RVA: 0x000E45BE File Offset: 0x000E27BE
		// (set) Token: 0x06002F34 RID: 12084 RVA: 0x000E45C6 File Offset: 0x000E27C6
		public string Region { get; set; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06002F35 RID: 12085 RVA: 0x000E45CF File Offset: 0x000E27CF
		// (set) Token: 0x06002F36 RID: 12086 RVA: 0x000E45D7 File Offset: 0x000E27D7
		public bool? IsPublic { get; set; }
	}

	// Token: 0x0200077F RID: 1919
	public class FriendLink
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06002F38 RID: 12088 RVA: 0x000E45E0 File Offset: 0x000E27E0
		// (set) Token: 0x06002F39 RID: 12089 RVA: 0x000E45E8 File Offset: 0x000E27E8
		public string my_playfab_id { get; set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06002F3A RID: 12090 RVA: 0x000E45F1 File Offset: 0x000E27F1
		// (set) Token: 0x06002F3B RID: 12091 RVA: 0x000E45F9 File Offset: 0x000E27F9
		public string my_mothership_id { get; set; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06002F3C RID: 12092 RVA: 0x000E4602 File Offset: 0x000E2802
		// (set) Token: 0x06002F3D RID: 12093 RVA: 0x000E460A File Offset: 0x000E280A
		public string my_friendlink_id { get; set; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06002F3E RID: 12094 RVA: 0x000E4613 File Offset: 0x000E2813
		// (set) Token: 0x06002F3F RID: 12095 RVA: 0x000E461B File Offset: 0x000E281B
		public string friend_playfab_id { get; set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06002F40 RID: 12096 RVA: 0x000E4624 File Offset: 0x000E2824
		// (set) Token: 0x06002F41 RID: 12097 RVA: 0x000E462C File Offset: 0x000E282C
		public string friend_mothership_id { get; set; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06002F42 RID: 12098 RVA: 0x000E4635 File Offset: 0x000E2835
		// (set) Token: 0x06002F43 RID: 12099 RVA: 0x000E463D File Offset: 0x000E283D
		public string friend_friendlink_id { get; set; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06002F44 RID: 12100 RVA: 0x000E4646 File Offset: 0x000E2846
		// (set) Token: 0x06002F45 RID: 12101 RVA: 0x000E464E File Offset: 0x000E284E
		public DateTime created { get; set; }
	}

	// Token: 0x02000780 RID: 1920
	[NullableContext(2)]
	[Nullable(0)]
	public class FriendIdResponse
	{
		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06002F47 RID: 12103 RVA: 0x000E4657 File Offset: 0x000E2857
		// (set) Token: 0x06002F48 RID: 12104 RVA: 0x000E465F File Offset: 0x000E285F
		public string PlayFabId { get; set; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06002F49 RID: 12105 RVA: 0x000E4668 File Offset: 0x000E2868
		// (set) Token: 0x06002F4A RID: 12106 RVA: 0x000E4670 File Offset: 0x000E2870
		public string MothershipId { get; set; } = "";
	}

	// Token: 0x02000781 RID: 1921
	public class FriendRequestRequest
	{
		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06002F4C RID: 12108 RVA: 0x000E468C File Offset: 0x000E288C
		// (set) Token: 0x06002F4D RID: 12109 RVA: 0x000E4694 File Offset: 0x000E2894
		public string PlayFabId { get; set; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06002F4E RID: 12110 RVA: 0x000E469D File Offset: 0x000E289D
		// (set) Token: 0x06002F4F RID: 12111 RVA: 0x000E46A5 File Offset: 0x000E28A5
		public string MothershipId { get; set; } = "";

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06002F50 RID: 12112 RVA: 0x000E46AE File Offset: 0x000E28AE
		// (set) Token: 0x06002F51 RID: 12113 RVA: 0x000E46B6 File Offset: 0x000E28B6
		public string PlayFabTicket { get; set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06002F52 RID: 12114 RVA: 0x000E46BF File Offset: 0x000E28BF
		// (set) Token: 0x06002F53 RID: 12115 RVA: 0x000E46C7 File Offset: 0x000E28C7
		public string MothershipToken { get; set; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06002F54 RID: 12116 RVA: 0x000E46D0 File Offset: 0x000E28D0
		// (set) Token: 0x06002F55 RID: 12117 RVA: 0x000E46D8 File Offset: 0x000E28D8
		public string MyFriendLinkId { get; set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06002F56 RID: 12118 RVA: 0x000E46E1 File Offset: 0x000E28E1
		// (set) Token: 0x06002F57 RID: 12119 RVA: 0x000E46E9 File Offset: 0x000E28E9
		public string FriendFriendLinkId { get; set; }
	}

	// Token: 0x02000782 RID: 1922
	public class GetFriendsRequest
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06002F59 RID: 12121 RVA: 0x000E4705 File Offset: 0x000E2905
		// (set) Token: 0x06002F5A RID: 12122 RVA: 0x000E470D File Offset: 0x000E290D
		public string PlayFabId { get; set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06002F5B RID: 12123 RVA: 0x000E4716 File Offset: 0x000E2916
		// (set) Token: 0x06002F5C RID: 12124 RVA: 0x000E471E File Offset: 0x000E291E
		public string MothershipId { get; set; } = "";

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06002F5D RID: 12125 RVA: 0x000E4727 File Offset: 0x000E2927
		// (set) Token: 0x06002F5E RID: 12126 RVA: 0x000E472F File Offset: 0x000E292F
		public string MothershipToken { get; set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06002F5F RID: 12127 RVA: 0x000E4738 File Offset: 0x000E2938
		// (set) Token: 0x06002F60 RID: 12128 RVA: 0x000E4740 File Offset: 0x000E2940
		public string PlayFabTicket { get; set; }
	}

	// Token: 0x02000783 RID: 1923
	public class GetFriendsResponse
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06002F62 RID: 12130 RVA: 0x000E475C File Offset: 0x000E295C
		// (set) Token: 0x06002F63 RID: 12131 RVA: 0x000E4764 File Offset: 0x000E2964
		[CanBeNull]
		public FriendBackendController.GetFriendsResult Result { get; set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06002F64 RID: 12132 RVA: 0x000E476D File Offset: 0x000E296D
		// (set) Token: 0x06002F65 RID: 12133 RVA: 0x000E4775 File Offset: 0x000E2975
		public int StatusCode { get; set; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06002F66 RID: 12134 RVA: 0x000E477E File Offset: 0x000E297E
		// (set) Token: 0x06002F67 RID: 12135 RVA: 0x000E4786 File Offset: 0x000E2986
		[Nullable(2)]
		public string Error { [NullableContext(2)] get; [NullableContext(2)] set; }
	}

	// Token: 0x02000784 RID: 1924
	public class GetFriendsResult
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06002F69 RID: 12137 RVA: 0x000E478F File Offset: 0x000E298F
		// (set) Token: 0x06002F6A RID: 12138 RVA: 0x000E4797 File Offset: 0x000E2997
		public List<FriendBackendController.Friend> Friends { get; set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002F6B RID: 12139 RVA: 0x000E47A0 File Offset: 0x000E29A0
		// (set) Token: 0x06002F6C RID: 12140 RVA: 0x000E47A8 File Offset: 0x000E29A8
		public FriendBackendController.PrivacyState MyPrivacyState { get; set; }
	}

	// Token: 0x02000785 RID: 1925
	public class SetPrivacyStateRequest
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06002F6E RID: 12142 RVA: 0x000E47B1 File Offset: 0x000E29B1
		// (set) Token: 0x06002F6F RID: 12143 RVA: 0x000E47B9 File Offset: 0x000E29B9
		public string PlayFabId { get; set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06002F70 RID: 12144 RVA: 0x000E47C2 File Offset: 0x000E29C2
		// (set) Token: 0x06002F71 RID: 12145 RVA: 0x000E47CA File Offset: 0x000E29CA
		public string PlayFabTicket { get; set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06002F72 RID: 12146 RVA: 0x000E47D3 File Offset: 0x000E29D3
		// (set) Token: 0x06002F73 RID: 12147 RVA: 0x000E47DB File Offset: 0x000E29DB
		public string PrivacyState { get; set; }
	}

	// Token: 0x02000786 RID: 1926
	[NullableContext(2)]
	[Nullable(0)]
	public class SetPrivacyStateResponse
	{
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06002F75 RID: 12149 RVA: 0x000E47E4 File Offset: 0x000E29E4
		// (set) Token: 0x06002F76 RID: 12150 RVA: 0x000E47EC File Offset: 0x000E29EC
		public int StatusCode { get; set; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06002F77 RID: 12151 RVA: 0x000E47F5 File Offset: 0x000E29F5
		// (set) Token: 0x06002F78 RID: 12152 RVA: 0x000E47FD File Offset: 0x000E29FD
		public string Error { get; set; }
	}

	// Token: 0x02000787 RID: 1927
	public class RemoveFriendRequest
	{
		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06002F7A RID: 12154 RVA: 0x000E4806 File Offset: 0x000E2A06
		// (set) Token: 0x06002F7B RID: 12155 RVA: 0x000E480E File Offset: 0x000E2A0E
		public string PlayFabId { get; set; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06002F7C RID: 12156 RVA: 0x000E4817 File Offset: 0x000E2A17
		// (set) Token: 0x06002F7D RID: 12157 RVA: 0x000E481F File Offset: 0x000E2A1F
		public string MothershipId { get; set; } = "";

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06002F7E RID: 12158 RVA: 0x000E4828 File Offset: 0x000E2A28
		// (set) Token: 0x06002F7F RID: 12159 RVA: 0x000E4830 File Offset: 0x000E2A30
		public string PlayFabTicket { get; set; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06002F80 RID: 12160 RVA: 0x000E4839 File Offset: 0x000E2A39
		// (set) Token: 0x06002F81 RID: 12161 RVA: 0x000E4841 File Offset: 0x000E2A41
		public string MothershipToken { get; set; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06002F82 RID: 12162 RVA: 0x000E484A File Offset: 0x000E2A4A
		// (set) Token: 0x06002F83 RID: 12163 RVA: 0x000E4852 File Offset: 0x000E2A52
		public string MyFriendLinkId { get; set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06002F84 RID: 12164 RVA: 0x000E485B File Offset: 0x000E2A5B
		// (set) Token: 0x06002F85 RID: 12165 RVA: 0x000E4863 File Offset: 0x000E2A63
		public string FriendFriendLinkId { get; set; }
	}

	// Token: 0x02000788 RID: 1928
	public enum PendingRequestStatus
	{
		// Token: 0x04003385 RID: 13189
		I_REQUESTED,
		// Token: 0x04003386 RID: 13190
		THEY_REQUESTED,
		// Token: 0x04003387 RID: 13191
		CONFIRMED,
		// Token: 0x04003388 RID: 13192
		NOT_FOUND
	}

	// Token: 0x02000789 RID: 1929
	public enum PrivacyState
	{
		// Token: 0x0400338A RID: 13194
		VISIBLE,
		// Token: 0x0400338B RID: 13195
		PUBLIC_ONLY,
		// Token: 0x0400338C RID: 13196
		HIDDEN
	}
}
