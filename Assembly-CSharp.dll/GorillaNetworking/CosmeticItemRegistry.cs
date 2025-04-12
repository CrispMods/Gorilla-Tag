using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000A87 RID: 2695
	public class CosmeticItemRegistry
	{
		// Token: 0x0600431E RID: 17182 RVA: 0x00174130 File Offset: 0x00172330
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

		// Token: 0x0600431F RID: 17183 RVA: 0x00174218 File Offset: 0x00172418
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

		// Token: 0x04004474 RID: 17524
		private bool _isInitialized;

		// Token: 0x04004475 RID: 17525
		private Dictionary<string, CosmeticItemInstance> nameToCosmeticMap = new Dictionary<string, CosmeticItemInstance>();

		// Token: 0x04004476 RID: 17526
		private GameObject nullItem;
	}
}
