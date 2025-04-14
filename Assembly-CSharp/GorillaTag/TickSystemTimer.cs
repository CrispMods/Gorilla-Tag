using System;
using System.Runtime.CompilerServices;

namespace GorillaTag
{
	// Token: 0x02000BB5 RID: 2997
	internal class TickSystemTimer : TickSystemTimerAbstract
	{
		// Token: 0x06004BCF RID: 19407 RVA: 0x00170F2A File Offset: 0x0016F12A
		public TickSystemTimer()
		{
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x00171046 File Offset: 0x0016F246
		public TickSystemTimer(float cd) : base(cd)
		{
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x0017104F File Offset: 0x0016F24F
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

		// Token: 0x04004D9C RID: 19868
		public Action callback;
	}
}
