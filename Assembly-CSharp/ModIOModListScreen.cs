﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GorillaNetworking;
using ModIO;
using PlayFab;
using TMPro;
using UnityEngine;

// Token: 0x02000680 RID: 1664
public class ModIOModListScreen : ModIOScreen
{
	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x0600295E RID: 10590 RVA: 0x000CC3D9 File Offset: 0x000CA5D9
	public int CurrentModPage
	{
		get
		{
			return this.currentModPage;
		}
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x0600295F RID: 10591 RVA: 0x000CC3E1 File Offset: 0x000CA5E1
	public int SelectedModIndex
	{
		get
		{
			return this.selectedModIndex;
		}
	}

	// Token: 0x06002960 RID: 10592 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void Initialize()
	{
	}

	// Token: 0x06002961 RID: 10593 RVA: 0x000CC3EC File Offset: 0x000CA5EC
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

	// Token: 0x06002962 RID: 10594 RVA: 0x000CC440 File Offset: 0x000CA640
	protected override void PressButton(ModIOKeyboardButton.ModIOKeyboardBindings buttonPressed)
	{
		if (!ModIOMapsTerminal.IsDriver)
		{
			return;
		}
		if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.goback)
		{
			return;
		}
		if (this.loadingText.gameObject.activeSelf)
		{
			return;
		}
		if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.option3)
		{
			ModIODataStore.Refresh(delegate(bool result)
			{
				if (result)
				{
					this.Refresh(null);
				}
			}, false);
			return;
		}
		if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.up)
		{
			this.selectedModIndex--;
			if ((this.IsOnFirstPage() && this.selectedModIndex < 0) || (!this.IsOnFirstPage() && this.selectedModIndex < -1))
			{
				this.selectedModIndex = (this.IsOnLastPage() ? (this.displayedModProfiles.Count - 1) : this.displayedModProfiles.Count);
			}
			this.UpdateModListSelection();
			ModIOMapsTerminal.SendTerminalStatus(false, false);
			return;
		}
		if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.down)
		{
			this.selectedModIndex++;
			if ((this.IsOnLastPage() && this.selectedModIndex >= this.displayedModProfiles.Count) || (!this.IsOnLastPage() && this.selectedModIndex > this.displayedModProfiles.Count))
			{
				this.selectedModIndex = (this.IsOnFirstPage() ? 0 : -1);
			}
			this.UpdateModListSelection();
			ModIOMapsTerminal.SendTerminalStatus(false, false);
			return;
		}
		if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.enter)
		{
			if (!this.IsOnFirstPage() && this.selectedModIndex == -1)
			{
				this.currentModPage--;
				this.selectedModIndex = 0;
				ModIOMapsTerminal.SendTerminalStatus(false, false);
				this.RefreshScreenState();
				return;
			}
			if (!this.IsOnLastPage() && this.selectedModIndex == this.displayedModProfiles.Count)
			{
				this.currentModPage++;
				this.selectedModIndex = 0;
				ModIOMapsTerminal.SendTerminalStatus(false, false);
				this.RefreshScreenState();
				return;
			}
			if (this.selectedModIndex >= 0 && this.selectedModIndex < this.displayedModProfiles.Count)
			{
				ModIOMapsTerminal.ShowDetailsScreen(this.displayedModProfiles[this.selectedModIndex]);
				return;
			}
		}
		else if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.option1)
		{
			this.SwapListDisplay();
		}
	}

	// Token: 0x06002963 RID: 10595 RVA: 0x000CC614 File Offset: 0x000CA814
	private void SwapListDisplay()
	{
		if (this.currentState == ModIOModListScreen.ListScreenState.AvailableMods)
		{
			this.currentState = ModIOModListScreen.ListScreenState.SubscribedMods;
		}
		else if (this.currentState == ModIOModListScreen.ListScreenState.SubscribedMods)
		{
			this.currentState = ModIOModListScreen.ListScreenState.AvailableMods;
		}
		this.selectedModIndex = 0;
		this.currentModPage = 0;
		ModIOMapsTerminal.UpdateListScreenState(this.currentState);
		ModIOMapsTerminal.SendTerminalStatus(this.currentState == ModIOModListScreen.ListScreenState.SubscribedMods, false);
		this.RefreshScreenState();
	}

	// Token: 0x06002964 RID: 10596 RVA: 0x000CC670 File Offset: 0x000CA870
	public void Refresh(long[] customModListIds = null)
	{
		if (this.loadingAvailableMods || this.loadingFeaturedMods || this.loadingCustomModList)
		{
			return;
		}
		this.currentModPage = 0;
		this.selectedModIndex = 0;
		ModIOMapsTerminal.SendTerminalStatus(false, true);
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

	// Token: 0x06002965 RID: 10597 RVA: 0x000CC73C File Offset: 0x000CA93C
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

	// Token: 0x06002966 RID: 10598 RVA: 0x000CC790 File Offset: 0x000CA990
	private void OnGetFeaturedModsTitleData(string data)
	{
		ModIOModListScreen.<OnGetFeaturedModsTitleData>d__54 <OnGetFeaturedModsTitleData>d__;
		<OnGetFeaturedModsTitleData>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnGetFeaturedModsTitleData>d__.<>4__this = this;
		<OnGetFeaturedModsTitleData>d__.data = data;
		<OnGetFeaturedModsTitleData>d__.<>1__state = -1;
		<OnGetFeaturedModsTitleData>d__.<>t__builder.Start<ModIOModListScreen.<OnGetFeaturedModsTitleData>d__54>(ref <OnGetFeaturedModsTitleData>d__);
	}

	// Token: 0x06002967 RID: 10599 RVA: 0x000CC7D0 File Offset: 0x000CA9D0
	private void RetrieveAvailableMods()
	{
		if (this.loadingAvailableMods)
		{
			return;
		}
		this.loadingAvailableMods = true;
		int num = this.currentAvailableModsRequestPage;
		this.currentAvailableModsRequestPage = num + 1;
		SearchFilter searchFilter = new SearchFilter(num, this.numModsPerRequest);
		searchFilter.SetSortBy(SortModsBy.Popular);
		searchFilter.SetToAscending(true);
		ModIOUnity.GetMods(searchFilter, new Action<ResultAnd<ModPage>>(this.OnAvailableModsRetrieved));
	}

	// Token: 0x06002968 RID: 10600 RVA: 0x000CC828 File Offset: 0x000CAA28
	private void OnAvailableModsRetrieved(ResultAnd<ModPage> result)
	{
		if (result.result.Succeeded())
		{
			this.totalAvailableMods = (int)result.value.totalSearchResultsFound;
			this.availableMods.AddRange(result.value.modProfiles);
			this.FilterAvailableMods();
		}
		else
		{
			this.errorLoadingAvailableMods = true;
		}
		this.loadingAvailableMods = false;
		this.RefreshScreenState();
	}

	// Token: 0x06002969 RID: 10601 RVA: 0x000CC888 File Offset: 0x000CAA88
	private void FilterAvailableMods()
	{
		if (this.availableMods.IsNullOrEmpty<ModProfile>())
		{
			return;
		}
		this.filteredAvailableMods.Clear();
		foreach (ModProfile modProfile in this.availableMods)
		{
			if (!(modProfile.id == ModIODataStore.GetNewMapsModId()) && (this.featuredModIds.IsNullOrEmpty<long>() || !this.featuredModIds.Contains(modProfile.id.id)))
			{
				this.filteredAvailableMods.Add(modProfile);
			}
		}
	}

	// Token: 0x0600296A RID: 10602 RVA: 0x000CC930 File Offset: 0x000CAB30
	private void RetrieveSubscribedMods()
	{
		this.subscribedMods = ModIODataStore.GetSubscribedMods();
		this.FilterSubscribedMods();
		this.totalSubscribedMods = this.filteredSubscribedMods.Count;
		this.RefreshScreenState();
	}

	// Token: 0x0600296B RID: 10603 RVA: 0x000CC95C File Offset: 0x000CAB5C
	private void FilterSubscribedMods()
	{
		if (this.subscribedMods.IsNullOrEmpty<SubscribedMod>())
		{
			return;
		}
		this.filteredSubscribedMods.Clear();
		foreach (SubscribedMod subscribedMod in this.subscribedMods)
		{
			if (!(subscribedMod.modProfile.id == ModIODataStore.GetNewMapsModId()))
			{
				this.filteredSubscribedMods.Add(subscribedMod);
			}
		}
	}

	// Token: 0x0600296C RID: 10604 RVA: 0x000CC9C4 File Offset: 0x000CABC4
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
				this.currentState = ModIOModListScreen.ListScreenState.CustomModList;
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
		this.currentState = ModIOModListScreen.ListScreenState.CustomModList;
		this.loadingCustomModList = true;
		this.RefreshScreenState();
		this.RetrieveCustomModList();
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x000CCAB0 File Offset: 0x000CACB0
	private void RetrieveCustomModList()
	{
		if (this.restartCustomModListRetrieval || this.customModListModIds.Count == 0)
		{
			return;
		}
		ModIODataStore.GetModProfile(new ModId(this.customModListModIds[this.customModList.Count]), new Action<ModIORequestResultAnd<ModProfile>>(this.OnModProfileReceived));
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x000CCB00 File Offset: 0x000CAD00
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
			ModIODataStore.GetModProfile(new ModId(this.customModListModIds[this.customModList.Count]), new Action<ModIORequestResultAnd<ModProfile>>(this.OnModProfileReceived));
			return;
		}
		this.loadingCustomModList = false;
		this.RefreshScreenState();
	}

	// Token: 0x0600296F RID: 10607 RVA: 0x000CCBA8 File Offset: 0x000CADA8
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

	// Token: 0x06002970 RID: 10608 RVA: 0x000CCC08 File Offset: 0x000CAE08
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

	// Token: 0x06002971 RID: 10609 RVA: 0x000CCC60 File Offset: 0x000CAE60
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

	// Token: 0x06002972 RID: 10610 RVA: 0x000CCCC0 File Offset: 0x000CAEC0
	private void RefreshScreenState()
	{
		this.displayedModProfiles.Clear();
		this.modListText.text = "";
		this.modListText.gameObject.SetActive(false);
		this.modPageLabel.gameObject.SetActive(false);
		this.modPageText.gameObject.SetActive(false);
		this.loadingText.gameObject.SetActive(true);
		switch (this.currentState)
		{
		case ModIOModListScreen.ListScreenState.AvailableMods:
		{
			this.titleText.text = this.browseModsTitle;
			this.showOnlySubscribedText.text = this.switchToShowSubscribedOnlyString;
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
		case ModIOModListScreen.ListScreenState.SubscribedMods:
		{
			this.titleText.text = this.subscribedOnlyTitle;
			this.showOnlySubscribedText.text = this.switchToShowAllString;
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
			ModIOMapsTerminal.SendTerminalStatus(true, false);
			return;
		}
		case ModIOModListScreen.ListScreenState.CustomModList:
			this.titleText.text = ModIOMapsTerminal.GetDriverNickname() + "'s " + this.subscribedOnlyTitle;
			this.showOnlySubscribedText.text = this.switchToShowAllString;
			if (this.loadingCustomModList)
			{
				return;
			}
			if (this.errorLoadingCustomModList)
			{
				this.modPageLabel.gameObject.SetActive(false);
				this.modPageText.gameObject.SetActive(false);
				if (ModIOMapsTerminal.IsDriver)
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

	// Token: 0x06002973 RID: 10611 RVA: 0x000CD040 File Offset: 0x000CB240
	public void UpdateModListSelection()
	{
		if (this.displayedModProfiles.IsNullOrEmpty<ModProfile>())
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (!this.IsOnFirstPage())
		{
			stringBuilder.Append((this.selectedModIndex == -1) ? ("> " + this.prevPageString + "\n") : ("  " + this.prevPageString + "\n"));
		}
		long num = (long)(this.currentModPage * this.modsPerPage + 1);
		bool flag = false;
		if (this.currentState == ModIOModListScreen.ListScreenState.AvailableMods)
		{
			if (this.IsOnFirstPage())
			{
				for (int i = 0; i < this.displayedModProfiles.Count; i++)
				{
					if (i < this.totalFeaturedMods)
					{
						stringBuilder.Append((i == this.selectedModIndex) ? ("> FEATURED: " + this.displayedModProfiles[i].name + "\n") : ("  FEATURED: " + this.displayedModProfiles[i].name + "\n"));
					}
					else
					{
						StringBuilder stringBuilder2 = stringBuilder;
						string value;
						if (i != this.selectedModIndex)
						{
							string format = "  {0}. {1}\n";
							long num2 = num;
							num = num2 + 1L;
							value = string.Format(format, num2, this.displayedModProfiles[i].name);
						}
						else
						{
							string format2 = "> {0}. {1}\n";
							long num3 = num;
							num = num3 + 1L;
							value = string.Format(format2, num3, this.displayedModProfiles[i].name);
						}
						stringBuilder2.Append(value);
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
			for (int j = 0; j < this.displayedModProfiles.Count; j++)
			{
				stringBuilder.Append((j == this.selectedModIndex) ? string.Format("> {0}. {1}\n", num + (long)j, this.displayedModProfiles[j].name) : string.Format("  {0}. {1}\n", num + (long)j, this.displayedModProfiles[j].name));
			}
		}
		if (!this.IsOnLastPage())
		{
			stringBuilder.Append((this.selectedModIndex == this.displayedModProfiles.Count) ? ("> " + this.nextPageString + "\n") : ("  " + this.nextPageString + "\n"));
		}
		this.modListText.text = stringBuilder.ToString();
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x000CD290 File Offset: 0x000CB490
	private void UpdatePageCount(int totalMods)
	{
		this.totalModCount = totalMods;
		this.modPageText.gameObject.SetActive(false);
		this.modPageLabel.gameObject.SetActive(false);
		if (this.totalModCount == 0)
		{
			this.modListText.text = ((this.currentState == ModIOModListScreen.ListScreenState.AvailableMods) ? this.noModsAvailableString : this.noSubscribedModsString);
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

	// Token: 0x06002975 RID: 10613 RVA: 0x000CD348 File Offset: 0x000CB548
	public int GetNumPages()
	{
		if (this.currentState == ModIOModListScreen.ListScreenState.CustomModList)
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

	// Token: 0x06002976 RID: 10614 RVA: 0x000CD388 File Offset: 0x000CB588
	private bool IsOnFirstPage()
	{
		return this.currentModPage == 0;
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x000CD394 File Offset: 0x000CB594
	private bool IsOnLastPage()
	{
		long num = (long)this.GetNumPages();
		return (long)(this.currentModPage + 1) == num;
	}

	// Token: 0x06002978 RID: 10616 RVA: 0x000CD3B8 File Offset: 0x000CB5B8
	public void UpdateFromTerminalStatus(ModIOMapsTerminal.TerminalStatus localStatus)
	{
		switch (localStatus.currentScreen)
		{
		case ModIOMapsTerminal.ScreenType.AvailableMods:
			this.currentState = ModIOModListScreen.ListScreenState.AvailableMods;
			break;
		case ModIOMapsTerminal.ScreenType.SubscribedMods:
			if (this.currentState == ModIOModListScreen.ListScreenState.CustomModList && ModIOMapsTerminal.IsDriver)
			{
				this.currentState = ModIOModListScreen.ListScreenState.SubscribedMods;
				this.currentModPage = 0;
				this.selectedModIndex = 0;
				this.customModListPageCount = -1;
				return;
			}
			this.currentState = (ModIOMapsTerminal.IsDriver ? ModIOModListScreen.ListScreenState.SubscribedMods : ModIOModListScreen.ListScreenState.CustomModList);
			break;
		}
		this.currentModPage = localStatus.pageIndex;
		this.selectedModIndex = localStatus.modIndex;
		this.customModListPageCount = localStatus.numModPages;
	}

	// Token: 0x06002979 RID: 10617 RVA: 0x000CD459 File Offset: 0x000CB659
	public void RefreshDriverNickname(string driverNickname)
	{
		if (this.currentState == ModIOModListScreen.ListScreenState.CustomModList)
		{
			this.titleText.text = driverNickname + "'s " + this.subscribedOnlyTitle;
		}
	}

	// Token: 0x04002E97 RID: 11927
	[SerializeField]
	private TMP_Text loadingText;

	// Token: 0x04002E98 RID: 11928
	[SerializeField]
	private TMP_Text modListText;

	// Token: 0x04002E99 RID: 11929
	[SerializeField]
	private TMP_Text modPageLabel;

	// Token: 0x04002E9A RID: 11930
	[SerializeField]
	private TMP_Text modPageText;

	// Token: 0x04002E9B RID: 11931
	[SerializeField]
	private TMP_Text showOnlySubscribedLabel;

	// Token: 0x04002E9C RID: 11932
	[SerializeField]
	private TMP_Text showOnlySubscribedText;

	// Token: 0x04002E9D RID: 11933
	[SerializeField]
	private TMP_Text titleText;

	// Token: 0x04002E9E RID: 11934
	[SerializeField]
	private string browseModsTitle = "AVAILABLE MODS";

	// Token: 0x04002E9F RID: 11935
	[SerializeField]
	private string subscribedOnlyTitle = "SUBSCRIBED MODS";

	// Token: 0x04002EA0 RID: 11936
	[SerializeField]
	private string switchToShowSubscribedOnlyString = "SHOW SUBSCRIBED MODS ONLY";

	// Token: 0x04002EA1 RID: 11937
	[SerializeField]
	private string switchToShowAllString = "SHOW ALL MODS";

	// Token: 0x04002EA2 RID: 11938
	[SerializeField]
	private string nextPageString = "NEXT PAGE";

	// Token: 0x04002EA3 RID: 11939
	[SerializeField]
	private string prevPageString = "PREVIOUS PAGE";

	// Token: 0x04002EA4 RID: 11940
	[SerializeField]
	private string noModsAvailableString = "NO MODS AVAILABLE";

	// Token: 0x04002EA5 RID: 11941
	[SerializeField]
	private string noSubscribedModsString = "NOT SUBSCRIBED TO ANY MODS";

	// Token: 0x04002EA6 RID: 11942
	[SerializeField]
	private string failedToRetrieveModsString = "FAILED TO RETRIEVE MODS FROM MOD.IO \nPRESS THE 'REFRESH' BUTTON TO RETRY";

	// Token: 0x04002EA7 RID: 11943
	[SerializeField]
	private int modsPerPage = 10;

	// Token: 0x04002EA8 RID: 11944
	[SerializeField]
	private int numModsPerRequest = 50;

	// Token: 0x04002EA9 RID: 11945
	[SerializeField]
	private string featuredModsPlayFabKey = "VStumpFeaturedMaps";

	// Token: 0x04002EAA RID: 11946
	private bool loadingFeaturedMods;

	// Token: 0x04002EAB RID: 11947
	private int totalFeaturedMods;

	// Token: 0x04002EAC RID: 11948
	private List<long> featuredModIds = new List<long>();

	// Token: 0x04002EAD RID: 11949
	private List<ModProfile> featuredMods = new List<ModProfile>();

	// Token: 0x04002EAE RID: 11950
	private int currentAvailableModsRequestPage;

	// Token: 0x04002EAF RID: 11951
	private bool loadingAvailableMods;

	// Token: 0x04002EB0 RID: 11952
	private int totalAvailableMods;

	// Token: 0x04002EB1 RID: 11953
	private bool errorLoadingAvailableMods;

	// Token: 0x04002EB2 RID: 11954
	private List<ModProfile> availableMods = new List<ModProfile>();

	// Token: 0x04002EB3 RID: 11955
	private List<ModProfile> filteredAvailableMods = new List<ModProfile>();

	// Token: 0x04002EB4 RID: 11956
	private int totalSubscribedMods;

	// Token: 0x04002EB5 RID: 11957
	private SubscribedMod[] subscribedMods;

	// Token: 0x04002EB6 RID: 11958
	private List<SubscribedMod> filteredSubscribedMods = new List<SubscribedMod>();

	// Token: 0x04002EB7 RID: 11959
	private bool loadingCustomModList;

	// Token: 0x04002EB8 RID: 11960
	private bool errorLoadingCustomModList;

	// Token: 0x04002EB9 RID: 11961
	private int customModListPageCount = -1;

	// Token: 0x04002EBA RID: 11962
	private List<long> customModListModIds = new List<long>();

	// Token: 0x04002EBB RID: 11963
	private List<ModProfile> customModList = new List<ModProfile>();

	// Token: 0x04002EBC RID: 11964
	private int selectedModIndex;

	// Token: 0x04002EBD RID: 11965
	private int currentModPage;

	// Token: 0x04002EBE RID: 11966
	private int totalModCount;

	// Token: 0x04002EBF RID: 11967
	private List<ModProfile> displayedModProfiles = new List<ModProfile>();

	// Token: 0x04002EC0 RID: 11968
	private bool restartCustomModListRetrieval;

	// Token: 0x04002EC1 RID: 11969
	public ModIOModListScreen.ListScreenState currentState;

	// Token: 0x02000681 RID: 1665
	public enum ListScreenState
	{
		// Token: 0x04002EC3 RID: 11971
		AvailableMods,
		// Token: 0x04002EC4 RID: 11972
		SubscribedMods,
		// Token: 0x04002EC5 RID: 11973
		CustomModList
	}
}
