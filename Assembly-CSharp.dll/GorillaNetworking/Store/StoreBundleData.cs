using System;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AFF RID: 2815
	public class StoreBundleData : ScriptableObject
	{
		// Token: 0x0600465F RID: 18015 RVA: 0x001837C8 File Offset: 0x001819C8
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

		// Token: 0x040047EB RID: 18411
		public string playfabBundleID = "NULL";

		// Token: 0x040047EC RID: 18412
		public string bundleSKU = "NULL SKU";

		// Token: 0x040047ED RID: 18413
		public Sprite bundleImage;

		// Token: 0x040047EE RID: 18414
		public string bundleDescriptionText = "THE NULL_BUNDLE PACK WITH 10,000 SHINY ROCKS IN THIS LIMITED TIME DLC!";
	}
}
