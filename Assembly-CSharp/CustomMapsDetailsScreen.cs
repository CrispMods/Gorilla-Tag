using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaTagScripts.ModIO;
using ModIO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x02000698 RID: 1688
public class CustomMapsDetailsScreen : CustomMapsTerminalScreen
{
	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x060029C2 RID: 10690 RVA: 0x0004C396 File Offset: 0x0004A596
	// (set) Token: 0x060029C1 RID: 10689 RVA: 0x0004C38D File Offset: 0x0004A58D
	public ModProfile currentModProfile { get; private set; }

	// Token: 0x060029C3 RID: 10691 RVA: 0x00030607 File Offset: 0x0002E807
	public override void Initialize()
	{
	}

	// Token: 0x060029C4 RID: 10692 RVA: 0x00117EFC File Offset: 0x001160FC
	public override void Show()
	{
		base.Show();
		GameEvents.ModIOModManagementEvent.RemoveListener(new UnityAction<ModManagementEventType, ModId, Result>(this.HandleModManagementEvent));
		CustomMapManager.OnMapLoadStatusChanged.RemoveListener(new UnityAction<MapLoadStatus, int, string>(this.OnMapLoadProgress));
		CustomMapManager.OnMapLoadComplete.RemoveListener(new UnityAction<bool>(this.OnMapLoadComplete));
		CustomMapManager.OnRoomMapChanged.RemoveListener(new UnityAction<ModId>(this.OnRoomMapChanged));
		CustomMapManager.OnMapUnloadComplete.RemoveListener(new UnityAction(this.OnMapUnloaded));
		GameEvents.ModIOModManagementEvent.AddListener(new UnityAction<ModManagementEventType, ModId, Result>(this.HandleModManagementEvent));
		CustomMapManager.OnMapLoadStatusChanged.AddListener(new UnityAction<MapLoadStatus, int, string>(this.OnMapLoadProgress));
		CustomMapManager.OnMapLoadComplete.AddListener(new UnityAction<bool>(this.OnMapLoadComplete));
		CustomMapManager.OnRoomMapChanged.AddListener(new UnityAction<ModId>(this.OnRoomMapChanged));
		CustomMapManager.OnMapUnloadComplete.AddListener(new UnityAction(this.OnMapUnloaded));
		for (int i = 0; i < this.buttonsToHide.Length; i++)
		{
			this.buttonsToHide[i].SetActive(false);
		}
		for (int j = 0; j < this.buttonsToShow.Length; j++)
		{
			this.buttonsToShow[j].SetActive(true);
		}
		this.ResetToDefaultView();
	}

	// Token: 0x060029C5 RID: 10693 RVA: 0x00118034 File Offset: 0x00116234
	public override void Hide()
	{
		base.Hide();
		GameEvents.ModIOModManagementEvent.RemoveListener(new UnityAction<ModManagementEventType, ModId, Result>(this.HandleModManagementEvent));
		CustomMapManager.OnMapLoadStatusChanged.RemoveListener(new UnityAction<MapLoadStatus, int, string>(this.OnMapLoadProgress));
		CustomMapManager.OnMapLoadComplete.RemoveListener(new UnityAction<bool>(this.OnMapLoadComplete));
		CustomMapManager.OnRoomMapChanged.RemoveListener(new UnityAction<ModId>(this.OnRoomMapChanged));
		CustomMapManager.OnMapUnloadComplete.RemoveListener(new UnityAction(this.OnMapUnloaded));
		this.lastCompletedOperation = ModManagementOperationType.None_ErrorOcurred;
	}

	// Token: 0x060029C6 RID: 10694 RVA: 0x001180BC File Offset: 0x001162BC
	private void HandleModManagementEvent(ModManagementEventType eventType, ModId modId, Result result)
	{
		if (base.isActiveAndEnabled && this.hasModProfile && this.currentModProfile.id.id == modId.id)
		{
			this.UpdateSubscriptionStatus(eventType == ModManagementEventType.InstallFailed || eventType == ModManagementEventType.UninstallFailed || eventType == ModManagementEventType.DownloadFailed || eventType == ModManagementEventType.UpdateFailed);
			if (result.errorCode == 20460U)
			{
				this.modDescriptionText.gameObject.SetActive(false);
				this.loadingMapLabelText.text = this.mapLoadingErrorString;
				this.loadingMapLabelText.gameObject.SetActive(true);
				this.loadingMapMessageText.text = this.mapLoadingErrorInvalidModFile;
				this.loadingMapMessageText.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x060029C7 RID: 10695 RVA: 0x00118174 File Offset: 0x00116374
	private void Update()
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (this.currentProgressHandle != null && this.currentProgressHandle.modId != this.currentModProfile.id)
		{
			this.currentProgressHandle = null;
		}
		if (this.currentProgressHandle == null)
		{
			if (!ModIOUnity.IsModManagementBusy())
			{
				return;
			}
			ProgressHandle currentModManagementOperation = ModIOUnity.GetCurrentModManagementOperation();
			if (currentModManagementOperation == null || currentModManagementOperation.Completed || !(currentModManagementOperation.modId == this.currentModProfile.id))
			{
				return;
			}
			this.currentProgressHandle = currentModManagementOperation;
		}
		string str;
		if (this.modOperationTypeStrings.TryGetValue(this.currentProgressHandle.OperationType, out str))
		{
			float f = this.currentProgressHandle.Progress * 100f;
			this.modStatusText.text = str + string.Format(" {0}%", Mathf.RoundToInt(f));
		}
		if (this.currentProgressHandle.Completed)
		{
			this.lastCompletedOperation = this.currentProgressHandle.OperationType;
			this.currentProgressHandle = null;
			this.UpdateSubscriptionStatus(false);
		}
	}

	// Token: 0x060029C8 RID: 10696 RVA: 0x00118278 File Offset: 0x00116478
	public void RetrieveProfileFromModIO(long id, Action<ModIORequestResultAnd<ModProfile>> callback = null)
	{
		if (this.hasModProfile && this.currentModProfile.id == id)
		{
			this.UpdateModDetails(true);
			return;
		}
		this.pendingModId = id;
		ModIOManager.GetModProfile(new ModId(id), (callback != null) ? callback : new Action<ModIORequestResultAnd<ModProfile>>(this.OnProfileReceived));
	}

	// Token: 0x060029C9 RID: 10697 RVA: 0x0004C39E File Offset: 0x0004A59E
	public void SetModProfile(ModProfile modProfile)
	{
		if (modProfile.id != ModId.Null)
		{
			this.pendingModId = 0L;
			this.currentModProfile = modProfile;
			this.hasModProfile = true;
			this.UpdateModDetails(true);
		}
	}

	// Token: 0x060029CA RID: 10698 RVA: 0x001182CC File Offset: 0x001164CC
	protected override void PressButton(CustomMapsTerminalButton.ModIOKeyboardBindings buttonPressed)
	{
		if (!base.isActiveAndEnabled || !CustomMapsTerminal.IsDriver)
		{
			return;
		}
		if (buttonPressed == CustomMapsTerminalButton.ModIOKeyboardBindings.goback)
		{
			if (CustomMapManager.IsLoading(0L))
			{
				return;
			}
			if (CustomMapManager.IsUnloading())
			{
				return;
			}
			if (this.mapLoadError)
			{
				this.mapLoadError = false;
				CustomMapManager.ClearRoomMap();
				this.ResetToDefaultView();
				return;
			}
			if (!CustomMapLoader.IsModLoaded(0L) && !(CustomMapManager.GetRoomMapId() != ModId.Null))
			{
				CustomMapsTerminal.ReturnFromDetailsScreen();
				this.hasModProfile = false;
				this.currentModProfile = default(ModProfile);
				return;
			}
			string text;
			if (!this.CanChangeMapState(false, out text))
			{
				this.modDescriptionText.gameObject.SetActive(false);
				this.errorText.text = text;
				this.errorText.gameObject.SetActive(true);
				return;
			}
			this.UnloadMod();
			return;
		}
		else
		{
			if (!this.hasModProfile)
			{
				return;
			}
			if (buttonPressed != CustomMapsTerminalButton.ModIOKeyboardBindings.option3)
			{
				if (buttonPressed == CustomMapsTerminalButton.ModIOKeyboardBindings.map)
				{
					if (CustomMapLoader.IsModLoaded(0L) || CustomMapManager.IsLoading(0L) || CustomMapManager.IsUnloading())
					{
						return;
					}
					this.errorText.gameObject.SetActive(false);
					this.errorText.text = "";
					this.loadingMapLabelText.gameObject.SetActive(false);
					this.loadingMapMessageText.gameObject.SetActive(false);
					this.modDescriptionText.gameObject.SetActive(true);
					ModIOManager.Refresh(delegate(bool result)
					{
						SubscribedModStatus subscribedModStatus2;
						if (ModIOManager.GetSubscribedModStatus(this.currentModProfile.id, out subscribedModStatus2))
						{
							ModIOManager.UnsubscribeFromMod(this.currentModProfile.id, delegate(Result result)
							{
								if (result.Succeeded())
								{
									this.UpdateModDetails(false);
								}
							});
							return;
						}
						ModIOManager.SubscribeToMod(this.currentModProfile.id, delegate(Result result)
						{
							if (result.Succeeded())
							{
								this.UpdateModDetails(false);
								ModIOManager.DownloadMod(this.currentModProfile.id, delegate(ModIORequestResult result)
								{
									GorillaTelemetry.PostCustomMapDownloadEvent(this.currentModProfile.name, this.currentModProfile.id, this.currentModProfile.creator.username);
									this.UpdateModDetails(false);
								});
							}
						});
					}, false);
				}
				SubscribedModStatus subscribedModStatus;
				if (buttonPressed == CustomMapsTerminalButton.ModIOKeyboardBindings.enter && !CustomMapManager.IsLoading(0L) && !CustomMapManager.IsUnloading() && !CustomMapLoader.IsModLoaded(0L) && this.lastCompletedOperation != ModManagementOperationType.Update && ModIOManager.GetSubscribedModStatus(this.currentModProfile.id, out subscribedModStatus))
				{
					if (subscribedModStatus == SubscribedModStatus.Installed)
					{
						string text2;
						if (!this.CanChangeMapState(true, out text2))
						{
							this.modDescriptionText.gameObject.SetActive(false);
							this.errorText.text = text2;
							this.errorText.gameObject.SetActive(true);
							return;
						}
						this.LoadMap();
						return;
					}
					else if (subscribedModStatus == SubscribedModStatus.WaitingToDownload || (subscribedModStatus == SubscribedModStatus.WaitingToUpdate && this.lastCompletedOperation != ModManagementOperationType.Update))
					{
						ModIOManager.DownloadMod(this.currentModProfile.id, delegate(ModIORequestResult result)
						{
							this.UpdateSubscriptionStatus(!result.success);
							GorillaTelemetry.PostCustomMapDownloadEvent(this.currentModProfile.name, this.currentModProfile.id, this.currentModProfile.creator.username);
						});
					}
				}
				return;
			}
			if (CustomMapLoader.IsModLoaded(0L) || CustomMapManager.IsLoading(0L) || CustomMapManager.IsUnloading() || CustomMapManager.GetRoomMapId() != ModId.Null)
			{
				return;
			}
			if (this.hasModProfile)
			{
				long currentModId = this.currentModProfile.id.id;
				this.hasModProfile = false;
				this.currentModProfile = default(ModProfile);
				ModIOManager.Refresh(delegate(bool result)
				{
					this.RetrieveProfileFromModIO(currentModId, null);
				}, false);
			}
			return;
		}
	}

	// Token: 0x060029CB RID: 10699 RVA: 0x00118564 File Offset: 0x00116764
	private void OnProfileReceived(ModIORequestResultAnd<ModProfile> profile)
	{
		if (profile.result.success)
		{
			this.SetModProfile(profile.data);
			return;
		}
		this.modDescriptionText.gameObject.SetActive(false);
		this.errorText.text = "Failed to retrieve mod details";
		this.errorText.gameObject.SetActive(true);
	}

	// Token: 0x060029CC RID: 10700 RVA: 0x001185C0 File Offset: 0x001167C0
	private void ResetToDefaultView()
	{
		this.loadingMapLabelText.gameObject.SetActive(false);
		this.loadingMapMessageText.gameObject.SetActive(false);
		this.mapReadyText.gameObject.SetActive(false);
		this.errorText.gameObject.SetActive(false);
		this.modNameText.gameObject.SetActive(false);
		this.modCreatorLabelText.gameObject.SetActive(false);
		this.modCreatorText.gameObject.SetActive(false);
		this.modDescriptionText.gameObject.SetActive(false);
		this.modStatusText.gameObject.SetActive(false);
		this.modSubscriptionStatusText.gameObject.SetActive(false);
		this.OptionButtonTooltipText.gameObject.SetActive(false);
		this.mapScreenshotImage.gameObject.SetActive(false);
		this.loadRoomMapPromptText.gameObject.SetActive(false);
		this.outdatedText.gameObject.SetActive(false);
		this.loadingText.gameObject.SetActive(true);
		if (CustomMapLoader.IsModLoaded(0L) || CustomMapManager.IsLoading(0L) || CustomMapManager.IsUnloading())
		{
			ModId modId = new ModId(CustomMapLoader.IsModLoaded(0L) ? CustomMapLoader.LoadedMapModId : (CustomMapManager.IsLoading(0L) ? CustomMapManager.LoadingMapId : CustomMapManager.UnloadingMapId));
			if (this.hasModProfile && this.currentModProfile.id == modId)
			{
				this.UpdateModDetails(true);
				return;
			}
			this.RetrieveProfileFromModIO(modId, delegate(ModIORequestResultAnd<ModProfile> result)
			{
				this.OnProfileReceived(result);
			});
			return;
		}
		else
		{
			if (CustomMapManager.GetRoomMapId() != ModId.Null)
			{
				this.OnRoomMapChanged(CustomMapManager.GetRoomMapId());
				return;
			}
			if (this.hasModProfile)
			{
				this.UpdateModDetails(true);
			}
			return;
		}
	}

	// Token: 0x060029CD RID: 10701 RVA: 0x0011877C File Offset: 0x0011697C
	private void UpdateModDetails(bool refreshScreenState = true)
	{
		if (!this.hasModProfile)
		{
			return;
		}
		this.modNameText.text = this.currentModProfile.name;
		this.modCreatorText.text = this.currentModProfile.creator.username;
		this.modDescriptionText.text = this.currentModProfile.description;
		this.UpdateSubscriptionStatus(false);
		ModIOUnity.DownloadTexture(this.currentModProfile.logoImage320x180, new Action<ResultAnd<Texture2D>>(this.OnTextureDownloaded));
		if (refreshScreenState)
		{
			this.loadingText.gameObject.SetActive(false);
			this.loadingMapLabelText.gameObject.SetActive(false);
			this.loadingMapMessageText.gameObject.SetActive(false);
			this.loadRoomMapPromptText.gameObject.SetActive(false);
			this.mapReadyText.gameObject.SetActive(false);
			this.unloadPromptText.gameObject.SetActive(false);
			this.errorText.gameObject.SetActive(false);
			this.modNameText.gameObject.SetActive(true);
			this.modCreatorLabelText.gameObject.SetActive(true);
			this.modCreatorText.gameObject.SetActive(true);
			this.modDescriptionText.gameObject.SetActive(true);
			if (CustomMapLoader.IsModLoaded(0L))
			{
				ModId modId = new ModId(CustomMapLoader.LoadedMapModId);
				if (this.currentModProfile.id == modId)
				{
					this.OnMapLoadComplete_UIUpdate();
					return;
				}
				this.RetrieveProfileFromModIO(modId, delegate(ModIORequestResultAnd<ModProfile> result)
				{
					this.OnProfileReceived(result);
				});
				return;
			}
			else
			{
				if (CustomMapManager.IsLoading(0L))
				{
					this.modDescriptionText.gameObject.SetActive(false);
					this.loadingMapLabelText.text = this.mapLoadingString + " 0%";
					this.loadingMapLabelText.gameObject.SetActive(true);
					return;
				}
				if (CustomMapManager.IsUnloading())
				{
					this.modDescriptionText.gameObject.SetActive(false);
					this.loadingMapLabelText.text = this.mapUnloadingString;
					this.loadingMapLabelText.gameObject.SetActive(true);
					return;
				}
				if (CustomMapManager.GetRoomMapId() != ModId.Null)
				{
					this.modDescriptionText.gameObject.SetActive(false);
					this.loadRoomMapPromptText.gameObject.SetActive(true);
					return;
				}
				if (this.mapLoadError)
				{
					this.modDescriptionText.gameObject.SetActive(false);
					this.loadingMapLabelText.gameObject.SetActive(true);
					this.loadingMapMessageText.gameObject.SetActive(true);
				}
			}
		}
	}

	// Token: 0x060029CE RID: 10702 RVA: 0x001189F8 File Offset: 0x00116BF8
	private void OnTextureDownloaded(ResultAnd<Texture2D> resultAnd)
	{
		if (!resultAnd.result.Succeeded())
		{
			return;
		}
		Texture2D value = resultAnd.value;
		this.mapScreenshotImage.sprite = Sprite.Create(value, new Rect(0f, 0f, (float)value.width, (float)value.height), new Vector2(0.5f, 0.5f));
		this.mapScreenshotImage.gameObject.SetActive(true);
	}

	// Token: 0x060029CF RID: 10703 RVA: 0x00118A68 File Offset: 0x00116C68
	private void UpdateSubscriptionStatus(bool errorEncountered = false)
	{
		if (base.isActiveAndEnabled)
		{
			bool flag = this.mapLoadError || CustomMapManager.IsUnloading() || CustomMapManager.IsLoading(0L) || CustomMapLoader.IsModLoaded(0L) || CustomMapManager.GetRoomMapId() != ModId.Null;
			this.outdatedText.gameObject.SetActive(false);
			if (!flag)
			{
				SubscribedModStatus subscribedModStatus2;
				bool subscribedModStatus = ModIOManager.GetSubscribedModStatus(this.currentModProfile.id, out subscribedModStatus2);
				if (errorEncountered)
				{
					this.lastCompletedOperation = ModManagementOperationType.None_ErrorOcurred;
					subscribedModStatus2 = SubscribedModStatus.ProblemOccurred;
				}
				this.modSubscriptionStatusText.text = (subscribedModStatus ? this.subscribedStatusString : this.unsubscribedStatusString);
				if (this.subscriptionToggleButton.IsNotNull())
				{
					this.subscriptionToggleButton.SetButtonStatus(subscribedModStatus);
				}
				if (subscribedModStatus)
				{
					switch (subscribedModStatus2)
					{
					case SubscribedModStatus.Installed:
					{
						this.selectButtonTooltipText.text = this.loadMapTooltipString;
						this.selectButtonTooltipText.gameObject.SetActive(true);
						this.lastCompletedOperation = ModManagementOperationType.None_ErrorOcurred;
						int num;
						if (ModIOManager.IsModOutdated(this.currentModProfile.id, out num))
						{
							this.outdatedText.gameObject.SetActive(true);
							goto IL_186;
						}
						this.outdatedText.gameObject.SetActive(false);
						goto IL_186;
					}
					case SubscribedModStatus.WaitingToDownload:
						this.selectButtonTooltipText.text = this.downloadMapTooltipString;
						this.selectButtonTooltipText.gameObject.SetActive(true);
						goto IL_186;
					case SubscribedModStatus.WaitingToUpdate:
						this.selectButtonTooltipText.text = this.updateMapTooltipString;
						this.selectButtonTooltipText.gameObject.SetActive(true);
						goto IL_186;
					}
					this.selectButtonTooltipText.gameObject.SetActive(false);
					IL_186:
					string text;
					if (subscribedModStatus2 == SubscribedModStatus.WaitingToUpdate && this.lastCompletedOperation == ModManagementOperationType.Update)
					{
						this.modStatusText.text = "INSTALLING";
					}
					else if (this.modStatusStrings.TryGetValue(subscribedModStatus2, out text))
					{
						this.modStatusText.text = text;
					}
					else
					{
						this.modStatusText.text = "STATUS STRING MISSING!";
					}
					if (ModIOManager.IsModInDownloadQueue(this.currentModProfile.id))
					{
						this.selectButtonTooltipText.gameObject.SetActive(false);
						this.modStatusText.text = this.mapDownloadQueuedString;
					}
					this.OptionButtonTooltipText.text = this.unsubscribeTooltipString;
				}
				else
				{
					this.selectButtonTooltipText.gameObject.SetActive(false);
					this.modStatusText.text = this.modAvailableString;
					this.OptionButtonTooltipText.text = this.subscribeTooltipString;
				}
				this.modStatusText.gameObject.SetActive(true);
				this.OptionButtonTooltipText.gameObject.SetActive(true);
				this.modSubscriptionStatusText.gameObject.SetActive(true);
				return;
			}
			this.lastCompletedOperation = ModManagementOperationType.None_ErrorOcurred;
			this.selectButtonTooltipText.gameObject.SetActive(false);
			this.modStatusText.gameObject.SetActive(false);
			this.OptionButtonTooltipText.gameObject.SetActive(false);
			this.modSubscriptionStatusText.gameObject.SetActive(false);
		}
	}

	// Token: 0x060029D0 RID: 10704 RVA: 0x00118D44 File Offset: 0x00116F44
	private bool CanChangeMapState(bool load, out string disallowedReason)
	{
		disallowedReason = "";
		if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.SessionIsPrivate)
		{
			if (!CustomMapManager.AreAllPlayersInVirtualStump())
			{
				disallowedReason = "ALL PLAYERS IN THE ROOM MUST BE INSIDE THE VIRTUAL STUMP BEFORE " + (load ? "" : "UN") + "LOADING A MAP.";
				return false;
			}
			return true;
		}
		else
		{
			if (!CustomMapManager.IsLocalPlayerInVirtualStump())
			{
				disallowedReason = "YOU MUST BE INSIDE THE VIRTUAL STUMP TO " + (load ? "" : "UN") + "LOAD A MAP.";
				return false;
			}
			return true;
		}
	}

	// Token: 0x060029D1 RID: 10705 RVA: 0x00118DC8 File Offset: 0x00116FC8
	private void LoadMap()
	{
		this.modDescriptionText.gameObject.SetActive(false);
		this.OptionButtonTooltipText.gameObject.SetActive(false);
		this.selectButtonTooltipText.gameObject.SetActive(false);
		this.modStatusText.gameObject.SetActive(false);
		this.modSubscriptionStatusText.gameObject.SetActive(false);
		this.outdatedText.gameObject.SetActive(false);
		this.loadingMapLabelText.gameObject.SetActive(true);
		if (NetworkSystem.Instance.InRoom && !NetworkSystem.Instance.SessionIsPrivate)
		{
			NetworkSystem.Instance.ReturnToSinglePlayer();
		}
		this.networkObject.LoadModSynced(this.currentModProfile.id);
	}

	// Token: 0x060029D2 RID: 10706 RVA: 0x0004C3CF File Offset: 0x0004A5CF
	private void UnloadMod()
	{
		this.networkObject.UnloadModSynced();
	}

	// Token: 0x060029D3 RID: 10707 RVA: 0x0004C3DC File Offset: 0x0004A5DC
	public void OnMapLoadComplete(bool success)
	{
		if (success)
		{
			this.OnMapLoadComplete_UIUpdate();
			return;
		}
		this.mapLoadError = true;
	}

	// Token: 0x060029D4 RID: 10708 RVA: 0x00118E8C File Offset: 0x0011708C
	private void OnMapLoadComplete_UIUpdate()
	{
		this.modDescriptionText.gameObject.SetActive(false);
		this.loadingMapLabelText.gameObject.SetActive(false);
		this.loadingMapMessageText.gameObject.SetActive(false);
		this.loadRoomMapPromptText.gameObject.SetActive(false);
		this.errorText.gameObject.SetActive(false);
		this.mapReadyText.gameObject.SetActive(true);
		this.unloadPromptText.gameObject.SetActive(true);
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x0004C3EF File Offset: 0x0004A5EF
	private void OnMapUnloaded()
	{
		this.mapLoadError = false;
		this.UpdateModDetails(true);
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x00118F10 File Offset: 0x00117110
	private void OnRoomMapChanged(ModId roomMapID)
	{
		if (roomMapID == ModId.Null)
		{
			this.UpdateModDetails(true);
			return;
		}
		if (this.currentModProfile.id != roomMapID)
		{
			this.RetrieveProfileFromModIO(roomMapID.id, new Action<ModIORequestResultAnd<ModProfile>>(this.OnRoomMapProfileReceived));
			return;
		}
		this.ShowLoadRoomMapPrompt();
	}

	// Token: 0x060029D7 RID: 10711 RVA: 0x0004C3FF File Offset: 0x0004A5FF
	private void OnRoomMapProfileReceived(ModIORequestResultAnd<ModProfile> result)
	{
		this.OnProfileReceived(result);
		if (result.result.success)
		{
			this.ShowLoadRoomMapPrompt();
		}
	}

	// Token: 0x060029D8 RID: 10712 RVA: 0x00118F64 File Offset: 0x00117164
	private void ShowLoadRoomMapPrompt()
	{
		if (CustomMapManager.IsUnloading() || CustomMapManager.IsLoading(0L) || CustomMapLoader.IsModLoaded(this.currentModProfile.id))
		{
			return;
		}
		this.modDescriptionText.gameObject.SetActive(false);
		this.loadingText.gameObject.SetActive(false);
		this.loadingMapLabelText.gameObject.SetActive(false);
		this.mapReadyText.gameObject.SetActive(false);
		this.unloadPromptText.gameObject.SetActive(false);
		this.loadRoomMapPromptText.gameObject.SetActive(true);
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x00119000 File Offset: 0x00117200
	public void OnMapLoadProgress(MapLoadStatus loadStatus, int progress, string message)
	{
		if (loadStatus != MapLoadStatus.None)
		{
			this.mapLoadError = false;
			this.loadRoomMapPromptText.gameObject.SetActive(false);
			this.modDescriptionText.gameObject.SetActive(false);
		}
		switch (loadStatus)
		{
		case MapLoadStatus.Downloading:
			this.loadingMapLabelText.text = this.mapAutoDownloadingString;
			this.loadingMapLabelText.gameObject.SetActive(true);
			this.loadingMapMessageText.gameObject.SetActive(false);
			this.loadingMapMessageText.text = "";
			return;
		case MapLoadStatus.Loading:
			this.loadingMapLabelText.text = this.mapLoadingString + " " + progress.ToString() + "%";
			this.loadingMapLabelText.gameObject.SetActive(true);
			this.loadingMapMessageText.text = message;
			this.loadingMapMessageText.gameObject.SetActive(true);
			return;
		case MapLoadStatus.Unloading:
			this.mapReadyText.gameObject.SetActive(false);
			this.unloadPromptText.gameObject.SetActive(false);
			this.loadingMapLabelText.text = this.mapUnloadingString;
			this.loadingMapLabelText.gameObject.SetActive(true);
			this.loadingMapMessageText.gameObject.SetActive(false);
			this.loadingMapMessageText.text = "";
			return;
		case MapLoadStatus.Error:
			this.loadingMapLabelText.text = this.mapLoadingErrorString;
			this.loadingMapLabelText.gameObject.SetActive(true);
			if (CustomMapsTerminal.IsDriver)
			{
				this.loadingMapMessageText.text = message + "\n" + this.mapLoadingErrorDriverString;
			}
			else
			{
				this.loadingMapMessageText.text = message + "\n" + this.mapLoadingErrorNonDriverString;
			}
			this.loadingMapMessageText.gameObject.SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x0004C41B File Offset: 0x0004A61B
	public long GetModId()
	{
		return this.currentModProfile.id.id;
	}

	// Token: 0x04002F0A RID: 12042
	[SerializeField]
	private SpriteRenderer mapScreenshotImage;

	// Token: 0x04002F0B RID: 12043
	[SerializeField]
	private TMP_Text loadingText;

	// Token: 0x04002F0C RID: 12044
	[SerializeField]
	private TMP_Text modNameText;

	// Token: 0x04002F0D RID: 12045
	[SerializeField]
	private TMP_Text modCreatorLabelText;

	// Token: 0x04002F0E RID: 12046
	[SerializeField]
	private TMP_Text modCreatorText;

	// Token: 0x04002F0F RID: 12047
	[SerializeField]
	private TMP_Text modDescriptionText;

	// Token: 0x04002F10 RID: 12048
	[SerializeField]
	private TMP_Text selectButtonTooltipText;

	// Token: 0x04002F11 RID: 12049
	[SerializeField]
	private TMP_Text OptionButtonTooltipText;

	// Token: 0x04002F12 RID: 12050
	[SerializeField]
	private TMP_Text modStatusText;

	// Token: 0x04002F13 RID: 12051
	[SerializeField]
	private TMP_Text modSubscriptionStatusText;

	// Token: 0x04002F14 RID: 12052
	[SerializeField]
	private TMP_Text loadingMapLabelText;

	// Token: 0x04002F15 RID: 12053
	[SerializeField]
	private TMP_Text loadingMapMessageText;

	// Token: 0x04002F16 RID: 12054
	[SerializeField]
	private TMP_Text loadRoomMapPromptText;

	// Token: 0x04002F17 RID: 12055
	[SerializeField]
	private TMP_Text mapReadyText;

	// Token: 0x04002F18 RID: 12056
	[SerializeField]
	private TMP_Text unloadPromptText;

	// Token: 0x04002F19 RID: 12057
	[SerializeField]
	private TMP_Text errorText;

	// Token: 0x04002F1A RID: 12058
	[SerializeField]
	private TMP_Text outdatedText;

	// Token: 0x04002F1B RID: 12059
	[SerializeField]
	private GameObject[] buttonsToHide;

	// Token: 0x04002F1C RID: 12060
	[SerializeField]
	private GameObject[] buttonsToShow;

	// Token: 0x04002F1D RID: 12061
	[SerializeField]
	private CustomMapsTerminalToggleButton subscriptionToggleButton;

	// Token: 0x04002F1E RID: 12062
	[SerializeField]
	private string modAvailableString = "AVAILABLE";

	// Token: 0x04002F1F RID: 12063
	[SerializeField]
	private string mapAutoDownloadingString = "DOWNLOADING...";

	// Token: 0x04002F20 RID: 12064
	[SerializeField]
	private string mapDownloadQueuedString = "DOWNLOAD QUEUED";

	// Token: 0x04002F21 RID: 12065
	[SerializeField]
	private string mapLoadingString = "LOADING:";

	// Token: 0x04002F22 RID: 12066
	[SerializeField]
	private string mapUnloadingString = "UNLOADING...";

	// Token: 0x04002F23 RID: 12067
	[SerializeField]
	private string mapLoadingErrorString = "ERROR:";

	// Token: 0x04002F24 RID: 12068
	[SerializeField]
	private string mapLoadingErrorDriverString = "PRESS THE 'BACK' BUTTON TO TRY AGAIN";

	// Token: 0x04002F25 RID: 12069
	[SerializeField]
	private string mapLoadingErrorNonDriverString = "LEAVE AND REJOIN THE VIRTUAL STUMP TO TRY AGAIN";

	// Token: 0x04002F26 RID: 12070
	[SerializeField]
	private string mapLoadingErrorInvalidModFile = "INSTALL FAILED DUE TO INVALID MAP FILE";

	// Token: 0x04002F27 RID: 12071
	[SerializeField]
	private VirtualStumpSerializer networkObject;

	// Token: 0x04002F28 RID: 12072
	private Dictionary<SubscribedModStatus, string> modStatusStrings = new Dictionary<SubscribedModStatus, string>
	{
		{
			SubscribedModStatus.Installed,
			"READY"
		},
		{
			SubscribedModStatus.WaitingToDownload,
			"NOT DOWNLOADED"
		},
		{
			SubscribedModStatus.WaitingToInstall,
			"INSTALL QUEUED"
		},
		{
			SubscribedModStatus.WaitingToUpdate,
			"NEEDS UPDATE"
		},
		{
			SubscribedModStatus.WaitingToUninstall,
			"UNINSTALL QUEUED"
		},
		{
			SubscribedModStatus.Downloading,
			"DOWNLOADING"
		},
		{
			SubscribedModStatus.Installing,
			"INSTALLING"
		},
		{
			SubscribedModStatus.Uninstalling,
			"UNINSTALLING"
		},
		{
			SubscribedModStatus.Updating,
			"UPDATING"
		},
		{
			SubscribedModStatus.ProblemOccurred,
			"ERROR"
		},
		{
			SubscribedModStatus.None,
			""
		}
	};

	// Token: 0x04002F29 RID: 12073
	private Dictionary<ModManagementOperationType, string> modOperationTypeStrings = new Dictionary<ModManagementOperationType, string>
	{
		{
			ModManagementOperationType.Install,
			"INSTALLING"
		},
		{
			ModManagementOperationType.Download,
			"DOWNLOADING"
		},
		{
			ModManagementOperationType.Update,
			"UPDATING"
		},
		{
			ModManagementOperationType.Uninstall,
			"UNINSTALLING"
		},
		{
			ModManagementOperationType.None_ErrorOcurred,
			"ERROR"
		},
		{
			ModManagementOperationType.None_AlreadyInstalled,
			"READY"
		}
	};

	// Token: 0x04002F2A RID: 12074
	[FormerlySerializedAs("unsubscribedTooltipString")]
	[SerializeField]
	private string subscribeTooltipString = "MAP: SUBSCRIBE TO MAP";

	// Token: 0x04002F2B RID: 12075
	[FormerlySerializedAs("subscribedTooltipString")]
	[SerializeField]
	private string unsubscribeTooltipString = "MAP: UNSUBSCRIBE FROM MAP";

	// Token: 0x04002F2C RID: 12076
	[SerializeField]
	private string subscribedStatusString = "SUBSCRIBED";

	// Token: 0x04002F2D RID: 12077
	[SerializeField]
	private string unsubscribedStatusString = "NOT SUBSCRIBED";

	// Token: 0x04002F2E RID: 12078
	[SerializeField]
	private string loadMapTooltipString = "SELECT: LOAD MAP";

	// Token: 0x04002F2F RID: 12079
	[SerializeField]
	private string downloadMapTooltipString = "SELECT: DOWNLOAD MAP";

	// Token: 0x04002F30 RID: 12080
	[SerializeField]
	private string updateMapTooltipString = "SELECT: UPDATE MAP";

	// Token: 0x04002F31 RID: 12081
	public long pendingModId;

	// Token: 0x04002F33 RID: 12083
	private bool hasModProfile;

	// Token: 0x04002F34 RID: 12084
	private bool mapLoadError;

	// Token: 0x04002F35 RID: 12085
	private const uint IO_ModFileInvalid_ErrorCode = 20460U;

	// Token: 0x04002F36 RID: 12086
	private ProgressHandle currentProgressHandle;

	// Token: 0x04002F37 RID: 12087
	private ModManagementOperationType lastCompletedOperation = ModManagementOperationType.None_ErrorOcurred;
}
