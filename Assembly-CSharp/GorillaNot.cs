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

// Token: 0x02000590 RID: 1424
public class GorillaNot : MonoBehaviour
{
	// Token: 0x17000389 RID: 905
	// (get) Token: 0x060022FF RID: 8959 RVA: 0x0003A34F File Offset: 0x0003854F
	private NetworkRunner runner
	{
		get
		{
			return ((NetworkSystemFusion)NetworkSystem.Instance).runner;
		}
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06002300 RID: 8960 RVA: 0x00047ADF File Offset: 0x00045CDF
	// (set) Token: 0x06002301 RID: 8961 RVA: 0x00047AE7 File Offset: 0x00045CE7
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

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06002302 RID: 8962 RVA: 0x00047AF8 File Offset: 0x00045CF8
	// (set) Token: 0x06002303 RID: 8963 RVA: 0x00047B00 File Offset: 0x00045D00
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

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06002304 RID: 8964 RVA: 0x00047B1B File Offset: 0x00045D1B
	// (set) Token: 0x06002305 RID: 8965 RVA: 0x00047B23 File Offset: 0x00045D23
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

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06002306 RID: 8966 RVA: 0x00047B3E File Offset: 0x00045D3E
	// (set) Token: 0x06002307 RID: 8967 RVA: 0x00047B46 File Offset: 0x00045D46
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

	// Token: 0x06002308 RID: 8968 RVA: 0x000FB89C File Offset: 0x000F9A9C
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

	// Token: 0x06002309 RID: 8969 RVA: 0x00047B61 File Offset: 0x00045D61
	private void OnApplicationPause(bool paused)
	{
		if (paused || !RoomSystem.JoinedRoom)
		{
			return;
		}
		this.lastServerTimestamp = NetworkSystem.Instance.SimTick;
		this.RefreshRPCs();
	}

	// Token: 0x0600230A RID: 8970 RVA: 0x000FB95C File Offset: 0x000F9B5C
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

	// Token: 0x0600230B RID: 8971 RVA: 0x00047B84 File Offset: 0x00045D84
	public void SendReport(string susReason, string susId, string susNick)
	{
		this.suspiciousReason = susReason;
		this.suspiciousPlayerId = susId;
		this.suspiciousPlayerName = susNick;
		this.sendReport = true;
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000FBA60 File Offset: 0x000F9C60
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

	// Token: 0x0600230D RID: 8973 RVA: 0x00047BA2 File Offset: 0x00045DA2
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

	// Token: 0x0600230E RID: 8974 RVA: 0x000FBBE4 File Offset: 0x000F9DE4
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

	// Token: 0x0600230F RID: 8975 RVA: 0x000FBC74 File Offset: 0x000F9E74
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

	// Token: 0x06002310 RID: 8976 RVA: 0x00047BB1 File Offset: 0x00045DB1
	public void OnPlayerEnteredRoom(NetPlayer newPlayer)
	{
		this.cachedPlayerList = (NetworkSystem.Instance.AllNetPlayers ?? new NetPlayer[0]);
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000FBCD0 File Offset: 0x000F9ED0
	public void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.cachedPlayerList = (NetworkSystem.Instance.AllNetPlayers ?? new NetPlayer[0]);
		Dictionary<string, GorillaNot.RPCCallTracker> dictionary;
		if (this.userRPCCalls.TryGetValue(otherPlayer.UserId, out dictionary))
		{
			this.userRPCCalls.Remove(otherPlayer.UserId);
		}
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x00047BCD File Offset: 0x00045DCD
	public static void IncrementRPCCall(PhotonMessageInfo info, [CallerMemberName] string callingMethod = "")
	{
		GorillaNot.IncrementRPCCall(new PhotonMessageInfoWrapped(info), callingMethod);
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x00047BDB File Offset: 0x00045DDB
	public static void IncrementRPCCall(PhotonMessageInfoWrapped infoWrapped, [CallerMemberName] string callingMethod = "")
	{
		GorillaNot.instance.IncrementRPCCallLocal(infoWrapped, callingMethod);
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000FBD20 File Offset: 0x000F9F20
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

	// Token: 0x06002315 RID: 8981 RVA: 0x000FBD88 File Offset: 0x000F9F88
	private bool IncrementRPCTracker(in NetPlayer sender, in string rpcFunction, in int callLimit)
	{
		string userId = sender.UserId;
		return this.IncrementRPCTracker(userId, rpcFunction, callLimit);
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x000FBDA8 File Offset: 0x000F9FA8
	private bool IncrementRPCTracker(in Player sender, in string rpcFunction, in int callLimit)
	{
		string userId = sender.UserId;
		return this.IncrementRPCTracker(userId, rpcFunction, callLimit);
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x000FBDC8 File Offset: 0x000F9FC8
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

	// Token: 0x06002318 RID: 8984 RVA: 0x000FBE1C File Offset: 0x000FA01C
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

	// Token: 0x06002319 RID: 8985 RVA: 0x00047BEB File Offset: 0x00045DEB
	private IEnumerator QuitDelay()
	{
		yield return new WaitForSeconds(1f);
		NetworkSystem.Instance.ReturnToSinglePlayer();
		yield break;
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x000FBE9C File Offset: 0x000FA09C
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

	// Token: 0x0600231B RID: 8987 RVA: 0x000FBF04 File Offset: 0x000FA104
	private bool ShouldDisconnectFromRoom()
	{
		return this._suspiciousReason.Contains("too many players") || this._suspiciousReason.Contains("invalid room name") || this._suspiciousReason.Contains("invalid game mode") || this._suspiciousReason.Contains("missing player ids");
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x00047BF3 File Offset: 0x00045DF3
	private void CloseInvalidRoom()
	{
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;
		PhotonNetwork.CurrentRoom.MaxPlayers = PhotonNetworkController.Instance.GetRoomSize(PhotonNetworkController.Instance.currentGameType);
	}

	// Token: 0x040026A4 RID: 9892
	[OnEnterPlay_SetNull]
	public static volatile GorillaNot instance;

	// Token: 0x040026A5 RID: 9893
	private bool _sendReport;

	// Token: 0x040026A6 RID: 9894
	private string _suspiciousPlayerId = "";

	// Token: 0x040026A7 RID: 9895
	private string _suspiciousPlayerName = "";

	// Token: 0x040026A8 RID: 9896
	private string _suspiciousReason = "";

	// Token: 0x040026A9 RID: 9897
	internal List<string> reportedPlayers = new List<string>();

	// Token: 0x040026AA RID: 9898
	public byte roomSize;

	// Token: 0x040026AB RID: 9899
	public float lastCheck;

	// Token: 0x040026AC RID: 9900
	public float checkCooldown = 3f;

	// Token: 0x040026AD RID: 9901
	public float userDecayTime = 15f;

	// Token: 0x040026AE RID: 9902
	public NetPlayer currentMasterClient;

	// Token: 0x040026AF RID: 9903
	public bool testAssault;

	// Token: 0x040026B0 RID: 9904
	private const byte ReportAssault = 8;

	// Token: 0x040026B1 RID: 9905
	private int lowestActorNumber;

	// Token: 0x040026B2 RID: 9906
	private int calls;

	// Token: 0x040026B3 RID: 9907
	public int rpcCallLimit = 50;

	// Token: 0x040026B4 RID: 9908
	public int logErrorMax = 50;

	// Token: 0x040026B5 RID: 9909
	public int rpcErrorMax = 10;

	// Token: 0x040026B6 RID: 9910
	private object outObj;

	// Token: 0x040026B7 RID: 9911
	private NetPlayer tempPlayer;

	// Token: 0x040026B8 RID: 9912
	private int logErrorCount;

	// Token: 0x040026B9 RID: 9913
	private int stringIndex;

	// Token: 0x040026BA RID: 9914
	private string playerID;

	// Token: 0x040026BB RID: 9915
	private string playerNick;

	// Token: 0x040026BC RID: 9916
	private int lastServerTimestamp;

	// Token: 0x040026BD RID: 9917
	private const string InvalidRPC = "invalid RPC stuff";

	// Token: 0x040026BE RID: 9918
	public NetPlayer[] cachedPlayerList;

	// Token: 0x040026BF RID: 9919
	private static int[] targetActors = new int[]
	{
		-1
	};

	// Token: 0x040026C0 RID: 9920
	private Dictionary<string, Dictionary<string, GorillaNot.RPCCallTracker>> userRPCCalls = new Dictionary<string, Dictionary<string, GorillaNot.RPCCallTracker>>();

	// Token: 0x040026C1 RID: 9921
	private ExitGames.Client.Photon.Hashtable hashTable;

	// Token: 0x02000591 RID: 1425
	private class RPCCallTracker
	{
		// Token: 0x040026C2 RID: 9922
		public int RPCCalls;

		// Token: 0x040026C3 RID: 9923
		public int RPCCallsMax;
	}
}
