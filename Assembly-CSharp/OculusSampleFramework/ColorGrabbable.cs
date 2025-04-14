using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A3D RID: 2621
	public class ColorGrabbable : OVRGrabbable
	{
		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x00135992 File Offset: 0x00133B92
		// (set) Token: 0x06004152 RID: 16722 RVA: 0x0013599A File Offset: 0x00133B9A
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

		// Token: 0x06004153 RID: 16723 RVA: 0x001359A9 File Offset: 0x00133BA9
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

		// Token: 0x06004154 RID: 16724 RVA: 0x001359DF File Offset: 0x00133BDF
		public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
		{
			base.GrabBegin(hand, grabPoint);
			this.UpdateColor();
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x001359EF File Offset: 0x00133BEF
		public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
		{
			base.GrabEnd(linearVelocity, angularVelocity);
			this.UpdateColor();
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x00135A00 File Offset: 0x00133C00
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
			this.m_color = new Color(Random.Range(0.1f, 0.95f), Random.Range(0.1f, 0.95f), Random.Range(0.1f, 0.95f), 1f);
			this.SetColor(this.m_color);
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x00135AB4 File Offset: 0x00133CB4
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

		// Token: 0x04004274 RID: 17012
		public static readonly Color COLOR_GRAB = new Color(1f, 0.5f, 0f, 1f);

		// Token: 0x04004275 RID: 17013
		public static readonly Color COLOR_HIGHLIGHT = new Color(1f, 0f, 1f, 1f);

		// Token: 0x04004276 RID: 17014
		private Color m_color = Color.black;

		// Token: 0x04004277 RID: 17015
		private MeshRenderer[] m_meshRenderers;

		// Token: 0x04004278 RID: 17016
		private bool m_highlight;
	}
}
