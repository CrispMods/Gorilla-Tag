using System;

// Token: 0x0200048E RID: 1166
public struct GameBallId
{
	// Token: 0x06001C2B RID: 7211 RVA: 0x00088B3D File Offset: 0x00086D3D
	public GameBallId(int index)
	{
		this.index = index;
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x00088B46 File Offset: 0x00086D46
	public bool IsValid()
	{
		return this.index != -1;
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x00088B54 File Offset: 0x00086D54
	public static bool operator ==(GameBallId obj1, GameBallId obj2)
	{
		return obj1.index == obj2.index;
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x00088B64 File Offset: 0x00086D64
	public static bool operator !=(GameBallId obj1, GameBallId obj2)
	{
		return obj1.index != obj2.index;
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x00088B78 File Offset: 0x00086D78
	public override bool Equals(object obj)
	{
		GameBallId gameBallId = (GameBallId)obj;
		return this.index == gameBallId.index;
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x00088B9A File Offset: 0x00086D9A
	public override int GetHashCode()
	{
		return this.index.GetHashCode();
	}

	// Token: 0x04001F3B RID: 7995
	public static GameBallId Invalid = new GameBallId(-1);

	// Token: 0x04001F3C RID: 7996
	public int index;
}
