using System;

// Token: 0x0200011C RID: 284
public class PlayerGameEvents
{
	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06000797 RID: 1943 RVA: 0x0008B260 File Offset: 0x00089460
	// (remove) Token: 0x06000798 RID: 1944 RVA: 0x0008B294 File Offset: 0x00089494
	public static event Action<string> OnGameModeObjectiveTrigger;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06000799 RID: 1945 RVA: 0x0008B2C8 File Offset: 0x000894C8
	// (remove) Token: 0x0600079A RID: 1946 RVA: 0x0008B2FC File Offset: 0x000894FC
	public static event Action<string> OnGameModeCompleteRound;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x0600079B RID: 1947 RVA: 0x0008B330 File Offset: 0x00089530
	// (remove) Token: 0x0600079C RID: 1948 RVA: 0x0008B364 File Offset: 0x00089564
	public static event Action<string> OnGrabbedObject;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x0600079D RID: 1949 RVA: 0x0008B398 File Offset: 0x00089598
	// (remove) Token: 0x0600079E RID: 1950 RVA: 0x0008B3CC File Offset: 0x000895CC
	public static event Action<string> OnDroppedObject;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x0600079F RID: 1951 RVA: 0x0008B400 File Offset: 0x00089600
	// (remove) Token: 0x060007A0 RID: 1952 RVA: 0x0008B434 File Offset: 0x00089634
	public static event Action<string> OnEatObject;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x060007A1 RID: 1953 RVA: 0x0008B468 File Offset: 0x00089668
	// (remove) Token: 0x060007A2 RID: 1954 RVA: 0x0008B49C File Offset: 0x0008969C
	public static event Action<string> OnTapObject;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x060007A3 RID: 1955 RVA: 0x0008B4D0 File Offset: 0x000896D0
	// (remove) Token: 0x060007A4 RID: 1956 RVA: 0x0008B504 File Offset: 0x00089704
	public static event Action<string> OnLaunchedProjectile;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x060007A5 RID: 1957 RVA: 0x0008B538 File Offset: 0x00089738
	// (remove) Token: 0x060007A6 RID: 1958 RVA: 0x0008B56C File Offset: 0x0008976C
	public static event Action<float, float> OnPlayerMoved;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x060007A7 RID: 1959 RVA: 0x0008B5A0 File Offset: 0x000897A0
	// (remove) Token: 0x060007A8 RID: 1960 RVA: 0x0008B5D4 File Offset: 0x000897D4
	public static event Action<float, float> OnPlayerSwam;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x060007A9 RID: 1961 RVA: 0x0008B608 File Offset: 0x00089808
	// (remove) Token: 0x060007AA RID: 1962 RVA: 0x0008B63C File Offset: 0x0008983C
	public static event Action<string> OnTriggerHandEffect;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x060007AB RID: 1963 RVA: 0x0008B670 File Offset: 0x00089870
	// (remove) Token: 0x060007AC RID: 1964 RVA: 0x0008B6A4 File Offset: 0x000898A4
	public static event Action<string> OnEnterLocation;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x060007AD RID: 1965 RVA: 0x0008B6D8 File Offset: 0x000898D8
	// (remove) Token: 0x060007AE RID: 1966 RVA: 0x0008B70C File Offset: 0x0008990C
	public static event Action<string> OnMiscEvent;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060007AF RID: 1967 RVA: 0x0008B740 File Offset: 0x00089940
	// (remove) Token: 0x060007B0 RID: 1968 RVA: 0x0008B774 File Offset: 0x00089974
	public static event Action<string> OnCritterEvent;

	// Token: 0x060007B1 RID: 1969 RVA: 0x0008B7A8 File Offset: 0x000899A8
	public static void GameModeObjectiveTriggered()
	{
		string obj = GorillaGameManager.instance.GameModeName();
		Action<string> onGameModeObjectiveTrigger = PlayerGameEvents.OnGameModeObjectiveTrigger;
		if (onGameModeObjectiveTrigger == null)
		{
			return;
		}
		onGameModeObjectiveTrigger(obj);
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x0008B7D0 File Offset: 0x000899D0
	public static void GameModeCompleteRound()
	{
		string obj = GorillaGameManager.instance.GameModeName();
		Action<string> onGameModeCompleteRound = PlayerGameEvents.OnGameModeCompleteRound;
		if (onGameModeCompleteRound == null)
		{
			return;
		}
		onGameModeCompleteRound(obj);
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x000356C1 File Offset: 0x000338C1
	public static void GrabbedObject(string objectName)
	{
		Action<string> onGrabbedObject = PlayerGameEvents.OnGrabbedObject;
		if (onGrabbedObject == null)
		{
			return;
		}
		onGrabbedObject(objectName);
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x000356D3 File Offset: 0x000338D3
	public static void DroppedObject(string objectName)
	{
		Action<string> onDroppedObject = PlayerGameEvents.OnDroppedObject;
		if (onDroppedObject == null)
		{
			return;
		}
		onDroppedObject(objectName);
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x000356E5 File Offset: 0x000338E5
	public static void EatObject(string objectName)
	{
		Action<string> onEatObject = PlayerGameEvents.OnEatObject;
		if (onEatObject == null)
		{
			return;
		}
		onEatObject(objectName);
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x000356F7 File Offset: 0x000338F7
	public static void TapObject(string objectName)
	{
		Action<string> onTapObject = PlayerGameEvents.OnTapObject;
		if (onTapObject == null)
		{
			return;
		}
		onTapObject(objectName);
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x00035709 File Offset: 0x00033909
	public static void LaunchedProjectile(string objectName)
	{
		Action<string> onLaunchedProjectile = PlayerGameEvents.OnLaunchedProjectile;
		if (onLaunchedProjectile == null)
		{
			return;
		}
		onLaunchedProjectile(objectName);
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x0003571B File Offset: 0x0003391B
	public static void PlayerMoved(float distance, float speed)
	{
		Action<float, float> onPlayerMoved = PlayerGameEvents.OnPlayerMoved;
		if (onPlayerMoved == null)
		{
			return;
		}
		onPlayerMoved(distance, speed);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0003572E File Offset: 0x0003392E
	public static void PlayerSwam(float distance, float speed)
	{
		Action<float, float> onPlayerSwam = PlayerGameEvents.OnPlayerSwam;
		if (onPlayerSwam == null)
		{
			return;
		}
		onPlayerSwam(distance, speed);
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00035741 File Offset: 0x00033941
	public static void TriggerHandEffect(string effectName)
	{
		Action<string> onTriggerHandEffect = PlayerGameEvents.OnTriggerHandEffect;
		if (onTriggerHandEffect == null)
		{
			return;
		}
		onTriggerHandEffect(effectName);
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00035753 File Offset: 0x00033953
	public static void TriggerEnterLocation(string locationName)
	{
		Action<string> onEnterLocation = PlayerGameEvents.OnEnterLocation;
		if (onEnterLocation == null)
		{
			return;
		}
		onEnterLocation(locationName);
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00035765 File Offset: 0x00033965
	public static void MiscEvent(string eventName)
	{
		Action<string> onMiscEvent = PlayerGameEvents.OnMiscEvent;
		if (onMiscEvent == null)
		{
			return;
		}
		onMiscEvent(eventName);
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00035777 File Offset: 0x00033977
	public static void CritterEvent(string eventName)
	{
		Action<string> onCritterEvent = PlayerGameEvents.OnCritterEvent;
		if (onCritterEvent == null)
		{
			return;
		}
		onCritterEvent(eventName);
	}

	// Token: 0x0200011D RID: 285
	public enum EventType
	{
		// Token: 0x04000904 RID: 2308
		NONE,
		// Token: 0x04000905 RID: 2309
		GameModeObjective,
		// Token: 0x04000906 RID: 2310
		GameModeCompleteRound,
		// Token: 0x04000907 RID: 2311
		GrabbedObject,
		// Token: 0x04000908 RID: 2312
		DroppedObject,
		// Token: 0x04000909 RID: 2313
		EatObject,
		// Token: 0x0400090A RID: 2314
		TapObject,
		// Token: 0x0400090B RID: 2315
		LaunchedProjectile,
		// Token: 0x0400090C RID: 2316
		PlayerMoved,
		// Token: 0x0400090D RID: 2317
		PlayerSwam,
		// Token: 0x0400090E RID: 2318
		TriggerHandEfffect,
		// Token: 0x0400090F RID: 2319
		EnterLocation,
		// Token: 0x04000910 RID: 2320
		MiscEvent
	}
}
