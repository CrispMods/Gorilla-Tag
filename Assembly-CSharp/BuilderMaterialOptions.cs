using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D3 RID: 1235
[CreateAssetMenu(fileName = "BuilderMaterialOptions01a", menuName = "Gorilla Tag/Builder/Options", order = 0)]
public class BuilderMaterialOptions : ScriptableObject
{
	// Token: 0x06001E07 RID: 7687 RVA: 0x000E4338 File Offset: 0x000E2538
	public void GetMaterialFromType(int materialType, out Material material, out int soundIndex)
	{
		if (this.options == null)
		{
			material = null;
			soundIndex = -1;
			return;
		}
		foreach (BuilderMaterialOptions.Options options in this.options)
		{
			if (options.materialId.GetHashCode() == materialType)
			{
				material = options.material;
				soundIndex = options.soundIndex;
				return;
			}
		}
		material = null;
		soundIndex = -1;
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x000E43BC File Offset: 0x000E25BC
	public void GetDefaultMaterial(out int materialType, out Material material, out int soundIndex)
	{
		if (this.options.Count > 0)
		{
			materialType = this.options[0].materialId.GetHashCode();
			material = this.options[0].material;
			soundIndex = this.options[0].soundIndex;
			return;
		}
		materialType = -1;
		material = null;
		soundIndex = -1;
	}

	// Token: 0x04002121 RID: 8481
	public List<BuilderMaterialOptions.Options> options;

	// Token: 0x020004D4 RID: 1236
	[Serializable]
	public class Options
	{
		// Token: 0x04002122 RID: 8482
		public string materialId;

		// Token: 0x04002123 RID: 8483
		public Material material;

		// Token: 0x04002124 RID: 8484
		[GorillaSoundLookup]
		public int soundIndex;

		// Token: 0x04002125 RID: 8485
		[NonSerialized]
		public int materialType;
	}
}
