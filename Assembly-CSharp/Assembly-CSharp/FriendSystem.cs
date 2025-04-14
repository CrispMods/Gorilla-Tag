using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000795 RID: 1941
public class FriendSystem : MonoBehaviour
{
	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06003007 RID: 12295 RVA: 0x000E80D2 File Offset: 0x000E62D2
	public FriendSystem.PlayerPrivacy LocalPlayerPrivacy
	{
		get
		{
			return this.localPlayerPrivacy;
		}
	}

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06003008 RID: 12296 RVA: 0x000E80DC File Offset: 0x000E62DC
	// (remove) Token: 0x06003009 RID: 12297 RVA: 0x000E8114 File Offset: 0x000E6314
	public event Action<List<FriendBackendController.Friend>> OnFriendListRefresh;

	// Token: 0x0600300A RID: 12298 RVA: 0x000E814C File Offset: 0x000E634C
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

	// Token: 0x0600300B RID: 12299 RVA: 0x000E8189 File Offset: 0x000E6389
	public void RefreshFriendsList()
	{
		FriendBackendController.Instance.GetFriends();
	}

	// Token: 0x0600300C RID: 12300 RVA: 0x000E8198 File Offset: 0x000E6398
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

	// Token: 0x0600300D RID: 12301 RVA: 0x000E8214 File Offset: 0x000E6414
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

	// Token: 0x0600300E RID: 12302 RVA: 0x000E8270 File Offset: 0x000E6470
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

	// Token: 0x0600300F RID: 12303 RVA: 0x000E82C0 File Offset: 0x000E64C0
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

	// Token: 0x06003010 RID: 12304 RVA: 0x000E8339 File Offset: 0x000E6539
	private void Awake()
	{
		if (FriendSystem.Instance == null)
		{
			FriendSystem.Instance = this;
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x06003011 RID: 12305 RVA: 0x000E835C File Offset: 0x000E655C
	private void Start()
	{
		FriendBackendController.Instance.OnGetFriendsComplete += this.OnGetFriendsReturned;
		FriendBackendController.Instance.OnAddFriendComplete += this.OnAddFriendReturned;
		FriendBackendController.Instance.OnRemoveFriendComplete += this.OnRemoveFriendReturned;
	}

	// Token: 0x06003012 RID: 12306 RVA: 0x000E83B4 File Offset: 0x000E65B4
	private void OnDestroy()
	{
		if (FriendBackendController.Instance != null)
		{
			FriendBackendController.Instance.OnGetFriendsComplete -= this.OnGetFriendsReturned;
			FriendBackendController.Instance.OnAddFriendComplete -= this.OnAddFriendReturned;
			FriendBackendController.Instance.OnRemoveFriendComplete -= this.OnRemoveFriendReturned;
		}
	}

	// Token: 0x06003013 RID: 12307 RVA: 0x000E8418 File Offset: 0x000E6618
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

	// Token: 0x06003014 RID: 12308 RVA: 0x000E8488 File Offset: 0x000E6688
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

	// Token: 0x06003015 RID: 12309 RVA: 0x000E8594 File Offset: 0x000E6794
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

	// Token: 0x040033F8 RID: 13304
	[OnEnterPlay_SetNull]
	public static volatile FriendSystem Instance;

	// Token: 0x040033F9 RID: 13305
	[SerializeField]
	private float friendRequestExpirationTime = 10f;

	// Token: 0x040033FA RID: 13306
	private FriendSystem.PlayerPrivacy localPlayerPrivacy;

	// Token: 0x040033FB RID: 13307
	private List<FriendSystem.FriendRequestData> pendingFriendRequests = new List<FriendSystem.FriendRequestData>();

	// Token: 0x040033FC RID: 13308
	private List<FriendSystem.FriendRemovalData> pendingFriendRemovals = new List<FriendSystem.FriendRemovalData>();

	// Token: 0x040033FD RID: 13309
	private List<int> indexesToRemove = new List<int>();

	// Token: 0x040033FF RID: 13311
	private float lastFriendsListRefresh;

	// Token: 0x02000796 RID: 1942
	// (Invoke) Token: 0x06003018 RID: 12312
	public delegate void FriendRequestCallback(GTZone zone, int localId, int friendId, bool success);

	// Token: 0x02000797 RID: 1943
	private struct FriendRequestData
	{
		// Token: 0x04003400 RID: 13312
		public GTZone zone;

		// Token: 0x04003401 RID: 13313
		public int sendingPlayerId;

		// Token: 0x04003402 RID: 13314
		public int targetPlayerId;

		// Token: 0x04003403 RID: 13315
		public float localTimeSent;

		// Token: 0x04003404 RID: 13316
		public FriendSystem.FriendRequestCallback completionCallback;
	}

	// Token: 0x02000798 RID: 1944
	// (Invoke) Token: 0x0600301C RID: 12316
	public delegate void FriendRemovalCallback(int friendId, bool success);

	// Token: 0x02000799 RID: 1945
	private struct FriendRemovalData
	{
		// Token: 0x04003405 RID: 13317
		public int targetPlayerId;

		// Token: 0x04003406 RID: 13318
		public float localTimeSent;

		// Token: 0x04003407 RID: 13319
		public FriendSystem.FriendRemovalCallback completionCallback;
	}

	// Token: 0x0200079A RID: 1946
	private enum FriendRequestStatus
	{
		// Token: 0x04003409 RID: 13321
		Pending,
		// Token: 0x0400340A RID: 13322
		Succeeded,
		// Token: 0x0400340B RID: 13323
		Failed
	}

	// Token: 0x0200079B RID: 1947
	public enum PlayerPrivacy
	{
		// Token: 0x0400340D RID: 13325
		Visible,
		// Token: 0x0400340E RID: 13326
		PublicOnly,
		// Token: 0x0400340F RID: 13327
		Hidden
	}
}
