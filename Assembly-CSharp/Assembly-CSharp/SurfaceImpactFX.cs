using System;
using UnityEngine;

// Token: 0x020008B3 RID: 2227
public class SurfaceImpactFX : MonoBehaviour
{
	// Token: 0x060035E5 RID: 13797 RVA: 0x000FF858 File Offset: 0x000FDA58
	public void Awake()
	{
		if (this.particleFX == null)
		{
			this.particleFX = base.GetComponent<ParticleSystem>();
		}
		if (this.particleFX == null)
		{
			Debug.LogError("SurfaceImpactFX: No ParticleSystem found! Disabling component.", this);
			base.enabled = false;
			return;
		}
		this.fxMainModule = this.particleFX.main;
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x000FF8B1 File Offset: 0x000FDAB1
	public void SetScale(float scale)
	{
		this.fxMainModule.gravityModifierMultiplier = this.startingGravityModifier * scale;
		base.transform.localScale = this.startingScale * scale;
	}

	// Token: 0x0400381B RID: 14363
	public ParticleSystem particleFX;

	// Token: 0x0400381C RID: 14364
	public float startingGravityModifier;

	// Token: 0x0400381D RID: 14365
	public Vector3 startingScale = Vector3.one;

	// Token: 0x0400381E RID: 14366
	private ParticleSystem.MainModule fxMainModule;
}
