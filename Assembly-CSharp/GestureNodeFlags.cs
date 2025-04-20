using System;

// Token: 0x02000165 RID: 357
[Flags]
public enum GestureNodeFlags : uint
{
	// Token: 0x04000AC2 RID: 2754
	None = 0U,
	// Token: 0x04000AC3 RID: 2755
	HandLeft = 1U,
	// Token: 0x04000AC4 RID: 2756
	HandRight = 2U,
	// Token: 0x04000AC5 RID: 2757
	HandOpen = 4U,
	// Token: 0x04000AC6 RID: 2758
	HandClosed = 8U,
	// Token: 0x04000AC7 RID: 2759
	DigitOpen = 16U,
	// Token: 0x04000AC8 RID: 2760
	DigitClosed = 32U,
	// Token: 0x04000AC9 RID: 2761
	DigitBent = 64U,
	// Token: 0x04000ACA RID: 2762
	TowardFace = 128U,
	// Token: 0x04000ACB RID: 2763
	AwayFromFace = 256U,
	// Token: 0x04000ACC RID: 2764
	AxisWorldUp = 512U,
	// Token: 0x04000ACD RID: 2765
	AxisWorldDown = 1024U
}
