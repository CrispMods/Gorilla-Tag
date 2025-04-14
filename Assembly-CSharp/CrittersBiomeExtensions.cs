using System;
using System.Collections.Generic;

// Token: 0x0200003F RID: 63
public static class CrittersBiomeExtensions
{
	// Token: 0x0600012F RID: 303 RVA: 0x000087B4 File Offset: 0x000069B4
	static CrittersBiomeExtensions()
	{
		CrittersBiomeExtensions._allScannableBiomes = new List<CrittersBiome>();
		foreach (object obj in Enum.GetValues(typeof(CrittersBiome)))
		{
			CrittersBiome crittersBiome = (CrittersBiome)obj;
			if (crittersBiome != CrittersBiome.Any && crittersBiome != CrittersBiome.IntroArea)
			{
				CrittersBiomeExtensions._allScannableBiomes.Add(crittersBiome);
			}
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000883C File Offset: 0x00006A3C
	public static string GetHabitatDescription(this CrittersBiome biome)
	{
		string text;
		if (!CrittersBiomeExtensions._habitatLookup.TryGetValue(biome, out text))
		{
			if (biome == CrittersBiome.Any)
			{
				text = "Any";
			}
			else
			{
				if (CrittersBiomeExtensions._habitatBiomes == null)
				{
					CrittersBiomeExtensions._habitatBiomes = new List<CrittersBiome>();
				}
				CrittersBiomeExtensions._habitatBiomes.Clear();
				for (int i = 0; i < CrittersBiomeExtensions._allScannableBiomes.Count; i++)
				{
					if (biome.HasFlag(CrittersBiomeExtensions._allScannableBiomes[i]))
					{
						CrittersBiomeExtensions._habitatBiomes.Add(CrittersBiomeExtensions._allScannableBiomes[i]);
					}
				}
			}
			text = ((CrittersBiomeExtensions._habitatBiomes.Count > 3) ? "Various" : string.Join<CrittersBiome>(", ", CrittersBiomeExtensions._habitatBiomes));
			CrittersBiomeExtensions._habitatLookup[biome] = text;
		}
		return text;
	}

	// Token: 0x0400017C RID: 380
	private static List<CrittersBiome> _allScannableBiomes;

	// Token: 0x0400017D RID: 381
	private static Dictionary<CrittersBiome, string> _habitatLookup = new Dictionary<CrittersBiome, string>();

	// Token: 0x0400017E RID: 382
	private static List<CrittersBiome> _habitatBiomes;
}
