using System;
using UnityEngine;

// Token: 0x020008B0 RID: 2224
public class SurfaceImpactFX : MonoBehaviour
{
	// Token: 0x060035D9 RID: 13785 RVA: 0x000FF290 File Offset: 0x000FD490
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

	// Token: 0x060035DA RID: 13786 RVA: 0x000FF2E9 File Offset: 0x000FD4E9
	public void SetScale(float scale)
	{
		this.fxMainModule.gravityModifierMultiplier = this.startingGravityModifier * scale;
		base.transform.localScale = this.startingScale * scale;
	}

	// Token: 0x04003809 RID: 14345
	public ParticleSystem particleFX;

	// Token: 0x0400380A RID: 14346
	public float startingGravityModifier;

	// Token: 0x0400380B RID: 14347
	public Vector3 startingScale = Vector3.one;

	// Token: 0x0400380C RID: 14348
	private ParticleSystem.MainModule fxMainModule;
}
