using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
[Serializable]
public class CritterConfiguration
{
	// Token: 0x060000AC RID: 172 RVA: 0x00030B19 File Offset: 0x0002ED19
	public CritterConfiguration()
	{
		this.animalType = CritterConfiguration.AnimalType.UNKNOWN;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00030B45 File Offset: 0x0002ED45
	public int GetIndex()
	{
		return CrittersManager.instance.creatureIndex.critterTypes.IndexOf(this);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00030B5E File Offset: 0x0002ED5E
	private bool RegionMatches(CrittersRegion region)
	{
		return !region || (region.Biome & this.biome) > (CrittersBiome)0;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00030B7A File Offset: 0x0002ED7A
	private bool SpawnCriteriaMatches()
	{
		return !this.spawnCriteria || this.spawnCriteria.CanSpawn();
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00030B96 File Offset: 0x0002ED96
	public bool CanSpawn()
	{
		return this.SpawnCriteriaMatches();
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00030B9E File Offset: 0x0002ED9E
	public bool CanSpawn(CrittersRegion region)
	{
		return this.RegionMatches(region) && this.SpawnCriteriaMatches();
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00030BB1 File Offset: 0x0002EDB1
	public bool DateConditionsMet(DateTime utcDate)
	{
		return !this.dateLimit || this.dateLimit.MatchesDate(utcDate);
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00030BCE File Offset: 0x0002EDCE
	public bool ShouldDespawn()
	{
		return !this.SpawnCriteriaMatches();
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00030BD9 File Offset: 0x0002EDD9
	public void ApplyToCreature(CrittersPawn crittersPawn)
	{
		this.behaviour.ApplyToCritter(crittersPawn);
		if (CrittersManager.instance.LocalAuthority())
		{
			this.ApplyVisualsTo(crittersPawn, true);
			return;
		}
		this.ApplyVisualsTo(crittersPawn, false);
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00030C06 File Offset: 0x0002EE06
	private void ApplyVisualsTo(CrittersPawn critter, bool generateAppearance = true)
	{
		this.ApplyVisualsTo(critter.visuals, generateAppearance);
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00030C15 File Offset: 0x0002EE15
	public void ApplyVisualsTo(CritterVisuals visuals, bool generateAppearance = true)
	{
		visuals.critterType = this.GetIndex();
		visuals.ApplyMesh(CritterIndex.GetMesh(this.animalType));
		visuals.ApplyMaterial(this.critterMat);
		if (generateAppearance)
		{
			visuals.SetAppearance(this.GenerateAppearance());
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x0006A7F8 File Offset: 0x000689F8
	public CritterAppearance GenerateAppearance()
	{
		string hatName = "";
		if (UnityEngine.Random.value <= this.behaviour.GetTemplateValue<float>("hatChance"))
		{
			GameObject[] templateValue = this.behaviour.GetTemplateValue<GameObject[]>("hats");
			if (!templateValue.IsNullOrEmpty<GameObject>())
			{
				hatName = templateValue[UnityEngine.Random.Range(0, templateValue.Length)].name;
			}
		}
		float templateValue2 = this.behaviour.GetTemplateValue<float>("minSize");
		float templateValue3 = this.behaviour.GetTemplateValue<float>("maxSize");
		float size = UnityEngine.Random.Range(templateValue2, templateValue3);
		return new CritterAppearance(hatName, size);
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00030C4F File Offset: 0x0002EE4F
	public override string ToString()
	{
		return string.Format("{0} B:{1} C:{2}", this.critterName, this.behaviour, this.spawnCriteria);
	}

	// Token: 0x040000C6 RID: 198
	[Tooltip("Basic internal description of critter.  Could be role, purpose, player experience, etc.")]
	public string internalDescription;

	// Token: 0x040000C7 RID: 199
	public string critterName = "UNNAMED CRITTER";

	// Token: 0x040000C8 RID: 200
	public CritterConfiguration.AnimalType animalType;

	// Token: 0x040000C9 RID: 201
	public CritterTemplate behaviour;

	// Token: 0x040000CA RID: 202
	public CritterSpawnCriteria spawnCriteria;

	// Token: 0x040000CB RID: 203
	public RealWorldDateTimeWindow dateLimit;

	// Token: 0x040000CC RID: 204
	public CrittersBiome biome = CrittersBiome.Any;

	// Token: 0x040000CD RID: 205
	public float spawnWeight = 1f;

	// Token: 0x040000CE RID: 206
	public Material critterMat;

	// Token: 0x02000030 RID: 48
	public enum AnimalType
	{
		// Token: 0x040000D0 RID: 208
		Raccoon,
		// Token: 0x040000D1 RID: 209
		Cat,
		// Token: 0x040000D2 RID: 210
		Bird,
		// Token: 0x040000D3 RID: 211
		Goblin,
		// Token: 0x040000D4 RID: 212
		Egg,
		// Token: 0x040000D5 RID: 213
		UNKNOWN = -1
	}
}
