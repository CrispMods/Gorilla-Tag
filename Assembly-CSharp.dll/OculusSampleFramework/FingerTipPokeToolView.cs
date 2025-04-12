using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A5C RID: 2652
	public class FingerTipPokeToolView : MonoBehaviour, InteractableToolView
	{
		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06004205 RID: 16901 RVA: 0x0005A397 File Offset: 0x00058597
		// (set) Token: 0x06004206 RID: 16902 RVA: 0x0005A39F File Offset: 0x0005859F
		public InteractableTool InteractableTool { get; set; }

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06004207 RID: 16903 RVA: 0x0005A3A8 File Offset: 0x000585A8
		// (set) Token: 0x06004208 RID: 16904 RVA: 0x0005A3B5 File Offset: 0x000585B5
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
		// (get) Token: 0x06004209 RID: 16905 RVA: 0x0005A3C3 File Offset: 0x000585C3
		// (set) Token: 0x0600420A RID: 16906 RVA: 0x0005A3CB File Offset: 0x000585CB
		public bool ToolActivateState { get; set; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x0005A3D4 File Offset: 0x000585D4
		// (set) Token: 0x0600420C RID: 16908 RVA: 0x0005A3DC File Offset: 0x000585DC
		public float SphereRadius { get; private set; }

		// Token: 0x0600420D RID: 16909 RVA: 0x0005A3E5 File Offset: 0x000585E5
		private void Awake()
		{
			this.SphereRadius = this._sphereMeshRenderer.transform.localScale.z * 0.5f;
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void SetFocusedInteractable(Interactable interactable)
		{
		}

		// Token: 0x04004318 RID: 17176
		[SerializeField]
		private MeshRenderer _sphereMeshRenderer;
	}
}
