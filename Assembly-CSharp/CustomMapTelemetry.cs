using System;
using System.Collections;
using ModIO;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02000672 RID: 1650
public class CustomMapTelemetry : MonoBehaviour
{
	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x060028F2 RID: 10482 RVA: 0x0004BB9C File Offset: 0x00049D9C
	public static bool IsActive
	{
		get
		{
			return CustomMapTelemetry.metricsCaptureStarted || CustomMapTelemetry.perfCaptureStarted;
		}
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x0004BBAC File Offset: 0x00049DAC
	private void Awake()
	{
		if (CustomMapTelemetry.instance == null)
		{
			CustomMapTelemetry.instance = this;
			return;
		}
		if (CustomMapTelemetry.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x0004BBE0 File Offset: 0x00049DE0
	private static void OnPlayerJoinedRoom(NetPlayer obj)
	{
		CustomMapTelemetry.runningPlayerCount++;
		CustomMapTelemetry.maxPlayersInMap = Math.Max(CustomMapTelemetry.runningPlayerCount, CustomMapTelemetry.maxPlayersInMap);
	}

	// Token: 0x060028F5 RID: 10485 RVA: 0x0004BC02 File Offset: 0x00049E02
	private static void OnPlayerLeftRoom(NetPlayer obj)
	{
		CustomMapTelemetry.runningPlayerCount--;
		CustomMapTelemetry.minPlayersInMap = Math.Min(CustomMapTelemetry.runningPlayerCount, CustomMapTelemetry.minPlayersInMap);
	}

	// Token: 0x060028F6 RID: 10486 RVA: 0x00114230 File Offset: 0x00112430
	public static void StartMapTracking()
	{
		if (CustomMapTelemetry.metricsCaptureStarted || CustomMapTelemetry.perfCaptureStarted)
		{
			return;
		}
		CustomMapTelemetry.mapEnterTime = Time.unscaledTime;
		float value = UnityEngine.Random.value;
		if (value <= 0.01f)
		{
			CustomMapTelemetry.StartMetricsCapture();
		}
		else if (value >= 0.99f)
		{
			CustomMapTelemetry.StartPerfCapture();
		}
		if (CustomMapTelemetry.metricsCaptureStarted || CustomMapTelemetry.perfCaptureStarted)
		{
			ModIOManager.GetModProfile(new ModId(CustomMapLoader.LoadedMapModId), delegate(ModIORequestResultAnd<ModProfile> resultAndProfile)
			{
				if (resultAndProfile.result.success)
				{
					CustomMapTelemetry.mapName = resultAndProfile.data.name;
					CustomMapTelemetry.mapModId = resultAndProfile.data.id.id;
					CustomMapTelemetry.mapCreatorUsername = resultAndProfile.data.creator.username;
				}
			});
		}
	}

	// Token: 0x060028F7 RID: 10487 RVA: 0x0004BC24 File Offset: 0x00049E24
	public static void EndMapTracking()
	{
		CustomMapTelemetry.EndMetricsCapture();
		CustomMapTelemetry.EndPerfCapture();
		CustomMapTelemetry.mapName = "NULL";
		CustomMapTelemetry.mapCreatorUsername = "NULL";
		CustomMapTelemetry.mapEnterTime = -1f;
		CustomMapTelemetry.mapModId = 0L;
	}

	// Token: 0x060028F8 RID: 10488 RVA: 0x001142B4 File Offset: 0x001124B4
	private static void StartMetricsCapture()
	{
		if (CustomMapTelemetry.metricsCaptureStarted)
		{
			return;
		}
		CustomMapTelemetry.metricsCaptureStarted = true;
		NetworkSystem.Instance.OnPlayerJoined -= CustomMapTelemetry.OnPlayerJoinedRoom;
		NetworkSystem.Instance.OnPlayerJoined += CustomMapTelemetry.OnPlayerJoinedRoom;
		NetworkSystem.Instance.OnPlayerLeft -= CustomMapTelemetry.OnPlayerLeftRoom;
		NetworkSystem.Instance.OnPlayerLeft += CustomMapTelemetry.OnPlayerLeftRoom;
		CustomMapTelemetry.runningPlayerCount = NetworkSystem.Instance.RoomPlayerCount;
		CustomMapTelemetry.minPlayersInMap = CustomMapTelemetry.runningPlayerCount;
		CustomMapTelemetry.maxPlayersInMap = CustomMapTelemetry.runningPlayerCount;
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x0011434C File Offset: 0x0011254C
	private static void EndMetricsCapture()
	{
		if (!CustomMapTelemetry.metricsCaptureStarted)
		{
			return;
		}
		CustomMapTelemetry.metricsCaptureStarted = false;
		NetworkSystem.Instance.OnPlayerJoined -= CustomMapTelemetry.OnPlayerJoinedRoom;
		NetworkSystem.Instance.OnPlayerLeft -= CustomMapTelemetry.OnPlayerLeftRoom;
		CustomMapTelemetry.inPrivateRoom = (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.SessionIsPrivate);
		int num = Mathf.RoundToInt(Time.unscaledTime - CustomMapTelemetry.mapEnterTime);
		if (num < 30)
		{
			return;
		}
		if (CustomMapTelemetry.mapName.Equals("NULL") || CustomMapTelemetry.mapModId == 0L)
		{
			Debug.LogError("[CustomMapTelemetry::EndMetricsCapture] mapName or mapModID is invalid, throwing out this capture data...");
			return;
		}
		GorillaTelemetry.PostCustomMapTracking(CustomMapTelemetry.mapName, CustomMapTelemetry.mapModId, CustomMapTelemetry.mapCreatorUsername, CustomMapTelemetry.minPlayersInMap, CustomMapTelemetry.maxPlayersInMap, num, CustomMapTelemetry.inPrivateRoom);
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x00114410 File Offset: 0x00112610
	private static void StartPerfCapture()
	{
		if (CustomMapTelemetry.perfCaptureStarted)
		{
			return;
		}
		CustomMapTelemetry.perfCaptureStarted = true;
		if (CustomMapTelemetry.instance.perfCaptureCoroutine != null)
		{
			CustomMapTelemetry.EndPerfCapture();
		}
		CustomMapTelemetry.drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count", 1, ProfilerRecorderOptions.Default);
		CustomMapTelemetry.LowestFPS = int.MaxValue;
		CustomMapTelemetry.HighestFPS = int.MinValue;
		CustomMapTelemetry.totalFPS = 0;
		CustomMapTelemetry.totalDrawCalls = 0;
		CustomMapTelemetry.totalPlayerCount = 0;
		CustomMapTelemetry.frameCounter = 0;
		CustomMapTelemetry.instance.perfCaptureCoroutine = CustomMapTelemetry.instance.StartCoroutine(CustomMapTelemetry.instance.CaptureMapPerformance());
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x001144A8 File Offset: 0x001126A8
	private static void EndPerfCapture()
	{
		if (!CustomMapTelemetry.perfCaptureStarted)
		{
			return;
		}
		CustomMapTelemetry.perfCaptureStarted = false;
		if (CustomMapTelemetry.instance.perfCaptureCoroutine != null)
		{
			CustomMapTelemetry.instance.StopAllCoroutines();
			CustomMapTelemetry.instance.perfCaptureCoroutine = null;
		}
		CustomMapTelemetry.drawCallsRecorder.Dispose();
		if (CustomMapTelemetry.frameCounter == 0)
		{
			return;
		}
		int num = Mathf.RoundToInt(Time.unscaledTime - CustomMapTelemetry.mapEnterTime);
		CustomMapTelemetry.AverageFPS = CustomMapTelemetry.totalFPS / CustomMapTelemetry.frameCounter;
		CustomMapTelemetry.AverageDrawCalls = CustomMapTelemetry.totalDrawCalls / CustomMapTelemetry.frameCounter;
		CustomMapTelemetry.AveragePlayerCount = CustomMapTelemetry.totalPlayerCount / CustomMapTelemetry.frameCounter;
		if (num < 30)
		{
			return;
		}
		if (CustomMapTelemetry.mapName.Equals("NULL") || CustomMapTelemetry.mapModId == 0L)
		{
			Debug.LogError("[CustomMapTelemetry::EndPerfCapture] mapName or mapModID is invalid, throwing out this capture data...");
			return;
		}
		GorillaTelemetry.PostCustomMapPerformance(CustomMapTelemetry.mapName, CustomMapTelemetry.mapModId, CustomMapTelemetry.LowestFPS, CustomMapTelemetry.LowestFPSDrawCalls, CustomMapTelemetry.LowestFPSPlayerCount, CustomMapTelemetry.AverageFPS, CustomMapTelemetry.AverageDrawCalls, CustomMapTelemetry.AveragePlayerCount, CustomMapTelemetry.HighestFPS, CustomMapTelemetry.HighestFPSDrawCalls, CustomMapTelemetry.HighestFPSPlayerCount, num);
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x0004BC55 File Offset: 0x00049E55
	private IEnumerator CaptureMapPerformance()
	{
		for (;;)
		{
			int num = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
			int num2 = Mathf.RoundToInt((float)CustomMapTelemetry.drawCallsRecorder.LastValue);
			int roomPlayerCount = NetworkSystem.Instance.RoomPlayerCount;
			CustomMapTelemetry.totalFPS += num;
			CustomMapTelemetry.totalDrawCalls += num2;
			CustomMapTelemetry.totalPlayerCount += roomPlayerCount;
			if (num > CustomMapTelemetry.HighestFPS)
			{
				CustomMapTelemetry.HighestFPS = num;
				CustomMapTelemetry.HighestFPSDrawCalls = num2;
				CustomMapTelemetry.HighestFPSPlayerCount = roomPlayerCount;
			}
			if (num < CustomMapTelemetry.LowestFPS)
			{
				CustomMapTelemetry.LowestFPS = num;
				CustomMapTelemetry.LowestFPSDrawCalls = num2;
				CustomMapTelemetry.LowestFPSPlayerCount = roomPlayerCount;
			}
			CustomMapTelemetry.frameCounter++;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060028FD RID: 10493 RVA: 0x0004BC5D File Offset: 0x00049E5D
	private void OnDestroy()
	{
		if (this.perfCaptureCoroutine != null)
		{
			CustomMapTelemetry.EndMapTracking();
		}
	}

	// Token: 0x04002E49 RID: 11849
	[OnEnterPlay_SetNull]
	private static volatile CustomMapTelemetry instance;

	// Token: 0x04002E4A RID: 11850
	private static string mapName;

	// Token: 0x04002E4B RID: 11851
	private static long mapModId;

	// Token: 0x04002E4C RID: 11852
	private static string mapCreatorUsername;

	// Token: 0x04002E4D RID: 11853
	private static bool metricsCaptureStarted;

	// Token: 0x04002E4E RID: 11854
	private static float mapEnterTime;

	// Token: 0x04002E4F RID: 11855
	private static int runningPlayerCount;

	// Token: 0x04002E50 RID: 11856
	private static int minPlayersInMap;

	// Token: 0x04002E51 RID: 11857
	private static int maxPlayersInMap;

	// Token: 0x04002E52 RID: 11858
	private static bool inPrivateRoom;

	// Token: 0x04002E53 RID: 11859
	private const int minimumPlaytimeForTracking = 30;

	// Token: 0x04002E54 RID: 11860
	private static int LowestFPS = int.MaxValue;

	// Token: 0x04002E55 RID: 11861
	private static int LowestFPSDrawCalls;

	// Token: 0x04002E56 RID: 11862
	private static int LowestFPSPlayerCount;

	// Token: 0x04002E57 RID: 11863
	private static int AverageFPS;

	// Token: 0x04002E58 RID: 11864
	private static int AverageDrawCalls;

	// Token: 0x04002E59 RID: 11865
	private static int AveragePlayerCount;

	// Token: 0x04002E5A RID: 11866
	private static int HighestFPS = int.MinValue;

	// Token: 0x04002E5B RID: 11867
	private static int HighestFPSDrawCalls;

	// Token: 0x04002E5C RID: 11868
	private static int HighestFPSPlayerCount;

	// Token: 0x04002E5D RID: 11869
	private static int totalFPS;

	// Token: 0x04002E5E RID: 11870
	private static int totalDrawCalls;

	// Token: 0x04002E5F RID: 11871
	private static int totalPlayerCount;

	// Token: 0x04002E60 RID: 11872
	private static int frameCounter;

	// Token: 0x04002E61 RID: 11873
	private Coroutine perfCaptureCoroutine;

	// Token: 0x04002E62 RID: 11874
	private static ProfilerRecorder drawCallsRecorder;

	// Token: 0x04002E63 RID: 11875
	private static bool perfCaptureStarted;
}
