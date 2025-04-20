using System;
using System.Collections.Generic;
using GorillaNetworking;
using GorillaTagScripts.ModIO;
using KID.Model;
using ModIO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200069D RID: 1693
public class CustomMapsTerminal : MonoBehaviour
{
	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06002A10 RID: 10768 RVA: 0x0004C5EA File Offset: 0x0004A7EA
	public static int LocalPlayerID
	{
		get
		{
			return NetworkSystem.Instance.LocalPlayer.ActorNumber;
		}
	}

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06002A11 RID: 10769 RVA: 0x0004C5FB File Offset: 0x0004A7FB
	public static bool IsDriver
	{
		get
		{
			return CustomMapsTerminal.localDriverID == CustomMapsTerminal.LocalPlayerID;
		}
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x0004C609 File Offset: 0x0004A809
	private void Awake()
	{
		CustomMapsTerminal.instance = this;
		CustomMapsTerminal.hasInstance = true;
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x0011AD78 File Offset: 0x00118F78
	private void Start()
	{
		CustomMapsTerminal.localDriverID = -2;
		CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.Access;
		CustomMapsTerminal.localStatus.previousScreen = CustomMapsTerminal.ScreenType.Access;
		CustomMapsTerminal.localStatus.modsPerPage = CustomMapsTerminal.instance.modListScreen.ModsPerPage;
		CustomMapsTerminal.cachedNetStatus.modsPerPage = CustomMapsTerminal.localStatus.modsPerPage;
		CustomMapsTerminal.HideTerminalControlScreen();
		this.accessScreen.Show();
		this.modListScreen.Hide();
		this.modDetailsScreen.Hide();
		GameEvents.OnModIOLoggedIn.AddListener(new UnityAction(this.OnModIOLoggedIn));
		GameEvents.OnModIOLoggedOut.AddListener(new UnityAction(this.OnModIOLoggedOut));
		NetworkSystem.Instance.OnMultiplayerStarted += this.OnJoinedRoom;
		NetworkSystem.Instance.OnReturnedToSinglePlayer += this.OnReturnedToSinglePlayer;
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x0011AE50 File Offset: 0x00119050
	private void LateUpdate()
	{
		if (CustomMapsTerminal.localDriverID == -2)
		{
			return;
		}
		if (GorillaComputer.instance == null)
		{
			return;
		}
		if (CustomMapsTerminal.useNametags == GorillaComputer.instance.NametagsEnabled)
		{
			return;
		}
		CustomMapsTerminal.useNametags = GorillaComputer.instance.NametagsEnabled;
		CustomMapsTerminal.RefreshDriverNickName();
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x0011AEA4 File Offset: 0x001190A4
	private void OnDestroy()
	{
		GameEvents.OnModIOLoggedIn.RemoveListener(new UnityAction(this.OnModIOLoggedIn));
		GameEvents.OnModIOLoggedOut.RemoveListener(new UnityAction(this.OnModIOLoggedOut));
		NetworkSystem.Instance.OnMultiplayerStarted -= this.OnJoinedRoom;
		NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.OnReturnedToSinglePlayer;
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x0011AF0C File Offset: 0x0011910C
	public static CustomMapsTerminal.TerminalStatus UpdateAndRetrieveLocalStatus()
	{
		CustomMapsTerminal.localStatus.sortType = CustomMapsTerminal.instance.modListScreen.SortType;
		if (CustomMapsTerminal.instance.modDetailsScreen.isActiveAndEnabled)
		{
			CustomMapsTerminal.localStatus.modDetailsID = CustomMapsTerminal.instance.modDetailsScreen.GetModId();
		}
		if (CustomMapsTerminal.instance.modListScreen.isActiveAndEnabled)
		{
			CustomMapsTerminal.localStatus.modIndex = CustomMapsTerminal.instance.modListScreen.SelectedModIndex;
			CustomMapsTerminal.localStatus.pageIndex = CustomMapsTerminal.instance.modListScreen.CurrentModPage;
			if (CustomMapsTerminal.instance.modListScreen.currentState == CustomMapsListScreen.ListScreenState.SubscribedMods)
			{
				CustomMapsTerminal.instance.modListScreen.GetModList(out CustomMapsTerminal.localStatus.modList);
				CustomMapsTerminal.localStatus.numModPages = CustomMapsTerminal.instance.modListScreen.GetNumPages();
			}
			else
			{
				CustomMapsTerminal.localStatus.modList = Array.Empty<long>();
				CustomMapsTerminal.localStatus.numModPages = -1;
			}
		}
		return CustomMapsTerminal.localStatus;
	}

	// Token: 0x06002A17 RID: 10775 RVA: 0x0011B008 File Offset: 0x00119208
	public static void UpdateListScreenState(CustomMapsListScreen.ListScreenState screenState)
	{
		CustomMapsTerminal.localStatus.previousScreen = CustomMapsTerminal.localStatus.currentScreen;
		CustomMapsTerminal.localStatus.currentScreen = ((screenState == CustomMapsListScreen.ListScreenState.AvailableMods) ? CustomMapsTerminal.ScreenType.AvailableMods : CustomMapsTerminal.ScreenType.SubscribedMods);
		if (CustomMapsTerminal.hasInstance && CustomMapsTerminal.instance.subscribedOnlyButton != null)
		{
			CustomMapsTerminal.instance.subscribedOnlyButton.SetButtonStatus(screenState == CustomMapsListScreen.ListScreenState.SubscribedMods);
		}
	}

	// Token: 0x06002A18 RID: 10776 RVA: 0x0011B068 File Offset: 0x00119268
	public static void ShowDetailsScreen(ModProfile profile)
	{
		CustomMapsTerminal.localStatus.previousScreen = CustomMapsTerminal.localStatus.currentScreen;
		CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.ModDetails;
		CustomMapsTerminal.instance.modListScreen.Hide();
		CustomMapsTerminal.instance.accessScreen.Hide();
		CustomMapsTerminal.instance.modDetailsScreen.Show();
		CustomMapsTerminal.instance.modDetailsScreen.SetModProfile(profile);
		CustomMapsTerminal.SendTerminalStatus(false, false);
	}

	// Token: 0x06002A19 RID: 10777 RVA: 0x0011B0D8 File Offset: 0x001192D8
	public static void ReturnFromDetailsScreen()
	{
		CustomMapsTerminal.ScreenType previousScreen = CustomMapsTerminal.localStatus.previousScreen;
		if (previousScreen == CustomMapsTerminal.ScreenType.ModDetails || previousScreen == CustomMapsTerminal.ScreenType.Invalid || previousScreen == CustomMapsTerminal.ScreenType.TerminalControlPrompt)
		{
			CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.AvailableMods;
			CustomMapsTerminal.localStatus.previousScreen = CustomMapsTerminal.ScreenType.AvailableMods;
			CustomMapsTerminal.localStatus.pageIndex = 0;
			CustomMapsTerminal.localStatus.modIndex = 0;
		}
		else
		{
			CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.localStatus.previousScreen;
		}
		switch (CustomMapsTerminal.localStatus.currentScreen)
		{
		case CustomMapsTerminal.ScreenType.Access:
			CustomMapsTerminal.instance.modListScreen.Hide();
			CustomMapsTerminal.instance.modDetailsScreen.Hide();
			CustomMapsTerminal.instance.accessScreen.Show();
			break;
		case CustomMapsTerminal.ScreenType.AvailableMods:
			CustomMapsTerminal.instance.modListScreen.UpdateFromTerminalStatus(CustomMapsTerminal.localStatus);
			CustomMapsTerminal.instance.modListScreen.Show();
			CustomMapsTerminal.instance.modDetailsScreen.Hide();
			CustomMapsTerminal.instance.accessScreen.Hide();
			break;
		case CustomMapsTerminal.ScreenType.SubscribedMods:
			CustomMapsTerminal.instance.modListScreen.UpdateFromTerminalStatus(CustomMapsTerminal.localStatus);
			CustomMapsTerminal.instance.modListScreen.Show();
			CustomMapsTerminal.instance.modDetailsScreen.Hide();
			CustomMapsTerminal.instance.accessScreen.Hide();
			break;
		}
		CustomMapsTerminal.SendTerminalStatus(CustomMapsTerminal.localStatus.currentScreen == CustomMapsTerminal.ScreenType.SubscribedMods, false);
	}

	// Token: 0x06002A1A RID: 10778 RVA: 0x0011B230 File Offset: 0x00119430
	public static bool SetTagButtonStatus(short tagIndex, out string tagText)
	{
		if (!CustomMapsTerminal.hasInstance)
		{
			tagText = string.Empty;
			return false;
		}
		int num = (int)MathF.Max(0f, (float)(tagIndex - 1));
		short num2 = (short)MathF.Pow(2f, (float)num);
		int num3;
		bool flag;
		if ((CustomMapsTerminal.localStatus.tagFlags & num2) == 0)
		{
			num3 = (int)(CustomMapsTerminal.localStatus.tagFlags | num2);
			flag = true;
		}
		else
		{
			num3 = (int)(CustomMapsTerminal.localStatus.tagFlags & ~(int)num2);
			flag = false;
		}
		CustomMapsTerminal.localStatus.tagFlags = (short)num3;
		CustomMapsTerminal.instance.tagButtons[num].SetButtonStatus(flag);
		tagText = CustomMapsTerminal.instance.tagButtons[num].tagText;
		return flag;
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x0004C617 File Offset: 0x0004A817
	public static void SendTerminalStatus(bool sendFullModList = false, bool forceSearch = false)
	{
		if (!CustomMapsTerminal.hasInstance)
		{
			return;
		}
		CustomMapsTerminal.instance.mapTerminalNetworkObject.SendTerminalStatus(sendFullModList, forceSearch);
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x0004C632 File Offset: 0x0004A832
	public static void ResetTerminalControl()
	{
		CustomMapsTerminal.localDriverID = -2;
		CustomMapsTerminal.instance.terminalControlButton.UnlockTerminalControl();
		CustomMapsTerminal.ShowTerminalControlScreen();
	}

	// Token: 0x06002A1D RID: 10781 RVA: 0x0004C64F File Offset: 0x0004A84F
	public static void HandleTerminalControlStatusChangeRequest(bool lockedStatus, int playerID)
	{
		if (lockedStatus && playerID == -2)
		{
			return;
		}
		if (CustomMapsTerminal.localDriverID == -2)
		{
			if (!lockedStatus)
			{
				return;
			}
		}
		else if (CustomMapsTerminal.localDriverID != playerID)
		{
			return;
		}
		CustomMapsTerminal.SetTerminalControlStatus(lockedStatus, playerID, true);
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x0011B2D8 File Offset: 0x001194D8
	public static void SetTerminalControlStatus(bool isLocked, int driverID = -2, bool sendRPC = false)
	{
		if (!ModIOManager.IsLoggedIn())
		{
			CustomMapsTerminal.cachedNetStatus.driverID = driverID;
			return;
		}
		if (isLocked)
		{
			CustomMapsTerminal.localDriverID = driverID;
			CustomMapsTerminal.instance.terminalControlButton.LockTerminalControl();
			CustomMapsTerminal.HideTerminalControlScreen();
		}
		else
		{
			CustomMapsTerminal.localDriverID = -2;
			CustomMapsTerminal.instance.terminalControlButton.UnlockTerminalControl();
			CustomMapsTerminal.ShowTerminalControlScreen();
		}
		if (sendRPC && NetworkSystem.Instance.IsMasterClient)
		{
			CustomMapsTerminal.instance.mapTerminalNetworkObject.SetTerminalControlStatus(isLocked, CustomMapsTerminal.localDriverID);
		}
	}

	// Token: 0x06002A1F RID: 10783 RVA: 0x0011B358 File Offset: 0x00119558
	public static void UpdateStatusFromDriver(long[] data, int driverID, bool forceSearch = false)
	{
		if (!CustomMapsTerminal.hasInstance)
		{
			return;
		}
		CustomMapsTerminal.localDriverID = driverID;
		CustomMapsTerminal.cachedNetStatus.UnpackData(data);
		if (!ModIOManager.IsLoggedIn())
		{
			return;
		}
		CustomMapsTerminal.localStatus.UnpackData(data);
		if (CustomMapsTerminal.localDriverID != -2)
		{
			CustomMapsTerminal.RefreshDriverNickName();
		}
		CustomMapsTerminal.instance.UpdateScreenToMatchStatus(forceSearch);
	}

	// Token: 0x06002A20 RID: 10784 RVA: 0x0004C678 File Offset: 0x0004A878
	public static void ClearTags()
	{
		CustomMapsTerminal.localTagStatus = 0;
		CustomMapsTerminal.instance.modListScreen.ClearTags(true);
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x0011B3AC File Offset: 0x001195AC
	private void UpdateTagsAndSortFromDriver()
	{
		CustomMapsTerminal.instance.modListScreen.SortType = CustomMapsTerminal.localStatus.sortType;
		if (CustomMapsTerminal.localTagStatus == CustomMapsTerminal.localStatus.tagFlags)
		{
			return;
		}
		CustomMapsTerminal.localTagStatus = CustomMapsTerminal.localStatus.tagFlags;
		List<string> list = new List<string>();
		for (int i = 0; i < 8; i++)
		{
			int num = (int)Mathf.Pow(2f, (float)i);
			if (((int)CustomMapsTerminal.localStatus.tagFlags & num) == 0)
			{
				CustomMapsTerminal.instance.tagButtons[i].SetButtonStatus(false);
			}
			else
			{
				CustomMapsTerminal.instance.tagButtons[i].SetButtonStatus(true);
				list.Add(CustomMapsTerminal.instance.tagButtons[i].tagText);
			}
		}
		CustomMapsTerminal.instance.modListScreen.UpdateTagsFromDriver(list);
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x0011B47C File Offset: 0x0011967C
	private void UpdateScreenToMatchStatus(bool forceSearch = false)
	{
		if (CustomMapsTerminal.localDriverID == -2)
		{
			this.terminalControlButton.UnlockTerminalControl();
		}
		else
		{
			this.terminalControlButton.LockTerminalControl();
		}
		this.ValidateLocalStatus();
		this.UpdateTagsAndSortFromDriver();
		switch (CustomMapsTerminal.localStatus.currentScreen)
		{
		case CustomMapsTerminal.ScreenType.Access:
			this.modListScreen.Hide();
			this.modDetailsScreen.Hide();
			this.accessScreen.Show();
			return;
		case CustomMapsTerminal.ScreenType.TerminalControlPrompt:
			this.modListScreen.Hide();
			this.modDetailsScreen.Hide();
			this.accessScreen.Show();
			this.accessScreen.ShowTerminalControlPrompt();
			return;
		case CustomMapsTerminal.ScreenType.AvailableMods:
			this.accessScreen.Hide();
			this.modDetailsScreen.Hide();
			this.modListScreen.UpdateFromTerminalStatus(CustomMapsTerminal.localStatus);
			this.modListScreen.Show();
			if (forceSearch)
			{
				this.modListScreen.Refresh(null);
			}
			return;
		case CustomMapsTerminal.ScreenType.SubscribedMods:
			this.accessScreen.Hide();
			this.modDetailsScreen.Hide();
			this.modListScreen.UpdateFromTerminalStatus(CustomMapsTerminal.localStatus);
			this.modListScreen.Show();
			if (forceSearch)
			{
				this.modListScreen.Refresh(CustomMapsTerminal.localStatus.modList);
				return;
			}
			if (!CustomMapsTerminal.IsDriver && !this.modListScreen.DoesModListMatchDisplay(CustomMapsTerminal.localStatus.modList))
			{
				this.modListScreen.ShowCustomModList(CustomMapsTerminal.localStatus.modList);
			}
			return;
		case CustomMapsTerminal.ScreenType.ModDetails:
			this.accessScreen.Hide();
			this.modListScreen.UpdateFromTerminalStatus(CustomMapsTerminal.localStatus);
			this.modListScreen.Hide();
			this.modDetailsScreen.Show();
			this.modDetailsScreen.RetrieveProfileFromModIO(CustomMapsTerminal.localStatus.modDetailsID, null);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002A23 RID: 10787 RVA: 0x0011B634 File Offset: 0x00119834
	private void ValidateLocalStatus()
	{
		if (!ModIOManager.IsLoggedIn() || CustomMapsTerminal.localDriverID == -2)
		{
			return;
		}
		if (CustomMapLoader.IsModLoaded(0L))
		{
			CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.ModDetails;
			CustomMapsTerminal.localStatus.modDetailsID = CustomMapLoader.LoadedMapModId;
			CustomMapsTerminal.SendTerminalStatus(false, false);
			return;
		}
		if (CustomMapManager.IsLoading(0L))
		{
			CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.ModDetails;
			CustomMapsTerminal.localStatus.modDetailsID = CustomMapManager.LoadingMapId;
			CustomMapsTerminal.SendTerminalStatus(false, false);
			return;
		}
		if (CustomMapManager.GetRoomMapId() != ModId.Null)
		{
			CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.ModDetails;
			CustomMapsTerminal.localStatus.modDetailsID = CustomMapManager.GetRoomMapId().id;
			CustomMapsTerminal.SendTerminalStatus(false, false);
		}
	}

	// Token: 0x06002A24 RID: 10788 RVA: 0x0011B6E0 File Offset: 0x001198E0
	private void OnModIOLoggedIn()
	{
		if (CustomMapsTerminal.localStatus.currentScreen == CustomMapsTerminal.ScreenType.Access)
		{
			if (!NetworkSystem.Instance.InRoom || CustomMapsTerminal.cachedNetStatus.currentScreen == CustomMapsTerminal.ScreenType.Invalid)
			{
				CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.TerminalControlPrompt;
				CustomMapsTerminal.localStatus.previousScreen = CustomMapsTerminal.ScreenType.TerminalControlPrompt;
				CustomMapsTerminal.localStatus.modIndex = 0;
			}
			else
			{
				CustomMapsTerminal.localStatus.Copy(CustomMapsTerminal.cachedNetStatus);
				this.UpdateScreenToMatchStatus(false);
			}
			if (CustomMapsTerminal.localDriverID == -2)
			{
				CustomMapsTerminal.ShowTerminalControlScreen();
			}
		}
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x0004C690 File Offset: 0x0004A890
	private void OnModIOLoggedOut()
	{
		CustomMapsTerminal.ResetTerminalControl();
		CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.Access;
		CustomMapsTerminal.localStatus.previousScreen = CustomMapsTerminal.ScreenType.Access;
		this.accessScreen.Reset();
		this.UpdateScreenToMatchStatus(false);
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x0011B75C File Offset: 0x0011995C
	public void HandleTerminalControlButtonPressed()
	{
		if (!ModIOManager.IsLoggedIn())
		{
			this.accessScreen.DisplayError("User is logged out of mod.io, not allowing terminal check-out");
			return;
		}
		if (!NetworkSystem.Instance.InRoom)
		{
			CustomMapsTerminal.SetTerminalControlStatus(!this.terminalControlButton.IsLocked, CustomMapsTerminal.LocalPlayerID, false);
			return;
		}
		if (CustomMapsTerminal.localDriverID != -2 && !CustomMapsTerminal.IsDriver)
		{
			return;
		}
		if (this.mapTerminalNetworkObject.HasAuthority)
		{
			CustomMapsTerminal.HandleTerminalControlStatusChangeRequest(!this.terminalControlButton.IsLocked, CustomMapsTerminal.LocalPlayerID);
			return;
		}
		this.mapTerminalNetworkObject.RequestTerminalControlStatusChange(!this.terminalControlButton.IsLocked);
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x0011B7F8 File Offset: 0x001199F8
	private static void ShowTerminalControlScreen()
	{
		if (!CustomMapsTerminal.hasInstance)
		{
			return;
		}
		CustomMapsTerminal.instance.terminalControllerLabelText.gameObject.SetActive(false);
		CustomMapsTerminal.instance.terminalControllerText.gameObject.SetActive(false);
		if (!ModIOManager.IsLoggedIn())
		{
			return;
		}
		if (CustomMapsTerminal.localStatus.currentScreen > CustomMapsTerminal.ScreenType.TerminalControlPrompt)
		{
			CustomMapsTerminal.localStatus.previousScreen = CustomMapsTerminal.localStatus.currentScreen;
		}
		else
		{
			CustomMapsTerminal.localStatus.previousScreen = CustomMapsTerminal.ScreenType.TerminalControlPrompt;
		}
		CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.TerminalControlPrompt;
		CustomMapsTerminal.instance.UpdateScreenToMatchStatus(false);
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x0011B884 File Offset: 0x00119A84
	private static void HideTerminalControlScreen()
	{
		if (!CustomMapsTerminal.hasInstance || !ModIOManager.IsLoggedIn())
		{
			return;
		}
		CustomMapsTerminal.RefreshDriverNickName();
		if (CustomMapsTerminal.localStatus.currentScreen != CustomMapsTerminal.ScreenType.TerminalControlPrompt)
		{
			return;
		}
		if (CustomMapsTerminal.localStatus.previousScreen > CustomMapsTerminal.ScreenType.TerminalControlPrompt)
		{
			CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.localStatus.previousScreen;
		}
		else if (CustomMapLoader.IsModLoaded(0L) || CustomMapManager.IsLoading(0L) || CustomMapManager.GetRoomMapId() != ModId.Null)
		{
			CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.ModDetails;
		}
		else
		{
			CustomMapsTerminal.localStatus.currentScreen = CustomMapsTerminal.ScreenType.AvailableMods;
		}
		CustomMapsTerminal.instance.UpdateScreenToMatchStatus(false);
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x0004C6BF File Offset: 0x0004A8BF
	public static void RequestDriverNickNameRefresh()
	{
		if (!CustomMapsTerminal.hasInstance)
		{
			return;
		}
		if (!CustomMapsTerminal.IsDriver)
		{
			return;
		}
		CustomMapsTerminal.RefreshDriverNickName();
		CustomMapsTerminal.instance.mapTerminalNetworkObject.RefreshDriverNickName();
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x0011B91C File Offset: 0x00119B1C
	public static void RefreshDriverNickName()
	{
		if (!CustomMapsTerminal.hasInstance)
		{
			return;
		}
		ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
		bool flag = (customNicknamePermissionStatus.Item1 || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
		CustomMapsTerminal.instance.terminalControllerLabelText.gameObject.SetActive(true);
		if (NetworkSystem.Instance.InRoom)
		{
			NetPlayer netPlayerByID = NetworkSystem.Instance.GetNetPlayerByID(CustomMapsTerminal.localDriverID);
			CustomMapsTerminal.instance.terminalControllerText.text = netPlayerByID.DefaultName;
			if (CustomMapsTerminal.useNametags && flag)
			{
				RigContainer rigContainer;
				if (netPlayerByID.IsLocal)
				{
					CustomMapsTerminal.instance.terminalControllerText.text = netPlayerByID.NickName;
				}
				else if (VRRigCache.Instance.TryGetVrrig(netPlayerByID, out rigContainer))
				{
					CustomMapsTerminal.instance.terminalControllerText.text = rigContainer.Rig.playerNameVisible;
				}
			}
		}
		else
		{
			CustomMapsTerminal.instance.terminalControllerText.text = ((CustomMapsTerminal.useNametags && flag) ? NetworkSystem.Instance.LocalPlayer.NickName : NetworkSystem.Instance.LocalPlayer.DefaultName);
		}
		CustomMapsTerminal.instance.terminalControllerText.gameObject.SetActive(true);
		CustomMapsTerminal.instance.modListScreen.RefreshDriverNickname(CustomMapsTerminal.instance.terminalControllerText.text);
	}

	// Token: 0x06002A2B RID: 10795 RVA: 0x0004C6E5 File Offset: 0x0004A8E5
	private void OnReturnedToSinglePlayer()
	{
		if (CustomMapsTerminal.localDriverID != CustomMapsTerminal.cachedLocalPlayerID)
		{
			CustomMapsTerminal.ResetTerminalControl();
		}
		else
		{
			CustomMapsTerminal.localDriverID = CustomMapsTerminal.LocalPlayerID;
		}
		CustomMapsTerminal.cachedLocalPlayerID = -1;
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x0004C70A File Offset: 0x0004A90A
	private void OnJoinedRoom()
	{
		CustomMapsTerminal.cachedLocalPlayerID = CustomMapsTerminal.LocalPlayerID;
		CustomMapsTerminal.ResetTerminalControl();
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x0004C71B File Offset: 0x0004A91B
	public static bool IsLocked()
	{
		return CustomMapsTerminal.localDriverID != -2;
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x0004C729 File Offset: 0x0004A929
	public static int GetDriverID()
	{
		return CustomMapsTerminal.localDriverID;
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x0004C730 File Offset: 0x0004A930
	public static string GetDriverNickname()
	{
		if (!CustomMapsTerminal.hasInstance)
		{
			return "";
		}
		return CustomMapsTerminal.instance.terminalControllerText.text;
	}

	// Token: 0x04002F80 RID: 12160
	[SerializeField]
	private CustomMapsAccessScreen accessScreen;

	// Token: 0x04002F81 RID: 12161
	[SerializeField]
	private CustomMapsListScreen modListScreen;

	// Token: 0x04002F82 RID: 12162
	[SerializeField]
	private CustomMapsDetailsScreen modDetailsScreen;

	// Token: 0x04002F83 RID: 12163
	[SerializeField]
	private VirtualStumpSerializer mapTerminalNetworkObject;

	// Token: 0x04002F84 RID: 12164
	[SerializeField]
	private CustomMapsTerminalControlButton terminalControlButton;

	// Token: 0x04002F85 RID: 12165
	[SerializeField]
	private TMP_Text terminalControllerLabelText;

	// Token: 0x04002F86 RID: 12166
	[SerializeField]
	private TMP_Text terminalControllerText;

	// Token: 0x04002F87 RID: 12167
	[SerializeField]
	private List<CustomMapsTerminalTagButton> tagButtons = new List<CustomMapsTerminalTagButton>();

	// Token: 0x04002F88 RID: 12168
	[SerializeField]
	private CustomMapsTerminalToggleButton subscribedOnlyButton;

	// Token: 0x04002F89 RID: 12169
	public const int NO_DRIVER_ID = -2;

	// Token: 0x04002F8A RID: 12170
	private const short NUM_OF_TAGS = 8;

	// Token: 0x04002F8B RID: 12171
	private static CustomMapsTerminal instance;

	// Token: 0x04002F8C RID: 12172
	private static bool hasInstance;

	// Token: 0x04002F8D RID: 12173
	private static CustomMapsTerminal.TerminalStatus localStatus = new CustomMapsTerminal.TerminalStatus();

	// Token: 0x04002F8E RID: 12174
	private static CustomMapsTerminal.TerminalStatus cachedNetStatus = new CustomMapsTerminal.TerminalStatus();

	// Token: 0x04002F8F RID: 12175
	private static int localDriverID = -1;

	// Token: 0x04002F90 RID: 12176
	private static int cachedLocalPlayerID = -1;

	// Token: 0x04002F91 RID: 12177
	private static bool useNametags;

	// Token: 0x04002F92 RID: 12178
	private static short localTagStatus = 0;

	// Token: 0x0200069E RID: 1694
	public enum ScreenType
	{
		// Token: 0x04002F94 RID: 12180
		Invalid = -1,
		// Token: 0x04002F95 RID: 12181
		Access,
		// Token: 0x04002F96 RID: 12182
		TerminalControlPrompt,
		// Token: 0x04002F97 RID: 12183
		AvailableMods,
		// Token: 0x04002F98 RID: 12184
		SubscribedMods,
		// Token: 0x04002F99 RID: 12185
		ModDetails
	}

	// Token: 0x0200069F RID: 1695
	public class TerminalStatus
	{
		// Token: 0x06002A32 RID: 10802 RVA: 0x0011BA64 File Offset: 0x00119C64
		public long[] PackData(bool packModList)
		{
			long[] array;
			if (packModList && !this.modList.IsNullOrEmpty<long>())
			{
				array = new long[3 + this.modList.Length];
				for (int i = 3; i < array.Length; i++)
				{
					array[i] = this.modList[i - 3];
				}
			}
			else
			{
				array = new long[3];
			}
			array[0] = (long)this.currentScreen;
			array[0] += (long)((long)this.previousScreen << 4);
			array[0] += (long)((long)(this.modIndex + 1) << 8);
			array[0] += (long)((long)(this.numModPages + 1) << 16);
			array[0] += (long)(this.pageIndex + 1) << 32;
			array[1] = this.modDetailsID;
			array[2] = (long)this.tagFlags;
			array[2] += (long)this.sortType << 32;
			return array;
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x0011BB40 File Offset: 0x00119D40
		public void UnpackData(long[] data)
		{
			if (data.Length < 3 || data.Length > 3 + this.modsPerPage)
			{
				return;
			}
			int num = (int)(data[0] & 15L);
			this.currentScreen = (CustomMapsTerminal.ScreenType)((num >= -1 && num <= 4) ? num : -1);
			num = (int)(data[0] >> 4 & 15L);
			this.previousScreen = (CustomMapsTerminal.ScreenType)((num >= -1 && num <= 4) ? num : -1);
			this.modIndex = (int)(data[0] >> 8 & 255L);
			this.modIndex = Mathf.Clamp(this.modIndex - 1, -1, this.modsPerPage);
			this.numModPages = (int)(data[0] >> 16 & 65535L);
			this.numModPages = Mathf.Clamp(this.numModPages - 1, -1, 65535);
			this.pageIndex = (int)(data[0] >> 32);
			this.pageIndex = Mathf.Max(this.pageIndex - 1, -1);
			this.modDetailsID = ((data[1] > 0L) ? data[1] : 0L);
			this.tagFlags = (short)Mathf.Clamp((float)(data[2] & 255L), 0f, 255f);
			num = (int)(data[2] >> 32);
			this.sortType = (SortModsBy)((num >= 0 && num <= 6) ? num : 3);
			if (data.Length <= 3)
			{
				return;
			}
			this.modList = new long[data.Length - 3];
			for (int i = 0; i < this.modList.Length; i++)
			{
				this.modList[i] = data[i + 3];
			}
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x0011BC9C File Offset: 0x00119E9C
		public void Copy(CustomMapsTerminal.TerminalStatus other)
		{
			this.currentScreen = other.currentScreen;
			this.previousScreen = other.previousScreen;
			this.driverID = other.driverID;
			this.modIndex = other.modIndex;
			this.numModPages = other.numModPages;
			this.pageIndex = other.pageIndex;
			this.modDetailsID = other.modDetailsID;
			this.modList = other.modList;
			this.tagFlags = other.tagFlags;
			this.sortType = other.sortType;
		}

		// Token: 0x04002F9A RID: 12186
		public CustomMapsTerminal.ScreenType currentScreen = CustomMapsTerminal.ScreenType.Invalid;

		// Token: 0x04002F9B RID: 12187
		public CustomMapsTerminal.ScreenType previousScreen = CustomMapsTerminal.ScreenType.Invalid;

		// Token: 0x04002F9C RID: 12188
		public int driverID;

		// Token: 0x04002F9D RID: 12189
		public int modIndex;

		// Token: 0x04002F9E RID: 12190
		public int pageIndex;

		// Token: 0x04002F9F RID: 12191
		public long modDetailsID;

		// Token: 0x04002FA0 RID: 12192
		public long[] modList;

		// Token: 0x04002FA1 RID: 12193
		public int numModPages = -1;

		// Token: 0x04002FA2 RID: 12194
		public short tagFlags = 128;

		// Token: 0x04002FA3 RID: 12195
		public SortModsBy sortType = SortModsBy.Popular;

		// Token: 0x04002FA4 RID: 12196
		private const int MINIMUM_ARRAY_LENGTH = 3;

		// Token: 0x04002FA5 RID: 12197
		public int modsPerPage;
	}
}
