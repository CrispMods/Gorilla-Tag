using System;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class MonkeBallPlayer : MonoBehaviour
{
	// Token: 0x06001CD7 RID: 7383 RVA: 0x0008CC5B File Offset: 0x0008AE5B
	private void Awake()
	{
		if (this.gamePlayer == null)
		{
			this.gamePlayer = base.GetComponent<GamePlayer>();
		}
	}

	// Token: 0x04001FC0 RID: 8128
	public GamePlayer gamePlayer;

	// Token: 0x04001FC1 RID: 8129
	public MonkeBallGoalZone currGoalZone;
}
