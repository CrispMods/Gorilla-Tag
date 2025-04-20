using System;

// Token: 0x02000822 RID: 2082
internal interface ITickSystemTick
{
	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x0600335E RID: 13150
	// (set) Token: 0x0600335F RID: 13151
	bool TickRunning { get; set; }

	// Token: 0x06003360 RID: 13152
	void Tick();
}
