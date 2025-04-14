using System;

// Token: 0x02000039 RID: 57
public class CrittersAttachPoint : CrittersActor
{
	// Token: 0x06000121 RID: 289 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void ProcessRemote()
	{
	}

	// Token: 0x04000158 RID: 344
	public bool fixedOrientation = true;

	// Token: 0x04000159 RID: 345
	public CrittersAttachPoint.AnchoredLocationTypes anchorLocation;

	// Token: 0x0400015A RID: 346
	public bool isLeft;

	// Token: 0x0200003A RID: 58
	public enum AnchoredLocationTypes
	{
		// Token: 0x0400015C RID: 348
		Arm,
		// Token: 0x0400015D RID: 349
		Chest,
		// Token: 0x0400015E RID: 350
		Back
	}
}
