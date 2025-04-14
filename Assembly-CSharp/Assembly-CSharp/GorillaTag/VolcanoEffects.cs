using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaTag.GuidedRefs;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000BB2 RID: 2994
	public class VolcanoEffects : BaseGuidedRefTargetMono
	{
		// Token: 0x06004BAB RID: 19371 RVA: 0x00170900 File Offset: 0x0016EB00
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

		// Token: 0x06004BAC RID: 19372 RVA: 0x00170B90 File Offset: 0x0016ED90
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

		// Token: 0x06004BAD RID: 19373 RVA: 0x00170BE8 File Offset: 0x0016EDE8
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

		// Token: 0x06004BAE RID: 19374 RVA: 0x00170C48 File Offset: 0x0016EE48
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

		// Token: 0x06004BAF RID: 19375 RVA: 0x00170CF0 File Offset: 0x0016EEF0
		private void SetLavaAudioEnabled(bool toEnable)
		{
			AudioSource[] array = this.lavaSurfaceAudioSrcs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(toEnable);
			}
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x00170D20 File Offset: 0x0016EF20
		private void SetLavaAudioEnabled(bool toEnable, float volume)
		{
			foreach (AudioSource audioSource in this.lavaSurfaceAudioSrcs)
			{
				audioSource.volume = volume;
				audioSource.gameObject.SetActive(toEnable);
			}
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x00170D58 File Offset: 0x0016EF58
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

		// Token: 0x06004BB2 RID: 19378 RVA: 0x00170E14 File Offset: 0x0016F014
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

		// Token: 0x06004BB3 RID: 19379 RVA: 0x001710E9 File Offset: 0x0016F2E9
		public void SetDrainedState()
		{
			this.ResetState();
			this.SetLavaAudioEnabled(false);
			this.currentStateFX = this.drainedStateFX;
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x00171104 File Offset: 0x0016F304
		public void UpdateDrainedState(float time)
		{
			this.ResetState();
			this.UpdateState(time, float.MaxValue, float.MinValue);
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x0017111D File Offset: 0x0016F31D
		public void SetEruptingState()
		{
			this.ResetState();
			this.SetLavaAudioEnabled(false, 0f);
			this.currentStateFX = this.eruptingStateFX;
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x0017113D File Offset: 0x0016F33D
		public void UpdateEruptingState(float time, float timeRemaining, float progress)
		{
			this.UpdateState(time, timeRemaining, progress);
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x00171148 File Offset: 0x0016F348
		public void SetRisingState()
		{
			this.ResetState();
			this.SetLavaAudioEnabled(true, 0f);
			this.currentStateFX = this.risingStateFX;
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x00171168 File Offset: 0x0016F368
		public void UpdateRisingState(float time, float timeRemaining, float progress)
		{
			this.UpdateState(time, timeRemaining, progress);
			AudioSource[] array = this.lavaSurfaceAudioSrcs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].volume = Mathf.Lerp(0f, 1f, Mathf.Clamp01(time));
			}
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x001711B0 File Offset: 0x0016F3B0
		public void SetFullState()
		{
			this.ResetState();
			this.SetLavaAudioEnabled(true, 1f);
			this.currentStateFX = this.fullStateFX;
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x0017113D File Offset: 0x0016F33D
		public void UpdateFullState(float time, float timeRemaining, float progress)
		{
			this.UpdateState(time, timeRemaining, progress);
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x001711D0 File Offset: 0x0016F3D0
		public void SetDrainingState()
		{
			this.ResetState();
			this.SetLavaAudioEnabled(true, 1f);
			this.currentStateFX = this.drainingStateFX;
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x001711F0 File Offset: 0x0016F3F0
		public void UpdateDrainingState(float time, float timeRemaining, float progress)
		{
			this.UpdateState(time, timeRemaining, progress);
			AudioSource[] array = this.lavaSurfaceAudioSrcs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].volume = Mathf.Lerp(1f, 0f, progress);
			}
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x00171234 File Offset: 0x0016F434
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

		// Token: 0x06004BBE RID: 19390 RVA: 0x001712B4 File Offset: 0x0016F4B4
		private bool RemoveNullsFromArray<T>(ref T[] array) where T : Object
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

		// Token: 0x06004BBF RID: 19391 RVA: 0x0017130E File Offset: 0x0016F50E
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

		// Token: 0x04004D6D RID: 19821
		[Tooltip("Only one VolcanoEffects should change shader globals in the scene (lava color, lava light) at a time.")]
		[SerializeField]
		private bool applyShaderGlobals = true;

		// Token: 0x04004D6E RID: 19822
		[Tooltip("Game trigger notification sounds will play through this.")]
		[SerializeField]
		private AudioSource forestSpeakerAudioSrc;

		// Token: 0x04004D6F RID: 19823
		[Tooltip("The accumulator value of rocks being thrown into the volcano has been reset.")]
		[SerializeField]
		private AudioClip warnVolcanoBellyEmptied;

		// Token: 0x04004D70 RID: 19824
		[Tooltip("Accept stone sounds will play through here.")]
		[SerializeField]
		private AudioSource volcanoAudioSource;

		// Token: 0x04004D71 RID: 19825
		[Tooltip("volcano ate rock but needs more.")]
		[SerializeField]
		private AudioClip volcanoAcceptStone;

		// Token: 0x04004D72 RID: 19826
		[Tooltip("volcano ate last needed rock.")]
		[SerializeField]
		private AudioClip volcanoAcceptLastStone;

		// Token: 0x04004D73 RID: 19827
		[Tooltip("This will be faded in while lava is rising.")]
		[SerializeField]
		private AudioSource[] lavaSurfaceAudioSrcs;

		// Token: 0x04004D74 RID: 19828
		[Tooltip("Emission will be adjusted for these particles during eruption.")]
		[SerializeField]
		private ParticleSystem[] lavaSpewParticleSystems;

		// Token: 0x04004D75 RID: 19829
		[Tooltip("Smoke emits during all states but it's intensity and color will change when erupting/idling.")]
		[SerializeField]
		private ParticleSystem[] smokeParticleSystems;

		// Token: 0x04004D76 RID: 19830
		[SerializeField]
		private VolcanoEffects.LavaStateFX drainedStateFX;

		// Token: 0x04004D77 RID: 19831
		[SerializeField]
		private VolcanoEffects.LavaStateFX eruptingStateFX;

		// Token: 0x04004D78 RID: 19832
		[SerializeField]
		private VolcanoEffects.LavaStateFX risingStateFX;

		// Token: 0x04004D79 RID: 19833
		[SerializeField]
		private VolcanoEffects.LavaStateFX fullStateFX;

		// Token: 0x04004D7A RID: 19834
		[SerializeField]
		private VolcanoEffects.LavaStateFX drainingStateFX;

		// Token: 0x04004D7B RID: 19835
		private VolcanoEffects.LavaStateFX currentStateFX;

		// Token: 0x04004D7C RID: 19836
		private ParticleSystem.EmissionModule[] lavaSpewEmissionModules;

		// Token: 0x04004D7D RID: 19837
		private float[] lavaSpewEmissionDefaultRateMultipliers;

		// Token: 0x04004D7E RID: 19838
		private ParticleSystem.Burst[][] lavaSpewDefaultEmitBursts;

		// Token: 0x04004D7F RID: 19839
		private ParticleSystem.Burst[][] lavaSpewAdjustedEmitBursts;

		// Token: 0x04004D80 RID: 19840
		private ParticleSystem.MainModule[] smokeMainModules;

		// Token: 0x04004D81 RID: 19841
		private ParticleSystem.EmissionModule[] smokeEmissionModules;

		// Token: 0x04004D82 RID: 19842
		private float[] smokeEmissionDefaultRateMultipliers;

		// Token: 0x04004D83 RID: 19843
		private int shaderProp_ZoneLiquidLightColor = Shader.PropertyToID("_ZoneLiquidLightColor");

		// Token: 0x04004D84 RID: 19844
		private int shaderProp_ZoneLiquidLightDistScale = Shader.PropertyToID("_ZoneLiquidLightDistScale");

		// Token: 0x04004D85 RID: 19845
		private float timeVolcanoBellyWasLastEmpty;

		// Token: 0x04004D86 RID: 19846
		private bool hasVolcanoAudioSrc;

		// Token: 0x04004D87 RID: 19847
		private bool hasForestSpeakerAudioSrc;

		// Token: 0x02000BB3 RID: 2995
		[Serializable]
		public class LavaStateFX
		{
			// Token: 0x04004D88 RID: 19848
			public AudioClip startSound;

			// Token: 0x04004D89 RID: 19849
			public AudioSource startSoundAudioSrc;

			// Token: 0x04004D8A RID: 19850
			[Tooltip("Multiplied by the AudioSource's volume.")]
			public float startSoundVol = 1f;

			// Token: 0x04004D8B RID: 19851
			[FormerlySerializedAs("startSoundPad")]
			public float startSoundDelay;

			// Token: 0x04004D8C RID: 19852
			public AudioClip endSound;

			// Token: 0x04004D8D RID: 19853
			public AudioSource endSoundAudioSrc;

			// Token: 0x04004D8E RID: 19854
			[Tooltip("Multiplied by the AudioSource's volume.")]
			public float endSoundVol = 1f;

			// Token: 0x04004D8F RID: 19855
			[Tooltip("How much time should there be between the end of the clip playing and the end of the state.")]
			public float endSoundPadTime;

			// Token: 0x04004D90 RID: 19856
			public AudioSource loop1AudioSrc;

			// Token: 0x04004D91 RID: 19857
			public AnimationCurve loop1VolAnim;

			// Token: 0x04004D92 RID: 19858
			public AudioSource loop2AudioSrc;

			// Token: 0x04004D93 RID: 19859
			public AnimationCurve loop2VolAnim;

			// Token: 0x04004D94 RID: 19860
			public AnimationCurve lavaSpewEmissionAnim;

			// Token: 0x04004D95 RID: 19861
			public AnimationCurve smokeEmissionAnim;

			// Token: 0x04004D96 RID: 19862
			public Gradient smokeStartColorAnim;

			// Token: 0x04004D97 RID: 19863
			public Gradient lavaLightColor;

			// Token: 0x04004D98 RID: 19864
			public AnimationCurve lavaLightIntensityAnim = AnimationCurve.Constant(0f, 1f, 60f);

			// Token: 0x04004D99 RID: 19865
			public AnimationCurve lavaLightAttenuationAnim = AnimationCurve.Constant(0f, 1f, 0.1f);

			// Token: 0x04004D9A RID: 19866
			[NonSerialized]
			public bool startSoundExists;

			// Token: 0x04004D9B RID: 19867
			[NonSerialized]
			public bool startSoundPlayed;

			// Token: 0x04004D9C RID: 19868
			[NonSerialized]
			public bool endSoundExists;

			// Token: 0x04004D9D RID: 19869
			[NonSerialized]
			public bool endSoundPlayed;

			// Token: 0x04004D9E RID: 19870
			[NonSerialized]
			public bool loop1Exists;

			// Token: 0x04004D9F RID: 19871
			[NonSerialized]
			public float loop1DefaultVolume;

			// Token: 0x04004DA0 RID: 19872
			[NonSerialized]
			public bool loop2Exists;

			// Token: 0x04004DA1 RID: 19873
			[NonSerialized]
			public float loop2DefaultVolume;
		}
	}
}
