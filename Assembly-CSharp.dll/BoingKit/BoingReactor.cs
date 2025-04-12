using System;

namespace BoingKit
{
	// Token: 0x02000CCE RID: 3278
	public class BoingReactor : BoingBehavior
	{
		// Token: 0x060052A4 RID: 21156 RVA: 0x00064CB4 File Offset: 0x00062EB4
		protected override void Register()
		{
			BoingManager.Register(this);
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x00064CBC File Offset: 0x00062EBC
		protected override void Unregister()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x00064CC4 File Offset: 0x00062EC4
		public override void PrepareExecute()
		{
			base.PrepareExecute(true);
		}
	}
}
