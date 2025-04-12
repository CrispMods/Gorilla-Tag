using System;
using System.Collections.Generic;
using System.Text;
using GorillaGameModes;
using KID.Model;
using TMPro;
using UnityEngine;

// Token: 0x0200058E RID: 1422
public class GorillaScoreBoard : MonoBehaviour
{
	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06002331 RID: 9009 RVA: 0x00046BDB File Offset: 0x00044DDB
	// (set) Token: 0x06002332 RID: 9010 RVA: 0x00046BF2 File Offset: 0x00044DF2
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

	// Token: 0x06002333 RID: 9011 RVA: 0x00046BFB File Offset: 0x00044DFB
	public void SetSleepState(bool awake)
	{
		this.boardText.enabled = awake;
		this.buttonText.enabled = awake;
		if (this.linesParent != null)
		{
			this.linesParent.SetActive(awake);
		}
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void OnDestroy()
	{
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x00046C2F File Offset: 0x00044E2F
	public string GetBeginningString()
	{
		return "ROOM ID: " + (NetworkSystem.Instance.SessionIsPrivate ? "-PRIVATE- GAME: " : (NetworkSystem.Instance.RoomName + "   GAME: ")) + this.RoomType() + "\n  PLAYER     COLOR  MUTE   REPORT";
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x000FAD00 File Offset: 0x000F8F00
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

	// Token: 0x06002337 RID: 9015 RVA: 0x000FAD80 File Offset: 0x000F8F80
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

	// Token: 0x06002338 RID: 9016 RVA: 0x000FAF90 File Offset: 0x000F9190
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

	// Token: 0x06002339 RID: 9017 RVA: 0x00046C6D File Offset: 0x00044E6D
	private void Start()
	{
		GorillaScoreboardTotalUpdater.RegisterScoreboard(this);
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x00046C75 File Offset: 0x00044E75
	private void OnEnable()
	{
		GorillaScoreboardTotalUpdater.RegisterScoreboard(this);
		this._isDirty = true;
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x00046C84 File Offset: 0x00044E84
	private void OnDisable()
	{
		GorillaScoreboardTotalUpdater.UnregisterScoreboard(this);
	}

	// Token: 0x040026C0 RID: 9920
	public GameObject scoreBoardLinePrefab;

	// Token: 0x040026C1 RID: 9921
	public int startingYValue;

	// Token: 0x040026C2 RID: 9922
	public int lineHeight;

	// Token: 0x040026C3 RID: 9923
	public bool includeMMR;

	// Token: 0x040026C4 RID: 9924
	public bool isActive;

	// Token: 0x040026C5 RID: 9925
	public GameObject linesParent;

	// Token: 0x040026C6 RID: 9926
	[SerializeField]
	public List<GorillaPlayerScoreboardLine> lines;

	// Token: 0x040026C7 RID: 9927
	public TextMeshPro boardText;

	// Token: 0x040026C8 RID: 9928
	public TextMeshPro buttonText;

	// Token: 0x040026C9 RID: 9929
	public bool needsUpdate;

	// Token: 0x040026CA RID: 9930
	public TextMeshPro notInRoomText;

	// Token: 0x040026CB RID: 9931
	public string initialGameMode;

	// Token: 0x040026CC RID: 9932
	private string tempGmName;

	// Token: 0x040026CD RID: 9933
	private string gmName;

	// Token: 0x040026CE RID: 9934
	private const string error = "ERROR";

	// Token: 0x040026CF RID: 9935
	private List<string> gmNames;

	// Token: 0x040026D0 RID: 9936
	private bool _isDirty = true;

	// Token: 0x040026D1 RID: 9937
	private StringBuilder stringBuilder = new StringBuilder(220);

	// Token: 0x040026D2 RID: 9938
	private StringBuilder buttonStringBuilder = new StringBuilder(720);
}
