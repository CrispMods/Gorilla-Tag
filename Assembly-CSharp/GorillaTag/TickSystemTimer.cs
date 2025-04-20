using System;
using System.Runtime.CompilerServices;

namespace GorillaTag
{
	// Token: 0x02000BE3 RID: 3043
	internal class TickSystemTimer : TickSystemTimerAbstract
	{
		// Token: 0x06004D18 RID: 19736 RVA: 0x000628C1 File Offset: 0x00060AC1
		public TickSystemTimer()
		{
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x000629D6 File Offset: 0x00060BD6
		public TickSystemTimer(float cd) : base(cd)
		{
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x000629DF File Offset: 0x00060BDF
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

		// Token: 0x04004E92 RID: 20114
		public Action callback;
	}
}
