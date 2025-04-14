using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000794 RID: 1940
public class FriendSystem : MonoBehaviour
{
	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06002FFF RID: 12287 RVA: 0x000E7C52 File Offset: 0x000E5E52
	public FriendSystem.PlayerPrivacy LocalPlayerPrivacy
	{
		get
		{
			return this.localPlayerPrivacy;
		}
	}

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06003000 RID: 12288 RVA: 0x000E7C5C File Offset: 0x000E5E5C
	// (remove) Token: 0x06003001 RID: 12289 RVA: 0x000E7C94 File Offset: 0x000E5E94
	public event Action<List<FriendBackendController.Friend>> OnFriendListRefresh;

	// Token: 0x06003002 RID: 12290 RVA: 0x000E7CCC File Offset: 0x000E5ECC
	public void SetLocalPlayerPrivacy(FriendSystem.PlayerPrivacy privacyState)
	{
		this.localPlayerPrivacy = privacyState;
		FriendBackendController.PrivacyState privacyState2;
		switch (privacyState)
		{
		default:
			privacyState2 = FriendBackendController.PrivacyState.VISIBLE;
			break;
		case FriendSystem.PlayerPrivacy.PublicOnly:
			privacyState2 = FriendBackendController.PrivacyState.PUBLIC_ONLY;
			break;
		case FriendSystem.PlayerPrivacy.Hidden:
			privacyState2 = FriendBackendController.PrivacyState.HIDDEN;
			break;
		}
		FriendBackendController.Instance.SetPrivacyState(privacyState2);
	}

	// Token: 0x06003003 RID: 12291 RVA: 0x000E7D09 File Offset: 0x000E5F09
	public void RefreshFriendsList()
	{
		FriendBackendController.Instance.GetFriends();
	}

	// Token: 0x06003004 RID: 12292 RVA: 0x000E7D18 File Offset: 0x000E5F18
	public void SendFriendRequest(NetPlayer targetPlayer, GTZone stationZone, FriendSystem.FriendRequestCallback callback)
	{
		FriendSystem.FriendRequestData item = new FriendSystem.FriendRequestData
		{
			completionCallback = callback,
			sendingPlayerId = NetworkSystem.Instance.LocalPlayer.UserId.GetHashCode(),
			targetPlayerId = targetPlayer.UserId.GetHashCode(),
			localTimeSent = Time.time,
			zone = stationZone
		};
		this.pendingFriendRequests.Add(item);
		FriendBackendController.Instance.AddFriend(targetPlayer);
	}

	// Token: 0x06003005 RID: 12293 RVA: 0x000E7D94 File Offset: 0x000E5F94
	public void RemoveFriend(FriendBackendController.Friend friend, FriendSystem.FriendRemovalCallback callback = null)
	{
		this.pendingFriendRemovals.Add(new FriendSystem.FriendRemovalData
		{
			completionCallback = callback,
			targetPlayerId = friend.Presence.FriendLinkId.GetHashCode(),
			localTimeSent = Time.time
		});
		FriendBackendController.Instance.RemoveFriend(friend);
	}

	// Token: 0x06003006 RID: 12294 RVA: 0x000E7DF0 File Offset: 0x000E5FF0
	public bool HasPendingFriendRequest(GTZone zone, int senderId)
	{
		for (int i = 0; i < this.pendingFriendRequests.Count; i++)
		{
			if (this.pendingFriendRequests[i].zone == zone && this.pendingFriendRequests[i].sendingPlayerId == senderId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003007 RID: 12295 RVA: 0x000E7E40 File Offset: 0x000E6040
	public bool CheckFriendshipWithPlayer(int targetActorNumber)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(targetActorNumber);
		if (player != null)
		{
			int hashCode = player.UserId.GetHashCode();
			List<FriendBackendController.Friend> friendsList = FriendBackendController.Instance.FriendsList;
			for (int i = 0; i < friendsList.Count; i++)
			{
				if (friendsList[i] != null && friendsList[i].Presence != null && friendsList[i].Presence.FriendLinkId.GetHashCode() == hashCode)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003008 RID: 12296 RVA: 0x000E7EB9 File Offset: 0x000E60B9
	private void Awake()
	{
		if (FriendSystem.Instance == null)
		{
			FriendSystem.Instance = this;
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x06003009 RID: 12297 RVA: 0x000E7EDC File Offset: 0x000E60DC
	private void Start()
	{
		FriendBackendController.Instance.OnGetFriendsComplete += this.OnGetFriendsReturned;
		FriendBackendController.Instance.OnAddFriendComplete += this.OnAddFriendReturned;
		FriendBackendController.Instance.OnRemoveFriendComplete += this.OnRemoveFriendReturned;
	}

	// Token: 0x0600300A RID: 12298 RVA: 0x000E7F34 File Offset: 0x000E6134
	private void OnDestroy()
	{
		if (FriendBackendController.Instance != null)
		{
			FriendBackendController.Instance.OnGetFriendsComplete -= this.OnGetFriendsReturned;
			FriendBackendController.Instance.OnAddFriendComplete -= this.OnAddFriendReturned;
			FriendBackendController.Instance.OnRemoveFriendComplete -= this.OnRemoveFriendReturned;
		}
	}

	// Token: 0x0600300B RID: 12299 RVA: 0x000E7F98 File Offset: 0x000E6198
	private void OnGetFriendsReturned(bool succeeded)
	{
		if (succeeded)
		{
			this.lastFriendsListRefresh = Time.time;
			switch (FriendBackendController.Instance.MyPrivacyState)
			{
			default:
				this.localPlayerPrivacy = FriendSystem.PlayerPrivacy.Visible;
				break;
			case FriendBackendController.PrivacyState.PUBLIC_ONLY:
				this.localPlayerPrivacy = FriendSystem.PlayerPrivacy.PublicOnly;
				break;
			case FriendBackendController.PrivacyState.HIDDEN:
				this.localPlayerPrivacy = FriendSystem.PlayerPrivacy.Hidden;
				break;
			}
			Action<List<FriendBackendController.Friend>> onFriendListRefresh = this.OnFriendListRefresh;
			if (onFriendListRefresh == null)
			{
				return;
			}
			onFriendListRefresh(FriendBackendController.Instance.FriendsList);
		}
	}

	// Token: 0x0600300C RID: 12300 RVA: 0x000E8008 File Offset: 0x000E6208
	private void OnAddFriendReturned(NetPlayer targetPlayer, bool succeeded)
	{
		int hashCode = targetPlayer.UserId.GetHashCode();
		this.indexesToRemove.Clear();
		for (int i = 0; i < this.pendingFriendRequests.Count; i++)
		{
			if (this.pendingFriendRequests[i].targetPlayerId == hashCode)
			{
				FriendSystem.FriendRequestCallback completionCallback = this.pendingFriendRequests[i].completionCallback;
				if (completionCallback != null)
				{
					completionCallback(this.pendingFriendRequests[i].zone, this.pendingFriendRequests[i].sendingPlayerId, this.pendingFriendRequests[i].targetPlayerId, succeeded);
				}
				this.indexesToRemove.Add(i);
			}
			else if (this.pendingFriendRequests[i].localTimeSent + this.friendRequestExpirationTime < Time.time)
			{
				this.indexesToRemove.Add(i);
			}
		}
		for (int j = this.indexesToRemove.Count - 1; j >= 0; j--)
		{
			this.pendingFriendRequests.RemoveAt(this.indexesToRemove[j]);
		}
	}

	// Token: 0x0600300D RID: 12301 RVA: 0x000E8114 File Offset: 0x000E6314
	private void OnRemoveFriendReturned(FriendBackendController.Friend friend, bool succeeded)
	{
		if (friend != null && friend.Presence != null)
		{
			int hashCode = friend.Presence.FriendLinkId.GetHashCode();
			this.indexesToRemove.Clear();
			for (int i = 0; i < this.pendingFriendRemovals.Count; i++)
			{
				if (this.pendingFriendRemovals[i].targetPlayerId == hashCode)
				{
					FriendSystem.FriendRemovalCallback completionCallback = this.pendingFriendRemovals[i].completionCallback;
					if (completionCallback != null)
					{
						completionCallback(hashCode, succeeded);
					}
					this.indexesToRemove.Add(i);
				}
				else if (this.pendingFriendRemovals[i].localTimeSent + this.friendRequestExpirationTime < Time.time)
				{
					this.indexesToRemove.Add(i);
				}
			}
			for (int j = this.indexesToRemove.Count - 1; j >= 0; j--)
			{
				this.pendingFriendRemovals.RemoveAt(this.indexesToRemove[j]);
			}
		}
	}

	// Token: 0x040033F2 RID: 13298
	[OnEnterPlay_SetNull]
	public static volatile FriendSystem Instance;

	// Token: 0x040033F3 RID: 13299
	[SerializeField]
	private float friendRequestExpirationTime = 10f;

	// Token: 0x040033F4 RID: 13300
	private FriendSystem.PlayerPrivacy localPlayerPrivacy;

	// Token: 0x040033F5 RID: 13301
	private List<FriendSystem.FriendRequestData> pendingFriendRequests = new List<FriendSystem.FriendRequestData>();

	// Token: 0x040033F6 RID: 13302
	private List<FriendSystem.FriendRemovalData> pendingFriendRemovals = new List<FriendSystem.FriendRemovalData>();

	// Token: 0x040033F7 RID: 13303
	private List<int> indexesToRemove = new List<int>();

	// Token: 0x040033F9 RID: 13305
	private float lastFriendsListRefresh;

	// Token: 0x02000795 RID: 1941
	// (Invoke) Token: 0x06003010 RID: 12304
	public delegate void FriendRequestCallback(GTZone zone, int localId, int friendId, bool success);

	// Token: 0x02000796 RID: 1942
	private struct FriendRequestData
	{
		// Token: 0x040033FA RID: 13306
		public GTZone zone;

		// Token: 0x040033FB RID: 13307
		public int sendingPlayerId;

		// Token: 0x040033FC RID: 13308
		public int targetPlayerId;

		// Token: 0x040033FD RID: 13309
		public float localTimeSent;

		// Token: 0x040033FE RID: 13310
		public FriendSystem.FriendRequestCallback completionCallback;
	}

	// Token: 0x02000797 RID: 1943
	// (Invoke) Token: 0x06003014 RID: 12308
	public delegate void FriendRemovalCallback(int friendId, bool success);

	// Token: 0x02000798 RID: 1944
	private struct FriendRemovalData
	{
		// Token: 0x040033FF RID: 13311
		public int targetPlayerId;

		// Token: 0x04003400 RID: 13312
		public float localTimeSent;

		// Token: 0x04003401 RID: 13313
		public FriendSystem.FriendRemovalCallback completionCallback;
	}

	// Token: 0x02000799 RID: 1945
	private enum FriendRequestStatus
	{
		// Token: 0x04003403 RID: 13315
		Pending,
		// Token: 0x04003404 RID: 13316
		Succeeded,
		// Token: 0x04003405 RID: 13317
		Failed
	}

	// Token: 0x0200079A RID: 1946
	public enum PlayerPrivacy
	{
		// Token: 0x04003407 RID: 13319
		Visible,
		// Token: 0x04003408 RID: 13320
		PublicOnly,
		// Token: 0x04003409 RID: 13321
		Hidden
	}
}
