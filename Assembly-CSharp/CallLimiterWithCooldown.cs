using System;
using UnityEngine;

// Token: 0x020007E3 RID: 2019
[Serializable]
public class CallLimiterWithCooldown : CallLimiter
{
	// Token: 0x060031FC RID: 12796 RVA: 0x000F05CD File Offset: 0x000EE7CD
	public CallLimiterWithCooldown(float coolDownSpam, int historyLength, float coolDown) : base(historyLength, coolDown, 0.5f)
	{
		this.spamCoolDown = coolDownSpam;
	}

	// Token: 0x060031FD RID: 12797 RVA: 0x000F05E3 File Offset: 0x000EE7E3
	public CallLimiterWithCooldown(float coolDownSpam, int historyLength, float coolDown, float latencyMax) : base(historyLength, coolDown, latencyMax)
	{
		this.spamCoolDown = coolDownSpam;
	}

	// Token: 0x060031FE RID: 12798 RVA: 0x000F05F6 File Offset: 0x000EE7F6
	public override bool CheckCallTime(float time)
	{
		if (this.blockCall && time < this.blockStartTime + this.spamCoolDown)
		{
			this.blockStartTime = time;
			return false;
		}
		return base.CheckCallTime(time);
	}

	// Token: 0x0400358C RID: 13708
	[SerializeField]
	private float spamCoolDown;
}
