using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A3E RID: 2622
	public class DistanceGrabbable : OVRGrabbable
	{
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x0600415A RID: 16730 RVA: 0x00135B4F File Offset: 0x00133D4F
		// (set) Token: 0x0600415B RID: 16731 RVA: 0x00135B57 File Offset: 0x00133D57
		public bool InRange
		{
			get
			{
				return this.m_inRange;
			}
			set
			{
				this.m_inRange = value;
				this.RefreshCrosshair();
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x0600415C RID: 16732 RVA: 0x00135B66 File Offset: 0x00133D66
		// (set) Token: 0x0600415D RID: 16733 RVA: 0x00135B6E File Offset: 0x00133D6E
		public bool Targeted
		{
			get
			{
				return this.m_targeted;
			}
			set
			{
				this.m_targeted = value;
				this.RefreshCrosshair();
			}
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x00135B80 File Offset: 0x00133D80
		protected override void Start()
		{
			base.Start();
			this.m_crosshair = base.gameObject.GetComponentInChildren<GrabbableCrosshair>();
			this.m_renderer = base.gameObject.GetComponent<Renderer>();
			this.m_crosshairManager = Object.FindObjectOfType<GrabManager>();
			this.m_mpb = new MaterialPropertyBlock();
			this.RefreshCrosshair();
			this.m_renderer.SetPropertyBlock(this.m_mpb);
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x00135BE4 File Offset: 0x00133DE4
		private void RefreshCrosshair()
		{
			if (this.m_crosshair)
			{
				if (base.isGrabbed)
				{
					this.m_crosshair.SetState(GrabbableCrosshair.CrosshairState.Disabled);
				}
				else if (!this.InRange)
				{
					this.m_crosshair.SetState(GrabbableCrosshair.CrosshairState.Disabled);
				}
				else
				{
					this.m_crosshair.SetState(this.Targeted ? GrabbableCrosshair.CrosshairState.Targeted : GrabbableCrosshair.CrosshairState.Enabled);
				}
			}
			if (this.m_materialColorField != null)
			{
				this.m_renderer.GetPropertyBlock(this.m_mpb);
				if (base.isGrabbed || !this.InRange)
				{
					this.m_mpb.SetColor(this.m_materialColorField, this.m_crosshairManager.OutlineColorOutOfRange);
				}
				else if (this.Targeted)
				{
					this.m_mpb.SetColor(this.m_materialColorField, this.m_crosshairManager.OutlineColorHighlighted);
				}
				else
				{
					this.m_mpb.SetColor(this.m_materialColorField, this.m_crosshairManager.OutlineColorInRange);
				}
				this.m_renderer.SetPropertyBlock(this.m_mpb);
			}
		}

		// Token: 0x04004279 RID: 17017
		public string m_materialColorField;

		// Token: 0x0400427A RID: 17018
		private GrabbableCrosshair m_crosshair;

		// Token: 0x0400427B RID: 17019
		private GrabManager m_crosshairManager;

		// Token: 0x0400427C RID: 17020
		private Renderer m_renderer;

		// Token: 0x0400427D RID: 17021
		private MaterialPropertyBlock m_mpb;

		// Token: 0x0400427E RID: 17022
		private bool m_inRange;

		// Token: 0x0400427F RID: 17023
		private bool m_targeted;
	}
}
