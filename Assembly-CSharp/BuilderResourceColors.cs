using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
[CreateAssetMenu(fileName = "BuilderMaterialResourceColors", menuName = "Gorilla Tag/Builder/ResourceColors", order = 0)]
public class BuilderResourceColors : ScriptableObject
{
	// Token: 0x06001D1F RID: 7455 RVA: 0x0008DDDC File Offset: 0x0008BFDC
	public Color GetColorForType(BuilderResourceType type)
	{
		foreach (BuilderResourceColor builderResourceColor in this.colors)
		{
			if (builderResourceColor.type == type)
			{
				return builderResourceColor.color;
			}
		}
		return Color.black;
	}

	// Token: 0x04002021 RID: 8225
	public List<BuilderResourceColor> colors;
}
