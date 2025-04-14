using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A77 RID: 2679
	public class WindmillBladesController : MonoBehaviour
	{
		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060042C9 RID: 17097 RVA: 0x0013AD96 File Offset: 0x00138F96
		// (set) Token: 0x060042CA RID: 17098 RVA: 0x0013AD9E File Offset: 0x00138F9E
		public bool IsMoving { get; private set; }

		// Token: 0x060042CB RID: 17099 RVA: 0x0013ADA7 File Offset: 0x00138FA7
		private void Start()
		{
			this._originalRotation = base.transform.localRotation;
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x0013ADBC File Offset: 0x00138FBC
		private void Update()
		{
			this._rotAngle += this._currentSpeed * Time.deltaTime;
			if (this._rotAngle > 360f)
			{
				this._rotAngle = 0f;
			}
			base.transform.localRotation = this._originalRotation * Quaternion.AngleAxis(this._rotAngle, Vector3.forward);
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x0013AE20 File Offset: 0x00139020
		public void SetMoveState(bool newMoveState, float goalSpeed)
		{
			this.IsMoving = newMoveState;
			if (this._lerpSpeedCoroutine != null)
			{
				base.StopCoroutine(this._lerpSpeedCoroutine);
			}
			this._lerpSpeedCoroutine = base.StartCoroutine(this.LerpToSpeed(goalSpeed));
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x0013AE50 File Offset: 0x00139050
		private IEnumerator LerpToSpeed(float goalSpeed)
		{
			float totalTime = 0f;
			float startSpeed = this._currentSpeed;
			if (this._audioChangeCr != null)
			{
				base.StopCoroutine(this._audioChangeCr);
			}
			if (this.IsMoving)
			{
				this._audioChangeCr = base.StartCoroutine(this.PlaySoundDelayed(this._windMillStartSound, this._windMillRotationSound, this._windMillStartSound.length * 0.95f));
			}
			else
			{
				this.PlaySound(this._windMillStopSound, false);
			}
			for (float num = Mathf.Abs(this._currentSpeed - goalSpeed); num > Mathf.Epsilon; num = Mathf.Abs(this._currentSpeed - goalSpeed))
			{
				this._currentSpeed = Mathf.Lerp(startSpeed, goalSpeed, totalTime / 1f);
				totalTime += Time.deltaTime;
				yield return null;
			}
			this._lerpSpeedCoroutine = null;
			yield break;
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x0013AE66 File Offset: 0x00139066
		private IEnumerator PlaySoundDelayed(AudioClip initial, AudioClip clip, float timeDelayAfterInitial)
		{
			this.PlaySound(initial, false);
			yield return new WaitForSeconds(timeDelayAfterInitial);
			this.PlaySound(clip, true);
			yield break;
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x0013AE8A File Offset: 0x0013908A
		private void PlaySound(AudioClip clip, bool loop = false)
		{
			this._audioSource.loop = loop;
			this._audioSource.timeSamples = 0;
			this._audioSource.clip = clip;
			this._audioSource.Play();
		}

		// Token: 0x040043E1 RID: 17377
		private const float MAX_TIME = 1f;

		// Token: 0x040043E2 RID: 17378
		[SerializeField]
		private AudioSource _audioSource;

		// Token: 0x040043E3 RID: 17379
		[SerializeField]
		private AudioClip _windMillRotationSound;

		// Token: 0x040043E4 RID: 17380
		[SerializeField]
		private AudioClip _windMillStartSound;

		// Token: 0x040043E5 RID: 17381
		[SerializeField]
		private AudioClip _windMillStopSound;

		// Token: 0x040043E7 RID: 17383
		private float _currentSpeed;

		// Token: 0x040043E8 RID: 17384
		private Coroutine _lerpSpeedCoroutine;

		// Token: 0x040043E9 RID: 17385
		private Coroutine _audioChangeCr;

		// Token: 0x040043EA RID: 17386
		private Quaternion _originalRotation;

		// Token: 0x040043EB RID: 17387
		private float _rotAngle;
	}
}
