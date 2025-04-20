using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A6A RID: 2666
	public class ColorGrabbable : OVRGrabbable
	{
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06004296 RID: 17046 RVA: 0x0005B7B5 File Offset: 0x000599B5
		// (set) Token: 0x06004297 RID: 17047 RVA: 0x0005B7BD File Offset: 0x000599BD
		public bool Highlight
		{
			get
			{
				return this.m_highlight;
			}
			set
			{
				this.m_highlight = value;
				this.UpdateColor();
			}
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x0005B7CC File Offset: 0x000599CC
		protected void UpdateColor()
		{
			if (base.isGrabbed)
			{
				this.SetColor(ColorGrabbable.COLOR_GRAB);
				return;
			}
			if (this.Highlight)
			{
				this.SetColor(ColorGrabbable.COLOR_HIGHLIGHT);
				return;
			}
			this.SetColor(this.m_color);
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x0005B802 File Offset: 0x00059A02
		public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
		{
			base.GrabBegin(hand, grabPoint);
			this.UpdateColor();
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x0005B812 File Offset: 0x00059A12
		public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
		{
			base.GrabEnd(linearVelocity, angularVelocity);
			this.UpdateColor();
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x00175A28 File Offset: 0x00173C28
		private void Awake()
		{
			if (this.m_grabPoints.Length == 0)
			{
				Collider component = base.GetComponent<Collider>();
				if (component == null)
				{
					throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
				}
				this.m_grabPoints = new Collider[]
				{
					component
				};
				this.m_meshRenderers = new MeshRenderer[1];
				this.m_meshRenderers[0] = base.GetComponent<MeshRenderer>();
			}
			else
			{
				this.m_meshRenderers = base.GetComponentsInChildren<MeshRenderer>();
			}
			this.m_color = new Color(UnityEngine.Random.Range(0.1f, 0.95f), UnityEngine.Random.Range(0.1f, 0.95f), UnityEngine.Random.Range(0.1f, 0.95f), 1f);
			this.SetColor(this.m_color);
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x00175ADC File Offset: 0x00173CDC
		private void SetColor(Color color)
		{
			for (int i = 0; i < this.m_meshRenderers.Length; i++)
			{
				MeshRenderer meshRenderer = this.m_meshRenderers[i];
				for (int j = 0; j < meshRenderer.materials.Length; j++)
				{
					meshRenderer.materials[j].color = color;
				}
			}
		}

		// Token: 0x0400436E RID: 17262
		public static readonly Color COLOR_GRAB = new Color(1f, 0.5f, 0f, 1f);

		// Token: 0x0400436F RID: 17263
		public static readonly Color COLOR_HIGHLIGHT = new Color(1f, 0f, 1f, 1f);

		// Token: 0x04004370 RID: 17264
		private Color m_color = Color.black;

		// Token: 0x04004371 RID: 17265
		private MeshRenderer[] m_meshRenderers;

		// Token: 0x04004372 RID: 17266
		private bool m_highlight;
	}
}
