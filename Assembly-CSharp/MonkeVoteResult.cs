using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000108 RID: 264
public class MonkeVoteResult : MonoBehaviour
{
	// Token: 0x170000AE RID: 174
	// (get) Token: 0x0600071C RID: 1820 RVA: 0x00028891 File Offset: 0x00026A91
	// (set) Token: 0x0600071D RID: 1821 RVA: 0x0002889C File Offset: 0x00026A9C
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

	// Token: 0x0600071E RID: 1822 RVA: 0x000288C0 File Offset: 0x00026AC0
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

	// Token: 0x0600071F RID: 1823 RVA: 0x00028964 File Offset: 0x00026B64
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

	// Token: 0x06000720 RID: 1824 RVA: 0x000289C0 File Offset: 0x00026BC0
	private void ShowRockPile(int percentage)
	{
		this._rockPiles.Show(percentage);
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x000289D0 File Offset: 0x00026BD0
	public void SetDynamicMeshesVisible(bool visible)
	{
		this._mostPopularIndicator.SetActive(visible);
		this._voteIndicator.SetActive(visible);
		this._guessWinIndicator.SetActive(visible);
		this._guessLoseIndicator.SetActive(visible);
		this._rockPiles.Show(visible ? 100 : -1);
	}

	// Token: 0x04000870 RID: 2160
	[SerializeField]
	private GameObject _optionIndicator;

	// Token: 0x04000871 RID: 2161
	[SerializeField]
	private TMP_Text _optionText;

	// Token: 0x04000872 RID: 2162
	[FormerlySerializedAs("_scoreLabelPost")]
	[SerializeField]
	private GameObject _scoreIndicator;

	// Token: 0x04000873 RID: 2163
	[SerializeField]
	private TMP_Text _scoreText;

	// Token: 0x04000874 RID: 2164
	[SerializeField]
	private GameObject _voteIndicator;

	// Token: 0x04000875 RID: 2165
	[SerializeField]
	private GameObject _guessWinIndicator;

	// Token: 0x04000876 RID: 2166
	[SerializeField]
	private GameObject _guessLoseIndicator;

	// Token: 0x04000877 RID: 2167
	[SerializeField]
	private GameObject _mostPopularIndicator;

	// Token: 0x04000878 RID: 2168
	[SerializeField]
	private GameObject _youWinIndicator;

	// Token: 0x04000879 RID: 2169
	[SerializeField]
	private RockPiles _rockPiles;

	// Token: 0x0400087A RID: 2170
	private MonkeVoteMachine _machine;

	// Token: 0x0400087B RID: 2171
	private string _text = string.Empty;

	// Token: 0x0400087C RID: 2172
	private bool _canVote;

	// Token: 0x0400087D RID: 2173
	private float _rockPileHeight;
}
