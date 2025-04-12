using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000936 RID: 2358
	internal class UserStats
	{
		// Token: 0x060038AB RID: 14507 RVA: 0x0005457C File Offset: 0x0005277C
		static UserStats()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x060038AC RID: 14508
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_IsReady")]
		internal static extern int IsReady(StatusCallback IsReadyCallback);

		// Token: 0x060038AD RID: 14509
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_IsReady")]
		internal static extern int IsReady_64(StatusCallback IsReadyCallback);

		// Token: 0x060038AE RID: 14510
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_DownloadStats")]
		internal static extern int DownloadStats(StatusCallback downloadStatsCallback);

		// Token: 0x060038AF RID: 14511
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_DownloadStats")]
		internal static extern int DownloadStats_64(StatusCallback downloadStatsCallback);

		// Token: 0x060038B0 RID: 14512
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetStat0")]
		internal static extern int GetStat(string pchName, ref int pnData);

		// Token: 0x060038B1 RID: 14513
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetStat0")]
		internal static extern int GetStat_64(string pchName, ref int pnData);

		// Token: 0x060038B2 RID: 14514
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetStat")]
		internal static extern int GetStat(string pchName, ref float pfData);

		// Token: 0x060038B3 RID: 14515
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetStat")]
		internal static extern int GetStat_64(string pchName, ref float pfData);

		// Token: 0x060038B4 RID: 14516
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_SetStat0")]
		internal static extern int SetStat(string pchName, int nData);

		// Token: 0x060038B5 RID: 14517
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_SetStat0")]
		internal static extern int SetStat_64(string pchName, int nData);

		// Token: 0x060038B6 RID: 14518
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_SetStat")]
		internal static extern int SetStat(string pchName, float fData);

		// Token: 0x060038B7 RID: 14519
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_SetStat")]
		internal static extern int SetStat_64(string pchName, float fData);

		// Token: 0x060038B8 RID: 14520
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_UploadStats")]
		internal static extern int UploadStats(StatusCallback uploadStatsCallback);

		// Token: 0x060038B9 RID: 14521
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_UploadStats")]
		internal static extern int UploadStats_64(StatusCallback uploadStatsCallback);

		// Token: 0x060038BA RID: 14522
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetAchievement")]
		internal static extern int GetAchievement(string pchName, ref int pbAchieved);

		// Token: 0x060038BB RID: 14523
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetAchievement")]
		internal static extern int GetAchievement_64(string pchName, ref int pbAchieved);

		// Token: 0x060038BC RID: 14524
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetAchievementUnlockTime")]
		internal static extern int GetAchievementUnlockTime(string pchName, ref int punUnlockTime);

		// Token: 0x060038BD RID: 14525
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetAchievementUnlockTime")]
		internal static extern int GetAchievementUnlockTime_64(string pchName, ref int punUnlockTime);

		// Token: 0x060038BE RID: 14526
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_SetAchievement")]
		internal static extern int SetAchievement(string pchName);

		// Token: 0x060038BF RID: 14527
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_SetAchievement")]
		internal static extern int SetAchievement_64(string pchName);

		// Token: 0x060038C0 RID: 14528
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_ClearAchievement")]
		internal static extern int ClearAchievement(string pchName);

		// Token: 0x060038C1 RID: 14529
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_ClearAchievement")]
		internal static extern int ClearAchievement_64(string pchName);

		// Token: 0x060038C2 RID: 14530
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_DownloadLeaderboardScores")]
		internal static extern int DownloadLeaderboardScores(StatusCallback downloadLeaderboardScoresCB, string pchLeaderboardName, ELeaderboardDataRequest eLeaderboardDataRequest, ELeaderboardDataTimeRange eLeaderboardDataTimeRange, int nRangeStart, int nRangeEnd);

		// Token: 0x060038C3 RID: 14531
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_DownloadLeaderboardScores")]
		internal static extern int DownloadLeaderboardScores_64(StatusCallback downloadLeaderboardScoresCB, string pchLeaderboardName, ELeaderboardDataRequest eLeaderboardDataRequest, ELeaderboardDataTimeRange eLeaderboardDataTimeRange, int nRangeStart, int nRangeEnd);

		// Token: 0x060038C4 RID: 14532
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_UploadLeaderboardScore")]
		internal static extern int UploadLeaderboardScore(StatusCallback uploadLeaderboardScoreCB, string pchLeaderboardName, int nScore);

		// Token: 0x060038C5 RID: 14533
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_UploadLeaderboardScore")]
		internal static extern int UploadLeaderboardScore_64(StatusCallback uploadLeaderboardScoreCB, string pchLeaderboardName, int nScore);

		// Token: 0x060038C6 RID: 14534
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardScore")]
		internal static extern int GetLeaderboardScore(int index, ref LeaderboardEntry_t pLeaderboardEntry);

		// Token: 0x060038C7 RID: 14535
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardScore")]
		internal static extern int GetLeaderboardScore_64(int index, ref LeaderboardEntry_t pLeaderboardEntry);

		// Token: 0x060038C8 RID: 14536
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardScoreCount")]
		internal static extern int GetLeaderboardScoreCount();

		// Token: 0x060038C9 RID: 14537
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardScoreCount")]
		internal static extern int GetLeaderboardScoreCount_64();

		// Token: 0x060038CA RID: 14538
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardSortMethod")]
		internal static extern ELeaderboardSortMethod GetLeaderboardSortMethod();

		// Token: 0x060038CB RID: 14539
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardSortMethod")]
		internal static extern ELeaderboardSortMethod GetLeaderboardSortMethod_64();

		// Token: 0x060038CC RID: 14540
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardDisplayType")]
		internal static extern ELeaderboardDisplayType GetLeaderboardDisplayType();

		// Token: 0x060038CD RID: 14541
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUserStats_GetLeaderboardDisplayType")]
		internal static extern ELeaderboardDisplayType GetLeaderboardDisplayType_64();
	}
}
