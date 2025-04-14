using System;

// Token: 0x0200048E RID: 1166
public struct GameBallId
{
	// Token: 0x06001C2E RID: 7214 RVA: 0x00088EC1 File Offset: 0x000870C1
	public GameBallId(int index)
	{
		this.index = index;
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x00088ECA File Offset: 0x000870CA
	public bool IsValid()
	{
		return this.index != -1;
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x00088ED8 File Offset: 0x000870D8
	public static bool operator ==(GameBallId obj1, GameBallId obj2)
	{
		return obj1.index == obj2.index;
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x00088EE8 File Offset: 0x000870E8
	public static bool operator !=(GameBallId obj1, GameBallId obj2)
	{
		return obj1.index != obj2.index;
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x00088EFC File Offset: 0x000870FC
	public override bool Equals(object obj)
	{
		GameBallId gameBallId = (GameBallId)obj;
		return this.index == gameBallId.index;
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x00088F1E File Offset: 0x0008711E
	public override int GetHashCode()
	{
		return this.index.GetHashCode();
	}

	// Token: 0x04001F3C RID: 7996
	public static GameBallId Invalid = new GameBallId(-1);

	// Token: 0x04001F3D RID: 7997
	public int index;
}
