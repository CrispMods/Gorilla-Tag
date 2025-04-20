using System;
using GorillaTag;

// Token: 0x02000872 RID: 2162
public class LoopingArray<T> : ObjectPoolEvents
{
	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x0600348E RID: 13454 RVA: 0x000529C5 File Offset: 0x00050BC5
	public int Length
	{
		get
		{
			return this.m_length;
		}
	}

	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x0600348F RID: 13455 RVA: 0x000529CD File Offset: 0x00050BCD
	public int CurrentIndex
	{
		get
		{
			return this.m_currentIndex;
		}
	}

	// Token: 0x1700056E RID: 1390
	public T this[int index]
	{
		get
		{
			return this.m_array[index];
		}
		set
		{
			this.m_array[index] = value;
		}
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x000529F2 File Offset: 0x00050BF2
	public LoopingArray() : this(0)
	{
	}

	// Token: 0x06003493 RID: 13459 RVA: 0x000529FB File Offset: 0x00050BFB
	public LoopingArray(int capicity)
	{
		this.m_length = capicity;
		this.m_array = new T[capicity];
		this.Clear();
	}

	// Token: 0x06003494 RID: 13460 RVA: 0x00052A1C File Offset: 0x00050C1C
	public int AddAndIncrement(in T value)
	{
		int currentIndex = this.m_currentIndex;
		this.m_array[this.m_currentIndex] = value;
		this.m_currentIndex = (this.m_currentIndex + 1) % this.m_length;
		return currentIndex;
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x00052A50 File Offset: 0x00050C50
	public int IncrementAndAdd(in T value)
	{
		this.m_currentIndex = (this.m_currentIndex + 1) % this.m_length;
		this.m_array[this.m_currentIndex] = value;
		return this.m_currentIndex;
	}

	// Token: 0x06003496 RID: 13462 RVA: 0x0013E420 File Offset: 0x0013C620
	public void Clear()
	{
		this.m_currentIndex = 0;
		for (int i = 0; i < this.m_array.Length; i++)
		{
			this.m_array[i] = default(T);
		}
	}

	// Token: 0x06003497 RID: 13463 RVA: 0x00052A84 File Offset: 0x00050C84
	void ObjectPoolEvents.OnTaken()
	{
		this.Clear();
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x00030607 File Offset: 0x0002E807
	void ObjectPoolEvents.OnReturned()
	{
	}

	// Token: 0x040037C2 RID: 14274
	private int m_length;

	// Token: 0x040037C3 RID: 14275
	private int m_currentIndex;

	// Token: 0x040037C4 RID: 14276
	private T[] m_array;

	// Token: 0x02000873 RID: 2163
	public class Pool : ObjectPool<LoopingArray<T>>
	{
		// Token: 0x06003499 RID: 13465 RVA: 0x00052A8C File Offset: 0x00050C8C
		private Pool(int amount) : base(amount)
		{
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x00052A95 File Offset: 0x00050C95
		public Pool(int size, int amount) : this(size, amount, amount)
		{
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x00052AA0 File Offset: 0x00050CA0
		public Pool(int size, int initialAmount, int maxAmount)
		{
			this.m_size = size;
			base.InitializePool(initialAmount, maxAmount);
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x00052AB7 File Offset: 0x00050CB7
		public override LoopingArray<T> CreateInstance()
		{
			return new LoopingArray<T>(this.m_size);
		}

		// Token: 0x040037C5 RID: 14277
		private readonly int m_size;
	}
}
