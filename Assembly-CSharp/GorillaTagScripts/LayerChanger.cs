using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009EC RID: 2540
	public class LayerChanger : MonoBehaviour
	{
		// Token: 0x06003F6A RID: 16234 RVA: 0x00059611 File Offset: 0x00057811
		public void InitializeLayers(Transform parent)
		{
			if (!this.layersStored)
			{
				this.StoreOriginalLayers(parent);
				this.layersStored = true;
			}
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x00168674 File Offset: 0x00166874
		private void StoreOriginalLayers(Transform parent)
		{
			if (!this.includeChildren)
			{
				this.StoreOriginalLayers(parent);
				return;
			}
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				this.originalLayers[transform] = transform.gameObject.layer;
				this.StoreOriginalLayers(transform);
			}
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x00059629 File Offset: 0x00057829
		public void ChangeLayer(Transform parent, string newLayer)
		{
			if (!this.layersStored)
			{
				Debug.LogWarning("Layers have not been initialized. Call InitializeLayers first.");
				return;
			}
			this.ChangeLayers(parent, LayerMask.NameToLayer(newLayer));
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x001686F0 File Offset: 0x001668F0
		private void ChangeLayers(Transform parent, int newLayer)
		{
			if (!this.includeChildren)
			{
				if (!this.restrictedLayers.Contains(parent.gameObject.layer))
				{
					parent.gameObject.layer = newLayer;
				}
				return;
			}
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				if (!this.restrictedLayers.Contains(transform.gameObject.layer))
				{
					transform.gameObject.layer = newLayer;
					this.ChangeLayers(transform, newLayer);
				}
			}
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x00168798 File Offset: 0x00166998
		public void RestoreOriginalLayers()
		{
			if (!this.layersStored)
			{
				Debug.LogWarning("Layers have not been initialized. Call InitializeLayers first.");
				return;
			}
			foreach (KeyValuePair<Transform, int> keyValuePair in this.originalLayers)
			{
				keyValuePair.Key.gameObject.layer = keyValuePair.Value;
			}
		}

		// Token: 0x0400405D RID: 16477
		public LayerMask restrictedLayers;

		// Token: 0x0400405E RID: 16478
		public bool includeChildren = true;

		// Token: 0x0400405F RID: 16479
		private Dictionary<Transform, int> originalLayers = new Dictionary<Transform, int>();

		// Token: 0x04004060 RID: 16480
		private bool layersStored;
	}
}
