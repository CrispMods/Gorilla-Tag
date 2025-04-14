using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A5C RID: 2652
	public class FingerTipPokeToolView : MonoBehaviour, InteractableToolView
	{
		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06004205 RID: 16901 RVA: 0x00138658 File Offset: 0x00136858
		// (set) Token: 0x06004206 RID: 16902 RVA: 0x00138660 File Offset: 0x00136860
		public InteractableTool InteractableTool { get; set; }

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06004207 RID: 16903 RVA: 0x00138669 File Offset: 0x00136869
		// (set) Token: 0x06004208 RID: 16904 RVA: 0x00138676 File Offset: 0x00136876
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

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06004209 RID: 16905 RVA: 0x00138684 File Offset: 0x00136884
		// (set) Token: 0x0600420A RID: 16906 RVA: 0x0013868C File Offset: 0x0013688C
		public bool ToolActivateState { get; set; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x00138695 File Offset: 0x00136895
		// (set) Token: 0x0600420C RID: 16908 RVA: 0x0013869D File Offset: 0x0013689D
		public float SphereRadius { get; private set; }

		// Token: 0x0600420D RID: 16909 RVA: 0x001386A6 File Offset: 0x001368A6
		private void Awake()
		{
			this.SphereRadius = this._sphereMeshRenderer.transform.localScale.z * 0.5f;
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x000023F4 File Offset: 0x000005F4
		public void SetFocusedInteractable(Interactable interactable)
		{
		}

		// Token: 0x04004318 RID: 17176
		[SerializeField]
		private MeshRenderer _sphereMeshRenderer;
	}
}
