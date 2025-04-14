using System;

// Token: 0x0200080A RID: 2058
internal interface ITickSystemPre
{
	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x060032AC RID: 12972
	// (set) Token: 0x060032AD RID: 12973
	bool PreTickRunning { get; set; }

	// Token: 0x060032AE RID: 12974
	void PreTick();
}
