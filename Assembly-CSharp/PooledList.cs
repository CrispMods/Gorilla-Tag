using System;
using System.Collections.Generic;
using GorillaTag;

// Token: 0x02000896 RID: 2198
public class PooledList<T> : ObjectPoolEvents
{
	// Token: 0x06003526 RID: 13606 RVA: 0x000023F4 File Offset: 0x000005F4
	void ObjectPoolEvents.OnTaken()
	{
	}

	// Token: 0x06003527 RID: 13607 RVA: 0x000FD5DF File Offset: 0x000FB7DF
	void ObjectPoolEvents.OnReturned()
	{
		this.List.Clear();
	}

	// Token: 0x040037AC RID: 14252
	public List<T> List = new List<T>();
}
