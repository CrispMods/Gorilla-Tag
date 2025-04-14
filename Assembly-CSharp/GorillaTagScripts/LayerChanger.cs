using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009C6 RID: 2502
	public class LayerChanger : MonoBehaviour
	{
		// Token: 0x06003E52 RID: 15954 RVA: 0x001271ED File Offset: 0x001253ED
		public void InitializeLayers(Transform parent)
		{
			if (!this.layersStored)
			{
				this.StoreOriginalLayers(parent);
				this.layersStored = true;
			}
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x00127208 File Offset: 0x00125408
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

		// Token: 0x06003E54 RID: 15956 RVA: 0x00127284 File Offset: 0x00125484
		public void ChangeLayer(Transform parent, string newLayer)
		{
			if (!this.layersStored)
			{
				Debug.LogWarning("Layers have not been initialized. Call InitializeLayers first.");
				return;
			}
			this.ChangeLayers(parent, LayerMask.NameToLayer(newLayer));
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x001272A8 File Offset: 0x001254A8
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

		// Token: 0x06003E56 RID: 15958 RVA: 0x00127350 File Offset: 0x00125550
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

		// Token: 0x04003F83 RID: 16259
		public LayerMask restrictedLayers;

		// Token: 0x04003F84 RID: 16260
		public bool includeChildren = true;

		// Token: 0x04003F85 RID: 16261
		private Dictionary<Transform, int> originalLayers = new Dictionary<Transform, int>();

		// Token: 0x04003F86 RID: 16262
		private bool layersStored;
	}
}
