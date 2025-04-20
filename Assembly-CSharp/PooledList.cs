using System;
using System.Collections.Generic;
using GorillaTag;

// Token: 0x020008B2 RID: 2226
public class PooledList<T> : ObjectPoolEvents
{
	// Token: 0x060035F2 RID: 13810 RVA: 0x00030607 File Offset: 0x0002E807
	void ObjectPoolEvents.OnTaken()
	{
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x000536E1 File Offset: 0x000518E1
	void ObjectPoolEvents.OnReturned()
	{
		this.List.Clear();
	}

	// Token: 0x0400386C RID: 14444
	public List<T> List = new List<T>();
}
