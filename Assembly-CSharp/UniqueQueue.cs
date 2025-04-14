using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200088E RID: 2190
public class UniqueQueue<T> : IEnumerable<!0>, IEnumerable
{
	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06003503 RID: 13571 RVA: 0x000FD117 File Offset: 0x000FB317
	public int Count
	{
		get
		{
			return this.queue.Count;
		}
	}

	// Token: 0x06003504 RID: 13572 RVA: 0x000FD124 File Offset: 0x000FB324
	public UniqueQueue()
	{
		this.queuedItems = new HashSet<T>();
		this.queue = new Queue<T>();
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x000FD142 File Offset: 0x000FB342
	public UniqueQueue(int capacity)
	{
		this.queuedItems = new HashSet<T>(capacity);
		this.queue = new Queue<T>(capacity);
	}

	// Token: 0x06003506 RID: 13574 RVA: 0x000FD162 File Offset: 0x000FB362
	public void Clear()
	{
		this.queuedItems.Clear();
		this.queue.Clear();
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x000FD17A File Offset: 0x000FB37A
	public bool Enqueue(T item)
	{
		if (!this.queuedItems.Add(item))
		{
			return false;
		}
		this.queue.Enqueue(item);
		return true;
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x000FD19C File Offset: 0x000FB39C
	public T Dequeue()
	{
		T t = this.queue.Dequeue();
		this.queuedItems.Remove(t);
		return t;
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x000FD1C3 File Offset: 0x000FB3C3
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

	// Token: 0x0600350A RID: 13578 RVA: 0x000FD1E9 File Offset: 0x000FB3E9
	public T Peek()
	{
		return this.queue.Peek();
	}

	// Token: 0x0600350B RID: 13579 RVA: 0x000FD1F6 File Offset: 0x000FB3F6
	public bool Contains(T item)
	{
		return this.queuedItems.Contains(item);
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x000FD204 File Offset: 0x000FB404
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return this.queue.GetEnumerator();
	}

	// Token: 0x0600350D RID: 13581 RVA: 0x000FD204 File Offset: 0x000FB404
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.queue.GetEnumerator();
	}

	// Token: 0x04003799 RID: 14233
	private HashSet<T> queuedItems;

	// Token: 0x0400379A RID: 14234
	private Queue<T> queue;
}
