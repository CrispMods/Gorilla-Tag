using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000112 RID: 274
public class MonkeVoteResult : MonoBehaviour
{
	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x0600075D RID: 1885 RVA: 0x00035461 File Offset: 0x00033661
	// (set) Token: 0x0600075E RID: 1886 RVA: 0x0008A5C4 File Offset: 0x000887C4
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

	// Token: 0x0600075F RID: 1887 RVA: 0x0008A5E8 File Offset: 0x000887E8
	public void ShowResult(string questionOption, int percentage, bool showVote, bool showPrediction, bool isWinner)
	{
		this._optionText.text = questionOption;
		this._optionIndicator.SetActive(true);
		this._scoreText.text = ((percentage >= 0) ? string.Format("{0}%", percentage) : "--");
		this._voteIndicator.SetActive(showVote);
		this._guessWinIndicator.SetActive(showPrediction && isWinner);
		this._guessLoseIndicator.SetActive(showPrediction && !isWinner);
		this._youWinIndicator.SetActive(isWinner && showPrediction);
		this._mostPopularIndicator.SetActive(isWinner);
		this.ShowRockPile(percentage);
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x0008A68C File Offset: 0x0008888C
	public void HideResult()
	{
		this._optionIndicator.SetActive(false);
		this._voteIndicator.SetActive(false);
		this._guessWinIndicator.SetActive(false);
		this._guessLoseIndicator.SetActive(false);
		this._youWinIndicator.SetActive(false);
		this._mostPopularIndicator.SetActive(false);
		this.ShowRockPile(0);
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x00035469 File Offset: 0x00033669
	private void ShowRockPile(int percentage)
	{
		this._rockPiles.Show(percentage);
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0008A6E8 File Offset: 0x000888E8
	public void SetDynamicMeshesVisible(bool visible)
	{
		this._mostPopularIndicator.SetActive(visible);
		this._voteIndicator.SetActive(visible);
		this._guessWinIndicator.SetActive(visible);
		this._guessLoseIndicator.SetActive(visible);
		this._rockPiles.Show(visible ? 100 : -1);
	}

	// Token: 0x040008B1 RID: 2225
	[SerializeField]
	private GameObject _optionIndicator;

	// Token: 0x040008B2 RID: 2226
	[SerializeField]
	private TMP_Text _optionText;

	// Token: 0x040008B3 RID: 2227
	[FormerlySerializedAs("_scoreLabelPost")]
	[SerializeField]
	private GameObject _scoreIndicator;

	// Token: 0x040008B4 RID: 2228
	[SerializeField]
	private TMP_Text _scoreText;

	// Token: 0x040008B5 RID: 2229
	[SerializeField]
	private GameObject _voteIndicator;

	// Token: 0x040008B6 RID: 2230
	[SerializeField]
	private GameObject _guessWinIndicator;

	// Token: 0x040008B7 RID: 2231
	[SerializeField]
	private GameObject _guessLoseIndicator;

	// Token: 0x040008B8 RID: 2232
	[SerializeField]
	private GameObject _mostPopularIndicator;

	// Token: 0x040008B9 RID: 2233
	[SerializeField]
	private GameObject _youWinIndicator;

	// Token: 0x040008BA RID: 2234
	[SerializeField]
	private RockPiles _rockPiles;

	// Token: 0x040008BB RID: 2235
	private MonkeVoteMachine _machine;

	// Token: 0x040008BC RID: 2236
	private string _text = string.Empty;

	// Token: 0x040008BD RID: 2237
	private bool _canVote;

	// Token: 0x040008BE RID: 2238
	private float _rockPileHeight;
}
