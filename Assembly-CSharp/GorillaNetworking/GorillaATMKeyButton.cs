using System;

namespace GorillaNetworking
{
	// Token: 0x02000AD2 RID: 2770
	public class GorillaATMKeyButton : GorillaKeyButton<GorillaATMKeyBindings>
	{
		// Token: 0x06004555 RID: 17749 RVA: 0x0005D355 File Offset: 0x0005B555
		public override void OnButtonPressedEvent()
		{
			GameEvents.OnGorrillaATMKeyButtonPressedEvent.Invoke(this.Binding);
		}
	}
}
