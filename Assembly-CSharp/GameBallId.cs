using System;

// Token: 0x0200049A RID: 1178
public struct GameBallId
{
	// Token: 0x06001C7F RID: 7295 RVA: 0x00043AD2 File Offset: 0x00041CD2
	public GameBallId(int index)
	{
		this.index = index;
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x00043ADB File Offset: 0x00041CDB
	public bool IsValid()
	{
		return this.index != -1;
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x00043AE9 File Offset: 0x00041CE9
	public static bool operator ==(GameBallId obj1, GameBallId obj2)
	{
		return obj1.index == obj2.index;
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x00043AF9 File Offset: 0x00041CF9
	public static bool operator !=(GameBallId obj1, GameBallId obj2)
	{
		return obj1.index != obj2.index;
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x000DC500 File Offset: 0x000DA700
	public override bool Equals(object obj)
	{
		GameBallId gameBallId = (GameBallId)obj;
		return this.index == gameBallId.index;
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x00043B0C File Offset: 0x00041D0C
	public override int GetHashCode()
	{
		return this.index.GetHashCode();
	}

	// Token: 0x04001F8A RID: 8074
	public static GameBallId Invalid = new GameBallId(-1);

	// Token: 0x04001F8B RID: 8075
	public int index;
}
