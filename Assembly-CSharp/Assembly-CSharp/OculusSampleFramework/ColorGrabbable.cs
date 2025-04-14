﻿using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A40 RID: 2624
	public class ColorGrabbable : OVRGrabbable
	{
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x0600415D RID: 16733 RVA: 0x00135F5A File Offset: 0x0013415A
		// (set) Token: 0x0600415E RID: 16734 RVA: 0x00135F62 File Offset: 0x00134162
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

		// Token: 0x0600415F RID: 16735 RVA: 0x00135F71 File Offset: 0x00134171
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

		// Token: 0x06004160 RID: 16736 RVA: 0x00135FA7 File Offset: 0x001341A7
		public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
		{
			base.GrabBegin(hand, grabPoint);
			this.UpdateColor();
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x00135FB7 File Offset: 0x001341B7
		public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
		{
			base.GrabEnd(linearVelocity, angularVelocity);
			this.UpdateColor();
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x00135FC8 File Offset: 0x001341C8
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

		// Token: 0x06004163 RID: 16739 RVA: 0x0013607C File Offset: 0x0013427C
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

		// Token: 0x04004286 RID: 17030
		public static readonly Color COLOR_GRAB = new Color(1f, 0.5f, 0f, 1f);

		// Token: 0x04004287 RID: 17031
		public static readonly Color COLOR_HIGHLIGHT = new Color(1f, 0f, 1f, 1f);

		// Token: 0x04004288 RID: 17032
		private Color m_color = Color.black;

		// Token: 0x04004289 RID: 17033
		private MeshRenderer[] m_meshRenderers;

		// Token: 0x0400428A RID: 17034
		private bool m_highlight;
	}
}
