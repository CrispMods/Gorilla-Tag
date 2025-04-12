using System;

// Token: 0x02000112 RID: 274
public class PlayerGameEvents
{
	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06000758 RID: 1880 RVA: 0x00088948 File Offset: 0x00086B48
	// (remove) Token: 0x06000759 RID: 1881 RVA: 0x0008897C File Offset: 0x00086B7C
	public static event Action<string> OnGameModeObjectiveTrigger;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x0600075A RID: 1882 RVA: 0x000889B0 File Offset: 0x00086BB0
	// (remove) Token: 0x0600075B RID: 1883 RVA: 0x000889E4 File Offset: 0x00086BE4
	public static event Action<string> OnGameModeCompleteRound;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x0600075C RID: 1884 RVA: 0x00088A18 File Offset: 0x00086C18
	// (remove) Token: 0x0600075D RID: 1885 RVA: 0x00088A4C File Offset: 0x00086C4C
	public static event Action<string> OnGrabbedObject;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x0600075E RID: 1886 RVA: 0x00088A80 File Offset: 0x00086C80
	// (remove) Token: 0x0600075F RID: 1887 RVA: 0x00088AB4 File Offset: 0x00086CB4
	public static event Action<string> OnDroppedObject;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06000760 RID: 1888 RVA: 0x00088AE8 File Offset: 0x00086CE8
	// (remove) Token: 0x06000761 RID: 1889 RVA: 0x00088B1C File Offset: 0x00086D1C
	public static event Action<string> OnEatObject;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06000762 RID: 1890 RVA: 0x00088B50 File Offset: 0x00086D50
	// (remove) Token: 0x06000763 RID: 1891 RVA: 0x00088B84 File Offset: 0x00086D84
	public static event Action<string> OnTapObject;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06000764 RID: 1892 RVA: 0x00088BB8 File Offset: 0x00086DB8
	// (remove) Token: 0x06000765 RID: 1893 RVA: 0x00088BEC File Offset: 0x00086DEC
	public static event Action<string> OnLaunchedProjectile;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06000766 RID: 1894 RVA: 0x00088C20 File Offset: 0x00086E20
	// (remove) Token: 0x06000767 RID: 1895 RVA: 0x00088C54 File Offset: 0x00086E54
	public static event Action<float, float> OnPlayerMoved;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06000768 RID: 1896 RVA: 0x00088C88 File Offset: 0x00086E88
	// (remove) Token: 0x06000769 RID: 1897 RVA: 0x00088CBC File Offset: 0x00086EBC
	public static event Action<float, float> OnPlayerSwam;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x0600076A RID: 1898 RVA: 0x00088CF0 File Offset: 0x00086EF0
	// (remove) Token: 0x0600076B RID: 1899 RVA: 0x00088D24 File Offset: 0x00086F24
	public static event Action<string> OnTriggerHandEffect;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x0600076C RID: 1900 RVA: 0x00088D58 File Offset: 0x00086F58
	// (remove) Token: 0x0600076D RID: 1901 RVA: 0x00088D8C File Offset: 0x00086F8C
	public static event Action<string> OnEnterLocation;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x0600076E RID: 1902 RVA: 0x00088DC0 File Offset: 0x00086FC0
	// (remove) Token: 0x0600076F RID: 1903 RVA: 0x00088DF4 File Offset: 0x00086FF4
	public static event Action<string> OnMiscEvent;

	// Token: 0x06000770 RID: 1904 RVA: 0x00088E28 File Offset: 0x00087028
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

	// Token: 0x06000771 RID: 1905 RVA: 0x00088E50 File Offset: 0x00087050
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

	// Token: 0x06000772 RID: 1906 RVA: 0x0003445D File Offset: 0x0003265D
	public static void GrabbedObject(string objectName)
	{
		Action<string> onGrabbedObject = PlayerGameEvents.OnGrabbedObject;
		if (onGrabbedObject == null)
		{
			return;
		}
		onGrabbedObject(objectName);
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0003446F File Offset: 0x0003266F
	public static void DroppedObject(string objectName)
	{
		Action<string> onDroppedObject = PlayerGameEvents.OnDroppedObject;
		if (onDroppedObject == null)
		{
			return;
		}
		onDroppedObject(objectName);
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x00034481 File Offset: 0x00032681
	public static void EatObject(string objectName)
	{
		Action<string> onEatObject = PlayerGameEvents.OnEatObject;
		if (onEatObject == null)
		{
			return;
		}
		onEatObject(objectName);
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x00034493 File Offset: 0x00032693
	public static void TapObject(string objectName)
	{
		Action<string> onTapObject = PlayerGameEvents.OnTapObject;
		if (onTapObject == null)
		{
			return;
		}
		onTapObject(objectName);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x000344A5 File Offset: 0x000326A5
	public static void LaunchedProjectile(string objectName)
	{
		Action<string> onLaunchedProjectile = PlayerGameEvents.OnLaunchedProjectile;
		if (onLaunchedProjectile == null)
		{
			return;
		}
		onLaunchedProjectile(objectName);
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x000344B7 File Offset: 0x000326B7
	public static void PlayerMoved(float distance, float speed)
	{
		Action<float, float> onPlayerMoved = PlayerGameEvents.OnPlayerMoved;
		if (onPlayerMoved == null)
		{
			return;
		}
		onPlayerMoved(distance, speed);
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x000344CA File Offset: 0x000326CA
	public static void PlayerSwam(float distance, float speed)
	{
		Action<float, float> onPlayerSwam = PlayerGameEvents.OnPlayerSwam;
		if (onPlayerSwam == null)
		{
			return;
		}
		onPlayerSwam(distance, speed);
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x000344DD File Offset: 0x000326DD
	public static void TriggerHandEffect(string effectName)
	{
		Action<string> onTriggerHandEffect = PlayerGameEvents.OnTriggerHandEffect;
		if (onTriggerHandEffect == null)
		{
			return;
		}
		onTriggerHandEffect(effectName);
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x000344EF File Offset: 0x000326EF
	public static void TriggerEnterLocation(string locationName)
	{
		Action<string> onEnterLocation = PlayerGameEvents.OnEnterLocation;
		if (onEnterLocation == null)
		{
			return;
		}
		onEnterLocation(locationName);
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00034501 File Offset: 0x00032701
	public static void MiscEvent(string eventName)
	{
		Action<string> onMiscEvent = PlayerGameEvents.OnMiscEvent;
		if (onMiscEvent == null)
		{
			return;
		}
		onMiscEvent(eventName);
	}

	// Token: 0x02000113 RID: 275
	public enum EventType
	{
		// Token: 0x040008C3 RID: 2243
		NONE,
		// Token: 0x040008C4 RID: 2244
		GameModeObjective,
		// Token: 0x040008C5 RID: 2245
		GameModeCompleteRound,
		// Token: 0x040008C6 RID: 2246
		GrabbedObject,
		// Token: 0x040008C7 RID: 2247
		DroppedObject,
		// Token: 0x040008C8 RID: 2248
		EatObject,
		// Token: 0x040008C9 RID: 2249
		TapObject,
		// Token: 0x040008CA RID: 2250
		LaunchedProjectile,
		// Token: 0x040008CB RID: 2251
		PlayerMoved,
		// Token: 0x040008CC RID: 2252
		PlayerSwam,
		// Token: 0x040008CD RID: 2253
		TriggerHandEfffect,
		// Token: 0x040008CE RID: 2254
		EnterLocation,
		// Token: 0x040008CF RID: 2255
		MiscEvent
	}
}
