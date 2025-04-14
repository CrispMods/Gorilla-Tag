using System;

// Token: 0x02000657 RID: 1623
public interface ITimeOfDaySystem
{
	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x0600284A RID: 10314
	double currentTimeInSeconds { get; }

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x0600284B RID: 10315
	double totalTimeInSeconds { get; }
}
