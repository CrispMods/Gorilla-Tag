using System;

// Token: 0x02000821 RID: 2081
internal interface ITickSystemPre
{
	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x0600335B RID: 13147
	// (set) Token: 0x0600335C RID: 13148
	bool PreTickRunning { get; set; }

	// Token: 0x0600335D RID: 13149
	void PreTick();
}
