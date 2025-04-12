using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using Fusion;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000583 RID: 1411
public class GorillaNot : MonoBehaviour
{
	// Token: 0x17000382 RID: 898
	// (get) Token: 0x060022A7 RID: 8871 RVA: 0x0003908F File Offset: 0x0003728F
	private NetworkRunner runner
	{
		get
		{
			return ((NetworkSystemFusion)NetworkSystem.Instance).runner;
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x060022A8 RID: 8872 RVA: 0x000466E1 File Offset: 0x000448E1
	// (set) Token: 0x060022A9 RID: 8873 RVA: 0x000466E9 File Offset: 0x000448E9
	private bool sendReport
	{
		get
		{
			return this._sendReport;
		}
		set
		{
			if (!this._sendReport)
			{
				this._sendReport = true;
			}
		}
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x060022AA RID: 8874 RVA: 0x000466FA File Offset: 0x000448FA
	// (set) Token: 0x060022AB RID: 8875 RVA: 0x00046702 File Offset: 0x00044902
	private string suspiciousPlayerId
	{
		get
		{
			return this._suspiciousPlayerId;
		}
		set
		{
			if (this._suspiciousPlayerId == "")
			{
				this._suspiciousPlayerId = value;
			}
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x060022AC RID: 8876 RVA: 0x0004671D File Offset: 0x0004491D
	// (set) Token: 0x060022AD RID: 8877 RVA: 0x00046725 File Offset: 0x00044925
	private string suspiciousPlayerName
	{
		get
		{
			return this._suspiciousPlayerName;
		}
		set
		{
			if (this._suspiciousPlayerName == "")
			{
				this._suspiciousPlayerName = value;
			}
		}
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x060022AE RID: 8878 RVA: 0x00046740 File Offset: 0x00044940
	// (set) Token: 0x060022AF RID: 8879 RVA: 0x00046748 File Offset: 0x00044948
	private string suspiciousReason
	{
		get
		{
			return this._suspiciousReason;
		}
		set
		{
			if (this._suspiciousReason == "")
			{
				this._suspiciousReason = value;
			}
		}
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x000F8B20 File Offset: 0x000F6D20
	private void Start()
	{
		if (GorillaNot.instance == null)
		{
			GorillaNot.instance = this;
		}
		else if (GorillaNot.instance != this)
		{
			UnityEngine.Object.Destroy(this);
		}
		RoomSystem.PlayerJoinedEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerJoinedEvent, new Action<NetPlayer>(this.OnPlayerEnteredRoom));
		RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(delegate()
		{
			this.cachedPlayerList = (NetworkSystem.Instance.AllNetPlayers ?? new NetPlayer[0]);
		}));
		base.StartCoroutine(this.CheckReports());
		this.logErrorCount = 0;
		Application.logMessageReceived += this.LogErrorCount;
	}

	// Token: 0x060022B1 RID: 8881 RVA: 0x00046763 File Offset: 0x00044963
	private void OnApplicationPause(bool paused)
	{
		if (paused || !RoomSystem.JoinedRoom)
		{
			return;
		}
		this.lastServerTimestamp = NetworkSystem.Instance.SimTick;
		this.RefreshRPCs();
	}

	// Token: 0x060022B2 RID: 8882 RVA: 0x000F8BE0 File Offset: 0x000F6DE0
	public void LogErrorCount(string logString, string stackTrace, LogType type)
	{
		if (type == LogType.Error)
		{
			this.logErrorCount++;
			this.stringIndex = logString.LastIndexOf("Sender is ");
			if (logString.Contains("RPC") && this.stringIndex >= 0)
			{
				this.playerID = logString.Substring(this.stringIndex + 10);
				this.tempPlayer = null;
				for (int i = 0; i < this.cachedPlayerList.Length; i++)
				{
					if (this.cachedPlayerList[i].UserId == this.playerID)
					{
						this.tempPlayer = this.cachedPlayerList[i];
						break;
					}
				}
				string text = "invalid RPC stuff";
				if (!this.IncrementRPCTracker(this.tempPlayer, text, this.rpcErrorMax))
				{
					this.SendReport("invalid RPC stuff", this.tempPlayer.UserId, this.tempPlayer.NickName);
				}
				this.tempPlayer = null;
			}
			if (this.logErrorCount > this.logErrorMax)
			{
				Debug.unityLogger.logEnabled = false;
			}
		}
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x00046786 File Offset: 0x00044986
	public void SendReport(string susReason, string susId, string susNick)
	{
		this.suspiciousReason = susReason;
		this.suspiciousPlayerId = susId;
		this.suspiciousPlayerName = susNick;
		this.sendReport = true;
	}

	// Token: 0x060022B4 RID: 8884 RVA: 0x000F8CE4 File Offset: 0x000F6EE4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void DispatchReport()
	{
		if ((this.sendReport || this.testAssault) && this.suspiciousPlayerId != "" && this.reportedPlayers.IndexOf(this.suspiciousPlayerId) == -1)
		{
			if (this._suspiciousPlayerName.Length > 12)
			{
				this._suspiciousPlayerName = this._suspiciousPlayerName.Remove(12);
			}
			this.reportedPlayers.Add(this.suspiciousPlayerId);
			this.testAssault = false;
			WebFlags flags = new WebFlags(1);
			NetEventOptions options = new NetEventOptions
			{
				TargetActors = GorillaNot.targetActors,
				Reciever = NetEventOptions.RecieverTarget.master,
				Flags = flags
			};
			string[] array = new string[this.cachedPlayerList.Length];
			int num = 0;
			foreach (NetPlayer netPlayer in this.cachedPlayerList)
			{
				array[num] = netPlayer.UserId;
				num++;
			}
			object[] data = new object[]
			{
				NetworkSystem.Instance.RoomStringStripped(),
				array,
				NetworkSystem.Instance.MasterClient.UserId,
				this.suspiciousPlayerId,
				this.suspiciousPlayerName,
				this.suspiciousReason,
				NetworkSystemConfig.AppVersion
			};
			NetworkSystemRaiseEvent.RaiseEvent(8, data, options, true);
			if (this.ShouldDisconnectFromRoom())
			{
				base.StartCoroutine(this.QuitDelay());
			}
		}
		this._sendReport = false;
		this._suspiciousPlayerId = "";
		this._suspiciousPlayerName = "";
		this._suspiciousReason = "";
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x000467A4 File Offset: 0x000449A4
	private IEnumerator CheckReports()
	{
		for (;;)
		{
			try
			{
				this.logErrorCount = 0;
				if (NetworkSystem.Instance.InRoom)
				{
					this.lastCheck = Time.time;
					this.lastServerTimestamp = NetworkSystem.Instance.SimTick;
					if (!PhotonNetwork.CurrentRoom.PublishUserId)
					{
						this.sendReport = true;
						this.suspiciousReason = "missing player ids";
						this.SetToRoomCreatorIfHere();
						this.CloseInvalidRoom();
						Debug.Log("publish user id's is off");
					}
					if (this.cachedPlayerList.Length > (int)PhotonNetworkController.Instance.GetRoomSize(PhotonNetworkController.Instance.currentGameType))
					{
						this.sendReport = true;
						this.suspiciousReason = "too many players";
						this.SetToRoomCreatorIfHere();
						this.CloseInvalidRoom();
					}
					if (this.currentMasterClient != NetworkSystem.Instance.MasterClient || this.LowestActorNumber() != NetworkSystem.Instance.MasterClient.ActorNumber)
					{
						foreach (NetPlayer netPlayer in this.cachedPlayerList)
						{
							if (this.currentMasterClient == netPlayer)
							{
								this.sendReport = true;
								this.suspiciousReason = "room host force changed";
								this.suspiciousPlayerId = NetworkSystem.Instance.MasterClient.UserId;
								this.suspiciousPlayerName = NetworkSystem.Instance.MasterClient.NickName;
							}
						}
						this.currentMasterClient = NetworkSystem.Instance.MasterClient;
					}
					this.RefreshRPCs();
					this.DispatchReport();
				}
			}
			catch
			{
			}
			yield return new WaitForSeconds(1.01f);
		}
		yield break;
	}

	// Token: 0x060022B6 RID: 8886 RVA: 0x000F8E68 File Offset: 0x000F7068
	private void RefreshRPCs()
	{
		foreach (Dictionary<string, GorillaNot.RPCCallTracker> dictionary in this.userRPCCalls.Values)
		{
			foreach (GorillaNot.RPCCallTracker rpccallTracker in dictionary.Values)
			{
				rpccallTracker.RPCCalls = 0;
			}
		}
	}

	// Token: 0x060022B7 RID: 8887 RVA: 0x000F8EF8 File Offset: 0x000F70F8
	private int LowestActorNumber()
	{
		this.lowestActorNumber = NetworkSystem.Instance.LocalPlayer.ActorNumber;
		foreach (NetPlayer netPlayer in this.cachedPlayerList)
		{
			if (netPlayer.ActorNumber < this.lowestActorNumber)
			{
				this.lowestActorNumber = netPlayer.ActorNumber;
			}
		}
		return this.lowestActorNumber;
	}

	// Token: 0x060022B8 RID: 8888 RVA: 0x000467B3 File Offset: 0x000449B3
	public void OnPlayerEnteredRoom(NetPlayer newPlayer)
	{
		this.cachedPlayerList = (NetworkSystem.Instance.AllNetPlayers ?? new NetPlayer[0]);
	}

	// Token: 0x060022B9 RID: 8889 RVA: 0x000F8F54 File Offset: 0x000F7154
	public void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.cachedPlayerList = (NetworkSystem.Instance.AllNetPlayers ?? new NetPlayer[0]);
		Dictionary<string, GorillaNot.RPCCallTracker> dictionary;
		if (this.userRPCCalls.TryGetValue(otherPlayer.UserId, out dictionary))
		{
			this.userRPCCalls.Remove(otherPlayer.UserId);
		}
	}

	// Token: 0x060022BA RID: 8890 RVA: 0x000467CF File Offset: 0x000449CF
	public static void IncrementRPCCall(PhotonMessageInfo info, [CallerMemberName] string callingMethod = "")
	{
		GorillaNot.IncrementRPCCall(new PhotonMessageInfoWrapped(info), callingMethod);
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000467DD File Offset: 0x000449DD
	public static void IncrementRPCCall(PhotonMessageInfoWrapped infoWrapped, [CallerMemberName] string callingMethod = "")
	{
		GorillaNot.instance.IncrementRPCCallLocal(infoWrapped, callingMethod);
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x000F8FA4 File Offset: 0x000F71A4
	private void IncrementRPCCallLocal(PhotonMessageInfoWrapped infoWrapped, string rpcFunction)
	{
		if (infoWrapped.sentTick < this.lastServerTimestamp)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(infoWrapped.senderID);
		if (player == null)
		{
			return;
		}
		string userId = player.UserId;
		if (!this.IncrementRPCTracker(userId, rpcFunction, this.rpcCallLimit))
		{
			this.SendReport("too many rpc calls! " + rpcFunction, player.UserId, player.NickName);
			return;
		}
	}

	// Token: 0x060022BD RID: 8893 RVA: 0x000F900C File Offset: 0x000F720C
	private bool IncrementRPCTracker(in NetPlayer sender, in string rpcFunction, in int callLimit)
	{
		string userId = sender.UserId;
		return this.IncrementRPCTracker(userId, rpcFunction, callLimit);
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x000F902C File Offset: 0x000F722C
	private bool IncrementRPCTracker(in Player sender, in string rpcFunction, in int callLimit)
	{
		string userId = sender.UserId;
		return this.IncrementRPCTracker(userId, rpcFunction, callLimit);
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000F904C File Offset: 0x000F724C
	private bool IncrementRPCTracker(in string userId, in string rpcFunction, in int callLimit)
	{
		GorillaNot.RPCCallTracker rpccallTracker = this.GetRPCCallTracker(userId, rpcFunction);
		if (rpccallTracker == null)
		{
			return true;
		}
		rpccallTracker.RPCCalls++;
		if (rpccallTracker.RPCCalls > rpccallTracker.RPCCallsMax)
		{
			rpccallTracker.RPCCallsMax = rpccallTracker.RPCCalls;
		}
		return rpccallTracker.RPCCalls <= callLimit;
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000F90A0 File Offset: 0x000F72A0
	private GorillaNot.RPCCallTracker GetRPCCallTracker(string userID, string rpcFunction)
	{
		if (userID == null)
		{
			return null;
		}
		GorillaNot.RPCCallTracker rpccallTracker = null;
		Dictionary<string, GorillaNot.RPCCallTracker> dictionary;
		if (!this.userRPCCalls.TryGetValue(userID, out dictionary))
		{
			rpccallTracker = new GorillaNot.RPCCallTracker
			{
				RPCCalls = 0,
				RPCCallsMax = 0
			};
			Dictionary<string, GorillaNot.RPCCallTracker> dictionary2 = new Dictionary<string, GorillaNot.RPCCallTracker>();
			dictionary2.Add(rpcFunction, rpccallTracker);
			this.userRPCCalls.Add(userID, dictionary2);
		}
		else if (!dictionary.TryGetValue(rpcFunction, out rpccallTracker))
		{
			rpccallTracker = new GorillaNot.RPCCallTracker
			{
				RPCCalls = 0,
				RPCCallsMax = 0
			};
			dictionary.Add(rpcFunction, rpccallTracker);
		}
		return rpccallTracker;
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000467ED File Offset: 0x000449ED
	private IEnumerator QuitDelay()
	{
		yield return new WaitForSeconds(1f);
		NetworkSystem.Instance.ReturnToSinglePlayer();
		yield break;
	}

	// Token: 0x060022C2 RID: 8898 RVA: 0x000F9120 File Offset: 0x000F7320
	private void SetToRoomCreatorIfHere()
	{
		this.tempPlayer = PhotonNetwork.CurrentRoom.GetPlayer(1, false);
		if (this.tempPlayer != null)
		{
			this.suspiciousPlayerId = this.tempPlayer.UserId;
			this.suspiciousPlayerName = this.tempPlayer.NickName;
			return;
		}
		this.suspiciousPlayerId = "n/a";
		this.suspiciousPlayerName = "n/a";
	}

	// Token: 0x060022C3 RID: 8899 RVA: 0x000F9188 File Offset: 0x000F7388
	private bool ShouldDisconnectFromRoom()
	{
		return this._suspiciousReason.Contains("too many players") || this._suspiciousReason.Contains("invalid room name") || this._suspiciousReason.Contains("invalid game mode") || this._suspiciousReason.Contains("missing player ids");
	}

	// Token: 0x060022C4 RID: 8900 RVA: 0x000467F5 File Offset: 0x000449F5
	private void CloseInvalidRoom()
	{
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;
		PhotonNetwork.CurrentRoom.MaxPlayers = PhotonNetworkController.Instance.GetRoomSize(PhotonNetworkController.Instance.currentGameType);
	}

	// Token: 0x0400264F RID: 9807
	[OnEnterPlay_SetNull]
	public static volatile GorillaNot instance;

	// Token: 0x04002650 RID: 9808
	private bool _sendReport;

	// Token: 0x04002651 RID: 9809
	private string _suspiciousPlayerId = "";

	// Token: 0x04002652 RID: 9810
	private string _suspiciousPlayerName = "";

	// Token: 0x04002653 RID: 9811
	private string _suspiciousReason = "";

	// Token: 0x04002654 RID: 9812
	internal List<string> reportedPlayers = new List<string>();

	// Token: 0x04002655 RID: 9813
	public byte roomSize;

	// Token: 0x04002656 RID: 9814
	public float lastCheck;

	// Token: 0x04002657 RID: 9815
	public float checkCooldown = 3f;

	// Token: 0x04002658 RID: 9816
	public float userDecayTime = 15f;

	// Token: 0x04002659 RID: 9817
	public NetPlayer currentMasterClient;

	// Token: 0x0400265A RID: 9818
	public bool testAssault;

	// Token: 0x0400265B RID: 9819
	private const byte ReportAssault = 8;

	// Token: 0x0400265C RID: 9820
	private int lowestActorNumber;

	// Token: 0x0400265D RID: 9821
	private int calls;

	// Token: 0x0400265E RID: 9822
	public int rpcCallLimit = 50;

	// Token: 0x0400265F RID: 9823
	public int logErrorMax = 50;

	// Token: 0x04002660 RID: 9824
	public int rpcErrorMax = 10;

	// Token: 0x04002661 RID: 9825
	private object outObj;

	// Token: 0x04002662 RID: 9826
	private NetPlayer tempPlayer;

	// Token: 0x04002663 RID: 9827
	private int logErrorCount;

	// Token: 0x04002664 RID: 9828
	private int stringIndex;

	// Token: 0x04002665 RID: 9829
	private string playerID;

	// Token: 0x04002666 RID: 9830
	private string playerNick;

	// Token: 0x04002667 RID: 9831
	private int lastServerTimestamp;

	// Token: 0x04002668 RID: 9832
	private const string InvalidRPC = "invalid RPC stuff";

	// Token: 0x04002669 RID: 9833
	public NetPlayer[] cachedPlayerList;

	// Token: 0x0400266A RID: 9834
	private static int[] targetActors = new int[]
	{
		-1
	};

	// Token: 0x0400266B RID: 9835
	private Dictionary<string, Dictionary<string, GorillaNot.RPCCallTracker>> userRPCCalls = new Dictionary<string, Dictionary<string, GorillaNot.RPCCallTracker>>();

	// Token: 0x0400266C RID: 9836
	private ExitGames.Client.Photon.Hashtable hashTable;

	// Token: 0x02000584 RID: 1412
	private class RPCCallTracker
	{
		// Token: 0x0400266D RID: 9837
		public int RPCCalls;

		// Token: 0x0400266E RID: 9838
		public int RPCCallsMax;
	}
}
