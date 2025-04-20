using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020008AA RID: 2218
public class UniqueQueue<T> : IEnumerable<!0>, IEnumerable
{
	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x060035CF RID: 13775 RVA: 0x0005351B File Offset: 0x0005171B
	public int Count
	{
		get
		{
			return this.queue.Count;
		}
	}

	// Token: 0x060035D0 RID: 13776 RVA: 0x00053528 File Offset: 0x00051728
	public UniqueQueue()
	{
		this.queuedItems = new HashSet<T>();
		this.queue = new Queue<T>();
	}

	// Token: 0x060035D1 RID: 13777 RVA: 0x00053546 File Offset: 0x00051746
	public UniqueQueue(int capacity)
	{
		this.queuedItems = new HashSet<T>(capacity);
		this.queue = new Queue<T>(capacity);
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x00053566 File Offset: 0x00051766
	public void Clear()
	{
		this.queuedItems.Clear();
		this.queue.Clear();
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x0005357E File Offset: 0x0005177E
	public bool Enqueue(T item)
	{
		if (!this.queuedItems.Add(item))
		{
			return false;
		}
		this.queue.Enqueue(item);
		return true;
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x00143A48 File Offset: 0x00141C48
	public T Dequeue()
	{
		T t = this.queue.Dequeue();
		this.queuedItems.Remove(t);
		return t;
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x0005359D File Offset: 0x0005179D
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

	// Token: 0x060035D6 RID: 13782 RVA: 0x000535C3 File Offset: 0x000517C3
	public T Peek()
	{
		return this.queue.Peek();
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x000535D0 File Offset: 0x000517D0
	public bool Contains(T item)
	{
		return this.queuedItems.Contains(item);
	}

	// Token: 0x060035D8 RID: 13784 RVA: 0x000535DE File Offset: 0x000517DE
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return this.queue.GetEnumerator();
	}

	// Token: 0x060035D9 RID: 13785 RVA: 0x000535DE File Offset: 0x000517DE
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.queue.GetEnumerator();
	}

	// Token: 0x04003859 RID: 14425
	private HashSet<T> queuedItems;

	// Token: 0x0400385A RID: 14426
	private Queue<T> queue;
}
