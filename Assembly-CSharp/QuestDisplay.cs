using System;
using TMPro;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class QuestDisplay : MonoBehaviour
{
	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x0600080B RID: 2059 RVA: 0x00035AA2 File Offset: 0x00033CA2
	public bool IsChanged
	{
		get
		{
			return this.quest.lastChange > this._lastUpdate;
		}
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0008C7D4 File Offset: 0x0008A9D4
	public void UpdateDisplay()
	{
		this.text.text = this.quest.GetTextDescription();
		if (this.quest.isQuestComplete)
		{
			this.progressDisplay.SetVisible(false);
		}
		else if (this.quest.requiredOccurenceCount > 1)
		{
			this.progressDisplay.SetProgress(this.quest.occurenceCount, this.quest.requiredOccurenceCount);
			this.progressDisplay.SetVisible(true);
		}
		else
		{
			this.progressDisplay.SetVisible(false);
		}
		this.UpdateCompletionIndicator();
		this._lastUpdate = Time.frameCount;
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0008C86C File Offset: 0x0008AA6C
	private void UpdateCompletionIndicator()
	{
		bool isQuestComplete = this.quest.isQuestComplete;
		bool flag = !isQuestComplete && this.quest.requiredOccurenceCount == 1;
		this.dailyIncompleteIndicator.SetActive(this.quest.isDailyQuest && flag);
		this.dailyCompleteIndicator.SetActive(this.quest.isDailyQuest && isQuestComplete);
		this.weeklyIncompleteIndicator.SetActive(!this.quest.isDailyQuest && flag);
		this.weeklyCompleteIndicator.SetActive(!this.quest.isDailyQuest && isQuestComplete);
	}

	// Token: 0x0400095C RID: 2396
	[SerializeField]
	private ProgressDisplay progressDisplay;

	// Token: 0x0400095D RID: 2397
	[SerializeField]
	private TMP_Text text;

	// Token: 0x0400095E RID: 2398
	[SerializeField]
	private TMP_Text statusText;

	// Token: 0x0400095F RID: 2399
	[SerializeField]
	private GameObject dailyIncompleteIndicator;

	// Token: 0x04000960 RID: 2400
	[SerializeField]
	private GameObject dailyCompleteIndicator;

	// Token: 0x04000961 RID: 2401
	[SerializeField]
	private GameObject weeklyIncompleteIndicator;

	// Token: 0x04000962 RID: 2402
	[SerializeField]
	private GameObject weeklyCompleteIndicator;

	// Token: 0x04000963 RID: 2403
	[NonSerialized]
	public RotatingQuestsManager.RotatingQuest quest;

	// Token: 0x04000964 RID: 2404
	private int _lastUpdate = -1;
}
