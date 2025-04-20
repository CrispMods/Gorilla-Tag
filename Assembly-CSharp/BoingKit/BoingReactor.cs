using System;

namespace BoingKit
{
	// Token: 0x02000CFC RID: 3324
	public class BoingReactor : BoingBehavior
	{
		// Token: 0x060053FA RID: 21498 RVA: 0x0006672A File Offset: 0x0006492A
		protected override void Register()
		{
			BoingManager.Register(this);
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x00066732 File Offset: 0x00064932
		protected override void Unregister()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x0006673A File Offset: 0x0006493A
		public override void PrepareExecute()
		{
			base.PrepareExecute(true);
		}
	}
}
