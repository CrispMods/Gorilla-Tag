using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class CritterIndex : ScriptableObject
{
	// Token: 0x1700000D RID: 13
	public CritterConfiguration this[int index]
	{
		get
		{
			if (index < 0 || index >= this.critterTypes.Count)
			{
				return null;
			}
			return this.critterTypes[index];
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000052A5 File Offset: 0x000034A5
	public int GetRandomCritterType(CrittersRegion region = null)
	{
		return this.critterTypes.IndexOf(this.GetRandomConfiguration(region));
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x000052BC File Offset: 0x000034BC
	public CritterConfiguration GetRandomConfiguration(CrittersRegion region = null)
	{
		this._currentConfigs.Clear();
		foreach (CritterConfiguration critterConfiguration in this.critterTypes)
		{
			if (critterConfiguration.CanSpawn(region))
			{
				this._currentConfigs.Add(critterConfiguration, critterConfiguration.spawnWeight);
			}
		}
		if (this._currentConfigs.Count != 0)
		{
			return this._currentConfigs.GetRandomItem();
		}
		return null;
	}

	// Token: 0x040000D4 RID: 212
	public List<CritterConfiguration> critterTypes;

	// Token: 0x040000D5 RID: 213
	private WeightedList<CritterConfiguration> _currentConfigs = new WeightedList<CritterConfiguration>();
}
