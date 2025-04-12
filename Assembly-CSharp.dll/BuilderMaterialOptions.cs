using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
[CreateAssetMenu(fileName = "BuilderMaterialOptions01a", menuName = "Gorilla Tag/Builder/Options", order = 0)]
public class BuilderMaterialOptions : ScriptableObject
{
	// Token: 0x06001DB1 RID: 7601 RVA: 0x000E15FC File Offset: 0x000DF7FC
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

	// Token: 0x06001DB2 RID: 7602 RVA: 0x000E1680 File Offset: 0x000DF880
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

	// Token: 0x040020CF RID: 8399
	public List<BuilderMaterialOptions.Options> options;

	// Token: 0x020004C7 RID: 1223
	[Serializable]
	public class Options
	{
		// Token: 0x040020D0 RID: 8400
		public string materialId;

		// Token: 0x040020D1 RID: 8401
		public Material material;

		// Token: 0x040020D2 RID: 8402
		[GorillaSoundLookup]
		public int soundIndex;

		// Token: 0x040020D3 RID: 8403
		[NonSerialized]
		public int materialType;
	}
}
