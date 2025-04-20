using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x0200094A RID: 2378
	internal struct LeaderboardEntry_t
	{
		// Token: 0x04003BB6 RID: 15286
		internal int m_nGlobalRank;

		// Token: 0x04003BB7 RID: 15287
		internal int m_nScore;

		// Token: 0x04003BB8 RID: 15288
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		internal string m_pUserName;
	}
}
