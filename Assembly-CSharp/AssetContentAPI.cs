using System;
using UnityEngine;

// Token: 0x0200089F RID: 2207
public class AssetContentAPI : ScriptableObject
{
	// Token: 0x040037BF RID: 14271
	public string bundleName;

	// Token: 0x040037C0 RID: 14272
	public LazyLoadReference<TextAsset> bundleFile;

	// Token: 0x040037C1 RID: 14273
	public Object[] assets = new Object[0];
}
