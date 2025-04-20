using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B29 RID: 2857
	public class StoreBundleData : ScriptableObject
	{
		// Token: 0x0600479B RID: 18331 RVA: 0x0018A6C0 File Offset: 0x001888C0
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

		// Token: 0x040048CE RID: 18638
		public string playfabBundleID = "NULL";

		// Token: 0x040048CF RID: 18639
		public string bundleSKU = "NULL SKU";

		// Token: 0x040048D0 RID: 18640
		public Sprite bundleImage;

		// Token: 0x040048D1 RID: 18641
		public string bundleDescriptionText = "THE NULL_BUNDLE PACK WITH 10,000 SHINY ROCKS IN THIS LIMITED TIME DLC!";
	}
}
