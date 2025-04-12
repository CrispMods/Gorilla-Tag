using System;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class MonkeBallPlayer : MonoBehaviour
{
	// Token: 0x06001CDA RID: 7386 RVA: 0x00042BEB File Offset: 0x00040DEB
	private void Awake()
	{
		if (this.gamePlayer == null)
		{
			this.gamePlayer = base.GetComponent<GamePlayer>();
		}
	}

	// Token: 0x04001FC1 RID: 8129
	public GamePlayer gamePlayer;

	// Token: 0x04001FC2 RID: 8130
	public MonkeBallGoalZone currGoalZone;
}
