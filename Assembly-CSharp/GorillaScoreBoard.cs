using System;
using System.Collections.Generic;
using System.Text;
using GorillaGameModes;
using KID.Model;
using TMPro;
using UnityEngine;

// Token: 0x0200058D RID: 1421
public class GorillaScoreBoard : MonoBehaviour
{
	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06002329 RID: 9001 RVA: 0x000AE390 File Offset: 0x000AC590
	// (set) Token: 0x0600232A RID: 9002 RVA: 0x000AE3A7 File Offset: 0x000AC5A7
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

	// Token: 0x0600232B RID: 9003 RVA: 0x000AE3B0 File Offset: 0x000AC5B0
	public void SetSleepState(bool awake)
	{
		this.boardText.enabled = awake;
		this.buttonText.enabled = awake;
		if (this.linesParent != null)
		{
			this.linesParent.SetActive(awake);
		}
	}

	// Token: 0x0600232C RID: 9004 RVA: 0x000023F4 File Offset: 0x000005F4
	private void OnDestroy()
	{
	}

	// Token: 0x0600232D RID: 9005 RVA: 0x000AE3E4 File Offset: 0x000AC5E4
	public string GetBeginningString()
	{
		return "ROOM ID: " + (NetworkSystem.Instance.SessionIsPrivate ? "-PRIVATE- GAME: " : (NetworkSystem.Instance.RoomName + "   GAME: ")) + this.RoomType() + "\n  PLAYER     COLOR  MUTE   REPORT";
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x000AE424 File Offset: 0x000AC624
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

	// Token: 0x0600232F RID: 9007 RVA: 0x000AE4A4 File Offset: 0x000AC6A4
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

	// Token: 0x06002330 RID: 9008 RVA: 0x000AE6B4 File Offset: 0x000AC8B4
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

	// Token: 0x06002331 RID: 9009 RVA: 0x000AE713 File Offset: 0x000AC913
	private void Start()
	{
		GorillaScoreboardTotalUpdater.RegisterScoreboard(this);
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x000AE71B File Offset: 0x000AC91B
	private void OnEnable()
	{
		GorillaScoreboardTotalUpdater.RegisterScoreboard(this);
		this._isDirty = true;
	}

	// Token: 0x06002333 RID: 9011 RVA: 0x000AE72A File Offset: 0x000AC92A
	private void OnDisable()
	{
		GorillaScoreboardTotalUpdater.UnregisterScoreboard(this);
	}

	// Token: 0x040026BA RID: 9914
	public GameObject scoreBoardLinePrefab;

	// Token: 0x040026BB RID: 9915
	public int startingYValue;

	// Token: 0x040026BC RID: 9916
	public int lineHeight;

	// Token: 0x040026BD RID: 9917
	public bool includeMMR;

	// Token: 0x040026BE RID: 9918
	public bool isActive;

	// Token: 0x040026BF RID: 9919
	public GameObject linesParent;

	// Token: 0x040026C0 RID: 9920
	[SerializeField]
	public List<GorillaPlayerScoreboardLine> lines;

	// Token: 0x040026C1 RID: 9921
	public TextMeshPro boardText;

	// Token: 0x040026C2 RID: 9922
	public TextMeshPro buttonText;

	// Token: 0x040026C3 RID: 9923
	public bool needsUpdate;

	// Token: 0x040026C4 RID: 9924
	public TextMeshPro notInRoomText;

	// Token: 0x040026C5 RID: 9925
	public string initialGameMode;

	// Token: 0x040026C6 RID: 9926
	private string tempGmName;

	// Token: 0x040026C7 RID: 9927
	private string gmName;

	// Token: 0x040026C8 RID: 9928
	private const string error = "ERROR";

	// Token: 0x040026C9 RID: 9929
	private List<string> gmNames;

	// Token: 0x040026CA RID: 9930
	private bool _isDirty = true;

	// Token: 0x040026CB RID: 9931
	private StringBuilder stringBuilder = new StringBuilder(220);

	// Token: 0x040026CC RID: 9932
	private StringBuilder buttonStringBuilder = new StringBuilder(720);
}
