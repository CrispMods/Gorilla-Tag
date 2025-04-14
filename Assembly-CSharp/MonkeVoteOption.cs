using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000106 RID: 262
public class MonkeVoteOption : MonoBehaviour
{
	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06000705 RID: 1797 RVA: 0x000285C8 File Offset: 0x000267C8
	// (remove) Token: 0x06000706 RID: 1798 RVA: 0x00028600 File Offset: 0x00026800
	public event Action<MonkeVoteOption, Collider> OnVote;

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000707 RID: 1799 RVA: 0x00028635 File Offset: 0x00026835
	// (set) Token: 0x06000708 RID: 1800 RVA: 0x00028640 File Offset: 0x00026840
	public string Text
	{
		get
		{
			return this._text;
		}
		set
		{
			TMP_Text optionText = this._optionText;
			this._text = value;
			optionText.text = value;
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000709 RID: 1801 RVA: 0x00028662 File Offset: 0x00026862
	// (set) Token: 0x0600070A RID: 1802 RVA: 0x0002866C File Offset: 0x0002686C
	public bool CanVote
	{
		get
		{
			return this._canVote;
		}
		set
		{
			Collider trigger = this._trigger;
			this._canVote = value;
			trigger.enabled = value;
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0002868E File Offset: 0x0002688E
	private void Reset()
	{
		this.Configure();
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00028698 File Offset: 0x00026898
	private void Configure()
	{
		foreach (Collider collider in base.GetComponentsInChildren<Collider>())
		{
			if (collider.isTrigger)
			{
				this._trigger = collider;
				break;
			}
		}
		if (!this._optionText)
		{
			this._optionText = base.GetComponentInChildren<TMP_Text>();
		}
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x000286E8 File Offset: 0x000268E8
	private void OnTriggerEnter(Collider other)
	{
		if (!this.IsValidVotingRock(other))
		{
			return;
		}
		Action<MonkeVoteOption, Collider> onVote = this.OnVote;
		if (onVote == null)
		{
			return;
		}
		onVote(this, other);
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x00028708 File Offset: 0x00026908
	private bool IsValidVotingRock(Collider other)
	{
		SlingshotProjectile component = other.GetComponent<SlingshotProjectile>();
		return component && component.projectileOwner.IsLocal;
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x00028731 File Offset: 0x00026931
	public void ResetState()
	{
		this.OnVote = null;
		this.ShowIndicators(false, false, true);
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x00028743 File Offset: 0x00026943
	public void ShowIndicators(bool showVote, bool showPrediction, bool instant = true)
	{
		this._voteIndicator.SetVisible(showVote, instant);
		this._guessIndicator.SetVisible(showPrediction, instant);
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0002875F File Offset: 0x0002695F
	private void Vote()
	{
		this.SendVote(null);
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00028768 File Offset: 0x00026968
	private void SendVote(Collider other)
	{
		if (!this._canVote)
		{
			return;
		}
		Action<MonkeVoteOption, Collider> onVote = this.OnVote;
		if (onVote == null)
		{
			return;
		}
		onVote(this, other);
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x00028785 File Offset: 0x00026985
	public void SetDynamicMeshesVisible(bool visible)
	{
		this._voteIndicator.SetVisible(visible, true);
		this._guessIndicator.SetVisible(visible, true);
	}

	// Token: 0x04000865 RID: 2149
	[SerializeField]
	private Collider _trigger;

	// Token: 0x04000866 RID: 2150
	[SerializeField]
	private TMP_Text _optionText;

	// Token: 0x04000867 RID: 2151
	[SerializeField]
	private VotingCard _voteIndicator;

	// Token: 0x04000868 RID: 2152
	[FormerlySerializedAs("_predictionIndicator")]
	[SerializeField]
	private VotingCard _guessIndicator;

	// Token: 0x0400086A RID: 2154
	private string _text = string.Empty;

	// Token: 0x0400086B RID: 2155
	private bool _canVote;
}
