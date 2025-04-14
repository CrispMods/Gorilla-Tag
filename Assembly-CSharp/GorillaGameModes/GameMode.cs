using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using Photon.Realtime;
using UnityEngine;

namespace GorillaGameModes
{
	// Token: 0x02000966 RID: 2406
	public class GameMode : MonoBehaviour
	{
		// Token: 0x06003A9F RID: 15007 RVA: 0x0010DC50 File Offset: 0x0010BE50
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
			Object.Destroy(this);
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x0010DCF5 File Offset: 0x0010BEF5
		private void OnDestroy()
		{
			if (GameMode.instance == this)
			{
				GameMode.instance = null;
			}
		}

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06003AA1 RID: 15009 RVA: 0x0010DD0C File Offset: 0x0010BF0C
		// (remove) Token: 0x06003AA2 RID: 15010 RVA: 0x0010DD40 File Offset: 0x0010BF40
		public static event GameMode.OnStartGameModeAction OnStartGameMode;

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06003AA3 RID: 15011 RVA: 0x0010DD73 File Offset: 0x0010BF73
		public static GorillaGameManager ActiveGameMode
		{
			get
			{
				return GameMode.activeGameMode;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06003AA4 RID: 15012 RVA: 0x0010DD7A File Offset: 0x0010BF7A
		internal static GameModeSerializer ActiveNetworkHandler
		{
			get
			{
				return GameMode.activeNetworkHandler;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06003AA5 RID: 15013 RVA: 0x0010DD81 File Offset: 0x0010BF81
		public static GameModeZoneMapping GameModeZoneMapping
		{
			get
			{
				return GameMode.instance.gameModeZoneMapping;
			}
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x0010DD90 File Offset: 0x0010BF90
		static GameMode()
		{
			GameMode.StaticLoad();
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x0010DE14 File Offset: 0x0010C014
		[OnEnterPlay_Run]
		private static void StaticLoad()
		{
			RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(GameMode.ResetGameModes));
			RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(GameMode.RefreshPlayers));
			RoomSystem.PlayersChangedEvent = (Action)Delegate.Combine(RoomSystem.PlayersChangedEvent, new Action(GameMode.RefreshPlayers));
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x0010DE81 File Offset: 0x0010C081
		internal static bool LoadGameModeFromProperty()
		{
			return GameMode.LoadGameMode(GameMode.FindGameModeFromRoomProperty());
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x0010DE8D File Offset: 0x0010C08D
		internal static bool ChangeGameFromProperty()
		{
			return GameMode.ChangeGameMode(GameMode.FindGameModeFromRoomProperty());
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x0010DE99 File Offset: 0x0010C099
		internal static bool LoadGameModeFromProperty(string prop)
		{
			return GameMode.LoadGameMode(GameMode.FindGameModeInString(prop));
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x0010DEA6 File Offset: 0x0010C0A6
		internal static bool ChangeGameFromProperty(string prop)
		{
			return GameMode.ChangeGameMode(GameMode.FindGameModeInString(prop));
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x0010DEB4 File Offset: 0x0010C0B4
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

		// Token: 0x06003AAD RID: 15021 RVA: 0x0010DEF2 File Offset: 0x0010C0F2
		private static string FindGameModeFromRoomProperty()
		{
			if (!NetworkSystem.Instance.InRoom || string.IsNullOrEmpty(NetworkSystem.Instance.GameModeString))
			{
				return null;
			}
			return GameMode.FindGameModeInString(NetworkSystem.Instance.GameModeString);
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x0010DF22 File Offset: 0x0010C122
		public static bool IsValidGameMode(string gameMode)
		{
			return !string.IsNullOrEmpty(gameMode) && GameMode.gameModeKeyByName.ContainsKey(gameMode);
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x0010DF3C File Offset: 0x0010C13C
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

		// Token: 0x06003AB0 RID: 15024 RVA: 0x0010DF7C File Offset: 0x0010C17C
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

		// Token: 0x06003AB1 RID: 15025 RVA: 0x0010DFC0 File Offset: 0x0010C1C0
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

		// Token: 0x06003AB2 RID: 15026 RVA: 0x0010E0D4 File Offset: 0x0010C2D4
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

		// Token: 0x06003AB3 RID: 15027 RVA: 0x0010E110 File Offset: 0x0010C310
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

		// Token: 0x06003AB4 RID: 15028 RVA: 0x0010E184 File Offset: 0x0010C384
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

		// Token: 0x06003AB5 RID: 15029 RVA: 0x0010E21F File Offset: 0x0010C41F
		internal static void RemoveNetworkLink(GameModeSerializer networkSerializer)
		{
			if (GameMode.activeGameMode.IsNotNull() && networkSerializer == GameMode.activeNetworkHandler)
			{
				GameMode.activeGameMode.NetworkLinkDestroyed(networkSerializer);
				GameMode.activeNetworkHandler = null;
				return;
			}
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x0010E24C File Offset: 0x0010C44C
		public static GorillaGameManager GetGameModeInstance(GameModeType type)
		{
			return GameMode.GetGameModeInstance((int)type);
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x0010E254 File Offset: 0x0010C454
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

		// Token: 0x06003AB8 RID: 15032 RVA: 0x0010E2C4 File Offset: 0x0010C4C4
		public static T GetGameModeInstance<T>(GameModeType type) where T : GorillaGameManager
		{
			return GameMode.GetGameModeInstance<T>((int)type);
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x0010E2CC File Offset: 0x0010C4CC
		public static T GetGameModeInstance<T>(int type) where T : GorillaGameManager
		{
			T t = GameMode.GetGameModeInstance(type) as T;
			if (t != null)
			{
				return t;
			}
			return default(T);
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x0010E300 File Offset: 0x0010C500
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

		// Token: 0x06003ABB RID: 15035 RVA: 0x0010E364 File Offset: 0x0010C564
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

		// Token: 0x06003ABC RID: 15036 RVA: 0x0010E38C File Offset: 0x0010C58C
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

		// Token: 0x06003ABD RID: 15037 RVA: 0x0010E3B4 File Offset: 0x0010C5B4
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

		// Token: 0x06003ABE RID: 15038 RVA: 0x0010E3DC File Offset: 0x0010C5DC
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

		// Token: 0x06003ABF RID: 15039 RVA: 0x0010E41A File Offset: 0x0010C61A
		public static void ReportHit()
		{
			if (NetworkSystem.Instance.InRoom && GameMode.activeNetworkHandler.IsNotNull())
			{
				GameMode.activeNetworkHandler.SendRPC("RPC_ReportHit", false, Array.Empty<object>());
			}
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x0010E449 File Offset: 0x0010C649
		public static void BroadcastRoundComplete()
		{
			if (NetworkSystem.Instance.IsMasterClient && NetworkSystem.Instance.InRoom && GameMode.activeNetworkHandler.IsNotNull())
			{
				GameMode.activeNetworkHandler.SendRPC("RPC_BroadcastRoundComplete", true, Array.Empty<object>());
			}
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x0010E484 File Offset: 0x0010C684
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

		// Token: 0x06003AC2 RID: 15042 RVA: 0x0010E4DA File Offset: 0x0010C6DA
		public static void OptOut(VRRig rig)
		{
			GameMode.OptOut(rig.creator.ActorNumber);
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x0010E4EC File Offset: 0x0010C6EC
		public static void OptOut(NetPlayer player)
		{
			GameMode.OptOut(player.ActorNumber);
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x0010E4F9 File Offset: 0x0010C6F9
		public static void OptOut(int playerActorNumber)
		{
			if (GameMode.optOutPlayers.Add(playerActorNumber))
			{
				GameMode.RefreshPlayers();
			}
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x0010E50D File Offset: 0x0010C70D
		public static void OptIn(VRRig rig)
		{
			GameMode.OptIn(rig.creator.ActorNumber);
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x0010E51F File Offset: 0x0010C71F
		public static void OptIn(NetPlayer player)
		{
			GameMode.OptIn(player.ActorNumber);
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x0010E52C File Offset: 0x0010C72C
		public static void OptIn(int playerActorNumber)
		{
			if (GameMode.optOutPlayers.Remove(playerActorNumber))
			{
				GameMode.RefreshPlayers();
			}
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x0010E540 File Offset: 0x0010C740
		private static bool CanParticipate(NetPlayer player)
		{
			return player.InRoom() && !GameMode.optOutPlayers.Contains(player.ActorNumber) && NetworkSystem.Instance.GetPlayerTutorialCompletion(player.ActorNumber);
		}

		// Token: 0x04003BBE RID: 15294
		[SerializeField]
		private GameModeZoneMapping gameModeZoneMapping;

		// Token: 0x04003BC0 RID: 15296
		[OnEnterPlay_SetNull]
		private static GameMode instance;

		// Token: 0x04003BC1 RID: 15297
		[OnEnterPlay_Clear]
		private static Dictionary<int, GorillaGameManager> gameModeTable = new Dictionary<int, GorillaGameManager>();

		// Token: 0x04003BC2 RID: 15298
		[OnEnterPlay_Clear]
		private static Dictionary<string, int> gameModeKeyByName = new Dictionary<string, int>();

		// Token: 0x04003BC3 RID: 15299
		[OnEnterPlay_Clear]
		private static Dictionary<int, FusionGameModeData> fusionTypeTable = new Dictionary<int, FusionGameModeData>();

		// Token: 0x04003BC4 RID: 15300
		[OnEnterPlay_Clear]
		private static List<GorillaGameManager> gameModes = new List<GorillaGameManager>(10);

		// Token: 0x04003BC5 RID: 15301
		[OnEnterPlay_Clear]
		public static readonly List<string> gameModeNames = new List<string>(10);

		// Token: 0x04003BC6 RID: 15302
		[OnEnterPlay_Clear]
		private static readonly List<GorillaGameManager> activatedGameModes = new List<GorillaGameManager>(9);

		// Token: 0x04003BC7 RID: 15303
		[OnEnterPlay_SetNull]
		private static GorillaGameManager activeGameMode = null;

		// Token: 0x04003BC8 RID: 15304
		[OnEnterPlay_SetNull]
		private static GameModeSerializer activeNetworkHandler = null;

		// Token: 0x04003BC9 RID: 15305
		private static List<Player> participatingPlayers = new List<Player>(10);

		// Token: 0x04003BCA RID: 15306
		[OnEnterPlay_Clear]
		private static readonly HashSet<int> optOutPlayers = new HashSet<int>(10);

		// Token: 0x04003BCB RID: 15307
		[OnEnterPlay_Clear]
		public static readonly List<NetPlayer> ParticipatingPlayers = new List<NetPlayer>(10);

		// Token: 0x02000967 RID: 2407
		// (Invoke) Token: 0x06003ACB RID: 15051
		public delegate void OnStartGameModeAction(GameModeType newGameModeType);
	}
}
