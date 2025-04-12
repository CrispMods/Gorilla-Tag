using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using Photon.Realtime;
using UnityEngine;

namespace GorillaGameModes
{
	// Token: 0x02000969 RID: 2409
	public class GameMode : MonoBehaviour
	{
		// Token: 0x06003AAB RID: 15019 RVA: 0x0014B78C File Offset: 0x0014998C
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

		// Token: 0x06003AAC RID: 15020 RVA: 0x000556F1 File Offset: 0x000538F1
		private void OnDestroy()
		{
			if (GameMode.instance == this)
			{
				GameMode.instance = null;
			}
		}

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06003AAD RID: 15021 RVA: 0x0014B834 File Offset: 0x00149A34
		// (remove) Token: 0x06003AAE RID: 15022 RVA: 0x0014B868 File Offset: 0x00149A68
		public static event GameMode.OnStartGameModeAction OnStartGameMode;

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06003AAF RID: 15023 RVA: 0x00055706 File Offset: 0x00053906
		public static GorillaGameManager ActiveGameMode
		{
			get
			{
				return GameMode.activeGameMode;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06003AB0 RID: 15024 RVA: 0x0005570D File Offset: 0x0005390D
		internal static GameModeSerializer ActiveNetworkHandler
		{
			get
			{
				return GameMode.activeNetworkHandler;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x00055714 File Offset: 0x00053914
		public static GameModeZoneMapping GameModeZoneMapping
		{
			get
			{
				return GameMode.instance.gameModeZoneMapping;
			}
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x0014B89C File Offset: 0x00149A9C
		static GameMode()
		{
			GameMode.StaticLoad();
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x0014B920 File Offset: 0x00149B20
		[OnEnterPlay_Run]
		private static void StaticLoad()
		{
			RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(GameMode.ResetGameModes));
			RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(GameMode.RefreshPlayers));
			RoomSystem.PlayersChangedEvent = (Action)Delegate.Combine(RoomSystem.PlayersChangedEvent, new Action(GameMode.RefreshPlayers));
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x00055720 File Offset: 0x00053920
		internal static bool LoadGameModeFromProperty()
		{
			return GameMode.LoadGameMode(GameMode.FindGameModeFromRoomProperty());
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x0005572C File Offset: 0x0005392C
		internal static bool ChangeGameFromProperty()
		{
			return GameMode.ChangeGameMode(GameMode.FindGameModeFromRoomProperty());
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x00055738 File Offset: 0x00053938
		internal static bool LoadGameModeFromProperty(string prop)
		{
			return GameMode.LoadGameMode(GameMode.FindGameModeInString(prop));
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x00055745 File Offset: 0x00053945
		internal static bool ChangeGameFromProperty(string prop)
		{
			return GameMode.ChangeGameMode(GameMode.FindGameModeInString(prop));
		}

		// Token: 0x06003AB8 RID: 15032 RVA: 0x0014B990 File Offset: 0x00149B90
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

		// Token: 0x06003AB9 RID: 15033 RVA: 0x00055752 File Offset: 0x00053952
		private static string FindGameModeFromRoomProperty()
		{
			if (!NetworkSystem.Instance.InRoom || string.IsNullOrEmpty(NetworkSystem.Instance.GameModeString))
			{
				return null;
			}
			return GameMode.FindGameModeInString(NetworkSystem.Instance.GameModeString);
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x00055782 File Offset: 0x00053982
		public static bool IsValidGameMode(string gameMode)
		{
			return !string.IsNullOrEmpty(gameMode) && GameMode.gameModeKeyByName.ContainsKey(gameMode);
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x0014B9D0 File Offset: 0x00149BD0
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

		// Token: 0x06003ABC RID: 15036 RVA: 0x0014BA10 File Offset: 0x00149C10
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

		// Token: 0x06003ABD RID: 15037 RVA: 0x0014BA54 File Offset: 0x00149C54
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

		// Token: 0x06003ABE RID: 15038 RVA: 0x0014BB68 File Offset: 0x00149D68
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

		// Token: 0x06003ABF RID: 15039 RVA: 0x0014BBA4 File Offset: 0x00149DA4
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

		// Token: 0x06003AC0 RID: 15040 RVA: 0x0014BC18 File Offset: 0x00149E18
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

		// Token: 0x06003AC1 RID: 15041 RVA: 0x00055799 File Offset: 0x00053999
		internal static void RemoveNetworkLink(GameModeSerializer networkSerializer)
		{
			if (GameMode.activeGameMode.IsNotNull() && networkSerializer == GameMode.activeNetworkHandler)
			{
				GameMode.activeGameMode.NetworkLinkDestroyed(networkSerializer);
				GameMode.activeNetworkHandler = null;
				return;
			}
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x000557C6 File Offset: 0x000539C6
		public static GorillaGameManager GetGameModeInstance(GameModeType type)
		{
			return GameMode.GetGameModeInstance((int)type);
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x0014BCB4 File Offset: 0x00149EB4
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

		// Token: 0x06003AC4 RID: 15044 RVA: 0x000557CE File Offset: 0x000539CE
		public static T GetGameModeInstance<T>(GameModeType type) where T : GorillaGameManager
		{
			return GameMode.GetGameModeInstance<T>((int)type);
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x0014BD24 File Offset: 0x00149F24
		public static T GetGameModeInstance<T>(int type) where T : GorillaGameManager
		{
			T t = GameMode.GetGameModeInstance(type) as T;
			if (t != null)
			{
				return t;
			}
			return default(T);
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x0014BD58 File Offset: 0x00149F58
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

		// Token: 0x06003AC7 RID: 15047 RVA: 0x0014BDBC File Offset: 0x00149FBC
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

		// Token: 0x06003AC8 RID: 15048 RVA: 0x0014BDE4 File Offset: 0x00149FE4
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

		// Token: 0x06003AC9 RID: 15049 RVA: 0x0014BE0C File Offset: 0x0014A00C
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

		// Token: 0x06003ACA RID: 15050 RVA: 0x000557D6 File Offset: 0x000539D6
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

		// Token: 0x06003ACB RID: 15051 RVA: 0x00055814 File Offset: 0x00053A14
		public static void ReportHit()
		{
			if (NetworkSystem.Instance.InRoom && GameMode.activeNetworkHandler.IsNotNull())
			{
				GameMode.activeNetworkHandler.SendRPC("RPC_ReportHit", false, Array.Empty<object>());
			}
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x00055843 File Offset: 0x00053A43
		public static void BroadcastRoundComplete()
		{
			if (NetworkSystem.Instance.IsMasterClient && NetworkSystem.Instance.InRoom && GameMode.activeNetworkHandler.IsNotNull())
			{
				GameMode.activeNetworkHandler.SendRPC("RPC_BroadcastRoundComplete", true, Array.Empty<object>());
			}
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x0014BE34 File Offset: 0x0014A034
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

		// Token: 0x06003ACE RID: 15054 RVA: 0x0005587E File Offset: 0x00053A7E
		public static void OptOut(VRRig rig)
		{
			GameMode.OptOut(rig.creator.ActorNumber);
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x00055890 File Offset: 0x00053A90
		public static void OptOut(NetPlayer player)
		{
			GameMode.OptOut(player.ActorNumber);
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x0005589D File Offset: 0x00053A9D
		public static void OptOut(int playerActorNumber)
		{
			if (GameMode.optOutPlayers.Add(playerActorNumber))
			{
				GameMode.RefreshPlayers();
			}
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x000558B1 File Offset: 0x00053AB1
		public static void OptIn(VRRig rig)
		{
			GameMode.OptIn(rig.creator.ActorNumber);
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x000558C3 File Offset: 0x00053AC3
		public static void OptIn(NetPlayer player)
		{
			GameMode.OptIn(player.ActorNumber);
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x000558D0 File Offset: 0x00053AD0
		public static void OptIn(int playerActorNumber)
		{
			if (GameMode.optOutPlayers.Remove(playerActorNumber))
			{
				GameMode.RefreshPlayers();
			}
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x000558E4 File Offset: 0x00053AE4
		private static bool CanParticipate(NetPlayer player)
		{
			return player.InRoom() && !GameMode.optOutPlayers.Contains(player.ActorNumber) && NetworkSystem.Instance.GetPlayerTutorialCompletion(player.ActorNumber);
		}

		// Token: 0x04003BD0 RID: 15312
		[SerializeField]
		private GameModeZoneMapping gameModeZoneMapping;

		// Token: 0x04003BD2 RID: 15314
		[OnEnterPlay_SetNull]
		private static GameMode instance;

		// Token: 0x04003BD3 RID: 15315
		[OnEnterPlay_Clear]
		private static Dictionary<int, GorillaGameManager> gameModeTable = new Dictionary<int, GorillaGameManager>();

		// Token: 0x04003BD4 RID: 15316
		[OnEnterPlay_Clear]
		private static Dictionary<string, int> gameModeKeyByName = new Dictionary<string, int>();

		// Token: 0x04003BD5 RID: 15317
		[OnEnterPlay_Clear]
		private static Dictionary<int, FusionGameModeData> fusionTypeTable = new Dictionary<int, FusionGameModeData>();

		// Token: 0x04003BD6 RID: 15318
		[OnEnterPlay_Clear]
		private static List<GorillaGameManager> gameModes = new List<GorillaGameManager>(10);

		// Token: 0x04003BD7 RID: 15319
		[OnEnterPlay_Clear]
		public static readonly List<string> gameModeNames = new List<string>(10);

		// Token: 0x04003BD8 RID: 15320
		[OnEnterPlay_Clear]
		private static readonly List<GorillaGameManager> activatedGameModes = new List<GorillaGameManager>(9);

		// Token: 0x04003BD9 RID: 15321
		[OnEnterPlay_SetNull]
		private static GorillaGameManager activeGameMode = null;

		// Token: 0x04003BDA RID: 15322
		[OnEnterPlay_SetNull]
		private static GameModeSerializer activeNetworkHandler = null;

		// Token: 0x04003BDB RID: 15323
		private static List<Player> participatingPlayers = new List<Player>(10);

		// Token: 0x04003BDC RID: 15324
		[OnEnterPlay_Clear]
		private static readonly HashSet<int> optOutPlayers = new HashSet<int>(10);

		// Token: 0x04003BDD RID: 15325
		[OnEnterPlay_Clear]
		public static readonly List<NetPlayer> ParticipatingPlayers = new List<NetPlayer>(10);

		// Token: 0x0200096A RID: 2410
		// (Invoke) Token: 0x06003AD7 RID: 15063
		public delegate void OnStartGameModeAction(GameModeType newGameModeType);
	}
}
