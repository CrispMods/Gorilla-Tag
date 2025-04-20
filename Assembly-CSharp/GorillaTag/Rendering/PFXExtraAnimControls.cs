using System;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C37 RID: 3127
	public class PFXExtraAnimControls : MonoBehaviour
	{
		// Token: 0x06004E53 RID: 20051 RVA: 0x001AECA8 File Offset: 0x001ACEA8
		protected void Awake()
		{
			this.emissionModules = new ParticleSystem.EmissionModule[this.particleSystems.Length];
			this.cachedEmitBursts = new ParticleSystem.Burst[this.particleSystems.Length][];
			this.adjustedEmitBursts = new ParticleSystem.Burst[this.particleSystems.Length][];
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				ParticleSystem.EmissionModule emission = this.particleSystems[i].emission;
				this.cachedEmitBursts[i] = new ParticleSystem.Burst[emission.burstCount];
				this.adjustedEmitBursts[i] = new ParticleSystem.Burst[emission.burstCount];
				for (int j = 0; j < emission.burstCount; j++)
				{
					this.cachedEmitBursts[i][j] = emission.GetBurst(j);
					this.adjustedEmitBursts[i][j] = emission.GetBurst(j);
				}
				this.emissionModules[i] = emission;
			}
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x001AED88 File Offset: 0x001ACF88
		protected void LateUpdate()
		{
			for (int i = 0; i < this.emissionModules.Length; i++)
			{
				this.emissionModules[i].rateOverTimeMultiplier = this.emitRateMult;
				Mathf.Min(this.emissionModules[i].burstCount, this.cachedEmitBursts[i].Length);
				for (int j = 0; j < this.cachedEmitBursts[i].Length; j++)
				{
					this.adjustedEmitBursts[i][j].probability = this.cachedEmitBursts[i][j].probability * this.emitBurstProbabilityMult;
				}
				this.emissionModules[i].SetBursts(this.adjustedEmitBursts[i]);
			}
		}

		// Token: 0x04005044 RID: 20548
		public float emitRateMult = 1f;

		// Token: 0x04005045 RID: 20549
		public float emitBurstProbabilityMult = 1f;

		// Token: 0x04005046 RID: 20550
		[SerializeField]
		private ParticleSystem[] particleSystems;

		// Token: 0x04005047 RID: 20551
		private ParticleSystem.EmissionModule[] emissionModules;

		// Token: 0x04005048 RID: 20552
		private ParticleSystem.Burst[][] cachedEmitBursts;

		// Token: 0x04005049 RID: 20553
		private ParticleSystem.Burst[][] adjustedEmitBursts;
	}
}
