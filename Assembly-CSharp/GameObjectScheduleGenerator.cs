using System;
using System.Globalization;
using GameObjectScheduling;
using UnityEngine;

// Token: 0x0200008B RID: 139
[CreateAssetMenu(fileName = "New Game Object Schedule Generator", menuName = "Game Object Scheduling/Game Object Schedule Generator")]
public class GameObjectScheduleGenerator : ScriptableObject
{
	// Token: 0x0600038A RID: 906 RVA: 0x00079798 File Offset: 0x00077998
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

	// Token: 0x04000410 RID: 1040
	[SerializeField]
	private GameObjectSchedule[] schedules;

	// Token: 0x04000411 RID: 1041
	[SerializeField]
	private string scheduleStart = "1/1/0001 00:00:00";

	// Token: 0x04000412 RID: 1042
	[SerializeField]
	private string scheduleEnd = "1/1/0001 00:00:00";

	// Token: 0x04000413 RID: 1043
	[SerializeField]
	private GameObjectScheduleGenerator.ScheduleType scheduleType;

	// Token: 0x0200008C RID: 140
	private enum ScheduleType
	{
		// Token: 0x04000415 RID: 1045
		DailyShuffle
	}
}
