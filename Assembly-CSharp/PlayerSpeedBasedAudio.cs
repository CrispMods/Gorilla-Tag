using System;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class PlayerSpeedBasedAudio : MonoBehaviour
{
	// Token: 0x06000878 RID: 2168 RVA: 0x00035F7E File Offset: 0x0003417E
	private void Start()
	{
		this.fadeRate = 1f / this.fadeTime;
		this.baseVolume = this.audioSource.volume;
		this.localPlayerVelocityEstimator.TryResolve<GorillaVelocityEstimator>(out this.velocityEstimator);
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x0008E328 File Offset: 0x0008C528
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

	// Token: 0x040009DE RID: 2526
	[SerializeField]
	private float minVolumeSpeed;

	// Token: 0x040009DF RID: 2527
	[SerializeField]
	private float fullVolumeSpeed;

	// Token: 0x040009E0 RID: 2528
	[SerializeField]
	private float fadeTime;

	// Token: 0x040009E1 RID: 2529
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x040009E2 RID: 2530
	[SerializeField]
	private XSceneRef localPlayerVelocityEstimator;

	// Token: 0x040009E3 RID: 2531
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x040009E4 RID: 2532
	private float baseVolume;

	// Token: 0x040009E5 RID: 2533
	private float fadeRate;

	// Token: 0x040009E6 RID: 2534
	private float currentFadeLevel;
}
