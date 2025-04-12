using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class PlayerSpeedBasedAudio : MonoBehaviour
{
	// Token: 0x06000836 RID: 2102 RVA: 0x00034D08 File Offset: 0x00032F08
	private void Start()
	{
		this.fadeRate = 1f / this.fadeTime;
		this.baseVolume = this.audioSource.volume;
		this.localPlayerVelocityEstimator.TryResolve<GorillaVelocityEstimator>(out this.velocityEstimator);
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0008B9A0 File Offset: 0x00089BA0
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

	// Token: 0x0400099C RID: 2460
	[SerializeField]
	private float minVolumeSpeed;

	// Token: 0x0400099D RID: 2461
	[SerializeField]
	private float fullVolumeSpeed;

	// Token: 0x0400099E RID: 2462
	[SerializeField]
	private float fadeTime;

	// Token: 0x0400099F RID: 2463
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x040009A0 RID: 2464
	[SerializeField]
	private XSceneRef localPlayerVelocityEstimator;

	// Token: 0x040009A1 RID: 2465
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x040009A2 RID: 2466
	private float baseVolume;

	// Token: 0x040009A3 RID: 2467
	private float fadeRate;

	// Token: 0x040009A4 RID: 2468
	private float currentFadeLevel;
}
