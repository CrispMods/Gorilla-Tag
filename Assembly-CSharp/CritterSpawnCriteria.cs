using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class CritterSpawnCriteria : ScriptableObject
{
	// Token: 0x0600025E RID: 606 RVA: 0x00073C00 File Offset: 0x00071E00
	public bool CanSpawn()
	{
		if (this.spawnTimings.Length == 0)
		{
			return true;
		}
		string currentTimeOfDay = BetterDayNightManager.instance.currentTimeOfDay;
		string[] array = this.spawnTimings;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == currentTimeOfDay)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040002E9 RID: 745
	public string[] spawnTimings;
}
