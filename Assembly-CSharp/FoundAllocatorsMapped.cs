using System;
using System.Collections.Generic;

// Token: 0x02000130 RID: 304
[Serializable]
public class FoundAllocatorsMapped
{
	// Token: 0x04000973 RID: 2419
	public string path;

	// Token: 0x04000974 RID: 2420
	public List<ViewsAndAllocator> allocators = new List<ViewsAndAllocator>();

	// Token: 0x04000975 RID: 2421
	public List<FoundAllocatorsMapped> subGroups = new List<FoundAllocatorsMapped>();
}
