using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaNetworking
{
	// Token: 0x02000AE2 RID: 2786
	[Serializable]
	public class GorillaText
	{
		// Token: 0x06004622 RID: 17954 RVA: 0x0005DC3A File Offset: 0x0005BE3A
		public void Initialize(Material[] originalMaterials, Material failureMaterial, UnityEvent<string> callback = null, UnityEvent<Material[]> materialCallback = null)
		{
			this.failureMaterial = failureMaterial;
			this.originalMaterials = originalMaterials;
			this.currentMaterials = originalMaterials;
			Debug.Log("Original text = " + this.originalText);
			this.updateTextCallback = callback;
			this.updateMaterialCallback = materialCallback;
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06004623 RID: 17955 RVA: 0x0005DC75 File Offset: 0x0005BE75
		// (set) Token: 0x06004624 RID: 17956 RVA: 0x0005DC7D File Offset: 0x0005BE7D
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

		// Token: 0x06004625 RID: 17957 RVA: 0x00184FCC File Offset: 0x001831CC
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

		// Token: 0x06004626 RID: 17958 RVA: 0x00185034 File Offset: 0x00183234
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

		// Token: 0x0400474B RID: 18251
		private string failureText;

		// Token: 0x0400474C RID: 18252
		private string originalText = string.Empty;

		// Token: 0x0400474D RID: 18253
		private bool failedState;

		// Token: 0x0400474E RID: 18254
		private Material[] originalMaterials;

		// Token: 0x0400474F RID: 18255
		private Material failureMaterial;

		// Token: 0x04004750 RID: 18256
		internal Material[] currentMaterials;

		// Token: 0x04004751 RID: 18257
		private UnityEvent<string> updateTextCallback;

		// Token: 0x04004752 RID: 18258
		private UnityEvent<Material[]> updateMaterialCallback;
	}
}
