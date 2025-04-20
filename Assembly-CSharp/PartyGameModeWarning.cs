using System;
using System.Collections;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class PartyGameModeWarning : MonoBehaviour
{
	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00037C4A File Offset: 0x00035E4A
	public bool ShouldShowWarning
	{
		get
		{
			return FriendshipGroupDetection.Instance.IsInParty && FriendshipGroupDetection.Instance.AnyPartyMembersOutsideFriendCollider();
		}
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0009985C File Offset: 0x00097A5C
	private void Awake()
	{
		GameObject[] array = this.showParts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x00037C64 File Offset: 0x00035E64
	public void Show()
	{
		this.visibleUntilTimestamp = Time.time + this.visibleDuration;
		if (this.hideCoroutine == null)
		{
			this.hideCoroutine = base.StartCoroutine(this.HideCo());
		}
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x00037C92 File Offset: 0x00035E92
	private IEnumerator HideCo()
	{
		GameObject[] array = this.showParts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
		array = this.hideParts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		float lastVisible;
		do
		{
			lastVisible = this.visibleUntilTimestamp;
			yield return new WaitForSeconds(this.visibleUntilTimestamp - Time.time);
		}
		while (lastVisible != this.visibleUntilTimestamp);
		array = this.showParts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		array = this.hideParts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
		this.hideCoroutine = null;
		yield break;
	}

	// Token: 0x04000D70 RID: 3440
	[SerializeField]
	private GameObject[] showParts;

	// Token: 0x04000D71 RID: 3441
	[SerializeField]
	private GameObject[] hideParts;

	// Token: 0x04000D72 RID: 3442
	[SerializeField]
	private float visibleDuration;

	// Token: 0x04000D73 RID: 3443
	private float visibleUntilTimestamp;

	// Token: 0x04000D74 RID: 3444
	private Coroutine hideCoroutine;
}
