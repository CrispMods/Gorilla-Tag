using System;

// Token: 0x0200086A RID: 2154
public struct EnterPlayID
{
	// Token: 0x06003432 RID: 13362 RVA: 0x00051997 File Offset: 0x0004FB97
	[OnEnterPlay_Run]
	private static void NextID()
	{
		EnterPlayID.currentID++;
	}

	// Token: 0x06003433 RID: 13363 RVA: 0x0013A2D0 File Offset: 0x001384D0
	public static EnterPlayID GetCurrent()
	{
		return new EnterPlayID
		{
			id = EnterPlayID.currentID
		};
	}

	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06003434 RID: 13364 RVA: 0x000519A5 File Offset: 0x0004FBA5
	public bool IsCurrent
	{
		get
		{
			return this.id == EnterPlayID.currentID;
		}
	}

	// Token: 0x0400372C RID: 14124
	private static int currentID = 1;

	// Token: 0x0400372D RID: 14125
	private int id;
}
