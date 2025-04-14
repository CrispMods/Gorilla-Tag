using System;

namespace BuildSafe
{
	// Token: 0x02000A34 RID: 2612
	public class SessionState
	{
		// Token: 0x17000689 RID: 1673
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

		// Token: 0x04004261 RID: 16993
		public static readonly SessionState Shared = new SessionState();
	}
}
