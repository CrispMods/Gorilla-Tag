using System;

namespace GorillaNetworking
{
	// Token: 0x02000AB5 RID: 2741
	public class GorillaKeyboardButton : GorillaKeyButton<GorillaKeyboardBindings>
	{
		// Token: 0x060044DD RID: 17629 RVA: 0x001474A2 File Offset: 0x001456A2
		public override void OnButtonPressedEvent()
		{
			GameEvents.OnGorrillaKeyboardButtonPressedEvent.Invoke(this.Binding);
		}
	}
}
