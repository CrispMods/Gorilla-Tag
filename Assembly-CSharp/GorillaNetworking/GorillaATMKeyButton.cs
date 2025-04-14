using System;

namespace GorillaNetworking
{
	// Token: 0x02000AA6 RID: 2726
	public class GorillaATMKeyButton : GorillaKeyButton<GorillaATMKeyBindings>
	{
		// Token: 0x06004412 RID: 17426 RVA: 0x0014277D File Offset: 0x0014097D
		public override void OnButtonPressedEvent()
		{
			GameEvents.OnGorrillaATMKeyButtonPressedEvent.Invoke(this.Binding);
		}
	}
}
