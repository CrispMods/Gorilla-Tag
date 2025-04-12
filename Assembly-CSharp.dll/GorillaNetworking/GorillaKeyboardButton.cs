using System;

namespace GorillaNetworking
{
	// Token: 0x02000AB8 RID: 2744
	public class GorillaKeyboardButton : GorillaKeyButton<GorillaKeyboardBindings>
	{
		// Token: 0x060044E9 RID: 17641 RVA: 0x0005C249 File Offset: 0x0005A449
		public override void OnButtonPressedEvent()
		{
			GameEvents.OnGorrillaKeyboardButtonPressedEvent.Invoke(this.Binding);
		}
	}
}
