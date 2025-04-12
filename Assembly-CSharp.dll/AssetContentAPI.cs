using System;
using UnityEngine;

// Token: 0x020008A2 RID: 2210
public class AssetContentAPI : ScriptableObject
{
	// Token: 0x040037D1 RID: 14289
	public string bundleName;

	// Token: 0x040037D2 RID: 14290
	public LazyLoadReference<TextAsset> bundleFile;

	// Token: 0x040037D3 RID: 14291
	public UnityEngine.Object[] assets = new UnityEngine.Object[0];
}
