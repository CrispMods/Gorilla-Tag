using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A75 RID: 2677
	public class TrainLocomotive : TrainCarBase
	{
		// Token: 0x060042B7 RID: 17079 RVA: 0x0013ABA4 File Offset: 0x00138DA4
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

		// Token: 0x060042B8 RID: 17080 RVA: 0x0013AC33 File Offset: 0x00138E33
		private void Update()
		{
			this.UpdatePosition();
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x0013AC3C File Offset: 0x00138E3C
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

		// Token: 0x060042BA RID: 17082 RVA: 0x0013AC8F File Offset: 0x00138E8F
		public void StartStopStateChanged()
		{
			if (this._startStopTrainCr == null)
			{
				this._startStopTrainCr = base.StartCoroutine(this.StartStopTrain(!this._isMoving));
			}
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x0013ACB4 File Offset: 0x00138EB4
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

		// Token: 0x060042BC RID: 17084 RVA: 0x0013ACCC File Offset: 0x00138ECC
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

		// Token: 0x060042BD RID: 17085 RVA: 0x0013AD78 File Offset: 0x00138F78
		private void UpdateDistance()
		{
			float num = this._reverse ? (-this._currentSpeed) : this._currentSpeed;
			base.Distance = (base.Distance + num * Time.deltaTime) % this._trainTrack.TrackLength;
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x0013ADC0 File Offset: 0x00138FC0
		public void DecreaseSpeedStateChanged()
		{
			if (this._startStopTrainCr == null && this._isMoving)
			{
				this._currentSpeed = Mathf.Clamp(this._currentSpeed - this._speedDiv, 0.2f, 2.7f);
				this.UpdateSmokeEmissionBasedOnSpeed();
				this.PlayEngineSound(TrainLocomotive.EngineSoundState.AccelerateOrSetProperSpeed);
			}
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x0013AE10 File Offset: 0x00139010
		public void IncreaseSpeedStateChanged()
		{
			if (this._startStopTrainCr == null && this._isMoving)
			{
				this._currentSpeed = Mathf.Clamp(this._currentSpeed + this._speedDiv, 0.2f, 2.7f);
				this.UpdateSmokeEmissionBasedOnSpeed();
				this.PlayEngineSound(TrainLocomotive.EngineSoundState.AccelerateOrSetProperSpeed);
			}
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x0013AE60 File Offset: 0x00139060
		private void UpdateSmokeEmissionBasedOnSpeed()
		{
			this._smoke1.emission.rateOverTimeMultiplier = this.GetCurrentSmokeEmission();
			this._smoke1.main.maxParticles = (int)Mathf.Lerp((float)this._standardMaxParticles, (float)(this._standardMaxParticles * 3), this._currentSpeed / 2.5f);
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x0013AEBB File Offset: 0x001390BB
		private float GetCurrentSmokeEmission()
		{
			return Mathf.Lerp(this._standardRateOverTimeMultiplier, this._standardRateOverTimeMultiplier * 8f, this._currentSpeed / 2.5f);
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x0013AEE0 File Offset: 0x001390E0
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

		// Token: 0x060042C3 RID: 17091 RVA: 0x0013AF38 File Offset: 0x00139138
		public void WhistleButtonStateChanged()
		{
			if (this._whistleSound != null)
			{
				this._whistleAudioSource.clip = this._whistleSound;
				this._whistleAudioSource.timeSamples = 0;
				this._whistleAudioSource.Play();
			}
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x0013AF70 File Offset: 0x00139170
		public void ReverseButtonStateChanged()
		{
			this._reverse = !this._reverse;
		}

		// Token: 0x040043C0 RID: 17344
		private const float MIN_SPEED = 0.2f;

		// Token: 0x040043C1 RID: 17345
		private const float MAX_SPEED = 2.7f;

		// Token: 0x040043C2 RID: 17346
		private const float SMOKE_SPEED_MULTIPLIER = 8f;

		// Token: 0x040043C3 RID: 17347
		private const int MAX_PARTICLES_MULTIPLIER = 3;

		// Token: 0x040043C4 RID: 17348
		[SerializeField]
		[Range(0.2f, 2.7f)]
		protected float _initialSpeed;

		// Token: 0x040043C5 RID: 17349
		[SerializeField]
		private GameObject _startStopButton;

		// Token: 0x040043C6 RID: 17350
		[SerializeField]
		private GameObject _decreaseSpeedButton;

		// Token: 0x040043C7 RID: 17351
		[SerializeField]
		private GameObject _increaseSpeedButton;

		// Token: 0x040043C8 RID: 17352
		[SerializeField]
		private GameObject _smokeButton;

		// Token: 0x040043C9 RID: 17353
		[SerializeField]
		private GameObject _whistleButton;

		// Token: 0x040043CA RID: 17354
		[SerializeField]
		private GameObject _reverseButton;

		// Token: 0x040043CB RID: 17355
		[SerializeField]
		private AudioSource _whistleAudioSource;

		// Token: 0x040043CC RID: 17356
		[SerializeField]
		private AudioClip _whistleSound;

		// Token: 0x040043CD RID: 17357
		[SerializeField]
		private AudioSource _engineAudioSource;

		// Token: 0x040043CE RID: 17358
		[SerializeField]
		private AudioClip[] _accelerationSounds;

		// Token: 0x040043CF RID: 17359
		[SerializeField]
		private AudioClip[] _decelerationSounds;

		// Token: 0x040043D0 RID: 17360
		[SerializeField]
		private AudioClip _startUpSound;

		// Token: 0x040043D1 RID: 17361
		[SerializeField]
		private AudioSource _smokeStackAudioSource;

		// Token: 0x040043D2 RID: 17362
		[SerializeField]
		private AudioClip _smokeSound;

		// Token: 0x040043D3 RID: 17363
		[SerializeField]
		private ParticleSystem _smoke1;

		// Token: 0x040043D4 RID: 17364
		[SerializeField]
		private ParticleSystem _smoke2;

		// Token: 0x040043D5 RID: 17365
		[SerializeField]
		private TrainCarBase[] _childCars;

		// Token: 0x040043D6 RID: 17366
		private bool _isMoving = true;

		// Token: 0x040043D7 RID: 17367
		private bool _reverse;

		// Token: 0x040043D8 RID: 17368
		private float _currentSpeed;

		// Token: 0x040043D9 RID: 17369
		private float _speedDiv;

		// Token: 0x040043DA RID: 17370
		private float _standardRateOverTimeMultiplier;

		// Token: 0x040043DB RID: 17371
		private int _standardMaxParticles;

		// Token: 0x040043DC RID: 17372
		private Coroutine _startStopTrainCr;

		// Token: 0x02000A76 RID: 2678
		private enum EngineSoundState
		{
			// Token: 0x040043DE RID: 17374
			Start,
			// Token: 0x040043DF RID: 17375
			AccelerateOrSetProperSpeed,
			// Token: 0x040043E0 RID: 17376
			Stop
		}
	}
}
