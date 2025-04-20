using System;

// Token: 0x02000857 RID: 2135
internal class CircularBuffer<T>
{
	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06003416 RID: 13334 RVA: 0x000523D5 File Offset: 0x000505D5
	// (set) Token: 0x06003417 RID: 13335 RVA: 0x000523DD File Offset: 0x000505DD
	public int Count { get; private set; }

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x06003418 RID: 13336 RVA: 0x000523E6 File Offset: 0x000505E6
	// (set) Token: 0x06003419 RID: 13337 RVA: 0x000523EE File Offset: 0x000505EE
	public int Capacity { get; private set; }

	// Token: 0x0600341A RID: 13338 RVA: 0x000523F7 File Offset: 0x000505F7
	public CircularBuffer(int capacity)
	{
		this.backingArray = new T[capacity];
		this.Capacity = capacity;
		this.Count = 0;
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x0013D0D0 File Offset: 0x0013B2D0
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

	// Token: 0x0600341C RID: 13340 RVA: 0x00052419 File Offset: 0x00050619
	public void Clear()
	{
		this.Count = 0;
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x00052422 File Offset: 0x00050622
	public T Last()
	{
		return this.backingArray[this.lastWriteIdx];
	}

	// Token: 0x17000563 RID: 1379
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

	// Token: 0x04003771 RID: 14193
	private T[] backingArray;

	// Token: 0x04003774 RID: 14196
	private int nextWriteIdx;

	// Token: 0x04003775 RID: 14197
	private int lastWriteIdx;
}
