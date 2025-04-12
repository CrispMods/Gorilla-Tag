using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C54 RID: 3156
	public class ProjectileChargeShader : MonoBehaviour
	{
		// Token: 0x06004EA6 RID: 20134 RVA: 0x0006299B File Offset: 0x00060B9B
		private void Awake()
		{
			this.renderer = base.GetComponentInChildren<Renderer>();
			this.chargerMPB = new MaterialPropertyBlock();
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x001B2284 File Offset: 0x001B0484
		public void UpdateChargeProgress(float value)
		{
			if (this.chargerMPB == null)
			{
				this.chargerMPB = new MaterialPropertyBlock();
			}
			if (this.renderer)
			{
				this.renderer.GetPropertyBlock(this.chargerMPB, 1);
				this.chargerMPB.SetVector(ProjectileChargeShader.UvShiftOffset, new Vector2(value, 0f));
				this.renderer.SetPropertyBlock(this.chargerMPB, 1);
			}
		}

		// Token: 0x04005238 RID: 21048
		private Renderer renderer;

		// Token: 0x04005239 RID: 21049
		private MaterialPropertyBlock chargerMPB;

		// Token: 0x0400523A RID: 21050
		private static readonly int UvShiftOffset = Shader.PropertyToID("_UvShiftOffset");

		// Token: 0x0400523B RID: 21051
		public int shaderAnimSteps = 4;
	}
}
