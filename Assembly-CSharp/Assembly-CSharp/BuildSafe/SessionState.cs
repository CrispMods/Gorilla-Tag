using System;

namespace BuildSafe
{
	// Token: 0x02000A37 RID: 2615
	public class SessionState
	{
		// Token: 0x1700068A RID: 1674
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

		// Token: 0x04004273 RID: 17011
		public static readonly SessionState Shared = new SessionState();
	}
}
