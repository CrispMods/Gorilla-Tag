using System;

// Token: 0x0200015A RID: 346
[Flags]
public enum GestureNodeFlags : uint
{
	// Token: 0x04000A7C RID: 2684
	None = 0U,
	// Token: 0x04000A7D RID: 2685
	HandLeft = 1U,
	// Token: 0x04000A7E RID: 2686
	HandRight = 2U,
	// Token: 0x04000A7F RID: 2687
	HandOpen = 4U,
	// Token: 0x04000A80 RID: 2688
	HandClosed = 8U,
	// Token: 0x04000A81 RID: 2689
	DigitOpen = 16U,
	// Token: 0x04000A82 RID: 2690
	DigitClosed = 32U,
	// Token: 0x04000A83 RID: 2691
	DigitBent = 64U,
	// Token: 0x04000A84 RID: 2692
	TowardFace = 128U,
	// Token: 0x04000A85 RID: 2693
	AwayFromFace = 256U,
	// Token: 0x04000A86 RID: 2694
	AxisWorldUp = 512U,
	// Token: 0x04000A87 RID: 2695
	AxisWorldDown = 1024U
}
