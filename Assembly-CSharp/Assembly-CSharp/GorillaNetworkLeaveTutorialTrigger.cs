using System;

// Token: 0x020007A2 RID: 1954
public class GorillaNetworkLeaveTutorialTrigger : GorillaTriggerBox
{
	// Token: 0x0600303E RID: 12350 RVA: 0x000E932C File Offset: 0x000E752C
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		NetworkSystem.Instance.SetMyTutorialComplete();
	}
}
