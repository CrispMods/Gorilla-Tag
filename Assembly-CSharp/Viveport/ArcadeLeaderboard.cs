using System;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x0200090F RID: 2319
	public class ArcadeLeaderboard
	{
		// Token: 0x06003796 RID: 14230 RVA: 0x00105F41 File Offset: 0x00104141
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsReadyIl2cppCallback(int errorCode)
		{
			ArcadeLeaderboard.isReadyIl2cppCallback(errorCode);
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x00105F50 File Offset: 0x00104150
		public static void IsReady(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			ArcadeLeaderboard.isReadyIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(ArcadeLeaderboard.IsReadyIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				ArcadeLeaderboard.IsReady_64(new StatusCallback(ArcadeLeaderboard.IsReadyIl2cppCallback));
				return;
			}
			ArcadeLeaderboard.IsReady(new StatusCallback(ArcadeLeaderboard.IsReadyIl2cppCallback));
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x00105FBD File Offset: 0x001041BD
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void DownloadLeaderboardScoresIl2cppCallback(int errorCode)
		{
			ArcadeLeaderboard.downloadLeaderboardScoresIl2cppCallback(errorCode);
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x00105FCC File Offset: 0x001041CC
		public static void DownloadLeaderboardScores(StatusCallback callback, string pchLeaderboardName, ArcadeLeaderboard.LeaderboardTimeRange eLeaderboardDataTimeRange, int nCount)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			ArcadeLeaderboard.downloadLeaderboardScoresIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(ArcadeLeaderboard.DownloadLeaderboardScoresIl2cppCallback));
			eLeaderboardDataTimeRange = ArcadeLeaderboard.LeaderboardTimeRange.AllTime;
			if (IntPtr.Size == 8)
			{
				ArcadeLeaderboard.DownloadLeaderboardScores_64(new StatusCallback(ArcadeLeaderboard.DownloadLeaderboardScoresIl2cppCallback), pchLeaderboardName, (ELeaderboardDataTimeRange)eLeaderboardDataTimeRange, nCount);
				return;
			}
			ArcadeLeaderboard.DownloadLeaderboardScores(new StatusCallback(ArcadeLeaderboard.DownloadLeaderboardScoresIl2cppCallback), pchLeaderboardName, (ELeaderboardDataTimeRange)eLeaderboardDataTimeRange, nCount);
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x00106042 File Offset: 0x00104242
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void UploadLeaderboardScoreIl2cppCallback(int errorCode)
		{
			ArcadeLeaderboard.uploadLeaderboardScoreIl2cppCallback(errorCode);
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x00106050 File Offset: 0x00104250
		public static void UploadLeaderboardScore(StatusCallback callback, string pchLeaderboardName, string pchUserName, int nScore)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			ArcadeLeaderboard.uploadLeaderboardScoreIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(ArcadeLeaderboard.UploadLeaderboardScoreIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				ArcadeLeaderboard.UploadLeaderboardScore_64(new StatusCallback(ArcadeLeaderboard.UploadLeaderboardScoreIl2cppCallback), pchLeaderboardName, pchUserName, nScore);
				return;
			}
			ArcadeLeaderboard.UploadLeaderboardScore(new StatusCallback(ArcadeLeaderboard.UploadLeaderboardScoreIl2cppCallback), pchLeaderboardName, pchUserName, nScore);
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x001060C4 File Offset: 0x001042C4
		public static Leaderboard GetLeaderboardScore(int index)
		{
			LeaderboardEntry_t leaderboardEntry_t;
			leaderboardEntry_t.m_nGlobalRank = 0;
			leaderboardEntry_t.m_nScore = 0;
			leaderboardEntry_t.m_pUserName = "";
			if (IntPtr.Size == 8)
			{
				ArcadeLeaderboard.GetLeaderboardScore_64(index, ref leaderboardEntry_t);
			}
			else
			{
				ArcadeLeaderboard.GetLeaderboardScore(index, ref leaderboardEntry_t);
			}
			return new Leaderboard
			{
				Rank = leaderboardEntry_t.m_nGlobalRank,
				Score = leaderboardEntry_t.m_nScore,
				UserName = leaderboardEntry_t.m_pUserName
			};
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x00106130 File Offset: 0x00104330
		public static int GetLeaderboardScoreCount()
		{
			if (IntPtr.Size == 8)
			{
				return ArcadeLeaderboard.GetLeaderboardScoreCount_64();
			}
			return ArcadeLeaderboard.GetLeaderboardScoreCount();
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x00106145 File Offset: 0x00104345
		public static int GetLeaderboardUserRank()
		{
			if (IntPtr.Size == 8)
			{
				return ArcadeLeaderboard.GetLeaderboardUserRank_64();
			}
			return ArcadeLeaderboard.GetLeaderboardUserRank();
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x0010615A File Offset: 0x0010435A
		public static int GetLeaderboardUserScore()
		{
			if (IntPtr.Size == 8)
			{
				return ArcadeLeaderboard.GetLeaderboardUserScore_64();
			}
			return ArcadeLeaderboard.GetLeaderboardUserScore();
		}

		// Token: 0x04003A81 RID: 14977
		private static StatusCallback isReadyIl2cppCallback;

		// Token: 0x04003A82 RID: 14978
		private static StatusCallback downloadLeaderboardScoresIl2cppCallback;

		// Token: 0x04003A83 RID: 14979
		private static StatusCallback uploadLeaderboardScoreIl2cppCallback;

		// Token: 0x02000910 RID: 2320
		public enum LeaderboardTimeRange
		{
			// Token: 0x04003A85 RID: 14981
			AllTime
		}
	}
}
