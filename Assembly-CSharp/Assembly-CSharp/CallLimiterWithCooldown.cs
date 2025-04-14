using System;
using UnityEngine;

// Token: 0x020007E6 RID: 2022
[Serializable]
public class CallLimiterWithCooldown : CallLimiter
{
	// Token: 0x06003208 RID: 12808 RVA: 0x000F0B95 File Offset: 0x000EED95
	public CallLimiterWithCooldown(float coolDownSpam, int historyLength, float coolDown) : base(historyLength, coolDown, 0.5f)
	{
		this.spamCoolDown = coolDownSpam;
	}

	// Token: 0x06003209 RID: 12809 RVA: 0x000F0BAB File Offset: 0x000EEDAB
	public CallLimiterWithCooldown(float coolDownSpam, int historyLength, float coolDown, float latencyMax) : base(historyLength, coolDown, latencyMax)
	{
		this.spamCoolDown = coolDownSpam;
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x000F0BBE File Offset: 0x000EEDBE
	public override bool CheckCallTime(float time)
	{
		if (this.blockCall && time < this.blockStartTime + this.spamCoolDown)
		{
			this.blockStartTime = time;
			return false;
		}
		return base.CheckCallTime(time);
	}

	// Token: 0x0400359E RID: 13726
	[SerializeField]
	private float spamCoolDown;
}
