using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AFC RID: 2812
	public class StoreBundleData : ScriptableObject
	{
		// Token: 0x06004653 RID: 18003 RVA: 0x0014D980 File Offset: 0x0014BB80
		public void OnValidate()
		{
			if (this.playfabBundleID.Contains(' '))
			{
				Debug.LogError("ERROR THERE IS A SPACE IN THE PLAYFAB BUNDLE ID " + base.name);
			}
			if (this.bundleSKU.Contains(' '))
			{
				Debug.LogError("ERROR THERE IS A SPACE IN THE BUNDLE SKU " + base.name);
			}
		}

		// Token: 0x040047D9 RID: 18393
		public string playfabBundleID = "NULL";

		// Token: 0x040047DA RID: 18394
		public string bundleSKU = "NULL SKU";

		// Token: 0x040047DB RID: 18395
		public Sprite bundleImage;

		// Token: 0x040047DC RID: 18396
		public string bundleDescriptionText = "THE NULL_BUNDLE PACK WITH 10,000 SHINY ROCKS IN THIS LIMITED TIME DLC!";
	}
}
