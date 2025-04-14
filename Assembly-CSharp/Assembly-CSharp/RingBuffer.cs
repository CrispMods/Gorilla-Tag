using System;
using System.Collections.Generic;

// Token: 0x020006D1 RID: 1745
public class RingBuffer<T>
{
	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06002B2F RID: 11055 RVA: 0x000D5E26 File Offset: 0x000D4026
	public int Size
	{
		get
		{
			return this._size;
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06002B30 RID: 11056 RVA: 0x000D5E2E File Offset: 0x000D402E
	public int Capacity
	{
		get
		{
			return this._capacity;
		}
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06002B31 RID: 11057 RVA: 0x000D5E36 File Offset: 0x000D4036
	public bool IsFull
	{
		get
		{
			return this._size == this._capacity;
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06002B32 RID: 11058 RVA: 0x000D5E46 File Offset: 0x000D4046
	public bool IsEmpty
	{
		get
		{
			return this._size == 0;
		}
	}

	// Token: 0x06002B33 RID: 11059 RVA: 0x000D5E51 File Offset: 0x000D4051
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

	// Token: 0x06002B34 RID: 11060 RVA: 0x000D5E87 File Offset: 0x000D4087
	public RingBuffer(IList<T> list) : this(list.Count)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		list.CopyTo(this._items, 0);
	}

	// Token: 0x06002B35 RID: 11061 RVA: 0x000D5EB0 File Offset: 0x000D40B0
	public ref T PeekFirst()
	{
		return ref this._items[this._head];
	}

	// Token: 0x06002B36 RID: 11062 RVA: 0x000D5EC3 File Offset: 0x000D40C3
	public ref T PeekLast()
	{
		return ref this._items[this._tail];
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x000D5ED8 File Offset: 0x000D40D8
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

	// Token: 0x06002B38 RID: 11064 RVA: 0x000D5F2C File Offset: 0x000D412C
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

	// Token: 0x06002B39 RID: 11065 RVA: 0x000D5F80 File Offset: 0x000D4180
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

	// Token: 0x06002B3A RID: 11066 RVA: 0x000D5FD9 File Offset: 0x000D41D9
	public void Clear()
	{
		this._head = 0;
		this._tail = 0;
		this._size = 0;
		Array.Clear(this._items, 0, this._capacity);
	}

	// Token: 0x06002B3B RID: 11067 RVA: 0x000D6002 File Offset: 0x000D4202
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

	// Token: 0x06002B3C RID: 11068 RVA: 0x000D6036 File Offset: 0x000D4236
	public ArraySegment<T> AsSegment()
	{
		return new ArraySegment<T>(this._items);
	}

	// Token: 0x040030AC RID: 12460
	private T[] _items;

	// Token: 0x040030AD RID: 12461
	private int _head;

	// Token: 0x040030AE RID: 12462
	private int _tail;

	// Token: 0x040030AF RID: 12463
	private int _size;

	// Token: 0x040030B0 RID: 12464
	private readonly int _capacity;
}
