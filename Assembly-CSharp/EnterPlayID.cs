using System;

// Token: 0x02000883 RID: 2179
public struct EnterPlayID
{
	// Token: 0x060034F2 RID: 13554 RVA: 0x00052EA4 File Offset: 0x000510A4
	[OnEnterPlay_Run]
	private static void NextID()
	{
		EnterPlayID.currentID++;
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x0013F8B8 File Offset: 0x0013DAB8
	public static EnterPlayID GetCurrent()
	{
		return new EnterPlayID
		{
			id = EnterPlayID.currentID
		};
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x060034F4 RID: 13556 RVA: 0x00052EB2 File Offset: 0x000510B2
	public bool IsCurrent
	{
		get
		{
			return this.id == EnterPlayID.currentID;
		}
	}

	// Token: 0x040037DA RID: 14298
	private static int currentID = 1;

	// Token: 0x040037DB RID: 14299
	private int id;
}
