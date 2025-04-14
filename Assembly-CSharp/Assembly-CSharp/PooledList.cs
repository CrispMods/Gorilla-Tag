using System;
using System.Collections.Generic;
using GorillaTag;

// Token: 0x02000899 RID: 2201
public class PooledList<T> : ObjectPoolEvents
{
	// Token: 0x06003532 RID: 13618 RVA: 0x000023F4 File Offset: 0x000005F4
	void ObjectPoolEvents.OnTaken()
	{
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x000FDBA7 File Offset: 0x000FBDA7
	void ObjectPoolEvents.OnReturned()
	{
		this.List.Clear();
	}

	// Token: 0x040037BE RID: 14270
	public List<T> List = new List<T>();
}
