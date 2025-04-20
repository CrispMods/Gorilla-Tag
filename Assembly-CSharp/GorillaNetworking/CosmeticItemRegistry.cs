using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AB1 RID: 2737
	public class CosmeticItemRegistry
	{
		// Token: 0x06004457 RID: 17495 RVA: 0x0017AFB4 File Offset: 0x001791B4
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

		// Token: 0x06004458 RID: 17496 RVA: 0x0017B09C File Offset: 0x0017929C
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

		// Token: 0x0400455C RID: 17756
		private bool _isInitialized;

		// Token: 0x0400455D RID: 17757
		private Dictionary<string, CosmeticItemInstance> nameToCosmeticMap = new Dictionary<string, CosmeticItemInstance>();

		// Token: 0x0400455E RID: 17758
		private GameObject nullItem;
	}
}
