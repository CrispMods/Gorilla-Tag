using System;
using System.Collections.Generic;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007B3 RID: 1971
public class MockFriendServer : MonoBehaviourPun
{
	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x060030C9 RID: 12489 RVA: 0x000504EA File Offset: 0x0004E6EA
	public int LocalPlayerId
	{
		get
		{
			return PhotonNetwork.LocalPlayer.UserId.GetHashCode();
		}
	}

	// Token: 0x060030CA RID: 12490 RVA: 0x000504FB File Offset: 0x0004E6FB
	private void Awake()
	{
		if (MockFriendServer.Instance == null)
		{
			MockFriendServer.Instance = this;
			PhotonNetwork.AddCallbackTarget(this);
			NetworkSystem.Instance.OnMultiplayerStarted += this.OnMultiplayerStarted;
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060030CB RID: 12491 RVA: 0x00050539 File Offset: 0x0004E739
	private void OnMultiplayerStarted()
	{
		this.RegisterLocalPlayer(this.LocalPlayerId);
	}

	// Token: 0x060030CC RID: 12492 RVA: 0x00131810 File Offset: 0x0012FA10
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

	// Token: 0x060030CD RID: 12493 RVA: 0x00131ABC File Offset: 0x0012FCBC
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

	// Token: 0x060030CE RID: 12494 RVA: 0x00131B1C File Offset: 0x0012FD1C
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

	// Token: 0x060030CF RID: 12495 RVA: 0x00131B74 File Offset: 0x0012FD74
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

	// Token: 0x060030D0 RID: 12496 RVA: 0x00131BCC File Offset: 0x0012FDCC
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

	// Token: 0x060030D1 RID: 12497 RVA: 0x00131C54 File Offset: 0x0012FE54
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
				float num = UnityEngine.Random.Range(this.friendRequestCompletionDelayRange.x, this.friendRequestCompletionDelayRange.y);
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

	// Token: 0x060030D2 RID: 12498 RVA: 0x00050547 File Offset: 0x0004E747
	[PunRPC]
	public void RequestAddFriendRPC(int localPlayerPublicId, int otherPlayerPublicId, PhotonMessageInfo info)
	{
		this.RequestAddFriendInternal(localPlayerPublicId, otherPlayerPublicId);
	}

	// Token: 0x060030D3 RID: 12499 RVA: 0x00131D14 File Offset: 0x0012FF14
	private void RequestRemoveFriendInternal(int localPlayerPublicId, int otherPlayerPublicId)
	{
		int privateIdA;
		int privateIdB;
		if (base.photonView.IsMine && this.TryLookupPrivateId(localPlayerPublicId, out privateIdA) && this.TryLookupPrivateId(otherPlayerPublicId, out privateIdB))
		{
			this.RemoveFriend(privateIdA, privateIdB);
		}
	}

	// Token: 0x060030D4 RID: 12500 RVA: 0x00050551 File Offset: 0x0004E751
	[PunRPC]
	public void RequestRemoveFriendRPC(int localPlayerPublicId, int otherPlayerPublicId, PhotonMessageInfo info)
	{
		this.RequestRemoveFriendInternal(localPlayerPublicId, otherPlayerPublicId);
	}

	// Token: 0x060030D5 RID: 12501 RVA: 0x00131D4C File Offset: 0x0012FF4C
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

	// Token: 0x060030D6 RID: 12502 RVA: 0x0005055B File Offset: 0x0004E75B
	[PunRPC]
	public void RegisterLocalPlayerRPC(int playerPublicId, int playerPrivateId, PhotonMessageInfo info)
	{
		this.RegisterLocalPlayerInternal(playerPublicId, playerPrivateId);
	}

	// Token: 0x060030D7 RID: 12503 RVA: 0x00050565 File Offset: 0x0004E765
	[PunRPC]
	public void AddFriendPairRPC(int publicIdA, int publicIdB, int privateIdA, int privateIdB, PhotonMessageInfo info)
	{
		this.AddFriend(publicIdA, publicIdB, privateIdA, privateIdB);
	}

	// Token: 0x060030D8 RID: 12504 RVA: 0x00131E00 File Offset: 0x00130000
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

	// Token: 0x060030D9 RID: 12505 RVA: 0x00131EAC File Offset: 0x001300AC
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

	// Token: 0x060030DA RID: 12506 RVA: 0x00131F64 File Offset: 0x00130164
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

	// Token: 0x040034B4 RID: 13492
	[OnEnterPlay_SetNull]
	public static volatile MockFriendServer Instance;

	// Token: 0x040034B5 RID: 13493
	[SerializeField]
	private Vector2 friendRequestCompletionDelayRange = new Vector2(0.5f, 1f);

	// Token: 0x040034B6 RID: 13494
	[SerializeField]
	private float friendRequestExpirationTime = 10f;

	// Token: 0x040034B7 RID: 13495
	private List<MockFriendServer.FriendPair> friendPairList = new List<MockFriendServer.FriendPair>();

	// Token: 0x040034B8 RID: 13496
	private List<MockFriendServer.PrivateIdEncryptionPlaceholder> privateIdLookup = new List<MockFriendServer.PrivateIdEncryptionPlaceholder>();

	// Token: 0x040034B9 RID: 13497
	private List<MockFriendServer.FriendRequest> friendRequests = new List<MockFriendServer.FriendRequest>();

	// Token: 0x040034BA RID: 13498
	private List<int> indexesToRemove = new List<int>();

	// Token: 0x020007B4 RID: 1972
	public struct FriendPair
	{
		// Token: 0x040034BB RID: 13499
		public int publicIdPlayerA;

		// Token: 0x040034BC RID: 13500
		public int publicIdPlayerB;

		// Token: 0x040034BD RID: 13501
		public int privateIdPlayerA;

		// Token: 0x040034BE RID: 13502
		public int privateIdPlayerB;
	}

	// Token: 0x020007B5 RID: 1973
	public struct PrivateIdEncryptionPlaceholder
	{
		// Token: 0x040034BF RID: 13503
		public int playerPublicId;

		// Token: 0x040034C0 RID: 13504
		public int playerPrivateId;
	}

	// Token: 0x020007B6 RID: 1974
	public struct FriendRequest
	{
		// Token: 0x040034C1 RID: 13505
		public int requestorPublicId;

		// Token: 0x040034C2 RID: 13506
		public int requesteePublicId;

		// Token: 0x040034C3 RID: 13507
		public float requestTime;

		// Token: 0x040034C4 RID: 13508
		public float completionTime;
	}
}
