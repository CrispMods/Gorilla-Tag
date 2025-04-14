using System;
using UnityEngine;

// Token: 0x02000832 RID: 2098
public abstract class BasePageHandler : MonoBehaviour
{
	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x06003337 RID: 13111 RVA: 0x000F4C64 File Offset: 0x000F2E64
	// (set) Token: 0x06003338 RID: 13112 RVA: 0x000F4C6C File Offset: 0x000F2E6C
	private protected int selectedIndex { protected get; private set; }

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x06003339 RID: 13113 RVA: 0x000F4C75 File Offset: 0x000F2E75
	// (set) Token: 0x0600333A RID: 13114 RVA: 0x000F4C7D File Offset: 0x000F2E7D
	private protected int currentPage { protected get; private set; }

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x0600333B RID: 13115 RVA: 0x000F4C86 File Offset: 0x000F2E86
	// (set) Token: 0x0600333C RID: 13116 RVA: 0x000F4C8E File Offset: 0x000F2E8E
	private protected int pages { protected get; private set; }

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x0600333D RID: 13117 RVA: 0x000F4C97 File Offset: 0x000F2E97
	// (set) Token: 0x0600333E RID: 13118 RVA: 0x000F4C9F File Offset: 0x000F2E9F
	private protected int maxEntires { protected get; private set; }

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x0600333F RID: 13119
	protected abstract int pageSize { get; }

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x06003340 RID: 13120
	protected abstract int entriesCount { get; }

	// Token: 0x06003341 RID: 13121 RVA: 0x000F4CA8 File Offset: 0x000F2EA8
	protected virtual void Start()
	{
		Debug.Log("base page handler " + this.entriesCount.ToString() + " " + this.pageSize.ToString());
		this.pages = this.entriesCount / this.pageSize + 1;
		this.maxEntires = this.pages * this.pageSize;
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x000F4D10 File Offset: 0x000F2F10
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

	// Token: 0x06003343 RID: 13123 RVA: 0x000F4D4C File Offset: 0x000F2F4C
	public void SelectEntryFromIndex(int index)
	{
		this.selectedIndex = index;
		this.currentPage = this.selectedIndex / this.pageSize;
		int pageEntry = index - this.pageSize * this.currentPage;
		this.PageEntrySelected(pageEntry, index);
		this.SetPage(this.currentPage);
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x000F4D98 File Offset: 0x000F2F98
	public void ChangePage(bool left)
	{
		int num = left ? -1 : 1;
		this.SetPage(Mathf.Abs((this.currentPage + num) % this.pages));
	}

	// Token: 0x06003345 RID: 13125 RVA: 0x000F4DC8 File Offset: 0x000F2FC8
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

	// Token: 0x06003346 RID: 13126
	protected abstract void ShowPage(int selectedPage, int startIndex, int endIndex);

	// Token: 0x06003347 RID: 13127
	protected abstract void PageEntrySelected(int pageEntry, int selectionIndex);
}
