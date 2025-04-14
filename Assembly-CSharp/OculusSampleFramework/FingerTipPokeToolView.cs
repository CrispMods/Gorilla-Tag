using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A59 RID: 2649
	public class FingerTipPokeToolView : MonoBehaviour, InteractableToolView
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060041F9 RID: 16889 RVA: 0x00138090 File Offset: 0x00136290
		// (set) Token: 0x060041FA RID: 16890 RVA: 0x00138098 File Offset: 0x00136298
		public InteractableTool InteractableTool { get; set; }

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060041FB RID: 16891 RVA: 0x001380A1 File Offset: 0x001362A1
		// (set) Token: 0x060041FC RID: 16892 RVA: 0x001380AE File Offset: 0x001362AE
		public bool EnableState
		{
			get
			{
				return this._sphereMeshRenderer.enabled;
			}
			set
			{
				this._sphereMeshRenderer.enabled = value;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x060041FD RID: 16893 RVA: 0x001380BC File Offset: 0x001362BC
		// (set) Token: 0x060041FE RID: 16894 RVA: 0x001380C4 File Offset: 0x001362C4
		public bool ToolActivateState { get; set; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x060041FF RID: 16895 RVA: 0x001380CD File Offset: 0x001362CD
		// (set) Token: 0x06004200 RID: 16896 RVA: 0x001380D5 File Offset: 0x001362D5
		public float SphereRadius { get; private set; }

		// Token: 0x06004201 RID: 16897 RVA: 0x001380DE File Offset: 0x001362DE
		private void Awake()
		{
			this.SphereRadius = this._sphereMeshRenderer.transform.localScale.z * 0.5f;
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x000023F4 File Offset: 0x000005F4
		public void SetFocusedInteractable(Interactable interactable)
		{
		}

		// Token: 0x04004306 RID: 17158
		[SerializeField]
		private MeshRenderer _sphereMeshRenderer;
	}
}
