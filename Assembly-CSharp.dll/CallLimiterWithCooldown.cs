using System;
using UnityEngine;

// Token: 0x020007E6 RID: 2022
[Serializable]
public class CallLimiterWithCooldown : CallLimiter
{
	// Token: 0x06003208 RID: 12808 RVA: 0x0005045D File Offset: 0x0004E65D
	public CallLimiterWithCooldown(float coolDownSpam, int historyLength, float coolDown) : base(historyLength, coolDown, 0.5f)
	{
		this.spamCoolDown = coolDownSpam;
	}

	// Token: 0x06003209 RID: 12809 RVA: 0x00050473 File Offset: 0x0004E673
	public CallLimiterWithCooldown(float coolDownSpam, int historyLength, float coolDown, float latencyMax) : base(historyLength, coolDown, latencyMax)
	{
		this.spamCoolDown = coolDownSpam;
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x00050486 File Offset: 0x0004E686
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
