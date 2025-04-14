using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200083C RID: 2108
internal class CallbackContainer<T> where T : ICallBack
{
	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x06003354 RID: 13140 RVA: 0x000F540D File Offset: 0x000F360D
	public int Count
	{
		get
		{
			return this.callbackList.Count;
		}
	}

	// Token: 0x06003355 RID: 13141 RVA: 0x000F541A File Offset: 0x000F361A
	public CallbackContainer()
	{
		this.callbackList = new List<T>(100);
		this.currentIndex = -1;
		this.callbackCount = -1;
	}

	// Token: 0x06003356 RID: 13142 RVA: 0x000F543D File Offset: 0x000F363D
	public CallbackContainer(int capacity)
	{
		this.callbackList = new List<T>(capacity);
		this.currentIndex = -1;
		this.callbackCount = -1;
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x000F545F File Offset: 0x000F365F
	public virtual void Add(T inCallback)
	{
		this.callbackCount++;
		this.callbackList.Add(inCallback);
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x000F547C File Offset: 0x000F367C
	public virtual void Remove(T inCallback)
	{
		int num = this.callbackList.IndexOf(inCallback);
		if (num > -1)
		{
			if (num <= this.currentIndex)
			{
				this.currentIndex--;
			}
			this.callbackCount--;
			this.callbackList.RemoveAt(num);
		}
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x000F54CC File Offset: 0x000F36CC
	public virtual void TryRunCallbacks()
	{
		this.callbackCount = this.callbackList.Count;
		this.currentIndex = 0;
		while (this.currentIndex < this.callbackCount)
		{
			try
			{
				T t = this.callbackList[this.currentIndex];
				t.CallBack();
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.ToString());
			}
			this.currentIndex++;
		}
	}

	// Token: 0x0600335A RID: 13146 RVA: 0x000F5550 File Offset: 0x000F3750
	public virtual void Clear()
	{
		this.callbackList.Clear();
	}

	// Token: 0x040036B2 RID: 14002
	protected List<T> callbackList;

	// Token: 0x040036B3 RID: 14003
	protected int currentIndex;

	// Token: 0x040036B4 RID: 14004
	protected int callbackCount;
}
