using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000930 RID: 2352
	internal class ArcadeLeaderboard
	{
		// Token: 0x06003877 RID: 14455 RVA: 0x00108133 File Offset: 0x00106333
		static ArcadeLeaderboard()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x06003878 RID: 14456
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_IsReady")]
		internal static extern void IsReady(StatusCallback IsReadyCallback);

		// Token: 0x06003879 RID: 14457
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_IsReady")]
		internal static extern void IsReady_64(StatusCallback IsReadyCallback);

		// Token: 0x0600387A RID: 14458
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_DownloadLeaderboardScores")]
		internal static extern void DownloadLeaderboardScores(StatusCallback downloadLeaderboardScoresCB, string pchLeaderboardName, ELeaderboardDataTimeRange eLeaderboardDataTimeRange, int nCount);

		// Token: 0x0600387B RID: 14459
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_DownloadLeaderboardScores")]
		internal static extern void DownloadLeaderboardScores_64(StatusCallback downloadLeaderboardScoresCB, string pchLeaderboardName, ELeaderboardDataTimeRange eLeaderboardDataTimeRange, int nCount);

		// Token: 0x0600387C RID: 14460
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_UploadLeaderboardScore")]
		internal static extern void UploadLeaderboardScore(StatusCallback uploadLeaderboardScoreCB, string pchLeaderboardName, string pchUserName, int nScore);

		// Token: 0x0600387D RID: 14461
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_UploadLeaderboardScore")]
		internal static extern void UploadLeaderboardScore_64(StatusCallback uploadLeaderboardScoreCB, string pchLeaderboardName, string pchUserName, int nScore);

		// Token: 0x0600387E RID: 14462
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardScore")]
		internal static extern void GetLeaderboardScore(int index, ref LeaderboardEntry_t pLeaderboardEntry);

		// Token: 0x0600387F RID: 14463
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardScore")]
		internal static extern void GetLeaderboardScore_64(int index, ref LeaderboardEntry_t pLeaderboardEntry);

		// Token: 0x06003880 RID: 14464
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardScoreCount")]
		internal static extern int GetLeaderboardScoreCount();

		// Token: 0x06003881 RID: 14465
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardScoreCount")]
		internal static extern int GetLeaderboardScoreCount_64();

		// Token: 0x06003882 RID: 14466
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardUserRank")]
		internal static extern int GetLeaderboardUserRank();

		// Token: 0x06003883 RID: 14467
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardUserRank")]
		internal static extern int GetLeaderboardUserRank_64();

		// Token: 0x06003884 RID: 14468
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardUserScore")]
		internal static extern int GetLeaderboardUserScore();

		// Token: 0x06003885 RID: 14469
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportArcadeLeaderboard_GetLeaderboardUserScore")]
		internal static extern int GetLeaderboardUserScore_64();
	}
}
