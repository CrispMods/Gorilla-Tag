using System;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x0200092C RID: 2348
	public class ArcadeLeaderboard
	{
		// Token: 0x06003867 RID: 14439 RVA: 0x00055344 File Offset: 0x00053544
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsReadyIl2cppCallback(int errorCode)
		{
			ArcadeLeaderboard.isReadyIl2cppCallback(errorCode);
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x0014AA30 File Offset: 0x00148C30
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

		// Token: 0x06003869 RID: 14441 RVA: 0x00055351 File Offset: 0x00053551
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void DownloadLeaderboardScoresIl2cppCallback(int errorCode)
		{
			ArcadeLeaderboard.downloadLeaderboardScoresIl2cppCallback(errorCode);
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x0014AAA0 File Offset: 0x00148CA0
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

		// Token: 0x0600386B RID: 14443 RVA: 0x0005535E File Offset: 0x0005355E
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void UploadLeaderboardScoreIl2cppCallback(int errorCode)
		{
			ArcadeLeaderboard.uploadLeaderboardScoreIl2cppCallback(errorCode);
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x0014AB18 File Offset: 0x00148D18
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

		// Token: 0x0600386D RID: 14445 RVA: 0x0014AB8C File Offset: 0x00148D8C
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

		// Token: 0x0600386E RID: 14446 RVA: 0x0005536B File Offset: 0x0005356B
		public static int GetLeaderboardScoreCount()
		{
			if (IntPtr.Size == 8)
			{
				return ArcadeLeaderboard.GetLeaderboardScoreCount_64();
			}
			return ArcadeLeaderboard.GetLeaderboardScoreCount();
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x00055380 File Offset: 0x00053580
		public static int GetLeaderboardUserRank()
		{
			if (IntPtr.Size == 8)
			{
				return ArcadeLeaderboard.GetLeaderboardUserRank_64();
			}
			return ArcadeLeaderboard.GetLeaderboardUserRank();
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x00055395 File Offset: 0x00053595
		public static int GetLeaderboardUserScore()
		{
			if (IntPtr.Size == 8)
			{
				return ArcadeLeaderboard.GetLeaderboardUserScore_64();
			}
			return ArcadeLeaderboard.GetLeaderboardUserScore();
		}

		// Token: 0x04003B46 RID: 15174
		private static StatusCallback isReadyIl2cppCallback;

		// Token: 0x04003B47 RID: 15175
		private static StatusCallback downloadLeaderboardScoresIl2cppCallback;

		// Token: 0x04003B48 RID: 15176
		private static StatusCallback uploadLeaderboardScoreIl2cppCallback;

		// Token: 0x0200092D RID: 2349
		public enum LeaderboardTimeRange
		{
			// Token: 0x04003B4A RID: 15178
			AllTime
		}
	}
}
