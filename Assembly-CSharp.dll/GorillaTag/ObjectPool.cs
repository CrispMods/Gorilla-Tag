using System;
using System.Collections.Generic;

namespace GorillaTag
{
	// Token: 0x02000BBB RID: 3003
	public class ObjectPool<T> where T : ObjectPoolEvents, new()
	{
		// Token: 0x06004BE3 RID: 19427 RVA: 0x00061082 File Offset: 0x0005F282
		private ObjectPool()
		{
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x001A2E3C File Offset: 0x001A103C
		public ObjectPool(int amount)
		{
			this.maxInstances = amount;
			for (int i = 0; i < this.maxInstances; i++)
			{
				this.pool.Push(Activator.CreateInstance<T>());
			}
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x001A2E90 File Offset: 0x001A1090
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

		// Token: 0x06004BE6 RID: 19430 RVA: 0x000610A2 File Offset: 0x0005F2A2
		public void Return(T instance)
		{
			instance.OnReturned();
			if (this.pool.Count == this.maxInstances)
			{
				return;
			}
			this.pool.Push(instance);
		}

		// Token: 0x04004DB1 RID: 19889
		private Stack<T> pool = new Stack<T>(100);

		// Token: 0x04004DB2 RID: 19890
		public int maxInstances = 500;
	}
}
