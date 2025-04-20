using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000110 RID: 272
public class MonkeVoteOption : MonoBehaviour
{
	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06000746 RID: 1862 RVA: 0x0008A420 File Offset: 0x00088620
	// (remove) Token: 0x06000747 RID: 1863 RVA: 0x0008A458 File Offset: 0x00088658
	public event Action<MonkeVoteOption, Collider> OnVote;

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000748 RID: 1864 RVA: 0x00035338 File Offset: 0x00033538
	// (set) Token: 0x06000749 RID: 1865 RVA: 0x0008A490 File Offset: 0x00088690
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

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x0600074A RID: 1866 RVA: 0x00035340 File Offset: 0x00033540
	// (set) Token: 0x0600074B RID: 1867 RVA: 0x0008A4B4 File Offset: 0x000886B4
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

	// Token: 0x0600074C RID: 1868 RVA: 0x00035348 File Offset: 0x00033548
	private void Reset()
	{
		this.Configure();
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0008A4D8 File Offset: 0x000886D8
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

	// Token: 0x0600074E RID: 1870 RVA: 0x00035350 File Offset: 0x00033550
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

	// Token: 0x0600074F RID: 1871 RVA: 0x0008A528 File Offset: 0x00088728
	private bool IsValidVotingRock(Collider other)
	{
		SlingshotProjectile component = other.GetComponent<SlingshotProjectile>();
		return component && component.projectileOwner.IsLocal;
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x0003536E File Offset: 0x0003356E
	public void ResetState()
	{
		this.OnVote = null;
		this.ShowIndicators(false, false, true);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x00035380 File Offset: 0x00033580
	public void ShowIndicators(bool showVote, bool showPrediction, bool instant = true)
	{
		this._voteIndicator.SetVisible(showVote, instant);
		this._guessIndicator.SetVisible(showPrediction, instant);
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0003539C File Offset: 0x0003359C
	private void Vote()
	{
		this.SendVote(null);
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x000353A5 File Offset: 0x000335A5
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

	// Token: 0x06000754 RID: 1876 RVA: 0x000353C2 File Offset: 0x000335C2
	public void SetDynamicMeshesVisible(bool visible)
	{
		this._voteIndicator.SetVisible(visible, true);
		this._guessIndicator.SetVisible(visible, true);
	}

	// Token: 0x040008A6 RID: 2214
	[SerializeField]
	private Collider _trigger;

	// Token: 0x040008A7 RID: 2215
	[SerializeField]
	private TMP_Text _optionText;

	// Token: 0x040008A8 RID: 2216
	[SerializeField]
	private VotingCard _voteIndicator;

	// Token: 0x040008A9 RID: 2217
	[FormerlySerializedAs("_predictionIndicator")]
	[SerializeField]
	private VotingCard _guessIndicator;

	// Token: 0x040008AB RID: 2219
	private string _text = string.Empty;

	// Token: 0x040008AC RID: 2220
	private bool _canVote;
}
