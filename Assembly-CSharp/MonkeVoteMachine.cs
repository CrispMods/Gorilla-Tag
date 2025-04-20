using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameObjectScheduling;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200010C RID: 268
public class MonkeVoteMachine : MonoBehaviour
{
	// Token: 0x0600071B RID: 1819 RVA: 0x00035199 File Offset: 0x00033399
	private void Reset()
	{
		this.Configure();
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x000351A1 File Offset: 0x000333A1
	private void Awake()
	{
		this._proximityTrigger.OnEnter += this.OnPlayerEnteredVoteProximity;
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0008934C File Offset: 0x0008754C
	private void Start()
	{
		MonkeVoteController.instance.OnPollsUpdated += this.HandleOnPollsUpdated;
		MonkeVoteController.instance.OnVoteAccepted += this.HandleOnVoteAccepted;
		MonkeVoteController.instance.OnVoteFailed += this.HandleOnVoteFailed;
		MonkeVoteController.instance.OnCurrentPollEnded += this.HandleCurrentPollEnded;
		this.Init();
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x000893B8 File Offset: 0x000875B8
	private void OnDestroy()
	{
		this._proximityTrigger.OnEnter -= this.OnPlayerEnteredVoteProximity;
		MonkeVoteController.instance.OnPollsUpdated -= this.HandleOnPollsUpdated;
		MonkeVoteController.instance.OnVoteAccepted -= this.HandleOnVoteAccepted;
		MonkeVoteController.instance.OnVoteFailed -= this.HandleOnVoteFailed;
		MonkeVoteController.instance.OnCurrentPollEnded -= this.HandleCurrentPollEnded;
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00089434 File Offset: 0x00087634
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

	// Token: 0x06000720 RID: 1824 RVA: 0x000351BA File Offset: 0x000333BA
	private void OnPlayerEnteredVoteProximity()
	{
		MonkeVoteController.instance.RequestPolls();
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x000351C6 File Offset: 0x000333C6
	private void HandleOnPollsUpdated()
	{
		this.UpdatePollDisplays();
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00089494 File Offset: 0x00087694
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

	// Token: 0x06000723 RID: 1827 RVA: 0x00089578 File Offset: 0x00087778
	private void HandleOnVoteAccepted()
	{
		int lastVotePollId = MonkeVoteController.instance.GetLastVotePollId();
		int lastVoteSelectedOption = MonkeVoteController.instance.GetLastVoteSelectedOption();
		bool lastVoteWasPrediction = MonkeVoteController.instance.GetLastVoteWasPrediction();
		this.OnVoteResponseReceived(lastVotePollId, lastVoteSelectedOption, lastVoteWasPrediction, true);
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x000895B0 File Offset: 0x000877B0
	private void HandleOnVoteFailed()
	{
		this._waitingOnVote = false;
		int lastVotePollId = MonkeVoteController.instance.GetLastVotePollId();
		int lastVoteSelectedOption = MonkeVoteController.instance.GetLastVoteSelectedOption();
		bool lastVoteWasPrediction = MonkeVoteController.instance.GetLastVoteWasPrediction();
		this.OnVoteResponseReceived(lastVotePollId, lastVoteSelectedOption, lastVoteWasPrediction, false);
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x000351CE File Offset: 0x000333CE
	private void HandleCurrentPollEnded()
	{
		if (this._proximityTrigger.isPlayerNearby)
		{
			MonkeVoteController.instance.RequestPolls();
		}
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x000351E7 File Offset: 0x000333E7
	[Tooltip("Hide dynamic child meshes to avoid them getting combined into the parent mesh on awake")]
	private void HideDynamicMeshes()
	{
		this.SetDynamicMeshesVisible(false);
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x000351F0 File Offset: 0x000333F0
	[Tooltip("Show dynamic child meshes to allow easy visualization")]
	private void ShowDynamicMeshes()
	{
		this.SetDynamicMeshesVisible(true);
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x000895F0 File Offset: 0x000877F0
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

	// Token: 0x06000729 RID: 1833 RVA: 0x000351F9 File Offset: 0x000333F9
	private void Configure()
	{
		this._audio = base.GetComponentInChildren<AudioSource>();
		this._audio.spatialBlend = 1f;
		this._votingOptions = base.GetComponentsInChildren<MonkeVoteOption>();
		this._results = base.GetComponentsInChildren<MonkeVoteResult>();
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0008963C File Offset: 0x0008783C
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
		string question = "Test Question Number: " + UnityEngine.Random.Range(1, 101).ToString();
		string text = "Answer " + UnityEngine.Random.Range(1, 101).ToString();
		string text2 = "Answer " + UnityEngine.Random.Range(1, 101).ToString();
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

	// Token: 0x0600072B RID: 1835 RVA: 0x0003522F File Offset: 0x0003342F
	private void VoteLeft()
	{
		this.OnVoteEntered(this._votingOptions[0], null);
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00035240 File Offset: 0x00033440
	private void VoteRight()
	{
		this.OnVoteEntered(this._votingOptions[1], null);
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00089760 File Offset: 0x00087960
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

	// Token: 0x0600072E RID: 1838 RVA: 0x00035251 File Offset: 0x00033451
	private void ClearLocalData()
	{
		this.ClearLocalVoteAndPredictionData();
		this.UpdatePollDisplays();
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x000897B0 File Offset: 0x000879B0
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

	// Token: 0x06000730 RID: 1840 RVA: 0x00089A2C File Offset: 0x00087C2C
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

	// Token: 0x06000731 RID: 1841 RVA: 0x00089BDC File Offset: 0x00087DDC
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

	// Token: 0x06000732 RID: 1842 RVA: 0x00089D28 File Offset: 0x00087F28
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

	// Token: 0x06000733 RID: 1843 RVA: 0x0003525F File Offset: 0x0003345F
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

	// Token: 0x06000734 RID: 1844 RVA: 0x00089DB8 File Offset: 0x00087FB8
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

	// Token: 0x06000735 RID: 1845 RVA: 0x00089E6C File Offset: 0x0008806C
	private void PlayVoteSuccessEffects()
	{
		MonkeVoteMachine.<PlayVoteSuccessEffects>d__68 <PlayVoteSuccessEffects>d__;
		<PlayVoteSuccessEffects>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<PlayVoteSuccessEffects>d__.<>4__this = this;
		<PlayVoteSuccessEffects>d__.<>1__state = -1;
		<PlayVoteSuccessEffects>d__.<>t__builder.Start<MonkeVoteMachine.<PlayVoteSuccessEffects>d__68>(ref <PlayVoteSuccessEffects>d__);
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00035295 File Offset: 0x00033495
	private void PlayVoteFailEffects()
	{
		this._audio.GTPlayOneShot(this._voteFailSound, this._audio.volume);
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00089EA4 File Offset: 0x000880A4
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

	// Token: 0x06000738 RID: 1848 RVA: 0x00089F60 File Offset: 0x00088160
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

	// Token: 0x06000739 RID: 1849 RVA: 0x000352B3 File Offset: 0x000334B3
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

	// Token: 0x0600073A RID: 1850 RVA: 0x000352ED File Offset: 0x000334ED
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

	// Token: 0x0600073B RID: 1851 RVA: 0x00089FCC File Offset: 0x000881CC
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

	// Token: 0x0600073C RID: 1852 RVA: 0x0008A01C File Offset: 0x0008821C
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

	// Token: 0x0600073E RID: 1854 RVA: 0x0008A108 File Offset: 0x00088308
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

	// Token: 0x0600073F RID: 1855 RVA: 0x0008A150 File Offset: 0x00088350
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

	// Token: 0x0400086E RID: 2158
	private const string kVoteCurrentIdKey = "Vote_Current_Id";

	// Token: 0x0400086F RID: 2159
	private const string kVoteCurrentOptionKey = "Vote_Current_Option";

	// Token: 0x04000870 RID: 2160
	private const string kVoteCurrentPredictionKey = "Vote_Current_Prediction";

	// Token: 0x04000871 RID: 2161
	private const string kVoteCurrentStreak = "Vote_Current_Streak";

	// Token: 0x04000872 RID: 2162
	private const string kVotePreviousIdKey = "Vote_Previous_Id";

	// Token: 0x04000873 RID: 2163
	private const string kVotePreviousOptionKey = "Vote_Previous_Option";

	// Token: 0x04000874 RID: 2164
	private const string kVotePreviousPredictionKey = "Vote_Previous_Prediction";

	// Token: 0x04000875 RID: 2165
	private const string kVotePreviousStreak = "Vote_Previous_Streak";

	// Token: 0x04000876 RID: 2166
	[SerializeField]
	private MonkeVoteProximityTrigger _proximityTrigger;

	// Token: 0x04000877 RID: 2167
	[Header("VOTING")]
	[SerializeField]
	private string _pollsClosedText = "POLLS CLOSED";

	// Token: 0x04000878 RID: 2168
	[SerializeField]
	private string _defaultTitle = "MONKE VOTE";

	// Token: 0x04000879 RID: 2169
	[SerializeField]
	private string _voteTitle = "VOTE";

	// Token: 0x0400087A RID: 2170
	[SerializeField]
	private string _predictTitle = "GUESS";

	// Token: 0x0400087B RID: 2171
	[SerializeField]
	private string _completeTitle = "VOTING COMPLETE";

	// Token: 0x0400087C RID: 2172
	[SerializeField]
	private string _defaultQuestion = "COME BACK LATER";

	// Token: 0x0400087D RID: 2173
	[SerializeField]
	private string _predictQuestion = "WHICH WILL BE MORE POPULAR?";

	// Token: 0x0400087E RID: 2174
	[Tooltip("Must be in the format \"STREAK: {0}\"")]
	[SerializeField]
	private string _streakBlurb = "PREDICTION STREAK: {0}";

	// Token: 0x0400087F RID: 2175
	[Tooltip("Must be in the format \"LOST {0} PREDICTION STREAK! STREAK: {1}\"")]
	[SerializeField]
	private string _streakLostBlurb = "<color=red>{0} POLL STREAK LOST!</color>  STREAK: {1}";

	// Token: 0x04000880 RID: 2176
	[SerializeField]
	private float _voteCooldown = 1f;

	// Token: 0x04000881 RID: 2177
	[SerializeField]
	private MonkeVoteOption[] _votingOptions;

	// Token: 0x04000882 RID: 2178
	[SerializeField]
	private CountdownText _timerText;

	// Token: 0x04000883 RID: 2179
	[SerializeField]
	private TMP_Text _titleText;

	// Token: 0x04000884 RID: 2180
	[SerializeField]
	private TMP_Text _questionText;

	// Token: 0x04000885 RID: 2181
	[Header("RESULTS")]
	[SerializeField]
	private string _defaultResultsTitle = "PREVIOUS QUESTION";

	// Token: 0x04000886 RID: 2182
	[SerializeField]
	private TMP_Text _resultsTitleText;

	// Token: 0x04000887 RID: 2183
	[SerializeField]
	private TMP_Text _resultsQuestionText;

	// Token: 0x04000888 RID: 2184
	[SerializeField]
	private TMP_Text _resultsStreakText;

	// Token: 0x04000889 RID: 2185
	[SerializeField]
	private MonkeVoteResult[] _results;

	// Token: 0x0400088A RID: 2186
	[FormerlySerializedAs("_sound")]
	[Header("FX")]
	[SerializeField]
	private AudioSource _audio;

	// Token: 0x0400088B RID: 2187
	[FormerlySerializedAs("_voteProcessingAudio")]
	[SerializeField]
	private AudioSource _voteTubeAudio;

	// Token: 0x0400088C RID: 2188
	[SerializeField]
	private AudioClip[] _voteFailSound;

	// Token: 0x0400088D RID: 2189
	[SerializeField]
	private AudioClip[] _voteSuccessDing;

	// Token: 0x0400088E RID: 2190
	[FormerlySerializedAs("_voteSuccessSound")]
	[SerializeField]
	private AudioClip[] _voteProcessingSound;

	// Token: 0x0400088F RID: 2191
	private MonkeVoteMachine.VotingState _state;

	// Token: 0x04000890 RID: 2192
	private float _voteCooldownEnd;

	// Token: 0x04000891 RID: 2193
	private bool _waitingOnVote;

	// Token: 0x04000892 RID: 2194
	private MonkeVoteMachine.PollEntry _currentPoll;

	// Token: 0x04000893 RID: 2195
	private MonkeVoteMachine.PollEntry _previousPoll;

	// Token: 0x04000894 RID: 2196
	private DateTime _nextPollUpdate;

	// Token: 0x04000895 RID: 2197
	private bool _isTestingPoll;

	// Token: 0x0200010D RID: 269
	public enum VotingState
	{
		// Token: 0x04000897 RID: 2199
		None,
		// Token: 0x04000898 RID: 2200
		Voting,
		// Token: 0x04000899 RID: 2201
		Predicting,
		// Token: 0x0400089A RID: 2202
		Complete
	}

	// Token: 0x0200010E RID: 270
	public class PollEntry
	{
		// Token: 0x06000740 RID: 1856 RVA: 0x0008A18C File Offset: 0x0008838C
		public PollEntry(int pollId, string question, string[] voteOptions)
		{
			this.PollId = pollId;
			this.Question = question;
			this.VoteOptions = voteOptions;
			this.VoteCount = new int[2];
			this.VoteCount[0] = UnityEngine.Random.Range(0, 50000);
			this.VoteCount[1] = UnityEngine.Random.Range(0, 50000);
			this.PredictionCount = new int[2];
			this.PredictionCount[0] = UnityEngine.Random.Range(0, 50000);
			this.PredictionCount[1] = UnityEngine.Random.Range(0, 50000);
			this.StartTime = DateTime.Now;
			this.EndTime = DateTime.Now + TimeSpan.FromSeconds(20.0);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0008A244 File Offset: 0x00088444
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

		// Token: 0x06000742 RID: 1858 RVA: 0x0008A2BC File Offset: 0x000884BC
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

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x0008A310 File Offset: 0x00088510
		public bool IsValid
		{
			get
			{
				string[] voteOptions = this.VoteOptions;
				return voteOptions != null && voteOptions.Length == 2;
			}
		}

		// Token: 0x0400089B RID: 2203
		public int PollId;

		// Token: 0x0400089C RID: 2204
		public string Question;

		// Token: 0x0400089D RID: 2205
		public string[] VoteOptions;

		// Token: 0x0400089E RID: 2206
		public int[] VoteCount;

		// Token: 0x0400089F RID: 2207
		public int[] PredictionCount;

		// Token: 0x040008A0 RID: 2208
		public DateTime StartTime;

		// Token: 0x040008A1 RID: 2209
		public DateTime EndTime;
	}
}
