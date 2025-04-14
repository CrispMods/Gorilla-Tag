using System;

// Token: 0x02000807 RID: 2055
internal interface ITickSystemPre
{
	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x060032A0 RID: 12960
	// (set) Token: 0x060032A1 RID: 12961
	bool PreTickRunning { get; set; }

	// Token: 0x060032A2 RID: 12962
	void PreTick();
}
