using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A6B RID: 2667
	public class DistanceGrabbable : OVRGrabbable
	{
		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600429F RID: 17055 RVA: 0x0005B873 File Offset: 0x00059A73
		// (set) Token: 0x060042A0 RID: 17056 RVA: 0x0005B87B File Offset: 0x00059A7B
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

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060042A1 RID: 17057 RVA: 0x0005B88A File Offset: 0x00059A8A
		// (set) Token: 0x060042A2 RID: 17058 RVA: 0x0005B892 File Offset: 0x00059A92
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

		// Token: 0x060042A3 RID: 17059 RVA: 0x00175B28 File Offset: 0x00173D28
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

		// Token: 0x060042A4 RID: 17060 RVA: 0x00175B8C File Offset: 0x00173D8C
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

		// Token: 0x04004373 RID: 17267
		public string m_materialColorField;

		// Token: 0x04004374 RID: 17268
		private GrabbableCrosshair m_crosshair;

		// Token: 0x04004375 RID: 17269
		private GrabManager m_crosshairManager;

		// Token: 0x04004376 RID: 17270
		private Renderer m_renderer;

		// Token: 0x04004377 RID: 17271
		private MaterialPropertyBlock m_mpb;

		// Token: 0x04004378 RID: 17272
		private bool m_inRange;

		// Token: 0x04004379 RID: 17273
		private bool m_targeted;
	}
}
