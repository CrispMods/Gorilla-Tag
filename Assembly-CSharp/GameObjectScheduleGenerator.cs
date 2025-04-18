﻿using System;
using System.Globalization;
using GameObjectScheduling;
using UnityEngine;

// Token: 0x02000084 RID: 132
[CreateAssetMenu(fileName = "New Game Object Schedule Generator", menuName = "Game Object Scheduling/Game Object Schedule Generator")]
public class GameObjectScheduleGenerator : ScriptableObject
{
	// Token: 0x06000358 RID: 856 RVA: 0x00015454 File Offset: 0x00013654
	private void GenerateSchedule()
	{
		DateTime startDate;
		try
		{
			startDate = DateTime.Parse(this.scheduleStart, CultureInfo.InvariantCulture);
		}
		catch
		{
			Debug.LogError("Don't understand Start Date " + this.scheduleStart);
			return;
		}
		DateTime endDate;
		try
		{
			endDate = DateTime.Parse(this.scheduleEnd, CultureInfo.InvariantCulture);
		}
		catch
		{
			Debug.LogError("Don't understand End Date " + this.scheduleEnd);
			return;
		}
		if (this.scheduleType == GameObjectScheduleGenerator.ScheduleType.DailyShuffle)
		{
			GameObjectSchedule.GenerateDailyShuffle(startDate, endDate, this.schedules);
		}
	}

	// Token: 0x040003DC RID: 988
	[SerializeField]
	private GameObjectSchedule[] schedules;

	// Token: 0x040003DD RID: 989
	[SerializeField]
	private string scheduleStart = "1/1/0001 00:00:00";

	// Token: 0x040003DE RID: 990
	[SerializeField]
	private string scheduleEnd = "1/1/0001 00:00:00";

	// Token: 0x040003DF RID: 991
	[SerializeField]
	private GameObjectScheduleGenerator.ScheduleType scheduleType;

	// Token: 0x02000085 RID: 133
	private enum ScheduleType
	{
		// Token: 0x040003E1 RID: 993
		DailyShuffle
	}
}
