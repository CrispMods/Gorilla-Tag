using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000891 RID: 2193
public class UniqueQueue<T> : IEnumerable<!0>, IEnumerable
{
	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x0600350F RID: 13583 RVA: 0x0005200E File Offset: 0x0005020E
	public int Count
	{
		get
		{
			return this.queue.Count;
		}
	}

	// Token: 0x06003510 RID: 13584 RVA: 0x0005201B File Offset: 0x0005021B
	public UniqueQueue()
	{
		this.queuedItems = new HashSet<T>();
		this.queue = new Queue<T>();
	}

	// Token: 0x06003511 RID: 13585 RVA: 0x00052039 File Offset: 0x00050239
	public UniqueQueue(int capacity)
	{
		this.queuedItems = new HashSet<T>(capacity);
		this.queue = new Queue<T>(capacity);
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x00052059 File Offset: 0x00050259
	public void Clear()
	{
		this.queuedItems.Clear();
		this.queue.Clear();
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x00052071 File Offset: 0x00050271
	public bool Enqueue(T item)
	{
		if (!this.queuedItems.Add(item))
		{
			return false;
		}
		this.queue.Enqueue(item);
		return true;
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x0013E460 File Offset: 0x0013C660
	public T Dequeue()
	{
		T t = this.queue.Dequeue();
		this.queuedItems.Remove(t);
		return t;
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x00052090 File Offset: 0x00050290
	public bool TryDequeue(out T item)
	{
		if (this.queue.Count < 1)
		{
			item = default(T);
			return false;
		}
		item = this.Dequeue();
		return true;
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x000520B6 File Offset: 0x000502B6
	public T Peek()
	{
		return this.queue.Peek();
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x000520C3 File Offset: 0x000502C3
	public bool Contains(T item)
	{
		return this.queuedItems.Contains(item);
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x000520D1 File Offset: 0x000502D1
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return this.queue.GetEnumerator();
	}

	// Token: 0x06003519 RID: 13593 RVA: 0x000520D1 File Offset: 0x000502D1
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.queue.GetEnumerator();
	}

	// Token: 0x040037AB RID: 14251
	private HashSet<T> queuedItems;

	// Token: 0x040037AC RID: 14252
	private Queue<T> queue;
}
