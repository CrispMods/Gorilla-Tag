using System;
using UnityEngine;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BF7 RID: 3063
	public class ShakeReaction : MonoBehaviour, ITickSystemPost
	{
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06004D6C RID: 19820 RVA: 0x00062C6E File Offset: 0x00060E6E
		private float loopSoundTotalDuration
		{
			get
			{
				return this.loopSoundFadeInDuration + this.loopSoundSustainDuration + this.loopSoundFadeOutDuration;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06004D6D RID: 19821 RVA: 0x00062C84 File Offset: 0x00060E84
		// (set) Token: 0x06004D6E RID: 19822 RVA: 0x00062C8C File Offset: 0x00060E8C
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004D6F RID: 19823 RVA: 0x001AB60C File Offset: 0x001A980C
		protected void Awake()
		{
			this.sampleHistoryPos = new Vector3[256];
			this.sampleHistoryTime = new float[256];
			this.sampleHistoryVel = new Vector3[256];
			if (this.particles != null)
			{
				this.maxEmissionRate = this.particles.emission.rateOverTime.constant;
			}
			Application.quitting += this.HandleApplicationQuitting;
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x001AB68C File Offset: 0x001A988C
		protected void OnEnable()
		{
			float unscaledTime = Time.unscaledTime;
			Vector3 position = this.shakeXform.position;
			for (int i = 0; i < 256; i++)
			{
				this.sampleHistoryTime[i] = unscaledTime;
				this.sampleHistoryPos[i] = position;
				this.sampleHistoryVel[i] = Vector3.zero;
			}
			if (this.loopSoundAudioSource != null)
			{
				this.loopSoundAudioSource.loop = true;
				this.loopSoundAudioSource.GTPlay();
			}
			this.hasLoopSound = (this.loopSoundAudioSource != null);
			this.hasShakeSound = (this.shakeSoundBankPlayer != null);
			this.hasParticleSystem = (this.particles != null);
			TickSystem<object>.AddPostTickCallback(this);
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x00062C95 File Offset: 0x00060E95
		protected void OnDisable()
		{
			if (this.loopSoundAudioSource != null)
			{
				this.loopSoundAudioSource.GTStop();
			}
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x0004A56E File Offset: 0x0004876E
		private void HandleApplicationQuitting()
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x001AB744 File Offset: 0x001A9944
		void ITickSystemPost.PostTick()
		{
			float unscaledTime = Time.unscaledTime;
			Vector3 position = this.shakeXform.position;
			int num = (this.currentIndex - 1 + 256) % 256;
			this.currentIndex = (this.currentIndex + 1) % 256;
			this.sampleHistoryTime[this.currentIndex] = unscaledTime;
			float num2 = unscaledTime - this.sampleHistoryTime[num];
			this.sampleHistoryPos[this.currentIndex] = position;
			if (num2 > 0f)
			{
				Vector3 a = position - this.sampleHistoryPos[num];
				this.sampleHistoryVel[this.currentIndex] = a / num2;
			}
			else
			{
				this.sampleHistoryVel[this.currentIndex] = Vector3.zero;
			}
			float sqrMagnitude = (this.sampleHistoryVel[num] - this.sampleHistoryVel[this.currentIndex]).sqrMagnitude;
			this.poopVelocity = Mathf.Round(Mathf.Sqrt(sqrMagnitude) * 1000f) / 1000f;
			float num3 = this.shakeXform.lossyScale.x * this.velocityThreshold * this.velocityThreshold;
			if (sqrMagnitude >= num3)
			{
				this.lastShakeTime = unscaledTime;
			}
			float num4 = unscaledTime - this.lastShakeTime;
			float time = Mathf.Clamp01(num4 / this.particleDuration);
			if (this.hasParticleSystem)
			{
				this.particles.emission.rateOverTime = this.emissionCurve.Evaluate(time) * this.maxEmissionRate;
			}
			if (this.hasShakeSound && this.lastShakeTime - this.lastShakeSoundTime > this.shakeSoundCooldown)
			{
				this.shakeSoundBankPlayer.Play();
				this.lastShakeSoundTime = unscaledTime;
			}
			if (this.hasLoopSound)
			{
				if (num4 < this.loopSoundFadeInDuration)
				{
					this.loopSoundAudioSource.volume = this.loopSoundBaseVolume * this.loopSoundFadeInCurve.Evaluate(Mathf.Clamp01(num4 / this.loopSoundFadeInDuration));
					return;
				}
				if (num4 < this.loopSoundFadeInDuration + this.loopSoundSustainDuration)
				{
					this.loopSoundAudioSource.volume = this.loopSoundBaseVolume;
					return;
				}
				this.loopSoundAudioSource.volume = this.loopSoundBaseVolume * this.loopSoundFadeOutCurve.Evaluate(Mathf.Clamp01((num4 - this.loopSoundFadeInDuration - this.loopSoundSustainDuration) / this.loopSoundFadeOutDuration));
			}
		}

		// Token: 0x04004EEF RID: 20207
		[SerializeField]
		private Transform shakeXform;

		// Token: 0x04004EF0 RID: 20208
		[SerializeField]
		private float velocityThreshold = 5f;

		// Token: 0x04004EF1 RID: 20209
		[SerializeField]
		private SoundBankPlayer shakeSoundBankPlayer;

		// Token: 0x04004EF2 RID: 20210
		[SerializeField]
		private float shakeSoundCooldown = 1f;

		// Token: 0x04004EF3 RID: 20211
		[SerializeField]
		private AudioSource loopSoundAudioSource;

		// Token: 0x04004EF4 RID: 20212
		[SerializeField]
		private float loopSoundBaseVolume = 1f;

		// Token: 0x04004EF5 RID: 20213
		[SerializeField]
		private float loopSoundSustainDuration = 1f;

		// Token: 0x04004EF6 RID: 20214
		[SerializeField]
		private float loopSoundFadeInDuration = 1f;

		// Token: 0x04004EF7 RID: 20215
		[SerializeField]
		private AnimationCurve loopSoundFadeInCurve;

		// Token: 0x04004EF8 RID: 20216
		[SerializeField]
		private float loopSoundFadeOutDuration = 1f;

		// Token: 0x04004EF9 RID: 20217
		[SerializeField]
		private AnimationCurve loopSoundFadeOutCurve;

		// Token: 0x04004EFA RID: 20218
		[SerializeField]
		private ParticleSystem particles;

		// Token: 0x04004EFB RID: 20219
		[SerializeField]
		private AnimationCurve emissionCurve;

		// Token: 0x04004EFC RID: 20220
		[SerializeField]
		private float particleDuration = 5f;

		// Token: 0x04004EFE RID: 20222
		private const int sampleHistorySize = 256;

		// Token: 0x04004EFF RID: 20223
		private float[] sampleHistoryTime;

		// Token: 0x04004F00 RID: 20224
		private Vector3[] sampleHistoryPos;

		// Token: 0x04004F01 RID: 20225
		private Vector3[] sampleHistoryVel;

		// Token: 0x04004F02 RID: 20226
		private int currentIndex;

		// Token: 0x04004F03 RID: 20227
		private float lastShakeSoundTime = float.MinValue;

		// Token: 0x04004F04 RID: 20228
		private float lastShakeTime = float.MinValue;

		// Token: 0x04004F05 RID: 20229
		private float maxEmissionRate;

		// Token: 0x04004F06 RID: 20230
		private bool hasLoopSound;

		// Token: 0x04004F07 RID: 20231
		private bool hasShakeSound;

		// Token: 0x04004F08 RID: 20232
		private bool hasParticleSystem;

		// Token: 0x04004F09 RID: 20233
		[DebugReadout]
		private float poopVelocity;
	}
}
