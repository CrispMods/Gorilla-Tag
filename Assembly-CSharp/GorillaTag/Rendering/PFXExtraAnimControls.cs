using System;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C09 RID: 3081
	public class PFXExtraAnimControls : MonoBehaviour
	{
		// Token: 0x06004D07 RID: 19719 RVA: 0x00176940 File Offset: 0x00174B40
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

		// Token: 0x06004D08 RID: 19720 RVA: 0x00176A20 File Offset: 0x00174C20
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

		// Token: 0x04004F4E RID: 20302
		public float emitRateMult = 1f;

		// Token: 0x04004F4F RID: 20303
		public float emitBurstProbabilityMult = 1f;

		// Token: 0x04004F50 RID: 20304
		[SerializeField]
		private ParticleSystem[] particleSystems;

		// Token: 0x04004F51 RID: 20305
		private ParticleSystem.EmissionModule[] emissionModules;

		// Token: 0x04004F52 RID: 20306
		private ParticleSystem.Burst[][] cachedEmitBursts;

		// Token: 0x04004F53 RID: 20307
		private ParticleSystem.Burst[][] adjustedEmitBursts;
	}
}
