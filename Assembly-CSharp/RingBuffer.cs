using System;
using System.Collections.Generic;

// Token: 0x020006D0 RID: 1744
public class RingBuffer<T>
{
	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06002B27 RID: 11047 RVA: 0x000D59A6 File Offset: 0x000D3BA6
	public int Size
	{
		get
		{
			return this._size;
		}
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06002B28 RID: 11048 RVA: 0x000D59AE File Offset: 0x000D3BAE
	public int Capacity
	{
		get
		{
			return this._capacity;
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06002B29 RID: 11049 RVA: 0x000D59B6 File Offset: 0x000D3BB6
	public bool IsFull
	{
		get
		{
			return this._size == this._capacity;
		}
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06002B2A RID: 11050 RVA: 0x000D59C6 File Offset: 0x000D3BC6
	public bool IsEmpty
	{
		get
		{
			return this._size == 0;
		}
	}

	// Token: 0x06002B2B RID: 11051 RVA: 0x000D59D1 File Offset: 0x000D3BD1
	public RingBuffer(int capacity)
	{
		if (capacity < 1)
		{
			throw new ArgumentException("Can't be zero or negative", "capacity");
		}
		this._size = 0;
		this._capacity = capacity;
		this._items = new T[capacity];
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x000D5A07 File Offset: 0x000D3C07
	public RingBuffer(IList<T> list) : this(list.Count)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		list.CopyTo(this._items, 0);
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x000D5A30 File Offset: 0x000D3C30
	public ref T PeekFirst()
	{
		return ref this._items[this._head];
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x000D5A43 File Offset: 0x000D3C43
	public ref T PeekLast()
	{
		return ref this._items[this._tail];
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x000D5A58 File Offset: 0x000D3C58
	public bool Push(T item)
	{
		if (this._size == this._capacity)
		{
			return false;
		}
		this._items[this._tail] = item;
		this._tail = (this._tail + 1) % this._capacity;
		this._size++;
		return true;
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x000D5AAC File Offset: 0x000D3CAC
	public T Pop()
	{
		if (this._size == 0)
		{
			return default(T);
		}
		T result = this._items[this._head];
		this._head = (this._head + 1) % this._capacity;
		this._size--;
		return result;
	}

	// Token: 0x06002B31 RID: 11057 RVA: 0x000D5B00 File Offset: 0x000D3D00
	public bool TryPop(out T item)
	{
		if (this._size == 0)
		{
			item = default(T);
			return false;
		}
		item = this._items[this._head];
		this._head = (this._head + 1) % this._capacity;
		this._size--;
		return true;
	}

	// Token: 0x06002B32 RID: 11058 RVA: 0x000D5B59 File Offset: 0x000D3D59
	public void Clear()
	{
		this._head = 0;
		this._tail = 0;
		this._size = 0;
		Array.Clear(this._items, 0, this._capacity);
	}

	// Token: 0x06002B33 RID: 11059 RVA: 0x000D5B82 File Offset: 0x000D3D82
	public bool TryGet(int i, out T item)
	{
		if (this._size == 0)
		{
			item = default(T);
			return false;
		}
		item = this._items[this._head + i % this._size];
		return true;
	}

	// Token: 0x06002B34 RID: 11060 RVA: 0x000D5BB6 File Offset: 0x000D3DB6
	public ArraySegment<T> AsSegment()
	{
		return new ArraySegment<T>(this._items);
	}

	// Token: 0x040030A6 RID: 12454
	private T[] _items;

	// Token: 0x040030A7 RID: 12455
	private int _head;

	// Token: 0x040030A8 RID: 12456
	private int _tail;

	// Token: 0x040030A9 RID: 12457
	private int _size;

	// Token: 0x040030AA RID: 12458
	private readonly int _capacity;
}
