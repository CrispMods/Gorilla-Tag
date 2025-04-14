using System;

// Token: 0x02000658 RID: 1624
public interface ITimeOfDaySystem
{
	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06002852 RID: 10322
	double currentTimeInSeconds { get; }

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06002853 RID: 10323
	double totalTimeInSeconds { get; }
}
