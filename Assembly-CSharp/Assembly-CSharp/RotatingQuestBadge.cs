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
	// (get) Token: 0x060007CD RID: 1997 RVA: 0x0002B4D3 File Offset: 0x000296D3
	// (set) Token: 0x060007CE RID: 1998 RVA: 0x0002B4DB File Offset: 0x000296DB
	public bool IsSpawned { get; set; }

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060007CF RID: 1999 RVA: 0x0002B4E4 File Offset: 0x000296E4
	// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0002B4EC File Offset: 0x000296EC
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060007D1 RID: 2001 RVA: 0x0002B4F8 File Offset: 0x000296F8
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

	// Token: 0x060007D2 RID: 2002 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnDespawn()
	{
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0002B551 File Offset: 0x00029751
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

	// Token: 0x060007D4 RID: 2004 RVA: 0x0002B577 File Offset: 0x00029777
	private void OnDisable()
	{
		if (this.forWardrobe && this.myRig)
		{
			this.myRig.OnQuestScoreChanged -= this.OnProgressScoreChanged;
			this.myRig = null;
		}
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0002B5AC File Offset: 0x000297AC
	private IEnumerator DoFindRig()
	{
		WaitForSeconds intervalWait = new WaitForSeconds(0.1f);
		while (!this.TryGetRig())
		{
			yield return intervalWait;
		}
		yield break;
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x0002B5BC File Offset: 0x000297BC
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

	// Token: 0x060007D7 RID: 2007 RVA: 0x0002B618 File Offset: 0x00029818
	private void OnProgressScoreChanged(int score)
	{
		score = Mathf.Clamp(score, 0, 99999);
		this.displayField.text = score.ToString();
		this.UpdateBadge(score);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0002B644 File Offset: 0x00029844
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

	// Token: 0x060007D9 RID: 2009 RVA: 0x0002B6AC File Offset: 0x000298AC
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
