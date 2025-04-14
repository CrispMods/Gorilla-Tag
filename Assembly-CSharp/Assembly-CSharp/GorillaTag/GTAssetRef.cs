using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GorillaTag
{
	// Token: 0x02000B80 RID: 2944
	[Serializable]
	public class GTAssetRef<TObject> : AssetReferenceT<TObject> where TObject : Object
	{
		// Token: 0x06004A8D RID: 19085 RVA: 0x00169978 File Offset: 0x00167B78
		public GTAssetRef(string guid) : base(guid)
		{
		}
	}
}
