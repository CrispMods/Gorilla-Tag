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

// Token: 0x02000793 RID: 1939
public class FriendBackendController : MonoBehaviour
{
	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06002FAB RID: 12203 RVA: 0x0012D44C File Offset: 0x0012B64C
	// (remove) Token: 0x06002FAC RID: 12204 RVA: 0x0012D484 File Offset: 0x0012B684
	public event Action<bool> OnGetFriendsComplete;

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06002FAD RID: 12205 RVA: 0x0012D4BC File Offset: 0x0012B6BC
	// (remove) Token: 0x06002FAE RID: 12206 RVA: 0x0012D4F4 File Offset: 0x0012B6F4
	public event Action<bool> OnSetPrivacyStateComplete;

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06002FAF RID: 12207 RVA: 0x0012D52C File Offset: 0x0012B72C
	// (remove) Token: 0x06002FB0 RID: 12208 RVA: 0x0012D564 File Offset: 0x0012B764
	public event Action<NetPlayer, bool> OnAddFriendComplete;

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06002FB1 RID: 12209 RVA: 0x0012D59C File Offset: 0x0012B79C
	// (remove) Token: 0x06002FB2 RID: 12210 RVA: 0x0012D5D4 File Offset: 0x0012B7D4
	public event Action<FriendBackendController.Friend, bool> OnRemoveFriendComplete;

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06002FB3 RID: 12211 RVA: 0x0004FBA4 File Offset: 0x0004DDA4
	public List<FriendBackendController.Friend> FriendsList
	{
		get
		{
			return this.lastFriendsList;
		}
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x0004FBAC File Offset: 0x0004DDAC
	public FriendBackendController.PrivacyState MyPrivacyState
	{
		get
		{
			return this.lastPrivacyState;
		}
	}

	// Token: 0x06002FB5 RID: 12213 RVA: 0x0004FBB4 File Offset: 0x0004DDB4
	public void GetFriends()
	{
		if (!this.getFriendsInProgress)
		{
			this.getFriendsInProgress = true;
			this.GetFriendsInternal();
		}
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x0004FBCB File Offset: 0x0004DDCB
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

	// Token: 0x06002FB7 RID: 12215 RVA: 0x0012D60C File Offset: 0x0012B80C
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

	// Token: 0x06002FB8 RID: 12216 RVA: 0x0012D67C File Offset: 0x0012B87C
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

	// Token: 0x06002FB9 RID: 12217 RVA: 0x0004FBF6 File Offset: 0x0004DDF6
	private void Awake()
	{
		if (FriendBackendController.Instance == null)
		{
			FriendBackendController.Instance = this;
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06002FBA RID: 12218 RVA: 0x0012D6F4 File Offset: 0x0012B8F4
	private void GetFriendsInternal()
	{
		base.StartCoroutine(this.SendGetFriendsRequest(new FriendBackendController.GetFriendsRequest
		{
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			MothershipId = ""
		}, new Action<FriendBackendController.GetFriendsResponse>(this.GetFriendsComplete)));
	}

	// Token: 0x06002FBB RID: 12219 RVA: 0x0004FC16 File Offset: 0x0004DE16
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

	// Token: 0x06002FBC RID: 12220 RVA: 0x0012D750 File Offset: 0x0012B950
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

	// Token: 0x06002FBD RID: 12221 RVA: 0x0012D82C File Offset: 0x0012BA2C
	private void SetPrivacyStateInternal()
	{
		base.StartCoroutine(this.SendSetPrivacyStateRequest(new FriendBackendController.SetPrivacyStateRequest
		{
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			PrivacyState = this.setPrivacyStateState.ToString()
		}, new Action<FriendBackendController.SetPrivacyStateResponse>(this.SetPrivacyStateComplete)));
	}

	// Token: 0x06002FBE RID: 12222 RVA: 0x0004FC33 File Offset: 0x0004DE33
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

	// Token: 0x06002FBF RID: 12223 RVA: 0x0012D894 File Offset: 0x0012BA94
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

	// Token: 0x06002FC0 RID: 12224 RVA: 0x0012D8FC File Offset: 0x0012BAFC
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

	// Token: 0x06002FC1 RID: 12225 RVA: 0x0004FC50 File Offset: 0x0004DE50
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

	// Token: 0x06002FC2 RID: 12226 RVA: 0x0012D988 File Offset: 0x0012BB88
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

	// Token: 0x06002FC3 RID: 12227 RVA: 0x0012DA08 File Offset: 0x0012BC08
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

	// Token: 0x06002FC4 RID: 12228 RVA: 0x0004FC6D File Offset: 0x0004DE6D
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

	// Token: 0x06002FC5 RID: 12229 RVA: 0x0012DA90 File Offset: 0x0012BC90
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

	// Token: 0x06002FC6 RID: 12230 RVA: 0x0012DB10 File Offset: 0x0012BD10
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

	// Token: 0x06002FC7 RID: 12231 RVA: 0x0012DBA8 File Offset: 0x0012BDA8
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

	// Token: 0x06002FC8 RID: 12232 RVA: 0x0004FC8A File Offset: 0x0004DE8A
	private void TestAddFriendCompleteCallback(NetPlayer player, bool success)
	{
		if (success)
		{
			Debug.Log("FriendBackend: TestAddFriendCompleteCallback returned with success = true");
			return;
		}
		Debug.Log("FriendBackend: TestAddFriendCompleteCallback returned with success = false");
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x0012DC14 File Offset: 0x0012BE14
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

	// Token: 0x06002FCA RID: 12234 RVA: 0x0004FCA4 File Offset: 0x0004DEA4
	private void TestRemoveFriendCompleteCallback(FriendBackendController.Friend friend, bool success)
	{
		if (success)
		{
			Debug.Log("FriendBackend: TestRemoveFriendCompleteCallback returned with success = true");
			return;
		}
		Debug.Log("FriendBackend: TestRemoveFriendCompleteCallback returned with success = false");
	}

	// Token: 0x06002FCB RID: 12235 RVA: 0x0004FCBE File Offset: 0x0004DEBE
	private void TestGetFriends()
	{
		this.OnGetFriendsComplete -= this.TestGetFriendsCompleteCallback;
		this.OnGetFriendsComplete += this.TestGetFriendsCompleteCallback;
		this.GetFriends();
	}

	// Token: 0x06002FCC RID: 12236 RVA: 0x0012DC7C File Offset: 0x0012BE7C
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

	// Token: 0x06002FCD RID: 12237 RVA: 0x0004FCEA File Offset: 0x0004DEEA
	private void TestSetPrivacyState()
	{
		this.OnSetPrivacyStateComplete -= this.TestSetPrivacyStateCompleteCallback;
		this.OnSetPrivacyStateComplete += this.TestSetPrivacyStateCompleteCallback;
		this.SetPrivacyState(this.privacyStateToSet);
	}

	// Token: 0x06002FCE RID: 12238 RVA: 0x0012DDDC File Offset: 0x0012BFDC
	private void TestSetPrivacyStateCompleteCallback(bool success)
	{
		if (success)
		{
			Debug.Log(string.Format("SetPrivacyState Success: Status: {0} Error: {1}", this.lastPrivacyStateResponse.StatusCode, this.lastPrivacyStateResponse.Error));
			return;
		}
		Debug.Log(string.Format("SetPrivacyState Failed: Status: {0} Error: {1}", this.lastPrivacyStateResponse.StatusCode, this.lastPrivacyStateResponse.Error));
	}

	// Token: 0x040033E0 RID: 13280
	[OnEnterPlay_SetNull]
	public static volatile FriendBackendController Instance;

	// Token: 0x040033E5 RID: 13285
	private int maxRetriesOnFail = 3;

	// Token: 0x040033E6 RID: 13286
	private int getFriendsRetryCount;

	// Token: 0x040033E7 RID: 13287
	private int setPrivacyStateRetryCount;

	// Token: 0x040033E8 RID: 13288
	private int addFriendRetryCount;

	// Token: 0x040033E9 RID: 13289
	private int removeFriendRetryCount;

	// Token: 0x040033EA RID: 13290
	private bool getFriendsInProgress;

	// Token: 0x040033EB RID: 13291
	private FriendBackendController.GetFriendsResponse lastGetFriendsResponse;

	// Token: 0x040033EC RID: 13292
	private List<FriendBackendController.Friend> lastFriendsList = new List<FriendBackendController.Friend>();

	// Token: 0x040033ED RID: 13293
	private bool setPrivacyStateInProgress;

	// Token: 0x040033EE RID: 13294
	private FriendBackendController.PrivacyState setPrivacyStateState;

	// Token: 0x040033EF RID: 13295
	private FriendBackendController.SetPrivacyStateResponse lastPrivacyStateResponse;

	// Token: 0x040033F0 RID: 13296
	private Queue<FriendBackendController.PrivacyState> setPrivacyStateQueue = new Queue<FriendBackendController.PrivacyState>();

	// Token: 0x040033F1 RID: 13297
	private FriendBackendController.PrivacyState lastPrivacyState;

	// Token: 0x040033F2 RID: 13298
	private bool addFriendInProgress;

	// Token: 0x040033F3 RID: 13299
	private int addFriendTargetIdHash;

	// Token: 0x040033F4 RID: 13300
	private NetPlayer addFriendTargetPlayer;

	// Token: 0x040033F5 RID: 13301
	private Queue<ValueTuple<int, NetPlayer>> addFriendRequestQueue = new Queue<ValueTuple<int, NetPlayer>>();

	// Token: 0x040033F6 RID: 13302
	private bool removeFriendInProgress;

	// Token: 0x040033F7 RID: 13303
	private int removeFriendTargetIdHash;

	// Token: 0x040033F8 RID: 13304
	private FriendBackendController.Friend removeFriendTarget;

	// Token: 0x040033F9 RID: 13305
	private Queue<ValueTuple<int, FriendBackendController.Friend>> removeFriendRequestQueue = new Queue<ValueTuple<int, FriendBackendController.Friend>>();

	// Token: 0x040033FA RID: 13306
	[SerializeField]
	private int netPlayerIndexToAddFriend;

	// Token: 0x040033FB RID: 13307
	[SerializeField]
	private int friendListIndexToRemoveFriend;

	// Token: 0x040033FC RID: 13308
	[SerializeField]
	private FriendBackendController.PrivacyState privacyStateToSet;

	// Token: 0x02000794 RID: 1940
	public class Friend
	{
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06002FD0 RID: 12240 RVA: 0x0004FD57 File Offset: 0x0004DF57
		// (set) Token: 0x06002FD1 RID: 12241 RVA: 0x0004FD5F File Offset: 0x0004DF5F
		public FriendBackendController.FriendPresence Presence { get; set; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06002FD2 RID: 12242 RVA: 0x0004FD68 File Offset: 0x0004DF68
		// (set) Token: 0x06002FD3 RID: 12243 RVA: 0x0004FD70 File Offset: 0x0004DF70
		public DateTime Created { get; set; }
	}

	// Token: 0x02000795 RID: 1941
	public class FriendPresence
	{
		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06002FD5 RID: 12245 RVA: 0x0004FD79 File Offset: 0x0004DF79
		// (set) Token: 0x06002FD6 RID: 12246 RVA: 0x0004FD81 File Offset: 0x0004DF81
		public string FriendLinkId { get; set; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x0004FD8A File Offset: 0x0004DF8A
		// (set) Token: 0x06002FD8 RID: 12248 RVA: 0x0004FD92 File Offset: 0x0004DF92
		public string UserName { get; set; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06002FD9 RID: 12249 RVA: 0x0004FD9B File Offset: 0x0004DF9B
		// (set) Token: 0x06002FDA RID: 12250 RVA: 0x0004FDA3 File Offset: 0x0004DFA3
		public string RoomId { get; set; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06002FDB RID: 12251 RVA: 0x0004FDAC File Offset: 0x0004DFAC
		// (set) Token: 0x06002FDC RID: 12252 RVA: 0x0004FDB4 File Offset: 0x0004DFB4
		public string Zone { get; set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06002FDD RID: 12253 RVA: 0x0004FDBD File Offset: 0x0004DFBD
		// (set) Token: 0x06002FDE RID: 12254 RVA: 0x0004FDC5 File Offset: 0x0004DFC5
		public string Region { get; set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06002FDF RID: 12255 RVA: 0x0004FDCE File Offset: 0x0004DFCE
		// (set) Token: 0x06002FE0 RID: 12256 RVA: 0x0004FDD6 File Offset: 0x0004DFD6
		public bool? IsPublic { get; set; }
	}

	// Token: 0x02000796 RID: 1942
	public class FriendLink
	{
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06002FE2 RID: 12258 RVA: 0x0004FDDF File Offset: 0x0004DFDF
		// (set) Token: 0x06002FE3 RID: 12259 RVA: 0x0004FDE7 File Offset: 0x0004DFE7
		public string my_playfab_id { get; set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x0004FDF0 File Offset: 0x0004DFF0
		// (set) Token: 0x06002FE5 RID: 12261 RVA: 0x0004FDF8 File Offset: 0x0004DFF8
		public string my_mothership_id { get; set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x0004FE01 File Offset: 0x0004E001
		// (set) Token: 0x06002FE7 RID: 12263 RVA: 0x0004FE09 File Offset: 0x0004E009
		public string my_friendlink_id { get; set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x0004FE12 File Offset: 0x0004E012
		// (set) Token: 0x06002FE9 RID: 12265 RVA: 0x0004FE1A File Offset: 0x0004E01A
		public string friend_playfab_id { get; set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06002FEA RID: 12266 RVA: 0x0004FE23 File Offset: 0x0004E023
		// (set) Token: 0x06002FEB RID: 12267 RVA: 0x0004FE2B File Offset: 0x0004E02B
		public string friend_mothership_id { get; set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06002FEC RID: 12268 RVA: 0x0004FE34 File Offset: 0x0004E034
		// (set) Token: 0x06002FED RID: 12269 RVA: 0x0004FE3C File Offset: 0x0004E03C
		public string friend_friendlink_id { get; set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06002FEE RID: 12270 RVA: 0x0004FE45 File Offset: 0x0004E045
		// (set) Token: 0x06002FEF RID: 12271 RVA: 0x0004FE4D File Offset: 0x0004E04D
		public DateTime created { get; set; }
	}

	// Token: 0x02000797 RID: 1943
	[NullableContext(2)]
	[Nullable(0)]
	public class FriendIdResponse
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06002FF1 RID: 12273 RVA: 0x0004FE56 File Offset: 0x0004E056
		// (set) Token: 0x06002FF2 RID: 12274 RVA: 0x0004FE5E File Offset: 0x0004E05E
		public string PlayFabId { get; set; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x0004FE67 File Offset: 0x0004E067
		// (set) Token: 0x06002FF4 RID: 12276 RVA: 0x0004FE6F File Offset: 0x0004E06F
		public string MothershipId { get; set; } = "";
	}

	// Token: 0x02000798 RID: 1944
	public class FriendRequestRequest
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x0004FE8B File Offset: 0x0004E08B
		// (set) Token: 0x06002FF7 RID: 12279 RVA: 0x0004FE93 File Offset: 0x0004E093
		public string PlayFabId { get; set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x0004FE9C File Offset: 0x0004E09C
		// (set) Token: 0x06002FF9 RID: 12281 RVA: 0x0004FEA4 File Offset: 0x0004E0A4
		public string MothershipId { get; set; } = "";

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06002FFA RID: 12282 RVA: 0x0004FEAD File Offset: 0x0004E0AD
		// (set) Token: 0x06002FFB RID: 12283 RVA: 0x0004FEB5 File Offset: 0x0004E0B5
		public string PlayFabTicket { get; set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x0004FEBE File Offset: 0x0004E0BE
		// (set) Token: 0x06002FFD RID: 12285 RVA: 0x0004FEC6 File Offset: 0x0004E0C6
		public string MothershipToken { get; set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x0004FECF File Offset: 0x0004E0CF
		// (set) Token: 0x06002FFF RID: 12287 RVA: 0x0004FED7 File Offset: 0x0004E0D7
		public string MyFriendLinkId { get; set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06003000 RID: 12288 RVA: 0x0004FEE0 File Offset: 0x0004E0E0
		// (set) Token: 0x06003001 RID: 12289 RVA: 0x0004FEE8 File Offset: 0x0004E0E8
		public string FriendFriendLinkId { get; set; }
	}

	// Token: 0x02000799 RID: 1945
	public class GetFriendsRequest
	{
		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06003003 RID: 12291 RVA: 0x0004FF04 File Offset: 0x0004E104
		// (set) Token: 0x06003004 RID: 12292 RVA: 0x0004FF0C File Offset: 0x0004E10C
		public string PlayFabId { get; set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06003005 RID: 12293 RVA: 0x0004FF15 File Offset: 0x0004E115
		// (set) Token: 0x06003006 RID: 12294 RVA: 0x0004FF1D File Offset: 0x0004E11D
		public string MothershipId { get; set; } = "";

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06003007 RID: 12295 RVA: 0x0004FF26 File Offset: 0x0004E126
		// (set) Token: 0x06003008 RID: 12296 RVA: 0x0004FF2E File Offset: 0x0004E12E
		public string MothershipToken { get; set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06003009 RID: 12297 RVA: 0x0004FF37 File Offset: 0x0004E137
		// (set) Token: 0x0600300A RID: 12298 RVA: 0x0004FF3F File Offset: 0x0004E13F
		public string PlayFabTicket { get; set; }
	}

	// Token: 0x0200079A RID: 1946
	public class GetFriendsResponse
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x0004FF5B File Offset: 0x0004E15B
		// (set) Token: 0x0600300D RID: 12301 RVA: 0x0004FF63 File Offset: 0x0004E163
		[CanBeNull]
		public FriendBackendController.GetFriendsResult Result { get; set; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x0004FF6C File Offset: 0x0004E16C
		// (set) Token: 0x0600300F RID: 12303 RVA: 0x0004FF74 File Offset: 0x0004E174
		public int StatusCode { get; set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06003010 RID: 12304 RVA: 0x0004FF7D File Offset: 0x0004E17D
		// (set) Token: 0x06003011 RID: 12305 RVA: 0x0004FF85 File Offset: 0x0004E185
		[Nullable(2)]
		public string Error { [NullableContext(2)] get; [NullableContext(2)] set; }
	}

	// Token: 0x0200079B RID: 1947
	public class GetFriendsResult
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06003013 RID: 12307 RVA: 0x0004FF8E File Offset: 0x0004E18E
		// (set) Token: 0x06003014 RID: 12308 RVA: 0x0004FF96 File Offset: 0x0004E196
		public List<FriendBackendController.Friend> Friends { get; set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06003015 RID: 12309 RVA: 0x0004FF9F File Offset: 0x0004E19F
		// (set) Token: 0x06003016 RID: 12310 RVA: 0x0004FFA7 File Offset: 0x0004E1A7
		public FriendBackendController.PrivacyState MyPrivacyState { get; set; }
	}

	// Token: 0x0200079C RID: 1948
	public class SetPrivacyStateRequest
	{
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x0004FFB0 File Offset: 0x0004E1B0
		// (set) Token: 0x06003019 RID: 12313 RVA: 0x0004FFB8 File Offset: 0x0004E1B8
		public string PlayFabId { get; set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x0600301A RID: 12314 RVA: 0x0004FFC1 File Offset: 0x0004E1C1
		// (set) Token: 0x0600301B RID: 12315 RVA: 0x0004FFC9 File Offset: 0x0004E1C9
		public string PlayFabTicket { get; set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x0004FFD2 File Offset: 0x0004E1D2
		// (set) Token: 0x0600301D RID: 12317 RVA: 0x0004FFDA File Offset: 0x0004E1DA
		public string PrivacyState { get; set; }
	}

	// Token: 0x0200079D RID: 1949
	[NullableContext(2)]
	[Nullable(0)]
	public class SetPrivacyStateResponse
	{
		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x0004FFE3 File Offset: 0x0004E1E3
		// (set) Token: 0x06003020 RID: 12320 RVA: 0x0004FFEB File Offset: 0x0004E1EB
		public int StatusCode { get; set; }

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x0004FFF4 File Offset: 0x0004E1F4
		// (set) Token: 0x06003022 RID: 12322 RVA: 0x0004FFFC File Offset: 0x0004E1FC
		public string Error { get; set; }
	}

	// Token: 0x0200079E RID: 1950
	public class RemoveFriendRequest
	{
		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06003024 RID: 12324 RVA: 0x00050005 File Offset: 0x0004E205
		// (set) Token: 0x06003025 RID: 12325 RVA: 0x0005000D File Offset: 0x0004E20D
		public string PlayFabId { get; set; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x00050016 File Offset: 0x0004E216
		// (set) Token: 0x06003027 RID: 12327 RVA: 0x0005001E File Offset: 0x0004E21E
		public string MothershipId { get; set; } = "";

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06003028 RID: 12328 RVA: 0x00050027 File Offset: 0x0004E227
		// (set) Token: 0x06003029 RID: 12329 RVA: 0x0005002F File Offset: 0x0004E22F
		public string PlayFabTicket { get; set; }

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x00050038 File Offset: 0x0004E238
		// (set) Token: 0x0600302B RID: 12331 RVA: 0x00050040 File Offset: 0x0004E240
		public string MothershipToken { get; set; }

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x0600302C RID: 12332 RVA: 0x00050049 File Offset: 0x0004E249
		// (set) Token: 0x0600302D RID: 12333 RVA: 0x00050051 File Offset: 0x0004E251
		public string MyFriendLinkId { get; set; }

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x0600302E RID: 12334 RVA: 0x0005005A File Offset: 0x0004E25A
		// (set) Token: 0x0600302F RID: 12335 RVA: 0x00050062 File Offset: 0x0004E262
		public string FriendFriendLinkId { get; set; }
	}

	// Token: 0x0200079F RID: 1951
	public enum PendingRequestStatus
	{
		// Token: 0x04003429 RID: 13353
		I_REQUESTED,
		// Token: 0x0400342A RID: 13354
		THEY_REQUESTED,
		// Token: 0x0400342B RID: 13355
		CONFIRMED,
		// Token: 0x0400342C RID: 13356
		NOT_FOUND
	}

	// Token: 0x020007A0 RID: 1952
	public enum PrivacyState
	{
		// Token: 0x0400342E RID: 13358
		VISIBLE,
		// Token: 0x0400342F RID: 13359
		PUBLIC_ONLY,
		// Token: 0x04003430 RID: 13360
		HIDDEN
	}
}
