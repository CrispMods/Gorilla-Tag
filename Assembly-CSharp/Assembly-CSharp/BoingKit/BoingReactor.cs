using System;

namespace BoingKit
{
	// Token: 0x02000CCE RID: 3278
	public class BoingReactor : BoingBehavior
	{
		// Token: 0x060052A4 RID: 21156 RVA: 0x00194A39 File Offset: 0x00192C39
		protected override void Register()
		{
			BoingManager.Register(this);
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x00194A41 File Offset: 0x00192C41
		protected override void Unregister()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x00194A49 File Offset: 0x00192C49
		public override void PrepareExecute()
		{
			base.PrepareExecute(true);
		}
	}
}
