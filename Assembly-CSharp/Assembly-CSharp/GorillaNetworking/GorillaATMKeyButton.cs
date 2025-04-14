using System;

namespace GorillaNetworking
{
	// Token: 0x02000AA9 RID: 2729
	public class GorillaATMKeyButton : GorillaKeyButton<GorillaATMKeyBindings>
	{
		// Token: 0x0600441E RID: 17438 RVA: 0x00142D45 File Offset: 0x00140F45
		public override void OnButtonPressedEvent()
		{
			GameEvents.OnGorrillaATMKeyButtonPressedEvent.Invoke(this.Binding);
		}
	}
}
