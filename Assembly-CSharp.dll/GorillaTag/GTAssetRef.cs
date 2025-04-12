using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GorillaTag
{
	// Token: 0x02000B80 RID: 2944
	[Serializable]
	public class GTAssetRef<TObject> : AssetReferenceT<TObject> where TObject : UnityEngine.Object
	{
		// Token: 0x06004A8D RID: 19085 RVA: 0x00060364 File Offset: 0x0005E564
		public GTAssetRef(string guid) : base(guid)
		{
		}
	}
}
