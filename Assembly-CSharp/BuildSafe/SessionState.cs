using System;

namespace BuildSafe
{
	// Token: 0x02000A61 RID: 2657
	public class SessionState
	{
		// Token: 0x170006A5 RID: 1701
		public string this[string key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x0400435B RID: 17243
		public static readonly SessionState Shared = new SessionState();
	}
}
