using System;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x02000925 RID: 2341
	public class UserStats
	{
		// Token: 0x0600384D RID: 14413 RVA: 0x00055259 File Offset: 0x00053459
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsReadyIl2cppCallback(int errorCode)
		{
			UserStats.isReadyIl2cppCallback(errorCode);
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x0014A6CC File Offset: 0x001488CC
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

		// Token: 0x0600384F RID: 14415 RVA: 0x00055266 File Offset: 0x00053466
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void DownloadStatsIl2cppCallback(int errorCode)
		{
			UserStats.downloadStatsIl2cppCallback(errorCode);
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x0014A73C File Offset: 0x0014893C
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

		// Token: 0x06003851 RID: 14417 RVA: 0x0014A7AC File Offset: 0x001489AC
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

		// Token: 0x06003852 RID: 14418 RVA: 0x0014A7D8 File Offset: 0x001489D8
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

		// Token: 0x06003853 RID: 14419 RVA: 0x00055273 File Offset: 0x00053473
		public static void SetStat(string name, int value)
		{
			if (IntPtr.Size == 8)
			{
				UserStats.SetStat_64(name, value);
				return;
			}
			UserStats.SetStat(name, value);
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x0005528E File Offset: 0x0005348E
		public static void SetStat(string name, float value)
		{
			if (IntPtr.Size == 8)
			{
				UserStats.SetStat_64(name, value);
				return;
			}
			UserStats.SetStat(name, value);
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x000552A9 File Offset: 0x000534A9
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void UploadStatsIl2cppCallback(int errorCode)
		{
			UserStats.uploadStatsIl2cppCallback(errorCode);
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x0014A804 File Offset: 0x00148A04
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

		// Token: 0x06003857 RID: 14423 RVA: 0x0014A874 File Offset: 0x00148A74
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

		// Token: 0x06003858 RID: 14424 RVA: 0x0014A8A4 File Offset: 0x00148AA4
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

		// Token: 0x06003859 RID: 14425 RVA: 0x000552B6 File Offset: 0x000534B6
		public static string GetAchievementIcon(string pchName)
		{
			return "";
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x000552B6 File Offset: 0x000534B6
		public static string GetAchievementDisplayAttribute(string pchName, UserStats.AchievementDisplayAttribute attr)
		{
			return "";
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x000552B6 File Offset: 0x000534B6
		public static string GetAchievementDisplayAttribute(string pchName, UserStats.AchievementDisplayAttribute attr, Locale locale)
		{
			return "";
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x000552BD File Offset: 0x000534BD
		public static int SetAchievement(string pchName)
		{
			if (IntPtr.Size == 8)
			{
				return UserStats.SetAchievement_64(pchName);
			}
			return UserStats.SetAchievement(pchName);
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x000552D4 File Offset: 0x000534D4
		public static int ClearAchievement(string pchName)
		{
			if (IntPtr.Size == 8)
			{
				return UserStats.ClearAchievement_64(pchName);
			}
			return UserStats.ClearAchievement(pchName);
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x000552EB File Offset: 0x000534EB
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void DownloadLeaderboardScoresIl2cppCallback(int errorCode)
		{
			UserStats.downloadLeaderboardScoresIl2cppCallback(errorCode);
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x0014A8D0 File Offset: 0x00148AD0
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

		// Token: 0x06003860 RID: 14432 RVA: 0x000552F8 File Offset: 0x000534F8
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void UploadLeaderboardScoreIl2cppCallback(int errorCode)
		{
			UserStats.uploadLeaderboardScoreIl2cppCallback(errorCode);
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x0014A94C File Offset: 0x00148B4C
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

		// Token: 0x06003862 RID: 14434 RVA: 0x0014A9C0 File Offset: 0x00148BC0
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

		// Token: 0x06003863 RID: 14435 RVA: 0x00055305 File Offset: 0x00053505
		public static int GetLeaderboardScoreCount()
		{
			if (IntPtr.Size == 8)
			{
				return UserStats.GetLeaderboardScoreCount_64();
			}
			return UserStats.GetLeaderboardScoreCount();
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x0005531A File Offset: 0x0005351A
		public static UserStats.LeaderBoardSortMethod GetLeaderboardSortMethod()
		{
			if (IntPtr.Size == 8)
			{
				return (UserStats.LeaderBoardSortMethod)UserStats.GetLeaderboardSortMethod_64();
			}
			return (UserStats.LeaderBoardSortMethod)UserStats.GetLeaderboardSortMethod();
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x0005532F File Offset: 0x0005352F
		public static UserStats.LeaderBoardDiaplayType GetLeaderboardDisplayType()
		{
			if (IntPtr.Size == 8)
			{
				return (UserStats.LeaderBoardDiaplayType)UserStats.GetLeaderboardDisplayType_64();
			}
			return (UserStats.LeaderBoardDiaplayType)UserStats.GetLeaderboardDisplayType();
		}

		// Token: 0x04003B26 RID: 15142
		private static StatusCallback isReadyIl2cppCallback;

		// Token: 0x04003B27 RID: 15143
		private static StatusCallback downloadStatsIl2cppCallback;

		// Token: 0x04003B28 RID: 15144
		private static StatusCallback uploadStatsIl2cppCallback;

		// Token: 0x04003B29 RID: 15145
		private static StatusCallback downloadLeaderboardScoresIl2cppCallback;

		// Token: 0x04003B2A RID: 15146
		private static StatusCallback uploadLeaderboardScoreIl2cppCallback;

		// Token: 0x02000926 RID: 2342
		public enum LeaderBoardRequestType
		{
			// Token: 0x04003B2C RID: 15148
			GlobalData,
			// Token: 0x04003B2D RID: 15149
			GlobalDataAroundUser,
			// Token: 0x04003B2E RID: 15150
			LocalData,
			// Token: 0x04003B2F RID: 15151
			LocalDataAroundUser
		}

		// Token: 0x02000927 RID: 2343
		public enum LeaderBoardTimeRange
		{
			// Token: 0x04003B31 RID: 15153
			AllTime,
			// Token: 0x04003B32 RID: 15154
			Daily,
			// Token: 0x04003B33 RID: 15155
			Weekly,
			// Token: 0x04003B34 RID: 15156
			Monthly
		}

		// Token: 0x02000928 RID: 2344
		public enum LeaderBoardSortMethod
		{
			// Token: 0x04003B36 RID: 15158
			None,
			// Token: 0x04003B37 RID: 15159
			Ascending,
			// Token: 0x04003B38 RID: 15160
			Descending
		}

		// Token: 0x02000929 RID: 2345
		public enum LeaderBoardDiaplayType
		{
			// Token: 0x04003B3A RID: 15162
			None,
			// Token: 0x04003B3B RID: 15163
			Numeric,
			// Token: 0x04003B3C RID: 15164
			TimeSeconds,
			// Token: 0x04003B3D RID: 15165
			TimeMilliSeconds
		}

		// Token: 0x0200092A RID: 2346
		public enum LeaderBoardScoreMethod
		{
			// Token: 0x04003B3F RID: 15167
			None,
			// Token: 0x04003B40 RID: 15168
			KeepBest,
			// Token: 0x04003B41 RID: 15169
			ForceUpdate
		}

		// Token: 0x0200092B RID: 2347
		public enum AchievementDisplayAttribute
		{
			// Token: 0x04003B43 RID: 15171
			Name,
			// Token: 0x04003B44 RID: 15172
			Desc,
			// Token: 0x04003B45 RID: 15173
			Hidden
		}
	}
}
