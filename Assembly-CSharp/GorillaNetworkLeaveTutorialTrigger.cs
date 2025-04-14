using System;

// Token: 0x020007A1 RID: 1953
public class GorillaNetworkLeaveTutorialTrigger : GorillaTriggerBox
{
	// Token: 0x06003036 RID: 12342 RVA: 0x000E8EAC File Offset: 0x000E70AC
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		NetworkSystem.Instance.SetMyTutorialComplete();
	}
}
