using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class CritterSpawnCriteria : ScriptableObject
{
	// Token: 0x06000240 RID: 576 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
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

	// Token: 0x040002C4 RID: 708
	public string[] spawnTimings;
}
