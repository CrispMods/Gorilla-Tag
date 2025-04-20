using System;
using UnityEngine;

// Token: 0x020008CC RID: 2252
public class SurfaceImpactFX : MonoBehaviour
{
	// Token: 0x060036A1 RID: 13985 RVA: 0x00144F58 File Offset: 0x00143158
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

	// Token: 0x060036A2 RID: 13986 RVA: 0x0005407F File Offset: 0x0005227F
	public void SetScale(float scale)
	{
		this.fxMainModule.gravityModifierMultiplier = this.startingGravityModifier * scale;
		base.transform.localScale = this.startingScale * scale;
	}

	// Token: 0x040038CA RID: 14538
	public ParticleSystem particleFX;

	// Token: 0x040038CB RID: 14539
	public float startingGravityModifier;

	// Token: 0x040038CC RID: 14540
	public Vector3 startingScale = Vector3.one;

	// Token: 0x040038CD RID: 14541
	private ParticleSystem.MainModule fxMainModule;
}
