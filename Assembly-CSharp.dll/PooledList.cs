using System;
using System.Collections.Generic;
using GorillaTag;

// Token: 0x02000899 RID: 2201
public class PooledList<T> : ObjectPoolEvents
{
	// Token: 0x06003532 RID: 13618 RVA: 0x0002F75F File Offset: 0x0002D95F
	void ObjectPoolEvents.OnTaken()
	{
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x000521D4 File Offset: 0x000503D4
	void ObjectPoolEvents.OnReturned()
	{
		this.List.Clear();
	}

	// Token: 0x040037BE RID: 14270
	public List<T> List = new List<T>();
}
