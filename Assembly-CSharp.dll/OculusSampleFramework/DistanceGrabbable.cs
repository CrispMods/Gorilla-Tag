using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A41 RID: 2625
	public class DistanceGrabbable : OVRGrabbable
	{
		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06004166 RID: 16742 RVA: 0x00059E71 File Offset: 0x00058071
		// (set) Token: 0x06004167 RID: 16743 RVA: 0x00059E79 File Offset: 0x00058079
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

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06004168 RID: 16744 RVA: 0x00059E88 File Offset: 0x00058088
		// (set) Token: 0x06004169 RID: 16745 RVA: 0x00059E90 File Offset: 0x00058090
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

		// Token: 0x0600416A RID: 16746 RVA: 0x0016ECA4 File Offset: 0x0016CEA4
		protected override void Start()
		{
			base.Start();
			this.m_crosshair = base.gameObject.GetComponentInChildren<GrabbableCrosshair>();
			this.m_renderer = base.gameObject.GetComponent<Renderer>();
			this.m_crosshairManager = UnityEngine.Object.FindObjectOfType<GrabManager>();
			this.m_mpb = new MaterialPropertyBlock();
			this.RefreshCrosshair();
			this.m_renderer.SetPropertyBlock(this.m_mpb);
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x0016ED08 File Offset: 0x0016CF08
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

		// Token: 0x0400428B RID: 17035
		public string m_materialColorField;

		// Token: 0x0400428C RID: 17036
		private GrabbableCrosshair m_crosshair;

		// Token: 0x0400428D RID: 17037
		private GrabManager m_crosshairManager;

		// Token: 0x0400428E RID: 17038
		private Renderer m_renderer;

		// Token: 0x0400428F RID: 17039
		private MaterialPropertyBlock m_mpb;

		// Token: 0x04004290 RID: 17040
		private bool m_inRange;

		// Token: 0x04004291 RID: 17041
		private bool m_targeted;
	}
}
