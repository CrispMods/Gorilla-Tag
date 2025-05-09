﻿using System;

// Token: 0x0200048E RID: 1166
public struct GameBallId
{
	// Token: 0x06001C2E RID: 7214 RVA: 0x00042799 File Offset: 0x00040999
	public GameBallId(int index)
	{
		this.index = index;
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x000427A2 File Offset: 0x000409A2
	public bool IsValid()
	{
		return this.index != -1;
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x000427B0 File Offset: 0x000409B0
	public static bool operator ==(GameBallId obj1, GameBallId obj2)
	{
		return obj1.index == obj2.index;
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x000427C0 File Offset: 0x000409C0
	public static bool operator !=(GameBallId obj1, GameBallId obj2)
	{
		return obj1.index != obj2.index;
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x000D9850 File Offset: 0x000D7A50
	public override bool Equals(object obj)
	{
		GameBallId gameBallId = (GameBallId)obj;
		return this.index == gameBallId.index;
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x000427D3 File Offset: 0x000409D3
	public override int GetHashCode()
	{
		return this.index.GetHashCode();
	}

	// Token: 0x04001F3C RID: 7996
	public static GameBallId Invalid = new GameBallId(-1);

	// Token: 0x04001F3D RID: 7997
	public int index;
}
