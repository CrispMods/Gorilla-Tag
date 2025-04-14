using System;

// Token: 0x0200086A RID: 2154
public struct EnterPlayID
{
	// Token: 0x06003432 RID: 13362 RVA: 0x000F8D2C File Offset: 0x000F6F2C
	[OnEnterPlay_Run]
	private static void NextID()
	{
		EnterPlayID.currentID++;
	}

	// Token: 0x06003433 RID: 13363 RVA: 0x000F8D3C File Offset: 0x000F6F3C
	public static EnterPlayID GetCurrent()
	{
		return new EnterPlayID
		{
			id = EnterPlayID.currentID
		};
	}

	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06003434 RID: 13364 RVA: 0x000F8D5E File Offset: 0x000F6F5E
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
