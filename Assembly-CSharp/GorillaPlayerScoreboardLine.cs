using System;
using GorillaExtensions;
using GorillaNetworking;
using KID.Model;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000668 RID: 1640
public class GorillaPlayerScoreboardLine : MonoBehaviour
{
	// Token: 0x0600288F RID: 10383 RVA: 0x000C70AA File Offset: 0x000C52AA
	public void Start()
	{
		this.emptyRigCount = 0;
		this.reportedCheating = false;
		this.reportedHateSpeech = false;
		this.reportedToxicity = false;
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x000C70C8 File Offset: 0x000C52C8
	public void InitializeLine()
	{
		this.currentNickname = string.Empty;
		this.UpdatePlayerText();
		if (this.linePlayer == NetworkSystem.Instance.LocalPlayer)
		{
			this.muteButton.gameObject.SetActive(false);
			this.reportButton.gameObject.SetActive(false);
			this.hateSpeechButton.SetActive(false);
			this.toxicityButton.SetActive(false);
			this.cheatingButton.SetActive(false);
			this.cancelButton.SetActive(false);
			return;
		}
		this.muteButton.gameObject.SetActive(true);
		if (GorillaScoreboardTotalUpdater.instance != null && GorillaScoreboardTotalUpdater.instance.reportDict.ContainsKey(this.playerActorNumber))
		{
			GorillaScoreboardTotalUpdater.PlayerReports playerReports = GorillaScoreboardTotalUpdater.instance.reportDict[this.playerActorNumber];
			this.reportedCheating = playerReports.cheating;
			this.reportedHateSpeech = playerReports.hateSpeech;
			this.reportedToxicity = playerReports.toxicity;
			this.reportInProgress = playerReports.pressedReport;
		}
		else
		{
			this.reportedCheating = false;
			this.reportedHateSpeech = false;
			this.reportedToxicity = false;
			this.reportInProgress = false;
		}
		this.reportButton.isOn = (this.reportedCheating || this.reportedHateSpeech || this.reportedToxicity);
		this.reportButton.UpdateColor();
		this.SwapToReportState(this.reportInProgress);
		this.muteButton.gameObject.SetActive(true);
		this.isMuteManual = PlayerPrefs.HasKey(this.linePlayer.UserId);
		this.mute = PlayerPrefs.GetInt(this.linePlayer.UserId, 0);
		this.muteButton.isOn = (this.mute != 0);
		this.muteButton.isAutoOn = false;
		this.muteButton.UpdateColor();
		if (this.rigContainer != null)
		{
			this.rigContainer.hasManualMute = this.isMuteManual;
			this.rigContainer.Muted = (this.mute != 0);
		}
	}

	// Token: 0x06002891 RID: 10385 RVA: 0x000C72C4 File Offset: 0x000C54C4
	public void SetLineData(NetPlayer netPlayer)
	{
		if (!netPlayer.InRoom || netPlayer == this.linePlayer)
		{
			return;
		}
		if (this.playerActorNumber != netPlayer.ActorNumber)
		{
			this.initTime = Time.time;
		}
		this.playerActorNumber = netPlayer.ActorNumber;
		this.linePlayer = netPlayer;
		this.playerNameValue = (netPlayer.NickName ?? "");
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(netPlayer, out rigContainer))
		{
			this.rigContainer = rigContainer;
			this.playerVRRig = rigContainer.Rig;
		}
		this.InitializeLine();
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x000C734C File Offset: 0x000C554C
	public void UpdateLine()
	{
		if (this.linePlayer != null)
		{
			if (this.playerNameVisible != this.playerVRRig.playerNameVisible)
			{
				this.UpdatePlayerText();
				this.parentScoreboard.IsDirty = true;
			}
			if (this.rigContainer != null)
			{
				if (Time.time > this.initTime + this.emptyRigCooldown)
				{
					if (this.playerVRRig.netView != null)
					{
						this.emptyRigCount = 0;
					}
					else
					{
						this.emptyRigCount++;
						if (this.emptyRigCount > 30)
						{
							GorillaNot.instance.SendReport("empty rig", this.linePlayer.UserId, this.linePlayer.NickName);
						}
					}
				}
				Material material;
				if (this.playerVRRig.setMatIndex == 0)
				{
					material = this.playerVRRig.scoreboardMaterial;
				}
				else
				{
					material = this.playerVRRig.materialsToChangeTo[this.playerVRRig.setMatIndex];
				}
				if (this.playerSwatch.material != material)
				{
					this.playerSwatch.material = material;
				}
				if (this.playerSwatch.color != this.playerVRRig.materialsToChangeTo[0].color)
				{
					this.playerSwatch.color = this.playerVRRig.materialsToChangeTo[0].color;
				}
				if (this.myRecorder == null)
				{
					this.myRecorder = NetworkSystem.Instance.LocalRecorder;
				}
				if (this.playerVRRig != null)
				{
					if (this.playerVRRig.remoteUseReplacementVoice || this.playerVRRig.localUseReplacementVoice || GorillaComputer.instance.voiceChatOn == "FALSE")
					{
						if (this.playerVRRig.SpeakingLoudness > this.playerVRRig.replacementVoiceLoudnessThreshold && !this.rigContainer.ForceMute && !this.rigContainer.Muted)
						{
							this.speakerIcon.enabled = true;
						}
						else
						{
							this.speakerIcon.enabled = false;
						}
					}
					else if ((this.rigContainer.Voice != null && this.rigContainer.Voice.IsSpeaking) || (this.playerVRRig.rigSerializer != null && this.playerVRRig.rigSerializer.IsLocallyOwned && this.myRecorder != null && this.myRecorder.IsCurrentlyTransmitting))
					{
						this.speakerIcon.enabled = true;
					}
					else
					{
						this.speakerIcon.enabled = false;
					}
				}
				else
				{
					this.speakerIcon.enabled = false;
				}
				if (!this.isMuteManual)
				{
					bool isPlayerAutoMuted = this.rigContainer.GetIsPlayerAutoMuted();
					if (this.muteButton.isAutoOn != isPlayerAutoMuted)
					{
						this.muteButton.isAutoOn = isPlayerAutoMuted;
						this.muteButton.UpdateColor();
					}
				}
			}
		}
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x000C761C File Offset: 0x000C581C
	private void UpdatePlayerText()
	{
		try
		{
			if (this.rigContainer.IsNull() || this.playerVRRig.IsNull())
			{
				this.playerNameVisible = this.NormalizeName(this.linePlayer.NickName != this.currentNickname, this.linePlayer.NickName);
				this.currentNickname = this.linePlayer.NickName;
			}
			else if (this.rigContainer.Initialized)
			{
				this.playerNameVisible = this.playerVRRig.playerNameVisible;
			}
			else if (this.currentNickname.IsNullOrEmpty() || GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(this.linePlayer.UserId))
			{
				this.playerNameVisible = this.NormalizeName(this.linePlayer.NickName != this.currentNickname, this.linePlayer.NickName);
			}
			ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
			bool flag = (customNicknamePermissionStatus.Item1 || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
			this.currentNickname = this.linePlayer.NickName;
			this.playerName.text = (flag ? this.playerNameVisible : this.linePlayer.DefaultName);
		}
		catch (Exception)
		{
			this.playerNameVisible = this.linePlayer.DefaultName;
			GorillaNot.instance.SendReport("NmError", this.linePlayer.UserId, this.linePlayer.NickName);
		}
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x000C77B8 File Offset: 0x000C59B8
	public void PressButton(bool isOn, GorillaPlayerLineButton.ButtonType buttonType)
	{
		if (buttonType != GorillaPlayerLineButton.ButtonType.Mute)
		{
			if (buttonType == GorillaPlayerLineButton.ButtonType.Report)
			{
				this.SetReportState(true, buttonType);
				return;
			}
			this.SetReportState(false, buttonType);
		}
		else if (this.linePlayer != null && this.playerVRRig != null)
		{
			this.isMuteManual = true;
			this.muteButton.isAutoOn = false;
			this.mute = (isOn ? 1 : 0);
			PlayerPrefs.SetInt(this.linePlayer.UserId, this.mute);
			if (this.rigContainer != null)
			{
				this.rigContainer.hasManualMute = this.isMuteManual;
				this.rigContainer.Muted = (this.mute != 0);
			}
			PlayerPrefs.Save();
			this.muteButton.UpdateColor();
			GorillaScoreboardTotalUpdater.ReportMute(this.linePlayer, this.mute);
			return;
		}
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x000C7890 File Offset: 0x000C5A90
	public void SetReportState(bool reportState, GorillaPlayerLineButton.ButtonType buttonType)
	{
		this.canPressNextReportButton = (buttonType != GorillaPlayerLineButton.ButtonType.Toxicity && buttonType != GorillaPlayerLineButton.ButtonType.Report);
		this.reportInProgress = reportState;
		if (reportState)
		{
			this.SwapToReportState(true);
		}
		else
		{
			this.SwapToReportState(false);
			if (this.linePlayer != null && buttonType != GorillaPlayerLineButton.ButtonType.Cancel)
			{
				if ((!this.reportedHateSpeech && buttonType == GorillaPlayerLineButton.ButtonType.HateSpeech) || (!this.reportedToxicity && buttonType == GorillaPlayerLineButton.ButtonType.Toxicity) || (!this.reportedCheating && buttonType == GorillaPlayerLineButton.ButtonType.Cheating))
				{
					GorillaPlayerScoreboardLine.ReportPlayer(this.linePlayer.UserId, buttonType, this.playerNameVisible);
					this.doneReporting = true;
				}
				this.reportedCheating = (this.reportedCheating || buttonType == GorillaPlayerLineButton.ButtonType.Cheating);
				this.reportedToxicity = (this.reportedToxicity || buttonType == GorillaPlayerLineButton.ButtonType.Toxicity);
				this.reportedHateSpeech = (this.reportedHateSpeech || buttonType == GorillaPlayerLineButton.ButtonType.HateSpeech);
				this.reportButton.isOn = true;
				this.reportButton.UpdateColor();
			}
		}
		if (GorillaScoreboardTotalUpdater.instance != null)
		{
			GorillaScoreboardTotalUpdater.instance.UpdateLineState(this);
		}
		this.parentScoreboard.RedrawPlayerLines();
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x000C799C File Offset: 0x000C5B9C
	public static void ReportPlayer(string PlayerID, GorillaPlayerLineButton.ButtonType buttonType, string OtherPlayerNickName)
	{
		if (OtherPlayerNickName.Length > 12)
		{
			OtherPlayerNickName.Remove(12);
		}
		WebFlags flags = new WebFlags(1);
		NetEventOptions options = new NetEventOptions
		{
			Flags = flags,
			TargetActors = GorillaPlayerScoreboardLine.targetActors
		};
		byte code = 50;
		object[] data = new object[]
		{
			PlayerID,
			buttonType,
			OtherPlayerNickName,
			NetworkSystem.Instance.LocalPlayer.NickName,
			!NetworkSystem.Instance.SessionIsPrivate,
			NetworkSystem.Instance.RoomStringStripped()
		};
		NetworkSystemRaiseEvent.RaiseEvent(code, data, options, true);
	}

	// Token: 0x06002897 RID: 10391 RVA: 0x000C7A34 File Offset: 0x000C5C34
	public static void MutePlayer(string PlayerID, string OtherPlayerNickName, int muting)
	{
		if (OtherPlayerNickName.Length > 12)
		{
			OtherPlayerNickName.Remove(12);
		}
		WebFlags flags = new WebFlags(1);
		NetEventOptions options = new NetEventOptions
		{
			Flags = flags,
			TargetActors = GorillaPlayerScoreboardLine.targetActors
		};
		byte code = 51;
		object[] data = new object[]
		{
			PlayerID,
			muting,
			OtherPlayerNickName,
			NetworkSystem.Instance.LocalPlayer.NickName,
			!NetworkSystem.Instance.SessionIsPrivate,
			NetworkSystem.Instance.RoomStringStripped()
		};
		NetworkSystemRaiseEvent.RaiseEvent(code, data, options, true);
	}

	// Token: 0x06002898 RID: 10392 RVA: 0x000C7ACC File Offset: 0x000C5CCC
	public string NormalizeName(bool doIt, string text)
	{
		if (doIt)
		{
			if (GorillaComputer.instance.CheckAutoBanListForName(text))
			{
				text = new string(Array.FindAll<char>(text.ToCharArray(), (char c) => Utils.IsASCIILetterOrDigit(c)));
				if (text.Length > 12)
				{
					text = text.Substring(0, 11);
				}
				text = text.ToUpper();
			}
			else
			{
				text = "BADGORILLA";
				GorillaNot.instance.SendReport("evading the name ban", this.linePlayer.UserId, this.linePlayer.NickName);
			}
		}
		return text;
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x000C7B6D File Offset: 0x000C5D6D
	public void ResetData()
	{
		this.emptyRigCount = 0;
		this.playerActorNumber = -1;
		this.linePlayer = null;
		this.playerNameValue = string.Empty;
		this.currentNickname = string.Empty;
	}

	// Token: 0x0600289A RID: 10394 RVA: 0x000C7B9A File Offset: 0x000C5D9A
	private void OnEnable()
	{
		GorillaScoreboardTotalUpdater.RegisterSL(this);
	}

	// Token: 0x0600289B RID: 10395 RVA: 0x000C7BA2 File Offset: 0x000C5DA2
	private void OnDisable()
	{
		GorillaScoreboardTotalUpdater.UnregisterSL(this);
	}

	// Token: 0x0600289C RID: 10396 RVA: 0x000C7BAC File Offset: 0x000C5DAC
	private void SwapToReportState(bool reportInProgress)
	{
		this.reportButton.gameObject.SetActive(!reportInProgress);
		this.hateSpeechButton.SetActive(reportInProgress);
		this.toxicityButton.SetActive(reportInProgress);
		this.cheatingButton.SetActive(reportInProgress);
		this.cancelButton.SetActive(reportInProgress);
	}

	// Token: 0x04002D82 RID: 11650
	private static int[] targetActors = new int[]
	{
		-1
	};

	// Token: 0x04002D83 RID: 11651
	public Text playerName;

	// Token: 0x04002D84 RID: 11652
	public Text playerLevel;

	// Token: 0x04002D85 RID: 11653
	public Text playerMMR;

	// Token: 0x04002D86 RID: 11654
	public Image playerSwatch;

	// Token: 0x04002D87 RID: 11655
	public Texture infectedTexture;

	// Token: 0x04002D88 RID: 11656
	public NetPlayer linePlayer;

	// Token: 0x04002D89 RID: 11657
	public VRRig playerVRRig;

	// Token: 0x04002D8A RID: 11658
	public string playerLevelValue;

	// Token: 0x04002D8B RID: 11659
	public string playerMMRValue;

	// Token: 0x04002D8C RID: 11660
	public string playerNameValue;

	// Token: 0x04002D8D RID: 11661
	public string playerNameVisible;

	// Token: 0x04002D8E RID: 11662
	public int playerActorNumber;

	// Token: 0x04002D8F RID: 11663
	public GorillaPlayerLineButton muteButton;

	// Token: 0x04002D90 RID: 11664
	public GorillaPlayerLineButton reportButton;

	// Token: 0x04002D91 RID: 11665
	public GameObject hateSpeechButton;

	// Token: 0x04002D92 RID: 11666
	public GameObject toxicityButton;

	// Token: 0x04002D93 RID: 11667
	public GameObject cheatingButton;

	// Token: 0x04002D94 RID: 11668
	public GameObject cancelButton;

	// Token: 0x04002D95 RID: 11669
	public SpriteRenderer speakerIcon;

	// Token: 0x04002D96 RID: 11670
	public bool canPressNextReportButton = true;

	// Token: 0x04002D97 RID: 11671
	public Text[] texts;

	// Token: 0x04002D98 RID: 11672
	public SpriteRenderer[] sprites;

	// Token: 0x04002D99 RID: 11673
	public MeshRenderer[] meshes;

	// Token: 0x04002D9A RID: 11674
	public Image[] images;

	// Token: 0x04002D9B RID: 11675
	private Recorder myRecorder;

	// Token: 0x04002D9C RID: 11676
	private bool isMuteManual;

	// Token: 0x04002D9D RID: 11677
	private int mute;

	// Token: 0x04002D9E RID: 11678
	private int emptyRigCount;

	// Token: 0x04002D9F RID: 11679
	public GameObject myRig;

	// Token: 0x04002DA0 RID: 11680
	public bool reportedCheating;

	// Token: 0x04002DA1 RID: 11681
	public bool reportedToxicity;

	// Token: 0x04002DA2 RID: 11682
	public bool reportedHateSpeech;

	// Token: 0x04002DA3 RID: 11683
	public bool reportInProgress;

	// Token: 0x04002DA4 RID: 11684
	private string currentNickname;

	// Token: 0x04002DA5 RID: 11685
	public bool doneReporting;

	// Token: 0x04002DA6 RID: 11686
	public bool lastVisible = true;

	// Token: 0x04002DA7 RID: 11687
	public GorillaScoreBoard parentScoreboard;

	// Token: 0x04002DA8 RID: 11688
	public float initTime;

	// Token: 0x04002DA9 RID: 11689
	public float emptyRigCooldown = 10f;

	// Token: 0x04002DAA RID: 11690
	internal RigContainer rigContainer;
}
