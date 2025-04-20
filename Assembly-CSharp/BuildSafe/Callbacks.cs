using System;
using System.Diagnostics;

namespace BuildSafe
{
	// Token: 0x02000A4E RID: 2638
	public static class Callbacks
	{
		// Token: 0x02000A4F RID: 2639
		[Conditional("UNITY_EDITOR")]
		public class DidReloadScripts : Attribute
		{
			// Token: 0x0600423D RID: 16957 RVA: 0x0005B554 File Offset: 0x00059754
			public DidReloadScripts(bool activeOnly = false)
			{
				this.activeOnly = activeOnly;
			}

			// Token: 0x04004342 RID: 17218
			public bool activeOnly;
		}
	}
}
