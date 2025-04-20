using System;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000031 RID: 49
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

	// Token: 0x060000BA RID: 186 RVA: 0x00030C8F File Offset: 0x0002EE8F
	private void OnEnable()
	{
		CritterIndex._instance = this;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00030C97 File Offset: 0x0002EE97
	public static Mesh GetMesh(CritterConfiguration.AnimalType animalType)
	{
		if (animalType < CritterConfiguration.AnimalType.Raccoon || animalType >= (CritterConfiguration.AnimalType)CritterIndex._instance.animalMeshes.Count)
		{
			return null;
		}
		return CritterIndex._instance.animalMeshes[(int)animalType].mesh;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00030CC6 File Offset: 0x0002EEC6
	public int GetRandomCritterType(CrittersRegion region = null)
	{
		return this.critterTypes.IndexOf(this.GetRandomConfiguration(region));
	}

	// Token: 0x060000BD RID: 189 RVA: 0x0006A880 File Offset: 0x00068A80
	public CritterConfiguration GetRandomConfiguration(CrittersRegion region = null)
	{
		WeightedList<CritterConfiguration> validCritterTypes = this.GetValidCritterTypes(region);
		if (validCritterTypes.Count == 0)
		{
			return null;
		}
		return validCritterTypes.GetRandomItem();
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00030CDA File Offset: 0x0002EEDA
	public static DateTime GetCritterDateTime()
	{
		if (!GorillaComputer.instance)
		{
			return DateTime.UtcNow;
		}
		return GorillaComputer.instance.GetServerTime();
	}

	// Token: 0x060000BF RID: 191 RVA: 0x0006A8A8 File Offset: 0x00068AA8
	private WeightedList<CritterConfiguration> GetValidCritterTypes(CrittersRegion region = null)
	{
		this._currentConfigs.Clear();
		DateTime critterDateTime = CritterIndex.GetCritterDateTime();
		foreach (CritterConfiguration critterConfiguration in this.critterTypes)
		{
			if (critterConfiguration.DateConditionsMet(critterDateTime) && critterConfiguration.CanSpawn(region))
			{
				this._currentConfigs.Add(critterConfiguration, critterConfiguration.spawnWeight);
			}
		}
		return this._currentConfigs;
	}

	// Token: 0x040000D6 RID: 214
	public List<CritterIndex.AnimalTypeMeshEntry> animalMeshes = new List<CritterIndex.AnimalTypeMeshEntry>();

	// Token: 0x040000D7 RID: 215
	public List<CritterConfiguration> critterTypes;

	// Token: 0x040000D8 RID: 216
	private WeightedList<CritterConfiguration> _currentConfigs = new WeightedList<CritterConfiguration>();

	// Token: 0x040000D9 RID: 217
	private static CritterIndex _instance;

	// Token: 0x02000032 RID: 50
	[Serializable]
	public class AnimalTypeMeshEntry
	{
		// Token: 0x040000DA RID: 218
		public CritterConfiguration.AnimalType animalType;

		// Token: 0x040000DB RID: 219
		public Mesh mesh;
	}
}
