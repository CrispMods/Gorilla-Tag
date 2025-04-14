using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameObjectScheduling;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000102 RID: 258
public class MonkeVoteMachine : MonoBehaviour
{
	// Token: 0x060006DA RID: 1754 RVA: 0x0002734E File Offset: 0x0002554E
	private void Reset()
	{
		this.Configure();
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x00027356 File Offset: 0x00025556
	private void Awake()
	{
		this._proximityTrigger.OnEnter += this.OnPlayerEnteredVoteProximity;
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x00027370 File Offset: 0x00025570
	private void Start()
	{
		MonkeVoteController.instance.OnPollsUpdated += this.HandleOnPollsUpdated;
		MonkeVoteController.instance.OnVoteAccepted += this.HandleOnVoteAccepted;
		MonkeVoteController.instance.OnVoteFailed += this.HandleOnVoteFailed;
		MonkeVoteController.instance.OnCurrentPollEnded += this.HandleCurrentPollEnded;
		this.Init();
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x000273DC File Offset: 0x000255DC
	private void OnDestroy()
	{
		this._proximityTrigger.OnEnter -= this.OnPlayerEnteredVoteProximity;
		MonkeVoteController.instance.OnPollsUpdated -= this.HandleOnPollsUpdated;
		MonkeVoteController.instance.OnVoteAccepted -= this.HandleOnVoteAccepted;
		MonkeVoteController.instance.OnVoteFailed -= this.HandleOnVoteFailed;
		MonkeVoteController.instance.OnCurrentPollEnded -= this.HandleCurrentPollEnded;
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x00027458 File Offset: 0x00025658
	public void Init()
	{
		this._isTestingPoll = false;
		this._previousPoll = (this._currentPoll = null);
		this._waitingOnVote = false;
		foreach (MonkeVoteOption monkeVoteOption in this._votingOptions)
		{
			monkeVoteOption.ResetState();
			monkeVoteOption.OnVote += this.OnVoteEntered;
		}
		this.UpdatePollDisplays();
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x000274B8 File Offset: 0x000256B8
	private void OnPlayerEnteredVoteProximity()
	{
		MonkeVoteController.instance.RequestPolls();
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x000274C4 File Offset: 0x000256C4
	private void HandleOnPollsUpdated()
	{
		this.UpdatePollDisplays();
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x000274CC File Offset: 0x000256CC
	private void UpdatePollDisplays()
	{
		if (MonkeVoteController.instance == null)
		{
			this.SetState(MonkeVoteMachine.VotingState.None, true);
			this.ShowResults(null);
			return;
		}
		MonkeVoteController.FetchPollsResponse lastPollData = MonkeVoteController.instance.GetLastPollData();
		if (lastPollData != null)
		{
			this._previousPoll = new MonkeVoteMachine.PollEntry(lastPollData);
			this.ShowResults(this._previousPoll);
		}
		else
		{
			this.ShowResults(null);
		}
		MonkeVoteController.FetchPollsResponse currentPollData = MonkeVoteController.instance.GetCurrentPollData();
		if (currentPollData == null)
		{
			this.SetState(MonkeVoteMachine.VotingState.None, true);
			return;
		}
		this._nextPollUpdate = MonkeVoteController.instance.GetCurrentPollCompletionTime();
		this._currentPoll = new MonkeVoteMachine.PollEntry(currentPollData);
		MonkeVoteMachine.PollEntry currentPoll = this._currentPoll;
		if (currentPoll != null && currentPoll.IsValid)
		{
			ValueTuple<int, int> vote = this.GetVote(this._currentPoll.PollId);
			int item = vote.Item1;
			int item2 = vote.Item2;
			MonkeVoteMachine.VotingState newState = (item < 0) ? MonkeVoteMachine.VotingState.Voting : ((item2 < 0) ? MonkeVoteMachine.VotingState.Predicting : MonkeVoteMachine.VotingState.Complete);
			this.SetState(newState, true);
			return;
		}
		this.SetState(MonkeVoteMachine.VotingState.None, true);
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x000275B0 File Offset: 0x000257B0
	private void HandleOnVoteAccepted()
	{
		int lastVotePollId = MonkeVoteController.instance.GetLastVotePollId();
		int lastVoteSelectedOption = MonkeVoteController.instance.GetLastVoteSelectedOption();
		bool lastVoteWasPrediction = MonkeVoteController.instance.GetLastVoteWasPrediction();
		this.OnVoteResponseReceived(lastVotePollId, lastVoteSelectedOption, lastVoteWasPrediction, true);
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x000275E8 File Offset: 0x000257E8
	private void HandleOnVoteFailed()
	{
		this._waitingOnVote = false;
		int lastVotePollId = MonkeVoteController.instance.GetLastVotePollId();
		int lastVoteSelectedOption = MonkeVoteController.instance.GetLastVoteSelectedOption();
		bool lastVoteWasPrediction = MonkeVoteController.instance.GetLastVoteWasPrediction();
		this.OnVoteResponseReceived(lastVotePollId, lastVoteSelectedOption, lastVoteWasPrediction, false);
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00027627 File Offset: 0x00025827
	private void HandleCurrentPollEnded()
	{
		if (this._proximityTrigger.isPlayerNearby)
		{
			MonkeVoteController.instance.RequestPolls();
		}
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00027640 File Offset: 0x00025840
	[Tooltip("Hide dynamic child meshes to avoid them getting combined into the parent mesh on awake")]
	private void HideDynamicMeshes()
	{
		this.SetDynamicMeshesVisible(false);
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00027649 File Offset: 0x00025849
	[Tooltip("Show dynamic child meshes to allow easy visualization")]
	private void ShowDynamicMeshes()
	{
		this.SetDynamicMeshesVisible(true);
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00027654 File Offset: 0x00025854
	private void SetDynamicMeshesVisible(bool enabled)
	{
		MonkeVoteOption[] votingOptions = this._votingOptions;
		for (int i = 0; i < votingOptions.Length; i++)
		{
			votingOptions[i].SetDynamicMeshesVisible(enabled);
		}
		MonkeVoteResult[] results = this._results;
		for (int i = 0; i < results.Length; i++)
		{
			results[i].SetDynamicMeshesVisible(enabled);
		}
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x0002769D File Offset: 0x0002589D
	private void Configure()
	{
		this._audio = base.GetComponentInChildren<AudioSource>();
		this._audio.spatialBlend = 1f;
		this._votingOptions = base.GetComponentsInChildren<MonkeVoteOption>();
		this._results = base.GetComponentsInChildren<MonkeVoteResult>();
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x000276D4 File Offset: 0x000258D4
	public void CreateNextDummyPoll()
	{
		this._isTestingPoll = true;
		if (this._currentPoll != null)
		{
			this._previousPoll = this._currentPoll;
		}
		else
		{
			this._previousPoll = null;
		}
		this.ShowResults(this._previousPoll);
		int pollId = 0;
		if (this._previousPoll != null)
		{
			pollId = this._previousPoll.PollId + 1;
		}
		string question = "Test Question Number: " + Random.Range(1, 101).ToString();
		string text = "Answer " + Random.Range(1, 101).ToString();
		string text2 = "Answer " + Random.Range(1, 101).ToString();
		string[] voteOptions = new string[]
		{
			text,
			text2
		};
		this._currentPoll = new MonkeVoteMachine.PollEntry(pollId, question, voteOptions);
		MonkeVoteMachine.PollEntry currentPoll = this._currentPoll;
		if (currentPoll != null && currentPoll.IsValid)
		{
			ValueTuple<int, int> vote = this.GetVote(this._currentPoll.PollId);
			int item = vote.Item1;
			int item2 = vote.Item2;
			MonkeVoteMachine.VotingState newState = (item < 0) ? MonkeVoteMachine.VotingState.Voting : ((item2 < 0) ? MonkeVoteMachine.VotingState.Predicting : MonkeVoteMachine.VotingState.Complete);
			this.SetState(newState, true);
			return;
		}
		this.SetState(MonkeVoteMachine.VotingState.None, true);
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x000277F6 File Offset: 0x000259F6
	private void VoteLeft()
	{
		this.OnVoteEntered(this._votingOptions[0], null);
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00027807 File Offset: 0x00025A07
	private void VoteRight()
	{
		this.OnVoteEntered(this._votingOptions[1], null);
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x00027818 File Offset: 0x00025A18
	private void VoteWinner()
	{
		if (this._currentPoll != null)
		{
			if (this._currentPoll.VoteCount[0] > this._currentPoll.VoteCount[1])
			{
				this.OnVoteEntered(this._votingOptions[0], null);
				return;
			}
			this.OnVoteEntered(this._votingOptions[1], null);
		}
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00027868 File Offset: 0x00025A68
	private void ClearLocalData()
	{
		this.ClearLocalVoteAndPredictionData();
		this.UpdatePollDisplays();
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00027878 File Offset: 0x00025A78
	private void SetState(MonkeVoteMachine.VotingState newState, bool instant = true)
	{
		this._state = newState;
		MonkeVoteMachine.PollEntry currentPoll = this._currentPoll;
		bool flag = currentPoll != null && currentPoll.IsValid;
		if (this._state < MonkeVoteMachine.VotingState.None || this._state > MonkeVoteMachine.VotingState.Complete || (this._state != MonkeVoteMachine.VotingState.None && !flag))
		{
			this._state = MonkeVoteMachine.VotingState.None;
		}
		if (flag)
		{
			int item = this.GetVote(this._currentPoll.PollId).Item2;
			if (this._state < MonkeVoteMachine.VotingState.Predicting)
			{
				this.SaveVote(this._currentPoll.PollId, -1, item);
			}
			int item2 = this.GetVote(this._currentPoll.PollId).Item1;
			if (this._state < MonkeVoteMachine.VotingState.Complete)
			{
				this.SaveVote(this._currentPoll.PollId, item2, -1);
			}
		}
		bool flag2 = true;
		switch (this._state)
		{
		case MonkeVoteMachine.VotingState.None:
			this._timerText.SetFixedText(this._pollsClosedText);
			this._titleText.text = this._defaultTitle;
			this._questionText.text = this._defaultQuestion;
			flag2 = false;
			break;
		case MonkeVoteMachine.VotingState.Voting:
			this._timerText.SetCountdownTime(this._nextPollUpdate);
			this._titleText.text = this._voteTitle;
			this._questionText.text = this._currentPoll.Question;
			break;
		case MonkeVoteMachine.VotingState.Predicting:
			this._timerText.SetCountdownTime(this._nextPollUpdate);
			this._titleText.text = this._predictTitle;
			this._questionText.text = this._predictQuestion;
			break;
		case MonkeVoteMachine.VotingState.Complete:
			this._timerText.SetCountdownTime(this._nextPollUpdate);
			this._titleText.text = this._completeTitle;
			this._questionText.text = this._currentPoll.Question;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		int num;
		int num2;
		if (!flag)
		{
			num = -1;
			num2 = -1;
		}
		else
		{
			ValueTuple<int, int> vote = this.GetVote(this._currentPoll.PollId);
			num = vote.Item1;
			num2 = vote.Item2;
		}
		if (flag2)
		{
			for (int i = 0; i < this._votingOptions.Length; i++)
			{
				this._votingOptions[i].Text = this._currentPoll.VoteOptions[i];
				this._votingOptions[i].ShowIndicators(num == i, num2 == i, instant);
			}
			return;
		}
		foreach (MonkeVoteOption monkeVoteOption in this._votingOptions)
		{
			monkeVoteOption.Text = string.Empty;
			monkeVoteOption.ShowIndicators(false, false, true);
		}
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00027AF4 File Offset: 0x00025CF4
	private void ShowResults(MonkeVoteMachine.PollEntry entry)
	{
		if (entry != null && entry.IsValid)
		{
			ValueTuple<int, int> vote = this.GetVote(entry.PollId);
			int item = vote.Item1;
			int item2 = vote.Item2;
			GTDev.Log<string>(string.Format("Showing {0} V:{1} P:{2}", entry.Question, item, item2), null);
			List<int> list = this.ConvertToPercentages(entry.VoteCount);
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] > num)
				{
					num = list[i];
					num2 = i;
				}
			}
			this._resultsTitleText.text = this._defaultResultsTitle;
			this._resultsQuestionText.text = entry.Question;
			for (int j = 0; j < entry.VoteOptions.Length; j++)
			{
				this._results[j].ShowResult(entry.VoteOptions[j], list[j], item == j, item2 == j, num2 == j);
			}
			int prePollStreak = this.GetPrePollStreak(entry.PollId);
			int postPollStreak = this.GetPostPollStreak(entry);
			this._resultsStreakText.text = ((postPollStreak >= prePollStreak) ? string.Format(this._streakBlurb, postPollStreak) : string.Format(this._streakLostBlurb, prePollStreak, postPollStreak));
			return;
		}
		this._resultsTitleText.text = this._defaultResultsTitle;
		this._resultsQuestionText.text = this._defaultQuestion;
		this._resultsStreakText.text = string.Empty;
		MonkeVoteResult[] results = this._results;
		for (int k = 0; k < results.Length; k++)
		{
			results[k].HideResult();
		}
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00027CA4 File Offset: 0x00025EA4
	private List<int> ConvertToPercentages(int[] votes)
	{
		List<int> list = new List<int>();
		List<float> list2 = new List<float>();
		if (votes == null || votes.Length == 0)
		{
			list.Add(-1);
			list.Add(-1);
			return list;
		}
		if (votes.Length == 1)
		{
			list.Add(100);
			list.Add(0);
			return list;
		}
		int num = MonkeVoteMachine.<ConvertToPercentages>g__Sum|64_0(votes);
		if (num == 0)
		{
			list.Add(-1);
			list.Add(-1);
			return list;
		}
		int num2 = -1;
		int num3 = 0;
		for (int i = 0; i < votes.Length; i++)
		{
			if (votes[i] > num2)
			{
				num2 = votes[i];
				num3 = i;
			}
			float num4 = (float)votes[i] / (float)num * 100f;
			list.Add((int)num4);
			list2.Add(num4 - (float)((int)num4));
		}
		int num5 = MonkeVoteMachine.<ConvertToPercentages>g__Sum|64_0(list);
		int num6 = 100 - num5;
		for (int j = 0; j < num6; j++)
		{
			int num7 = MonkeVoteMachine.<ConvertToPercentages>g__LargestFractionIndex|64_1(list2);
			List<int> list3 = list;
			int index = num7;
			int num8 = list3[index];
			list3[index] = num8 + 1;
			list2[num7] = 0f;
		}
		if (list.Count == 2 && list[num3] == 50)
		{
			List<int> list4 = list;
			int num8 = num3;
			list4[num8]++;
			list4 = list;
			num8 = 1 - num3;
			list4[num8]--;
		}
		return list;
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00027DF0 File Offset: 0x00025FF0
	private void OnVoteEntered(MonkeVoteOption option, Collider votingCollider)
	{
		if (this._waitingOnVote || (Time.time < this._voteCooldownEnd && !this._isTestingPoll))
		{
			this.PlayVoteFailEffects();
			return;
		}
		int num = Array.IndexOf<MonkeVoteOption>(this._votingOptions, option);
		if (num < 0)
		{
			return;
		}
		switch (this._state)
		{
		case MonkeVoteMachine.VotingState.Voting:
			this.Vote(this._currentPoll.PollId, num, false);
			return;
		case MonkeVoteMachine.VotingState.Predicting:
			this.Vote(this._currentPoll.PollId, num, true);
			return;
		}
		this.PlayVoteFailEffects();
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00027E80 File Offset: 0x00026080
	private void Vote(int id, int option, bool isPrediction)
	{
		if (option < 0 || this._waitingOnVote)
		{
			return;
		}
		this._waitingOnVote = true;
		if (this._isTestingPoll)
		{
			this.OnVoteResponseReceived(id, option, isPrediction, true);
			return;
		}
		MonkeVoteController.instance.Vote(id, option, isPrediction);
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00027EB8 File Offset: 0x000260B8
	private void OnVoteResponseReceived(int id, int option, bool isPrediction, bool success)
	{
		this._waitingOnVote = false;
		if (success)
		{
			this.PlayVoteSuccessEffects();
			this._voteCooldownEnd = Time.time + this._voteCooldown;
			ValueTuple<int, int> vote = this.GetVote(id);
			int num = vote.Item1;
			int num2 = vote.Item2;
			if (!isPrediction)
			{
				int num3 = num2;
				num = option;
				num2 = num3;
			}
			else
			{
				num = num;
				num2 = option;
			}
			this.SaveVote(id, num, num2);
			MonkeVoteMachine.VotingState state = this._state;
			if (state != MonkeVoteMachine.VotingState.Voting)
			{
				if (state == MonkeVoteMachine.VotingState.Predicting)
				{
					this.SetState(MonkeVoteMachine.VotingState.Complete, false);
				}
			}
			else
			{
				this.SetState(MonkeVoteMachine.VotingState.Predicting, false);
			}
			if (isPrediction && id == this._currentPoll.PollId)
			{
				this.SavePrePollStreak(id, this.GetPostPollStreak(this._previousPoll));
				return;
			}
		}
		else
		{
			this.PlayVoteFailEffects();
		}
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00027F6C File Offset: 0x0002616C
	private void PlayVoteSuccessEffects()
	{
		MonkeVoteMachine.<PlayVoteSuccessEffects>d__68 <PlayVoteSuccessEffects>d__;
		<PlayVoteSuccessEffects>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<PlayVoteSuccessEffects>d__.<>4__this = this;
		<PlayVoteSuccessEffects>d__.<>1__state = -1;
		<PlayVoteSuccessEffects>d__.<>t__builder.Start<MonkeVoteMachine.<PlayVoteSuccessEffects>d__68>(ref <PlayVoteSuccessEffects>d__);
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x00027FA3 File Offset: 0x000261A3
	private void PlayVoteFailEffects()
	{
		this._audio.GTPlayOneShot(this._voteFailSound, this._audio.volume);
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00027FC4 File Offset: 0x000261C4
	private void SaveVote(int id, int voteOption, int predictionOption)
	{
		int @int = PlayerPrefs.GetInt("Vote_Current_Id", -1);
		if (@int == -1 || @int == id)
		{
			PlayerPrefs.SetInt("Vote_Current_Id", id);
			PlayerPrefs.SetInt("Vote_Current_Option", voteOption);
			PlayerPrefs.SetInt("Vote_Current_Prediction", predictionOption);
		}
		else
		{
			PlayerPrefs.SetInt("Vote_Previous_Id", @int);
			PlayerPrefs.SetInt("Vote_Previous_Option", PlayerPrefs.GetInt("Vote_Current_Option"));
			PlayerPrefs.SetInt("Vote_Previous_Prediction", PlayerPrefs.GetInt("Vote_Current_Prediction"));
			PlayerPrefs.SetInt("Vote_Previous_Streak", PlayerPrefs.GetInt("Vote_Current_Streak"));
			PlayerPrefs.SetInt("Vote_Current_Id", id);
			PlayerPrefs.SetInt("Vote_Current_Option", voteOption);
			PlayerPrefs.SetInt("Vote_Current_Prediction", predictionOption);
			PlayerPrefs.SetInt("Vote_Current_Streak", 0);
		}
		PlayerPrefs.Save();
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00028080 File Offset: 0x00026280
	[return: TupleElementNames(new string[]
	{
		"voteOption",
		"predictionOption"
	})]
	private ValueTuple<int, int> GetVote(int voteId)
	{
		if (PlayerPrefs.GetInt("Vote_Current_Id", -1) == voteId)
		{
			int @int = PlayerPrefs.GetInt("Vote_Current_Option", -1);
			int int2 = PlayerPrefs.GetInt("Vote_Current_Prediction", -1);
			return new ValueTuple<int, int>(@int, int2);
		}
		if (PlayerPrefs.GetInt("Vote_Previous_Id", -1) == voteId)
		{
			int int3 = PlayerPrefs.GetInt("Vote_Previous_Option", -1);
			int int4 = PlayerPrefs.GetInt("Vote_Previous_Prediction", -1);
			return new ValueTuple<int, int>(int3, int4);
		}
		return new ValueTuple<int, int>(-1, -1);
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x000280EC File Offset: 0x000262EC
	private void SavePrePollStreak(int id, int streak)
	{
		if (id < 0)
		{
			return;
		}
		if (PlayerPrefs.GetInt("Vote_Current_Id", -1) == id)
		{
			PlayerPrefs.SetInt("Vote_Current_Streak", streak);
			return;
		}
		if (PlayerPrefs.GetInt("Vote_Previous_Id", -1) == id)
		{
			PlayerPrefs.SetInt("Vote_Previous_Streak", streak);
		}
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00028126 File Offset: 0x00026326
	private int GetPrePollStreak(int id)
	{
		if (id < 0)
		{
			return 0;
		}
		if (PlayerPrefs.GetInt("Vote_Current_Id", -1) == id)
		{
			return PlayerPrefs.GetInt("Vote_Current_Streak", 0);
		}
		if (PlayerPrefs.GetInt("Vote_Previous_Id", -1) == id)
		{
			return PlayerPrefs.GetInt("Vote_Previous_Streak", 0);
		}
		return 0;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00028164 File Offset: 0x00026364
	private int GetPostPollStreak(MonkeVoteMachine.PollEntry entry)
	{
		if (entry == null || !entry.IsValid)
		{
			return 0;
		}
		int item = this.GetVote(entry.PollId).Item2;
		if (item < 0)
		{
			return 0;
		}
		int prePollStreak = this.GetPrePollStreak(entry.PollId);
		if (item != entry.GetWinner())
		{
			return 0;
		}
		return prePollStreak + 1;
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x000281B4 File Offset: 0x000263B4
	private void ClearLocalVoteAndPredictionData()
	{
		PlayerPrefs.DeleteKey("Vote_Current_Id");
		PlayerPrefs.DeleteKey("Vote_Current_Option");
		PlayerPrefs.DeleteKey("Vote_Current_Prediction");
		PlayerPrefs.DeleteKey("Vote_Current_Streak");
		PlayerPrefs.DeleteKey("Vote_Previous_Id");
		PlayerPrefs.DeleteKey("Vote_Previous_Option");
		PlayerPrefs.DeleteKey("Vote_Previous_Prediction");
		PlayerPrefs.DeleteKey("Vote_Previous_Streak");
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x000282A0 File Offset: 0x000264A0
	[CompilerGenerated]
	internal static int <ConvertToPercentages>g__Sum|64_0(IList<int> items)
	{
		int num = 0;
		foreach (int num2 in items)
		{
			num += num2;
		}
		return num;
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x000282E8 File Offset: 0x000264E8
	[CompilerGenerated]
	internal static int <ConvertToPercentages>g__LargestFractionIndex|64_1(IList<float> fractions)
	{
		float num = float.NegativeInfinity;
		int result = -1;
		for (int i = 0; i < fractions.Count; i++)
		{
			if (fractions[i] > num)
			{
				num = fractions[i];
				result = i;
			}
		}
		return result;
	}

	// Token: 0x0400082D RID: 2093
	private const string kVoteCurrentIdKey = "Vote_Current_Id";

	// Token: 0x0400082E RID: 2094
	private const string kVoteCurrentOptionKey = "Vote_Current_Option";

	// Token: 0x0400082F RID: 2095
	private const string kVoteCurrentPredictionKey = "Vote_Current_Prediction";

	// Token: 0x04000830 RID: 2096
	private const string kVoteCurrentStreak = "Vote_Current_Streak";

	// Token: 0x04000831 RID: 2097
	private const string kVotePreviousIdKey = "Vote_Previous_Id";

	// Token: 0x04000832 RID: 2098
	private const string kVotePreviousOptionKey = "Vote_Previous_Option";

	// Token: 0x04000833 RID: 2099
	private const string kVotePreviousPredictionKey = "Vote_Previous_Prediction";

	// Token: 0x04000834 RID: 2100
	private const string kVotePreviousStreak = "Vote_Previous_Streak";

	// Token: 0x04000835 RID: 2101
	[SerializeField]
	private MonkeVoteProximityTrigger _proximityTrigger;

	// Token: 0x04000836 RID: 2102
	[Header("VOTING")]
	[SerializeField]
	private string _pollsClosedText = "POLLS CLOSED";

	// Token: 0x04000837 RID: 2103
	[SerializeField]
	private string _defaultTitle = "MONKE VOTE";

	// Token: 0x04000838 RID: 2104
	[SerializeField]
	private string _voteTitle = "VOTE";

	// Token: 0x04000839 RID: 2105
	[SerializeField]
	private string _predictTitle = "GUESS";

	// Token: 0x0400083A RID: 2106
	[SerializeField]
	private string _completeTitle = "VOTING COMPLETE";

	// Token: 0x0400083B RID: 2107
	[SerializeField]
	private string _defaultQuestion = "COME BACK LATER";

	// Token: 0x0400083C RID: 2108
	[SerializeField]
	private string _predictQuestion = "WHICH WILL BE MORE POPULAR?";

	// Token: 0x0400083D RID: 2109
	[Tooltip("Must be in the format \"STREAK: {0}\"")]
	[SerializeField]
	private string _streakBlurb = "PREDICTION STREAK: {0}";

	// Token: 0x0400083E RID: 2110
	[Tooltip("Must be in the format \"LOST {0} PREDICTION STREAK! STREAK: {1}\"")]
	[SerializeField]
	private string _streakLostBlurb = "<color=red>{0} POLL STREAK LOST!</color>  STREAK: {1}";

	// Token: 0x0400083F RID: 2111
	[SerializeField]
	private float _voteCooldown = 1f;

	// Token: 0x04000840 RID: 2112
	[SerializeField]
	private MonkeVoteOption[] _votingOptions;

	// Token: 0x04000841 RID: 2113
	[SerializeField]
	private CountdownText _timerText;

	// Token: 0x04000842 RID: 2114
	[SerializeField]
	private TMP_Text _titleText;

	// Token: 0x04000843 RID: 2115
	[SerializeField]
	private TMP_Text _questionText;

	// Token: 0x04000844 RID: 2116
	[Header("RESULTS")]
	[SerializeField]
	private string _defaultResultsTitle = "PREVIOUS QUESTION";

	// Token: 0x04000845 RID: 2117
	[SerializeField]
	private TMP_Text _resultsTitleText;

	// Token: 0x04000846 RID: 2118
	[SerializeField]
	private TMP_Text _resultsQuestionText;

	// Token: 0x04000847 RID: 2119
	[SerializeField]
	private TMP_Text _resultsStreakText;

	// Token: 0x04000848 RID: 2120
	[SerializeField]
	private MonkeVoteResult[] _results;

	// Token: 0x04000849 RID: 2121
	[FormerlySerializedAs("_sound")]
	[Header("FX")]
	[SerializeField]
	private AudioSource _audio;

	// Token: 0x0400084A RID: 2122
	[FormerlySerializedAs("_voteProcessingAudio")]
	[SerializeField]
	private AudioSource _voteTubeAudio;

	// Token: 0x0400084B RID: 2123
	[SerializeField]
	private AudioClip[] _voteFailSound;

	// Token: 0x0400084C RID: 2124
	[SerializeField]
	private AudioClip[] _voteSuccessDing;

	// Token: 0x0400084D RID: 2125
	[FormerlySerializedAs("_voteSuccessSound")]
	[SerializeField]
	private AudioClip[] _voteProcessingSound;

	// Token: 0x0400084E RID: 2126
	private MonkeVoteMachine.VotingState _state;

	// Token: 0x0400084F RID: 2127
	private float _voteCooldownEnd;

	// Token: 0x04000850 RID: 2128
	private bool _waitingOnVote;

	// Token: 0x04000851 RID: 2129
	private MonkeVoteMachine.PollEntry _currentPoll;

	// Token: 0x04000852 RID: 2130
	private MonkeVoteMachine.PollEntry _previousPoll;

	// Token: 0x04000853 RID: 2131
	private DateTime _nextPollUpdate;

	// Token: 0x04000854 RID: 2132
	private bool _isTestingPoll;

	// Token: 0x02000103 RID: 259
	public enum VotingState
	{
		// Token: 0x04000856 RID: 2134
		None,
		// Token: 0x04000857 RID: 2135
		Voting,
		// Token: 0x04000858 RID: 2136
		Predicting,
		// Token: 0x04000859 RID: 2137
		Complete
	}

	// Token: 0x02000104 RID: 260
	public class PollEntry
	{
		// Token: 0x060006FF RID: 1791 RVA: 0x00028324 File Offset: 0x00026524
		public PollEntry(int pollId, string question, string[] voteOptions)
		{
			this.PollId = pollId;
			this.Question = question;
			this.VoteOptions = voteOptions;
			this.VoteCount = new int[2];
			this.VoteCount[0] = Random.Range(0, 50000);
			this.VoteCount[1] = Random.Range(0, 50000);
			this.PredictionCount = new int[2];
			this.PredictionCount[0] = Random.Range(0, 50000);
			this.PredictionCount[1] = Random.Range(0, 50000);
			this.StartTime = DateTime.Now;
			this.EndTime = DateTime.Now + TimeSpan.FromSeconds(20.0);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000283DC File Offset: 0x000265DC
		public PollEntry(MonkeVoteController.FetchPollsResponse poll)
		{
			this.PollId = poll.PollId;
			this.Question = poll.Question;
			this.VoteOptions = poll.VoteOptions.ToArray();
			this.VoteCount = poll.VoteCount.ToArray();
			this.PredictionCount = poll.PredictionCount.ToArray();
			this.StartTime = poll.StartTime;
			this.EndTime = poll.EndTime;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00028454 File Offset: 0x00026654
		public int GetWinner()
		{
			if (this.VoteCount == null || this.VoteCount.Length == 0)
			{
				return -1;
			}
			int num = int.MinValue;
			int result = -1;
			for (int i = 0; i < this.VoteCount.Length; i++)
			{
				if (this.VoteCount[i] > num)
				{
					num = this.VoteCount[i];
					result = i;
				}
			}
			return result;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x000284A8 File Offset: 0x000266A8
		public bool IsValid
		{
			get
			{
				string[] voteOptions = this.VoteOptions;
				return voteOptions != null && voteOptions.Length == 2;
			}
		}

		// Token: 0x0400085A RID: 2138
		public int PollId;

		// Token: 0x0400085B RID: 2139
		public string Question;

		// Token: 0x0400085C RID: 2140
		public string[] VoteOptions;

		// Token: 0x0400085D RID: 2141
		public int[] VoteCount;

		// Token: 0x0400085E RID: 2142
		public int[] PredictionCount;

		// Token: 0x0400085F RID: 2143
		public DateTime StartTime;

		// Token: 0x04000860 RID: 2144
		public DateTime EndTime;
	}
}
