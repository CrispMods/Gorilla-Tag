using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaNetworking
{
	// Token: 0x02000AB6 RID: 2742
	[Serializable]
	public class GorillaText
	{
		// Token: 0x060044DF RID: 17631 RVA: 0x001474BC File Offset: 0x001456BC
		public void Initialize(Material[] originalMaterials, Material failureMaterial, UnityEvent<string> callback = null, UnityEvent<Material[]> materialCallback = null)
		{
			this.failureMaterial = failureMaterial;
			this.originalMaterials = originalMaterials;
			this.currentMaterials = originalMaterials;
			Debug.Log("Original text = " + this.originalText);
			this.updateTextCallback = callback;
			this.updateMaterialCallback = materialCallback;
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060044E0 RID: 17632 RVA: 0x001474F7 File Offset: 0x001456F7
		// (set) Token: 0x060044E1 RID: 17633 RVA: 0x001474FF File Offset: 0x001456FF
		public string Text
		{
			get
			{
				return this.originalText;
			}
			set
			{
				if (this.originalText == value)
				{
					return;
				}
				this.originalText = value;
				if (!this.failedState)
				{
					UnityEvent<string> unityEvent = this.updateTextCallback;
					if (unityEvent == null)
					{
						return;
					}
					unityEvent.Invoke(value);
				}
			}
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x00147530 File Offset: 0x00145730
		public void EnableFailedState(string failText)
		{
			this.failedState = true;
			this.failureText = failText;
			UnityEvent<string> unityEvent = this.updateTextCallback;
			if (unityEvent != null)
			{
				unityEvent.Invoke(failText);
			}
			this.currentMaterials = (Material[])this.originalMaterials.Clone();
			this.currentMaterials[0] = this.failureMaterial;
			UnityEvent<Material[]> unityEvent2 = this.updateMaterialCallback;
			if (unityEvent2 == null)
			{
				return;
			}
			unityEvent2.Invoke(this.currentMaterials);
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x00147598 File Offset: 0x00145798
		public void DisableFailedState()
		{
			this.failedState = true;
			UnityEvent<string> unityEvent = this.updateTextCallback;
			if (unityEvent != null)
			{
				unityEvent.Invoke(this.originalText);
			}
			this.failureText = "";
			this.currentMaterials = this.originalMaterials;
			UnityEvent<Material[]> unityEvent2 = this.updateMaterialCallback;
			if (unityEvent2 == null)
			{
				return;
			}
			unityEvent2.Invoke(this.currentMaterials);
		}

		// Token: 0x04004654 RID: 18004
		private string failureText;

		// Token: 0x04004655 RID: 18005
		private string originalText = string.Empty;

		// Token: 0x04004656 RID: 18006
		private bool failedState;

		// Token: 0x04004657 RID: 18007
		private Material[] originalMaterials;

		// Token: 0x04004658 RID: 18008
		private Material failureMaterial;

		// Token: 0x04004659 RID: 18009
		internal Material[] currentMaterials;

		// Token: 0x0400465A RID: 18010
		private UnityEvent<string> updateTextCallback;

		// Token: 0x0400465B RID: 18011
		private UnityEvent<Material[]> updateMaterialCallback;
	}
}
