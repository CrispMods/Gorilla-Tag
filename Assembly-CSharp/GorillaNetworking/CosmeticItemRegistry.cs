using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000A84 RID: 2692
	public class CosmeticItemRegistry
	{
		// Token: 0x06004312 RID: 17170 RVA: 0x0013C0DC File Offset: 0x0013A2DC
		public void Initialize(GameObject[] cosmeticGObjs)
		{
			if (this._isInitialized)
			{
				return;
			}
			this._isInitialized = true;
			foreach (GameObject gameObject in cosmeticGObjs)
			{
				string key = gameObject.name.Replace("LEFT.", "").Replace("RIGHT.", "").TrimEnd();
				CosmeticItemInstance cosmeticItemInstance;
				if (this.nameToCosmeticMap.ContainsKey(key))
				{
					cosmeticItemInstance = this.nameToCosmeticMap[key];
				}
				else
				{
					cosmeticItemInstance = new CosmeticItemInstance();
					this.nameToCosmeticMap.Add(key, cosmeticItemInstance);
				}
				bool flag = gameObject.name.Contains("LEFT.");
				bool flag2 = gameObject.name.Contains("RIGHT.");
				if (flag)
				{
					cosmeticItemInstance.leftObjects.Add(gameObject);
				}
				else if (flag2)
				{
					cosmeticItemInstance.rightObjects.Add(gameObject);
				}
				else
				{
					cosmeticItemInstance.objects.Add(gameObject);
				}
			}
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x0013C1C4 File Offset: 0x0013A3C4
		public CosmeticItemInstance Cosmetic(string itemName)
		{
			if (!this._isInitialized)
			{
				Debug.LogError("Tried to use CosmeticItemRegistry before it was initialized!");
				return null;
			}
			if (string.IsNullOrEmpty(itemName) || itemName == "NOTHING")
			{
				return null;
			}
			CosmeticItemInstance result;
			if (!this.nameToCosmeticMap.TryGetValue(itemName, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x04004462 RID: 17506
		private bool _isInitialized;

		// Token: 0x04004463 RID: 17507
		private Dictionary<string, CosmeticItemInstance> nameToCosmeticMap = new Dictionary<string, CosmeticItemInstance>();

		// Token: 0x04004464 RID: 17508
		private GameObject nullItem;
	}
}
