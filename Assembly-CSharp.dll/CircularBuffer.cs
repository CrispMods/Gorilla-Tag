using System;

// Token: 0x02000840 RID: 2112
internal class CircularBuffer<T>
{
	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x06003367 RID: 13159 RVA: 0x00050FC7 File Offset: 0x0004F1C7
	// (set) Token: 0x06003368 RID: 13160 RVA: 0x00050FCF File Offset: 0x0004F1CF
	public int Count { get; private set; }

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x06003369 RID: 13161 RVA: 0x00050FD8 File Offset: 0x0004F1D8
	// (set) Token: 0x0600336A RID: 13162 RVA: 0x00050FE0 File Offset: 0x0004F1E0
	public int Capacity { get; private set; }

	// Token: 0x0600336B RID: 13163 RVA: 0x00050FE9 File Offset: 0x0004F1E9
	public CircularBuffer(int capacity)
	{
		this.backingArray = new T[capacity];
		this.Capacity = capacity;
		this.Count = 0;
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x00137B78 File Offset: 0x00135D78
	public void Add(T value)
	{
		this.backingArray[this.nextWriteIdx] = value;
		this.lastWriteIdx = this.nextWriteIdx;
		this.nextWriteIdx = (this.nextWriteIdx + 1) % this.Capacity;
		if (this.Count < this.Capacity)
		{
			int count = this.Count;
			this.Count = count + 1;
		}
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x0005100B File Offset: 0x0004F20B
	public void Clear()
	{
		this.Count = 0;
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x00051014 File Offset: 0x0004F214
	public T Last()
	{
		return this.backingArray[this.lastWriteIdx];
	}

	// Token: 0x17000556 RID: 1366
	public T this[int logicalIdx]
	{
		get
		{
			if (logicalIdx < 0 || logicalIdx >= this.Count)
			{
				throw new ArgumentOutOfRangeException("logicalIdx", logicalIdx, string.Format("Out of bounds index into Circular Buffer with length {0}", this.Count));
			}
			int num = (this.lastWriteIdx + this.Capacity - logicalIdx) % this.Capacity;
			return this.backingArray[num];
		}
	}

	// Token: 0x040036C7 RID: 14023
	private T[] backingArray;

	// Token: 0x040036CA RID: 14026
	private int nextWriteIdx;

	// Token: 0x040036CB RID: 14027
	private int lastWriteIdx;
}
