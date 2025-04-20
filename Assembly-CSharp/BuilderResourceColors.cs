using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004BF RID: 1215
[CreateAssetMenu(fileName = "BuilderMaterialResourceColors", menuName = "Gorilla Tag/Builder/ResourceColors", order = 0)]
public class BuilderResourceColors : ScriptableObject
{
	// Token: 0x06001D78 RID: 7544 RVA: 0x000E10C4 File Offset: 0x000DF2C4
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

	// Token: 0x04002074 RID: 8308
	public List<BuilderResourceColor> colors;
}
