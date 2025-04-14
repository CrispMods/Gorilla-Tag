using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaNetworking
{
	// Token: 0x02000AB9 RID: 2745
	[Serializable]
	public class GorillaText
	{
		// Token: 0x060044EB RID: 17643 RVA: 0x00147A84 File Offset: 0x00145C84
		public void Initialize(Material[] originalMaterials, Material failureMaterial, UnityEvent<string> callback = null, UnityEvent<Material[]> materialCallback = null)
		{
			this.failureMaterial = failureMaterial;
			this.originalMaterials = originalMaterials;
			this.currentMaterials = originalMaterials;
			Debug.Log("Original text = " + this.originalText);
			this.updateTextCallback = callback;
			this.updateMaterialCallback = materialCallback;
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060044EC RID: 17644 RVA: 0x00147ABF File Offset: 0x00145CBF
		// (set) Token: 0x060044ED RID: 17645 RVA: 0x00147AC7 File Offset: 0x00145CC7
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

		// Token: 0x060044EE RID: 17646 RVA: 0x00147AF8 File Offset: 0x00145CF8
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

		// Token: 0x060044EF RID: 17647 RVA: 0x00147B60 File Offset: 0x00145D60
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

		// Token: 0x04004666 RID: 18022
		private string failureText;

		// Token: 0x04004667 RID: 18023
		private string originalText = string.Empty;

		// Token: 0x04004668 RID: 18024
		private bool failedState;

		// Token: 0x04004669 RID: 18025
		private Material[] originalMaterials;

		// Token: 0x0400466A RID: 18026
		private Material failureMaterial;

		// Token: 0x0400466B RID: 18027
		internal Material[] currentMaterials;

		// Token: 0x0400466C RID: 18028
		private UnityEvent<string> updateTextCallback;

		// Token: 0x0400466D RID: 18029
		private UnityEvent<Material[]> updateMaterialCallback;
	}
}
