using System;
using System.Collections;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020001CC RID: 460
public class PartyGameModeWarning : MonoBehaviour
{
	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0003A5F8 File Offset: 0x000387F8
	public bool ShouldShowWarning
	{
		get
		{
			return FriendshipGroupDetection.Instance.IsInParty && FriendshipGroupDetection.Instance.AnyPartyMembersOutsideFriendCollider();
		}
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x0003A614 File Offset: 0x00038814
	private void Awake()
	{
		GameObject[] array = this.showParts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x0003A63F File Offset: 0x0003883F
	public void Show()
	{
		this.visibleUntilTimestamp = Time.time + this.visibleDuration;
		if (this.hideCoroutine == null)
		{
			this.hideCoroutine = base.StartCoroutine(this.HideCo());
		}
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x0003A66D File Offset: 0x0003886D
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

	// Token: 0x04000D2B RID: 3371
	[SerializeField]
	private GameObject[] showParts;

	// Token: 0x04000D2C RID: 3372
	[SerializeField]
	private GameObject[] hideParts;

	// Token: 0x04000D2D RID: 3373
	[SerializeField]
	private float visibleDuration;

	// Token: 0x04000D2E RID: 3374
	private float visibleUntilTimestamp;

	// Token: 0x04000D2F RID: 3375
	private Coroutine hideCoroutine;
}
