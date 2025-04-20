using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C82 RID: 3202
	public class ProjectileChargeShader : MonoBehaviour
	{
		// Token: 0x06004FFA RID: 20474 RVA: 0x000643C0 File Offset: 0x000625C0
		private void Awake()
		{
			this.renderer = base.GetComponentInChildren<Renderer>();
			this.chargerMPB = new MaterialPropertyBlock();
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x001BA368 File Offset: 0x001B8568
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

		// Token: 0x04005332 RID: 21298
		private Renderer renderer;

		// Token: 0x04005333 RID: 21299
		private MaterialPropertyBlock chargerMPB;

		// Token: 0x04005334 RID: 21300
		private static readonly int UvShiftOffset = Shader.PropertyToID("_UvShiftOffset");

		// Token: 0x04005335 RID: 21301
		public int shaderAnimSteps = 4;
	}
}
