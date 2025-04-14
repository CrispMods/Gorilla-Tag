using System;
using UnityEngine;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BC9 RID: 3017
	public class ShakeReaction : MonoBehaviour, ITickSystemPost
	{
		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06004C20 RID: 19488 RVA: 0x00172B8A File Offset: 0x00170D8A
		private float loopSoundTotalDuration
		{
			get
			{
				return this.loopSoundFadeInDuration + this.loopSoundSustainDuration + this.loopSoundFadeOutDuration;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06004C21 RID: 19489 RVA: 0x00172BA0 File Offset: 0x00170DA0
		// (set) Token: 0x06004C22 RID: 19490 RVA: 0x00172BA8 File Offset: 0x00170DA8
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004C23 RID: 19491 RVA: 0x00172BB4 File Offset: 0x00170DB4
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

		// Token: 0x06004C24 RID: 19492 RVA: 0x00172C34 File Offset: 0x00170E34
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

		// Token: 0x06004C25 RID: 19493 RVA: 0x00172CEB File Offset: 0x00170EEB
		protected void OnDisable()
		{
			if (this.loopSoundAudioSource != null)
			{
				this.loopSoundAudioSource.GTStop();
			}
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x000C141F File Offset: 0x000BF61F
		private void HandleApplicationQuitting()
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x00172D0C File Offset: 0x00170F0C
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

		// Token: 0x04004DF9 RID: 19961
		[SerializeField]
		private Transform shakeXform;

		// Token: 0x04004DFA RID: 19962
		[SerializeField]
		private float velocityThreshold = 5f;

		// Token: 0x04004DFB RID: 19963
		[SerializeField]
		private SoundBankPlayer shakeSoundBankPlayer;

		// Token: 0x04004DFC RID: 19964
		[SerializeField]
		private float shakeSoundCooldown = 1f;

		// Token: 0x04004DFD RID: 19965
		[SerializeField]
		private AudioSource loopSoundAudioSource;

		// Token: 0x04004DFE RID: 19966
		[SerializeField]
		private float loopSoundBaseVolume = 1f;

		// Token: 0x04004DFF RID: 19967
		[SerializeField]
		private float loopSoundSustainDuration = 1f;

		// Token: 0x04004E00 RID: 19968
		[SerializeField]
		private float loopSoundFadeInDuration = 1f;

		// Token: 0x04004E01 RID: 19969
		[SerializeField]
		private AnimationCurve loopSoundFadeInCurve;

		// Token: 0x04004E02 RID: 19970
		[SerializeField]
		private float loopSoundFadeOutDuration = 1f;

		// Token: 0x04004E03 RID: 19971
		[SerializeField]
		private AnimationCurve loopSoundFadeOutCurve;

		// Token: 0x04004E04 RID: 19972
		[SerializeField]
		private ParticleSystem particles;

		// Token: 0x04004E05 RID: 19973
		[SerializeField]
		private AnimationCurve emissionCurve;

		// Token: 0x04004E06 RID: 19974
		[SerializeField]
		private float particleDuration = 5f;

		// Token: 0x04004E08 RID: 19976
		private const int sampleHistorySize = 256;

		// Token: 0x04004E09 RID: 19977
		private float[] sampleHistoryTime;

		// Token: 0x04004E0A RID: 19978
		private Vector3[] sampleHistoryPos;

		// Token: 0x04004E0B RID: 19979
		private Vector3[] sampleHistoryVel;

		// Token: 0x04004E0C RID: 19980
		private int currentIndex;

		// Token: 0x04004E0D RID: 19981
		private float lastShakeSoundTime = float.MinValue;

		// Token: 0x04004E0E RID: 19982
		private float lastShakeTime = float.MinValue;

		// Token: 0x04004E0F RID: 19983
		private float maxEmissionRate;

		// Token: 0x04004E10 RID: 19984
		private bool hasLoopSound;

		// Token: 0x04004E11 RID: 19985
		private bool hasShakeSound;

		// Token: 0x04004E12 RID: 19986
		private bool hasParticleSystem;

		// Token: 0x04004E13 RID: 19987
		[DebugReadout]
		private float poopVelocity;
	}
}
