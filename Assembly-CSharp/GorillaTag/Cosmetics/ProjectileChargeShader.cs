using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C51 RID: 3153
	public class ProjectileChargeShader : MonoBehaviour
	{
		// Token: 0x06004E9A RID: 20122 RVA: 0x00181FF4 File Offset: 0x001801F4
		private void Awake()
		{
			this.renderer = base.GetComponentInChildren<Renderer>();
			this.chargerMPB = new MaterialPropertyBlock();
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x00182010 File Offset: 0x00180210
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

		// Token: 0x04005226 RID: 21030
		private Renderer renderer;

		// Token: 0x04005227 RID: 21031
		private MaterialPropertyBlock chargerMPB;

		// Token: 0x04005228 RID: 21032
		private static readonly int UvShiftOffset = Shader.PropertyToID("_UvShiftOffset");

		// Token: 0x04005229 RID: 21033
		public int shaderAnimSteps = 4;
	}
}
