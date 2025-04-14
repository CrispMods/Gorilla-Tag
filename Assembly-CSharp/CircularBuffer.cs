using System;

// Token: 0x0200083D RID: 2109
internal class CircularBuffer<T>
{
	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x0600335B RID: 13147 RVA: 0x000F555D File Offset: 0x000F375D
	// (set) Token: 0x0600335C RID: 13148 RVA: 0x000F5565 File Offset: 0x000F3765
	public int Count { get; private set; }

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x0600335D RID: 13149 RVA: 0x000F556E File Offset: 0x000F376E
	// (set) Token: 0x0600335E RID: 13150 RVA: 0x000F5576 File Offset: 0x000F3776
	public int Capacity { get; private set; }

	// Token: 0x0600335F RID: 13151 RVA: 0x000F557F File Offset: 0x000F377F
	public CircularBuffer(int capacity)
	{
		this.backingArray = new T[capacity];
		this.Capacity = capacity;
		this.Count = 0;
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x000F55A4 File Offset: 0x000F37A4
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

	// Token: 0x06003361 RID: 13153 RVA: 0x000F5602 File Offset: 0x000F3802
	public void Clear()
	{
		this.Count = 0;
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x000F560B File Offset: 0x000F380B
	public T Last()
	{
		return this.backingArray[this.lastWriteIdx];
	}

	// Token: 0x17000555 RID: 1365
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

	// Token: 0x040036B5 RID: 14005
	private T[] backingArray;

	// Token: 0x040036B8 RID: 14008
	private int nextWriteIdx;

	// Token: 0x040036B9 RID: 14009
	private int lastWriteIdx;
}
