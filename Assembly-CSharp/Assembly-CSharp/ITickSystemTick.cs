using System;

// Token: 0x0200080B RID: 2059
internal interface ITickSystemTick
{
	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x060032AF RID: 12975
	// (set) Token: 0x060032B0 RID: 12976
	bool TickRunning { get; set; }

	// Token: 0x060032B1 RID: 12977
	void Tick();
}
