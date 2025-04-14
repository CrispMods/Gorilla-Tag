using System;

// Token: 0x02000809 RID: 2057
internal interface ITickSystemPost
{
	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x060032A6 RID: 12966
	// (set) Token: 0x060032A7 RID: 12967
	bool PostTickRunning { get; set; }

	// Token: 0x060032A8 RID: 12968
	void PostTick();
}
