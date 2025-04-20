using System;
using UnityEngine;

// Token: 0x020008BB RID: 2235
public class AssetContentAPI : ScriptableObject
{
	// Token: 0x04003880 RID: 14464
	public string bundleName;

	// Token: 0x04003881 RID: 14465
	public LazyLoadReference<TextAsset> bundleFile;

	// Token: 0x04003882 RID: 14466
	public UnityEngine.Object[] assets = new UnityEngine.Object[0];
}
