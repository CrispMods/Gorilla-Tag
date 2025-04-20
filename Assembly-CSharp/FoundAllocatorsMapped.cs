using System;
using System.Collections.Generic;

// Token: 0x0200013A RID: 314
[Serializable]
public class FoundAllocatorsMapped
{
	// Token: 0x040009B6 RID: 2486
	public string path;

	// Token: 0x040009B7 RID: 2487
	public List<ViewsAndAllocator> allocators = new List<ViewsAndAllocator>();

	// Token: 0x040009B8 RID: 2488
	public List<FoundAllocatorsMapped> subGroups = new List<FoundAllocatorsMapped>();
}
