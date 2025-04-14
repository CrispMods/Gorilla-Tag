using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GorillaTag
{
	// Token: 0x02000B7D RID: 2941
	[Serializable]
	public class GTAssetRef<TObject> : AssetReferenceT<TObject> where TObject : Object
	{
		// Token: 0x06004A81 RID: 19073 RVA: 0x001693B0 File Offset: 0x001675B0
		public GTAssetRef(string guid) : base(guid)
		{
		}
	}
}
