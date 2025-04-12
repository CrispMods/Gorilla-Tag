using System;
using System.Collections.Generic;
using GorillaTagScripts.ModIO;
using ModIO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x0200067F RID: 1663
public class ModIOModDetailsScreen : ModIOScreen
{
	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06002943 RID: 10563 RVA: 0x0004B2C7 File Offset: 0x000494C7
	// (set) Token: 0x06002942 RID: 10562 RVA: 0x0004B2BE File Offset: 0x000494BE
	public ModProfile currentModProfile { get; private set; }

	// Token: 0x06002944 RID: 10564 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void Initialize()
	{
	}

	// Token: 0x06002945 RID: 10565 RVA: 0x001130B0 File Offset: 0x001112B0
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
		this.ResetToDefaultView();
	}

	// Token: 0x06002946 RID: 10566 RVA: 0x001131A8 File Offset: 0x001113A8
	public override void Hide()
	{
		base.Hide();
		GameEvents.ModIOModManagementEvent.RemoveListener(new UnityAction<ModManagementEventType, ModId, Result>(this.HandleModManagementEvent));
		CustomMapManager.OnMapLoadStatusChanged.RemoveListener(new UnityAction<MapLoadStatus, int, string>(this.OnMapLoadProgress));
		CustomMapManager.OnMapLoadComplete.RemoveListener(new UnityAction<bool>(this.OnMapLoadComplete));
		CustomMapManager.OnRoomMapChanged.RemoveListener(new UnityAction<ModId>(this.OnRoomMapChanged));
		CustomMapManager.OnMapUnloadComplete.RemoveListener(new UnityAction(this.OnMapUnloaded));
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x0011322C File Offset: 0x0011142C
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

	// Token: 0x06002948 RID: 10568 RVA: 0x001132E4 File Offset: 0x001114E4
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
			this.currentProgressHandle = null;
			this.UpdateSubscriptionStatus(false);
		}
	}

	// Token: 0x06002949 RID: 10569 RVA: 0x001133D8 File Offset: 0x001115D8
	public void RetrieveProfileFromModIO(long id, Action<ModIORequestResultAnd<ModProfile>> callback = null)
	{
		if (this.hasModProfile && this.currentModProfile.id == id)
		{
			this.UpdateModDetails(true);
			return;
		}
		this.pendingModId = id;
		ModIODataStore.GetModProfile(new ModId(id), (callback != null) ? callback : new Action<ModIORequestResultAnd<ModProfile>>(this.OnProfileReceived));
	}

	// Token: 0x0600294A RID: 10570 RVA: 0x0004B2CF File Offset: 0x000494CF
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

	// Token: 0x0600294B RID: 10571 RVA: 0x0011342C File Offset: 0x0011162C
	protected override void PressButton(ModIOKeyboardButton.ModIOKeyboardBindings buttonPressed)
	{
		if (!base.isActiveAndEnabled || !ModIOMapsTerminal.IsDriver)
		{
			return;
		}
		if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.goback)
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
			if (!ModIOMapLoader.IsModLoaded(0L) && !(CustomMapManager.GetRoomMapId() != ModId.Null))
			{
				ModIOMapsTerminal.ReturnFromDetailsScreen();
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
			if (buttonPressed != ModIOKeyboardButton.ModIOKeyboardBindings.option3)
			{
				if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.option1)
				{
					if (ModIOMapLoader.IsModLoaded(0L) || CustomMapManager.IsLoading(0L) || CustomMapManager.IsUnloading())
					{
						return;
					}
					this.errorText.gameObject.SetActive(false);
					this.errorText.text = "";
					this.loadingMapLabelText.gameObject.SetActive(false);
					this.loadingMapMessageText.gameObject.SetActive(false);
					this.modDescriptionText.gameObject.SetActive(true);
					ModIODataStore.Refresh(delegate(bool result)
					{
						SubscribedModStatus subscribedModStatus2;
						if (ModIODataStore.GetSubscribedModStatus(this.currentModProfile.id, out subscribedModStatus2))
						{
							ModIODataStore.UnsubscribeFromMod(this.currentModProfile.id, delegate(Result result)
							{
								if (result.Succeeded())
								{
									this.UpdateModDetails(false);
								}
							});
							return;
						}
						ModIODataStore.SubscribeToMod(this.currentModProfile.id, delegate(Result result)
						{
							if (result.Succeeded())
							{
								this.UpdateModDetails(false);
								ModIODataStore.DownloadMod(this.currentModProfile.id, delegate(ModIORequestResult result)
								{
									GorillaTelemetry.PostCustomMapDownloadEvent(this.currentModProfile.name, this.currentModProfile.id, this.currentModProfile.creator.username);
									this.UpdateModDetails(false);
								});
							}
						});
					}, false);
				}
				SubscribedModStatus subscribedModStatus;
				if (buttonPressed == ModIOKeyboardButton.ModIOKeyboardBindings.enter && !CustomMapManager.IsLoading(0L) && !CustomMapManager.IsUnloading() && !ModIOMapLoader.IsModLoaded(0L) && ModIODataStore.GetSubscribedModStatus(this.currentModProfile.id, out subscribedModStatus))
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
					else if (subscribedModStatus == SubscribedModStatus.WaitingToDownload || subscribedModStatus == SubscribedModStatus.WaitingToUpdate)
					{
						ModIODataStore.DownloadMod(this.currentModProfile.id, delegate(ModIORequestResult result)
						{
							this.UpdateSubscriptionStatus(!result.success);
							GorillaTelemetry.PostCustomMapDownloadEvent(this.currentModProfile.name, this.currentModProfile.id, this.currentModProfile.creator.username);
						});
					}
				}
				return;
			}
			if (ModIOMapLoader.IsModLoaded(0L) || CustomMapManager.IsLoading(0L) || CustomMapManager.IsUnloading() || CustomMapManager.GetRoomMapId() != ModId.Null)
			{
				return;
			}
			if (this.hasModProfile)
			{
				long currentModId = this.currentModProfile.id.id;
				this.hasModProfile = false;
				this.currentModProfile = default(ModProfile);
				ModIODataStore.Refresh(delegate(bool result)
				{
					this.RetrieveProfileFromModIO(currentModId, null);
				}, false);
			}
			return;
		}
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x001136AC File Offset: 0x001118AC
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

	// Token: 0x0600294D RID: 10573 RVA: 0x00113708 File Offset: 0x00111908
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
		if (ModIOMapLoader.IsModLoaded(0L) || CustomMapManager.IsLoading(0L) || CustomMapManager.IsUnloading())
		{
			ModId modId = new ModId(ModIOMapLoader.IsModLoaded(0L) ? ModIOMapLoader.LoadedMapModId : (CustomMapManager.IsLoading(0L) ? CustomMapManager.LoadingMapId : CustomMapManager.UnloadingMapId));
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

	// Token: 0x0600294E RID: 10574 RVA: 0x001138C4 File Offset: 0x00111AC4
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
			if (ModIOMapLoader.IsModLoaded(0L))
			{
				ModId modId = new ModId(ModIOMapLoader.LoadedMapModId);
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

	// Token: 0x0600294F RID: 10575 RVA: 0x00113B40 File Offset: 0x00111D40
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

	// Token: 0x06002950 RID: 10576 RVA: 0x00113BB0 File Offset: 0x00111DB0
	private void UpdateSubscriptionStatus(bool errorEncountered = false)
	{
		if (base.isActiveAndEnabled)
		{
			bool flag = this.mapLoadError || CustomMapManager.IsUnloading() || CustomMapManager.IsLoading(0L) || ModIOMapLoader.IsModLoaded(0L) || CustomMapManager.GetRoomMapId() != ModId.Null;
			this.outdatedText.gameObject.SetActive(false);
			if (!flag)
			{
				SubscribedModStatus key;
				bool subscribedModStatus = ModIODataStore.GetSubscribedModStatus(this.currentModProfile.id, out key);
				if (errorEncountered)
				{
					key = SubscribedModStatus.ProblemOccurred;
				}
				this.modSubscriptionStatusText.text = (subscribedModStatus ? this.subscribedStatusString : this.unsubscribedStatusString);
				if (subscribedModStatus)
				{
					switch (key)
					{
					case SubscribedModStatus.Installed:
					{
						this.selectButtonTooltipText.text = this.loadMapTooltipString;
						this.selectButtonTooltipText.gameObject.SetActive(true);
						int num;
						if (ModIODataStore.IsModOutdated(this.currentModProfile.id, out num))
						{
							this.outdatedText.gameObject.SetActive(true);
							goto IL_15F;
						}
						this.outdatedText.gameObject.SetActive(false);
						goto IL_15F;
					}
					case SubscribedModStatus.WaitingToDownload:
						this.selectButtonTooltipText.text = this.downloadMapTooltipString;
						this.selectButtonTooltipText.gameObject.SetActive(true);
						goto IL_15F;
					case SubscribedModStatus.WaitingToUpdate:
						this.selectButtonTooltipText.text = this.updateMapTooltipString;
						this.selectButtonTooltipText.gameObject.SetActive(true);
						goto IL_15F;
					}
					this.selectButtonTooltipText.gameObject.SetActive(false);
					IL_15F:
					string text;
					if (this.modStatusStrings.TryGetValue(key, out text))
					{
						this.modStatusText.text = text;
					}
					else
					{
						this.modStatusText.text = "STATUS STRING MISSING!";
					}
					if (ModIODataStore.IsModInDownloadQueue(this.currentModProfile.id))
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
			this.selectButtonTooltipText.gameObject.SetActive(false);
			this.modStatusText.gameObject.SetActive(false);
			this.OptionButtonTooltipText.gameObject.SetActive(false);
			this.modSubscriptionStatusText.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002951 RID: 10577 RVA: 0x00113E3C File Offset: 0x0011203C
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

	// Token: 0x06002952 RID: 10578 RVA: 0x00113EC0 File Offset: 0x001120C0
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

	// Token: 0x06002953 RID: 10579 RVA: 0x0004B300 File Offset: 0x00049500
	private void UnloadMod()
	{
		this.networkObject.UnloadModSynced();
	}

	// Token: 0x06002954 RID: 10580 RVA: 0x0004B30D File Offset: 0x0004950D
	public void OnMapLoadComplete(bool success)
	{
		if (success)
		{
			this.OnMapLoadComplete_UIUpdate();
			return;
		}
		this.mapLoadError = true;
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x00113F84 File Offset: 0x00112184
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

	// Token: 0x06002956 RID: 10582 RVA: 0x0004B320 File Offset: 0x00049520
	private void OnMapUnloaded()
	{
		this.mapLoadError = false;
		this.UpdateModDetails(true);
	}

	// Token: 0x06002957 RID: 10583 RVA: 0x00114008 File Offset: 0x00112208
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

	// Token: 0x06002958 RID: 10584 RVA: 0x0004B330 File Offset: 0x00049530
	private void OnRoomMapProfileReceived(ModIORequestResultAnd<ModProfile> result)
	{
		this.OnProfileReceived(result);
		if (result.result.success)
		{
			this.ShowLoadRoomMapPrompt();
		}
	}

	// Token: 0x06002959 RID: 10585 RVA: 0x0011405C File Offset: 0x0011225C
	private void ShowLoadRoomMapPrompt()
	{
		if (CustomMapManager.IsUnloading() || CustomMapManager.IsLoading(0L) || ModIOMapLoader.IsModLoaded(this.currentModProfile.id))
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

	// Token: 0x0600295A RID: 10586 RVA: 0x001140F8 File Offset: 0x001122F8
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
			if (ModIOMapsTerminal.IsDriver)
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

	// Token: 0x0600295B RID: 10587 RVA: 0x0004B34C File Offset: 0x0004954C
	public long GetModId()
	{
		return this.currentModProfile.id.id;
	}

	// Token: 0x04002E71 RID: 11889
	[SerializeField]
	private SpriteRenderer mapScreenshotImage;

	// Token: 0x04002E72 RID: 11890
	[SerializeField]
	private TMP_Text loadingText;

	// Token: 0x04002E73 RID: 11891
	[SerializeField]
	private TMP_Text modNameText;

	// Token: 0x04002E74 RID: 11892
	[SerializeField]
	private TMP_Text modCreatorLabelText;

	// Token: 0x04002E75 RID: 11893
	[SerializeField]
	private TMP_Text modCreatorText;

	// Token: 0x04002E76 RID: 11894
	[SerializeField]
	private TMP_Text modDescriptionText;

	// Token: 0x04002E77 RID: 11895
	[SerializeField]
	private TMP_Text selectButtonTooltipText;

	// Token: 0x04002E78 RID: 11896
	[SerializeField]
	private TMP_Text OptionButtonTooltipText;

	// Token: 0x04002E79 RID: 11897
	[SerializeField]
	private TMP_Text modStatusText;

	// Token: 0x04002E7A RID: 11898
	[SerializeField]
	private TMP_Text modSubscriptionStatusText;

	// Token: 0x04002E7B RID: 11899
	[SerializeField]
	private TMP_Text loadingMapLabelText;

	// Token: 0x04002E7C RID: 11900
	[SerializeField]
	private TMP_Text loadingMapMessageText;

	// Token: 0x04002E7D RID: 11901
	[SerializeField]
	private TMP_Text loadRoomMapPromptText;

	// Token: 0x04002E7E RID: 11902
	[SerializeField]
	private TMP_Text mapReadyText;

	// Token: 0x04002E7F RID: 11903
	[SerializeField]
	private TMP_Text unloadPromptText;

	// Token: 0x04002E80 RID: 11904
	[SerializeField]
	private TMP_Text errorText;

	// Token: 0x04002E81 RID: 11905
	[SerializeField]
	private TMP_Text outdatedText;

	// Token: 0x04002E82 RID: 11906
	[SerializeField]
	private string modAvailableString = "AVAILABLE";

	// Token: 0x04002E83 RID: 11907
	[SerializeField]
	private string mapAutoDownloadingString = "DOWNLOADING...";

	// Token: 0x04002E84 RID: 11908
	[SerializeField]
	private string mapDownloadQueuedString = "DOWNLOAD QUEUED";

	// Token: 0x04002E85 RID: 11909
	[SerializeField]
	private string mapLoadingString = "LOADING:";

	// Token: 0x04002E86 RID: 11910
	[SerializeField]
	private string mapUnloadingString = "UNLOADING...";

	// Token: 0x04002E87 RID: 11911
	[SerializeField]
	private string mapLoadingErrorString = "ERROR:";

	// Token: 0x04002E88 RID: 11912
	[SerializeField]
	private string mapLoadingErrorDriverString = "PRESS THE 'BACK' BUTTON TO TRY AGAIN";

	// Token: 0x04002E89 RID: 11913
	[SerializeField]
	private string mapLoadingErrorNonDriverString = "LEAVE AND REJOIN THE VIRTUAL STUMP TO TRY AGAIN";

	// Token: 0x04002E8A RID: 11914
	[SerializeField]
	private string mapLoadingErrorInvalidModFile = "INSTALL FAILED DUE TO INVALID MAP FILE";

	// Token: 0x04002E8B RID: 11915
	[SerializeField]
	private ModIOSerializer networkObject;

	// Token: 0x04002E8C RID: 11916
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

	// Token: 0x04002E8D RID: 11917
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

	// Token: 0x04002E8E RID: 11918
	[FormerlySerializedAs("unsubscribedTooltipString")]
	[SerializeField]
	private string subscribeTooltipString = "OPTION: SUBSCRIBE TO MAP";

	// Token: 0x04002E8F RID: 11919
	[FormerlySerializedAs("subscribedTooltipString")]
	[SerializeField]
	private string unsubscribeTooltipString = "OPTION: UNSUBSCRIBE FROM MAP";

	// Token: 0x04002E90 RID: 11920
	[SerializeField]
	private string subscribedStatusString = "SUBSCRIBED";

	// Token: 0x04002E91 RID: 11921
	[SerializeField]
	private string unsubscribedStatusString = "NOT SUBSCRIBED";

	// Token: 0x04002E92 RID: 11922
	[SerializeField]
	private string loadMapTooltipString = "SELECT: LOAD MAP";

	// Token: 0x04002E93 RID: 11923
	[SerializeField]
	private string downloadMapTooltipString = "SELECT: DOWNLOAD MAP";

	// Token: 0x04002E94 RID: 11924
	[SerializeField]
	private string updateMapTooltipString = "SELECT: UPDATE MAP";

	// Token: 0x04002E95 RID: 11925
	public long pendingModId;

	// Token: 0x04002E97 RID: 11927
	private bool hasModProfile;

	// Token: 0x04002E98 RID: 11928
	private bool mapLoadError;

	// Token: 0x04002E99 RID: 11929
	private const uint IO_ModFileInvalid_ErrorCode = 20460U;

	// Token: 0x04002E9A RID: 11930
	private ProgressHandle currentProgressHandle;
}
