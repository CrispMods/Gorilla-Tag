using System;
using System.Diagnostics;

namespace BuildSafe
{
	// Token: 0x02000A24 RID: 2596
	public static class Callbacks
	{
		// Token: 0x02000A25 RID: 2597
		[Conditional("UNITY_EDITOR")]
		public class DidReloadScripts : Attribute
		{
			// Token: 0x06004104 RID: 16644 RVA: 0x00059B52 File Offset: 0x00057D52
			public DidReloadScripts(bool activeOnly = false)
			{
				this.activeOnly = activeOnly;
			}

			// Token: 0x0400425A RID: 16986
			public bool activeOnly;
		}
	}
}
