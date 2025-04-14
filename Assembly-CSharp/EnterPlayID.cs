using System;

// Token: 0x02000867 RID: 2151
public struct EnterPlayID
{
	// Token: 0x06003426 RID: 13350 RVA: 0x000F8764 File Offset: 0x000F6964
	[OnEnterPlay_Run]
	private static void NextID()
	{
		EnterPlayID.currentID++;
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x000F8774 File Offset: 0x000F6974
	public static EnterPlayID GetCurrent()
	{
		return new EnterPlayID
		{
			id = EnterPlayID.currentID
		};
	}

	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x06003428 RID: 13352 RVA: 0x000F8796 File Offset: 0x000F6996
	public bool IsCurrent
	{
		get
		{
			return this.id == EnterPlayID.currentID;
		}
	}

	// Token: 0x0400371A RID: 14106
	private static int currentID = 1;

	// Token: 0x0400371B RID: 14107
	private int id;
}
