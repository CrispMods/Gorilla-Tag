using System;

// Token: 0x0200080C RID: 2060
internal interface ITickSystemPost
{
	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x060032B2 RID: 12978
	// (set) Token: 0x060032B3 RID: 12979
	bool PostTickRunning { get; set; }

	// Token: 0x060032B4 RID: 12980
	void PostTick();
}
