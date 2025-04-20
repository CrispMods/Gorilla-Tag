using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007AC RID: 1964
public class FriendSystem : MonoBehaviour
{
	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x060030B1 RID: 12465 RVA: 0x00050480 File Offset: 0x0004E680
	public FriendSystem.PlayerPrivacy LocalPlayerPrivacy
	{
		get
		{
			return this.localPlayerPrivacy;
		}
	}

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x060030B2 RID: 12466 RVA: 0x00131298 File Offset: 0x0012F498
	// (remove) Token: 0x060030B3 RID: 12467 RVA: 0x001312D0 File Offset: 0x0012F4D0
	public event Action<List<FriendBackendController.Friend>> OnFriendListRefresh;

	// Token: 0x060030B4 RID: 12468 RVA: 0x00131308 File Offset: 0x0012F508
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

	// Token: 0x060030B5 RID: 12469 RVA: 0x00050488 File Offset: 0x0004E688
	public void RefreshFriendsList()
	{
		FriendBackendController.Instance.GetFriends();
	}

	// Token: 0x060030B6 RID: 12470 RVA: 0x00131348 File Offset: 0x0012F548
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

	// Token: 0x060030B7 RID: 12471 RVA: 0x001313C4 File Offset: 0x0012F5C4
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

	// Token: 0x060030B8 RID: 12472 RVA: 0x00131420 File Offset: 0x0012F620
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

	// Token: 0x060030B9 RID: 12473 RVA: 0x00131470 File Offset: 0x0012F670
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

	// Token: 0x060030BA RID: 12474 RVA: 0x00050496 File Offset: 0x0004E696
	private void Awake()
	{
		if (FriendSystem.Instance == null)
		{
			FriendSystem.Instance = this;
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x001314EC File Offset: 0x0012F6EC
	private void Start()
	{
		FriendBackendController.Instance.OnGetFriendsComplete += this.OnGetFriendsReturned;
		FriendBackendController.Instance.OnAddFriendComplete += this.OnAddFriendReturned;
		FriendBackendController.Instance.OnRemoveFriendComplete += this.OnRemoveFriendReturned;
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x00131544 File Offset: 0x0012F744
	private void OnDestroy()
	{
		if (FriendBackendController.Instance != null)
		{
			FriendBackendController.Instance.OnGetFriendsComplete -= this.OnGetFriendsReturned;
			FriendBackendController.Instance.OnAddFriendComplete -= this.OnAddFriendReturned;
			FriendBackendController.Instance.OnRemoveFriendComplete -= this.OnRemoveFriendReturned;
		}
	}

	// Token: 0x060030BD RID: 12477 RVA: 0x001315A8 File Offset: 0x0012F7A8
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

	// Token: 0x060030BE RID: 12478 RVA: 0x00131618 File Offset: 0x0012F818
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

	// Token: 0x060030BF RID: 12479 RVA: 0x00131724 File Offset: 0x0012F924
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

	// Token: 0x0400349C RID: 13468
	[OnEnterPlay_SetNull]
	public static volatile FriendSystem Instance;

	// Token: 0x0400349D RID: 13469
	[SerializeField]
	private float friendRequestExpirationTime = 10f;

	// Token: 0x0400349E RID: 13470
	private FriendSystem.PlayerPrivacy localPlayerPrivacy;

	// Token: 0x0400349F RID: 13471
	private List<FriendSystem.FriendRequestData> pendingFriendRequests = new List<FriendSystem.FriendRequestData>();

	// Token: 0x040034A0 RID: 13472
	private List<FriendSystem.FriendRemovalData> pendingFriendRemovals = new List<FriendSystem.FriendRemovalData>();

	// Token: 0x040034A1 RID: 13473
	private List<int> indexesToRemove = new List<int>();

	// Token: 0x040034A3 RID: 13475
	private float lastFriendsListRefresh;

	// Token: 0x020007AD RID: 1965
	// (Invoke) Token: 0x060030C2 RID: 12482
	public delegate void FriendRequestCallback(GTZone zone, int localId, int friendId, bool success);

	// Token: 0x020007AE RID: 1966
	private struct FriendRequestData
	{
		// Token: 0x040034A4 RID: 13476
		public GTZone zone;

		// Token: 0x040034A5 RID: 13477
		public int sendingPlayerId;

		// Token: 0x040034A6 RID: 13478
		public int targetPlayerId;

		// Token: 0x040034A7 RID: 13479
		public float localTimeSent;

		// Token: 0x040034A8 RID: 13480
		public FriendSystem.FriendRequestCallback completionCallback;
	}

	// Token: 0x020007AF RID: 1967
	// (Invoke) Token: 0x060030C6 RID: 12486
	public delegate void FriendRemovalCallback(int friendId, bool success);

	// Token: 0x020007B0 RID: 1968
	private struct FriendRemovalData
	{
		// Token: 0x040034A9 RID: 13481
		public int targetPlayerId;

		// Token: 0x040034AA RID: 13482
		public float localTimeSent;

		// Token: 0x040034AB RID: 13483
		public FriendSystem.FriendRemovalCallback completionCallback;
	}

	// Token: 0x020007B1 RID: 1969
	private enum FriendRequestStatus
	{
		// Token: 0x040034AD RID: 13485
		Pending,
		// Token: 0x040034AE RID: 13486
		Succeeded,
		// Token: 0x040034AF RID: 13487
		Failed
	}

	// Token: 0x020007B2 RID: 1970
	public enum PlayerPrivacy
	{
		// Token: 0x040034B1 RID: 13489
		Visible,
		// Token: 0x040034B2 RID: 13490
		PublicOnly,
		// Token: 0x040034B3 RID: 13491
		Hidden
	}
}
