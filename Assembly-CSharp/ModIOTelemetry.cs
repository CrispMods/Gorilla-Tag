using System;
using System.Collections;
using ModIO;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02000612 RID: 1554
public class ModIOTelemetry : MonoBehaviour
{
	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x060026C1 RID: 9921 RVA: 0x000BEDB7 File Offset: 0x000BCFB7
	public static bool IsActive
	{
		get
		{
			return ModIOTelemetry.metricsCaptureStarted || ModIOTelemetry.perfCaptureStarted;
		}
	}

	// Token: 0x060026C2 RID: 9922 RVA: 0x000BEDC7 File Offset: 0x000BCFC7
	private void Awake()
	{
		if (ModIOTelemetry.instance == null)
		{
			ModIOTelemetry.instance = this;
			return;
		}
		if (ModIOTelemetry.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060026C3 RID: 9923 RVA: 0x000BEDFB File Offset: 0x000BCFFB
	private static void OnPlayerJoinedRoom(NetPlayer obj)
	{
		ModIOTelemetry.runningPlayerCount++;
		ModIOTelemetry.maxPlayersInMap = Math.Max(ModIOTelemetry.runningPlayerCount, ModIOTelemetry.maxPlayersInMap);
	}

	// Token: 0x060026C4 RID: 9924 RVA: 0x000BEE1D File Offset: 0x000BD01D
	private static void OnPlayerLeftRoom(NetPlayer obj)
	{
		ModIOTelemetry.runningPlayerCount--;
		ModIOTelemetry.minPlayersInMap = Math.Min(ModIOTelemetry.runningPlayerCount, ModIOTelemetry.minPlayersInMap);
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x000BEE40 File Offset: 0x000BD040
	public static void StartMapTracking()
	{
		if (ModIOTelemetry.metricsCaptureStarted || ModIOTelemetry.perfCaptureStarted)
		{
			return;
		}
		ModIOTelemetry.mapEnterTime = Time.unscaledTime;
		float value = Random.value;
		if (value <= 0.01f)
		{
			ModIOTelemetry.StartMetricsCapture();
		}
		else if (value >= 0.99f)
		{
			ModIOTelemetry.StartPerfCapture();
		}
		if (ModIOTelemetry.metricsCaptureStarted || ModIOTelemetry.perfCaptureStarted)
		{
			ModIODataStore.GetModProfile(new ModId(ModIOMapLoader.LoadedMapModId), delegate(ModIORequestResultAnd<ModProfile> resultAndProfile)
			{
				if (resultAndProfile.result.success)
				{
					ModIOTelemetry.mapName = resultAndProfile.data.name;
					ModIOTelemetry.mapModId = resultAndProfile.data.id.id;
					ModIOTelemetry.mapCreatorUsername = resultAndProfile.data.creator.username;
				}
			});
		}
	}

	// Token: 0x060026C6 RID: 9926 RVA: 0x000BEEC4 File Offset: 0x000BD0C4
	public static void EndMapTracking()
	{
		ModIOTelemetry.EndMetricsCapture();
		ModIOTelemetry.EndPerfCapture();
		ModIOTelemetry.mapName = "NULL";
		ModIOTelemetry.mapCreatorUsername = "NULL";
		ModIOTelemetry.mapEnterTime = -1f;
		ModIOTelemetry.mapModId = 0L;
	}

	// Token: 0x060026C7 RID: 9927 RVA: 0x000BEEF8 File Offset: 0x000BD0F8
	private static void StartMetricsCapture()
	{
		if (ModIOTelemetry.metricsCaptureStarted)
		{
			return;
		}
		ModIOTelemetry.metricsCaptureStarted = true;
		NetworkSystem.Instance.OnPlayerJoined -= ModIOTelemetry.OnPlayerJoinedRoom;
		NetworkSystem.Instance.OnPlayerJoined += ModIOTelemetry.OnPlayerJoinedRoom;
		NetworkSystem.Instance.OnPlayerLeft -= ModIOTelemetry.OnPlayerLeftRoom;
		NetworkSystem.Instance.OnPlayerLeft += ModIOTelemetry.OnPlayerLeftRoom;
		ModIOTelemetry.runningPlayerCount = NetworkSystem.Instance.RoomPlayerCount;
		ModIOTelemetry.minPlayersInMap = ModIOTelemetry.runningPlayerCount;
		ModIOTelemetry.maxPlayersInMap = ModIOTelemetry.runningPlayerCount;
	}

	// Token: 0x060026C8 RID: 9928 RVA: 0x000BEF90 File Offset: 0x000BD190
	private static void EndMetricsCapture()
	{
		if (!ModIOTelemetry.metricsCaptureStarted)
		{
			return;
		}
		ModIOTelemetry.metricsCaptureStarted = false;
		NetworkSystem.Instance.OnPlayerJoined -= ModIOTelemetry.OnPlayerJoinedRoom;
		NetworkSystem.Instance.OnPlayerLeft -= ModIOTelemetry.OnPlayerLeftRoom;
		ModIOTelemetry.inPrivateRoom = (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.SessionIsPrivate);
		int num = Mathf.RoundToInt(Time.unscaledTime - ModIOTelemetry.mapEnterTime);
		if (num < 30)
		{
			return;
		}
		if (ModIOTelemetry.mapName.Equals("NULL") || ModIOTelemetry.mapModId == 0L)
		{
			Debug.LogError("[ModIOTelemetry::EndMetricsCapture] mapName or mapModID is invalid, throwing out this capture data...");
			return;
		}
		GorillaTelemetry.PostCustomMapTracking(ModIOTelemetry.mapName, ModIOTelemetry.mapModId, ModIOTelemetry.mapCreatorUsername, ModIOTelemetry.minPlayersInMap, ModIOTelemetry.maxPlayersInMap, num, ModIOTelemetry.inPrivateRoom);
	}

	// Token: 0x060026C9 RID: 9929 RVA: 0x000BF054 File Offset: 0x000BD254
	private static void StartPerfCapture()
	{
		if (ModIOTelemetry.perfCaptureStarted)
		{
			return;
		}
		ModIOTelemetry.perfCaptureStarted = true;
		if (ModIOTelemetry.instance.perfCaptureCoroutine != null)
		{
			ModIOTelemetry.EndPerfCapture();
		}
		ModIOTelemetry.drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count", 1, ProfilerRecorderOptions.Default);
		ModIOTelemetry.LowestFPS = int.MaxValue;
		ModIOTelemetry.HighestFPS = int.MinValue;
		ModIOTelemetry.totalFPS = 0;
		ModIOTelemetry.totalDrawCalls = 0;
		ModIOTelemetry.totalPlayerCount = 0;
		ModIOTelemetry.frameCounter = 0;
		ModIOTelemetry.instance.perfCaptureCoroutine = ModIOTelemetry.instance.StartCoroutine(ModIOTelemetry.instance.CaptureMapPerformance());
	}

	// Token: 0x060026CA RID: 9930 RVA: 0x000BF0EC File Offset: 0x000BD2EC
	private static void EndPerfCapture()
	{
		if (!ModIOTelemetry.perfCaptureStarted)
		{
			return;
		}
		ModIOTelemetry.perfCaptureStarted = false;
		if (ModIOTelemetry.instance.perfCaptureCoroutine != null)
		{
			ModIOTelemetry.instance.StopAllCoroutines();
			ModIOTelemetry.instance.perfCaptureCoroutine = null;
		}
		ModIOTelemetry.drawCallsRecorder.Dispose();
		if (ModIOTelemetry.frameCounter == 0)
		{
			return;
		}
		int num = Mathf.RoundToInt(Time.unscaledTime - ModIOTelemetry.mapEnterTime);
		ModIOTelemetry.AverageFPS = ModIOTelemetry.totalFPS / ModIOTelemetry.frameCounter;
		ModIOTelemetry.AverageDrawCalls = ModIOTelemetry.totalDrawCalls / ModIOTelemetry.frameCounter;
		ModIOTelemetry.AveragePlayerCount = ModIOTelemetry.totalPlayerCount / ModIOTelemetry.frameCounter;
		if (num < 30)
		{
			return;
		}
		if (ModIOTelemetry.mapName.Equals("NULL") || ModIOTelemetry.mapModId == 0L)
		{
			Debug.LogError("[ModIOTelemetry::EndPerfCapture] mapName or mapModID is invalid, throwing out this capture data...");
			return;
		}
		GorillaTelemetry.PostCustomMapPerformance(ModIOTelemetry.mapName, ModIOTelemetry.mapModId, ModIOTelemetry.LowestFPS, ModIOTelemetry.LowestFPSDrawCalls, ModIOTelemetry.LowestFPSPlayerCount, ModIOTelemetry.AverageFPS, ModIOTelemetry.AverageDrawCalls, ModIOTelemetry.AveragePlayerCount, ModIOTelemetry.HighestFPS, ModIOTelemetry.HighestFPSDrawCalls, ModIOTelemetry.HighestFPSPlayerCount, num);
	}

	// Token: 0x060026CB RID: 9931 RVA: 0x000BF1E7 File Offset: 0x000BD3E7
	private IEnumerator CaptureMapPerformance()
	{
		for (;;)
		{
			int num = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
			int num2 = Mathf.RoundToInt((float)ModIOTelemetry.drawCallsRecorder.LastValue);
			int roomPlayerCount = NetworkSystem.Instance.RoomPlayerCount;
			ModIOTelemetry.totalFPS += num;
			ModIOTelemetry.totalDrawCalls += num2;
			ModIOTelemetry.totalPlayerCount += roomPlayerCount;
			if (num > ModIOTelemetry.HighestFPS)
			{
				ModIOTelemetry.HighestFPS = num;
				ModIOTelemetry.HighestFPSDrawCalls = num2;
				ModIOTelemetry.HighestFPSPlayerCount = roomPlayerCount;
			}
			if (num < ModIOTelemetry.LowestFPS)
			{
				ModIOTelemetry.LowestFPS = num;
				ModIOTelemetry.LowestFPSDrawCalls = num2;
				ModIOTelemetry.LowestFPSPlayerCount = roomPlayerCount;
			}
			ModIOTelemetry.frameCounter++;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x000BF1EF File Offset: 0x000BD3EF
	private void OnDestroy()
	{
		if (this.perfCaptureCoroutine != null)
		{
			ModIOTelemetry.EndMapTracking();
		}
	}

	// Token: 0x04002A91 RID: 10897
	[OnEnterPlay_SetNull]
	private static volatile ModIOTelemetry instance;

	// Token: 0x04002A92 RID: 10898
	private static string mapName;

	// Token: 0x04002A93 RID: 10899
	private static long mapModId;

	// Token: 0x04002A94 RID: 10900
	private static string mapCreatorUsername;

	// Token: 0x04002A95 RID: 10901
	private static bool metricsCaptureStarted;

	// Token: 0x04002A96 RID: 10902
	private static float mapEnterTime;

	// Token: 0x04002A97 RID: 10903
	private static int runningPlayerCount;

	// Token: 0x04002A98 RID: 10904
	private static int minPlayersInMap;

	// Token: 0x04002A99 RID: 10905
	private static int maxPlayersInMap;

	// Token: 0x04002A9A RID: 10906
	private static bool inPrivateRoom;

	// Token: 0x04002A9B RID: 10907
	private const int minimumPlaytimeForTracking = 30;

	// Token: 0x04002A9C RID: 10908
	private static int LowestFPS = int.MaxValue;

	// Token: 0x04002A9D RID: 10909
	private static int LowestFPSDrawCalls;

	// Token: 0x04002A9E RID: 10910
	private static int LowestFPSPlayerCount;

	// Token: 0x04002A9F RID: 10911
	private static int AverageFPS;

	// Token: 0x04002AA0 RID: 10912
	private static int AverageDrawCalls;

	// Token: 0x04002AA1 RID: 10913
	private static int AveragePlayerCount;

	// Token: 0x04002AA2 RID: 10914
	private static int HighestFPS = int.MinValue;

	// Token: 0x04002AA3 RID: 10915
	private static int HighestFPSDrawCalls;

	// Token: 0x04002AA4 RID: 10916
	private static int HighestFPSPlayerCount;

	// Token: 0x04002AA5 RID: 10917
	private static int totalFPS;

	// Token: 0x04002AA6 RID: 10918
	private static int totalDrawCalls;

	// Token: 0x04002AA7 RID: 10919
	private static int totalPlayerCount;

	// Token: 0x04002AA8 RID: 10920
	private static int frameCounter;

	// Token: 0x04002AA9 RID: 10921
	private Coroutine perfCaptureCoroutine;

	// Token: 0x04002AAA RID: 10922
	private static ProfilerRecorder drawCallsRecorder;

	// Token: 0x04002AAB RID: 10923
	private static bool perfCaptureStarted;
}
