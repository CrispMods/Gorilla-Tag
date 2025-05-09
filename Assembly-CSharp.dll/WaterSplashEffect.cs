﻿using System;
using GorillaLocomotion.Swimming;
using UnityEngine;

// Token: 0x0200020E RID: 526
public class WaterSplashEffect : MonoBehaviour
{
	// Token: 0x06000C3F RID: 3135 RVA: 0x000379F8 File Offset: 0x00035BF8
	private void OnEnable()
	{
		this.startTime = Time.time;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00037A05 File Offset: 0x00035C05
	public void Destroy()
	{
		this.DeactivateParticleSystems(this.bigSplashParticleSystems);
		this.DeactivateParticleSystems(this.smallSplashParticleSystems);
		this.waterVolume = null;
		ObjectPools.instance.Destroy(base.gameObject);
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x0009D0D8 File Offset: 0x0009B2D8
	public void PlayEffect(bool isBigSplash, bool isEntry, float scale, WaterVolume volume = null)
	{
		this.waterVolume = volume;
		if (isBigSplash)
		{
			this.DeactivateParticleSystems(this.smallSplashParticleSystems);
			this.SetParticleEffectParameters(this.bigSplashParticleSystems, scale, this.bigSplashBaseGravityMultiplier, this.bigSplashBaseStartSpeed, this.bigSplashBaseSimulationSpeed, this.waterVolume);
			this.PlayParticleEffects(this.bigSplashParticleSystems);
			this.PlayRandomAudioClipWithoutRepeats(this.bigSplashAudioClips, ref WaterSplashEffect.lastPlayedBigSplashAudioClipIndex);
			return;
		}
		if (isEntry)
		{
			this.DeactivateParticleSystems(this.bigSplashParticleSystems);
			this.SetParticleEffectParameters(this.smallSplashParticleSystems, scale, this.smallSplashBaseGravityMultiplier, this.smallSplashBaseStartSpeed, this.smallSplashBaseSimulationSpeed, this.waterVolume);
			this.PlayParticleEffects(this.smallSplashParticleSystems);
			this.PlayRandomAudioClipWithoutRepeats(this.smallSplashEntryAudioClips, ref WaterSplashEffect.lastPlayedSmallSplashEntryAudioClipIndex);
			return;
		}
		this.DeactivateParticleSystems(this.bigSplashParticleSystems);
		this.SetParticleEffectParameters(this.smallSplashParticleSystems, scale, this.smallSplashBaseGravityMultiplier, this.smallSplashBaseStartSpeed, this.smallSplashBaseSimulationSpeed, this.waterVolume);
		this.PlayParticleEffects(this.smallSplashParticleSystems);
		this.PlayRandomAudioClipWithoutRepeats(this.smallSplashExitAudioClips, ref WaterSplashEffect.lastPlayedSmallSplashExitAudioClipIndex);
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x0009D1E0 File Offset: 0x0009B3E0
	private void Update()
	{
		if (this.waterVolume != null && !this.waterVolume.isStationary && this.waterVolume.surfacePlane != null)
		{
			Vector3 b = Vector3.Dot(base.transform.position - this.waterVolume.surfacePlane.position, this.waterVolume.surfacePlane.up) * this.waterVolume.surfacePlane.up;
			base.transform.position = base.transform.position - b;
		}
		if ((Time.time - this.startTime) / this.lifeTime >= 1f)
		{
			this.Destroy();
			return;
		}
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x0009D2A8 File Offset: 0x0009B4A8
	private void DeactivateParticleSystems(ParticleSystem[] particleSystems)
	{
		if (particleSystems != null)
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				particleSystems[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0009D2D4 File Offset: 0x0009B4D4
	private void PlayParticleEffects(ParticleSystem[] particleSystems)
	{
		if (particleSystems != null)
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				particleSystems[i].gameObject.SetActive(true);
				particleSystems[i].Play();
			}
		}
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x0009D308 File Offset: 0x0009B508
	private void SetParticleEffectParameters(ParticleSystem[] particleSystems, float scale, float baseGravMultiplier, float baseStartSpeed, float baseSimulationSpeed, WaterVolume waterVolume = null)
	{
		if (particleSystems != null)
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				ParticleSystem.MainModule main = particleSystems[i].main;
				main.startSpeed = baseStartSpeed;
				main.gravityModifier = baseGravMultiplier;
				if (scale < 0.99f)
				{
					main.startSpeed = baseStartSpeed * scale * 2f;
					main.gravityModifier = baseGravMultiplier * scale * 0.5f;
				}
				if (waterVolume != null && waterVolume.Parameters != null)
				{
					particleSystems[i].colorBySpeed.color = waterVolume.Parameters.splashColorBySpeedGradient;
				}
			}
		}
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x0009D3C0 File Offset: 0x0009B5C0
	private void PlayRandomAudioClipWithoutRepeats(AudioClip[] audioClips, ref int lastPlayedAudioClipIndex)
	{
		if (this.audioSource != null && audioClips != null && audioClips.Length != 0)
		{
			int num = 0;
			if (audioClips.Length > 1)
			{
				int num2 = UnityEngine.Random.Range(0, audioClips.Length);
				if (num2 == lastPlayedAudioClipIndex)
				{
					num2 = ((UnityEngine.Random.Range(0f, 1f) > 0.5f) ? ((num2 + 1) % audioClips.Length) : (num2 - 1));
					if (num2 < 0)
					{
						num2 = audioClips.Length - 1;
					}
				}
				num = num2;
			}
			lastPlayedAudioClipIndex = num;
			this.audioSource.clip = audioClips[num];
			this.audioSource.GTPlay();
		}
	}

	// Token: 0x04000F7B RID: 3963
	private static int lastPlayedBigSplashAudioClipIndex = -1;

	// Token: 0x04000F7C RID: 3964
	private static int lastPlayedSmallSplashEntryAudioClipIndex = -1;

	// Token: 0x04000F7D RID: 3965
	private static int lastPlayedSmallSplashExitAudioClipIndex = -1;

	// Token: 0x04000F7E RID: 3966
	public ParticleSystem[] bigSplashParticleSystems;

	// Token: 0x04000F7F RID: 3967
	public ParticleSystem[] smallSplashParticleSystems;

	// Token: 0x04000F80 RID: 3968
	public float bigSplashBaseGravityMultiplier = 0.9f;

	// Token: 0x04000F81 RID: 3969
	public float bigSplashBaseStartSpeed = 1.9f;

	// Token: 0x04000F82 RID: 3970
	public float bigSplashBaseSimulationSpeed = 0.9f;

	// Token: 0x04000F83 RID: 3971
	public float smallSplashBaseGravityMultiplier = 0.6f;

	// Token: 0x04000F84 RID: 3972
	public float smallSplashBaseStartSpeed = 0.6f;

	// Token: 0x04000F85 RID: 3973
	public float smallSplashBaseSimulationSpeed = 0.6f;

	// Token: 0x04000F86 RID: 3974
	public float lifeTime = 1f;

	// Token: 0x04000F87 RID: 3975
	private float startTime = -1f;

	// Token: 0x04000F88 RID: 3976
	public AudioSource audioSource;

	// Token: 0x04000F89 RID: 3977
	public AudioClip[] bigSplashAudioClips;

	// Token: 0x04000F8A RID: 3978
	public AudioClip[] smallSplashEntryAudioClips;

	// Token: 0x04000F8B RID: 3979
	public AudioClip[] smallSplashExitAudioClips;

	// Token: 0x04000F8C RID: 3980
	private WaterVolume waterVolume;
}
