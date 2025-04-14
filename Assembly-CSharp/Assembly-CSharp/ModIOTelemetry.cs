using System;
using System.Collections;
using ModIO;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02000613 RID: 1555
public class ModIOTelemetry : MonoBehaviour
{
	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x060026C9 RID: 9929 RVA: 0x000BF237 File Offset: 0x000BD437
	public static bool IsActive
	{
		get
		{
			return ModIOTelemetry.metricsCaptureStarted || ModIOTelemetry.perfCaptureStarted;
		}
	}

	// Token: 0x060026CA RID: 9930 RVA: 0x000BF247 File Offset: 0x000BD447
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

	// Token: 0x060026CB RID: 9931 RVA: 0x000BF27B File Offset: 0x000BD47B
	private static void OnPlayerJoinedRoom(NetPlayer obj)
	{
		ModIOTelemetry.runningPlayerCount++;
		ModIOTelemetry.maxPlayersInMap = Math.Max(ModIOTelemetry.runningPlayerCount, ModIOTelemetry.maxPlayersInMap);
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x000BF29D File Offset: 0x000BD49D
	private static void OnPlayerLeftRoom(NetPlayer obj)
	{
		ModIOTelemetry.runningPlayerCount--;
		ModIOTelemetry.minPlayersInMap = Math.Min(ModIOTelemetry.runningPlayerCount, ModIOTelemetry.minPlayersInMap);
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x000BF2C0 File Offset: 0x000BD4C0
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

	// Token: 0x060026CE RID: 9934 RVA: 0x000BF344 File Offset: 0x000BD544
	public static void EndMapTracking()
	{
		ModIOTelemetry.EndMetricsCapture();
		ModIOTelemetry.EndPerfCapture();
		ModIOTelemetry.mapName = "NULL";
		ModIOTelemetry.mapCreatorUsername = "NULL";
		ModIOTelemetry.mapEnterTime = -1f;
		ModIOTelemetry.mapModId = 0L;
	}

	// Token: 0x060026CF RID: 9935 RVA: 0x000BF378 File Offset: 0x000BD578
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

	// Token: 0x060026D0 RID: 9936 RVA: 0x000BF410 File Offset: 0x000BD610
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

	// Token: 0x060026D1 RID: 9937 RVA: 0x000BF4D4 File Offset: 0x000BD6D4
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

	// Token: 0x060026D2 RID: 9938 RVA: 0x000BF56C File Offset: 0x000BD76C
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

	// Token: 0x060026D3 RID: 9939 RVA: 0x000BF667 File Offset: 0x000BD867
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

	// Token: 0x060026D4 RID: 9940 RVA: 0x000BF66F File Offset: 0x000BD86F
	private void OnDestroy()
	{
		if (this.perfCaptureCoroutine != null)
		{
			ModIOTelemetry.EndMapTracking();
		}
	}

	// Token: 0x04002A97 RID: 10903
	[OnEnterPlay_SetNull]
	private static volatile ModIOTelemetry instance;

	// Token: 0x04002A98 RID: 10904
	private static string mapName;

	// Token: 0x04002A99 RID: 10905
	private static long mapModId;

	// Token: 0x04002A9A RID: 10906
	private static string mapCreatorUsername;

	// Token: 0x04002A9B RID: 10907
	private static bool metricsCaptureStarted;

	// Token: 0x04002A9C RID: 10908
	private static float mapEnterTime;

	// Token: 0x04002A9D RID: 10909
	private static int runningPlayerCount;

	// Token: 0x04002A9E RID: 10910
	private static int minPlayersInMap;

	// Token: 0x04002A9F RID: 10911
	private static int maxPlayersInMap;

	// Token: 0x04002AA0 RID: 10912
	private static bool inPrivateRoom;

	// Token: 0x04002AA1 RID: 10913
	private const int minimumPlaytimeForTracking = 30;

	// Token: 0x04002AA2 RID: 10914
	private static int LowestFPS = int.MaxValue;

	// Token: 0x04002AA3 RID: 10915
	private static int LowestFPSDrawCalls;

	// Token: 0x04002AA4 RID: 10916
	private static int LowestFPSPlayerCount;

	// Token: 0x04002AA5 RID: 10917
	private static int AverageFPS;

	// Token: 0x04002AA6 RID: 10918
	private static int AverageDrawCalls;

	// Token: 0x04002AA7 RID: 10919
	private static int AveragePlayerCount;

	// Token: 0x04002AA8 RID: 10920
	private static int HighestFPS = int.MinValue;

	// Token: 0x04002AA9 RID: 10921
	private static int HighestFPSDrawCalls;

	// Token: 0x04002AAA RID: 10922
	private static int HighestFPSPlayerCount;

	// Token: 0x04002AAB RID: 10923
	private static int totalFPS;

	// Token: 0x04002AAC RID: 10924
	private static int totalDrawCalls;

	// Token: 0x04002AAD RID: 10925
	private static int totalPlayerCount;

	// Token: 0x04002AAE RID: 10926
	private static int frameCounter;

	// Token: 0x04002AAF RID: 10927
	private Coroutine perfCaptureCoroutine;

	// Token: 0x04002AB0 RID: 10928
	private static ProfilerRecorder drawCallsRecorder;

	// Token: 0x04002AB1 RID: 10929
	private static bool perfCaptureStarted;
}
