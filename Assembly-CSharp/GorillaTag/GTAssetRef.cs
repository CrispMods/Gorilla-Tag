using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GorillaTag
{
	// Token: 0x02000BAA RID: 2986
	[Serializable]
	public class GTAssetRef<TObject> : AssetReferenceT<TObject> where TObject : UnityEngine.Object
	{
		// Token: 0x06004BCC RID: 19404 RVA: 0x00061D9C File Offset: 0x0005FF9C
		public GTAssetRef(string guid) : base(guid)
		{
		}
	}
}
