using System;
using TMPro;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class QuestDisplay : MonoBehaviour
{
	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0002B382 File Offset: 0x00029582
	public bool IsChanged
	{
		get
		{
			return this.quest.lastChange > this._lastUpdate;
		}
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x0002B398 File Offset: 0x00029598
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

	// Token: 0x060007CB RID: 1995 RVA: 0x0002B430 File Offset: 0x00029630
	private void UpdateCompletionIndicator()
	{
		bool isQuestComplete = this.quest.isQuestComplete;
		bool flag = !isQuestComplete && this.quest.requiredOccurenceCount == 1;
		this.dailyIncompleteIndicator.SetActive(this.quest.isDailyQuest && flag);
		this.dailyCompleteIndicator.SetActive(this.quest.isDailyQuest && isQuestComplete);
		this.weeklyIncompleteIndicator.SetActive(!this.quest.isDailyQuest && flag);
		this.weeklyCompleteIndicator.SetActive(!this.quest.isDailyQuest && isQuestComplete);
	}

	// Token: 0x0400091B RID: 2331
	[SerializeField]
	private ProgressDisplay progressDisplay;

	// Token: 0x0400091C RID: 2332
	[SerializeField]
	private TMP_Text text;

	// Token: 0x0400091D RID: 2333
	[SerializeField]
	private TMP_Text statusText;

	// Token: 0x0400091E RID: 2334
	[SerializeField]
	private GameObject dailyIncompleteIndicator;

	// Token: 0x0400091F RID: 2335
	[SerializeField]
	private GameObject dailyCompleteIndicator;

	// Token: 0x04000920 RID: 2336
	[SerializeField]
	private GameObject weeklyIncompleteIndicator;

	// Token: 0x04000921 RID: 2337
	[SerializeField]
	private GameObject weeklyCompleteIndicator;

	// Token: 0x04000922 RID: 2338
	[NonSerialized]
	public RotatingQuestsManager.RotatingQuest quest;

	// Token: 0x04000923 RID: 2339
	private int _lastUpdate = -1;
}
