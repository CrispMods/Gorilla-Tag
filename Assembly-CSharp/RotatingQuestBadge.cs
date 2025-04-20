using System;
using System.Collections;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using TMPro;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class RotatingQuestBadge : MonoBehaviour, ISpawnable
{
	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x0600080F RID: 2063 RVA: 0x00035AC6 File Offset: 0x00033CC6
	// (set) Token: 0x06000810 RID: 2064 RVA: 0x00035ACE File Offset: 0x00033CCE
	public bool IsSpawned { get; set; }

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000811 RID: 2065 RVA: 0x00035AD7 File Offset: 0x00033CD7
	// (set) Token: 0x06000812 RID: 2066 RVA: 0x00035ADF File Offset: 0x00033CDF
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000813 RID: 2067 RVA: 0x0008C900 File Offset: 0x0008AB00
	public void OnSpawn(VRRig rig)
	{
		if (this.forWardrobe && !this.myRig)
		{
			this.TryGetRig();
			return;
		}
		this.myRig = rig;
		this.myRig.OnQuestScoreChanged += this.OnProgressScoreChanged;
		this.OnProgressScoreChanged(this.myRig.GetCurrentQuestScore());
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnDespawn()
	{
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x00035AE8 File Offset: 0x00033CE8
	private void OnEnable()
	{
		if (this.forWardrobe)
		{
			this.SetBadgeLevel(-1);
			if (!this.TryGetRig())
			{
				base.StartCoroutine(this.DoFindRig());
			}
		}
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00035B0E File Offset: 0x00033D0E
	private void OnDisable()
	{
		if (this.forWardrobe && this.myRig)
		{
			this.myRig.OnQuestScoreChanged -= this.OnProgressScoreChanged;
			this.myRig = null;
		}
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00035B43 File Offset: 0x00033D43
	private IEnumerator DoFindRig()
	{
		WaitForSeconds intervalWait = new WaitForSeconds(0.1f);
		while (!this.TryGetRig())
		{
			yield return intervalWait;
		}
		yield break;
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x0008C95C File Offset: 0x0008AB5C
	private bool TryGetRig()
	{
		GorillaTagger instance = GorillaTagger.Instance;
		this.myRig = ((instance != null) ? instance.offlineVRRig : null);
		if (this.myRig)
		{
			this.myRig.OnQuestScoreChanged += this.OnProgressScoreChanged;
			this.OnProgressScoreChanged(this.myRig.GetCurrentQuestScore());
			return true;
		}
		return false;
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00035B52 File Offset: 0x00033D52
	private void OnProgressScoreChanged(int score)
	{
		score = Mathf.Clamp(score, 0, 99999);
		this.displayField.text = score.ToString();
		this.UpdateBadge(score);
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0008C9B8 File Offset: 0x0008ABB8
	private void UpdateBadge(int score)
	{
		int num = -1;
		int badgeLevel = -1;
		for (int i = 0; i < this.badgeLevels.Length; i++)
		{
			if (this.badgeLevels[i].requiredPoints <= score && this.badgeLevels[i].requiredPoints > num)
			{
				num = this.badgeLevels[i].requiredPoints;
				badgeLevel = i;
			}
		}
		this.SetBadgeLevel(badgeLevel);
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0008CA20 File Offset: 0x0008AC20
	private void SetBadgeLevel(int level)
	{
		level = Mathf.Clamp(level, 0, this.badgeLevels.Length - 1);
		for (int i = 0; i < this.badgeLevels.Length; i++)
		{
			this.badgeLevels[i].badge.SetActive(i == level);
		}
	}

	// Token: 0x04000965 RID: 2405
	[SerializeField]
	private TextMeshPro displayField;

	// Token: 0x04000966 RID: 2406
	[SerializeField]
	private bool forWardrobe;

	// Token: 0x04000967 RID: 2407
	[SerializeField]
	private VRRig myRig;

	// Token: 0x04000968 RID: 2408
	[SerializeField]
	private RotatingQuestBadge.BadgeLevel[] badgeLevels;

	// Token: 0x0200012D RID: 301
	[Serializable]
	public struct BadgeLevel
	{
		// Token: 0x0400096B RID: 2411
		public GameObject badge;

		// Token: 0x0400096C RID: 2412
		public int requiredPoints;
	}
}
