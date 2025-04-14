using System;
using System.Collections.Generic;
using System.Text;
using GorillaGameModes;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200046A RID: 1130
public class GameModePages : BasePageHandler
{
	// Token: 0x1700030A RID: 778
	// (get) Token: 0x06001BAA RID: 7082 RVA: 0x0008790D File Offset: 0x00085B0D
	protected override int pageSize
	{
		get
		{
			return this.buttons.Length;
		}
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06001BAB RID: 7083 RVA: 0x00087917 File Offset: 0x00085B17
	protected override int entriesCount
	{
		get
		{
			return GameMode.gameModeNames.Count;
		}
	}

	// Token: 0x06001BAC RID: 7084 RVA: 0x00087924 File Offset: 0x00085B24
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

	// Token: 0x06001BAD RID: 7085 RVA: 0x00087977 File Offset: 0x00085B77
	protected override void Start()
	{
		base.Start();
		base.SelectEntryFromIndex(GameModePages.sharedSelectedIndex);
		this.initialized = true;
	}

	// Token: 0x06001BAE RID: 7086 RVA: 0x00087991 File Offset: 0x00085B91
	private void OnEnable()
	{
		if (this.initialized)
		{
			base.SelectEntryFromIndex(GameModePages.sharedSelectedIndex);
		}
	}

	// Token: 0x06001BAF RID: 7087 RVA: 0x000879A6 File Offset: 0x00085BA6
	private void OnDestroy()
	{
		GameModePages.gameModeSelectorInstances.Remove(this);
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x000879B4 File Offset: 0x00085BB4
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

	// Token: 0x06001BB1 RID: 7089 RVA: 0x00087A51 File Offset: 0x00085C51
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

	// Token: 0x06001BB2 RID: 7090 RVA: 0x00087A8C File Offset: 0x00085C8C
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

	// Token: 0x06001BB3 RID: 7091 RVA: 0x00087AF8 File Offset: 0x00085CF8
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

	// Token: 0x06001BB4 RID: 7092 RVA: 0x00087B54 File Offset: 0x00085D54
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

	// Token: 0x04001E8A RID: 7818
	private int currentButtonIndex;

	// Token: 0x04001E8B RID: 7819
	[SerializeField]
	private Text gameModeText;

	// Token: 0x04001E8C RID: 7820
	[SerializeField]
	private GameModeSelectButton[] buttons;

	// Token: 0x04001E8D RID: 7821
	private bool initialized;

	// Token: 0x04001E8E RID: 7822
	private static int sharedSelectedIndex = 0;

	// Token: 0x04001E8F RID: 7823
	private static StringBuilder textBuilder = new StringBuilder(50);

	// Token: 0x04001E90 RID: 7824
	[OnEnterPlay_Clear]
	private static List<GameModePages> gameModeSelectorInstances = new List<GameModePages>(7);
}
