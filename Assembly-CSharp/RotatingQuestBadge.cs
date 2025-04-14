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
	// (get) Token: 0x060007CB RID: 1995 RVA: 0x0002B1AF File Offset: 0x000293AF
	// (set) Token: 0x060007CC RID: 1996 RVA: 0x0002B1B7 File Offset: 0x000293B7
	public bool IsSpawned { get; set; }

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060007CD RID: 1997 RVA: 0x0002B1C0 File Offset: 0x000293C0
	// (set) Token: 0x060007CE RID: 1998 RVA: 0x0002B1C8 File Offset: 0x000293C8
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060007CF RID: 1999 RVA: 0x0002B1D4 File Offset: 0x000293D4
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

	// Token: 0x060007D0 RID: 2000 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnDespawn()
	{
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x0002B22D File Offset: 0x0002942D
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

	// Token: 0x060007D2 RID: 2002 RVA: 0x0002B253 File Offset: 0x00029453
	private void OnDisable()
	{
		if (this.forWardrobe && this.myRig)
		{
			this.myRig.OnQuestScoreChanged -= this.OnProgressScoreChanged;
			this.myRig = null;
		}
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0002B288 File Offset: 0x00029488
	private IEnumerator DoFindRig()
	{
		WaitForSeconds intervalWait = new WaitForSeconds(0.1f);
		while (!this.TryGetRig())
		{
			yield return intervalWait;
		}
		yield break;
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0002B298 File Offset: 0x00029498
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

	// Token: 0x060007D5 RID: 2005 RVA: 0x0002B2F4 File Offset: 0x000294F4
	private void OnProgressScoreChanged(int score)
	{
		score = Mathf.Clamp(score, 0, 99999);
		this.displayField.text = score.ToString();
		this.UpdateBadge(score);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x0002B320 File Offset: 0x00029520
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

	// Token: 0x060007D7 RID: 2007 RVA: 0x0002B388 File Offset: 0x00029588
	private void SetBadgeLevel(int level)
	{
		level = Mathf.Clamp(level, 0, this.badgeLevels.Length - 1);
		for (int i = 0; i < this.badgeLevels.Length; i++)
		{
			this.badgeLevels[i].badge.SetActive(i == level);
		}
	}

	// Token: 0x04000923 RID: 2339
	[SerializeField]
	private TextMeshPro displayField;

	// Token: 0x04000924 RID: 2340
	[SerializeField]
	private bool forWardrobe;

	// Token: 0x04000925 RID: 2341
	[SerializeField]
	private VRRig myRig;

	// Token: 0x04000926 RID: 2342
	[SerializeField]
	private RotatingQuestBadge.BadgeLevel[] badgeLevels;

	// Token: 0x02000123 RID: 291
	[Serializable]
	public struct BadgeLevel
	{
		// Token: 0x04000929 RID: 2345
		public GameObject badge;

		// Token: 0x0400092A RID: 2346
		public int requiredPoints;
	}
}
