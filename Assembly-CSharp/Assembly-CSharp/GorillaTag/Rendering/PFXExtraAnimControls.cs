using System;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C0C RID: 3084
	public class PFXExtraAnimControls : MonoBehaviour
	{
		// Token: 0x06004D13 RID: 19731 RVA: 0x00176F08 File Offset: 0x00175108
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

		// Token: 0x06004D14 RID: 19732 RVA: 0x00176FE8 File Offset: 0x001751E8
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

		// Token: 0x04004F60 RID: 20320
		public float emitRateMult = 1f;

		// Token: 0x04004F61 RID: 20321
		public float emitBurstProbabilityMult = 1f;

		// Token: 0x04004F62 RID: 20322
		[SerializeField]
		private ParticleSystem[] particleSystems;

		// Token: 0x04004F63 RID: 20323
		private ParticleSystem.EmissionModule[] emissionModules;

		// Token: 0x04004F64 RID: 20324
		private ParticleSystem.Burst[][] cachedEmitBursts;

		// Token: 0x04004F65 RID: 20325
		private ParticleSystem.Burst[][] adjustedEmitBursts;
	}
}
