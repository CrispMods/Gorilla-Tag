using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GorillaTag
{
	// Token: 0x02000BE6 RID: 3046
	public class ObjectPool<T> where T : ObjectPoolEvents, new()
	{
		// Token: 0x06004D20 RID: 19744 RVA: 0x00062A24 File Offset: 0x00060C24
		protected ObjectPool()
		{
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x00062A37 File Offset: 0x00060C37
		public ObjectPool(int amount) : this(amount, amount)
		{
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x00062A41 File Offset: 0x00060C41
		public ObjectPool(int initialAmount, int maxAmount)
		{
			this.InitializePool(initialAmount, maxAmount);
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x001A9E1C File Offset: 0x001A801C
		protected void InitializePool(int initialAmount, int maxAmount)
		{
			this.maxInstances = maxAmount;
			this.pool = new Stack<T>(initialAmount);
			for (int i = 0; i < initialAmount; i++)
			{
				this.pool.Push(this.CreateInstance());
			}
		}

		// Token: 0x06004D24 RID: 19748 RVA: 0x001A9E5C File Offset: 0x001A805C
		public T Take()
		{
			T result;
			if (this.pool.Count < 1)
			{
				result = this.CreateInstance();
			}
			else
			{
				result = this.pool.Pop();
			}
			result.OnTaken();
			return result;
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x00062A5C File Offset: 0x00060C5C
		public void Return(T instance)
		{
			instance.OnReturned();
			if (this.pool.Count == this.maxInstances)
			{
				return;
			}
			this.pool.Push(instance);
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x00062A8B File Offset: 0x00060C8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual T CreateInstance()
		{
			return Activator.CreateInstance<T>();
		}

		// Token: 0x04004E95 RID: 20117
		private Stack<T> pool;

		// Token: 0x04004E96 RID: 20118
		public int maxInstances = 500;
	}
}
