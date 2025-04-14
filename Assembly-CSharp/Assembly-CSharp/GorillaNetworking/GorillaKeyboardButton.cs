using System;

namespace GorillaNetworking
{
	// Token: 0x02000AB8 RID: 2744
	public class GorillaKeyboardButton : GorillaKeyButton<GorillaKeyboardBindings>
	{
		// Token: 0x060044E9 RID: 17641 RVA: 0x00147A6A File Offset: 0x00145C6A
		public override void OnButtonPressedEvent()
		{
			GameEvents.OnGorrillaKeyboardButtonPressedEvent.Invoke(this.Binding);
		}
	}
}
