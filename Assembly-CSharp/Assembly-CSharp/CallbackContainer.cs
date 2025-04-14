using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200083F RID: 2111
internal class CallbackContainer<T> where T : ICallBack
{
	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x06003360 RID: 13152 RVA: 0x000F59D5 File Offset: 0x000F3BD5
	public int Count
	{
		get
		{
			return this.callbackList.Count;
		}
	}

	// Token: 0x06003361 RID: 13153 RVA: 0x000F59E2 File Offset: 0x000F3BE2
	public CallbackContainer()
	{
		this.callbackList = new List<T>(100);
		this.currentIndex = -1;
		this.callbackCount = -1;
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x000F5A05 File Offset: 0x000F3C05
	public CallbackContainer(int capacity)
	{
		this.callbackList = new List<T>(capacity);
		this.currentIndex = -1;
		this.callbackCount = -1;
	}

	// Token: 0x06003363 RID: 13155 RVA: 0x000F5A27 File Offset: 0x000F3C27
	public virtual void Add(T inCallback)
	{
		this.callbackCount++;
		this.callbackList.Add(inCallback);
	}

	// Token: 0x06003364 RID: 13156 RVA: 0x000F5A44 File Offset: 0x000F3C44
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

	// Token: 0x06003365 RID: 13157 RVA: 0x000F5A94 File Offset: 0x000F3C94
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

	// Token: 0x06003366 RID: 13158 RVA: 0x000F5B18 File Offset: 0x000F3D18
	public virtual void Clear()
	{
		this.callbackList.Clear();
	}

	// Token: 0x040036C4 RID: 14020
	protected List<T> callbackList;

	// Token: 0x040036C5 RID: 14021
	protected int currentIndex;

	// Token: 0x040036C6 RID: 14022
	protected int callbackCount;
}
