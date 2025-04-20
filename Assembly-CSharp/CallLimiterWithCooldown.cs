using System;
using UnityEngine;

// Token: 0x020007FD RID: 2045
[Serializable]
public class CallLimiterWithCooldown : CallLimiter
{
	// Token: 0x060032B2 RID: 12978 RVA: 0x0005185F File Offset: 0x0004FA5F
	public CallLimiterWithCooldown(float coolDownSpam, int historyLength, float coolDown) : base(historyLength, coolDown, 0.5f)
	{
		this.spamCoolDown = coolDownSpam;
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x00051875 File Offset: 0x0004FA75
	public CallLimiterWithCooldown(float coolDownSpam, int historyLength, float coolDown, float latencyMax) : base(historyLength, coolDown, latencyMax)
	{
		this.spamCoolDown = coolDownSpam;
	}

	// Token: 0x060032B4 RID: 12980 RVA: 0x00051888 File Offset: 0x0004FA88
	public override bool CheckCallTime(float time)
	{
		if (this.blockCall && time < this.blockStartTime + this.spamCoolDown)
		{
			this.blockStartTime = time;
			return false;
		}
		return base.CheckCallTime(time);
	}

	// Token: 0x04003642 RID: 13890
	[SerializeField]
	private float spamCoolDown;
}
