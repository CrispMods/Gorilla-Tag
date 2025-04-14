using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
[CreateAssetMenu(fileName = "BuilderMaterialOptions01a", menuName = "Gorilla Tag/Builder/Options", order = 0)]
public class BuilderMaterialOptions : ScriptableObject
{
	// Token: 0x06001DAE RID: 7598 RVA: 0x00091638 File Offset: 0x0008F838
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

	// Token: 0x06001DAF RID: 7599 RVA: 0x000916BC File Offset: 0x0008F8BC
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

	// Token: 0x040020CE RID: 8398
	public List<BuilderMaterialOptions.Options> options;

	// Token: 0x020004C7 RID: 1223
	[Serializable]
	public class Options
	{
		// Token: 0x040020CF RID: 8399
		public string materialId;

		// Token: 0x040020D0 RID: 8400
		public Material material;

		// Token: 0x040020D1 RID: 8401
		[GorillaSoundLookup]
		public int soundIndex;

		// Token: 0x040020D2 RID: 8402
		[NonSerialized]
		public int materialType;
	}
}
