using System;

// Token: 0x0200003D RID: 61
public class CrittersAttachPoint : CrittersActor
{
	// Token: 0x06000135 RID: 309 RVA: 0x00030607 File Offset: 0x0002E807
	public override void ProcessRemote()
	{
	}

	// Token: 0x04000169 RID: 361
	public bool fixedOrientation = true;

	// Token: 0x0400016A RID: 362
	public CrittersAttachPoint.AnchoredLocationTypes anchorLocation;

	// Token: 0x0400016B RID: 363
	public bool isLeft;

	// Token: 0x0200003E RID: 62
	public enum AnchoredLocationTypes
	{
		// Token: 0x0400016D RID: 365
		Arm,
		// Token: 0x0400016E RID: 366
		Chest,
		// Token: 0x0400016F RID: 367
		Back
	}
}
