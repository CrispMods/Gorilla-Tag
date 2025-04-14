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

// Token: 0x0200077B RID: 1915
public class FriendBackendController : MonoBehaviour
{
	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06002EF9 RID: 12025 RVA: 0x000E3538 File Offset: 0x000E1738
	// (remove) Token: 0x06002EFA RID: 12026 RVA: 0x000E3570 File Offset: 0x000E1770
	public event Action<bool> OnGetFriendsComplete;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06002EFB RID: 12027 RVA: 0x000E35A8 File Offset: 0x000E17A8
	// (remove) Token: 0x06002EFC RID: 12028 RVA: 0x000E35E0 File Offset: 0x000E17E0
	public event Action<bool> OnSetPrivacyStateComplete;

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06002EFD RID: 12029 RVA: 0x000E3618 File Offset: 0x000E1818
	// (remove) Token: 0x06002EFE RID: 12030 RVA: 0x000E3650 File Offset: 0x000E1850
	public event Action<NetPlayer, bool> OnAddFriendComplete;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06002EFF RID: 12031 RVA: 0x000E3688 File Offset: 0x000E1888
	// (remove) Token: 0x06002F00 RID: 12032 RVA: 0x000E36C0 File Offset: 0x000E18C0
	public event Action<FriendBackendController.Friend, bool> OnRemoveFriendComplete;

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x06002F01 RID: 12033 RVA: 0x000E36F5 File Offset: 0x000E18F5
	public List<FriendBackendController.Friend> FriendsList
	{
		get
		{
			return this.lastFriendsList;
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06002F02 RID: 12034 RVA: 0x000E36FD File Offset: 0x000E18FD
	public FriendBackendController.PrivacyState MyPrivacyState
	{
		get
		{
			return this.lastPrivacyState;
		}
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x000E3705 File Offset: 0x000E1905
	public void GetFriends()
	{
		if (!this.getFriendsInProgress)
		{
			this.getFriendsInProgress = true;
			this.GetFriendsInternal();
		}
	}

	// Token: 0x06002F04 RID: 12036 RVA: 0x000E371C File Offset: 0x000E191C
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

	// Token: 0x06002F05 RID: 12037 RVA: 0x000E3748 File Offset: 0x000E1948
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

	// Token: 0x06002F06 RID: 12038 RVA: 0x000E37B8 File Offset: 0x000E19B8
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

	// Token: 0x06002F07 RID: 12039 RVA: 0x000E382D File Offset: 0x000E1A2D
	private void Awake()
	{
		if (FriendBackendController.Instance == null)
		{
			FriendBackendController.Instance = this;
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x06002F08 RID: 12040 RVA: 0x000E3850 File Offset: 0x000E1A50
	private void GetFriendsInternal()
	{
		base.StartCoroutine(this.SendGetFriendsRequest(new FriendBackendController.GetFriendsRequest
		{
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			MothershipId = ""
		}, new Action<FriendBackendController.GetFriendsResponse>(this.GetFriendsComplete)));
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x000E38AA File Offset: 0x000E1AAA
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

	// Token: 0x06002F0A RID: 12042 RVA: 0x000E38C8 File Offset: 0x000E1AC8
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

	// Token: 0x06002F0B RID: 12043 RVA: 0x000E39A4 File Offset: 0x000E1BA4
	private void SetPrivacyStateInternal()
	{
		base.StartCoroutine(this.SendSetPrivacyStateRequest(new FriendBackendController.SetPrivacyStateRequest
		{
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			PrivacyState = this.setPrivacyStateState.ToString()
		}, new Action<FriendBackendController.SetPrivacyStateResponse>(this.SetPrivacyStateComplete)));
	}

	// Token: 0x06002F0C RID: 12044 RVA: 0x000E3A0A File Offset: 0x000E1C0A
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

	// Token: 0x06002F0D RID: 12045 RVA: 0x000E3A28 File Offset: 0x000E1C28
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

	// Token: 0x06002F0E RID: 12046 RVA: 0x000E3A90 File Offset: 0x000E1C90
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

	// Token: 0x06002F0F RID: 12047 RVA: 0x000E3B1B File Offset: 0x000E1D1B
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

	// Token: 0x06002F10 RID: 12048 RVA: 0x000E3B38 File Offset: 0x000E1D38
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

	// Token: 0x06002F11 RID: 12049 RVA: 0x000E3BB8 File Offset: 0x000E1DB8
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

	// Token: 0x06002F12 RID: 12050 RVA: 0x000E3C3D File Offset: 0x000E1E3D
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

	// Token: 0x06002F13 RID: 12051 RVA: 0x000E3C5C File Offset: 0x000E1E5C
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

	// Token: 0x06002F14 RID: 12052 RVA: 0x000E3CDC File Offset: 0x000E1EDC
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

	// Token: 0x06002F15 RID: 12053 RVA: 0x000E3D74 File Offset: 0x000E1F74
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

	// Token: 0x06002F16 RID: 12054 RVA: 0x000E3DDD File Offset: 0x000E1FDD
	private void TestAddFriendCompleteCallback(NetPlayer player, bool success)
	{
		if (success)
		{
			Debug.Log("FriendBackend: TestAddFriendCompleteCallback returned with success = true");
			return;
		}
		Debug.Log("FriendBackend: TestAddFriendCompleteCallback returned with success = false");
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x000E3DF8 File Offset: 0x000E1FF8
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

	// Token: 0x06002F18 RID: 12056 RVA: 0x000E3E60 File Offset: 0x000E2060
	private void TestRemoveFriendCompleteCallback(FriendBackendController.Friend friend, bool success)
	{
		if (success)
		{
			Debug.Log("FriendBackend: TestRemoveFriendCompleteCallback returned with success = true");
			return;
		}
		Debug.Log("FriendBackend: TestRemoveFriendCompleteCallback returned with success = false");
	}

	// Token: 0x06002F19 RID: 12057 RVA: 0x000E3E7A File Offset: 0x000E207A
	private void TestGetFriends()
	{
		this.OnGetFriendsComplete -= this.TestGetFriendsCompleteCallback;
		this.OnGetFriendsComplete += this.TestGetFriendsCompleteCallback;
		this.GetFriends();
	}

	// Token: 0x06002F1A RID: 12058 RVA: 0x000E3EA8 File Offset: 0x000E20A8
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

	// Token: 0x06002F1B RID: 12059 RVA: 0x000E4005 File Offset: 0x000E2205
	private void TestSetPrivacyState()
	{
		this.OnSetPrivacyStateComplete -= this.TestSetPrivacyStateCompleteCallback;
		this.OnSetPrivacyStateComplete += this.TestSetPrivacyStateCompleteCallback;
		this.SetPrivacyState(this.privacyStateToSet);
	}

	// Token: 0x06002F1C RID: 12060 RVA: 0x000E4038 File Offset: 0x000E2238
	private void TestSetPrivacyStateCompleteCallback(bool success)
	{
		if (success)
		{
			Debug.Log(string.Format("SetPrivacyState Success: Status: {0} Error: {1}", this.lastPrivacyStateResponse.StatusCode, this.lastPrivacyStateResponse.Error));
			return;
		}
		Debug.Log(string.Format("SetPrivacyState Failed: Status: {0} Error: {1}", this.lastPrivacyStateResponse.StatusCode, this.lastPrivacyStateResponse.Error));
	}

	// Token: 0x04003336 RID: 13110
	[OnEnterPlay_SetNull]
	public static volatile FriendBackendController Instance;

	// Token: 0x0400333B RID: 13115
	private int maxRetriesOnFail = 3;

	// Token: 0x0400333C RID: 13116
	private int getFriendsRetryCount;

	// Token: 0x0400333D RID: 13117
	private int setPrivacyStateRetryCount;

	// Token: 0x0400333E RID: 13118
	private int addFriendRetryCount;

	// Token: 0x0400333F RID: 13119
	private int removeFriendRetryCount;

	// Token: 0x04003340 RID: 13120
	private bool getFriendsInProgress;

	// Token: 0x04003341 RID: 13121
	private FriendBackendController.GetFriendsResponse lastGetFriendsResponse;

	// Token: 0x04003342 RID: 13122
	private List<FriendBackendController.Friend> lastFriendsList = new List<FriendBackendController.Friend>();

	// Token: 0x04003343 RID: 13123
	private bool setPrivacyStateInProgress;

	// Token: 0x04003344 RID: 13124
	private FriendBackendController.PrivacyState setPrivacyStateState;

	// Token: 0x04003345 RID: 13125
	private FriendBackendController.SetPrivacyStateResponse lastPrivacyStateResponse;

	// Token: 0x04003346 RID: 13126
	private Queue<FriendBackendController.PrivacyState> setPrivacyStateQueue = new Queue<FriendBackendController.PrivacyState>();

	// Token: 0x04003347 RID: 13127
	private FriendBackendController.PrivacyState lastPrivacyState;

	// Token: 0x04003348 RID: 13128
	private bool addFriendInProgress;

	// Token: 0x04003349 RID: 13129
	private int addFriendTargetIdHash;

	// Token: 0x0400334A RID: 13130
	private NetPlayer addFriendTargetPlayer;

	// Token: 0x0400334B RID: 13131
	private Queue<ValueTuple<int, NetPlayer>> addFriendRequestQueue = new Queue<ValueTuple<int, NetPlayer>>();

	// Token: 0x0400334C RID: 13132
	private bool removeFriendInProgress;

	// Token: 0x0400334D RID: 13133
	private int removeFriendTargetIdHash;

	// Token: 0x0400334E RID: 13134
	private FriendBackendController.Friend removeFriendTarget;

	// Token: 0x0400334F RID: 13135
	private Queue<ValueTuple<int, FriendBackendController.Friend>> removeFriendRequestQueue = new Queue<ValueTuple<int, FriendBackendController.Friend>>();

	// Token: 0x04003350 RID: 13136
	[SerializeField]
	private int netPlayerIndexToAddFriend;

	// Token: 0x04003351 RID: 13137
	[SerializeField]
	private int friendListIndexToRemoveFriend;

	// Token: 0x04003352 RID: 13138
	[SerializeField]
	private FriendBackendController.PrivacyState privacyStateToSet;

	// Token: 0x0200077C RID: 1916
	public class Friend
	{
		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06002F1E RID: 12062 RVA: 0x000E40D8 File Offset: 0x000E22D8
		// (set) Token: 0x06002F1F RID: 12063 RVA: 0x000E40E0 File Offset: 0x000E22E0
		public FriendBackendController.FriendPresence Presence { get; set; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06002F20 RID: 12064 RVA: 0x000E40E9 File Offset: 0x000E22E9
		// (set) Token: 0x06002F21 RID: 12065 RVA: 0x000E40F1 File Offset: 0x000E22F1
		public DateTime Created { get; set; }
	}

	// Token: 0x0200077D RID: 1917
	public class FriendPresence
	{
		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06002F23 RID: 12067 RVA: 0x000E40FA File Offset: 0x000E22FA
		// (set) Token: 0x06002F24 RID: 12068 RVA: 0x000E4102 File Offset: 0x000E2302
		public string FriendLinkId { get; set; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06002F25 RID: 12069 RVA: 0x000E410B File Offset: 0x000E230B
		// (set) Token: 0x06002F26 RID: 12070 RVA: 0x000E4113 File Offset: 0x000E2313
		public string UserName { get; set; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06002F27 RID: 12071 RVA: 0x000E411C File Offset: 0x000E231C
		// (set) Token: 0x06002F28 RID: 12072 RVA: 0x000E4124 File Offset: 0x000E2324
		public string RoomId { get; set; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06002F29 RID: 12073 RVA: 0x000E412D File Offset: 0x000E232D
		// (set) Token: 0x06002F2A RID: 12074 RVA: 0x000E4135 File Offset: 0x000E2335
		public string Zone { get; set; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06002F2B RID: 12075 RVA: 0x000E413E File Offset: 0x000E233E
		// (set) Token: 0x06002F2C RID: 12076 RVA: 0x000E4146 File Offset: 0x000E2346
		public string Region { get; set; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06002F2D RID: 12077 RVA: 0x000E414F File Offset: 0x000E234F
		// (set) Token: 0x06002F2E RID: 12078 RVA: 0x000E4157 File Offset: 0x000E2357
		public bool? IsPublic { get; set; }
	}

	// Token: 0x0200077E RID: 1918
	public class FriendLink
	{
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06002F30 RID: 12080 RVA: 0x000E4160 File Offset: 0x000E2360
		// (set) Token: 0x06002F31 RID: 12081 RVA: 0x000E4168 File Offset: 0x000E2368
		public string my_playfab_id { get; set; }

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06002F32 RID: 12082 RVA: 0x000E4171 File Offset: 0x000E2371
		// (set) Token: 0x06002F33 RID: 12083 RVA: 0x000E4179 File Offset: 0x000E2379
		public string my_mothership_id { get; set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06002F34 RID: 12084 RVA: 0x000E4182 File Offset: 0x000E2382
		// (set) Token: 0x06002F35 RID: 12085 RVA: 0x000E418A File Offset: 0x000E238A
		public string my_friendlink_id { get; set; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06002F36 RID: 12086 RVA: 0x000E4193 File Offset: 0x000E2393
		// (set) Token: 0x06002F37 RID: 12087 RVA: 0x000E419B File Offset: 0x000E239B
		public string friend_playfab_id { get; set; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06002F38 RID: 12088 RVA: 0x000E41A4 File Offset: 0x000E23A4
		// (set) Token: 0x06002F39 RID: 12089 RVA: 0x000E41AC File Offset: 0x000E23AC
		public string friend_mothership_id { get; set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06002F3A RID: 12090 RVA: 0x000E41B5 File Offset: 0x000E23B5
		// (set) Token: 0x06002F3B RID: 12091 RVA: 0x000E41BD File Offset: 0x000E23BD
		public string friend_friendlink_id { get; set; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06002F3C RID: 12092 RVA: 0x000E41C6 File Offset: 0x000E23C6
		// (set) Token: 0x06002F3D RID: 12093 RVA: 0x000E41CE File Offset: 0x000E23CE
		public DateTime created { get; set; }
	}

	// Token: 0x0200077F RID: 1919
	[NullableContext(2)]
	[Nullable(0)]
	public class FriendIdResponse
	{
		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06002F3F RID: 12095 RVA: 0x000E41D7 File Offset: 0x000E23D7
		// (set) Token: 0x06002F40 RID: 12096 RVA: 0x000E41DF File Offset: 0x000E23DF
		public string PlayFabId { get; set; }

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06002F41 RID: 12097 RVA: 0x000E41E8 File Offset: 0x000E23E8
		// (set) Token: 0x06002F42 RID: 12098 RVA: 0x000E41F0 File Offset: 0x000E23F0
		public string MothershipId { get; set; } = "";
	}

	// Token: 0x02000780 RID: 1920
	public class FriendRequestRequest
	{
		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06002F44 RID: 12100 RVA: 0x000E420C File Offset: 0x000E240C
		// (set) Token: 0x06002F45 RID: 12101 RVA: 0x000E4214 File Offset: 0x000E2414
		public string PlayFabId { get; set; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06002F46 RID: 12102 RVA: 0x000E421D File Offset: 0x000E241D
		// (set) Token: 0x06002F47 RID: 12103 RVA: 0x000E4225 File Offset: 0x000E2425
		public string MothershipId { get; set; } = "";

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06002F48 RID: 12104 RVA: 0x000E422E File Offset: 0x000E242E
		// (set) Token: 0x06002F49 RID: 12105 RVA: 0x000E4236 File Offset: 0x000E2436
		public string PlayFabTicket { get; set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06002F4A RID: 12106 RVA: 0x000E423F File Offset: 0x000E243F
		// (set) Token: 0x06002F4B RID: 12107 RVA: 0x000E4247 File Offset: 0x000E2447
		public string MothershipToken { get; set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06002F4C RID: 12108 RVA: 0x000E4250 File Offset: 0x000E2450
		// (set) Token: 0x06002F4D RID: 12109 RVA: 0x000E4258 File Offset: 0x000E2458
		public string MyFriendLinkId { get; set; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06002F4E RID: 12110 RVA: 0x000E4261 File Offset: 0x000E2461
		// (set) Token: 0x06002F4F RID: 12111 RVA: 0x000E4269 File Offset: 0x000E2469
		public string FriendFriendLinkId { get; set; }
	}

	// Token: 0x02000781 RID: 1921
	public class GetFriendsRequest
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06002F51 RID: 12113 RVA: 0x000E4285 File Offset: 0x000E2485
		// (set) Token: 0x06002F52 RID: 12114 RVA: 0x000E428D File Offset: 0x000E248D
		public string PlayFabId { get; set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06002F53 RID: 12115 RVA: 0x000E4296 File Offset: 0x000E2496
		// (set) Token: 0x06002F54 RID: 12116 RVA: 0x000E429E File Offset: 0x000E249E
		public string MothershipId { get; set; } = "";

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06002F55 RID: 12117 RVA: 0x000E42A7 File Offset: 0x000E24A7
		// (set) Token: 0x06002F56 RID: 12118 RVA: 0x000E42AF File Offset: 0x000E24AF
		public string MothershipToken { get; set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06002F57 RID: 12119 RVA: 0x000E42B8 File Offset: 0x000E24B8
		// (set) Token: 0x06002F58 RID: 12120 RVA: 0x000E42C0 File Offset: 0x000E24C0
		public string PlayFabTicket { get; set; }
	}

	// Token: 0x02000782 RID: 1922
	public class GetFriendsResponse
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06002F5A RID: 12122 RVA: 0x000E42DC File Offset: 0x000E24DC
		// (set) Token: 0x06002F5B RID: 12123 RVA: 0x000E42E4 File Offset: 0x000E24E4
		[CanBeNull]
		public FriendBackendController.GetFriendsResult Result { get; set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06002F5C RID: 12124 RVA: 0x000E42ED File Offset: 0x000E24ED
		// (set) Token: 0x06002F5D RID: 12125 RVA: 0x000E42F5 File Offset: 0x000E24F5
		public int StatusCode { get; set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06002F5E RID: 12126 RVA: 0x000E42FE File Offset: 0x000E24FE
		// (set) Token: 0x06002F5F RID: 12127 RVA: 0x000E4306 File Offset: 0x000E2506
		[Nullable(2)]
		public string Error { [NullableContext(2)] get; [NullableContext(2)] set; }
	}

	// Token: 0x02000783 RID: 1923
	public class GetFriendsResult
	{
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06002F61 RID: 12129 RVA: 0x000E430F File Offset: 0x000E250F
		// (set) Token: 0x06002F62 RID: 12130 RVA: 0x000E4317 File Offset: 0x000E2517
		public List<FriendBackendController.Friend> Friends { get; set; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06002F63 RID: 12131 RVA: 0x000E4320 File Offset: 0x000E2520
		// (set) Token: 0x06002F64 RID: 12132 RVA: 0x000E4328 File Offset: 0x000E2528
		public FriendBackendController.PrivacyState MyPrivacyState { get; set; }
	}

	// Token: 0x02000784 RID: 1924
	public class SetPrivacyStateRequest
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002F66 RID: 12134 RVA: 0x000E4331 File Offset: 0x000E2531
		// (set) Token: 0x06002F67 RID: 12135 RVA: 0x000E4339 File Offset: 0x000E2539
		public string PlayFabId { get; set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06002F68 RID: 12136 RVA: 0x000E4342 File Offset: 0x000E2542
		// (set) Token: 0x06002F69 RID: 12137 RVA: 0x000E434A File Offset: 0x000E254A
		public string PlayFabTicket { get; set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06002F6A RID: 12138 RVA: 0x000E4353 File Offset: 0x000E2553
		// (set) Token: 0x06002F6B RID: 12139 RVA: 0x000E435B File Offset: 0x000E255B
		public string PrivacyState { get; set; }
	}

	// Token: 0x02000785 RID: 1925
	[NullableContext(2)]
	[Nullable(0)]
	public class SetPrivacyStateResponse
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06002F6D RID: 12141 RVA: 0x000E4364 File Offset: 0x000E2564
		// (set) Token: 0x06002F6E RID: 12142 RVA: 0x000E436C File Offset: 0x000E256C
		public int StatusCode { get; set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06002F6F RID: 12143 RVA: 0x000E4375 File Offset: 0x000E2575
		// (set) Token: 0x06002F70 RID: 12144 RVA: 0x000E437D File Offset: 0x000E257D
		public string Error { get; set; }
	}

	// Token: 0x02000786 RID: 1926
	public class RemoveFriendRequest
	{
		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06002F72 RID: 12146 RVA: 0x000E4386 File Offset: 0x000E2586
		// (set) Token: 0x06002F73 RID: 12147 RVA: 0x000E438E File Offset: 0x000E258E
		public string PlayFabId { get; set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06002F74 RID: 12148 RVA: 0x000E4397 File Offset: 0x000E2597
		// (set) Token: 0x06002F75 RID: 12149 RVA: 0x000E439F File Offset: 0x000E259F
		public string MothershipId { get; set; } = "";

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06002F76 RID: 12150 RVA: 0x000E43A8 File Offset: 0x000E25A8
		// (set) Token: 0x06002F77 RID: 12151 RVA: 0x000E43B0 File Offset: 0x000E25B0
		public string PlayFabTicket { get; set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06002F78 RID: 12152 RVA: 0x000E43B9 File Offset: 0x000E25B9
		// (set) Token: 0x06002F79 RID: 12153 RVA: 0x000E43C1 File Offset: 0x000E25C1
		public string MothershipToken { get; set; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06002F7A RID: 12154 RVA: 0x000E43CA File Offset: 0x000E25CA
		// (set) Token: 0x06002F7B RID: 12155 RVA: 0x000E43D2 File Offset: 0x000E25D2
		public string MyFriendLinkId { get; set; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06002F7C RID: 12156 RVA: 0x000E43DB File Offset: 0x000E25DB
		// (set) Token: 0x06002F7D RID: 12157 RVA: 0x000E43E3 File Offset: 0x000E25E3
		public string FriendFriendLinkId { get; set; }
	}

	// Token: 0x02000787 RID: 1927
	public enum PendingRequestStatus
	{
		// Token: 0x0400337F RID: 13183
		I_REQUESTED,
		// Token: 0x04003380 RID: 13184
		THEY_REQUESTED,
		// Token: 0x04003381 RID: 13185
		CONFIRMED,
		// Token: 0x04003382 RID: 13186
		NOT_FOUND
	}

	// Token: 0x02000788 RID: 1928
	public enum PrivacyState
	{
		// Token: 0x04003384 RID: 13188
		VISIBLE,
		// Token: 0x04003385 RID: 13189
		PUBLIC_ONLY,
		// Token: 0x04003386 RID: 13190
		HIDDEN
	}
}
