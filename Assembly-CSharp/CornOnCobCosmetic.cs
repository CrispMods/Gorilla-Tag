using System;
using UnityEngine;

// Token: 0x020000CA RID: 202
public class CornOnCobCosmetic : MonoBehaviour
{
	// Token: 0x06000534 RID: 1332 RVA: 0x00080CDC File Offset: 0x0007EEDC
	protected void Awake()
	{
		this.emissionModule = this.particleSys.emission;
		this.maxBurstProbability = ((this.emissionModule.burstCount > 0) ? this.emissionModule.GetBurst(0).probability : 0.2f);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00080D2C File Offset: 0x0007EF2C
	protected void LateUpdate()
	{
		for (int i = 0; i < this.emissionModule.burstCount; i++)
		{
			ParticleSystem.Burst burst = this.emissionModule.GetBurst(i);
			burst.probability = this.maxBurstProbability * this.particleEmissionCurve.Evaluate(this.thermalReceiver.celsius);
			this.emissionModule.SetBurst(i, burst);
		}
		int particleCount = this.particleSys.particleCount;
		if (particleCount > this.previousParticleCount)
		{
			this.soundBankPlayer.Play();
		}
		this.previousParticleCount = particleCount;
	}

	// Token: 0x0400060D RID: 1549
	[Tooltip("The corn will start popping based on the temperature from this ThermalReceiver.")]
	public ThermalReceiver thermalReceiver;

	// Token: 0x0400060E RID: 1550
	[Tooltip("The particle system that will be emitted when the heat source is hot enough.")]
	public ParticleSystem particleSys;

	// Token: 0x0400060F RID: 1551
	[Tooltip("The curve that determines how many particles will be emitted based on the heat source's temperature.\n\nThe x-axis is the heat source's temperature and the y-axis is the number of particles to emit.")]
	public AnimationCurve particleEmissionCurve;

	// Token: 0x04000610 RID: 1552
	public SoundBankPlayer soundBankPlayer;

	// Token: 0x04000611 RID: 1553
	private ParticleSystem.EmissionModule emissionModule;

	// Token: 0x04000612 RID: 1554
	private float maxBurstProbability;

	// Token: 0x04000613 RID: 1555
	private int previousParticleCount;
}
