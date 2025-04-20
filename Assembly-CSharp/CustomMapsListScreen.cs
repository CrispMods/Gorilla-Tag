using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GorillaNetworking;
using ModIO;
using PlayFab;
using TMPro;
using UnityEngine;

// Token: 0x0200069A RID: 1690
public class CustomMapsListScreen : CustomMapsTerminalScreen
{
	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x060029E5 RID: 10725 RVA: 0x0004C4C3 File Offset: 0x0004A6C3
	public int CurrentModPage
	{
		get
		{
			return this.currentModPage;
		}
	}

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x060029E6 RID: 10726 RVA: 0x0004C4CB File Offset: 0x0004A6CB
	public int SelectedModIndex
	{
		get
		{
			return this.selectedModIndex;
		}
	}

	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x060029E7 RID: 10727 RVA: 0x0004C4D3 File Offset: 0x0004A6D3
	public int ModsPerPage
	{
		get
		{
			return this.modsPerPage;
		}
	}

	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x060029E8 RID: 10728 RVA: 0x0004C4DB File Offset: 0x0004A6DB
	// (set) Token: 0x060029E9 RID: 10729 RVA: 0x00119420 File Offset: 0x00117620
	public SortModsBy SortType
	{
		get
		{
			return this.sortType;
		}
		set
		{
			if (this.sortType != value)
			{
				this.currentAvailableModsRequestPage = 0;
			}
			this.sortType = value;
			switch (this.sortType)
			{
			case SortModsBy.Name:
				this.isAscendingOrder = true;
				return;
			case SortModsBy.Price:
				break;
			case SortModsBy.Rating:
				this.isAscendingOrder = true;
				return;
			case SortModsBy.Popular:
				this.isAscendingOrder = false;
				return;
			case SortModsBy.Downloads:
				this.isAscendingOrder = true;
				return;
			case SortModsBy.Subscribers:
				this.isAscendingOrder = true;
				return;
			case SortModsBy.DateSubmitted:
				this.isAscendingOrder = true;
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x00030607 File Offset: 0x0002E807
	public override void Initialize()
	{
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x001194A0 File Offset: 0x001176A0
	public override void Show()
	{
		base.Show();
		if (this.featuredMods.IsNullOrEmpty<ModProfile>())
		{
			this.RetrieveFeaturedMods();
		}
		if (this.availableMods.IsNullOrEmpty<ModProfile>())
		{
			this.RetrieveAvailableMods();
		}
		if (this.subscribedMods.IsNullOrEmpty<SubscribedMod>())
		{
			this.RetrieveSubscribedMods();
		}
		this.RefreshScreenState();
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x001194F4 File Offset: 0x001176F4
	protected override void PressButton(CustomMapsTerminalButton.ModIOKeyboardBindings buttonPressed)
	{
		if (!CustomMapsTerminal.IsDriver)
		{
			return;
		}
		if (buttonPressed == CustomMapsTerminalButton.ModIOKeyboardBindings.goback)
		{
			return;
		}
		if (this.loadingText.gameObject.activeSelf)
		{
			return;
		}
		if (this.CheckTags(buttonPressed))
		{
			this.Refresh(null);
			return;
		}
		if (buttonPressed == CustomMapsTerminalButton.ModIOKeyboardBindings.option3)
		{
			ModIOManager.Refresh(delegate(bool result)
			{
				if (result)
				{
					this.Refresh(null);
				}
			}, false);
			return;
		}
		if (buttonPressed == CustomMapsTerminalButton.ModIOKeyboardBindings.up)
		{
			this.selectedModIndex--;
			if ((this.IsOnFirstPage() && this.selectedModIndex < 0) || (!this.IsOnFirstPage() && this.selectedModIndex < -1))
			{
				this.selectedModIndex = (this.IsOnLastPage() ? (this.displayedModProfiles.Count - 1) : this.displayedModProfiles.Count);
			}
			this.UpdateModListSelection();
			CustomMapsTerminal.SendTerminalStatus(false, false);
			return;
		}
		if (buttonPressed == CustomMapsTerminalButton.ModIOKeyboardBindings.down)
		{
			this.selectedModIndex++;
			if ((this.IsOnLastPage() && this.selectedModIndex >= this.displayedModProfiles.Count) || (!this.IsOnLastPage() && this.selectedModIndex > this.displayedModProfiles.Count))
			{
				this.selectedModIndex = (this.IsOnFirstPage() ? 0 : -1);
			}
			this.UpdateModListSelection();
			CustomMapsTerminal.SendTerminalStatus(false, false);
			return;
		}
		if (buttonPressed == CustomMapsTerminalButton.ModIOKeyboardBindings.enter)
		{
			if (!this.IsOnFirstPage() && this.selectedModIndex == -1)
			{
				this.currentModPage--;
				this.selectedModIndex = 0;
				CustomMapsTerminal.SendTerminalStatus(false, false);
				this.RefreshScreenState();
				return;
			}
			if (!this.IsOnLastPage() && this.selectedModIndex == this.displayedModProfiles.Count)
			{
				this.currentModPage++;
				this.selectedModIndex = 0;
				CustomMapsTerminal.SendTerminalStatus(false, false);
				this.RefreshScreenState();
				return;
			}
			if (this.selectedModIndex >= 0 && this.selectedModIndex < this.displayedModProfiles.Count)
			{
				CustomMapsTerminal.ShowDetailsScreen(this.displayedModProfiles[this.selectedModIndex]);
				return;
			}
		}
		else
		{
			if (buttonPressed == CustomMapsTerminalButton.ModIOKeyboardBindings.sub)
			{
				this.SwapListDisplay();
				return;
			}
			if (buttonPressed == CustomMapsTerminalButton.ModIOKeyboardBindings.sort)
			{
				this.SetSortType();
				this.Refresh(null);
			}
		}
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x0004C4E3 File Offset: 0x0004A6E3
	public void ClearTags(bool clearModLists = false)
	{
		this.searchTags.Clear();
		if (clearModLists)
		{
			this.featuredMods.Clear();
			this.availableMods.Clear();
		}
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x0004C509 File Offset: 0x0004A709
	public void UpdateTagsFromDriver(List<string> tags)
	{
		this.currentAvailableModsRequestPage = 0;
		this.searchTags.Clear();
		if (tags.IsNullOrEmpty<string>())
		{
			return;
		}
		this.searchTags.AddRange(tags);
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x001196EC File Offset: 0x001178EC
	private bool CheckTags(CustomMapsTerminalButton.ModIOKeyboardBindings buttonPressed)
	{
		bool result = false;
		short num = (short)buttonPressed;
		if (num > 0 && num < 10)
		{
			result = true;
			string item;
			if (CustomMapsTerminal.SetTagButtonStatus(num, out item))
			{
				if (!this.searchTags.Contains(item))
				{
					this.searchTags.Add(item);
				}
			}
			else if (this.searchTags.Contains(item))
			{
				this.searchTags.Remove(item);
			}
		}
		return result;
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x0011974C File Offset: 0x0011794C
	private void SetSortType()
	{
		this.currentAvailableModsRequestPage = 0;
		this.sortTypeIndex++;
		if (this.sortTypeIndex >= 6)
		{
			this.sortTypeIndex = 0;
		}
		switch (this.sortTypeIndex)
		{
		case 0:
			this.sortType = SortModsBy.Popular;
			this.isAscendingOrder = false;
			return;
		case 1:
			this.sortType = SortModsBy.Name;
			this.isAscendingOrder = true;
			return;
		case 2:
			this.sortType = SortModsBy.Rating;
			this.isAscendingOrder = true;
			return;
		case 3:
			this.sortType = SortModsBy.Downloads;
			this.isAscendingOrder = true;
			return;
		case 4:
			this.sortType = SortModsBy.Subscribers;
			this.isAscendingOrder = true;
			return;
		case 5:
			this.sortType = SortModsBy.DateSubmitted;
			this.isAscendingOrder = true;
			return;
		default:
			this.sortTypeIndex = 0;
			this.sortType = SortModsBy.Popular;
			this.isAscendingOrder = false;
			return;
		}
	}

	// Token: 0x060029F1 RID: 10737 RVA: 0x00119814 File Offset: 0x00117A14
	private void SwapListDisplay()
	{
		if (this.currentState == CustomMapsListScreen.ListScreenState.AvailableMods)
		{
			this.currentState = CustomMapsListScreen.ListScreenState.SubscribedMods;
		}
		else if (this.currentState == CustomMapsListScreen.ListScreenState.SubscribedMods)
		{
			this.currentState = CustomMapsListScreen.ListScreenState.AvailableMods;
		}
		this.selectedModIndex = 0;
		this.currentModPage = 0;
		CustomMapsTerminal.UpdateListScreenState(this.currentState);
		CustomMapsTerminal.SendTerminalStatus(this.currentState == CustomMapsListScreen.ListScreenState.SubscribedMods, false);
		this.RefreshScreenState();
	}

	// Token: 0x060029F2 RID: 10738 RVA: 0x00119870 File Offset: 0x00117A70
	public void Refresh(long[] customModListIds = null)
	{
		if (this.loadingAvailableMods || this.loadingFeaturedMods || this.loadingCustomModList)
		{
			return;
		}
		this.currentModPage = 0;
		this.selectedModIndex = 0;
		CustomMapsTerminal.SendTerminalStatus(false, true);
		this.featuredMods.Clear();
		this.availableMods.Clear();
		this.filteredAvailableMods.Clear();
		this.currentAvailableModsRequestPage = 0;
		this.errorLoadingAvailableMods = false;
		this.totalAvailableMods = 0;
		this.subscribedMods = null;
		this.filteredSubscribedMods.Clear();
		this.totalSubscribedMods = 0;
		if (customModListIds != null && customModListIds.Length != 0)
		{
			this.customModListModIds.Clear();
			this.customModListModIds.AddRange(customModListIds);
		}
		this.customModList.Clear();
		this.RetrieveFeaturedMods();
		this.RetrieveAvailableMods();
		this.RetrieveSubscribedMods();
		this.RetrieveCustomModList();
	}

	// Token: 0x060029F3 RID: 10739 RVA: 0x0011993C File Offset: 0x00117B3C
	private void RetrieveFeaturedMods()
	{
		if (this.loadingFeaturedMods || this.featuredMods.Count > 0)
		{
			return;
		}
		this.loadingFeaturedMods = true;
		PlayFabTitleDataCache.Instance.GetTitleData(this.featuredModsPlayFabKey, new Action<string>(this.OnGetFeaturedModsTitleData), delegate(PlayFabError error)
		{
			this.loadingFeaturedMods = false;
			this.RefreshScreenState();
		});
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x00119990 File Offset: 0x00117B90
	private void OnGetFeaturedModsTitleData(string data)
	{
		CustomMapsListScreen.<OnGetFeaturedModsTitleData>d__76 <OnGetFeaturedModsTitleData>d__;
		<OnGetFeaturedModsTitleData>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnGetFeaturedModsTitleData>d__.<>4__this = this;
		<OnGetFeaturedModsTitleData>d__.data = data;
		<OnGetFeaturedModsTitleData>d__.<>1__state = -1;
		<OnGetFeaturedModsTitleData>d__.<>t__builder.Start<CustomMapsListScreen.<OnGetFeaturedModsTitleData>d__76>(ref <OnGetFeaturedModsTitleData>d__);
	}

	// Token: 0x060029F5 RID: 10741 RVA: 0x001199D0 File Offset: 0x00117BD0
	private void RetrieveAvailableMods()
	{
		if (this.loadingAvailableMods)
		{
			return;
		}
		if (this.retrieveRetryCount > 10)
		{
			this.retrieveRetryCount = 0;
			return;
		}
		this.retrieveRetryCount++;
		this.loadingAvailableMods = true;
		int num = this.currentAvailableModsRequestPage;
		this.currentAvailableModsRequestPage = num + 1;
		SearchFilter searchFilter = new SearchFilter(num, this.numModsPerRequest);
		searchFilter.SetSortBy(this.sortType);
		if (!this.searchTags.IsNullOrEmpty<string>())
		{
			searchFilter.AddTags(this.searchTags);
		}
		searchFilter.SetToAscending(this.isAscendingOrder);
		ModIOUnity.GetMods(searchFilter, new Action<ResultAnd<ModPage>>(this.OnAvailableModsRetrieved));
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x00119A70 File Offset: 0x00117C70
	private void OnAvailableModsRetrieved(ResultAnd<ModPage> result)
	{
		if (result.result.Succeeded())
		{
			this.totalAvailableMods = (int)result.value.totalSearchResultsFound;
			if (this.totalAvailableMods == 0 && this.retrieveRetryCount < 10)
			{
				this.loadingAvailableMods = false;
				this.currentAvailableModsRequestPage = Mathf.Max(0, this.currentAvailableModsRequestPage - 1);
				this.RetrieveAvailableMods();
				return;
			}
			this.availableMods.AddRange(result.value.modProfiles);
			this.FilterAvailableMods();
		}
		else
		{
			this.errorLoadingAvailableMods = true;
		}
		this.retrieveRetryCount = 0;
		this.loadingAvailableMods = false;
		this.RefreshScreenState();
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x00119B0C File Offset: 0x00117D0C
	private void FilterAvailableMods()
	{
		if (this.availableMods.IsNullOrEmpty<ModProfile>())
		{
			return;
		}
		this.filteredAvailableMods.Clear();
		foreach (ModProfile modProfile in this.availableMods)
		{
			if (!(modProfile.id == ModIOManager.GetNewMapsModId()) && (this.featuredModIds.IsNullOrEmpty<long>() || !this.featuredModIds.Contains(modProfile.id.id)))
			{
				this.filteredAvailableMods.Add(modProfile);
			}
		}
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x0004C532 File Offset: 0x0004A732
	private void RetrieveSubscribedMods()
	{
		this.subscribedMods = ModIOManager.GetSubscribedMods();
		this.FilterSubscribedMods();
		this.totalSubscribedMods = this.filteredSubscribedMods.Count;
		this.RefreshScreenState();
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x00119BB4 File Offset: 0x00117DB4
	private void FilterSubscribedMods()
	{
		if (this.subscribedMods.IsNullOrEmpty<SubscribedMod>())
		{
			return;
		}
		this.filteredSubscribedMods.Clear();
		foreach (SubscribedMod subscribedMod in this.subscribedMods)
		{
			if (!(subscribedMod.modProfile.id == ModIOManager.GetNewMapsModId()))
			{
				this.filteredSubscribedMods.Add(subscribedMod);
			}
		}
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x00119C1C File Offset: 0x00117E1C
	public void ShowCustomModList(long[] modIds)
	{
		if (modIds == null || modIds.Length == 0)
		{
			return;
		}
		if (this.customModListModIds.Count > 0 && this.customModListModIds.Count == modIds.Length)
		{
			bool flag = true;
			foreach (long value in this.customModListModIds)
			{
				if (!modIds.Contains(value))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.currentState = CustomMapsListScreen.ListScreenState.CustomModList;
				this.currentModPage = 0;
				this.selectedModIndex = 0;
				this.RefreshScreenState();
				return;
			}
		}
		this.customModListModIds.Clear();
		this.customModList.Clear();
		this.customModListModIds.AddRange(modIds);
		if (this.loadingCustomModList)
		{
			this.restartCustomModListRetrieval = true;
		}
		this.currentState = CustomMapsListScreen.ListScreenState.CustomModList;
		this.loadingCustomModList = true;
		this.RefreshScreenState();
		this.RetrieveCustomModList();
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x00119D08 File Offset: 0x00117F08
	private void RetrieveCustomModList()
	{
		if (this.restartCustomModListRetrieval || this.customModListModIds.Count == 0)
		{
			return;
		}
		ModIOManager.GetModProfile(new ModId(this.customModListModIds[this.customModList.Count]), new Action<ModIORequestResultAnd<ModProfile>>(this.OnModProfileReceived));
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x00119D58 File Offset: 0x00117F58
	private void OnModProfileReceived(ModIORequestResultAnd<ModProfile> requestResult)
	{
		if (this.restartCustomModListRetrieval)
		{
			this.restartCustomModListRetrieval = false;
			this.RetrieveCustomModList();
			return;
		}
		if (!requestResult.result.success)
		{
			this.loadingCustomModList = false;
			this.errorLoadingCustomModList = true;
			this.RefreshScreenState();
			return;
		}
		this.customModList.Add(requestResult.data);
		if (this.customModList.Count < this.customModListModIds.Count)
		{
			ModIOManager.GetModProfile(new ModId(this.customModListModIds[this.customModList.Count]), new Action<ModIORequestResultAnd<ModProfile>>(this.OnModProfileReceived));
			return;
		}
		this.loadingCustomModList = false;
		this.RefreshScreenState();
	}

	// Token: 0x060029FD RID: 10749 RVA: 0x00119E00 File Offset: 0x00118000
	public bool DoesModListMatchDisplay(long[] modIds)
	{
		if (this.displayedModProfiles.IsNullOrEmpty<ModProfile>() || this.displayedModProfiles.Count != modIds.Length)
		{
			return false;
		}
		for (int i = 0; i < this.displayedModProfiles.Count; i++)
		{
			if (modIds[i] != this.displayedModProfiles[i].id.id)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060029FE RID: 10750 RVA: 0x00119E60 File Offset: 0x00118060
	public ModProfile GetProfile()
	{
		if (this.displayedModProfiles == null)
		{
			return default(ModProfile);
		}
		if (this.selectedModIndex < 0 || this.selectedModIndex >= this.displayedModProfiles.Count)
		{
			return default(ModProfile);
		}
		return this.displayedModProfiles[this.selectedModIndex];
	}

	// Token: 0x060029FF RID: 10751 RVA: 0x00119EB8 File Offset: 0x001180B8
	public void GetModList(out long[] modList)
	{
		if (this.displayedModProfiles == null)
		{
			modList = Array.Empty<long>();
			return;
		}
		modList = new long[this.displayedModProfiles.Count];
		for (int i = 0; i < this.displayedModProfiles.Count; i++)
		{
			modList[i] = this.displayedModProfiles[i].id.id;
		}
	}

	// Token: 0x06002A00 RID: 10752 RVA: 0x00119F18 File Offset: 0x00118118
	private void RefreshScreenState()
	{
		this.displayedModProfiles.Clear();
		this.modListText.text = "";
		this.modListText.gameObject.SetActive(false);
		this.modPageLabel.gameObject.SetActive(false);
		this.modPageText.gameObject.SetActive(false);
		this.mapScreenshotImage.gameObject.SetActive(false);
		this.createdByText.gameObject.SetActive(false);
		this.tagText.gameObject.SetActive(false);
		this.loadingText.gameObject.SetActive(true);
		for (int i = 0; i < this.buttonsToShow.Length; i++)
		{
			this.buttonsToShow[i].SetActive(true);
		}
		switch (this.currentState)
		{
		case CustomMapsListScreen.ListScreenState.AvailableMods:
		{
			this.titleText.text = this.browseModsTitle + string.Format(" (SORTED BY {0})", this.sortType);
			for (int j = 0; j < this.buttonsToHideForAvailableMaps.Length; j++)
			{
				this.buttonsToHideForAvailableMaps[j].SetActive(false);
			}
			if (this.loadingFeaturedMods || this.loadingAvailableMods)
			{
				return;
			}
			if (this.errorLoadingAvailableMods)
			{
				this.modListText.text = this.failedToRetrieveModsString;
				this.loadingText.gameObject.SetActive(false);
				this.modListText.gameObject.SetActive(true);
				return;
			}
			this.UpdatePageCount(this.totalAvailableMods);
			int num = 0;
			int num2 = this.modsPerPage - 1;
			if (this.IsOnFirstPage())
			{
				this.displayedModProfiles.AddRange(this.featuredMods);
				num2 -= this.totalFeaturedMods;
			}
			else
			{
				num = this.currentModPage * this.modsPerPage - this.totalFeaturedMods;
				num2 = num + this.modsPerPage - 1;
			}
			if (this.filteredAvailableMods.Count <= num2 && this.totalAvailableMods > this.availableMods.Count)
			{
				this.displayedModProfiles.Clear();
				this.RetrieveAvailableMods();
				return;
			}
			int num3 = num;
			while (num3 <= num2 && this.filteredAvailableMods.Count > num3)
			{
				this.displayedModProfiles.Add(this.filteredAvailableMods[num3]);
				num3++;
			}
			this.UpdateModListSelection();
			this.loadingText.gameObject.SetActive(false);
			this.modListText.gameObject.SetActive(true);
			return;
		}
		case CustomMapsListScreen.ListScreenState.SubscribedMods:
		{
			this.titleText.text = this.subscribedOnlyTitle;
			for (int k = 0; k < this.buttonsToHideForSubscribedMaps.Length; k++)
			{
				this.buttonsToHideForSubscribedMaps[k].SetActive(false);
			}
			this.UpdatePageCount(this.totalSubscribedMods);
			int num4 = this.currentModPage * this.modsPerPage;
			int num5 = num4;
			while (num5 < num4 + this.modsPerPage && this.filteredSubscribedMods.Count > num5)
			{
				this.displayedModProfiles.Add(this.filteredSubscribedMods[num5].modProfile);
				num5++;
			}
			this.UpdateModListSelection();
			this.loadingText.gameObject.SetActive(false);
			this.modListText.gameObject.SetActive(true);
			CustomMapsTerminal.SendTerminalStatus(true, false);
			return;
		}
		case CustomMapsListScreen.ListScreenState.CustomModList:
			this.titleText.text = CustomMapsTerminal.GetDriverNickname() + "'s " + this.subscribedOnlyTitle;
			for (int l = 0; l < this.buttonsToHideForSubscribedMaps.Length; l++)
			{
				this.buttonsToHideForSubscribedMaps[l].SetActive(false);
			}
			if (this.loadingCustomModList)
			{
				return;
			}
			if (this.errorLoadingCustomModList)
			{
				this.modPageLabel.gameObject.SetActive(false);
				this.modPageText.gameObject.SetActive(false);
				if (CustomMapsTerminal.IsDriver)
				{
					this.currentModPage = -1;
				}
				this.modListText.text = this.failedToRetrieveModsString;
				this.loadingText.gameObject.SetActive(false);
				this.modListText.gameObject.SetActive(true);
				return;
			}
			this.UpdatePageCount(this.customModList.Count);
			this.displayedModProfiles.AddRange(this.customModList);
			this.UpdateModListSelection();
			this.loadingText.gameObject.SetActive(false);
			this.modListText.gameObject.SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002A01 RID: 10753 RVA: 0x0011A34C File Offset: 0x0011854C
	public void UpdateModListSelection()
	{
		if (this.displayedModProfiles.IsNullOrEmpty<ModProfile>())
		{
			return;
		}
		if (this.selectedModIndex < -1 || this.selectedModIndex > this.displayedModProfiles.Count)
		{
			this.mapScreenshotImage.gameObject.SetActive(false);
			this.createdByText.gameObject.SetActive(false);
			this.tagText.gameObject.SetActive(false);
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (this.selectedModIndex == -1 || this.selectedModIndex == this.displayedModProfiles.Count)
		{
			this.mapScreenshotImage.gameObject.SetActive(false);
			this.createdByText.gameObject.SetActive(false);
			this.tagText.gameObject.SetActive(false);
		}
		else
		{
			this.DownloadThumbnail(this.displayedModProfiles[this.selectedModIndex].logoImage320x180);
			this.createdByText.gameObject.SetActive(true);
			this.tagText.gameObject.SetActive(true);
			this.createdByText.text = this.creatorString + this.displayedModProfiles[this.selectedModIndex].creator.username;
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int i = 0; i < this.displayedModProfiles[this.selectedModIndex].tags.Length; i++)
			{
				stringBuilder2.Append((i == 0) ? (this.displayedModProfiles[this.selectedModIndex].tags[i] ?? "") : (", " + this.displayedModProfiles[this.selectedModIndex].tags[i]));
			}
			this.tagText.text = this.tagString + stringBuilder2.ToString();
		}
		if (!this.IsOnFirstPage())
		{
			stringBuilder.Append((this.selectedModIndex == -1) ? ("> " + this.prevPageString + "\n") : ("  " + this.prevPageString + "\n"));
		}
		long num = (long)(this.currentModPage * this.modsPerPage + 1);
		bool flag = false;
		if (this.currentState == CustomMapsListScreen.ListScreenState.AvailableMods)
		{
			if (this.IsOnFirstPage())
			{
				for (int j = 0; j < this.displayedModProfiles.Count; j++)
				{
					if (j < this.totalFeaturedMods)
					{
						stringBuilder.Append((j == this.selectedModIndex) ? this.TruncateModName("> FEATURED: " + this.displayedModProfiles[j].name) : this.TruncateModName("  FEATURED: " + this.displayedModProfiles[j].name));
					}
					else
					{
						StringBuilder stringBuilder3 = stringBuilder;
						string value;
						if (j != this.selectedModIndex)
						{
							string format = "  {0}. {1}";
							long num2 = num;
							num = num2 + 1L;
							value = this.TruncateModName(string.Format(format, num2, this.displayedModProfiles[j].name));
						}
						else
						{
							string format2 = "> {0}. {1}";
							long num3 = num;
							num = num3 + 1L;
							value = this.TruncateModName(string.Format(format2, num3, this.displayedModProfiles[j].name));
						}
						stringBuilder3.Append(value);
					}
				}
				flag = true;
			}
			else
			{
				num -= (long)this.totalFeaturedMods;
			}
		}
		if (!flag)
		{
			for (int k = 0; k < this.displayedModProfiles.Count; k++)
			{
				stringBuilder.Append((k == this.selectedModIndex) ? this.TruncateModName(string.Format("> {0}. {1}", num + (long)k, this.displayedModProfiles[k].name)) : this.TruncateModName(string.Format("  {0}. {1}", num + (long)k, this.displayedModProfiles[k].name)));
			}
		}
		if (!this.IsOnLastPage())
		{
			stringBuilder.Append((this.selectedModIndex == this.displayedModProfiles.Count) ? ("> " + this.nextPageString + "\n") : ("  " + this.nextPageString + "\n"));
		}
		this.modListText.text = stringBuilder.ToString();
	}

	// Token: 0x06002A02 RID: 10754 RVA: 0x0004C55C File Offset: 0x0004A75C
	private string TruncateModName(string modname)
	{
		if (modname.Length <= this.maxModListItemLength)
		{
			return modname + "\n";
		}
		return modname.Substring(0, this.maxModListItemLength) + "\n";
	}

	// Token: 0x06002A03 RID: 10755 RVA: 0x0011A770 File Offset: 0x00118970
	private void DownloadThumbnail(DownloadReference thumbnail)
	{
		this.mapScreenshotImage.gameObject.SetActive(false);
		if (this.isDownloadingThumbnail)
		{
			this.newDownloadRequest = true;
			this.currentThumbnail = thumbnail;
			return;
		}
		this.isDownloadingThumbnail = true;
		ModIOUnity.DownloadTexture(thumbnail, new Action<ResultAnd<Texture2D>>(this.OnTextureDownloaded));
	}

	// Token: 0x06002A04 RID: 10756 RVA: 0x0011A7C0 File Offset: 0x001189C0
	private void OnTextureDownloaded(ResultAnd<Texture2D> resultAnd)
	{
		this.isDownloadingThumbnail = false;
		if (this.newDownloadRequest)
		{
			this.newDownloadRequest = false;
			this.mapScreenshotImage.gameObject.SetActive(false);
			ModIOUnity.DownloadTexture(this.currentThumbnail, new Action<ResultAnd<Texture2D>>(this.OnTextureDownloaded));
			return;
		}
		if (!resultAnd.result.Succeeded())
		{
			return;
		}
		Texture2D value = resultAnd.value;
		this.mapScreenshotImage.sprite = Sprite.Create(value, new Rect(0f, 0f, (float)value.width, (float)value.height), new Vector2(0.5f, 0.5f));
		this.mapScreenshotImage.gameObject.SetActive(true);
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x0011A870 File Offset: 0x00118A70
	private void UpdatePageCount(int totalMods)
	{
		this.totalModCount = totalMods;
		this.modPageText.gameObject.SetActive(false);
		this.modPageLabel.gameObject.SetActive(false);
		if (this.totalModCount == 0)
		{
			this.modListText.text = ((this.currentState == CustomMapsListScreen.ListScreenState.AvailableMods) ? this.noModsAvailableString : this.noSubscribedModsString);
			return;
		}
		int numPages = this.GetNumPages();
		if (numPages > 1)
		{
			this.modPageText.text = string.Format("{0} / {1}", this.currentModPage + 1, numPages);
			this.modPageText.gameObject.SetActive(true);
			this.modPageLabel.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002A06 RID: 10758 RVA: 0x0011A928 File Offset: 0x00118B28
	public int GetNumPages()
	{
		if (this.currentState == CustomMapsListScreen.ListScreenState.CustomModList)
		{
			return this.customModListPageCount;
		}
		int num = this.totalModCount % this.modsPerPage;
		int num2 = this.totalModCount / this.modsPerPage;
		if (num > 0)
		{
			num2++;
		}
		return num2;
	}

	// Token: 0x06002A07 RID: 10759 RVA: 0x0004C58F File Offset: 0x0004A78F
	private bool IsOnFirstPage()
	{
		return this.currentModPage == 0;
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x0011A968 File Offset: 0x00118B68
	private bool IsOnLastPage()
	{
		long num = (long)this.GetNumPages();
		return (long)(this.currentModPage + 1) == num;
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x0011A98C File Offset: 0x00118B8C
	public void UpdateFromTerminalStatus(CustomMapsTerminal.TerminalStatus localStatus)
	{
		switch (localStatus.currentScreen)
		{
		case CustomMapsTerminal.ScreenType.AvailableMods:
			this.currentState = CustomMapsListScreen.ListScreenState.AvailableMods;
			break;
		case CustomMapsTerminal.ScreenType.SubscribedMods:
			if (this.currentState == CustomMapsListScreen.ListScreenState.CustomModList && CustomMapsTerminal.IsDriver)
			{
				this.currentState = CustomMapsListScreen.ListScreenState.SubscribedMods;
				this.currentModPage = 0;
				this.selectedModIndex = 0;
				this.customModListPageCount = -1;
				return;
			}
			this.currentState = (CustomMapsTerminal.IsDriver ? CustomMapsListScreen.ListScreenState.SubscribedMods : CustomMapsListScreen.ListScreenState.CustomModList);
			break;
		}
		this.currentModPage = localStatus.pageIndex;
		this.selectedModIndex = localStatus.modIndex;
		this.customModListPageCount = localStatus.numModPages;
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x0004C59A File Offset: 0x0004A79A
	public void RefreshDriverNickname(string driverNickname)
	{
		if (this.currentState == CustomMapsListScreen.ListScreenState.CustomModList)
		{
			this.titleText.text = driverNickname + "'s " + this.subscribedOnlyTitle;
		}
	}

	// Token: 0x04002F3A RID: 12090
	[SerializeField]
	private TMP_Text loadingText;

	// Token: 0x04002F3B RID: 12091
	[SerializeField]
	private TMP_Text modListText;

	// Token: 0x04002F3C RID: 12092
	[SerializeField]
	private TMP_Text modPageLabel;

	// Token: 0x04002F3D RID: 12093
	[SerializeField]
	private TMP_Text modPageText;

	// Token: 0x04002F3E RID: 12094
	[SerializeField]
	private TMP_Text titleText;

	// Token: 0x04002F3F RID: 12095
	[SerializeField]
	private SpriteRenderer mapScreenshotImage;

	// Token: 0x04002F40 RID: 12096
	[SerializeField]
	private TMP_Text createdByText;

	// Token: 0x04002F41 RID: 12097
	[SerializeField]
	private TMP_Text tagText;

	// Token: 0x04002F42 RID: 12098
	[SerializeField]
	private GameObject[] buttonsToHideForAvailableMaps;

	// Token: 0x04002F43 RID: 12099
	[SerializeField]
	private GameObject[] buttonsToShow;

	// Token: 0x04002F44 RID: 12100
	[SerializeField]
	private GameObject[] buttonsToHideForSubscribedMaps;

	// Token: 0x04002F45 RID: 12101
	[SerializeField]
	private string browseModsTitle = "AVAILABLE MODS";

	// Token: 0x04002F46 RID: 12102
	[SerializeField]
	private string subscribedOnlyTitle = "SUBSCRIBED MODS";

	// Token: 0x04002F47 RID: 12103
	[SerializeField]
	private string nextPageString = "NEXT PAGE";

	// Token: 0x04002F48 RID: 12104
	[SerializeField]
	private string prevPageString = "PREVIOUS PAGE";

	// Token: 0x04002F49 RID: 12105
	[SerializeField]
	private string noModsAvailableString = "NO MODS AVAILABLE";

	// Token: 0x04002F4A RID: 12106
	[SerializeField]
	private string noSubscribedModsString = "NOT SUBSCRIBED TO ANY MODS";

	// Token: 0x04002F4B RID: 12107
	[SerializeField]
	private string failedToRetrieveModsString = "FAILED TO RETRIEVE MODS FROM MOD.IO \nPRESS THE 'REFRESH' BUTTON TO RETRY";

	// Token: 0x04002F4C RID: 12108
	[SerializeField]
	private string creatorString = "CREATED BY:\n";

	// Token: 0x04002F4D RID: 12109
	[SerializeField]
	private string tagString = "MAP TAGS:\n";

	// Token: 0x04002F4E RID: 12110
	[SerializeField]
	private int modsPerPage = 10;

	// Token: 0x04002F4F RID: 12111
	[SerializeField]
	private int numModsPerRequest = 50;

	// Token: 0x04002F50 RID: 12112
	[SerializeField]
	private int maxModListItemLength = 25;

	// Token: 0x04002F51 RID: 12113
	[SerializeField]
	private string featuredModsPlayFabKey = "VStumpFeaturedMaps";

	// Token: 0x04002F52 RID: 12114
	private bool loadingFeaturedMods;

	// Token: 0x04002F53 RID: 12115
	private int totalFeaturedMods;

	// Token: 0x04002F54 RID: 12116
	private List<long> featuredModIds = new List<long>();

	// Token: 0x04002F55 RID: 12117
	private List<ModProfile> featuredMods = new List<ModProfile>();

	// Token: 0x04002F56 RID: 12118
	private int currentAvailableModsRequestPage;

	// Token: 0x04002F57 RID: 12119
	private bool loadingAvailableMods;

	// Token: 0x04002F58 RID: 12120
	private int totalAvailableMods;

	// Token: 0x04002F59 RID: 12121
	private bool errorLoadingAvailableMods;

	// Token: 0x04002F5A RID: 12122
	private List<ModProfile> availableMods = new List<ModProfile>();

	// Token: 0x04002F5B RID: 12123
	private List<ModProfile> filteredAvailableMods = new List<ModProfile>();

	// Token: 0x04002F5C RID: 12124
	private int totalSubscribedMods;

	// Token: 0x04002F5D RID: 12125
	private SubscribedMod[] subscribedMods;

	// Token: 0x04002F5E RID: 12126
	private List<SubscribedMod> filteredSubscribedMods = new List<SubscribedMod>();

	// Token: 0x04002F5F RID: 12127
	private bool loadingCustomModList;

	// Token: 0x04002F60 RID: 12128
	private bool errorLoadingCustomModList;

	// Token: 0x04002F61 RID: 12129
	private int customModListPageCount = -1;

	// Token: 0x04002F62 RID: 12130
	private List<long> customModListModIds = new List<long>();

	// Token: 0x04002F63 RID: 12131
	private List<ModProfile> customModList = new List<ModProfile>();

	// Token: 0x04002F64 RID: 12132
	private int selectedModIndex;

	// Token: 0x04002F65 RID: 12133
	private int currentModPage;

	// Token: 0x04002F66 RID: 12134
	private int totalModCount;

	// Token: 0x04002F67 RID: 12135
	private List<ModProfile> displayedModProfiles = new List<ModProfile>();

	// Token: 0x04002F68 RID: 12136
	private int sortTypeIndex;

	// Token: 0x04002F69 RID: 12137
	private SortModsBy sortType = SortModsBy.Popular;

	// Token: 0x04002F6A RID: 12138
	private const int MAX_SORT_TYPES = 6;

	// Token: 0x04002F6B RID: 12139
	private List<string> searchTags = new List<string>();

	// Token: 0x04002F6C RID: 12140
	private bool isAscendingOrder = true;

	// Token: 0x04002F6D RID: 12141
	private bool isDownloadingThumbnail;

	// Token: 0x04002F6E RID: 12142
	private bool newDownloadRequest;

	// Token: 0x04002F6F RID: 12143
	private DownloadReference currentThumbnail;

	// Token: 0x04002F70 RID: 12144
	private bool restartCustomModListRetrieval;

	// Token: 0x04002F71 RID: 12145
	public CustomMapsListScreen.ListScreenState currentState;

	// Token: 0x04002F72 RID: 12146
	private int retrieveRetryCount;

	// Token: 0x04002F73 RID: 12147
	private const int maxRetryCount = 10;

	// Token: 0x0200069B RID: 1691
	public enum ListScreenState
	{
		// Token: 0x04002F75 RID: 12149
		AvailableMods,
		// Token: 0x04002F76 RID: 12150
		SubscribedMods,
		// Token: 0x04002F77 RID: 12151
		CustomModList
	}
}
