using System;
using System.Collections.Generic;

// Token: 0x02000043 RID: 67
public static class CrittersBiomeExtensions
{
	// Token: 0x06000145 RID: 325 RVA: 0x0006DD0C File Offset: 0x0006BF0C
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

	// Token: 0x06000146 RID: 326 RVA: 0x0006DD94 File Offset: 0x0006BF94
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

	// Token: 0x0400018F RID: 399
	private static List<CrittersBiome> _allScannableBiomes;

	// Token: 0x04000190 RID: 400
	private static Dictionary<CrittersBiome, string> _habitatLookup = new Dictionary<CrittersBiome, string>();

	// Token: 0x04000191 RID: 401
	private static List<CrittersBiome> _habitatBiomes;
}
