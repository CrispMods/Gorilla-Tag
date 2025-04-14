using System;
using UnityEngine;

// Token: 0x0200082F RID: 2095
public abstract class BasePageHandler : MonoBehaviour
{
	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x0600332B RID: 13099 RVA: 0x000F469C File Offset: 0x000F289C
	// (set) Token: 0x0600332C RID: 13100 RVA: 0x000F46A4 File Offset: 0x000F28A4
	private protected int selectedIndex { protected get; private set; }

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x0600332D RID: 13101 RVA: 0x000F46AD File Offset: 0x000F28AD
	// (set) Token: 0x0600332E RID: 13102 RVA: 0x000F46B5 File Offset: 0x000F28B5
	private protected int currentPage { protected get; private set; }

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x0600332F RID: 13103 RVA: 0x000F46BE File Offset: 0x000F28BE
	// (set) Token: 0x06003330 RID: 13104 RVA: 0x000F46C6 File Offset: 0x000F28C6
	private protected int pages { protected get; private set; }

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x06003331 RID: 13105 RVA: 0x000F46CF File Offset: 0x000F28CF
	// (set) Token: 0x06003332 RID: 13106 RVA: 0x000F46D7 File Offset: 0x000F28D7
	private protected int maxEntires { protected get; private set; }

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x06003333 RID: 13107
	protected abstract int pageSize { get; }

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x06003334 RID: 13108
	protected abstract int entriesCount { get; }

	// Token: 0x06003335 RID: 13109 RVA: 0x000F46E0 File Offset: 0x000F28E0
	protected virtual void Start()
	{
		Debug.Log("base page handler " + this.entriesCount.ToString() + " " + this.pageSize.ToString());
		this.pages = this.entriesCount / this.pageSize + 1;
		this.maxEntires = this.pages * this.pageSize;
	}

	// Token: 0x06003336 RID: 13110 RVA: 0x000F4748 File Offset: 0x000F2948
	public void SelectEntryOnPage(int entryIndex)
	{
		int num = entryIndex + this.pageSize * this.currentPage;
		if (num > this.entriesCount)
		{
			return;
		}
		this.selectedIndex = num;
		this.PageEntrySelected(entryIndex, this.selectedIndex);
	}

	// Token: 0x06003337 RID: 13111 RVA: 0x000F4784 File Offset: 0x000F2984
	public void SelectEntryFromIndex(int index)
	{
		this.selectedIndex = index;
		this.currentPage = this.selectedIndex / this.pageSize;
		int pageEntry = index - this.pageSize * this.currentPage;
		this.PageEntrySelected(pageEntry, index);
		this.SetPage(this.currentPage);
	}

	// Token: 0x06003338 RID: 13112 RVA: 0x000F47D0 File Offset: 0x000F29D0
	public void ChangePage(bool left)
	{
		int num = left ? -1 : 1;
		this.SetPage(Mathf.Abs((this.currentPage + num) % this.pages));
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x000F4800 File Offset: 0x000F2A00
	public void SetPage(int page)
	{
		if (page > this.pages)
		{
			return;
		}
		this.currentPage = page;
		int num = this.pageSize * page;
		this.ShowPage(this.currentPage, num, Mathf.Min(num + this.pageSize, this.entriesCount));
	}

	// Token: 0x0600333A RID: 13114
	protected abstract void ShowPage(int selectedPage, int startIndex, int endIndex);

	// Token: 0x0600333B RID: 13115
	protected abstract void PageEntrySelected(int pageEntry, int selectionIndex);
}
