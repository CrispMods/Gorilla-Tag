using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x0200092D RID: 2349
	internal struct LeaderboardEntry_t
	{
		// Token: 0x04003AF1 RID: 15089
		internal int m_nGlobalRank;

		// Token: 0x04003AF2 RID: 15090
		internal int m_nScore;

		// Token: 0x04003AF3 RID: 15091
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		internal string m_pUserName;
	}
}
