using System;
using GorillaLocomotion.Swimming;
using UnityEngine;

// Token: 0x02000219 RID: 537
public class WaterSplashEffect : MonoBehaviour
{
	// Token: 0x06000C88 RID: 3208 RVA: 0x00038CB8 File Offset: 0x00036EB8
	private void OnEnable()
	{
		this.startTime = Time.time;
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00038CC5 File Offset: 0x00036EC5
	public void Destroy()
	{
		this.DeactivateParticleSystems(this.bigSplashParticleSystems);
		this.DeactivateParticleSystems(this.smallSplashParticleSystems);
		this.waterVolume = null;
		ObjectPools.instance.Destroy(base.gameObject);
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x0009F964 File Offset: 0x0009DB64
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

	// Token: 0x06000C8B RID: 3211 RVA: 0x0009FA6C File Offset: 0x0009DC6C
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

	// Token: 0x06000C8C RID: 3212 RVA: 0x0009FB34 File Offset: 0x0009DD34
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

	// Token: 0x06000C8D RID: 3213 RVA: 0x0009FB60 File Offset: 0x0009DD60
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

	// Token: 0x06000C8E RID: 3214 RVA: 0x0009FB94 File Offset: 0x0009DD94
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

	// Token: 0x06000C8F RID: 3215 RVA: 0x0009FC4C File Offset: 0x0009DE4C
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

	// Token: 0x04000FC0 RID: 4032
	private static int lastPlayedBigSplashAudioClipIndex = -1;

	// Token: 0x04000FC1 RID: 4033
	private static int lastPlayedSmallSplashEntryAudioClipIndex = -1;

	// Token: 0x04000FC2 RID: 4034
	private static int lastPlayedSmallSplashExitAudioClipIndex = -1;

	// Token: 0x04000FC3 RID: 4035
	public ParticleSystem[] bigSplashParticleSystems;

	// Token: 0x04000FC4 RID: 4036
	public ParticleSystem[] smallSplashParticleSystems;

	// Token: 0x04000FC5 RID: 4037
	public float bigSplashBaseGravityMultiplier = 0.9f;

	// Token: 0x04000FC6 RID: 4038
	public float bigSplashBaseStartSpeed = 1.9f;

	// Token: 0x04000FC7 RID: 4039
	public float bigSplashBaseSimulationSpeed = 0.9f;

	// Token: 0x04000FC8 RID: 4040
	public float smallSplashBaseGravityMultiplier = 0.6f;

	// Token: 0x04000FC9 RID: 4041
	public float smallSplashBaseStartSpeed = 0.6f;

	// Token: 0x04000FCA RID: 4042
	public float smallSplashBaseSimulationSpeed = 0.6f;

	// Token: 0x04000FCB RID: 4043
	public float lifeTime = 1f;

	// Token: 0x04000FCC RID: 4044
	private float startTime = -1f;

	// Token: 0x04000FCD RID: 4045
	public AudioSource audioSource;

	// Token: 0x04000FCE RID: 4046
	public AudioClip[] bigSplashAudioClips;

	// Token: 0x04000FCF RID: 4047
	public AudioClip[] smallSplashEntryAudioClips;

	// Token: 0x04000FD0 RID: 4048
	public AudioClip[] smallSplashExitAudioClips;

	// Token: 0x04000FD1 RID: 4049
	private WaterVolume waterVolume;
}
