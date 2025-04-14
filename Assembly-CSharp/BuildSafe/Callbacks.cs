using System;
using System.Diagnostics;

namespace BuildSafe
{
	// Token: 0x02000A21 RID: 2593
	public static class Callbacks
	{
		// Token: 0x02000A22 RID: 2594
		[Conditional("UNITY_EDITOR")]
		public class DidReloadScripts : Attribute
		{
			// Token: 0x060040F8 RID: 16632 RVA: 0x00134D3D File Offset: 0x00132F3D
			public DidReloadScripts(bool activeOnly = false)
			{
				this.activeOnly = activeOnly;
			}

			// Token: 0x04004248 RID: 16968
			public bool activeOnly;
		}
	}
}
