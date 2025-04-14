using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
[Serializable]
public class CritterConfiguration
{
	// Token: 0x060000A6 RID: 166 RVA: 0x000050E6 File Offset: 0x000032E6
	public int GetIndex()
	{
		return CrittersManager.instance.creatureIndex.critterTypes.IndexOf(this);
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x000050FF File Offset: 0x000032FF
	public bool CanSpawn()
	{
		CritterSpawnCriteria critterSpawnCriteria = this.spawnCriteria;
		return critterSpawnCriteria == null || critterSpawnCriteria.CanSpawn();
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00005112 File Offset: 0x00003312
	public bool CanSpawn(CrittersRegion region)
	{
		if (!region)
		{
			return this.CanSpawn();
		}
		return (region.Biome & this.biome) != (CrittersBiome)0 && this.CanSpawn();
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x0000513A File Offset: 0x0000333A
	public bool ShouldDespawn()
	{
		return !this.CanSpawn();
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00005145 File Offset: 0x00003345
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

	// Token: 0x060000AB RID: 171 RVA: 0x00005172 File Offset: 0x00003372
	private void ApplyVisualsTo(CrittersPawn critter, bool generateAppearance = true)
	{
		this.ApplyVisualsTo(critter.visuals, generateAppearance);
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00005181 File Offset: 0x00003381
	public void ApplyVisualsTo(CritterVisuals visuals, bool generateAppearance = true)
	{
		visuals.critterType = this.GetIndex();
		visuals.ApplyMesh(this.critterMesh);
		visuals.ApplyMaterial(this.critterMat);
		if (generateAppearance)
		{
			visuals.SetAppearance(this.GenerateAppearance());
		}
	}

	// Token: 0x060000AD RID: 173 RVA: 0x000051B8 File Offset: 0x000033B8
	public CritterAppearance GenerateAppearance()
	{
		string hatName = "";
		if (Random.value <= this.behaviour.GetTemplateValue<float>("hatChance"))
		{
			GameObject[] templateValue = this.behaviour.GetTemplateValue<GameObject[]>("hats");
			if (!templateValue.IsNullOrEmpty<GameObject>())
			{
				hatName = templateValue[Random.Range(0, templateValue.Length)].name;
			}
		}
		float templateValue2 = this.behaviour.GetTemplateValue<float>("minSize");
		float templateValue3 = this.behaviour.GetTemplateValue<float>("maxSize");
		float size = Random.Range(templateValue2, templateValue3);
		return new CritterAppearance(hatName, size);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00005240 File Offset: 0x00003440
	public override string ToString()
	{
		return string.Format("{0} B:{1} C:{2}", this.critterName, this.behaviour, this.spawnCriteria);
	}

	// Token: 0x040000C5 RID: 197
	[Tooltip("Basic internal description of critter.  Could be role, purpose, player experience, etc.")]
	public string internalDescription;

	// Token: 0x040000C6 RID: 198
	public string critterName = "UNNAMED CRITTER";

	// Token: 0x040000C7 RID: 199
	public CritterConfiguration.AnimalType animalType;

	// Token: 0x040000C8 RID: 200
	public CritterTemplate behaviour;

	// Token: 0x040000C9 RID: 201
	public CritterSpawnCriteria spawnCriteria;

	// Token: 0x040000CA RID: 202
	public CrittersBiome biome = CrittersBiome.Any;

	// Token: 0x040000CB RID: 203
	public float spawnWeight = 1f;

	// Token: 0x040000CC RID: 204
	public Mesh critterMesh;

	// Token: 0x040000CD RID: 205
	public Material critterMat;

	// Token: 0x0200002E RID: 46
	public enum AnimalType
	{
		// Token: 0x040000CF RID: 207
		UNKNOWN,
		// Token: 0x040000D0 RID: 208
		Raccoon,
		// Token: 0x040000D1 RID: 209
		Cat,
		// Token: 0x040000D2 RID: 210
		Bird,
		// Token: 0x040000D3 RID: 211
		Goblin
	}
}
