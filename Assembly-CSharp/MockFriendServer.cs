using System;
using System.Collections.Generic;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200079B RID: 1947
public class MockFriendServer : MonoBehaviourPun
{
	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06003017 RID: 12311 RVA: 0x000E8232 File Offset: 0x000E6432
	public int LocalPlayerId
	{
		get
		{
			return PhotonNetwork.LocalPlayer.UserId.GetHashCode();
		}
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x000E8243 File Offset: 0x000E6443
	private void Awake()
	{
		if (MockFriendServer.Instance == null)
		{
			MockFriendServer.Instance = this;
			PhotonNetwork.AddCallbackTarget(this);
			NetworkSystem.Instance.OnMultiplayerStarted += this.OnMultiplayerStarted;
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x000E8281 File Offset: 0x000E6481
	private void OnMultiplayerStarted()
	{
		this.RegisterLocalPlayer(this.LocalPlayerId);
	}

	// Token: 0x0600301A RID: 12314 RVA: 0x000E8290 File Offset: 0x000E6490
	private void Update()
	{
		if (PhotonNetwork.InRoom && base.photonView.IsMine)
		{
			this.indexesToRemove.Clear();
			for (int i = 0; i < this.friendRequests.Count; i++)
			{
				if (this.friendRequests[i].requestTime + this.friendRequestExpirationTime < Time.time)
				{
					this.indexesToRemove.Add(i);
				}
			}
			for (int j = 0; j < this.indexesToRemove.Count; j++)
			{
				this.friendRequests.RemoveAt(this.indexesToRemove[j]);
			}
			this.indexesToRemove.Clear();
			for (int k = 0; k < this.friendRequests.Count; k++)
			{
				if (this.friendRequests[k].requestTime + this.friendRequestExpirationTime < Time.time)
				{
					this.indexesToRemove.Add(k);
				}
				else if (this.friendRequests[k].completionTime < Time.time)
				{
					for (int l = k + 1; l < this.friendRequests.Count; l++)
					{
						int num;
						int num2;
						if (this.friendRequests[l].completionTime < Time.time && this.friendRequests[k].requestorPublicId == this.friendRequests[l].requesteePublicId && this.friendRequests[k].requesteePublicId == this.friendRequests[l].requestorPublicId && this.TryLookupPrivateId(this.friendRequests[k].requestorPublicId, out num) && this.TryLookupPrivateId(this.friendRequests[k].requesteePublicId, out num2))
						{
							this.AddFriend(this.friendRequests[k].requestorPublicId, this.friendRequests[k].requesteePublicId, num, num2);
							this.indexesToRemove.Add(l);
							this.indexesToRemove.Add(k);
							base.photonView.RPC("AddFriendPairRPC", RpcTarget.Others, new object[]
							{
								this.friendRequests[k].requestorPublicId,
								this.friendRequests[k].requesteePublicId,
								num,
								num2
							});
							break;
						}
					}
				}
			}
			for (int m = 0; m < this.indexesToRemove.Count; m++)
			{
				this.friendRequests.RemoveAt(this.indexesToRemove[m]);
			}
		}
	}

	// Token: 0x0600301B RID: 12315 RVA: 0x000E853C File Offset: 0x000E673C
	public void RegisterLocalPlayer(int localPlayerPublicId)
	{
		int hashCode = PlayFabAuthenticator.instance.GetPlayFabPlayerId().GetHashCode();
		if (base.photonView.IsMine)
		{
			this.RegisterLocalPlayerInternal(localPlayerPublicId, hashCode);
			return;
		}
		base.photonView.RPC("RegisterLocalPlayerRPC", RpcTarget.MasterClient, new object[]
		{
			localPlayerPublicId,
			hashCode
		});
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x000E859C File Offset: 0x000E679C
	public void RequestAddFriend(int targetPlayerId)
	{
		if (base.photonView.IsMine)
		{
			this.RequestAddFriendInternal(this.LocalPlayerId, targetPlayerId);
			return;
		}
		base.photonView.RPC("RequestAddFriendRPC", RpcTarget.MasterClient, new object[]
		{
			this.LocalPlayerId,
			targetPlayerId
		});
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x000E85F4 File Offset: 0x000E67F4
	public void RequestRemoveFriend(int targetPlayerId)
	{
		if (base.photonView.IsMine)
		{
			this.RequestRemoveFriendInternal(this.LocalPlayerId, targetPlayerId);
			return;
		}
		base.photonView.RPC("RequestRemoveFriendRPC", RpcTarget.MasterClient, new object[]
		{
			this.LocalPlayerId,
			targetPlayerId
		});
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x000E864C File Offset: 0x000E684C
	public void GetFriendList(List<int> friendListResult)
	{
		int localPlayerId = this.LocalPlayerId;
		friendListResult.Clear();
		for (int i = 0; i < this.friendPairList.Count; i++)
		{
			if (this.friendPairList[i].publicIdPlayerA == localPlayerId)
			{
				friendListResult.Add(this.friendPairList[i].publicIdPlayerB);
			}
			else if (this.friendPairList[i].publicIdPlayerB == localPlayerId)
			{
				friendListResult.Add(this.friendPairList[i].publicIdPlayerA);
			}
		}
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x000E86D4 File Offset: 0x000E68D4
	private void RequestAddFriendInternal(int localPlayerPublicId, int otherPlayerPublicId)
	{
		if (base.photonView.IsMine)
		{
			bool flag = false;
			for (int i = 0; i < this.friendRequests.Count; i++)
			{
				if (this.friendRequests[i].requestorPublicId == localPlayerPublicId && this.friendRequests[i].requesteePublicId == otherPlayerPublicId)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				float time = Time.time;
				float num = Random.Range(this.friendRequestCompletionDelayRange.x, this.friendRequestCompletionDelayRange.y);
				this.friendRequests.Add(new MockFriendServer.FriendRequest
				{
					requestorPublicId = localPlayerPublicId,
					requesteePublicId = otherPlayerPublicId,
					requestTime = time,
					completionTime = time + num
				});
			}
		}
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x000E8791 File Offset: 0x000E6991
	[PunRPC]
	public void RequestAddFriendRPC(int localPlayerPublicId, int otherPlayerPublicId, PhotonMessageInfo info)
	{
		this.RequestAddFriendInternal(localPlayerPublicId, otherPlayerPublicId);
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x000E879C File Offset: 0x000E699C
	private void RequestRemoveFriendInternal(int localPlayerPublicId, int otherPlayerPublicId)
	{
		int privateIdA;
		int privateIdB;
		if (base.photonView.IsMine && this.TryLookupPrivateId(localPlayerPublicId, out privateIdA) && this.TryLookupPrivateId(otherPlayerPublicId, out privateIdB))
		{
			this.RemoveFriend(privateIdA, privateIdB);
		}
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x000E87D4 File Offset: 0x000E69D4
	[PunRPC]
	public void RequestRemoveFriendRPC(int localPlayerPublicId, int otherPlayerPublicId, PhotonMessageInfo info)
	{
		this.RequestRemoveFriendInternal(localPlayerPublicId, otherPlayerPublicId);
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x000E87E0 File Offset: 0x000E69E0
	private void RegisterLocalPlayerInternal(int publicId, int privateId)
	{
		if (base.photonView.IsMine)
		{
			bool flag = false;
			for (int i = 0; i < this.privateIdLookup.Count; i++)
			{
				if (publicId == this.privateIdLookup[i].playerPublicId || privateId == this.privateIdLookup[i].playerPrivateId)
				{
					MockFriendServer.PrivateIdEncryptionPlaceholder value = this.privateIdLookup[i];
					value.playerPublicId = publicId;
					value.playerPrivateId = privateId;
					this.privateIdLookup[i] = value;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.privateIdLookup.Add(new MockFriendServer.PrivateIdEncryptionPlaceholder
				{
					playerPublicId = publicId,
					playerPrivateId = privateId
				});
			}
		}
	}

	// Token: 0x06003024 RID: 12324 RVA: 0x000E8892 File Offset: 0x000E6A92
	[PunRPC]
	public void RegisterLocalPlayerRPC(int playerPublicId, int playerPrivateId, PhotonMessageInfo info)
	{
		this.RegisterLocalPlayerInternal(playerPublicId, playerPrivateId);
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x000E889C File Offset: 0x000E6A9C
	[PunRPC]
	public void AddFriendPairRPC(int publicIdA, int publicIdB, int privateIdA, int privateIdB, PhotonMessageInfo info)
	{
		this.AddFriend(publicIdA, publicIdB, privateIdA, privateIdB);
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x000E88AC File Offset: 0x000E6AAC
	private void AddFriend(int publicIdA, int publicIdB, int privateIdA, int privateIdB)
	{
		for (int i = 0; i < this.friendPairList.Count; i++)
		{
			if ((this.friendPairList[i].privateIdPlayerA == privateIdA && this.friendPairList[i].privateIdPlayerB == privateIdB) || (this.friendPairList[i].privateIdPlayerA == privateIdB && this.friendPairList[i].privateIdPlayerB == privateIdA))
			{
				return;
			}
		}
		this.friendPairList.Add(new MockFriendServer.FriendPair
		{
			publicIdPlayerA = publicIdA,
			publicIdPlayerB = publicIdB,
			privateIdPlayerA = privateIdA,
			privateIdPlayerB = privateIdB
		});
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x000E8958 File Offset: 0x000E6B58
	private void RemoveFriend(int privateIdA, int privateIdB)
	{
		this.indexesToRemove.Clear();
		for (int i = 0; i < this.friendPairList.Count; i++)
		{
			if ((this.friendPairList[i].privateIdPlayerA == privateIdA && this.friendPairList[i].privateIdPlayerB == privateIdB) || (this.friendPairList[i].privateIdPlayerA == privateIdB && this.friendPairList[i].privateIdPlayerB == privateIdA))
			{
				this.indexesToRemove.Add(i);
			}
		}
		for (int j = 0; j < this.friendPairList.Count; j++)
		{
			this.friendPairList.RemoveAt(this.indexesToRemove[j]);
		}
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x000E8A10 File Offset: 0x000E6C10
	private bool TryLookupPrivateId(int publicId, out int privateId)
	{
		for (int i = 0; i < this.privateIdLookup.Count; i++)
		{
			if (this.privateIdLookup[i].playerPublicId == publicId)
			{
				privateId = this.privateIdLookup[i].playerPrivateId;
				return true;
			}
		}
		privateId = -1;
		return false;
	}

	// Token: 0x0400340A RID: 13322
	[OnEnterPlay_SetNull]
	public static volatile MockFriendServer Instance;

	// Token: 0x0400340B RID: 13323
	[SerializeField]
	private Vector2 friendRequestCompletionDelayRange = new Vector2(0.5f, 1f);

	// Token: 0x0400340C RID: 13324
	[SerializeField]
	private float friendRequestExpirationTime = 10f;

	// Token: 0x0400340D RID: 13325
	private List<MockFriendServer.FriendPair> friendPairList = new List<MockFriendServer.FriendPair>();

	// Token: 0x0400340E RID: 13326
	private List<MockFriendServer.PrivateIdEncryptionPlaceholder> privateIdLookup = new List<MockFriendServer.PrivateIdEncryptionPlaceholder>();

	// Token: 0x0400340F RID: 13327
	private List<MockFriendServer.FriendRequest> friendRequests = new List<MockFriendServer.FriendRequest>();

	// Token: 0x04003410 RID: 13328
	private List<int> indexesToRemove = new List<int>();

	// Token: 0x0200079C RID: 1948
	public struct FriendPair
	{
		// Token: 0x04003411 RID: 13329
		public int publicIdPlayerA;

		// Token: 0x04003412 RID: 13330
		public int publicIdPlayerB;

		// Token: 0x04003413 RID: 13331
		public int privateIdPlayerA;

		// Token: 0x04003414 RID: 13332
		public int privateIdPlayerB;
	}

	// Token: 0x0200079D RID: 1949
	public struct PrivateIdEncryptionPlaceholder
	{
		// Token: 0x04003415 RID: 13333
		public int playerPublicId;

		// Token: 0x04003416 RID: 13334
		public int playerPrivateId;
	}

	// Token: 0x0200079E RID: 1950
	public struct FriendRequest
	{
		// Token: 0x04003417 RID: 13335
		public int requestorPublicId;

		// Token: 0x04003418 RID: 13336
		public int requesteePublicId;

		// Token: 0x04003419 RID: 13337
		public float requestTime;

		// Token: 0x0400341A RID: 13338
		public float completionTime;
	}
}
