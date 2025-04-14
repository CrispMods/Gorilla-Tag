using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class PlayerSpeedBasedAudio : MonoBehaviour
{
	// Token: 0x06000834 RID: 2100 RVA: 0x0002D08B File Offset: 0x0002B28B
	private void Start()
	{
		this.fadeRate = 1f / this.fadeTime;
		this.baseVolume = this.audioSource.volume;
		this.localPlayerVelocityEstimator.TryResolve<GorillaVelocityEstimator>(out this.velocityEstimator);
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0002D0C4 File Offset: 0x0002B2C4
	private void Update()
	{
		this.currentFadeLevel = Mathf.MoveTowards(this.currentFadeLevel, Mathf.InverseLerp(this.minVolumeSpeed, this.fullVolumeSpeed, this.velocityEstimator.linearVelocity.magnitude), this.fadeRate * Time.deltaTime);
		if (this.baseVolume == 0f || this.currentFadeLevel == 0f)
		{
			this.audioSource.volume = 0.0001f;
			return;
		}
		this.audioSource.volume = this.baseVolume * this.currentFadeLevel;
	}

	// Token: 0x0400099B RID: 2459
	[SerializeField]
	private float minVolumeSpeed;

	// Token: 0x0400099C RID: 2460
	[SerializeField]
	private float fullVolumeSpeed;

	// Token: 0x0400099D RID: 2461
	[SerializeField]
	private float fadeTime;

	// Token: 0x0400099E RID: 2462
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x0400099F RID: 2463
	[SerializeField]
	private XSceneRef localPlayerVelocityEstimator;

	// Token: 0x040009A0 RID: 2464
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x040009A1 RID: 2465
	private float baseVolume;

	// Token: 0x040009A2 RID: 2466
	private float fadeRate;

	// Token: 0x040009A3 RID: 2467
	private float currentFadeLevel;
}
