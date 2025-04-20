using System;
using UnityEngine;

// Token: 0x02000849 RID: 2121
public abstract class BasePageHandler : MonoBehaviour
{
	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x060033E6 RID: 13286 RVA: 0x0005224E File Offset: 0x0005044E
	// (set) Token: 0x060033E7 RID: 13287 RVA: 0x00052256 File Offset: 0x00050456
	private protected int selectedIndex { protected get; private set; }

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x060033E8 RID: 13288 RVA: 0x0005225F File Offset: 0x0005045F
	// (set) Token: 0x060033E9 RID: 13289 RVA: 0x00052267 File Offset: 0x00050467
	private protected int currentPage { protected get; private set; }

	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x060033EA RID: 13290 RVA: 0x00052270 File Offset: 0x00050470
	// (set) Token: 0x060033EB RID: 13291 RVA: 0x00052278 File Offset: 0x00050478
	private protected int pages { protected get; private set; }

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x060033EC RID: 13292 RVA: 0x00052281 File Offset: 0x00050481
	// (set) Token: 0x060033ED RID: 13293 RVA: 0x00052289 File Offset: 0x00050489
	private protected int maxEntires { protected get; private set; }

	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x060033EE RID: 13294
	protected abstract int pageSize { get; }

	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x060033EF RID: 13295
	protected abstract int entriesCount { get; }

	// Token: 0x060033F0 RID: 13296 RVA: 0x0013C394 File Offset: 0x0013A594
	protected virtual void Start()
	{
		Debug.Log("base page handler " + this.entriesCount.ToString() + " " + this.pageSize.ToString());
		this.pages = this.entriesCount / this.pageSize + 1;
		this.maxEntires = this.pages * this.pageSize;
	}

	// Token: 0x060033F1 RID: 13297 RVA: 0x0013C3FC File Offset: 0x0013A5FC
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

	// Token: 0x060033F2 RID: 13298 RVA: 0x0013C438 File Offset: 0x0013A638
	public void SelectEntryFromIndex(int index)
	{
		this.selectedIndex = index;
		this.currentPage = this.selectedIndex / this.pageSize;
		int pageEntry = index - this.pageSize * this.currentPage;
		this.PageEntrySelected(pageEntry, index);
		this.SetPage(this.currentPage);
	}

	// Token: 0x060033F3 RID: 13299 RVA: 0x0013C484 File Offset: 0x0013A684
	public void ChangePage(bool left)
	{
		int num = left ? -1 : 1;
		this.SetPage(Mathf.Abs((this.currentPage + num) % this.pages));
	}

	// Token: 0x060033F4 RID: 13300 RVA: 0x0013C4B4 File Offset: 0x0013A6B4
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

	// Token: 0x060033F5 RID: 13301
	protected abstract void ShowPage(int selectedPage, int startIndex, int endIndex);

	// Token: 0x060033F6 RID: 13302
	protected abstract void PageEntrySelected(int pageEntry, int selectionIndex);
}
