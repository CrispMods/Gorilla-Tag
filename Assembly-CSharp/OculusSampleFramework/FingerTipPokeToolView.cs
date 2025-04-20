using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A86 RID: 2694
	public class FingerTipPokeToolView : MonoBehaviour, InteractableToolView
	{
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x0600433E RID: 17214 RVA: 0x0005BD99 File Offset: 0x00059F99
		// (set) Token: 0x0600433F RID: 17215 RVA: 0x0005BDA1 File Offset: 0x00059FA1
		public InteractableTool InteractableTool { get; set; }

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06004340 RID: 17216 RVA: 0x0005BDAA File Offset: 0x00059FAA
		// (set) Token: 0x06004341 RID: 17217 RVA: 0x0005BDB7 File Offset: 0x00059FB7
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

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06004342 RID: 17218 RVA: 0x0005BDC5 File Offset: 0x00059FC5
		// (set) Token: 0x06004343 RID: 17219 RVA: 0x0005BDCD File Offset: 0x00059FCD
		public bool ToolActivateState { get; set; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06004344 RID: 17220 RVA: 0x0005BDD6 File Offset: 0x00059FD6
		// (set) Token: 0x06004345 RID: 17221 RVA: 0x0005BDDE File Offset: 0x00059FDE
		public float SphereRadius { get; private set; }

		// Token: 0x06004346 RID: 17222 RVA: 0x0005BDE7 File Offset: 0x00059FE7
		private void Awake()
		{
			this.SphereRadius = this._sphereMeshRenderer.transform.localScale.z * 0.5f;
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x00030607 File Offset: 0x0002E807
		public void SetFocusedInteractable(Interactable interactable)
		{
		}

		// Token: 0x04004400 RID: 17408
		[SerializeField]
		private MeshRenderer _sphereMeshRenderer;
	}
}
