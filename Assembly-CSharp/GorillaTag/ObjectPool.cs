using System;
using System.Collections.Generic;

namespace GorillaTag
{
	// Token: 0x02000BB8 RID: 3000
	public class ObjectPool<T> where T : ObjectPoolEvents, new()
	{
		// Token: 0x06004BD7 RID: 19415 RVA: 0x0017115E File Offset: 0x0016F35E
		private ObjectPool()
		{
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x00171180 File Offset: 0x0016F380
		public ObjectPool(int amount)
		{
			this.maxInstances = amount;
			for (int i = 0; i < this.maxInstances; i++)
			{
				this.pool.Push(Activator.CreateInstance<T>());
			}
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x001711D4 File Offset: 0x0016F3D4
		public T Take()
		{
			T result;
			if (this.pool.Count < 1)
			{
				result = Activator.CreateInstance<T>();
			}
			else
			{
				result = this.pool.Pop();
			}
			result.OnTaken();
			return result;
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x00171211 File Offset: 0x0016F411
		public void Return(T instance)
		{
			instance.OnReturned();
			if (this.pool.Count == this.maxInstances)
			{
				return;
			}
			this.pool.Push(instance);
		}

		// Token: 0x04004D9F RID: 19871
		private Stack<T> pool = new Stack<T>(100);

		// Token: 0x04004DA0 RID: 19872
		public int maxInstances = 500;
	}
}
