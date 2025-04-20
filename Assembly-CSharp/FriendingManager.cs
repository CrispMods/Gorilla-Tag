using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007A8 RID: 1960
public class FriendingManager : MonoBehaviourPun, IPunObservable, IGorillaSliceableSimple
{
	// Token: 0x0600307D RID: 12413 RVA: 0x00050328 File Offset: 0x0004E528
	private void Awake()
	{
		if (FriendingManager.Instance == null)
		{
			FriendingManager.Instance = this;
			PhotonNetwork.AddCallbackTarget(this);
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x0012F5E4 File Offset: 0x0012D7E4
	private void Start()
	{
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
		NetworkSystem.Instance.OnMultiplayerStarted += this.ValidateState;
		NetworkSystem.Instance.OnReturnedToSinglePlayer += this.ValidateState;
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x0012F634 File Offset: 0x0012D834
	private void OnDestroy()
	{
		if (NetworkSystem.Instance != null)
		{
			NetworkSystem.Instance.OnPlayerLeft -= this.OnPlayerLeftRoom;
			NetworkSystem.Instance.OnMultiplayerStarted -= this.ValidateState;
			NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.ValidateState;
		}
	}

	// Token: 0x06003080 RID: 12416 RVA: 0x00032C89 File Offset: 0x00030E89
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06003081 RID: 12417 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x00050350 File Offset: 0x0004E550
	public void SliceUpdate()
	{
		this.AuthorityUpdate();
	}

	// Token: 0x06003083 RID: 12419 RVA: 0x0012F690 File Offset: 0x0012D890
	private void AuthorityUpdate()
	{
		if (PhotonNetwork.InRoom && base.photonView.IsMine)
		{
			for (int i = 0; i < this.activeFriendStationData.Count; i++)
			{
				if (this.activeFriendStationData[i].state >= FriendingManager.FriendStationState.ButtonConfirmationTimer0 && this.activeFriendStationData[i].state <= FriendingManager.FriendStationState.ButtonConfirmationTimer4)
				{
					FriendingManager.FriendStationData friendStationData = this.activeFriendStationData[i];
					int num = 4;
					float num2 = (Time.time - friendStationData.progressBarStartTime) / this.progressBarDuration;
					if (num2 < 1f)
					{
						int num3 = Mathf.RoundToInt(num2 * (float)num);
						friendStationData.state = num3 + FriendingManager.FriendStationState.ButtonConfirmationTimer0;
					}
					else
					{
						base.photonView.RPC("NotifyClientsFriendRequestReadyRPC", RpcTarget.All, new object[]
						{
							friendStationData.zone
						});
						friendStationData.state = FriendingManager.FriendStationState.WaitingOnRequestBoth;
					}
					this.activeFriendStationData[i] = friendStationData;
				}
			}
		}
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x00050358 File Offset: 0x0004E558
	private void OnPlayerLeftRoom(NetPlayer player)
	{
		this.ValidateState();
	}

	// Token: 0x06003085 RID: 12421 RVA: 0x0012F780 File Offset: 0x0012D980
	private void ValidateState()
	{
		for (int i = 0; i < this.activeFriendStationData.Count; i++)
		{
			FriendingManager.FriendStationData friendStationData = this.activeFriendStationData[i];
			if (friendStationData.actorNumberA != -1 && NetworkSystem.Instance.GetNetPlayerByID(friendStationData.actorNumberA) == null)
			{
				friendStationData.actorNumberA = -1;
			}
			if (friendStationData.actorNumberB != -1 && NetworkSystem.Instance.GetNetPlayerByID(friendStationData.actorNumberB) == null)
			{
				friendStationData.actorNumberB = -1;
			}
			if (friendStationData.actorNumberA == -1 || friendStationData.actorNumberB == -1)
			{
				friendStationData.state = FriendingManager.FriendStationState.WaitingForPlayers;
			}
			this.activeFriendStationData[i] = friendStationData;
		}
		this.UpdateFriendingStations();
	}

	// Token: 0x06003086 RID: 12422 RVA: 0x0012F828 File Offset: 0x0012DA28
	private void UpdateFriendingStations()
	{
		for (int i = 0; i < this.activeFriendStationData.Count; i++)
		{
			FriendingStation friendingStation;
			if (this.friendingStations.TryGetValue(this.activeFriendStationData[i].zone, out friendingStation))
			{
				friendingStation.UpdateState(this.activeFriendStationData[i]);
			}
		}
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x00050360 File Offset: 0x0004E560
	public void RegisterFriendingStation(FriendingStation friendingStation)
	{
		if (!this.friendingStations.ContainsKey(friendingStation.Zone))
		{
			this.friendingStations.Add(friendingStation.Zone, friendingStation);
		}
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x00050387 File Offset: 0x0004E587
	public void UnregisterFriendingStation(FriendingStation friendingStation)
	{
		this.friendingStations.Remove(friendingStation.Zone);
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x0012F880 File Offset: 0x0012DA80
	private void DebugLogFriendingStations()
	{
		string text = string.Format("Friending Stations: Count: {0} ", this.friendingStations.Count);
		foreach (KeyValuePair<GTZone, FriendingStation> keyValuePair in this.friendingStations)
		{
			text += string.Format("Station Zone {0}", keyValuePair.Key);
		}
		Debug.Log(text);
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x0012F90C File Offset: 0x0012DB0C
	public void PlayerEnteredStation(GTZone zone, NetPlayer netPlayer)
	{
		if (netPlayer != null && netPlayer.ActorNumber == NetworkSystem.Instance.LocalPlayer.ActorNumber)
		{
			this.localPlayerZone = zone;
		}
		if (PhotonNetwork.InRoom && base.photonView.IsMine)
		{
			int num = -1;
			int i = 0;
			while (i < this.activeFriendStationData.Count)
			{
				if (this.activeFriendStationData[i].zone == zone)
				{
					num = i;
					if (this.activeFriendStationData[i].actorNumberA == -1 && this.activeFriendStationData[i].actorNumberB != netPlayer.ActorNumber)
					{
						FriendingManager.FriendStationData friendStationData = this.activeFriendStationData[i];
						friendStationData.actorNumberA = netPlayer.ActorNumber;
						if (friendStationData.actorNumberA != -1 && friendStationData.actorNumberB != -1)
						{
							friendStationData.state = FriendingManager.FriendStationState.WaitingOnFriendStatusBoth;
						}
						else
						{
							friendStationData.state = FriendingManager.FriendStationState.WaitingForPlayers;
						}
						this.activeFriendStationData[i] = friendStationData;
					}
					else if (this.activeFriendStationData[i].actorNumberA != -1 && this.activeFriendStationData[i].actorNumberA != netPlayer.ActorNumber && this.activeFriendStationData[i].actorNumberB == -1)
					{
						FriendingManager.FriendStationData friendStationData2 = this.activeFriendStationData[i];
						friendStationData2.actorNumberB = netPlayer.ActorNumber;
						if (friendStationData2.actorNumberA != -1 && friendStationData2.actorNumberB != -1)
						{
							friendStationData2.state = FriendingManager.FriendStationState.WaitingOnFriendStatusBoth;
						}
						else
						{
							friendStationData2.state = FriendingManager.FriendStationState.WaitingForPlayers;
						}
						this.activeFriendStationData[i] = friendStationData2;
					}
					if (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnFriendStatusBoth)
					{
						base.photonView.RPC("CheckFriendStatusRequestRPC", RpcTarget.All, new object[]
						{
							this.activeFriendStationData[i].zone,
							this.activeFriendStationData[i].actorNumberA,
							this.activeFriendStationData[i].actorNumberB
						});
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (num < 0)
			{
				this.activeFriendStationData.Add(new FriendingManager.FriendStationData
				{
					zone = zone,
					actorNumberA = netPlayer.ActorNumber,
					actorNumberB = -1,
					state = FriendingManager.FriendStationState.WaitingForPlayers
				});
			}
			this.UpdateFriendingStations();
		}
	}

	// Token: 0x0600308B RID: 12427 RVA: 0x0012FB50 File Offset: 0x0012DD50
	public void PlayerExitedStation(GTZone zone, NetPlayer netPlayer)
	{
		if (netPlayer != null && netPlayer.ActorNumber == NetworkSystem.Instance.LocalPlayer.ActorNumber)
		{
			this.localPlayerZone = GTZone.none;
		}
		if (PhotonNetwork.InRoom && base.photonView.IsMine)
		{
			int num = -1;
			int i = 0;
			while (i < this.activeFriendStationData.Count)
			{
				if (this.activeFriendStationData[i].zone == zone)
				{
					if ((this.activeFriendStationData[i].actorNumberA == netPlayer.ActorNumber && this.activeFriendStationData[i].actorNumberB == -1) || (this.activeFriendStationData[i].actorNumberA == -1 && this.activeFriendStationData[i].actorNumberB == netPlayer.ActorNumber))
					{
						FriendingManager.FriendStationData value = this.activeFriendStationData[i];
						value.actorNumberA = -1;
						value.actorNumberB = -1;
						value.state = FriendingManager.FriendStationState.WaitingForPlayers;
						this.activeFriendStationData[i] = value;
						num = i;
						break;
					}
					if (this.activeFriendStationData[i].actorNumberA != -1 && this.activeFriendStationData[i].actorNumberA != netPlayer.ActorNumber && this.activeFriendStationData[i].actorNumberB == netPlayer.ActorNumber)
					{
						FriendingManager.FriendStationData value2 = this.activeFriendStationData[i];
						value2.actorNumberB = -1;
						value2.state = FriendingManager.FriendStationState.WaitingForPlayers;
						this.activeFriendStationData[i] = value2;
						break;
					}
					if (this.activeFriendStationData[i].actorNumberB != -1 && this.activeFriendStationData[i].actorNumberB != netPlayer.ActorNumber && this.activeFriendStationData[i].actorNumberA == netPlayer.ActorNumber)
					{
						FriendingManager.FriendStationData value3 = this.activeFriendStationData[i];
						value3.actorNumberA = -1;
						value3.state = FriendingManager.FriendStationState.WaitingForPlayers;
						this.activeFriendStationData[i] = value3;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			this.UpdateFriendingStations();
			if (num >= 0)
			{
				base.photonView.RPC("StationNoLongerActiveRPC", RpcTarget.Others, new object[]
				{
					this.activeFriendStationData[num].zone
				});
				this.activeFriendStationData.RemoveAt(num);
			}
		}
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x0012FD90 File Offset: 0x0012DF90
	private void PlayerPressedButton(GTZone zone, int playerId)
	{
		if (PhotonNetwork.InRoom && base.photonView.IsMine)
		{
			int i = 0;
			while (i < this.activeFriendStationData.Count)
			{
				if (this.activeFriendStationData[i].zone == zone)
				{
					if (this.activeFriendStationData[i].actorNumberA == -1 || this.activeFriendStationData[i].actorNumberB == -1)
					{
						break;
					}
					if ((this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnButtonPlayerA && this.activeFriendStationData[i].actorNumberA == playerId) || (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnButtonPlayerB && this.activeFriendStationData[i].actorNumberB == playerId))
					{
						FriendingManager.FriendStationData value = this.activeFriendStationData[i];
						value.state = FriendingManager.FriendStationState.ButtonConfirmationTimer0;
						value.progressBarStartTime = Time.time;
						this.activeFriendStationData[i] = value;
						return;
					}
					if (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnButtonBoth && this.activeFriendStationData[i].actorNumberA == playerId)
					{
						FriendingManager.FriendStationData value2 = this.activeFriendStationData[i];
						value2.state = FriendingManager.FriendStationState.WaitingOnButtonPlayerB;
						this.activeFriendStationData[i] = value2;
						return;
					}
					if (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnButtonBoth && this.activeFriendStationData[i].actorNumberB == playerId)
					{
						FriendingManager.FriendStationData value3 = this.activeFriendStationData[i];
						value3.state = FriendingManager.FriendStationState.WaitingOnButtonPlayerA;
						this.activeFriendStationData[i] = value3;
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x0012FF30 File Offset: 0x0012E130
	private void PlayerUnpressedButton(GTZone zone, int playerId)
	{
		if (PhotonNetwork.InRoom && base.photonView.IsMine)
		{
			int i = 0;
			while (i < this.activeFriendStationData.Count)
			{
				if (this.activeFriendStationData[i].zone == zone)
				{
					if (this.activeFriendStationData[i].actorNumberA == -1 || this.activeFriendStationData[i].actorNumberB == -1)
					{
						break;
					}
					bool flag = this.activeFriendStationData[i].state >= FriendingManager.FriendStationState.ButtonConfirmationTimer0 && this.activeFriendStationData[i].state <= FriendingManager.FriendStationState.ButtonConfirmationTimer4;
					if (flag && this.activeFriendStationData[i].actorNumberA == playerId)
					{
						FriendingManager.FriendStationData value = this.activeFriendStationData[i];
						value.state = FriendingManager.FriendStationState.WaitingOnButtonPlayerA;
						this.activeFriendStationData[i] = value;
						return;
					}
					if (flag && this.activeFriendStationData[i].actorNumberB == playerId)
					{
						FriendingManager.FriendStationData value2 = this.activeFriendStationData[i];
						value2.state = FriendingManager.FriendStationState.WaitingOnButtonPlayerB;
						this.activeFriendStationData[i] = value2;
						return;
					}
					if ((this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnButtonPlayerA && this.activeFriendStationData[i].actorNumberB == playerId) || (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnButtonPlayerB && this.activeFriendStationData[i].actorNumberA == playerId))
					{
						FriendingManager.FriendStationData value3 = this.activeFriendStationData[i];
						value3.state = FriendingManager.FriendStationState.WaitingOnButtonBoth;
						this.activeFriendStationData[i] = value3;
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x0005039B File Offset: 0x0004E59B
	private void CheckFriendStatusRequest(GTZone zone, int actorNumberA, int actorNumberB)
	{
		FriendSystem.Instance.OnFriendListRefresh -= this.CheckFriendStatusOnFriendListRefresh;
		FriendSystem.Instance.OnFriendListRefresh += this.CheckFriendStatusOnFriendListRefresh;
		FriendSystem.Instance.RefreshFriendsList();
	}

	// Token: 0x0600308F RID: 12431 RVA: 0x001300D4 File Offset: 0x0012E2D4
	private void CheckFriendStatusOnFriendListRefresh(List<FriendBackendController.Friend> friendList)
	{
		FriendSystem.Instance.OnFriendListRefresh -= this.CheckFriendStatusOnFriendListRefresh;
		int i = 0;
		while (i < this.activeFriendStationData.Count)
		{
			if (this.activeFriendStationData[i].zone == this.localPlayerZone)
			{
				int actorNumber = NetworkSystem.Instance.LocalPlayer.ActorNumber;
				int num = -1;
				if (this.activeFriendStationData[i].actorNumberA == actorNumber)
				{
					num = this.activeFriendStationData[i].actorNumberB;
				}
				else if (this.activeFriendStationData[i].actorNumberB == actorNumber)
				{
					num = this.activeFriendStationData[i].actorNumberA;
				}
				if (num != -1 && FriendSystem.Instance.CheckFriendshipWithPlayer(num))
				{
					base.photonView.RPC("CheckFriendStatusResponseRPC", RpcTarget.MasterClient, new object[]
					{
						this.localPlayerZone,
						num,
						true
					});
					return;
				}
				base.photonView.RPC("CheckFriendStatusResponseRPC", RpcTarget.MasterClient, new object[]
				{
					this.localPlayerZone,
					num,
					false
				});
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06003090 RID: 12432 RVA: 0x00130214 File Offset: 0x0012E414
	private void CheckFriendStatusResponse(GTZone zone, int responderActorNumber, int friendTargetActorNumber, bool friends)
	{
		if (PhotonNetwork.InRoom && base.photonView.IsMine)
		{
			int i = 0;
			while (i < this.activeFriendStationData.Count)
			{
				if (this.activeFriendStationData[i].zone == zone)
				{
					if (this.activeFriendStationData[i].actorNumberA == -1 || this.activeFriendStationData[i].actorNumberB == -1)
					{
						break;
					}
					if ((this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnFriendStatusPlayerA && this.activeFriendStationData[i].actorNumberA == responderActorNumber) || (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnFriendStatusPlayerB && this.activeFriendStationData[i].actorNumberB == responderActorNumber))
					{
						FriendingManager.FriendStationData value = this.activeFriendStationData[i];
						if (friends)
						{
							value.state = FriendingManager.FriendStationState.AlreadyFriends;
						}
						else
						{
							value.state = FriendingManager.FriendStationState.WaitingOnButtonBoth;
						}
						this.activeFriendStationData[i] = value;
						return;
					}
					if (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnFriendStatusBoth && this.activeFriendStationData[i].actorNumberA == responderActorNumber)
					{
						FriendingManager.FriendStationData value2 = this.activeFriendStationData[i];
						if (friends)
						{
							value2.state = FriendingManager.FriendStationState.WaitingOnFriendStatusPlayerB;
						}
						else
						{
							value2.state = FriendingManager.FriendStationState.WaitingOnButtonBoth;
						}
						this.activeFriendStationData[i] = value2;
						return;
					}
					if (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnFriendStatusBoth && this.activeFriendStationData[i].actorNumberB == responderActorNumber)
					{
						FriendingManager.FriendStationData value3 = this.activeFriendStationData[i];
						if (friends)
						{
							value3.state = FriendingManager.FriendStationState.WaitingOnFriendStatusPlayerA;
						}
						else
						{
							value3.state = FriendingManager.FriendStationState.WaitingOnButtonBoth;
						}
						this.activeFriendStationData[i] = value3;
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x06003091 RID: 12433 RVA: 0x001303D0 File Offset: 0x0012E5D0
	private void SendFriendRequestIfApplicable(GTZone zone)
	{
		int i = 0;
		while (i < this.activeFriendStationData.Count)
		{
			if (this.activeFriendStationData[i].zone == zone)
			{
				FriendingManager.FriendStationData friendStationData = this.activeFriendStationData[i];
				int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
				NetPlayer netPlayer = null;
				if (friendStationData.actorNumberA == actorNumber)
				{
					netPlayer = NetworkSystem.Instance.GetNetPlayerByID(friendStationData.actorNumberB);
				}
				else if (friendStationData.actorNumberB == actorNumber)
				{
					netPlayer = NetworkSystem.Instance.GetNetPlayerByID(friendStationData.actorNumberA);
				}
				if (netPlayer == null)
				{
					return;
				}
				FriendingStation friendingStation;
				if (this.friendingStations.TryGetValue(friendStationData.zone, out friendingStation) && (GTPlayer.Instance.HeadCenterPosition - friendingStation.transform.position).sqrMagnitude < this.requiredProximityToStation * this.requiredProximityToStation)
				{
					FriendSystem.Instance.SendFriendRequest(netPlayer, friendStationData.zone, new FriendSystem.FriendRequestCallback(this.FriendRequestCallback));
				}
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06003092 RID: 12434 RVA: 0x001304CC File Offset: 0x0012E6CC
	private void FriendRequestCompletedAuthority(GTZone zone, int playerId, bool succeeded)
	{
		if (PhotonNetwork.InRoom && base.photonView.IsMine)
		{
			int i = 0;
			while (i < this.activeFriendStationData.Count)
			{
				if (this.activeFriendStationData[i].zone == zone)
				{
					if (!succeeded)
					{
						FriendingManager.FriendStationData value = this.activeFriendStationData[i];
						value.state = FriendingManager.FriendStationState.RequestFailed;
						this.activeFriendStationData[i] = value;
						return;
					}
					if ((this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnRequestPlayerA && this.activeFriendStationData[i].actorNumberA == playerId) || (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnRequestPlayerB && this.activeFriendStationData[i].actorNumberB == playerId))
					{
						FriendingManager.FriendStationData value2 = this.activeFriendStationData[i];
						value2.state = FriendingManager.FriendStationState.Friends;
						this.activeFriendStationData[i] = value2;
						return;
					}
					if (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnRequestBoth && this.activeFriendStationData[i].actorNumberA == playerId)
					{
						FriendingManager.FriendStationData value3 = this.activeFriendStationData[i];
						value3.state = FriendingManager.FriendStationState.WaitingOnRequestPlayerB;
						this.activeFriendStationData[i] = value3;
						return;
					}
					if (this.activeFriendStationData[i].state == FriendingManager.FriendStationState.WaitingOnRequestBoth && this.activeFriendStationData[i].actorNumberB == playerId)
					{
						FriendingManager.FriendStationData value4 = this.activeFriendStationData[i];
						value4.state = FriendingManager.FriendStationState.WaitingOnRequestPlayerA;
						this.activeFriendStationData[i] = value4;
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x00130664 File Offset: 0x0012E864
	private void FriendRequestCallback(GTZone zone, int localId, int friendId, bool success)
	{
		if (base.photonView.IsMine)
		{
			this.FriendRequestCompletedAuthority(zone, PhotonNetwork.LocalPlayer.ActorNumber, success);
			return;
		}
		base.photonView.RPC("FriendRequestCompletedRPC", RpcTarget.MasterClient, new object[]
		{
			zone,
			success
		});
	}

	// Token: 0x06003094 RID: 12436 RVA: 0x001306BC File Offset: 0x0012E8BC
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(this.activeFriendStationData.Count);
			for (int i = 0; i < this.activeFriendStationData.Count; i++)
			{
				FriendingManager.<OnPhotonSerializeView>g__SendFriendStationData|31_0(stream, this.activeFriendStationData[i]);
			}
		}
		else if (stream.IsReading && info.Sender.IsMasterClient)
		{
			int num = (int)stream.ReceiveNext();
			if (num >= 0 && num <= 10)
			{
				this.activeFriendStationData.Clear();
				for (int j = 0; j < num; j++)
				{
					this.activeFriendStationData.Add(FriendingManager.<OnPhotonSerializeView>g__ReceiveFriendStationData|31_1(stream));
				}
			}
		}
		this.UpdateFriendingStations();
	}

	// Token: 0x06003095 RID: 12437 RVA: 0x0013076C File Offset: 0x0012E96C
	[PunRPC]
	public void CheckFriendStatusRequestRPC(GTZone zone, int actorNumberA, int actorNumberB, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "CheckFriendStatusRequestRPC");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(info.Sender), out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[12].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		this.CheckFriendStatusRequest(zone, actorNumberA, actorNumberB);
	}

	// Token: 0x06003096 RID: 12438 RVA: 0x001307D4 File Offset: 0x0012E9D4
	[PunRPC]
	public void CheckFriendStatusResponseRPC(GTZone zone, int friendTargetActorNumber, bool friends, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "CheckFriendStatusRequestRPC");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(info.Sender), out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[12].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		this.CheckFriendStatusResponse(zone, info.Sender.ActorNumber, friendTargetActorNumber, friends);
	}

	// Token: 0x06003097 RID: 12439 RVA: 0x00130848 File Offset: 0x0012EA48
	[PunRPC]
	public void FriendButtonPressedRPC(GTZone zone, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "FriendButtonPressedRPC");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(info.Sender), out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[12].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		this.PlayerPressedButton(zone, info.Sender.ActorNumber);
	}

	// Token: 0x06003098 RID: 12440 RVA: 0x001308B8 File Offset: 0x0012EAB8
	[PunRPC]
	public void FriendButtonUnpressedRPC(GTZone zone, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "FriendButtonUnpressedRPC");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(info.Sender), out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[12].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		this.PlayerUnpressedButton(zone, info.Sender.ActorNumber);
	}

	// Token: 0x06003099 RID: 12441 RVA: 0x00130928 File Offset: 0x0012EB28
	[PunRPC]
	public void StationNoLongerActiveRPC(GTZone zone, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "StationNoLongerActiveRPC");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(info.Sender), out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[12].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		FriendingStation friendingStation;
		if (info.Sender.IsMasterClient && this.friendingStations.TryGetValue(zone, out friendingStation))
		{
			friendingStation.UpdateState(new FriendingManager.FriendStationData
			{
				zone = zone,
				actorNumberA = -1,
				actorNumberB = -1,
				state = FriendingManager.FriendStationState.WaitingForPlayers
			});
		}
	}

	// Token: 0x0600309A RID: 12442 RVA: 0x001309D0 File Offset: 0x0012EBD0
	[PunRPC]
	public void NotifyClientsFriendRequestReadyRPC(GTZone zone, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "NotifyClientsFriendRequestReadyRPC");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(info.Sender), out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[12].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		this.SendFriendRequestIfApplicable(zone);
	}

	// Token: 0x0600309B RID: 12443 RVA: 0x00130A34 File Offset: 0x0012EC34
	[PunRPC]
	public void FriendRequestCompletedRPC(GTZone zone, bool succeeded, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "FriendRequestCompletedRPC");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(info.Sender), out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[12].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		this.FriendRequestCompletedAuthority(zone, info.Sender.ActorNumber, succeeded);
	}

	// Token: 0x0600309D RID: 12445 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x0600309E RID: 12446 RVA: 0x00130AA4 File Offset: 0x0012ECA4
	[CompilerGenerated]
	internal static void <OnPhotonSerializeView>g__SendFriendStationData|31_0(PhotonStream stream, FriendingManager.FriendStationData data)
	{
		stream.SendNext((int)data.zone);
		stream.SendNext(data.actorNumberA);
		stream.SendNext(data.actorNumberB);
		stream.SendNext((int)data.state);
	}

	// Token: 0x0600309F RID: 12447 RVA: 0x00130AF8 File Offset: 0x0012ECF8
	[CompilerGenerated]
	internal static FriendingManager.FriendStationData <OnPhotonSerializeView>g__ReceiveFriendStationData|31_1(PhotonStream stream)
	{
		return new FriendingManager.FriendStationData
		{
			zone = (GTZone)((int)stream.ReceiveNext()),
			actorNumberA = (int)stream.ReceiveNext(),
			actorNumberB = (int)stream.ReceiveNext(),
			state = (FriendingManager.FriendStationState)((int)stream.ReceiveNext())
		};
	}

	// Token: 0x04003476 RID: 13430
	[OnEnterPlay_SetNull]
	public static volatile FriendingManager Instance;

	// Token: 0x04003477 RID: 13431
	[SerializeField]
	private float progressBarDuration = 3f;

	// Token: 0x04003478 RID: 13432
	[SerializeField]
	private float requiredProximityToStation = 3f;

	// Token: 0x04003479 RID: 13433
	private List<FriendingManager.FriendStationData> activeFriendStationData = new List<FriendingManager.FriendStationData>(10);

	// Token: 0x0400347A RID: 13434
	private Dictionary<GTZone, FriendingStation> friendingStations = new Dictionary<GTZone, FriendingStation>();

	// Token: 0x0400347B RID: 13435
	private GTZone localPlayerZone = GTZone.none;

	// Token: 0x020007A9 RID: 1961
	public enum FriendStationState
	{
		// Token: 0x0400347D RID: 13437
		NotInRoom,
		// Token: 0x0400347E RID: 13438
		WaitingForPlayers,
		// Token: 0x0400347F RID: 13439
		WaitingOnFriendStatusBoth,
		// Token: 0x04003480 RID: 13440
		WaitingOnFriendStatusPlayerA,
		// Token: 0x04003481 RID: 13441
		WaitingOnFriendStatusPlayerB,
		// Token: 0x04003482 RID: 13442
		WaitingOnButtonBoth,
		// Token: 0x04003483 RID: 13443
		WaitingOnButtonPlayerA,
		// Token: 0x04003484 RID: 13444
		WaitingOnButtonPlayerB,
		// Token: 0x04003485 RID: 13445
		ButtonConfirmationTimer0,
		// Token: 0x04003486 RID: 13446
		ButtonConfirmationTimer1,
		// Token: 0x04003487 RID: 13447
		ButtonConfirmationTimer2,
		// Token: 0x04003488 RID: 13448
		ButtonConfirmationTimer3,
		// Token: 0x04003489 RID: 13449
		ButtonConfirmationTimer4,
		// Token: 0x0400348A RID: 13450
		WaitingOnRequestBoth,
		// Token: 0x0400348B RID: 13451
		WaitingOnRequestPlayerA,
		// Token: 0x0400348C RID: 13452
		WaitingOnRequestPlayerB,
		// Token: 0x0400348D RID: 13453
		RequestFailed,
		// Token: 0x0400348E RID: 13454
		Friends,
		// Token: 0x0400348F RID: 13455
		AlreadyFriends
	}

	// Token: 0x020007AA RID: 1962
	public struct FriendStationData
	{
		// Token: 0x04003490 RID: 13456
		public GTZone zone;

		// Token: 0x04003491 RID: 13457
		public int actorNumberA;

		// Token: 0x04003492 RID: 13458
		public int actorNumberB;

		// Token: 0x04003493 RID: 13459
		public FriendingManager.FriendStationState state;

		// Token: 0x04003494 RID: 13460
		public float progressBarStartTime;
	}
}
