using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000930 RID: 2352
	internal struct LeaderboardEntry_t
	{
		// Token: 0x04003B03 RID: 15107
		internal int m_nGlobalRank;

		// Token: 0x04003B04 RID: 15108
		internal int m_nScore;

		// Token: 0x04003B05 RID: 15109
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		internal string m_pUserName;
	}
}
