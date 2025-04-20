using System;
using GorillaExtensions;
using GorillaNetworking;
using KID.Model;
using Photon.Realtime;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000647 RID: 1607
public class GorillaPlayerScoreboardLine : MonoBehaviour
{
	// Token: 0x060027BA RID: 10170 RVA: 0x0004B088 File Offset: 0x00049288
	public void Start()
	{
		this.emptyRigCount = 0;
		this.reportedCheating = false;
		this.reportedHateSpeech = false;
		this.reportedToxicity = false;
	}

	// Token: 0x060027BB RID: 10171 RVA: 0x0010DF18 File Offset: 0x0010C118
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

	// Token: 0x060027BC RID: 10172 RVA: 0x0010E114 File Offset: 0x0010C314
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

	// Token: 0x060027BD RID: 10173 RVA: 0x0010E19C File Offset: 0x0010C39C
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

	// Token: 0x060027BE RID: 10174 RVA: 0x0010E46C File Offset: 0x0010C66C
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

	// Token: 0x060027BF RID: 10175 RVA: 0x0010E608 File Offset: 0x0010C808
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

	// Token: 0x060027C0 RID: 10176 RVA: 0x0010E6E0 File Offset: 0x0010C8E0
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

	// Token: 0x060027C1 RID: 10177 RVA: 0x0010E7EC File Offset: 0x0010C9EC
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

	// Token: 0x060027C2 RID: 10178 RVA: 0x0010E884 File Offset: 0x0010CA84
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

	// Token: 0x060027C3 RID: 10179 RVA: 0x0010E91C File Offset: 0x0010CB1C
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

	// Token: 0x060027C4 RID: 10180 RVA: 0x0004B0A6 File Offset: 0x000492A6
	public void ResetData()
	{
		this.emptyRigCount = 0;
		this.playerActorNumber = -1;
		this.linePlayer = null;
		this.playerNameValue = string.Empty;
		this.currentNickname = string.Empty;
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x0004B0D3 File Offset: 0x000492D3
	private void OnEnable()
	{
		GorillaScoreboardTotalUpdater.RegisterSL(this);
	}

	// Token: 0x060027C6 RID: 10182 RVA: 0x0004B0DB File Offset: 0x000492DB
	private void OnDisable()
	{
		GorillaScoreboardTotalUpdater.UnregisterSL(this);
	}

	// Token: 0x060027C7 RID: 10183 RVA: 0x0010E9C0 File Offset: 0x0010CBC0
	private void SwapToReportState(bool reportInProgress)
	{
		this.reportButton.gameObject.SetActive(!reportInProgress);
		this.hateSpeechButton.SetActive(reportInProgress);
		this.toxicityButton.SetActive(reportInProgress);
		this.cheatingButton.SetActive(reportInProgress);
		this.cancelButton.SetActive(reportInProgress);
	}

	// Token: 0x04002CE8 RID: 11496
	private static int[] targetActors = new int[]
	{
		-1
	};

	// Token: 0x04002CE9 RID: 11497
	public Text playerName;

	// Token: 0x04002CEA RID: 11498
	public Text playerLevel;

	// Token: 0x04002CEB RID: 11499
	public Text playerMMR;

	// Token: 0x04002CEC RID: 11500
	public Image playerSwatch;

	// Token: 0x04002CED RID: 11501
	public Texture infectedTexture;

	// Token: 0x04002CEE RID: 11502
	public NetPlayer linePlayer;

	// Token: 0x04002CEF RID: 11503
	public VRRig playerVRRig;

	// Token: 0x04002CF0 RID: 11504
	public string playerLevelValue;

	// Token: 0x04002CF1 RID: 11505
	public string playerMMRValue;

	// Token: 0x04002CF2 RID: 11506
	public string playerNameValue;

	// Token: 0x04002CF3 RID: 11507
	public string playerNameVisible;

	// Token: 0x04002CF4 RID: 11508
	public int playerActorNumber;

	// Token: 0x04002CF5 RID: 11509
	public GorillaPlayerLineButton muteButton;

	// Token: 0x04002CF6 RID: 11510
	public GorillaPlayerLineButton reportButton;

	// Token: 0x04002CF7 RID: 11511
	public GameObject hateSpeechButton;

	// Token: 0x04002CF8 RID: 11512
	public GameObject toxicityButton;

	// Token: 0x04002CF9 RID: 11513
	public GameObject cheatingButton;

	// Token: 0x04002CFA RID: 11514
	public GameObject cancelButton;

	// Token: 0x04002CFB RID: 11515
	public SpriteRenderer speakerIcon;

	// Token: 0x04002CFC RID: 11516
	public bool canPressNextReportButton = true;

	// Token: 0x04002CFD RID: 11517
	public Text[] texts;

	// Token: 0x04002CFE RID: 11518
	public SpriteRenderer[] sprites;

	// Token: 0x04002CFF RID: 11519
	public MeshRenderer[] meshes;

	// Token: 0x04002D00 RID: 11520
	public Image[] images;

	// Token: 0x04002D01 RID: 11521
	private Recorder myRecorder;

	// Token: 0x04002D02 RID: 11522
	private bool isMuteManual;

	// Token: 0x04002D03 RID: 11523
	private int mute;

	// Token: 0x04002D04 RID: 11524
	private int emptyRigCount;

	// Token: 0x04002D05 RID: 11525
	public GameObject myRig;

	// Token: 0x04002D06 RID: 11526
	public bool reportedCheating;

	// Token: 0x04002D07 RID: 11527
	public bool reportedToxicity;

	// Token: 0x04002D08 RID: 11528
	public bool reportedHateSpeech;

	// Token: 0x04002D09 RID: 11529
	public bool reportInProgress;

	// Token: 0x04002D0A RID: 11530
	private string currentNickname;

	// Token: 0x04002D0B RID: 11531
	public bool doneReporting;

	// Token: 0x04002D0C RID: 11532
	public bool lastVisible = true;

	// Token: 0x04002D0D RID: 11533
	public GorillaScoreBoard parentScoreboard;

	// Token: 0x04002D0E RID: 11534
	public float initTime;

	// Token: 0x04002D0F RID: 11535
	public float emptyRigCooldown = 10f;

	// Token: 0x04002D10 RID: 11536
	internal RigContainer rigContainer;
}
