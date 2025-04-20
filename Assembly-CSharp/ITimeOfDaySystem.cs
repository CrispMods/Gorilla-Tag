using System;

// Token: 0x02000636 RID: 1590
public interface ITimeOfDaySystem
{
	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06002775 RID: 10101
	double currentTimeInSeconds { get; }

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06002776 RID: 10102
	double totalTimeInSeconds { get; }
}
