using System;

namespace GorillaNetworking
{
	// Token: 0x02000AE1 RID: 2785
	public class GorillaKeyboardButton : GorillaKeyButton<GorillaKeyboardBindings>
	{
		// Token: 0x06004620 RID: 17952 RVA: 0x0005DC20 File Offset: 0x0005BE20
		public override void OnButtonPressedEvent()
		{
			GameEvents.OnGorrillaKeyboardButtonPressedEvent.Invoke(this.Binding);
		}
	}
}
