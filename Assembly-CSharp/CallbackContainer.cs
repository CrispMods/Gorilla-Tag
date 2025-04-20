using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000856 RID: 2134
internal class CallbackContainer<T> where T : ICallBack
{
	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x0600340F RID: 13327 RVA: 0x0005235A File Offset: 0x0005055A
	public int Count
	{
		get
		{
			return this.callbackList.Count;
		}
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x00052367 File Offset: 0x00050567
	public CallbackContainer()
	{
		this.callbackList = new List<T>(100);
		this.currentIndex = -1;
		this.callbackCount = -1;
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x0005238A File Offset: 0x0005058A
	public CallbackContainer(int capacity)
	{
		this.callbackList = new List<T>(capacity);
		this.currentIndex = -1;
		this.callbackCount = -1;
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x000523AC File Offset: 0x000505AC
	public virtual void Add(T inCallback)
	{
		this.callbackCount++;
		this.callbackList.Add(inCallback);
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x0013CFFC File Offset: 0x0013B1FC
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

	// Token: 0x06003414 RID: 13332 RVA: 0x0013D04C File Offset: 0x0013B24C
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

	// Token: 0x06003415 RID: 13333 RVA: 0x000523C8 File Offset: 0x000505C8
	public virtual void Clear()
	{
		this.callbackList.Clear();
	}

	// Token: 0x0400376E RID: 14190
	protected List<T> callbackList;

	// Token: 0x0400376F RID: 14191
	protected int currentIndex;

	// Token: 0x04003770 RID: 14192
	protected int callbackCount;
}
