using System;
using UnityEngine;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BCC RID: 3020
	public class ShakeReaction : MonoBehaviour, ITickSystemPost
	{
		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06004C2C RID: 19500 RVA: 0x00173152 File Offset: 0x00171352
		private float loopSoundTotalDuration
		{
			get
			{
				return this.loopSoundFadeInDuration + this.loopSoundSustainDuration + this.loopSoundFadeOutDuration;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06004C2D RID: 19501 RVA: 0x00173168 File Offset: 0x00171368
		// (set) Token: 0x06004C2E RID: 19502 RVA: 0x00173170 File Offset: 0x00171370
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004C2F RID: 19503 RVA: 0x0017317C File Offset: 0x0017137C
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

		// Token: 0x06004C30 RID: 19504 RVA: 0x001731FC File Offset: 0x001713FC
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

		// Token: 0x06004C31 RID: 19505 RVA: 0x001732B3 File Offset: 0x001714B3
		protected void OnDisable()
		{
			if (this.loopSoundAudioSource != null)
			{
				this.loopSoundAudioSource.GTStop();
			}
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x000C189F File Offset: 0x000BFA9F
		private void HandleApplicationQuitting()
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x001732D4 File Offset: 0x001714D4
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

		// Token: 0x04004E0B RID: 19979
		[SerializeField]
		private Transform shakeXform;

		// Token: 0x04004E0C RID: 19980
		[SerializeField]
		private float velocityThreshold = 5f;

		// Token: 0x04004E0D RID: 19981
		[SerializeField]
		private SoundBankPlayer shakeSoundBankPlayer;

		// Token: 0x04004E0E RID: 19982
		[SerializeField]
		private float shakeSoundCooldown = 1f;

		// Token: 0x04004E0F RID: 19983
		[SerializeField]
		private AudioSource loopSoundAudioSource;

		// Token: 0x04004E10 RID: 19984
		[SerializeField]
		private float loopSoundBaseVolume = 1f;

		// Token: 0x04004E11 RID: 19985
		[SerializeField]
		private float loopSoundSustainDuration = 1f;

		// Token: 0x04004E12 RID: 19986
		[SerializeField]
		private float loopSoundFadeInDuration = 1f;

		// Token: 0x04004E13 RID: 19987
		[SerializeField]
		private AnimationCurve loopSoundFadeInCurve;

		// Token: 0x04004E14 RID: 19988
		[SerializeField]
		private float loopSoundFadeOutDuration = 1f;

		// Token: 0x04004E15 RID: 19989
		[SerializeField]
		private AnimationCurve loopSoundFadeOutCurve;

		// Token: 0x04004E16 RID: 19990
		[SerializeField]
		private ParticleSystem particles;

		// Token: 0x04004E17 RID: 19991
		[SerializeField]
		private AnimationCurve emissionCurve;

		// Token: 0x04004E18 RID: 19992
		[SerializeField]
		private float particleDuration = 5f;

		// Token: 0x04004E1A RID: 19994
		private const int sampleHistorySize = 256;

		// Token: 0x04004E1B RID: 19995
		private float[] sampleHistoryTime;

		// Token: 0x04004E1C RID: 19996
		private Vector3[] sampleHistoryPos;

		// Token: 0x04004E1D RID: 19997
		private Vector3[] sampleHistoryVel;

		// Token: 0x04004E1E RID: 19998
		private int currentIndex;

		// Token: 0x04004E1F RID: 19999
		private float lastShakeSoundTime = float.MinValue;

		// Token: 0x04004E20 RID: 20000
		private float lastShakeTime = float.MinValue;

		// Token: 0x04004E21 RID: 20001
		private float maxEmissionRate;

		// Token: 0x04004E22 RID: 20002
		private bool hasLoopSound;

		// Token: 0x04004E23 RID: 20003
		private bool hasShakeSound;

		// Token: 0x04004E24 RID: 20004
		private bool hasParticleSystem;

		// Token: 0x04004E25 RID: 20005
		[DebugReadout]
		private float poopVelocity;
	}
}
