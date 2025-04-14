using System;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class CornOnCobCosmetic : MonoBehaviour
{
	// Token: 0x060004FA RID: 1274 RVA: 0x0001DD10 File Offset: 0x0001BF10
	protected void Awake()
	{
		this.emissionModule = this.particleSys.emission;
		this.maxBurstProbability = ((this.emissionModule.burstCount > 0) ? this.emissionModule.GetBurst(0).probability : 0.2f);
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0001DD60 File Offset: 0x0001BF60
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

	// Token: 0x040005CE RID: 1486
	[Tooltip("The corn will start popping based on the temperature from this ThermalReceiver.")]
	public ThermalReceiver thermalReceiver;

	// Token: 0x040005CF RID: 1487
	[Tooltip("The particle system that will be emitted when the heat source is hot enough.")]
	public ParticleSystem particleSys;

	// Token: 0x040005D0 RID: 1488
	[Tooltip("The curve that determines how many particles will be emitted based on the heat source's temperature.\n\nThe x-axis is the heat source's temperature and the y-axis is the number of particles to emit.")]
	public AnimationCurve particleEmissionCurve;

	// Token: 0x040005D1 RID: 1489
	public SoundBankPlayer soundBankPlayer;

	// Token: 0x040005D2 RID: 1490
	private ParticleSystem.EmissionModule emissionModule;

	// Token: 0x040005D3 RID: 1491
	private float maxBurstProbability;

	// Token: 0x040005D4 RID: 1492
	private int previousParticleCount;
}
