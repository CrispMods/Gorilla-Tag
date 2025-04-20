using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using Photon.Realtime;
using UnityEngine;

namespace GorillaGameModes
{
	// Token: 0x0200098C RID: 2444
	public class GameMode : MonoBehaviour
	{
		// Token: 0x06003BB7 RID: 15287 RVA: 0x00151728 File Offset: 0x0014F928
		private void Awake()
		{
			if (GameMode.instance.IsNull())
			{
				GameMode.instance = this;
				foreach (GorillaGameManager gorillaGameManager in base.gameObject.GetComponentsInChildren<GorillaGameManager>(true))
				{
					int num = (int)gorillaGameManager.GameType();
					string text = gorillaGameManager.GameTypeName();
					if (GameMode.gameModeTable.ContainsKey(num))
					{
						Debug.LogWarning("Duplicate gamemode type, skipping this instance", gorillaGameManager);
					}
					else
					{
						GameMode.gameModeTable.Add((int)gorillaGameManager.GameType(), gorillaGameManager);
						GameMode.gameModeKeyByName.Add(text, num);
						GameMode.gameModes.Add(gorillaGameManager);
						GameMode.gameModeNames.Add(text);
					}
				}
				return;
			}
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x00056FB7 File Offset: 0x000551B7
		private void OnDestroy()
		{
			if (GameMode.instance == this)
			{
				GameMode.instance = null;
			}
		}

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06003BB9 RID: 15289 RVA: 0x001517D0 File Offset: 0x0014F9D0
		// (remove) Token: 0x06003BBA RID: 15290 RVA: 0x00151804 File Offset: 0x0014FA04
		public static event GameMode.OnStartGameModeAction OnStartGameMode;

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06003BBB RID: 15291 RVA: 0x00056FCC File Offset: 0x000551CC
		public static GorillaGameManager ActiveGameMode
		{
			get
			{
				return GameMode.activeGameMode;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06003BBC RID: 15292 RVA: 0x00056FD3 File Offset: 0x000551D3
		internal static GameModeSerializer ActiveNetworkHandler
		{
			get
			{
				return GameMode.activeNetworkHandler;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06003BBD RID: 15293 RVA: 0x00056FDA File Offset: 0x000551DA
		public static GameModeZoneMapping GameModeZoneMapping
		{
			get
			{
				return GameMode.instance.gameModeZoneMapping;
			}
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x00151838 File Offset: 0x0014FA38
		static GameMode()
		{
			GameMode.StaticLoad();
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x001518BC File Offset: 0x0014FABC
		[OnEnterPlay_Run]
		private static void StaticLoad()
		{
			RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(GameMode.ResetGameModes));
			RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(GameMode.RefreshPlayers));
			RoomSystem.PlayersChangedEvent = (Action)Delegate.Combine(RoomSystem.PlayersChangedEvent, new Action(GameMode.RefreshPlayers));
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x00056FE6 File Offset: 0x000551E6
		internal static bool LoadGameModeFromProperty()
		{
			return GameMode.LoadGameMode(GameMode.FindGameModeFromRoomProperty());
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x00056FF2 File Offset: 0x000551F2
		internal static bool ChangeGameFromProperty()
		{
			return GameMode.ChangeGameMode(GameMode.FindGameModeFromRoomProperty());
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x00056FFE File Offset: 0x000551FE
		internal static bool LoadGameModeFromProperty(string prop)
		{
			return GameMode.LoadGameMode(GameMode.FindGameModeInString(prop));
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x0005700B File Offset: 0x0005520B
		internal static bool ChangeGameFromProperty(string prop)
		{
			return GameMode.ChangeGameMode(GameMode.FindGameModeInString(prop));
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x0015192C File Offset: 0x0014FB2C
		public static int GetGameModeKeyFromRoomProp()
		{
			string text = GameMode.FindGameModeFromRoomProperty();
			int result;
			if (string.IsNullOrEmpty(text) || !GameMode.gameModeKeyByName.TryGetValue(text, out result))
			{
				GTDev.LogWarning<string>("Unable to find game mode key for " + text, null);
				return -1;
			}
			return result;
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x00057018 File Offset: 0x00055218
		private static string FindGameModeFromRoomProperty()
		{
			if (!NetworkSystem.Instance.InRoom || string.IsNullOrEmpty(NetworkSystem.Instance.GameModeString))
			{
				return null;
			}
			return GameMode.FindGameModeInString(NetworkSystem.Instance.GameModeString);
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x00057048 File Offset: 0x00055248
		public static bool IsValidGameMode(string gameMode)
		{
			return !string.IsNullOrEmpty(gameMode) && GameMode.gameModeKeyByName.ContainsKey(gameMode);
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x0015196C File Offset: 0x0014FB6C
		private static string FindGameModeInString(string gmString)
		{
			for (int i = 0; i < GameMode.gameModes.Count; i++)
			{
				string text = GameMode.gameModes[i].GameTypeName();
				if (gmString.EndsWith(text))
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x001519AC File Offset: 0x0014FBAC
		public static bool LoadGameMode(string gameMode)
		{
			if (gameMode == null)
			{
				Debug.LogError("GAME MODE NULL");
				return false;
			}
			int key;
			if (!GameMode.gameModeKeyByName.TryGetValue(gameMode, out key))
			{
				Debug.LogWarning("Unable to find game mode key for " + gameMode);
				return false;
			}
			return GameMode.LoadGameMode(key);
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x001519F0 File Offset: 0x0014FBF0
		public static bool LoadGameMode(int key)
		{
			foreach (KeyValuePair<int, GorillaGameManager> keyValuePair in GameMode.gameModeTable)
			{
			}
			if (!GameMode.gameModeTable.ContainsKey(key))
			{
				Debug.LogWarning("Missing game mode for key " + key.ToString());
				return false;
			}
			GameObject gameObject;
			VRRigCache.Instance.GetComponent<PhotonPrefabPool>().networkPrefabs.TryGetValue("GameMode", out gameObject);
			if (gameObject == null)
			{
				GTDev.LogError<string>("Unable to find game mode prefab to spawn", null);
				return false;
			}
			if (NetworkSystem.Instance.NetInstantiate(gameObject, Vector3.zero, Quaternion.identity, true, 0, new object[]
			{
				key
			}, delegate(NetworkRunner runner, NetworkObject no)
			{
				no.GetComponent<GameModeSerializer>().Init(key);
			}).IsNull())
			{
				GTDev.LogWarning<string>("Unable to create GameManager with key " + key.ToString(), null);
				return false;
			}
			return true;
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x00151B04 File Offset: 0x0014FD04
		internal static bool ChangeGameMode(string gameMode)
		{
			if (gameMode == null)
			{
				return false;
			}
			int key;
			if (!GameMode.gameModeKeyByName.TryGetValue(gameMode, out key))
			{
				Debug.LogWarning("Unable to find game mode key for " + gameMode);
				return false;
			}
			return GameMode.ChangeGameMode(key);
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x00151B40 File Offset: 0x0014FD40
		internal static bool ChangeGameMode(int key)
		{
			GorillaGameManager x;
			if (!NetworkSystem.Instance.IsMasterClient || !GameMode.gameModeTable.TryGetValue(key, out x) || x == GameMode.activeGameMode)
			{
				return false;
			}
			if (GameMode.activeNetworkHandler.IsNotNull())
			{
				NetworkSystem.Instance.NetDestroy(GameMode.activeNetworkHandler.gameObject);
			}
			GameMode.StopGameModeSafe(GameMode.activeGameMode);
			GameMode.activeGameMode = null;
			GameMode.activeNetworkHandler = null;
			return GameMode.LoadGameMode(key);
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x00151BB4 File Offset: 0x0014FDB4
		internal static void SetupGameModeRemote(GameModeSerializer networkSerializer)
		{
			GorillaGameManager gameModeInstance = networkSerializer.GameModeInstance;
			if (GameMode.activeGameMode.IsNotNull() && gameModeInstance.IsNotNull() && gameModeInstance != GameMode.activeGameMode)
			{
				GameMode.StopGameModeSafe(GameMode.activeGameMode);
			}
			GameMode.activeNetworkHandler = networkSerializer;
			GameMode.activeGameMode = gameModeInstance;
			GameMode.activeGameMode.NetworkLinkSetup(networkSerializer);
			GameMode.StartGameModeSafe(GameMode.activeGameMode);
			if (GameMode.OnStartGameMode != null)
			{
				GameMode.OnStartGameMode(GameMode.activeGameMode.GameType());
			}
			if (!GameMode.activatedGameModes.Contains(GameMode.activeGameMode))
			{
				GameMode.activatedGameModes.Add(GameMode.activeGameMode);
			}
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x0005705F File Offset: 0x0005525F
		internal static void RemoveNetworkLink(GameModeSerializer networkSerializer)
		{
			if (GameMode.activeGameMode.IsNotNull() && networkSerializer == GameMode.activeNetworkHandler)
			{
				GameMode.activeGameMode.NetworkLinkDestroyed(networkSerializer);
				GameMode.activeNetworkHandler = null;
				return;
			}
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x0005708C File Offset: 0x0005528C
		public static GorillaGameManager GetGameModeInstance(GameModeType type)
		{
			return GameMode.GetGameModeInstance((int)type);
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x00151C50 File Offset: 0x0014FE50
		public static GorillaGameManager GetGameModeInstance(int type)
		{
			GorillaGameManager gorillaGameManager;
			if (GameMode.gameModeTable.TryGetValue(type, out gorillaGameManager))
			{
				if (gorillaGameManager == null)
				{
					Debug.LogError("Couldnt get mode from table");
					foreach (KeyValuePair<int, GorillaGameManager> keyValuePair in GameMode.gameModeTable)
					{
					}
				}
				return gorillaGameManager;
			}
			return null;
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x00057094 File Offset: 0x00055294
		public static T GetGameModeInstance<T>(GameModeType type) where T : GorillaGameManager
		{
			return GameMode.GetGameModeInstance<T>((int)type);
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x00151CC0 File Offset: 0x0014FEC0
		public static T GetGameModeInstance<T>(int type) where T : GorillaGameManager
		{
			T t = GameMode.GetGameModeInstance(type) as T;
			if (t != null)
			{
				return t;
			}
			return default(T);
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x00151CF4 File Offset: 0x0014FEF4
		public static void ResetGameModes()
		{
			GameMode.activeGameMode = null;
			GameMode.activeNetworkHandler = null;
			GameMode.optOutPlayers.Clear();
			GameMode.ParticipatingPlayers.Clear();
			for (int i = 0; i < GameMode.activatedGameModes.Count; i++)
			{
				GorillaGameManager gameMode = GameMode.activatedGameModes[i];
				GameMode.StopGameModeSafe(gameMode);
				GameMode.ResetGameModeSafe(gameMode);
			}
			GameMode.activatedGameModes.Clear();
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x00151D58 File Offset: 0x0014FF58
		private static void StartGameModeSafe(GorillaGameManager gameMode)
		{
			try
			{
				gameMode.StartPlaying();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x00151D80 File Offset: 0x0014FF80
		private static void StopGameModeSafe(GorillaGameManager gameMode)
		{
			try
			{
				gameMode.StopPlaying();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x00151DA8 File Offset: 0x0014FFA8
		private static void ResetGameModeSafe(GorillaGameManager gameMode)
		{
			try
			{
				gameMode.Reset();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x0005709C File Offset: 0x0005529C
		public static void ReportTag(NetPlayer player)
		{
			if (NetworkSystem.Instance.InRoom && GameMode.activeNetworkHandler.IsNotNull())
			{
				GameMode.activeNetworkHandler.SendRPC("RPC_ReportTag", false, new object[]
				{
					player.ActorNumber
				});
			}
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x00151DD0 File Offset: 0x0014FFD0
		public static void ReportHit()
		{
			if (GorillaGameManager.instance.GameType() == GameModeType.Custom)
			{
				CustomGameMode.TaggedByEnvironment();
			}
			if (NetworkSystem.Instance.InRoom && GameMode.activeNetworkHandler.IsNotNull())
			{
				GameMode.activeNetworkHandler.SendRPC("RPC_ReportHit", false, Array.Empty<object>());
			}
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x000570DA File Offset: 0x000552DA
		public static void BroadcastRoundComplete()
		{
			if (NetworkSystem.Instance.IsMasterClient && NetworkSystem.Instance.InRoom && GameMode.activeNetworkHandler.IsNotNull())
			{
				GameMode.activeNetworkHandler.SendRPC("RPC_BroadcastRoundComplete", true, Array.Empty<object>());
			}
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x00151E1C File Offset: 0x0015001C
		public static void RefreshPlayers()
		{
			List<NetPlayer> playersInRoom = RoomSystem.PlayersInRoom;
			int num = Mathf.Min(playersInRoom.Count, 10);
			GameMode.ParticipatingPlayers.Clear();
			for (int i = 0; i < num; i++)
			{
				if (GameMode.CanParticipate(playersInRoom[i]))
				{
					GameMode.ParticipatingPlayers.Add(playersInRoom[i]);
				}
			}
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x00057115 File Offset: 0x00055315
		public static void OptOut(VRRig rig)
		{
			GameMode.OptOut(rig.creator.ActorNumber);
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x00057127 File Offset: 0x00055327
		public static void OptOut(NetPlayer player)
		{
			GameMode.OptOut(player.ActorNumber);
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x00057134 File Offset: 0x00055334
		public static void OptOut(int playerActorNumber)
		{
			if (GameMode.optOutPlayers.Add(playerActorNumber))
			{
				GameMode.RefreshPlayers();
			}
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x00057148 File Offset: 0x00055348
		public static void OptIn(VRRig rig)
		{
			GameMode.OptIn(rig.creator.ActorNumber);
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x0005715A File Offset: 0x0005535A
		public static void OptIn(NetPlayer player)
		{
			GameMode.OptIn(player.ActorNumber);
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x00057167 File Offset: 0x00055367
		public static void OptIn(int playerActorNumber)
		{
			if (GameMode.optOutPlayers.Remove(playerActorNumber))
			{
				GameMode.RefreshPlayers();
			}
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x0005717B File Offset: 0x0005537B
		private static bool CanParticipate(NetPlayer player)
		{
			return player.InRoom() && !GameMode.optOutPlayers.Contains(player.ActorNumber) && NetworkSystem.Instance.GetPlayerTutorialCompletion(player.ActorNumber);
		}

		// Token: 0x04003C98 RID: 15512
		[SerializeField]
		private GameModeZoneMapping gameModeZoneMapping;

		// Token: 0x04003C9A RID: 15514
		[OnEnterPlay_SetNull]
		private static GameMode instance;

		// Token: 0x04003C9B RID: 15515
		[OnEnterPlay_Clear]
		private static Dictionary<int, GorillaGameManager> gameModeTable = new Dictionary<int, GorillaGameManager>();

		// Token: 0x04003C9C RID: 15516
		[OnEnterPlay_Clear]
		private static Dictionary<string, int> gameModeKeyByName = new Dictionary<string, int>();

		// Token: 0x04003C9D RID: 15517
		[OnEnterPlay_Clear]
		private static Dictionary<int, FusionGameModeData> fusionTypeTable = new Dictionary<int, FusionGameModeData>();

		// Token: 0x04003C9E RID: 15518
		[OnEnterPlay_Clear]
		private static List<GorillaGameManager> gameModes = new List<GorillaGameManager>(10);

		// Token: 0x04003C9F RID: 15519
		[OnEnterPlay_Clear]
		public static readonly List<string> gameModeNames = new List<string>(10);

		// Token: 0x04003CA0 RID: 15520
		[OnEnterPlay_Clear]
		private static readonly List<GorillaGameManager> activatedGameModes = new List<GorillaGameManager>(9);

		// Token: 0x04003CA1 RID: 15521
		[OnEnterPlay_SetNull]
		private static GorillaGameManager activeGameMode = null;

		// Token: 0x04003CA2 RID: 15522
		[OnEnterPlay_SetNull]
		private static GameModeSerializer activeNetworkHandler = null;

		// Token: 0x04003CA3 RID: 15523
		private static List<Player> participatingPlayers = new List<Player>(10);

		// Token: 0x04003CA4 RID: 15524
		[OnEnterPlay_Clear]
		private static readonly HashSet<int> optOutPlayers = new HashSet<int>(10);

		// Token: 0x04003CA5 RID: 15525
		[OnEnterPlay_Clear]
		public static readonly List<NetPlayer> ParticipatingPlayers = new List<NetPlayer>(10);

		// Token: 0x0200098D RID: 2445
		// (Invoke) Token: 0x06003BE3 RID: 15331
		public delegate void OnStartGameModeAction(GameModeType newGameModeType);
	}
}
