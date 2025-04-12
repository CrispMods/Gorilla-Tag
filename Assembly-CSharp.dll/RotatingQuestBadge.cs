using System;
using System.Collections;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using TMPro;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class RotatingQuestBadge : MonoBehaviour, ISpawnable
{
	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060007CD RID: 1997 RVA: 0x00034850 File Offset: 0x00032A50
	// (set) Token: 0x060007CE RID: 1998 RVA: 0x00034858 File Offset: 0x00032A58
	public bool IsSpawned { get; set; }

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060007CF RID: 1999 RVA: 0x00034861 File Offset: 0x00032A61
	// (set) Token: 0x060007D0 RID: 2000 RVA: 0x00034869 File Offset: 0x00032A69
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060007D1 RID: 2001 RVA: 0x00089F80 File Offset: 0x00088180
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

	// Token: 0x060007D2 RID: 2002 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnDespawn()
	{
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00034872 File Offset: 0x00032A72
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

	// Token: 0x060007D4 RID: 2004 RVA: 0x00034898 File Offset: 0x00032A98
	private void OnDisable()
	{
		if (this.forWardrobe && this.myRig)
		{
			this.myRig.OnQuestScoreChanged -= this.OnProgressScoreChanged;
			this.myRig = null;
		}
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x000348CD File Offset: 0x00032ACD
	private IEnumerator DoFindRig()
	{
		WaitForSeconds intervalWait = new WaitForSeconds(0.1f);
		while (!this.TryGetRig())
		{
			yield return intervalWait;
		}
		yield break;
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00089FDC File Offset: 0x000881DC
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

	// Token: 0x060007D7 RID: 2007 RVA: 0x000348DC File Offset: 0x00032ADC
	private void OnProgressScoreChanged(int score)
	{
		score = Mathf.Clamp(score, 0, 99999);
		this.displayField.text = score.ToString();
		this.UpdateBadge(score);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0008A038 File Offset: 0x00088238
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

	// Token: 0x060007D9 RID: 2009 RVA: 0x0008A0A0 File Offset: 0x000882A0
	private void SetBadgeLevel(int level)
	{
		level = Mathf.Clamp(level, 0, this.badgeLevels.Length - 1);
		for (int i = 0; i < this.badgeLevels.Length; i++)
		{
			this.badgeLevels[i].badge.SetActive(i == level);
		}
	}

	// Token: 0x04000924 RID: 2340
	[SerializeField]
	private TextMeshPro displayField;

	// Token: 0x04000925 RID: 2341
	[SerializeField]
	private bool forWardrobe;

	// Token: 0x04000926 RID: 2342
	[SerializeField]
	private VRRig myRig;

	// Token: 0x04000927 RID: 2343
	[SerializeField]
	private RotatingQuestBadge.BadgeLevel[] badgeLevels;

	// Token: 0x02000123 RID: 291
	[Serializable]
	public struct BadgeLevel
	{
		// Token: 0x0400092A RID: 2346
		public GameObject badge;

		// Token: 0x0400092B RID: 2347
		public int requiredPoints;
	}
}
