using System;
using System.Collections.Generic;

// Token: 0x020006E5 RID: 1765
public class RingBuffer<T>
{
	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06002BBD RID: 11197 RVA: 0x0004D97A File Offset: 0x0004BB7A
	public int Size
	{
		get
		{
			return this._size;
		}
	}

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06002BBE RID: 11198 RVA: 0x0004D982 File Offset: 0x0004BB82
	public int Capacity
	{
		get
		{
			return this._capacity;
		}
	}

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06002BBF RID: 11199 RVA: 0x0004D98A File Offset: 0x0004BB8A
	public bool IsFull
	{
		get
		{
			return this._size == this._capacity;
		}
	}

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06002BC0 RID: 11200 RVA: 0x0004D99A File Offset: 0x0004BB9A
	public bool IsEmpty
	{
		get
		{
			return this._size == 0;
		}
	}

	// Token: 0x06002BC1 RID: 11201 RVA: 0x0004D9A5 File Offset: 0x0004BBA5
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

	// Token: 0x06002BC2 RID: 11202 RVA: 0x0004D9DB File Offset: 0x0004BBDB
	public RingBuffer(IList<T> list) : this(list.Count)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		list.CopyTo(this._items, 0);
	}

	// Token: 0x06002BC3 RID: 11203 RVA: 0x0004DA04 File Offset: 0x0004BC04
	public ref T PeekFirst()
	{
		return ref this._items[this._head];
	}

	// Token: 0x06002BC4 RID: 11204 RVA: 0x0004DA17 File Offset: 0x0004BC17
	public ref T PeekLast()
	{
		return ref this._items[this._tail];
	}

	// Token: 0x06002BC5 RID: 11205 RVA: 0x00121458 File Offset: 0x0011F658
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

	// Token: 0x06002BC6 RID: 11206 RVA: 0x001214AC File Offset: 0x0011F6AC
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

	// Token: 0x06002BC7 RID: 11207 RVA: 0x00121500 File Offset: 0x0011F700
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

	// Token: 0x06002BC8 RID: 11208 RVA: 0x0004DA2A File Offset: 0x0004BC2A
	public void Clear()
	{
		this._head = 0;
		this._tail = 0;
		this._size = 0;
		Array.Clear(this._items, 0, this._capacity);
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x0004DA53 File Offset: 0x0004BC53
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

	// Token: 0x06002BCA RID: 11210 RVA: 0x0004DA87 File Offset: 0x0004BC87
	public ArraySegment<T> AsSegment()
	{
		return new ArraySegment<T>(this._items);
	}

	// Token: 0x04003143 RID: 12611
	private T[] _items;

	// Token: 0x04003144 RID: 12612
	private int _head;

	// Token: 0x04003145 RID: 12613
	private int _tail;

	// Token: 0x04003146 RID: 12614
	private int _size;

	// Token: 0x04003147 RID: 12615
	private readonly int _capacity;
}
