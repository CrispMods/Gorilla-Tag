using System;

// Token: 0x02000823 RID: 2083
internal interface ITickSystemPost
{
	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x06003361 RID: 13153
	// (set) Token: 0x06003362 RID: 13154
	bool PostTickRunning { get; set; }

	// Token: 0x06003363 RID: 13155
	void PostTick();
}
