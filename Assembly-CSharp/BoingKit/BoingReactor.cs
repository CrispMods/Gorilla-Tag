using System;

namespace BoingKit
{
	// Token: 0x02000CCB RID: 3275
	public class BoingReactor : BoingBehavior
	{
		// Token: 0x06005298 RID: 21144 RVA: 0x00194471 File Offset: 0x00192671
		protected override void Register()
		{
			BoingManager.Register(this);
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x00194479 File Offset: 0x00192679
		protected override void Unregister()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x00194481 File Offset: 0x00192681
		public override void PrepareExecute()
		{
			base.PrepareExecute(true);
		}
	}
}
