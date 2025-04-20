using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A9F RID: 2719
	public class TrainLocomotive : TrainCarBase
	{
		// Token: 0x060043F0 RID: 17392 RVA: 0x00179950 File Offset: 0x00177B50
		private void Start()
		{
			this._standardRateOverTimeMultiplier = this._smoke1.emission.rateOverTimeMultiplier;
			this._standardMaxParticles = this._smoke1.main.maxParticles;
			base.Distance = 0f;
			this._speedDiv = 2.5f / (float)this._accelerationSounds.Length;
			this._currentSpeed = this._initialSpeed;
			base.UpdateCarPosition();
			this._smoke1.Stop();
			this._startStopTrainCr = base.StartCoroutine(this.StartStopTrain(true));
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x0005C4D4 File Offset: 0x0005A6D4
		private void Update()
		{
			this.UpdatePosition();
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x001799E0 File Offset: 0x00177BE0
		public override void UpdatePosition()
		{
			if (!this._isMoving)
			{
				return;
			}
			if (this._trainTrack != null)
			{
				this.UpdateDistance();
				base.UpdateCarPosition();
				base.RotateCarWheels();
			}
			TrainCarBase[] childCars = this._childCars;
			for (int i = 0; i < childCars.Length; i++)
			{
				childCars[i].UpdatePosition();
			}
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0005C4DC File Offset: 0x0005A6DC
		public void StartStopStateChanged()
		{
			if (this._startStopTrainCr == null)
			{
				this._startStopTrainCr = base.StartCoroutine(this.StartStopTrain(!this._isMoving));
			}
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0005C501 File Offset: 0x0005A701
		private IEnumerator StartStopTrain(bool startTrain)
		{
			float endSpeed = startTrain ? this._initialSpeed : 0f;
			float timePeriodForSpeedChange = 3f;
			if (startTrain)
			{
				this._smoke1.Play();
				this._isMoving = true;
				ParticleSystem.EmissionModule emission = this._smoke1.emission;
				ParticleSystem.MainModule main = this._smoke1.main;
				emission.rateOverTimeMultiplier = this._standardRateOverTimeMultiplier;
				main.maxParticles = this._standardMaxParticles;
				timePeriodForSpeedChange = this.PlayEngineSound(TrainLocomotive.EngineSoundState.Start);
			}
			else
			{
				timePeriodForSpeedChange = this.PlayEngineSound(TrainLocomotive.EngineSoundState.Stop);
			}
			this._engineAudioSource.loop = false;
			timePeriodForSpeedChange *= 0.9f;
			float startTime = Time.time;
			float endTime = Time.time + timePeriodForSpeedChange;
			float startSpeed = this._currentSpeed;
			while (Time.time < endTime)
			{
				float num = (Time.time - startTime) / timePeriodForSpeedChange;
				this._currentSpeed = startSpeed * (1f - num) + endSpeed * num;
				this.UpdateSmokeEmissionBasedOnSpeed();
				yield return null;
			}
			this._currentSpeed = endSpeed;
			this._startStopTrainCr = null;
			this._isMoving = startTrain;
			if (!this._isMoving)
			{
				this._smoke1.Stop();
			}
			else
			{
				this._engineAudioSource.loop = true;
				this.PlayEngineSound(TrainLocomotive.EngineSoundState.AccelerateOrSetProperSpeed);
			}
			yield break;
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x00179A34 File Offset: 0x00177C34
		private float PlayEngineSound(TrainLocomotive.EngineSoundState engineSoundState)
		{
			AudioClip audioClip;
			if (engineSoundState == TrainLocomotive.EngineSoundState.Start)
			{
				audioClip = this._startUpSound;
			}
			else
			{
				AudioClip[] array = (engineSoundState == TrainLocomotive.EngineSoundState.AccelerateOrSetProperSpeed) ? this._accelerationSounds : this._decelerationSounds;
				int num = array.Length;
				int value = (int)Mathf.Round((this._currentSpeed - 0.2f) / this._speedDiv);
				audioClip = array[Mathf.Clamp(value, 0, num - 1)];
			}
			if (this._engineAudioSource.clip == audioClip && this._engineAudioSource.isPlaying && engineSoundState == TrainLocomotive.EngineSoundState.AccelerateOrSetProperSpeed)
			{
				return 0f;
			}
			this._engineAudioSource.clip = audioClip;
			this._engineAudioSource.timeSamples = 0;
			this._engineAudioSource.Play();
			return audioClip.length;
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x00179AE0 File Offset: 0x00177CE0
		private void UpdateDistance()
		{
			float num = this._reverse ? (-this._currentSpeed) : this._currentSpeed;
			base.Distance = (base.Distance + num * Time.deltaTime) % this._trainTrack.TrackLength;
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x00179B28 File Offset: 0x00177D28
		public void DecreaseSpeedStateChanged()
		{
			if (this._startStopTrainCr == null && this._isMoving)
			{
				this._currentSpeed = Mathf.Clamp(this._currentSpeed - this._speedDiv, 0.2f, 2.7f);
				this.UpdateSmokeEmissionBasedOnSpeed();
				this.PlayEngineSound(TrainLocomotive.EngineSoundState.AccelerateOrSetProperSpeed);
			}
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x00179B78 File Offset: 0x00177D78
		public void IncreaseSpeedStateChanged()
		{
			if (this._startStopTrainCr == null && this._isMoving)
			{
				this._currentSpeed = Mathf.Clamp(this._currentSpeed + this._speedDiv, 0.2f, 2.7f);
				this.UpdateSmokeEmissionBasedOnSpeed();
				this.PlayEngineSound(TrainLocomotive.EngineSoundState.AccelerateOrSetProperSpeed);
			}
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x00179BC8 File Offset: 0x00177DC8
		private void UpdateSmokeEmissionBasedOnSpeed()
		{
			this._smoke1.emission.rateOverTimeMultiplier = this.GetCurrentSmokeEmission();
			this._smoke1.main.maxParticles = (int)Mathf.Lerp((float)this._standardMaxParticles, (float)(this._standardMaxParticles * 3), this._currentSpeed / 2.5f);
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0005C517 File Offset: 0x0005A717
		private float GetCurrentSmokeEmission()
		{
			return Mathf.Lerp(this._standardRateOverTimeMultiplier, this._standardRateOverTimeMultiplier * 8f, this._currentSpeed / 2.5f);
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x00179C24 File Offset: 0x00177E24
		public void SmokeButtonStateChanged()
		{
			if (this._isMoving)
			{
				this._smokeStackAudioSource.clip = this._smokeSound;
				this._smokeStackAudioSource.timeSamples = 0;
				this._smokeStackAudioSource.Play();
				this._smoke2.time = 0f;
				this._smoke2.Play();
			}
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0005C53C File Offset: 0x0005A73C
		public void WhistleButtonStateChanged()
		{
			if (this._whistleSound != null)
			{
				this._whistleAudioSource.clip = this._whistleSound;
				this._whistleAudioSource.timeSamples = 0;
				this._whistleAudioSource.Play();
			}
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x0005C574 File Offset: 0x0005A774
		public void ReverseButtonStateChanged()
		{
			this._reverse = !this._reverse;
		}

		// Token: 0x040044A8 RID: 17576
		private const float MIN_SPEED = 0.2f;

		// Token: 0x040044A9 RID: 17577
		private const float MAX_SPEED = 2.7f;

		// Token: 0x040044AA RID: 17578
		private const float SMOKE_SPEED_MULTIPLIER = 8f;

		// Token: 0x040044AB RID: 17579
		private const int MAX_PARTICLES_MULTIPLIER = 3;

		// Token: 0x040044AC RID: 17580
		[SerializeField]
		[Range(0.2f, 2.7f)]
		protected float _initialSpeed;

		// Token: 0x040044AD RID: 17581
		[SerializeField]
		private GameObject _startStopButton;

		// Token: 0x040044AE RID: 17582
		[SerializeField]
		private GameObject _decreaseSpeedButton;

		// Token: 0x040044AF RID: 17583
		[SerializeField]
		private GameObject _increaseSpeedButton;

		// Token: 0x040044B0 RID: 17584
		[SerializeField]
		private GameObject _smokeButton;

		// Token: 0x040044B1 RID: 17585
		[SerializeField]
		private GameObject _whistleButton;

		// Token: 0x040044B2 RID: 17586
		[SerializeField]
		private GameObject _reverseButton;

		// Token: 0x040044B3 RID: 17587
		[SerializeField]
		private AudioSource _whistleAudioSource;

		// Token: 0x040044B4 RID: 17588
		[SerializeField]
		private AudioClip _whistleSound;

		// Token: 0x040044B5 RID: 17589
		[SerializeField]
		private AudioSource _engineAudioSource;

		// Token: 0x040044B6 RID: 17590
		[SerializeField]
		private AudioClip[] _accelerationSounds;

		// Token: 0x040044B7 RID: 17591
		[SerializeField]
		private AudioClip[] _decelerationSounds;

		// Token: 0x040044B8 RID: 17592
		[SerializeField]
		private AudioClip _startUpSound;

		// Token: 0x040044B9 RID: 17593
		[SerializeField]
		private AudioSource _smokeStackAudioSource;

		// Token: 0x040044BA RID: 17594
		[SerializeField]
		private AudioClip _smokeSound;

		// Token: 0x040044BB RID: 17595
		[SerializeField]
		private ParticleSystem _smoke1;

		// Token: 0x040044BC RID: 17596
		[SerializeField]
		private ParticleSystem _smoke2;

		// Token: 0x040044BD RID: 17597
		[SerializeField]
		private TrainCarBase[] _childCars;

		// Token: 0x040044BE RID: 17598
		private bool _isMoving = true;

		// Token: 0x040044BF RID: 17599
		private bool _reverse;

		// Token: 0x040044C0 RID: 17600
		private float _currentSpeed;

		// Token: 0x040044C1 RID: 17601
		private float _speedDiv;

		// Token: 0x040044C2 RID: 17602
		private float _standardRateOverTimeMultiplier;

		// Token: 0x040044C3 RID: 17603
		private int _standardMaxParticles;

		// Token: 0x040044C4 RID: 17604
		private Coroutine _startStopTrainCr;

		// Token: 0x02000AA0 RID: 2720
		private enum EngineSoundState
		{
			// Token: 0x040044C6 RID: 17606
			Start,
			// Token: 0x040044C7 RID: 17607
			AccelerateOrSetProperSpeed,
			// Token: 0x040044C8 RID: 17608
			Stop
		}
	}
}
