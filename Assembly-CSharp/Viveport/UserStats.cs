using System;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x02000908 RID: 2312
	public class UserStats
	{
		// Token: 0x0600377C RID: 14204 RVA: 0x00105AF0 File Offset: 0x00103CF0
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsReadyIl2cppCallback(int errorCode)
		{
			UserStats.isReadyIl2cppCallback(errorCode);
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x00105B00 File Offset: 0x00103D00
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

		// Token: 0x0600377E RID: 14206 RVA: 0x00105B6D File Offset: 0x00103D6D
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void DownloadStatsIl2cppCallback(int errorCode)
		{
			UserStats.downloadStatsIl2cppCallback(errorCode);
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x00105B7C File Offset: 0x00103D7C
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

		// Token: 0x06003780 RID: 14208 RVA: 0x00105BEC File Offset: 0x00103DEC
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

		// Token: 0x06003781 RID: 14209 RVA: 0x00105C18 File Offset: 0x00103E18
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

		// Token: 0x06003782 RID: 14210 RVA: 0x00105C44 File Offset: 0x00103E44
		public static void SetStat(string name, int value)
		{
			if (IntPtr.Size == 8)
			{
				UserStats.SetStat_64(name, value);
				return;
			}
			UserStats.SetStat(name, value);
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x00105C5F File Offset: 0x00103E5F
		public static void SetStat(string name, float value)
		{
			if (IntPtr.Size == 8)
			{
				UserStats.SetStat_64(name, value);
				return;
			}
			UserStats.SetStat(name, value);
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x00105C7A File Offset: 0x00103E7A
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void UploadStatsIl2cppCallback(int errorCode)
		{
			UserStats.uploadStatsIl2cppCallback(errorCode);
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x00105C88 File Offset: 0x00103E88
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

		// Token: 0x06003786 RID: 14214 RVA: 0x00105CF8 File Offset: 0x00103EF8
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

		// Token: 0x06003787 RID: 14215 RVA: 0x00105D28 File Offset: 0x00103F28
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

		// Token: 0x06003788 RID: 14216 RVA: 0x00105D54 File Offset: 0x00103F54
		public static string GetAchievementIcon(string pchName)
		{
			return "";
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x00105D54 File Offset: 0x00103F54
		public static string GetAchievementDisplayAttribute(string pchName, UserStats.AchievementDisplayAttribute attr)
		{
			return "";
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x00105D54 File Offset: 0x00103F54
		public static string GetAchievementDisplayAttribute(string pchName, UserStats.AchievementDisplayAttribute attr, Locale locale)
		{
			return "";
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x00105D5B File Offset: 0x00103F5B
		public static int SetAchievement(string pchName)
		{
			if (IntPtr.Size == 8)
			{
				return UserStats.SetAchievement_64(pchName);
			}
			return UserStats.SetAchievement(pchName);
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x00105D72 File Offset: 0x00103F72
		public static int ClearAchievement(string pchName)
		{
			if (IntPtr.Size == 8)
			{
				return UserStats.ClearAchievement_64(pchName);
			}
			return UserStats.ClearAchievement(pchName);
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x00105D89 File Offset: 0x00103F89
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void DownloadLeaderboardScoresIl2cppCallback(int errorCode)
		{
			UserStats.downloadLeaderboardScoresIl2cppCallback(errorCode);
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x00105D98 File Offset: 0x00103F98
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

		// Token: 0x0600378F RID: 14223 RVA: 0x00105E13 File Offset: 0x00104013
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void UploadLeaderboardScoreIl2cppCallback(int errorCode)
		{
			UserStats.uploadLeaderboardScoreIl2cppCallback(errorCode);
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x00105E20 File Offset: 0x00104020
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

		// Token: 0x06003791 RID: 14225 RVA: 0x00105E94 File Offset: 0x00104094
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

		// Token: 0x06003792 RID: 14226 RVA: 0x00105F02 File Offset: 0x00104102
		public static int GetLeaderboardScoreCount()
		{
			if (IntPtr.Size == 8)
			{
				return UserStats.GetLeaderboardScoreCount_64();
			}
			return UserStats.GetLeaderboardScoreCount();
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x00105F17 File Offset: 0x00104117
		public static UserStats.LeaderBoardSortMethod GetLeaderboardSortMethod()
		{
			if (IntPtr.Size == 8)
			{
				return (UserStats.LeaderBoardSortMethod)UserStats.GetLeaderboardSortMethod_64();
			}
			return (UserStats.LeaderBoardSortMethod)UserStats.GetLeaderboardSortMethod();
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x00105F2C File Offset: 0x0010412C
		public static UserStats.LeaderBoardDiaplayType GetLeaderboardDisplayType()
		{
			if (IntPtr.Size == 8)
			{
				return (UserStats.LeaderBoardDiaplayType)UserStats.GetLeaderboardDisplayType_64();
			}
			return (UserStats.LeaderBoardDiaplayType)UserStats.GetLeaderboardDisplayType();
		}

		// Token: 0x04003A61 RID: 14945
		private static StatusCallback isReadyIl2cppCallback;

		// Token: 0x04003A62 RID: 14946
		private static StatusCallback downloadStatsIl2cppCallback;

		// Token: 0x04003A63 RID: 14947
		private static StatusCallback uploadStatsIl2cppCallback;

		// Token: 0x04003A64 RID: 14948
		private static StatusCallback downloadLeaderboardScoresIl2cppCallback;

		// Token: 0x04003A65 RID: 14949
		private static StatusCallback uploadLeaderboardScoreIl2cppCallback;

		// Token: 0x02000909 RID: 2313
		public enum LeaderBoardRequestType
		{
			// Token: 0x04003A67 RID: 14951
			GlobalData,
			// Token: 0x04003A68 RID: 14952
			GlobalDataAroundUser,
			// Token: 0x04003A69 RID: 14953
			LocalData,
			// Token: 0x04003A6A RID: 14954
			LocalDataAroundUser
		}

		// Token: 0x0200090A RID: 2314
		public enum LeaderBoardTimeRange
		{
			// Token: 0x04003A6C RID: 14956
			AllTime,
			// Token: 0x04003A6D RID: 14957
			Daily,
			// Token: 0x04003A6E RID: 14958
			Weekly,
			// Token: 0x04003A6F RID: 14959
			Monthly
		}

		// Token: 0x0200090B RID: 2315
		public enum LeaderBoardSortMethod
		{
			// Token: 0x04003A71 RID: 14961
			None,
			// Token: 0x04003A72 RID: 14962
			Ascending,
			// Token: 0x04003A73 RID: 14963
			Descending
		}

		// Token: 0x0200090C RID: 2316
		public enum LeaderBoardDiaplayType
		{
			// Token: 0x04003A75 RID: 14965
			None,
			// Token: 0x04003A76 RID: 14966
			Numeric,
			// Token: 0x04003A77 RID: 14967
			TimeSeconds,
			// Token: 0x04003A78 RID: 14968
			TimeMilliSeconds
		}

		// Token: 0x0200090D RID: 2317
		public enum LeaderBoardScoreMethod
		{
			// Token: 0x04003A7A RID: 14970
			None,
			// Token: 0x04003A7B RID: 14971
			KeepBest,
			// Token: 0x04003A7C RID: 14972
			ForceUpdate
		}

		// Token: 0x0200090E RID: 2318
		public enum AchievementDisplayAttribute
		{
			// Token: 0x04003A7E RID: 14974
			Name,
			// Token: 0x04003A7F RID: 14975
			Desc,
			// Token: 0x04003A80 RID: 14976
			Hidden
		}
	}
}
