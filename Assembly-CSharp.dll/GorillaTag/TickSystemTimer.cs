using System;
using System.Runtime.CompilerServices;

namespace GorillaTag
{
	// Token: 0x02000BB8 RID: 3000
	internal class TickSystemTimer : TickSystemTimerAbstract
	{
		// Token: 0x06004BDB RID: 19419 RVA: 0x00060F5A File Offset: 0x0005F15A
		public TickSystemTimer()
		{
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x00061034 File Offset: 0x0005F234
		public TickSystemTimer(float cd) : base(cd)
		{
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x0006103D File Offset: 0x0005F23D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void OnTimedEvent()
		{
			Action action = this.callback;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04004DAE RID: 19886
		public Action callback;
	}
}
