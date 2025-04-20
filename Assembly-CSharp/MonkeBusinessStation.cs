using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameObjectScheduling;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000117 RID: 279
public class MonkeBusinessStation : MonoBehaviourPunCallbacks
{
	// Token: 0x06000772 RID: 1906 RVA: 0x000354FD File Offset: 0x000336FD
	public override void OnEnable()
	{
		base.OnEnable();
		this.FindQuestManager();
		ProgressionController.OnQuestSelectionChanged += this.OnQuestSelectionChanged;
		ProgressionController.OnProgressEvent += this.OnProgress;
		ProgressionController.RequestProgressUpdate();
		this.UpdateCountdownTimers();
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x00035538 File Offset: 0x00033738
	public override void OnDisable()
	{
		base.OnDisable();
		ProgressionController.OnQuestSelectionChanged -= this.OnQuestSelectionChanged;
		ProgressionController.OnProgressEvent -= this.OnProgress;
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x00035562 File Offset: 0x00033762
	private void FindQuestManager()
	{
		if (!this._questManager)
		{
			this._questManager = UnityEngine.Object.FindObjectOfType<RotatingQuestsManager>();
		}
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0003557C File Offset: 0x0003377C
	private void UpdateCountdownTimers()
	{
		this._dailyCountdown.SetCountdownTime(this._questManager.DailyQuestCountdown);
		this._weeklyCountdown.SetCountdownTime(this._questManager.WeeklyQuestCountdown);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x000355AA File Offset: 0x000337AA
	private void OnQuestSelectionChanged()
	{
		this.UpdateCountdownTimers();
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x000355B2 File Offset: 0x000337B2
	private void OnProgress()
	{
		this.UpdateQuestStatus();
		this.UpdateProgressDisplays();
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x0008A95C File Offset: 0x00088B5C
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

	// Token: 0x06000779 RID: 1913 RVA: 0x0008AA10 File Offset: 0x00088C10
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

	// Token: 0x0600077A RID: 1914 RVA: 0x0008AAB4 File Offset: 0x00088CB4
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

	// Token: 0x0600077B RID: 1915 RVA: 0x000355C0 File Offset: 0x000337C0
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

	// Token: 0x0600077C RID: 1916 RVA: 0x0008AB40 File Offset: 0x00088D40
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
			Coroutine coroutine;
			if (this.perPlayerRedemptionSequence.TryGetValue(info.Sender, out coroutine))
			{
				if (coroutine != null)
				{
					base.StopCoroutine(coroutine);
				}
				this.perPlayerRedemptionSequence.Remove(info.Sender);
			}
			if (base.gameObject.activeInHierarchy)
			{
				Coroutine value = base.StartCoroutine(this.PerformRemotePointRedemptionSequence(info.Sender, redeemedPointCount));
				this.perPlayerRedemptionSequence.Add(info.Sender, value);
			}
		}
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x000355CF File Offset: 0x000337CF
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

	// Token: 0x0600077E RID: 1918 RVA: 0x0008AC20 File Offset: 0x00088E20
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

	// Token: 0x0600077F RID: 1919 RVA: 0x000355EC File Offset: 0x000337EC
	private void DestroyQuestList()
	{
		MonkeBusinessStation.<DestroyQuestList>g__DestroyChildren|40_0(this._dailyQuestContainer);
		MonkeBusinessStation.<DestroyQuestList>g__DestroyChildren|40_0(this._weeklyQuestContainer);
		this._quests.Clear();
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x0008ADF4 File Offset: 0x00088FF4
	[CompilerGenerated]
	internal static void <DestroyQuestList>g__DestroyChildren|40_0(Transform parent)
	{
		for (int i = parent.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(parent.GetChild(i).gameObject);
		}
	}

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	private RectTransform _questContainerParent;

	// Token: 0x040008CE RID: 2254
	[SerializeField]
	private RectTransform _dailyQuestContainer;

	// Token: 0x040008CF RID: 2255
	[SerializeField]
	private RectTransform _weeklyQuestContainer;

	// Token: 0x040008D0 RID: 2256
	[SerializeField]
	private QuestDisplay _questDisplayPrefab;

	// Token: 0x040008D1 RID: 2257
	[SerializeField]
	private List<QuestDisplay> _quests;

	// Token: 0x040008D2 RID: 2258
	[SerializeField]
	private ProgressDisplay _weeklyProgress;

	// Token: 0x040008D3 RID: 2259
	[SerializeField]
	private TMP_Text _unclaimedPoints;

	// Token: 0x040008D4 RID: 2260
	[SerializeField]
	private GorillaPressableButton _claimButton;

	// Token: 0x040008D5 RID: 2261
	[SerializeField]
	private AudioSource _audioSource;

	// Token: 0x040008D6 RID: 2262
	[SerializeField]
	private GameObject _claimablePointsObject;

	// Token: 0x040008D7 RID: 2263
	[SerializeField]
	private GameObject _noClaimablePointsObject;

	// Token: 0x040008D8 RID: 2264
	[SerializeField]
	private Transform _claimablePointsBadgePosition;

	// Token: 0x040008D9 RID: 2265
	[SerializeField]
	private Transform _noClaimablePointsBadgePosition;

	// Token: 0x040008DA RID: 2266
	[SerializeField]
	private Transform _badgeMount;

	// Token: 0x040008DB RID: 2267
	[Space]
	[SerializeField]
	private float _claimDelayPerPoint = 0.12f;

	// Token: 0x040008DC RID: 2268
	[SerializeField]
	private AudioClip _claimPointDefaultSFX;

	// Token: 0x040008DD RID: 2269
	[SerializeField]
	private AudioClip _claimPointFinalSFX;

	// Token: 0x040008DE RID: 2270
	[Header("Quest Timers")]
	[SerializeField]
	private CountdownText _dailyCountdown;

	// Token: 0x040008DF RID: 2271
	[SerializeField]
	private CountdownText _weeklyCountdown;

	// Token: 0x040008E0 RID: 2272
	private RotatingQuestsManager _questManager;

	// Token: 0x040008E1 RID: 2273
	private int _lastQuestChange = -1;

	// Token: 0x040008E2 RID: 2274
	private int _lastQuestDailyID = -1;

	// Token: 0x040008E3 RID: 2275
	private bool _isUpdatingPointCount;

	// Token: 0x040008E4 RID: 2276
	private int _tempUnclaimedPoints;

	// Token: 0x040008E5 RID: 2277
	private int _tempTotalPoints;

	// Token: 0x040008E6 RID: 2278
	private bool _hasBuiltQuestList;

	// Token: 0x040008E7 RID: 2279
	private Dictionary<NetPlayer, Coroutine> perPlayerRedemptionSequence = new Dictionary<NetPlayer, Coroutine>();
}
