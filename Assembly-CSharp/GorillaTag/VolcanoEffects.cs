using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaTag.GuidedRefs;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000BDB RID: 3035
	public class VolcanoEffects : BaseGuidedRefTargetMono
	{
		// Token: 0x06004CDF RID: 19679 RVA: 0x001A9234 File Offset: 0x001A7434
		protected override void Awake()
		{
			base.Awake();
			if (this.RemoveNullsFromArray<ParticleSystem>(ref this.lavaSpewParticleSystems))
			{
				this.LogNullsFoundInArray("lavaSpewParticleSystems");
			}
			if (this.RemoveNullsFromArray<ParticleSystem>(ref this.smokeParticleSystems))
			{
				this.LogNullsFoundInArray("smokeParticleSystems");
			}
			this.hasVolcanoAudioSrc = (this.volcanoAudioSource != null);
			this.hasForestSpeakerAudioSrc = (this.forestSpeakerAudioSrc != null);
			this.lavaSpewEmissionModules = new ParticleSystem.EmissionModule[this.lavaSpewParticleSystems.Length];
			this.lavaSpewEmissionDefaultRateMultipliers = new float[this.lavaSpewParticleSystems.Length];
			this.lavaSpewDefaultEmitBursts = new ParticleSystem.Burst[this.lavaSpewParticleSystems.Length][];
			this.lavaSpewAdjustedEmitBursts = new ParticleSystem.Burst[this.lavaSpewParticleSystems.Length][];
			for (int i = 0; i < this.lavaSpewParticleSystems.Length; i++)
			{
				ParticleSystem.EmissionModule emission = this.lavaSpewParticleSystems[i].emission;
				this.lavaSpewEmissionDefaultRateMultipliers[i] = emission.rateOverTimeMultiplier;
				this.lavaSpewDefaultEmitBursts[i] = new ParticleSystem.Burst[emission.burstCount];
				this.lavaSpewAdjustedEmitBursts[i] = new ParticleSystem.Burst[emission.burstCount];
				for (int j = 0; j < emission.burstCount; j++)
				{
					ParticleSystem.Burst burst = emission.GetBurst(j);
					this.lavaSpewDefaultEmitBursts[i][j] = burst;
					this.lavaSpewAdjustedEmitBursts[i][j] = new ParticleSystem.Burst(burst.time, burst.minCount, burst.maxCount, burst.cycleCount, burst.repeatInterval);
					this.lavaSpewAdjustedEmitBursts[i][j].count = burst.count;
				}
				this.lavaSpewEmissionModules[i] = emission;
			}
			this.smokeMainModules = new ParticleSystem.MainModule[this.smokeParticleSystems.Length];
			this.smokeEmissionModules = new ParticleSystem.EmissionModule[this.smokeParticleSystems.Length];
			this.smokeEmissionDefaultRateMultipliers = new float[this.smokeParticleSystems.Length];
			for (int k = 0; k < this.smokeParticleSystems.Length; k++)
			{
				this.smokeMainModules[k] = this.smokeParticleSystems[k].main;
				this.smokeEmissionModules[k] = this.smokeParticleSystems[k].emission;
				this.smokeEmissionDefaultRateMultipliers[k] = this.smokeEmissionModules[k].rateOverTimeMultiplier;
			}
			this.InitState(this.drainedStateFX);
			this.InitState(this.eruptingStateFX);
			this.InitState(this.risingStateFX);
			this.InitState(this.fullStateFX);
			this.InitState(this.drainingStateFX);
			this.currentStateFX = this.drainedStateFX;
			this.UpdateDrainedState(0f);
		}

		// Token: 0x06004CE0 RID: 19680 RVA: 0x001A94C4 File Offset: 0x001A76C4
		public void OnVolcanoBellyEmpty()
		{
			if (!this.hasForestSpeakerAudioSrc)
			{
				return;
			}
			if (Time.time - this.timeVolcanoBellyWasLastEmpty < this.warnVolcanoBellyEmptied.length)
			{
				return;
			}
			this.forestSpeakerAudioSrc.gameObject.SetActive(true);
			this.forestSpeakerAudioSrc.GTPlayOneShot(this.warnVolcanoBellyEmptied, 1f);
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x001A951C File Offset: 0x001A771C
		public void OnStoneAccepted(double activationProgress)
		{
			if (!this.hasVolcanoAudioSrc)
			{
				return;
			}
			this.volcanoAudioSource.gameObject.SetActive(true);
			if (activationProgress > 1.0)
			{
				this.volcanoAudioSource.GTPlayOneShot(this.volcanoAcceptLastStone, 1f);
				return;
			}
			this.volcanoAudioSource.GTPlayOneShot(this.volcanoAcceptStone, 1f);
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x001A957C File Offset: 0x001A777C
		private void InitState(VolcanoEffects.LavaStateFX fx)
		{
			fx.startSoundExists = (fx.startSound != null);
			fx.endSoundExists = (fx.endSound != null);
			fx.loop1Exists = (fx.loop1AudioSrc != null);
			fx.loop2Exists = (fx.loop2AudioSrc != null);
			if (fx.loop1Exists)
			{
				fx.loop1DefaultVolume = fx.loop1AudioSrc.volume;
				fx.loop1AudioSrc.volume = 0f;
			}
			if (fx.loop2Exists)
			{
				fx.loop2DefaultVolume = fx.loop2AudioSrc.volume;
				fx.loop2AudioSrc.volume = 0f;
			}
		}

		// Token: 0x06004CE3 RID: 19683 RVA: 0x001A9624 File Offset: 0x001A7824
		private void SetLavaAudioEnabled(bool toEnable)
		{
			AudioSource[] array = this.lavaSurfaceAudioSrcs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(toEnable);
			}
		}

		// Token: 0x06004CE4 RID: 19684 RVA: 0x001A9654 File Offset: 0x001A7854
		private void SetLavaAudioEnabled(bool toEnable, float volume)
		{
			foreach (AudioSource audioSource in this.lavaSurfaceAudioSrcs)
			{
				audioSource.volume = volume;
				audioSource.gameObject.SetActive(toEnable);
			}
		}

		// Token: 0x06004CE5 RID: 19685 RVA: 0x001A968C File Offset: 0x001A788C
		private void ResetState()
		{
			if (this.currentStateFX == null)
			{
				return;
			}
			this.currentStateFX.startSoundPlayed = false;
			this.currentStateFX.endSoundPlayed = false;
			if (this.currentStateFX.startSoundExists)
			{
				this.currentStateFX.startSoundAudioSrc.gameObject.SetActive(false);
			}
			if (this.currentStateFX.endSoundExists)
			{
				this.currentStateFX.endSoundAudioSrc.gameObject.SetActive(false);
			}
			if (this.currentStateFX.loop1Exists)
			{
				this.currentStateFX.loop1AudioSrc.gameObject.SetActive(false);
			}
			if (this.currentStateFX.loop2Exists)
			{
				this.currentStateFX.loop2AudioSrc.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004CE6 RID: 19686 RVA: 0x001A9748 File Offset: 0x001A7948
		private void UpdateState(float time, float timeRemaining, float progress)
		{
			if (this.currentStateFX == null)
			{
				return;
			}
			if (this.currentStateFX.startSoundExists && !this.currentStateFX.startSoundPlayed && time >= this.currentStateFX.startSoundDelay)
			{
				this.currentStateFX.startSoundPlayed = true;
				this.currentStateFX.startSoundAudioSrc.gameObject.SetActive(true);
				this.currentStateFX.startSoundAudioSrc.GTPlayOneShot(this.currentStateFX.startSound, this.currentStateFX.startSoundVol);
			}
			if (this.currentStateFX.endSoundExists && !this.currentStateFX.endSoundPlayed && timeRemaining <= this.currentStateFX.endSound.length + this.currentStateFX.endSoundPadTime)
			{
				this.currentStateFX.endSoundPlayed = true;
				this.currentStateFX.endSoundAudioSrc.gameObject.SetActive(true);
				this.currentStateFX.endSoundAudioSrc.GTPlayOneShot(this.currentStateFX.endSound, this.currentStateFX.endSoundVol);
			}
			if (this.currentStateFX.loop1Exists)
			{
				this.currentStateFX.loop1AudioSrc.volume = this.currentStateFX.loop1VolAnim.Evaluate(progress) * this.currentStateFX.loop1DefaultVolume;
				if (!this.currentStateFX.loop1AudioSrc.isPlaying)
				{
					this.currentStateFX.loop1AudioSrc.gameObject.SetActive(true);
					this.currentStateFX.loop1AudioSrc.GTPlay();
				}
			}
			if (this.currentStateFX.loop2Exists)
			{
				this.currentStateFX.loop2AudioSrc.volume = this.currentStateFX.loop2VolAnim.Evaluate(progress) * this.currentStateFX.loop2DefaultVolume;
				if (!this.currentStateFX.loop2AudioSrc.isPlaying)
				{
					this.currentStateFX.loop2AudioSrc.gameObject.SetActive(true);
					this.currentStateFX.loop2AudioSrc.GTPlay();
				}
			}
			for (int i = 0; i < this.smokeMainModules.Length; i++)
			{
				this.smokeMainModules[i].startColor = this.currentStateFX.smokeStartColorAnim.Evaluate(progress);
				this.smokeEmissionModules[i].rateOverTimeMultiplier = this.currentStateFX.smokeEmissionAnim.Evaluate(progress) * this.smokeEmissionDefaultRateMultipliers[i];
			}
			this.SetParticleEmissionRateAndBurst(this.currentStateFX.lavaSpewEmissionAnim.Evaluate(progress), this.lavaSpewEmissionModules, this.lavaSpewEmissionDefaultRateMultipliers, this.lavaSpewDefaultEmitBursts, this.lavaSpewAdjustedEmitBursts);
			if (this.applyShaderGlobals)
			{
				Shader.SetGlobalColor(this.shaderProp_ZoneLiquidLightColor, this.currentStateFX.lavaLightColor.Evaluate(progress) * this.currentStateFX.lavaLightIntensityAnim.Evaluate(progress));
				Shader.SetGlobalFloat(this.shaderProp_ZoneLiquidLightDistScale, this.currentStateFX.lavaLightAttenuationAnim.Evaluate(progress));
			}
		}

		// Token: 0x06004CE7 RID: 19687 RVA: 0x00062741 File Offset: 0x00060941
		public void SetDrainedState()
		{
			this.ResetState();
			this.SetLavaAudioEnabled(false);
			this.currentStateFX = this.drainedStateFX;
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x0006275C File Offset: 0x0006095C
		public void UpdateDrainedState(float time)
		{
			this.ResetState();
			this.UpdateState(time, float.MaxValue, float.MinValue);
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x00062775 File Offset: 0x00060975
		public void SetEruptingState()
		{
			this.ResetState();
			this.SetLavaAudioEnabled(false, 0f);
			this.currentStateFX = this.eruptingStateFX;
		}

		// Token: 0x06004CEA RID: 19690 RVA: 0x00062795 File Offset: 0x00060995
		public void UpdateEruptingState(float time, float timeRemaining, float progress)
		{
			this.UpdateState(time, timeRemaining, progress);
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x000627A0 File Offset: 0x000609A0
		public void SetRisingState()
		{
			this.ResetState();
			this.SetLavaAudioEnabled(true, 0f);
			this.currentStateFX = this.risingStateFX;
		}

		// Token: 0x06004CEC RID: 19692 RVA: 0x001A9A20 File Offset: 0x001A7C20
		public void UpdateRisingState(float time, float timeRemaining, float progress)
		{
			this.UpdateState(time, timeRemaining, progress);
			AudioSource[] array = this.lavaSurfaceAudioSrcs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].volume = Mathf.Lerp(0f, 1f, Mathf.Clamp01(time));
			}
		}

		// Token: 0x06004CED RID: 19693 RVA: 0x000627C0 File Offset: 0x000609C0
		public void SetFullState()
		{
			this.ResetState();
			this.SetLavaAudioEnabled(true, 1f);
			this.currentStateFX = this.fullStateFX;
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x00062795 File Offset: 0x00060995
		public void UpdateFullState(float time, float timeRemaining, float progress)
		{
			this.UpdateState(time, timeRemaining, progress);
		}

		// Token: 0x06004CEF RID: 19695 RVA: 0x000627E0 File Offset: 0x000609E0
		public void SetDrainingState()
		{
			this.ResetState();
			this.SetLavaAudioEnabled(true, 1f);
			this.currentStateFX = this.drainingStateFX;
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x001A9A68 File Offset: 0x001A7C68
		public void UpdateDrainingState(float time, float timeRemaining, float progress)
		{
			this.UpdateState(time, timeRemaining, progress);
			AudioSource[] array = this.lavaSurfaceAudioSrcs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].volume = Mathf.Lerp(1f, 0f, progress);
			}
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x001A9AAC File Offset: 0x001A7CAC
		private void SetParticleEmissionRateAndBurst(float multiplier, ParticleSystem.EmissionModule[] emissionModules, float[] defaultRateMultipliers, ParticleSystem.Burst[][] defaultEmitBursts, ParticleSystem.Burst[][] adjustedEmitBursts)
		{
			for (int i = 0; i < emissionModules.Length; i++)
			{
				emissionModules[i].rateOverTimeMultiplier = multiplier * defaultRateMultipliers[i];
				int num = Mathf.Min(emissionModules[i].burstCount, defaultEmitBursts[i].Length);
				for (int j = 0; j < num; j++)
				{
					adjustedEmitBursts[i][j].probability = defaultEmitBursts[i][j].probability * multiplier;
				}
				emissionModules[i].SetBursts(adjustedEmitBursts[i]);
			}
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x001A9B2C File Offset: 0x001A7D2C
		private bool RemoveNullsFromArray<T>(ref T[] array) where T : UnityEngine.Object
		{
			List<T> list = new List<T>(array.Length);
			foreach (T t in array)
			{
				if (t != null)
				{
					list.Add(t);
				}
			}
			int num = array.Length;
			array = list.ToArray();
			return num != array.Length;
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x00062800 File Offset: 0x00060A00
		private void LogNullsFoundInArray(string nameOfArray)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Null reference found in ",
				nameOfArray,
				" array of component: \"",
				this.GetComponentPath(int.MaxValue),
				"\""
			}), this);
		}

		// Token: 0x04004E4E RID: 20046
		[Tooltip("Only one VolcanoEffects should change shader globals in the scene (lava color, lava light) at a time.")]
		[SerializeField]
		private bool applyShaderGlobals = true;

		// Token: 0x04004E4F RID: 20047
		[Tooltip("Game trigger notification sounds will play through this.")]
		[SerializeField]
		private AudioSource forestSpeakerAudioSrc;

		// Token: 0x04004E50 RID: 20048
		[Tooltip("The accumulator value of rocks being thrown into the volcano has been reset.")]
		[SerializeField]
		private AudioClip warnVolcanoBellyEmptied;

		// Token: 0x04004E51 RID: 20049
		[Tooltip("Accept stone sounds will play through here.")]
		[SerializeField]
		private AudioSource volcanoAudioSource;

		// Token: 0x04004E52 RID: 20050
		[Tooltip("volcano ate rock but needs more.")]
		[SerializeField]
		private AudioClip volcanoAcceptStone;

		// Token: 0x04004E53 RID: 20051
		[Tooltip("volcano ate last needed rock.")]
		[SerializeField]
		private AudioClip volcanoAcceptLastStone;

		// Token: 0x04004E54 RID: 20052
		[Tooltip("This will be faded in while lava is rising.")]
		[SerializeField]
		private AudioSource[] lavaSurfaceAudioSrcs;

		// Token: 0x04004E55 RID: 20053
		[Tooltip("Emission will be adjusted for these particles during eruption.")]
		[SerializeField]
		private ParticleSystem[] lavaSpewParticleSystems;

		// Token: 0x04004E56 RID: 20054
		[Tooltip("Smoke emits during all states but it's intensity and color will change when erupting/idling.")]
		[SerializeField]
		private ParticleSystem[] smokeParticleSystems;

		// Token: 0x04004E57 RID: 20055
		[SerializeField]
		private VolcanoEffects.LavaStateFX drainedStateFX;

		// Token: 0x04004E58 RID: 20056
		[SerializeField]
		private VolcanoEffects.LavaStateFX eruptingStateFX;

		// Token: 0x04004E59 RID: 20057
		[SerializeField]
		private VolcanoEffects.LavaStateFX risingStateFX;

		// Token: 0x04004E5A RID: 20058
		[SerializeField]
		private VolcanoEffects.LavaStateFX fullStateFX;

		// Token: 0x04004E5B RID: 20059
		[SerializeField]
		private VolcanoEffects.LavaStateFX drainingStateFX;

		// Token: 0x04004E5C RID: 20060
		private VolcanoEffects.LavaStateFX currentStateFX;

		// Token: 0x04004E5D RID: 20061
		private ParticleSystem.EmissionModule[] lavaSpewEmissionModules;

		// Token: 0x04004E5E RID: 20062
		private float[] lavaSpewEmissionDefaultRateMultipliers;

		// Token: 0x04004E5F RID: 20063
		private ParticleSystem.Burst[][] lavaSpewDefaultEmitBursts;

		// Token: 0x04004E60 RID: 20064
		private ParticleSystem.Burst[][] lavaSpewAdjustedEmitBursts;

		// Token: 0x04004E61 RID: 20065
		private ParticleSystem.MainModule[] smokeMainModules;

		// Token: 0x04004E62 RID: 20066
		private ParticleSystem.EmissionModule[] smokeEmissionModules;

		// Token: 0x04004E63 RID: 20067
		private float[] smokeEmissionDefaultRateMultipliers;

		// Token: 0x04004E64 RID: 20068
		private int shaderProp_ZoneLiquidLightColor = Shader.PropertyToID("_ZoneLiquidLightColor");

		// Token: 0x04004E65 RID: 20069
		private int shaderProp_ZoneLiquidLightDistScale = Shader.PropertyToID("_ZoneLiquidLightDistScale");

		// Token: 0x04004E66 RID: 20070
		private float timeVolcanoBellyWasLastEmpty;

		// Token: 0x04004E67 RID: 20071
		private bool hasVolcanoAudioSrc;

		// Token: 0x04004E68 RID: 20072
		private bool hasForestSpeakerAudioSrc;

		// Token: 0x02000BDC RID: 3036
		[Serializable]
		public class LavaStateFX
		{
			// Token: 0x04004E69 RID: 20073
			public AudioClip startSound;

			// Token: 0x04004E6A RID: 20074
			public AudioSource startSoundAudioSrc;

			// Token: 0x04004E6B RID: 20075
			[Tooltip("Multiplied by the AudioSource's volume.")]
			public float startSoundVol = 1f;

			// Token: 0x04004E6C RID: 20076
			[FormerlySerializedAs("startSoundPad")]
			public float startSoundDelay;

			// Token: 0x04004E6D RID: 20077
			public AudioClip endSound;

			// Token: 0x04004E6E RID: 20078
			public AudioSource endSoundAudioSrc;

			// Token: 0x04004E6F RID: 20079
			[Tooltip("Multiplied by the AudioSource's volume.")]
			public float endSoundVol = 1f;

			// Token: 0x04004E70 RID: 20080
			[Tooltip("How much time should there be between the end of the clip playing and the end of the state.")]
			public float endSoundPadTime;

			// Token: 0x04004E71 RID: 20081
			public AudioSource loop1AudioSrc;

			// Token: 0x04004E72 RID: 20082
			public AnimationCurve loop1VolAnim;

			// Token: 0x04004E73 RID: 20083
			public AudioSource loop2AudioSrc;

			// Token: 0x04004E74 RID: 20084
			public AnimationCurve loop2VolAnim;

			// Token: 0x04004E75 RID: 20085
			public AnimationCurve lavaSpewEmissionAnim;

			// Token: 0x04004E76 RID: 20086
			public AnimationCurve smokeEmissionAnim;

			// Token: 0x04004E77 RID: 20087
			public Gradient smokeStartColorAnim;

			// Token: 0x04004E78 RID: 20088
			public Gradient lavaLightColor;

			// Token: 0x04004E79 RID: 20089
			public AnimationCurve lavaLightIntensityAnim = AnimationCurve.Constant(0f, 1f, 60f);

			// Token: 0x04004E7A RID: 20090
			public AnimationCurve lavaLightAttenuationAnim = AnimationCurve.Constant(0f, 1f, 0.1f);

			// Token: 0x04004E7B RID: 20091
			[NonSerialized]
			public bool startSoundExists;

			// Token: 0x04004E7C RID: 20092
			[NonSerialized]
			public bool startSoundPlayed;

			// Token: 0x04004E7D RID: 20093
			[NonSerialized]
			public bool endSoundExists;

			// Token: 0x04004E7E RID: 20094
			[NonSerialized]
			public bool endSoundPlayed;

			// Token: 0x04004E7F RID: 20095
			[NonSerialized]
			public bool loop1Exists;

			// Token: 0x04004E80 RID: 20096
			[NonSerialized]
			public float loop1DefaultVolume;

			// Token: 0x04004E81 RID: 20097
			[NonSerialized]
			public bool loop2Exists;

			// Token: 0x04004E82 RID: 20098
			[NonSerialized]
			public float loop2DefaultVolume;
		}
	}
}
