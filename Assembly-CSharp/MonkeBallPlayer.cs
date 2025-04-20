using System;
using UnityEngine;

// Token: 0x020004AE RID: 1198
public class MonkeBallPlayer : MonoBehaviour
{
	// Token: 0x06001D2B RID: 7467 RVA: 0x00043F24 File Offset: 0x00042124
	private void Awake()
	{
		if (this.gamePlayer == null)
		{
			this.gamePlayer = base.GetComponent<GamePlayer>();
		}
	}

	// Token: 0x0400200F RID: 8207
	public GamePlayer gamePlayer;

	// Token: 0x04002010 RID: 8208
	public MonkeBallGoalZone currGoalZone;
}
