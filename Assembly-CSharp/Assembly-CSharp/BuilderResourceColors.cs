using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
[CreateAssetMenu(fileName = "BuilderMaterialResourceColors", menuName = "Gorilla Tag/Builder/ResourceColors", order = 0)]
public class BuilderResourceColors : ScriptableObject
{
	// Token: 0x06001D22 RID: 7458 RVA: 0x0008E160 File Offset: 0x0008C360
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

	// Token: 0x04002022 RID: 8226
	public List<BuilderResourceColor> colors;
}
