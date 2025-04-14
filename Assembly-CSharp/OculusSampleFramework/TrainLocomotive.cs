using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A72 RID: 2674
	public class TrainLocomotive : TrainCarBase
	{
		// Token: 0x060042AB RID: 17067 RVA: 0x0013A5DC File Offset: 0x001387DC
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

		// Token: 0x060042AC RID: 17068 RVA: 0x0013A66B File Offset: 0x0013886B
		private void Update()
		{
			this.UpdatePosition();
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x0013A674 File Offset: 0x00138874
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

		// Token: 0x060042AE RID: 17070 RVA: 0x0013A6C7 File Offset: 0x001388C7
		public void StartStopStateChanged()
		{
			if (this._startStopTrainCr == null)
			{
				this._startStopTrainCr = base.StartCoroutine(this.StartStopTrain(!this._isMoving));
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x0013A6EC File Offset: 0x001388EC
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

		// Token: 0x060042B0 RID: 17072 RVA: 0x0013A704 File Offset: 0x00138904
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

		// Token: 0x060042B1 RID: 17073 RVA: 0x0013A7B0 File Offset: 0x001389B0
		private void UpdateDistance()
		{
			float num = this._reverse ? (-this._currentSpeed) : this._currentSpeed;
			base.Distance = (base.Distance + num * Time.deltaTime) % this._trainTrack.TrackLength;
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x0013A7F8 File Offset: 0x001389F8
		public void DecreaseSpeedStateChanged()
		{
			if (this._startStopTrainCr == null && this._isMoving)
			{
				this._currentSpeed = Mathf.Clamp(this._currentSpeed - this._speedDiv, 0.2f, 2.7f);
				this.UpdateSmokeEmissionBasedOnSpeed();
				this.PlayEngineSound(TrainLocomotive.EngineSoundState.AccelerateOrSetProperSpeed);
			}
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x0013A848 File Offset: 0x00138A48
		public void IncreaseSpeedStateChanged()
		{
			if (this._startStopTrainCr == null && this._isMoving)
			{
				this._currentSpeed = Mathf.Clamp(this._currentSpeed + this._speedDiv, 0.2f, 2.7f);
				this.UpdateSmokeEmissionBasedOnSpeed();
				this.PlayEngineSound(TrainLocomotive.EngineSoundState.AccelerateOrSetProperSpeed);
			}
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x0013A898 File Offset: 0x00138A98
		private void UpdateSmokeEmissionBasedOnSpeed()
		{
			this._smoke1.emission.rateOverTimeMultiplier = this.GetCurrentSmokeEmission();
			this._smoke1.main.maxParticles = (int)Mathf.Lerp((float)this._standardMaxParticles, (float)(this._standardMaxParticles * 3), this._currentSpeed / 2.5f);
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x0013A8F3 File Offset: 0x00138AF3
		private float GetCurrentSmokeEmission()
		{
			return Mathf.Lerp(this._standardRateOverTimeMultiplier, this._standardRateOverTimeMultiplier * 8f, this._currentSpeed / 2.5f);
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x0013A918 File Offset: 0x00138B18
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

		// Token: 0x060042B7 RID: 17079 RVA: 0x0013A970 File Offset: 0x00138B70
		public void WhistleButtonStateChanged()
		{
			if (this._whistleSound != null)
			{
				this._whistleAudioSource.clip = this._whistleSound;
				this._whistleAudioSource.timeSamples = 0;
				this._whistleAudioSource.Play();
			}
		}

		// Token: 0x060042B8 RID: 17080 RVA: 0x0013A9A8 File Offset: 0x00138BA8
		public void ReverseButtonStateChanged()
		{
			this._reverse = !this._reverse;
		}

		// Token: 0x040043AE RID: 17326
		private const float MIN_SPEED = 0.2f;

		// Token: 0x040043AF RID: 17327
		private const float MAX_SPEED = 2.7f;

		// Token: 0x040043B0 RID: 17328
		private const float SMOKE_SPEED_MULTIPLIER = 8f;

		// Token: 0x040043B1 RID: 17329
		private const int MAX_PARTICLES_MULTIPLIER = 3;

		// Token: 0x040043B2 RID: 17330
		[SerializeField]
		[Range(0.2f, 2.7f)]
		protected float _initialSpeed;

		// Token: 0x040043B3 RID: 17331
		[SerializeField]
		private GameObject _startStopButton;

		// Token: 0x040043B4 RID: 17332
		[SerializeField]
		private GameObject _decreaseSpeedButton;

		// Token: 0x040043B5 RID: 17333
		[SerializeField]
		private GameObject _increaseSpeedButton;

		// Token: 0x040043B6 RID: 17334
		[SerializeField]
		private GameObject _smokeButton;

		// Token: 0x040043B7 RID: 17335
		[SerializeField]
		private GameObject _whistleButton;

		// Token: 0x040043B8 RID: 17336
		[SerializeField]
		private GameObject _reverseButton;

		// Token: 0x040043B9 RID: 17337
		[SerializeField]
		private AudioSource _whistleAudioSource;

		// Token: 0x040043BA RID: 17338
		[SerializeField]
		private AudioClip _whistleSound;

		// Token: 0x040043BB RID: 17339
		[SerializeField]
		private AudioSource _engineAudioSource;

		// Token: 0x040043BC RID: 17340
		[SerializeField]
		private AudioClip[] _accelerationSounds;

		// Token: 0x040043BD RID: 17341
		[SerializeField]
		private AudioClip[] _decelerationSounds;

		// Token: 0x040043BE RID: 17342
		[SerializeField]
		private AudioClip _startUpSound;

		// Token: 0x040043BF RID: 17343
		[SerializeField]
		private AudioSource _smokeStackAudioSource;

		// Token: 0x040043C0 RID: 17344
		[SerializeField]
		private AudioClip _smokeSound;

		// Token: 0x040043C1 RID: 17345
		[SerializeField]
		private ParticleSystem _smoke1;

		// Token: 0x040043C2 RID: 17346
		[SerializeField]
		private ParticleSystem _smoke2;

		// Token: 0x040043C3 RID: 17347
		[SerializeField]
		private TrainCarBase[] _childCars;

		// Token: 0x040043C4 RID: 17348
		private bool _isMoving = true;

		// Token: 0x040043C5 RID: 17349
		private bool _reverse;

		// Token: 0x040043C6 RID: 17350
		private float _currentSpeed;

		// Token: 0x040043C7 RID: 17351
		private float _speedDiv;

		// Token: 0x040043C8 RID: 17352
		private float _standardRateOverTimeMultiplier;

		// Token: 0x040043C9 RID: 17353
		private int _standardMaxParticles;

		// Token: 0x040043CA RID: 17354
		private Coroutine _startStopTrainCr;

		// Token: 0x02000A73 RID: 2675
		private enum EngineSoundState
		{
			// Token: 0x040043CC RID: 17356
			Start,
			// Token: 0x040043CD RID: 17357
			AccelerateOrSetProperSpeed,
			// Token: 0x040043CE RID: 17358
			Stop
		}
	}
}
