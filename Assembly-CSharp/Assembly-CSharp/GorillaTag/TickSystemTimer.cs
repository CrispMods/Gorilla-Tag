using System;
using System.Runtime.CompilerServices;

namespace GorillaTag
{
	// Token: 0x02000BB8 RID: 3000
	internal class TickSystemTimer : TickSystemTimerAbstract
	{
		// Token: 0x06004BDB RID: 19419 RVA: 0x001714F2 File Offset: 0x0016F6F2
		public TickSystemTimer()
		{
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x0017160E File Offset: 0x0016F80E
		public TickSystemTimer(float cd) : base(cd)
		{
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x00171617 File Offset: 0x0016F817
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
