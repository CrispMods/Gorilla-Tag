using System;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class CosmeticCritterCatcherButterflyNet : CosmeticCritterCatcher
{
	// Token: 0x060003E8 RID: 1000 RVA: 0x0007A64C File Offset: 0x0007884C
	public override bool TryToCatch(CosmeticCritter critter)
	{
		return critter is CosmeticCritterButterfly && (critter.transform.position - this.velocityEstimator.transform.position).sqrMagnitude <= this.maxCatchRadius * this.maxCatchRadius && this.velocityEstimator.linearVelocity.sqrMagnitude >= this.minCatchSpeed * this.minCatchSpeed;
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x00032EFC File Offset: 0x000310FC
	public override void Catch(CosmeticCritter critter)
	{
		this.caughtButterflyParticleSystem.Emit((critter as CosmeticCritterButterfly).GetEmitParams, 1);
		this.catchFX.Play();
		this.catchSFX.Play();
	}

	// Token: 0x04000466 RID: 1126
	[SerializeField]
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000467 RID: 1127
	[SerializeField]
	private float maxCatchRadius;

	// Token: 0x04000468 RID: 1128
	[SerializeField]
	private float minCatchSpeed;

	// Token: 0x04000469 RID: 1129
	[SerializeField]
	private ParticleSystem caughtButterflyParticleSystem;

	// Token: 0x0400046A RID: 1130
	[SerializeField]
	private ParticleSystem catchFX;

	// Token: 0x0400046B RID: 1131
	[SerializeField]
	private AudioSource catchSFX;
}
