using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class CritterSpawnCriteria : ScriptableObject
{
	// Token: 0x06000241 RID: 577 RVA: 0x00071778 File Offset: 0x0006F978
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

	// Token: 0x040002C5 RID: 709
	public string[] spawnTimings;
}
