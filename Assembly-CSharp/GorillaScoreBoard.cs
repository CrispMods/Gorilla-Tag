using System;
using System.Collections.Generic;
using System.Text;
using GorillaGameModes;
using KID.Model;
using TMPro;
using UnityEngine;

// Token: 0x0200059B RID: 1435
public class GorillaScoreBoard : MonoBehaviour
{
	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06002389 RID: 9097 RVA: 0x00047FD9 File Offset: 0x000461D9
	// (set) Token: 0x0600238A RID: 9098 RVA: 0x00047FF0 File Offset: 0x000461F0
	public bool IsDirty
	{
		get
		{
			return this._isDirty || string.IsNullOrEmpty(this.initialGameMode);
		}
		set
		{
			this._isDirty = value;
		}
	}

	// Token: 0x0600238B RID: 9099 RVA: 0x00047FF9 File Offset: 0x000461F9
	public void SetSleepState(bool awake)
	{
		this.boardText.enabled = awake;
		this.buttonText.enabled = awake;
		if (this.linesParent != null)
		{
			this.linesParent.SetActive(awake);
		}
	}

	// Token: 0x0600238C RID: 9100 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnDestroy()
	{
	}

	// Token: 0x0600238D RID: 9101 RVA: 0x0004802D File Offset: 0x0004622D
	public string GetBeginningString()
	{
		return "ROOM ID: " + (NetworkSystem.Instance.SessionIsPrivate ? "-PRIVATE- GAME: " : (NetworkSystem.Instance.RoomName + "   GAME: ")) + this.RoomType() + "\n  PLAYER     COLOR  MUTE   REPORT";
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x000FDA7C File Offset: 0x000FBC7C
	public string RoomType()
	{
		this.initialGameMode = RoomSystem.RoomGameMode;
		this.gmNames = GameMode.gameModeNames;
		this.gmName = "ERROR";
		int count = this.gmNames.Count;
		for (int i = 0; i < count; i++)
		{
			this.tempGmName = this.gmNames[i];
			if (this.initialGameMode.Contains(this.tempGmName))
			{
				this.gmName = this.tempGmName;
				break;
			}
		}
		return this.gmName;
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000FDAFC File Offset: 0x000FBCFC
	public void RedrawPlayerLines()
	{
		this.stringBuilder.Clear();
		this.stringBuilder.Append(this.GetBeginningString());
		this.buttonStringBuilder.Clear();
		ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
		bool flag = (customNicknamePermissionStatus.Item1 || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
		for (int i = 0; i < this.lines.Count; i++)
		{
			try
			{
				if (this.lines[i].gameObject.activeInHierarchy)
				{
					this.lines[i].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, (float)(this.startingYValue - this.lineHeight * i), 0f);
					if (this.lines[i].linePlayer != null && this.lines[i].linePlayer.InRoom)
					{
						this.stringBuilder.Append("\n ");
						this.stringBuilder.Append(flag ? this.lines[i].playerNameVisible : this.lines[i].linePlayer.DefaultName);
						if (this.lines[i].linePlayer != NetworkSystem.Instance.LocalPlayer)
						{
							if (this.lines[i].reportButton.isActiveAndEnabled)
							{
								this.buttonStringBuilder.Append("MUTE                                REPORT\n");
							}
							else
							{
								this.buttonStringBuilder.Append("MUTE                HATE SPEECH    TOXICITY     CHEATING       CANCEL\n");
							}
						}
						else
						{
							this.buttonStringBuilder.Append("\n");
						}
					}
				}
			}
			catch
			{
			}
		}
		this.boardText.text = this.stringBuilder.ToString();
		this.buttonText.text = this.buttonStringBuilder.ToString();
		this._isDirty = false;
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000FDD0C File Offset: 0x000FBF0C
	public string NormalizeName(bool doIt, string text)
	{
		if (doIt)
		{
			text = new string(Array.FindAll<char>(text.ToCharArray(), (char c) => Utils.IsASCIILetterOrDigit(c)));
			if (text.Length > 12)
			{
				text = text.Substring(0, 10);
			}
			text = text.ToUpper();
		}
		return text;
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x0004806B File Offset: 0x0004626B
	private void Start()
	{
		GorillaScoreboardTotalUpdater.RegisterScoreboard(this);
	}

	// Token: 0x06002392 RID: 9106 RVA: 0x00048073 File Offset: 0x00046273
	private void OnEnable()
	{
		GorillaScoreboardTotalUpdater.RegisterScoreboard(this);
		this._isDirty = true;
	}

	// Token: 0x06002393 RID: 9107 RVA: 0x00048082 File Offset: 0x00046282
	private void OnDisable()
	{
		GorillaScoreboardTotalUpdater.UnregisterScoreboard(this);
	}

	// Token: 0x04002715 RID: 10005
	public GameObject scoreBoardLinePrefab;

	// Token: 0x04002716 RID: 10006
	public int startingYValue;

	// Token: 0x04002717 RID: 10007
	public int lineHeight;

	// Token: 0x04002718 RID: 10008
	public bool includeMMR;

	// Token: 0x04002719 RID: 10009
	public bool isActive;

	// Token: 0x0400271A RID: 10010
	public GameObject linesParent;

	// Token: 0x0400271B RID: 10011
	[SerializeField]
	public List<GorillaPlayerScoreboardLine> lines;

	// Token: 0x0400271C RID: 10012
	public TextMeshPro boardText;

	// Token: 0x0400271D RID: 10013
	public TextMeshPro buttonText;

	// Token: 0x0400271E RID: 10014
	public bool needsUpdate;

	// Token: 0x0400271F RID: 10015
	public TextMeshPro notInRoomText;

	// Token: 0x04002720 RID: 10016
	public string initialGameMode;

	// Token: 0x04002721 RID: 10017
	private string tempGmName;

	// Token: 0x04002722 RID: 10018
	private string gmName;

	// Token: 0x04002723 RID: 10019
	private const string error = "ERROR";

	// Token: 0x04002724 RID: 10020
	private List<string> gmNames;

	// Token: 0x04002725 RID: 10021
	private bool _isDirty = true;

	// Token: 0x04002726 RID: 10022
	private StringBuilder stringBuilder = new StringBuilder(220);

	// Token: 0x04002727 RID: 10023
	private StringBuilder buttonStringBuilder = new StringBuilder(720);
}
