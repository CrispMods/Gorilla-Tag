using System;

// Token: 0x02000808 RID: 2056
internal interface ITickSystemTick
{
	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x060032A3 RID: 12963
	// (set) Token: 0x060032A4 RID: 12964
	bool TickRunning { get; set; }

	// Token: 0x060032A5 RID: 12965
	void Tick();
}
