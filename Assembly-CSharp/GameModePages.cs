using System;
using System.Collections.Generic;
using System.Text;
using GorillaGameModes;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000476 RID: 1142
public class GameModePages : BasePageHandler
{
	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06001BFB RID: 7163 RVA: 0x00043364 File Offset: 0x00041564
	protected override int pageSize
	{
		get
		{
			return this.buttons.Length;
		}
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06001BFC RID: 7164 RVA: 0x0004336E File Offset: 0x0004156E
	protected override int entriesCount
	{
		get
		{
			return GameMode.gameModeNames.Count;
		}
	}

	// Token: 0x06001BFD RID: 7165 RVA: 0x000DB6BC File Offset: 0x000D98BC
	private void Awake()
	{
		GameModePages.gameModeSelectorInstances.Add(this);
		this.buttons = base.GetComponentsInChildren<GameModeSelectButton>();
		for (int i = 0; i < this.buttons.Length; i++)
		{
			this.buttons[i].buttonIndex = i;
			this.buttons[i].selector = this;
		}
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x0004337A File Offset: 0x0004157A
	protected override void Start()
	{
		base.Start();
		base.SelectEntryFromIndex(GameModePages.sharedSelectedIndex);
		this.initialized = true;
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x00043394 File Offset: 0x00041594
	private void OnEnable()
	{
		if (this.initialized)
		{
			base.SelectEntryFromIndex(GameModePages.sharedSelectedIndex);
		}
	}

	// Token: 0x06001C00 RID: 7168 RVA: 0x000433A9 File Offset: 0x000415A9
	private void OnDestroy()
	{
		GameModePages.gameModeSelectorInstances.Remove(this);
	}

	// Token: 0x06001C01 RID: 7169 RVA: 0x000DB710 File Offset: 0x000D9910
	protected override void ShowPage(int selectedPage, int startIndex, int endIndex)
	{
		GameModePages.textBuilder.Clear();
		for (int i = startIndex; i < endIndex; i++)
		{
			GameModePages.textBuilder.AppendLine(GameMode.gameModeNames[i]);
		}
		this.gameModeText.text = GameModePages.textBuilder.ToString();
		if (base.selectedIndex >= startIndex && base.selectedIndex <= endIndex)
		{
			this.UpdateAllButtons(this.currentButtonIndex);
		}
		else
		{
			this.UpdateAllButtons(-1);
		}
		int buttonsMissing = (selectedPage == base.pages - 1 && base.maxEntires > endIndex) ? (base.maxEntires - endIndex) : 0;
		this.EnableEntryButtons(buttonsMissing);
	}

	// Token: 0x06001C02 RID: 7170 RVA: 0x000433B7 File Offset: 0x000415B7
	protected override void PageEntrySelected(int pageEntry, int selectionIndex)
	{
		if (selectionIndex >= this.entriesCount)
		{
			return;
		}
		GameModePages.sharedSelectedIndex = selectionIndex;
		this.UpdateAllButtons(pageEntry);
		this.currentButtonIndex = pageEntry;
		GorillaComputer.instance.OnModeSelectButtonPress(GameMode.gameModeNames[selectionIndex], false);
	}

	// Token: 0x06001C03 RID: 7171 RVA: 0x000DB7B0 File Offset: 0x000D99B0
	private void UpdateAllButtons(int onButton)
	{
		for (int i = 0; i < this.buttons.Length; i++)
		{
			if (i == onButton)
			{
				this.buttons[onButton].isOn = true;
				this.buttons[onButton].UpdateColor();
			}
			else if (this.buttons[i].isOn)
			{
				this.buttons[i].isOn = false;
				this.buttons[i].UpdateColor();
			}
		}
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x000DB81C File Offset: 0x000D9A1C
	private void EnableEntryButtons(int buttonsMissing)
	{
		int num = this.buttons.Length - buttonsMissing;
		int i;
		for (i = 0; i < num; i++)
		{
			this.buttons[i].gameObject.SetActive(true);
		}
		while (i < this.buttons.Length)
		{
			this.buttons[i].gameObject.SetActive(false);
			i++;
		}
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x000DB878 File Offset: 0x000D9A78
	public static void SetSelectedGameModeShared(string gameMode)
	{
		GameModePages.sharedSelectedIndex = GameMode.gameModeNames.IndexOf(gameMode);
		if (GameModePages.sharedSelectedIndex < 0)
		{
			return;
		}
		for (int i = 0; i < GameModePages.gameModeSelectorInstances.Count; i++)
		{
			GameModePages.gameModeSelectorInstances[i].SelectEntryFromIndex(GameModePages.sharedSelectedIndex);
		}
	}

	// Token: 0x04001ED8 RID: 7896
	private int currentButtonIndex;

	// Token: 0x04001ED9 RID: 7897
	[SerializeField]
	private Text gameModeText;

	// Token: 0x04001EDA RID: 7898
	[SerializeField]
	private GameModeSelectButton[] buttons;

	// Token: 0x04001EDB RID: 7899
	private bool initialized;

	// Token: 0x04001EDC RID: 7900
	private static int sharedSelectedIndex = 0;

	// Token: 0x04001EDD RID: 7901
	private static StringBuilder textBuilder = new StringBuilder(50);

	// Token: 0x04001EDE RID: 7902
	[OnEnterPlay_Clear]
	private static List<GameModePages> gameModeSelectorInstances = new List<GameModePages>(7);
}
