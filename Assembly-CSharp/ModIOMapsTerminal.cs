using System;
using GorillaNetworking;
using GorillaTagScripts.ModIO;
using KID.Model;
using ModIO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200067B RID: 1659
public class ModIOMapsTerminal : MonoBehaviour
{
	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06002917 RID: 10519 RVA: 0x000C9F98 File Offset: 0x000C8198
	public static int LocalPlayerID
	{
		get
		{
			return NetworkSystem.Instance.LocalPlayer.ActorNumber;
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06002918 RID: 10520 RVA: 0x000C9FA9 File Offset: 0x000C81A9
	public static bool IsDriver
	{
		get
		{
			return ModIOMapsTerminal.localStatus.driverID == ModIOMapsTerminal.LocalPlayerID;
		}
	}

	// Token: 0x06002919 RID: 10521 RVA: 0x000C9FBC File Offset: 0x000C81BC
	private void Awake()
	{
		ModIOMapsTerminal.instance = this;
		ModIOMapsTerminal.hasInstance = true;
	}

	// Token: 0x0600291A RID: 10522 RVA: 0x000C9FCC File Offset: 0x000C81CC
	private void Start()
	{
		ModIOMapsTerminal.localStatus.driverID = -2;
		ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.Access;
		ModIOMapsTerminal.localStatus.previousScreen = ModIOMapsTerminal.ScreenType.Access;
		ModIOMapsTerminal.HideTerminalControlScreen();
		this.accessScreen.Show();
		this.modListScreen.Hide();
		this.modDetailsScreen.Hide();
		GameEvents.OnModIOLoggedIn.AddListener(new UnityAction(this.OnModIOLoggedIn));
		GameEvents.OnModIOLoggedOut.AddListener(new UnityAction(this.OnModIOLoggedOut));
		NetworkSystem.Instance.OnMultiplayerStarted += this.OnJoinedRoom;
		NetworkSystem.Instance.OnReturnedToSinglePlayer += this.OnReturnedToSinglePlayer;
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x000CA07C File Offset: 0x000C827C
	private void LateUpdate()
	{
		if (ModIOMapsTerminal.localStatus.driverID == -2)
		{
			return;
		}
		if (GorillaComputer.instance == null)
		{
			return;
		}
		if (ModIOMapsTerminal.useNametags == GorillaComputer.instance.NametagsEnabled)
		{
			return;
		}
		ModIOMapsTerminal.useNametags = GorillaComputer.instance.NametagsEnabled;
		ModIOMapsTerminal.RefreshDriverNickName();
	}

	// Token: 0x0600291C RID: 10524 RVA: 0x000CA0D4 File Offset: 0x000C82D4
	private void OnDestroy()
	{
		GameEvents.OnModIOLoggedIn.RemoveListener(new UnityAction(this.OnModIOLoggedIn));
		GameEvents.OnModIOLoggedOut.RemoveListener(new UnityAction(this.OnModIOLoggedOut));
		NetworkSystem.Instance.OnMultiplayerStarted -= this.OnJoinedRoom;
		NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.OnReturnedToSinglePlayer;
	}

	// Token: 0x0600291D RID: 10525 RVA: 0x000CA13C File Offset: 0x000C833C
	public static ModIOMapsTerminal.TerminalStatus UpdateAndRetrieveLocalStatus()
	{
		if (ModIOMapsTerminal.instance.modDetailsScreen.isActiveAndEnabled)
		{
			ModIOMapsTerminal.localStatus.modDetailsID = ModIOMapsTerminal.instance.modDetailsScreen.GetModId();
		}
		if (ModIOMapsTerminal.instance.modListScreen.isActiveAndEnabled)
		{
			ModIOMapsTerminal.localStatus.modIndex = ModIOMapsTerminal.instance.modListScreen.SelectedModIndex;
			ModIOMapsTerminal.localStatus.pageIndex = ModIOMapsTerminal.instance.modListScreen.CurrentModPage;
			if (ModIOMapsTerminal.instance.modListScreen.currentState == ModIOModListScreen.ListScreenState.SubscribedMods)
			{
				ModIOMapsTerminal.instance.modListScreen.GetModList(out ModIOMapsTerminal.localStatus.modList);
				ModIOMapsTerminal.localStatus.numModPages = ModIOMapsTerminal.instance.modListScreen.GetNumPages();
			}
			else
			{
				ModIOMapsTerminal.localStatus.modList = Array.Empty<long>();
				ModIOMapsTerminal.localStatus.numModPages = -1;
			}
		}
		return ModIOMapsTerminal.localStatus;
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x000CA21E File Offset: 0x000C841E
	public static void UpdateListScreenState(ModIOModListScreen.ListScreenState screenState)
	{
		ModIOMapsTerminal.localStatus.previousScreen = ModIOMapsTerminal.localStatus.currentScreen;
		ModIOMapsTerminal.localStatus.currentScreen = ((screenState == ModIOModListScreen.ListScreenState.AvailableMods) ? ModIOMapsTerminal.ScreenType.AvailableMods : ModIOMapsTerminal.ScreenType.SubscribedMods);
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x000CA248 File Offset: 0x000C8448
	public static void ShowDetailsScreen(ModProfile profile)
	{
		ModIOMapsTerminal.localStatus.previousScreen = ModIOMapsTerminal.localStatus.currentScreen;
		ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.ModDetails;
		ModIOMapsTerminal.instance.modListScreen.Hide();
		ModIOMapsTerminal.instance.accessScreen.Hide();
		ModIOMapsTerminal.instance.modDetailsScreen.Show();
		ModIOMapsTerminal.instance.modDetailsScreen.SetModProfile(profile);
		ModIOMapsTerminal.SendTerminalStatus(false, false);
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x000CA2B8 File Offset: 0x000C84B8
	public static void ReturnFromDetailsScreen()
	{
		ModIOMapsTerminal.ScreenType previousScreen = ModIOMapsTerminal.localStatus.previousScreen;
		if (previousScreen == ModIOMapsTerminal.ScreenType.ModDetails || previousScreen == ModIOMapsTerminal.ScreenType.Invalid || previousScreen == ModIOMapsTerminal.ScreenType.TerminalControlPrompt)
		{
			ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.AvailableMods;
			ModIOMapsTerminal.localStatus.previousScreen = ModIOMapsTerminal.ScreenType.AvailableMods;
			ModIOMapsTerminal.localStatus.pageIndex = 0;
			ModIOMapsTerminal.localStatus.modIndex = 0;
		}
		else
		{
			ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.localStatus.previousScreen;
		}
		switch (ModIOMapsTerminal.localStatus.currentScreen)
		{
		case ModIOMapsTerminal.ScreenType.Access:
			ModIOMapsTerminal.instance.modListScreen.Hide();
			ModIOMapsTerminal.instance.modDetailsScreen.Hide();
			ModIOMapsTerminal.instance.accessScreen.Show();
			break;
		case ModIOMapsTerminal.ScreenType.AvailableMods:
			ModIOMapsTerminal.instance.modListScreen.UpdateFromTerminalStatus(ModIOMapsTerminal.localStatus);
			ModIOMapsTerminal.instance.modListScreen.Show();
			ModIOMapsTerminal.instance.modDetailsScreen.Hide();
			ModIOMapsTerminal.instance.accessScreen.Hide();
			break;
		case ModIOMapsTerminal.ScreenType.SubscribedMods:
			ModIOMapsTerminal.instance.modListScreen.UpdateFromTerminalStatus(ModIOMapsTerminal.localStatus);
			ModIOMapsTerminal.instance.modListScreen.Show();
			ModIOMapsTerminal.instance.modDetailsScreen.Hide();
			ModIOMapsTerminal.instance.accessScreen.Hide();
			break;
		}
		ModIOMapsTerminal.SendTerminalStatus(ModIOMapsTerminal.localStatus.currentScreen == ModIOMapsTerminal.ScreenType.SubscribedMods, false);
	}

	// Token: 0x06002921 RID: 10529 RVA: 0x000CA40D File Offset: 0x000C860D
	public static void SendTerminalStatus(bool sendFullModList = false, bool forceSearch = false)
	{
		if (!ModIOMapsTerminal.hasInstance)
		{
			return;
		}
		ModIOMapsTerminal.instance.mapTerminalNetworkObject.SendTerminalStatus(sendFullModList, forceSearch);
	}

	// Token: 0x06002922 RID: 10530 RVA: 0x000CA428 File Offset: 0x000C8628
	public static void ResetTerminalControl()
	{
		ModIOMapsTerminal.localStatus.driverID = -2;
		ModIOMapsTerminal.instance.terminalControlButton.UnlockTerminalControl();
		ModIOMapsTerminal.ShowTerminalControlScreen();
	}

	// Token: 0x06002923 RID: 10531 RVA: 0x000CA44A File Offset: 0x000C864A
	public static void HandleTerminalControlStatusChangeRequest(bool lockedStatus, int playerID)
	{
		if (lockedStatus && playerID == -2)
		{
			return;
		}
		if (ModIOMapsTerminal.localStatus.driverID == -2)
		{
			if (!lockedStatus)
			{
				return;
			}
		}
		else if (ModIOMapsTerminal.localStatus.driverID != playerID)
		{
			return;
		}
		ModIOMapsTerminal.SetTerminalControlStatus(lockedStatus, playerID, true);
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x000CA480 File Offset: 0x000C8680
	public static void SetTerminalControlStatus(bool isLocked, int driverID = -2, bool sendRPC = false)
	{
		if (!ModIODataStore.IsLoggedIn())
		{
			ModIOMapsTerminal.cachedNetStatus.driverID = driverID;
			return;
		}
		if (isLocked)
		{
			ModIOMapsTerminal.localStatus.driverID = driverID;
			ModIOMapsTerminal.instance.terminalControlButton.LockTerminalControl();
			ModIOMapsTerminal.HideTerminalControlScreen();
		}
		else
		{
			ModIOMapsTerminal.localStatus.driverID = -2;
			ModIOMapsTerminal.instance.terminalControlButton.UnlockTerminalControl();
			ModIOMapsTerminal.ShowTerminalControlScreen();
		}
		if (sendRPC && NetworkSystem.Instance.IsMasterClient)
		{
			ModIOMapsTerminal.instance.mapTerminalNetworkObject.SetTerminalControlStatus(isLocked, ModIOMapsTerminal.localStatus.driverID);
		}
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x000CA50D File Offset: 0x000C870D
	public static void UpdateStatusFromDriver(long[] data, bool forceSearch = false)
	{
		if (!ModIOMapsTerminal.hasInstance)
		{
			return;
		}
		ModIOMapsTerminal.cachedNetStatus.UnpackData(data);
		if (!ModIODataStore.IsLoggedIn())
		{
			return;
		}
		ModIOMapsTerminal.localStatus.UnpackData(data);
		ModIOMapsTerminal.instance.UpdateScreenToMatchStatus(forceSearch);
	}

	// Token: 0x06002926 RID: 10534 RVA: 0x000CA540 File Offset: 0x000C8740
	private void UpdateScreenToMatchStatus(bool forceSearch = false)
	{
		if (ModIOMapsTerminal.localStatus.driverID == -2)
		{
			this.terminalControlButton.UnlockTerminalControl();
		}
		else
		{
			this.terminalControlButton.LockTerminalControl();
		}
		this.ValidateLocalStatus();
		switch (ModIOMapsTerminal.localStatus.currentScreen)
		{
		case ModIOMapsTerminal.ScreenType.Access:
			this.modListScreen.Hide();
			this.modDetailsScreen.Hide();
			this.accessScreen.Show();
			return;
		case ModIOMapsTerminal.ScreenType.TerminalControlPrompt:
			this.modListScreen.Hide();
			this.modDetailsScreen.Hide();
			this.accessScreen.Show();
			this.accessScreen.ShowTerminalControlPrompt();
			return;
		case ModIOMapsTerminal.ScreenType.AvailableMods:
			this.accessScreen.Hide();
			this.modDetailsScreen.Hide();
			this.modListScreen.UpdateFromTerminalStatus(ModIOMapsTerminal.localStatus);
			this.modListScreen.Show();
			if (forceSearch)
			{
				this.modListScreen.Refresh(null);
			}
			return;
		case ModIOMapsTerminal.ScreenType.SubscribedMods:
			this.accessScreen.Hide();
			this.modDetailsScreen.Hide();
			this.modListScreen.UpdateFromTerminalStatus(ModIOMapsTerminal.localStatus);
			this.modListScreen.Show();
			if (forceSearch)
			{
				this.modListScreen.Refresh(ModIOMapsTerminal.localStatus.modList);
				return;
			}
			if (!ModIOMapsTerminal.IsDriver && !this.modListScreen.DoesModListMatchDisplay(ModIOMapsTerminal.localStatus.modList))
			{
				this.modListScreen.ShowCustomModList(ModIOMapsTerminal.localStatus.modList);
			}
			return;
		case ModIOMapsTerminal.ScreenType.ModDetails:
			this.accessScreen.Hide();
			this.modListScreen.UpdateFromTerminalStatus(ModIOMapsTerminal.localStatus);
			this.modListScreen.Hide();
			this.modDetailsScreen.Show();
			this.modDetailsScreen.RetrieveProfileFromModIO(ModIOMapsTerminal.localStatus.modDetailsID, null);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x000CA6F4 File Offset: 0x000C88F4
	private void ValidateLocalStatus()
	{
		if (!ModIODataStore.IsLoggedIn() || ModIOMapsTerminal.localStatus.driverID == -2)
		{
			return;
		}
		if (ModIOMapLoader.IsModLoaded(0L))
		{
			ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.ModDetails;
			ModIOMapsTerminal.localStatus.modDetailsID = ModIOMapLoader.LoadedMapModId;
			ModIOMapsTerminal.SendTerminalStatus(false, false);
			return;
		}
		if (CustomMapManager.IsLoading(0L))
		{
			ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.ModDetails;
			ModIOMapsTerminal.localStatus.modDetailsID = CustomMapManager.LoadingMapId;
			ModIOMapsTerminal.SendTerminalStatus(false, false);
			return;
		}
		if (CustomMapManager.GetRoomMapId() != ModId.Null)
		{
			ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.ModDetails;
			ModIOMapsTerminal.localStatus.modDetailsID = CustomMapManager.GetRoomMapId().id;
			ModIOMapsTerminal.SendTerminalStatus(false, false);
		}
	}

	// Token: 0x06002928 RID: 10536 RVA: 0x000CA7A4 File Offset: 0x000C89A4
	private void OnModIOLoggedIn()
	{
		if (ModIOMapsTerminal.localStatus.currentScreen == ModIOMapsTerminal.ScreenType.Access)
		{
			if (!NetworkSystem.Instance.InRoom || ModIOMapsTerminal.cachedNetStatus.currentScreen == ModIOMapsTerminal.ScreenType.Invalid)
			{
				ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.TerminalControlPrompt;
				ModIOMapsTerminal.localStatus.previousScreen = ModIOMapsTerminal.ScreenType.TerminalControlPrompt;
				ModIOMapsTerminal.localStatus.modIndex = 0;
			}
			else
			{
				ModIOMapsTerminal.localStatus.Copy(ModIOMapsTerminal.cachedNetStatus);
				this.UpdateScreenToMatchStatus(false);
			}
			if (ModIOMapsTerminal.localStatus.driverID == -2)
			{
				ModIOMapsTerminal.ShowTerminalControlScreen();
			}
		}
	}

	// Token: 0x06002929 RID: 10537 RVA: 0x000CA822 File Offset: 0x000C8A22
	private void OnModIOLoggedOut()
	{
		ModIOMapsTerminal.ResetTerminalControl();
		ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.Access;
		ModIOMapsTerminal.localStatus.previousScreen = ModIOMapsTerminal.ScreenType.Access;
		this.accessScreen.Reset();
		this.UpdateScreenToMatchStatus(false);
	}

	// Token: 0x0600292A RID: 10538 RVA: 0x000CA854 File Offset: 0x000C8A54
	public void HandleTerminalControlButtonPressed()
	{
		if (!ModIODataStore.IsLoggedIn())
		{
			this.accessScreen.DisplayError("User is logged out of mod.io, not allowing terminal check-out");
			return;
		}
		if (!NetworkSystem.Instance.InRoom)
		{
			ModIOMapsTerminal.SetTerminalControlStatus(!this.terminalControlButton.IsLocked, ModIOMapsTerminal.LocalPlayerID, false);
			return;
		}
		if (ModIOMapsTerminal.localStatus.driverID != -2 && !ModIOMapsTerminal.IsDriver)
		{
			return;
		}
		if (this.mapTerminalNetworkObject.HasAuthority)
		{
			ModIOMapsTerminal.HandleTerminalControlStatusChangeRequest(!this.terminalControlButton.IsLocked, ModIOMapsTerminal.LocalPlayerID);
			return;
		}
		this.mapTerminalNetworkObject.RequestTerminalControlStatusChange(!this.terminalControlButton.IsLocked);
	}

	// Token: 0x0600292B RID: 10539 RVA: 0x000CA8F4 File Offset: 0x000C8AF4
	private static void ShowTerminalControlScreen()
	{
		if (!ModIOMapsTerminal.hasInstance)
		{
			return;
		}
		ModIOMapsTerminal.instance.terminalControllerLabelText.gameObject.SetActive(false);
		ModIOMapsTerminal.instance.terminalControllerText.gameObject.SetActive(false);
		if (!ModIODataStore.IsLoggedIn())
		{
			return;
		}
		if (ModIOMapsTerminal.localStatus.currentScreen > ModIOMapsTerminal.ScreenType.TerminalControlPrompt)
		{
			ModIOMapsTerminal.localStatus.previousScreen = ModIOMapsTerminal.localStatus.currentScreen;
		}
		else
		{
			ModIOMapsTerminal.localStatus.previousScreen = ModIOMapsTerminal.ScreenType.TerminalControlPrompt;
		}
		ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.TerminalControlPrompt;
		ModIOMapsTerminal.instance.UpdateScreenToMatchStatus(false);
	}

	// Token: 0x0600292C RID: 10540 RVA: 0x000CA980 File Offset: 0x000C8B80
	private static void HideTerminalControlScreen()
	{
		if (!ModIOMapsTerminal.hasInstance || !ModIODataStore.IsLoggedIn())
		{
			return;
		}
		ModIOMapsTerminal.RefreshDriverNickName();
		if (ModIOMapsTerminal.localStatus.currentScreen != ModIOMapsTerminal.ScreenType.TerminalControlPrompt)
		{
			return;
		}
		if (ModIOMapsTerminal.localStatus.previousScreen > ModIOMapsTerminal.ScreenType.TerminalControlPrompt)
		{
			ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.localStatus.previousScreen;
		}
		else if (ModIOMapLoader.IsModLoaded(0L) || CustomMapManager.IsLoading(0L) || CustomMapManager.GetRoomMapId() != ModId.Null)
		{
			ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.ModDetails;
		}
		else
		{
			ModIOMapsTerminal.localStatus.currentScreen = ModIOMapsTerminal.ScreenType.AvailableMods;
		}
		ModIOMapsTerminal.instance.UpdateScreenToMatchStatus(false);
	}

	// Token: 0x0600292D RID: 10541 RVA: 0x000CAA18 File Offset: 0x000C8C18
	public static void RequestDriverNickNameRefresh()
	{
		if (!ModIOMapsTerminal.hasInstance)
		{
			return;
		}
		if (!ModIOMapsTerminal.IsDriver)
		{
			return;
		}
		ModIOMapsTerminal.RefreshDriverNickName();
		ModIOMapsTerminal.instance.mapTerminalNetworkObject.RefreshDriverNickName();
	}

	// Token: 0x0600292E RID: 10542 RVA: 0x000CAA40 File Offset: 0x000C8C40
	public static void RefreshDriverNickName()
	{
		if (!ModIOMapsTerminal.hasInstance)
		{
			return;
		}
		ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
		bool flag = (customNicknamePermissionStatus.Item1 || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
		ModIOMapsTerminal.instance.terminalControllerLabelText.gameObject.SetActive(true);
		if (NetworkSystem.Instance.InRoom)
		{
			NetPlayer netPlayerByID = NetworkSystem.Instance.GetNetPlayerByID(ModIOMapsTerminal.localStatus.driverID);
			ModIOMapsTerminal.instance.terminalControllerText.text = netPlayerByID.DefaultName;
			if (ModIOMapsTerminal.useNametags && flag)
			{
				RigContainer rigContainer;
				if (netPlayerByID.IsLocal)
				{
					ModIOMapsTerminal.instance.terminalControllerText.text = netPlayerByID.NickName;
				}
				else if (VRRigCache.Instance.TryGetVrrig(netPlayerByID, out rigContainer))
				{
					ModIOMapsTerminal.instance.terminalControllerText.text = rigContainer.Rig.playerNameVisible;
				}
			}
		}
		else
		{
			ModIOMapsTerminal.instance.terminalControllerText.text = ((ModIOMapsTerminal.useNametags && flag) ? NetworkSystem.Instance.LocalPlayer.NickName : NetworkSystem.Instance.LocalPlayer.DefaultName);
		}
		ModIOMapsTerminal.instance.terminalControllerText.gameObject.SetActive(true);
		ModIOMapsTerminal.instance.modListScreen.RefreshDriverNickname(ModIOMapsTerminal.instance.terminalControllerText.text);
	}

	// Token: 0x0600292F RID: 10543 RVA: 0x000CAB90 File Offset: 0x000C8D90
	private void OnReturnedToSinglePlayer()
	{
		if (ModIOMapsTerminal.localStatus.driverID != ModIOMapsTerminal.cachedLocalPlayerID)
		{
			ModIOMapsTerminal.ResetTerminalControl();
		}
		else
		{
			ModIOMapsTerminal.localStatus.driverID = ModIOMapsTerminal.LocalPlayerID;
		}
		ModIOMapsTerminal.cachedLocalPlayerID = -1;
	}

	// Token: 0x06002930 RID: 10544 RVA: 0x000CABBF File Offset: 0x000C8DBF
	private void OnJoinedRoom()
	{
		ModIOMapsTerminal.cachedLocalPlayerID = ModIOMapsTerminal.LocalPlayerID;
		ModIOMapsTerminal.ResetTerminalControl();
	}

	// Token: 0x06002931 RID: 10545 RVA: 0x000CABD0 File Offset: 0x000C8DD0
	public static bool IsLocked()
	{
		return ModIOMapsTerminal.localStatus.driverID != -2;
	}

	// Token: 0x06002932 RID: 10546 RVA: 0x000CABE3 File Offset: 0x000C8DE3
	public static int GetDriverID()
	{
		return ModIOMapsTerminal.localStatus.driverID;
	}

	// Token: 0x06002933 RID: 10547 RVA: 0x000CABEF File Offset: 0x000C8DEF
	public static string GetDriverNickname()
	{
		if (!ModIOMapsTerminal.hasInstance)
		{
			return "";
		}
		return ModIOMapsTerminal.instance.terminalControllerText.text;
	}

	// Token: 0x04002E4D RID: 11853
	[SerializeField]
	private ModIOAccessScreen accessScreen;

	// Token: 0x04002E4E RID: 11854
	[SerializeField]
	private ModIOModListScreen modListScreen;

	// Token: 0x04002E4F RID: 11855
	[SerializeField]
	private ModIOModDetailsScreen modDetailsScreen;

	// Token: 0x04002E50 RID: 11856
	[SerializeField]
	private ModIOSerializer mapTerminalNetworkObject;

	// Token: 0x04002E51 RID: 11857
	[SerializeField]
	private ModIOTerminalControlButton terminalControlButton;

	// Token: 0x04002E52 RID: 11858
	[SerializeField]
	private TMP_Text terminalControllerLabelText;

	// Token: 0x04002E53 RID: 11859
	[SerializeField]
	private TMP_Text terminalControllerText;

	// Token: 0x04002E54 RID: 11860
	public const int NO_DRIVER_ID = -2;

	// Token: 0x04002E55 RID: 11861
	private static ModIOMapsTerminal instance;

	// Token: 0x04002E56 RID: 11862
	private static bool hasInstance;

	// Token: 0x04002E57 RID: 11863
	private static ModIOMapsTerminal.TerminalStatus localStatus = new ModIOMapsTerminal.TerminalStatus();

	// Token: 0x04002E58 RID: 11864
	private static ModIOMapsTerminal.TerminalStatus cachedNetStatus = new ModIOMapsTerminal.TerminalStatus();

	// Token: 0x04002E59 RID: 11865
	private static int cachedLocalPlayerID = -1;

	// Token: 0x04002E5A RID: 11866
	private static bool useNametags;

	// Token: 0x0200067C RID: 1660
	public enum ScreenType
	{
		// Token: 0x04002E5C RID: 11868
		Invalid = -1,
		// Token: 0x04002E5D RID: 11869
		Access,
		// Token: 0x04002E5E RID: 11870
		TerminalControlPrompt,
		// Token: 0x04002E5F RID: 11871
		AvailableMods,
		// Token: 0x04002E60 RID: 11872
		SubscribedMods,
		// Token: 0x04002E61 RID: 11873
		ModDetails
	}

	// Token: 0x0200067D RID: 1661
	public class TerminalStatus
	{
		// Token: 0x06002936 RID: 10550 RVA: 0x000CAC2C File Offset: 0x000C8E2C
		public long[] PackData(bool packModList)
		{
			long[] array;
			if (packModList && !this.modList.IsNullOrEmpty<long>())
			{
				array = new long[2 + this.modList.Length];
				for (int i = 2; i < array.Length; i++)
				{
					array[i] = this.modList[i - 2];
				}
			}
			else
			{
				array = new long[2];
			}
			array[0] = (long)this.currentScreen;
			array[0] += (long)((long)this.previousScreen << 4);
			array[0] += (long)((long)(this.modIndex + 1) << 8);
			array[0] += (long)((long)(this.numModPages + 1) << 16);
			array[0] += (long)(this.pageIndex + 1) << 32;
			array[1] = this.modDetailsID;
			return array;
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x000CACEC File Offset: 0x000C8EEC
		public void UnpackData(long[] data)
		{
			this.currentScreen = (ModIOMapsTerminal.ScreenType)(data[0] & 15L);
			this.previousScreen = (ModIOMapsTerminal.ScreenType)(data[0] >> 4 & 15L);
			this.modIndex = (int)(data[0] >> 8 & 255L);
			this.modIndex--;
			this.numModPages = (int)(data[0] >> 16 & 65535L);
			this.numModPages--;
			this.pageIndex = (int)(data[0] >> 32);
			this.pageIndex--;
			this.modDetailsID = data[1];
			if (data.Length <= 2)
			{
				return;
			}
			this.modList = new long[data.Length - 2];
			for (int i = 0; i < this.modList.Length; i++)
			{
				this.modList[i] = data[i + 2];
			}
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000CADB8 File Offset: 0x000C8FB8
		public void Copy(ModIOMapsTerminal.TerminalStatus other)
		{
			this.currentScreen = other.currentScreen;
			this.previousScreen = other.previousScreen;
			this.driverID = other.driverID;
			this.modIndex = other.modIndex;
			this.numModPages = other.numModPages;
			this.pageIndex = other.pageIndex;
			this.modDetailsID = other.modDetailsID;
			this.modList = other.modList;
		}

		// Token: 0x04002E62 RID: 11874
		public ModIOMapsTerminal.ScreenType currentScreen = ModIOMapsTerminal.ScreenType.Invalid;

		// Token: 0x04002E63 RID: 11875
		public ModIOMapsTerminal.ScreenType previousScreen = ModIOMapsTerminal.ScreenType.Invalid;

		// Token: 0x04002E64 RID: 11876
		public int driverID;

		// Token: 0x04002E65 RID: 11877
		public int modIndex;

		// Token: 0x04002E66 RID: 11878
		public int pageIndex;

		// Token: 0x04002E67 RID: 11879
		public long modDetailsID;

		// Token: 0x04002E68 RID: 11880
		public long[] modList;

		// Token: 0x04002E69 RID: 11881
		public int numModPages = -1;

		// Token: 0x04002E6A RID: 11882
		private const int MINIMUM_ARRAY_LENGTH = 2;
	}
}
