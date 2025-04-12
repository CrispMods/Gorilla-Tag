using System;

// Token: 0x020007A2 RID: 1954
public class GorillaNetworkLeaveTutorialTrigger : GorillaTriggerBox
{
	// Token: 0x0600303E RID: 12350 RVA: 0x0004F202 File Offset: 0x0004D402
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		NetworkSystem.Instance.SetMyTutorialComplete();
	}
}
