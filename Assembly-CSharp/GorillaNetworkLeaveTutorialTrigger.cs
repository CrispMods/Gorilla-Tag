using System;

// Token: 0x020007B9 RID: 1977
public class GorillaNetworkLeaveTutorialTrigger : GorillaTriggerBox
{
	// Token: 0x060030E8 RID: 12520 RVA: 0x00050604 File Offset: 0x0004E804
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		NetworkSystem.Instance.SetMyTutorialComplete();
	}
}
