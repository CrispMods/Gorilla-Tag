using System;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x0200090B RID: 2315
	public class UserStats
	{
		// Token: 0x06003788 RID: 14216 RVA: 0x001060B8 File Offset: 0x001042B8
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsReadyIl2cppCallback(int errorCode)
		{
			UserStats.isReadyIl2cppCallback(errorCode);
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x001060C8 File Offset: 0x001042C8
		public static int IsReady(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			UserStats.isReadyIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(UserStats.IsReadyIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				return UserStats.IsReady_64(new StatusCallback(UserStats.IsReadyIl2cppCallback));
			}
			return UserStats.IsReady(new StatusCallback(UserStats.IsReadyIl2cppCallback));
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x00106135 File Offset: 0x00104335
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void DownloadStatsIl2cppCallback(int errorCode)
		{
			UserStats.downloadStatsIl2cppCallback(errorCode);
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x00106144 File Offset: 0x00104344
		public static int DownloadStats(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			UserStats.downloadStatsIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(UserStats.DownloadStatsIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				return UserStats.DownloadStats_64(new StatusCallback(UserStats.DownloadStatsIl2cppCallback));
			}
			return UserStats.DownloadStats(new StatusCallback(UserStats.DownloadStatsIl2cppCallback));
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x001061B4 File Offset: 0x001043B4
		public static int GetStat(string name, int defaultValue)
		{
			int result = defaultValue;
			if (IntPtr.Size == 8)
			{
				UserStats.GetStat_64(name, ref result);
			}
			else
			{
				UserStats.GetStat(name, ref result);
			}
			return result;
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x001061E0 File Offset: 0x001043E0
		public static float GetStat(string name, float defaultValue)
		{
			float result = defaultValue;
			if (IntPtr.Size == 8)
			{
				UserStats.GetStat_64(name, ref result);
			}
			else
			{
				UserStats.GetStat(name, ref result);
			}
			return result;
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x0010620C File Offset: 0x0010440C
		public static void SetStat(string name, int value)
		{
			if (IntPtr.Size == 8)
			{
				UserStats.SetStat_64(name, value);
				return;
			}
			UserStats.SetStat(name, value);
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x00106227 File Offset: 0x00104427
		public static void SetStat(string name, float value)
		{
			if (IntPtr.Size == 8)
			{
				UserStats.SetStat_64(name, value);
				return;
			}
			UserStats.SetStat(name, value);
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x00106242 File Offset: 0x00104442
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void UploadStatsIl2cppCallback(int errorCode)
		{
			UserStats.uploadStatsIl2cppCallback(errorCode);
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x00106250 File Offset: 0x00104450
		public static int UploadStats(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			UserStats.uploadStatsIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(UserStats.UploadStatsIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				return UserStats.UploadStats_64(new StatusCallback(UserStats.UploadStatsIl2cppCallback));
			}
			return UserStats.UploadStats(new StatusCallback(UserStats.UploadStatsIl2cppCallback));
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x001062C0 File Offset: 0x001044C0
		public static bool GetAchievement(string pchName)
		{
			int num = 0;
			if (IntPtr.Size == 8)
			{
				UserStats.GetAchievement_64(pchName, ref num);
			}
			else
			{
				UserStats.GetAchievement(pchName, ref num);
			}
			return num == 1;
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x001062F0 File Offset: 0x001044F0
		public static int GetAchievementUnlockTime(string pchName)
		{
			int result = 0;
			if (IntPtr.Size == 8)
			{
				UserStats.GetAchievementUnlockTime_64(pchName, ref result);
			}
			else
			{
				UserStats.GetAchievementUnlockTime(pchName, ref result);
			}
			return result;
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x0010631C File Offset: 0x0010451C
		public static string GetAchievementIcon(string pchName)
		{
			return "";
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x0010631C File Offset: 0x0010451C
		public static string GetAchievementDisplayAttribute(string pchName, UserStats.AchievementDisplayAttribute attr)
		{
			return "";
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x0010631C File Offset: 0x0010451C
		public static string GetAchievementDisplayAttribute(string pchName, UserStats.AchievementDisplayAttribute attr, Locale locale)
		{
			return "";
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x00106323 File Offset: 0x00104523
		public static int SetAchievement(string pchName)
		{
			if (IntPtr.Size == 8)
			{
				return UserStats.SetAchievement_64(pchName);
			}
			return UserStats.SetAchievement(pchName);
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x0010633A File Offset: 0x0010453A
		public static int ClearAchievement(string pchName)
		{
			if (IntPtr.Size == 8)
			{
				return UserStats.ClearAchievement_64(pchName);
			}
			return UserStats.ClearAchievement(pchName);
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x00106351 File Offset: 0x00104551
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void DownloadLeaderboardScoresIl2cppCallback(int errorCode)
		{
			UserStats.downloadLeaderboardScoresIl2cppCallback(errorCode);
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x00106360 File Offset: 0x00104560
		public static int DownloadLeaderboardScores(StatusCallback callback, string pchLeaderboardName, UserStats.LeaderBoardRequestType eLeaderboardDataRequest, UserStats.LeaderBoardTimeRange eLeaderboardDataTimeRange, int nRangeStart, int nRangeEnd)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			UserStats.downloadLeaderboardScoresIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(UserStats.DownloadLeaderboardScoresIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				return UserStats.DownloadLeaderboardScores_64(new StatusCallback(UserStats.DownloadLeaderboardScoresIl2cppCallback), pchLeaderboardName, (ELeaderboardDataRequest)eLeaderboardDataRequest, (ELeaderboardDataTimeRange)eLeaderboardDataTimeRange, nRangeStart, nRangeEnd);
			}
			return UserStats.DownloadLeaderboardScores(new StatusCallback(UserStats.DownloadLeaderboardScoresIl2cppCallback), pchLeaderboardName, (ELeaderboardDataRequest)eLeaderboardDataRequest, (ELeaderboardDataTimeRange)eLeaderboardDataTimeRange, nRangeStart, nRangeEnd);
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x001063DB File Offset: 0x001045DB
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void UploadLeaderboardScoreIl2cppCallback(int errorCode)
		{
			UserStats.uploadLeaderboardScoreIl2cppCallback(errorCode);
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x001063E8 File Offset: 0x001045E8
		public static int UploadLeaderboardScore(StatusCallback callback, string pchLeaderboardName, int nScore)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			UserStats.uploadLeaderboardScoreIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(UserStats.UploadLeaderboardScoreIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				return UserStats.UploadLeaderboardScore_64(new StatusCallback(UserStats.UploadLeaderboardScoreIl2cppCallback), pchLeaderboardName, nScore);
			}
			return UserStats.UploadLeaderboardScore(new StatusCallback(UserStats.UploadLeaderboardScoreIl2cppCallback), pchLeaderboardName, nScore);
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x0010645C File Offset: 0x0010465C
		public static Leaderboard GetLeaderboardScore(int index)
		{
			LeaderboardEntry_t leaderboardEntry_t;
			leaderboardEntry_t.m_nGlobalRank = 0;
			leaderboardEntry_t.m_nScore = 0;
			leaderboardEntry_t.m_pUserName = "";
			if (IntPtr.Size == 8)
			{
				UserStats.GetLeaderboardScore_64(index, ref leaderboardEntry_t);
			}
			else
			{
				UserStats.GetLeaderboardScore(index, ref leaderboardEntry_t);
			}
			return new Leaderboard
			{
				Rank = leaderboardEntry_t.m_nGlobalRank,
				Score = leaderboardEntry_t.m_nScore,
				UserName = leaderboardEntry_t.m_pUserName
			};
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x001064CA File Offset: 0x001046CA
		public static int GetLeaderboardScoreCount()
		{
			if (IntPtr.Size == 8)
			{
				return UserStats.GetLeaderboardScoreCount_64();
			}
			return UserStats.GetLeaderboardScoreCount();
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x001064DF File Offset: 0x001046DF
		public static UserStats.LeaderBoardSortMethod GetLeaderboardSortMethod()
		{
			if (IntPtr.Size == 8)
			{
				return (UserStats.LeaderBoardSortMethod)UserStats.GetLeaderboardSortMethod_64();
			}
			return (UserStats.LeaderBoardSortMethod)UserStats.GetLeaderboardSortMethod();
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x001064F4 File Offset: 0x001046F4
		public static UserStats.LeaderBoardDiaplayType GetLeaderboardDisplayType()
		{
			if (IntPtr.Size == 8)
			{
				return (UserStats.LeaderBoardDiaplayType)UserStats.GetLeaderboardDisplayType_64();
			}
			return (UserStats.LeaderBoardDiaplayType)UserStats.GetLeaderboardDisplayType();
		}

		// Token: 0x04003A73 RID: 14963
		private static StatusCallback isReadyIl2cppCallback;

		// Token: 0x04003A74 RID: 14964
		private static StatusCallback downloadStatsIl2cppCallback;

		// Token: 0x04003A75 RID: 14965
		private static StatusCallback uploadStatsIl2cppCallback;

		// Token: 0x04003A76 RID: 14966
		private static StatusCallback downloadLeaderboardScoresIl2cppCallback;

		// Token: 0x04003A77 RID: 14967
		private static StatusCallback uploadLeaderboardScoreIl2cppCallback;

		// Token: 0x0200090C RID: 2316
		public enum LeaderBoardRequestType
		{
			// Token: 0x04003A79 RID: 14969
			GlobalData,
			// Token: 0x04003A7A RID: 14970
			GlobalDataAroundUser,
			// Token: 0x04003A7B RID: 14971
			LocalData,
			// Token: 0x04003A7C RID: 14972
			LocalDataAroundUser
		}

		// Token: 0x0200090D RID: 2317
		public enum LeaderBoardTimeRange
		{
			// Token: 0x04003A7E RID: 14974
			AllTime,
			// Token: 0x04003A7F RID: 14975
			Daily,
			// Token: 0x04003A80 RID: 14976
			Weekly,
			// Token: 0x04003A81 RID: 14977
			Monthly
		}

		// Token: 0x0200090E RID: 2318
		public enum LeaderBoardSortMethod
		{
			// Token: 0x04003A83 RID: 14979
			None,
			// Token: 0x04003A84 RID: 14980
			Ascending,
			// Token: 0x04003A85 RID: 14981
			Descending
		}

		// Token: 0x0200090F RID: 2319
		public enum LeaderBoardDiaplayType
		{
			// Token: 0x04003A87 RID: 14983
			None,
			// Token: 0x04003A88 RID: 14984
			Numeric,
			// Token: 0x04003A89 RID: 14985
			TimeSeconds,
			// Token: 0x04003A8A RID: 14986
			TimeMilliSeconds
		}

		// Token: 0x02000910 RID: 2320
		public enum LeaderBoardScoreMethod
		{
			// Token: 0x04003A8C RID: 14988
			None,
			// Token: 0x04003A8D RID: 14989
			KeepBest,
			// Token: 0x04003A8E RID: 14990
			ForceUpdate
		}

		// Token: 0x02000911 RID: 2321
		public enum AchievementDisplayAttribute
		{
			// Token: 0x04003A90 RID: 14992
			Name,
			// Token: 0x04003A91 RID: 14993
			Desc,
			// Token: 0x04003A92 RID: 14994
			Hidden
		}
	}
}
