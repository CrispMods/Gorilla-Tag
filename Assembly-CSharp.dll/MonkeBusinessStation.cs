using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameObjectScheduling;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010D RID: 269
public class MonkeBusinessStation : MonoBehaviourPunCallbacks
{
	// Token: 0x06000733 RID: 1843 RVA: 0x00034299 File Offset: 0x00032499
	public override void OnEnable()
	{
		base.OnEnable();
		this.FindQuestManager();
		ProgressionController.OnQuestSelectionChanged += this.OnQuestSelectionChanged;
		ProgressionController.OnProgressEvent += this.OnProgress;
		ProgressionController.RequestProgressUpdate();
		this.UpdateCountdownTimers();
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x000342D4 File Offset: 0x000324D4
	public override void OnDisable()
	{
		base.OnDisable();
		ProgressionController.OnQuestSelectionChanged -= this.OnQuestSelectionChanged;
		ProgressionController.OnProgressEvent -= this.OnProgress;
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x000342FE File Offset: 0x000324FE
	private void FindQuestManager()
	{
		if (!this._questManager)
		{
			this._questManager = UnityEngine.Object.FindObjectOfType<RotatingQuestsManager>();
		}
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00034318 File Offset: 0x00032518
	private void UpdateCountdownTimers()
	{
		this._dailyCountdown.SetCountdownTime(this._questManager.DailyQuestCountdown);
		this._weeklyCountdown.SetCountdownTime(this._questManager.WeeklyQuestCountdown);
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00034346 File Offset: 0x00032546
	private void OnQuestSelectionChanged()
	{
		this.UpdateCountdownTimers();
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x0003434E File Offset: 0x0003254E
	private void OnProgress()
	{
		this.UpdateQuestStatus();
		this.UpdateProgressDisplays();
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00088054 File Offset: 0x00086254
	private void UpdateProgressDisplays()
	{
		ValueTuple<int, int, int> progressionData = ProgressionController.GetProgressionData();
		int item = progressionData.Item1;
		int item2 = progressionData.Item2;
		this._weeklyProgress.SetProgress(item, ProgressionController.WeeklyCap);
		if (!this._isUpdatingPointCount)
		{
			this._unclaimedPoints.text = item2.ToString();
			this._claimButton.isOn = (item2 > 0);
		}
		bool flag = item2 > 0;
		this._claimablePointsObject.SetActive(flag);
		this._noClaimablePointsObject.SetActive(!flag);
		this._badgeMount.position = (flag ? this._claimablePointsBadgePosition.position : this._noClaimablePointsBadgePosition.position);
		this._claimButton.gameObject.SetActive(flag);
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00088108 File Offset: 0x00086308
	private void UpdateQuestStatus()
	{
		if (this._lastQuestChange >= RotatingQuestsManager.LastQuestChange)
		{
			return;
		}
		this.FindQuestManager();
		if (this._quests.Count == 0 || this._lastQuestDailyID != RotatingQuestsManager.LastQuestDailyID)
		{
			this.BuildQuestList();
		}
		foreach (QuestDisplay questDisplay in this._quests)
		{
			if (questDisplay.IsChanged)
			{
				questDisplay.UpdateDisplay();
			}
		}
		this._lastQuestChange = Time.frameCount;
		this._lastQuestDailyID = RotatingQuestsManager.LastQuestDailyID;
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x000881AC File Offset: 0x000863AC
	public void RedeemProgress()
	{
		if (this._claimButton.isOn)
		{
			this._isUpdatingPointCount = true;
			ValueTuple<int, int, int> progressionData = ProgressionController.GetProgressionData();
			int item = progressionData.Item2;
			int item2 = progressionData.Item3;
			this._tempUnclaimedPoints = item;
			this._tempTotalPoints = item2;
			this._claimButton.isOn = false;
			ProgressionController.RedeemProgress();
			if (PhotonNetwork.InRoom)
			{
				base.photonView.RPC("BroadcastRedeemQuestPoints", RpcTarget.Others, new object[]
				{
					this._tempUnclaimedPoints
				});
			}
			base.StartCoroutine(this.PerformPointRedemptionSequence());
		}
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x0003435C File Offset: 0x0003255C
	private IEnumerator PerformPointRedemptionSequence()
	{
		while (this._tempUnclaimedPoints > 0)
		{
			this._tempUnclaimedPoints--;
			this._tempTotalPoints++;
			this._unclaimedPoints.text = this._tempUnclaimedPoints.ToString();
			if (this._tempUnclaimedPoints == 0)
			{
				this._audioSource.PlayOneShot(this._claimPointFinalSFX);
			}
			else
			{
				this._audioSource.PlayOneShot(this._claimPointDefaultSFX);
			}
			yield return new WaitForSeconds(this._claimDelayPerPoint);
		}
		this._isUpdatingPointCount = false;
		this.UpdateProgressDisplays();
		yield break;
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00088238 File Offset: 0x00086438
	[PunRPC]
	private void BroadcastRedeemQuestPoints(int redeemedPointCount, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "BroadcastRedeemQuestPoints");
		PhotonMessageInfoWrapped photonMessageInfoWrapped = new PhotonMessageInfoWrapped(info);
		RigContainer rigContainer;
		if (photonMessageInfoWrapped.Sender != null && VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			if (!FXSystem.CheckCallSpam(rigContainer.Rig.fxSettings, 10, (double)Time.unscaledTime))
			{
				return;
			}
			redeemedPointCount = Mathf.Min(redeemedPointCount, 50);
			Coroutine routine;
			if (this.perPlayerRedemptionSequence.TryGetValue(info.Sender, out routine))
			{
				base.StopCoroutine(routine);
				this.perPlayerRedemptionSequence.Remove(info.Sender);
			}
			Coroutine value = base.StartCoroutine(this.PerformRemotePointRedemptionSequence(info.Sender, redeemedPointCount));
			this.perPlayerRedemptionSequence.Add(info.Sender, value);
		}
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x0003436B File Offset: 0x0003256B
	private IEnumerator PerformRemotePointRedemptionSequence(NetPlayer player, int redeemedPointCount)
	{
		while (redeemedPointCount > 0)
		{
			int num = redeemedPointCount;
			redeemedPointCount = num - 1;
			if (redeemedPointCount == 0)
			{
				this._audioSource.PlayOneShot(this._claimPointFinalSFX);
			}
			else
			{
				this._audioSource.PlayOneShot(this._claimPointDefaultSFX);
			}
			yield return new WaitForSeconds(this._claimDelayPerPoint);
		}
		this.perPlayerRedemptionSequence.Remove(player);
		yield break;
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00088308 File Offset: 0x00086508
	private void BuildQuestList()
	{
		this.DestroyQuestList();
		RotatingQuestsManager.RotatingQuestList quests = this._questManager.quests;
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup in quests.DailyQuests)
		{
			foreach (RotatingQuestsManager.RotatingQuest rotatingQuest in rotatingQuestGroup.quests)
			{
				if (rotatingQuest.isQuestActive)
				{
					QuestDisplay questDisplay = UnityEngine.Object.Instantiate<QuestDisplay>(this._questDisplayPrefab, this._dailyQuestContainer);
					questDisplay.quest = rotatingQuest;
					this._quests.Add(questDisplay);
				}
			}
		}
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup2 in quests.WeeklyQuests)
		{
			foreach (RotatingQuestsManager.RotatingQuest rotatingQuest2 in rotatingQuestGroup2.quests)
			{
				if (rotatingQuest2.isQuestActive)
				{
					QuestDisplay questDisplay2 = UnityEngine.Object.Instantiate<QuestDisplay>(this._questDisplayPrefab, this._weeklyQuestContainer);
					questDisplay2.quest = rotatingQuest2;
					this._quests.Add(questDisplay2);
				}
			}
		}
		foreach (QuestDisplay questDisplay3 in this._quests)
		{
			questDisplay3.UpdateDisplay();
		}
		if (!this._hasBuiltQuestList)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(this._questContainerParent);
			this._hasBuiltQuestList = true;
			return;
		}
		LayoutRebuilder.MarkLayoutForRebuild(this._questContainerParent);
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x00034388 File Offset: 0x00032588
	private void DestroyQuestList()
	{
		MonkeBusinessStation.<DestroyQuestList>g__DestroyChildren|40_0(this._dailyQuestContainer);
		MonkeBusinessStation.<DestroyQuestList>g__DestroyChildren|40_0(this._weeklyQuestContainer);
		this._quests.Clear();
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x000884DC File Offset: 0x000866DC
	[CompilerGenerated]
	internal static void <DestroyQuestList>g__DestroyChildren|40_0(Transform parent)
	{
		for (int i = parent.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(parent.GetChild(i).gameObject);
		}
	}

	// Token: 0x0400088D RID: 2189
	[SerializeField]
	private RectTransform _questContainerParent;

	// Token: 0x0400088E RID: 2190
	[SerializeField]
	private RectTransform _dailyQuestContainer;

	// Token: 0x0400088F RID: 2191
	[SerializeField]
	private RectTransform _weeklyQuestContainer;

	// Token: 0x04000890 RID: 2192
	[SerializeField]
	private QuestDisplay _questDisplayPrefab;

	// Token: 0x04000891 RID: 2193
	[SerializeField]
	private List<QuestDisplay> _quests;

	// Token: 0x04000892 RID: 2194
	[SerializeField]
	private ProgressDisplay _weeklyProgress;

	// Token: 0x04000893 RID: 2195
	[SerializeField]
	private TMP_Text _unclaimedPoints;

	// Token: 0x04000894 RID: 2196
	[SerializeField]
	private GorillaPressableButton _claimButton;

	// Token: 0x04000895 RID: 2197
	[SerializeField]
	private AudioSource _audioSource;

	// Token: 0x04000896 RID: 2198
	[SerializeField]
	private GameObject _claimablePointsObject;

	// Token: 0x04000897 RID: 2199
	[SerializeField]
	private GameObject _noClaimablePointsObject;

	// Token: 0x04000898 RID: 2200
	[SerializeField]
	private Transform _claimablePointsBadgePosition;

	// Token: 0x04000899 RID: 2201
	[SerializeField]
	private Transform _noClaimablePointsBadgePosition;

	// Token: 0x0400089A RID: 2202
	[SerializeField]
	private Transform _badgeMount;

	// Token: 0x0400089B RID: 2203
	[Space]
	[SerializeField]
	private float _claimDelayPerPoint = 0.12f;

	// Token: 0x0400089C RID: 2204
	[SerializeField]
	private AudioClip _claimPointDefaultSFX;

	// Token: 0x0400089D RID: 2205
	[SerializeField]
	private AudioClip _claimPointFinalSFX;

	// Token: 0x0400089E RID: 2206
	[Header("Quest Timers")]
	[SerializeField]
	private CountdownText _dailyCountdown;

	// Token: 0x0400089F RID: 2207
	[SerializeField]
	private CountdownText _weeklyCountdown;

	// Token: 0x040008A0 RID: 2208
	private RotatingQuestsManager _questManager;

	// Token: 0x040008A1 RID: 2209
	private int _lastQuestChange = -1;

	// Token: 0x040008A2 RID: 2210
	private int _lastQuestDailyID = -1;

	// Token: 0x040008A3 RID: 2211
	private bool _isUpdatingPointCount;

	// Token: 0x040008A4 RID: 2212
	private int _tempUnclaimedPoints;

	// Token: 0x040008A5 RID: 2213
	private int _tempTotalPoints;

	// Token: 0x040008A6 RID: 2214
	private bool _hasBuiltQuestList;

	// Token: 0x040008A7 RID: 2215
	private Dictionary<NetPlayer, Coroutine> perPlayerRedemptionSequence = new Dictionary<NetPlayer, Coroutine>();
}
