using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009F5 RID: 2549
	public class PlayerTimerManager : MonoBehaviourPunCallbacks
	{
		// Token: 0x06003FB8 RID: 16312 RVA: 0x00169C0C File Offset: 0x00167E0C
		private void Awake()
		{
			if (PlayerTimerManager.instance == null)
			{
				PlayerTimerManager.instance = this;
			}
			else if (PlayerTimerManager.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			this.callLimiters = new CallLimiter[2];
			this.callLimiters[0] = new CallLimiter(10, 1f, 0.5f);
			this.callLimiters[1] = new CallLimiter(30, 1f, 0.5f);
			this.playerTimerData = new Dictionary<int, PlayerTimerManager.PlayerTimerData>(10);
			this.timerToggleLimiters = new Dictionary<int, CallLimiter>(10);
			this.limiterPool = new List<CallLimiter>(10);
			this.serializedTimerData = new byte[256];
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x00169CBC File Offset: 0x00167EBC
		private CallLimiter CreateLimiterFromPool()
		{
			if (this.limiterPool.Count > 0)
			{
				CallLimiter result = this.limiterPool[this.limiterPool.Count - 1];
				this.limiterPool.RemoveAt(this.limiterPool.Count - 1);
				return result;
			}
			return new CallLimiter(5, 1f, 0.5f);
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x00059A87 File Offset: 0x00057C87
		private void ReturnCallLimiterToPool(CallLimiter limiter)
		{
			if (limiter == null)
			{
				return;
			}
			limiter.Reset();
			this.limiterPool.Add(limiter);
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x00059A9F File Offset: 0x00057C9F
		public void RegisterTimerBoard(PlayerTimerBoard board)
		{
			if (!PlayerTimerManager.timerBoards.Contains(board))
			{
				PlayerTimerManager.timerBoards.Add(board);
				this.UpdateTimerBoard(board);
			}
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x00059AC0 File Offset: 0x00057CC0
		public void UnregisterTimerBoard(PlayerTimerBoard board)
		{
			PlayerTimerManager.timerBoards.Remove(board);
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x00169D18 File Offset: 0x00167F18
		public bool IsLocalTimerStarted()
		{
			PlayerTimerManager.PlayerTimerData playerTimerData;
			return this.playerTimerData.TryGetValue(NetworkSystem.Instance.LocalPlayer.ActorNumber, out playerTimerData) && playerTimerData.isStarted;
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x00169D4C File Offset: 0x00167F4C
		public float GetTimeForPlayer(int actorNumber)
		{
			PlayerTimerManager.PlayerTimerData playerTimerData;
			if (!this.playerTimerData.TryGetValue(actorNumber, out playerTimerData))
			{
				return 0f;
			}
			if (playerTimerData.isStarted)
			{
				return Mathf.Clamp((PhotonNetwork.ServerTimestamp - playerTimerData.startTimeStamp) / 1000f, 0f, 3599.99f);
			}
			return Mathf.Clamp(playerTimerData.lastTimerDuration / 1000f, 0f, 3599.99f);
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x00169DB8 File Offset: 0x00167FB8
		public float GetLastDurationForPlayer(int actorNumber)
		{
			PlayerTimerManager.PlayerTimerData playerTimerData;
			if (this.playerTimerData.TryGetValue(actorNumber, out playerTimerData))
			{
				return Mathf.Clamp(playerTimerData.lastTimerDuration / 1000f, 0f, 3599.99f);
			}
			return -1f;
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x00169DF8 File Offset: 0x00167FF8
		[PunRPC]
		private void InitTimersMasterRPC(int numBytes, byte[] bytes, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "InitTimersMasterRPC");
			if (!this.ValidateCallLimits(PlayerTimerManager.RPC.InitTimersMaster, info))
			{
				return;
			}
			if (this.areTimersInitialized)
			{
				return;
			}
			this.DeserializeTimerState(bytes.Length, bytes);
			this.areTimersInitialized = true;
			this.UpdateAllTimerBoards();
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x00169E4C File Offset: 0x0016804C
		private int SerializeTimerState()
		{
			Array.Clear(this.serializedTimerData, 0, this.serializedTimerData.Length);
			MemoryStream memoryStream = new MemoryStream(this.serializedTimerData);
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			if (this.playerTimerData.Count > 10)
			{
				this.ClearOldPlayerData();
			}
			binaryWriter.Write(this.playerTimerData.Count);
			foreach (KeyValuePair<int, PlayerTimerManager.PlayerTimerData> keyValuePair in this.playerTimerData)
			{
				binaryWriter.Write(keyValuePair.Key);
				binaryWriter.Write(keyValuePair.Value.startTimeStamp);
				binaryWriter.Write(keyValuePair.Value.endTimeStamp);
				binaryWriter.Write(keyValuePair.Value.isStarted ? 1 : 0);
				binaryWriter.Write(keyValuePair.Value.lastTimerDuration);
			}
			return (int)memoryStream.Position;
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x00169F48 File Offset: 0x00168148
		private void DeserializeTimerState(int numBytes, byte[] bytes)
		{
			if (numBytes <= 0 || numBytes > 256)
			{
				return;
			}
			if (bytes == null || bytes.Length < numBytes)
			{
				return;
			}
			MemoryStream memoryStream = new MemoryStream(bytes);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			this.playerTimerData.Clear();
			try
			{
				List<Player> list = PhotonNetwork.PlayerList.ToList<Player>();
				if (bytes.Length < 4)
				{
					this.playerTimerData.Clear();
					return;
				}
				int num = binaryReader.ReadInt32();
				if (num < 0 || num > 10)
				{
					this.playerTimerData.Clear();
					return;
				}
				int num2 = 17;
				if (memoryStream.Position + (long)(num2 * num) > (long)bytes.Length)
				{
					this.playerTimerData.Clear();
					return;
				}
				for (int i = 0; i < num; i++)
				{
					int actorNum = binaryReader.ReadInt32();
					int startTimeStamp = binaryReader.ReadInt32();
					int endTimeStamp = binaryReader.ReadInt32();
					bool isStarted = binaryReader.ReadByte() > 0;
					uint lastTimerDuration = binaryReader.ReadUInt32();
					if (list.FindIndex((Player x) => x.ActorNumber == actorNum) >= 0)
					{
						PlayerTimerManager.PlayerTimerData value = new PlayerTimerManager.PlayerTimerData
						{
							startTimeStamp = startTimeStamp,
							endTimeStamp = endTimeStamp,
							isStarted = isStarted,
							lastTimerDuration = lastTimerDuration
						};
						this.playerTimerData.TryAdd(actorNum, value);
					}
				}
			}
			catch (Exception value2)
			{
				Console.WriteLine(value2);
				this.playerTimerData.Clear();
			}
			if (Time.time - this.requestSendTime < 5f && this.IsLocalTimerStarted() != this.localPlayerRequestedStart)
			{
				this.timerPV.RPC("RequestTimerToggleRPC", RpcTarget.MasterClient, new object[]
				{
					this.localPlayerRequestedStart
				});
			}
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x0016A10C File Offset: 0x0016830C
		private void ClearOldPlayerData()
		{
			List<int> list = new List<int>(this.playerTimerData.Count);
			List<Player> list2 = PhotonNetwork.PlayerList.ToList<Player>();
			using (Dictionary<int, PlayerTimerManager.PlayerTimerData>.KeyCollection.Enumerator enumerator = this.playerTimerData.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int actorNum = enumerator.Current;
					if (list2.FindIndex((Player x) => x.ActorNumber == actorNum) < 0)
					{
						list.Add(actorNum);
					}
				}
			}
			foreach (int key in list)
			{
				this.playerTimerData.Remove(key);
			}
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x00059ACE File Offset: 0x00057CCE
		public void RequestTimerToggle(bool startTimer)
		{
			this.requestSendTime = Time.time;
			this.localPlayerRequestedStart = startTimer;
			this.timerPV.RPC("RequestTimerToggleRPC", RpcTarget.MasterClient, new object[]
			{
				startTimer
			});
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x0016A1EC File Offset: 0x001683EC
		[PunRPC]
		private void RequestTimerToggleRPC(bool startTimer, PhotonMessageInfo info)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "RequestTimerToggleRPC");
			CallLimiter callLimiter;
			if (this.timerToggleLimiters.TryGetValue(info.Sender.ActorNumber, out callLimiter))
			{
				if (!callLimiter.CheckCallTime(Time.time))
				{
				}
			}
			else
			{
				CallLimiter callLimiter2 = this.CreateLimiterFromPool();
				this.timerToggleLimiters.Add(info.Sender.ActorNumber, callLimiter2);
				callLimiter2.CheckCallTime(Time.time);
			}
			if (info.Sender == null)
			{
				return;
			}
			PlayerTimerManager.PlayerTimerData playerTimerData;
			bool flag = this.playerTimerData.TryGetValue(info.Sender.ActorNumber, out playerTimerData);
			if (!startTimer && !flag)
			{
				return;
			}
			if (flag && !startTimer && !playerTimerData.isStarted)
			{
				return;
			}
			int num = info.SentServerTimestamp;
			if (PhotonNetwork.ServerTimestamp - num > PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout)
			{
				num = PhotonNetwork.ServerTimestamp - PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout;
			}
			this.timerPV.RPC("TimerToggledMasterRPC", RpcTarget.All, new object[]
			{
				startTimer,
				num,
				info.Sender
			});
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x0016A304 File Offset: 0x00168504
		[PunRPC]
		private void TimerToggledMasterRPC(bool startTimer, int toggleTimeStamp, Player player, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			GorillaNot.IncrementRPCCall(info, "TimerToggledMasterRPC");
			if (!this.ValidateCallLimits(PlayerTimerManager.RPC.ToggleTimerMaster, info))
			{
				return;
			}
			if (player == null)
			{
				return;
			}
			if (!this.areTimersInitialized)
			{
				return;
			}
			int num = toggleTimeStamp;
			int num2 = info.SentServerTimestamp;
			if (PhotonNetwork.ServerTimestamp - num2 > PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout)
			{
				num2 = PhotonNetwork.ServerTimestamp - PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout;
			}
			if (num2 - num > PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout)
			{
				num = num2 - PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout;
			}
			this.OnToggleTimerForPlayer(startTimer, player, num);
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x0016A3AC File Offset: 0x001685AC
		private void OnToggleTimerForPlayer(bool startTimer, Player player, int toggleTime)
		{
			PlayerTimerManager.PlayerTimerData playerTimerData;
			if (this.playerTimerData.TryGetValue(player.ActorNumber, out playerTimerData))
			{
				if (startTimer && !playerTimerData.isStarted)
				{
					playerTimerData.startTimeStamp = toggleTime;
					playerTimerData.isStarted = true;
					UnityEvent<int> onTimerStartedForPlayer = this.OnTimerStartedForPlayer;
					if (onTimerStartedForPlayer != null)
					{
						onTimerStartedForPlayer.Invoke(player.ActorNumber);
					}
					if (player.IsLocal)
					{
						UnityEvent onLocalTimerStarted = this.OnLocalTimerStarted;
						if (onLocalTimerStarted != null)
						{
							onLocalTimerStarted.Invoke();
						}
					}
				}
				else if (!startTimer && playerTimerData.isStarted)
				{
					playerTimerData.endTimeStamp = toggleTime;
					playerTimerData.isStarted = false;
					playerTimerData.lastTimerDuration = (uint)(playerTimerData.endTimeStamp - playerTimerData.startTimeStamp);
					UnityEvent<int, int> onTimerStopped = this.OnTimerStopped;
					if (onTimerStopped != null)
					{
						onTimerStopped.Invoke(player.ActorNumber, playerTimerData.endTimeStamp - playerTimerData.startTimeStamp);
					}
				}
				this.playerTimerData[player.ActorNumber] = playerTimerData;
			}
			else
			{
				PlayerTimerManager.PlayerTimerData value = new PlayerTimerManager.PlayerTimerData
				{
					startTimeStamp = (startTimer ? toggleTime : 0),
					endTimeStamp = (startTimer ? 0 : toggleTime),
					isStarted = startTimer,
					lastTimerDuration = 0U
				};
				this.playerTimerData.TryAdd(player.ActorNumber, value);
				UnityEvent<int> onTimerStartedForPlayer2 = this.OnTimerStartedForPlayer;
				if (onTimerStartedForPlayer2 != null)
				{
					onTimerStartedForPlayer2.Invoke(player.ActorNumber);
				}
				if (player.IsLocal)
				{
					UnityEvent onLocalTimerStarted2 = this.OnLocalTimerStarted;
					if (onLocalTimerStarted2 != null)
					{
						onLocalTimerStarted2.Invoke();
					}
				}
			}
			this.UpdateAllTimerBoards();
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x0016A504 File Offset: 0x00168704
		private bool ValidateCallLimits(PlayerTimerManager.RPC rpcCall, PhotonMessageInfo info)
		{
			return rpcCall >= PlayerTimerManager.RPC.InitTimersMaster && rpcCall < PlayerTimerManager.RPC.Count && this.callLimiters[(int)rpcCall].CheckCallTime(Time.time);
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x0016A534 File Offset: 0x00168734
		public override void OnMasterClientSwitched(Player newMasterClient)
		{
			base.OnMasterClientSwitched(newMasterClient);
			if (newMasterClient.IsLocal)
			{
				int num = this.SerializeTimerState();
				this.timerPV.RPC("InitTimersMasterRPC", RpcTarget.Others, new object[]
				{
					num,
					this.serializedTimerData
				});
				return;
			}
			this.playerTimerData.Clear();
			this.areTimersInitialized = false;
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x0016A594 File Offset: 0x00168794
		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			base.OnPlayerEnteredRoom(newPlayer);
			if (PhotonNetwork.IsMasterClient && !newPlayer.IsLocal)
			{
				int num = this.SerializeTimerState();
				this.timerPV.RPC("InitTimersMasterRPC", newPlayer, new object[]
				{
					num,
					this.serializedTimerData
				});
			}
			this.UpdateAllTimerBoards();
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x0016A5F0 File Offset: 0x001687F0
		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
			base.OnPlayerLeftRoom(otherPlayer);
			this.playerTimerData.Remove(otherPlayer.ActorNumber);
			CallLimiter limiter;
			if (this.timerToggleLimiters.TryGetValue(otherPlayer.ActorNumber, out limiter))
			{
				this.ReturnCallLimiterToPool(limiter);
				this.timerToggleLimiters.Remove(otherPlayer.ActorNumber);
			}
			this.UpdateAllTimerBoards();
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x0016A64C File Offset: 0x0016884C
		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
			this.joinedRoom = true;
			if (PhotonNetwork.IsMasterClient)
			{
				this.playerTimerData.Clear();
				foreach (CallLimiter limiter in this.timerToggleLimiters.Values)
				{
					this.ReturnCallLimiterToPool(limiter);
				}
				this.timerToggleLimiters.Clear();
				this.areTimersInitialized = true;
				this.UpdateAllTimerBoards();
				return;
			}
			this.requestSendTime = 0f;
			this.areTimersInitialized = false;
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x0016A6F0 File Offset: 0x001688F0
		public override void OnLeftRoom()
		{
			base.OnLeftRoom();
			this.joinedRoom = false;
			this.playerTimerData.Clear();
			foreach (CallLimiter limiter in this.timerToggleLimiters.Values)
			{
				this.ReturnCallLimiterToPool(limiter);
			}
			this.timerToggleLimiters.Clear();
			this.areTimersInitialized = false;
			this.requestSendTime = 0f;
			this.localPlayerRequestedStart = false;
			this.UpdateAllTimerBoards();
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x0016A78C File Offset: 0x0016898C
		private void UpdateAllTimerBoards()
		{
			foreach (PlayerTimerBoard board in PlayerTimerManager.timerBoards)
			{
				this.UpdateTimerBoard(board);
			}
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x0016A7E0 File Offset: 0x001689E0
		private void UpdateTimerBoard(PlayerTimerBoard board)
		{
			board.SetSleepState(this.joinedRoom);
			if (GorillaComputer.instance == null)
			{
				return;
			}
			if (!this.joinedRoom)
			{
				if (board.notInRoomText != null)
				{
					board.notInRoomText.gameObject.SetActive(true);
					board.notInRoomText.text = GorillaComputer.instance.offlineTextInitialString;
				}
				for (int i = 0; i < board.lines.Count; i++)
				{
					board.lines[i].ResetData();
				}
				return;
			}
			if (board.notInRoomText != null)
			{
				board.notInRoomText.gameObject.SetActive(false);
			}
			for (int j = 0; j < board.lines.Count; j++)
			{
				PlayerTimerBoardLine playerTimerBoardLine = board.lines[j];
				if (j < PhotonNetwork.PlayerList.Length)
				{
					playerTimerBoardLine.gameObject.SetActive(true);
					playerTimerBoardLine.SetLineData(NetworkSystem.Instance.GetPlayer(PhotonNetwork.PlayerList[j]));
					playerTimerBoardLine.UpdateLine();
				}
				else
				{
					playerTimerBoardLine.ResetData();
					playerTimerBoardLine.gameObject.SetActive(false);
				}
			}
			board.RedrawPlayerLines();
		}

		// Token: 0x040040BF RID: 16575
		public static PlayerTimerManager instance;

		// Token: 0x040040C0 RID: 16576
		public PhotonView timerPV;

		// Token: 0x040040C1 RID: 16577
		public UnityEvent OnLocalTimerStarted;

		// Token: 0x040040C2 RID: 16578
		public UnityEvent<int> OnTimerStartedForPlayer;

		// Token: 0x040040C3 RID: 16579
		public UnityEvent<int, int> OnTimerStopped;

		// Token: 0x040040C4 RID: 16580
		public const float MAX_DURATION_SECONDS = 3599.99f;

		// Token: 0x040040C5 RID: 16581
		private float requestSendTime;

		// Token: 0x040040C6 RID: 16582
		private bool localPlayerRequestedStart;

		// Token: 0x040040C7 RID: 16583
		private CallLimiter[] callLimiters;

		// Token: 0x040040C8 RID: 16584
		private Dictionary<int, CallLimiter> timerToggleLimiters;

		// Token: 0x040040C9 RID: 16585
		private List<CallLimiter> limiterPool;

		// Token: 0x040040CA RID: 16586
		private bool areTimersInitialized;

		// Token: 0x040040CB RID: 16587
		private Dictionary<int, PlayerTimerManager.PlayerTimerData> playerTimerData;

		// Token: 0x040040CC RID: 16588
		private const int MAX_TIMER_INIT_BYTES = 256;

		// Token: 0x040040CD RID: 16589
		private byte[] serializedTimerData;

		// Token: 0x040040CE RID: 16590
		private static List<PlayerTimerBoard> timerBoards = new List<PlayerTimerBoard>(10);

		// Token: 0x040040CF RID: 16591
		private bool joinedRoom;

		// Token: 0x020009F6 RID: 2550
		private enum RPC
		{
			// Token: 0x040040D1 RID: 16593
			InitTimersMaster,
			// Token: 0x040040D2 RID: 16594
			ToggleTimerMaster,
			// Token: 0x040040D3 RID: 16595
			Count
		}

		// Token: 0x020009F7 RID: 2551
		public struct PlayerTimerData
		{
			// Token: 0x040040D4 RID: 16596
			public int startTimeStamp;

			// Token: 0x040040D5 RID: 16597
			public int endTimeStamp;

			// Token: 0x040040D6 RID: 16598
			public bool isStarted;

			// Token: 0x040040D7 RID: 16599
			public uint lastTimerDuration;
		}
	}
}
