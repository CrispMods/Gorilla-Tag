using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A7A RID: 2682
	public class WindmillBladesController : MonoBehaviour
	{
		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060042D5 RID: 17109 RVA: 0x0005AC1C File Offset: 0x00058E1C
		// (set) Token: 0x060042D6 RID: 17110 RVA: 0x0005AC24 File Offset: 0x00058E24
		public bool IsMoving { get; private set; }

		// Token: 0x060042D7 RID: 17111 RVA: 0x0005AC2D File Offset: 0x00058E2D
		private void Start()
		{
			this._originalRotation = base.transform.localRotation;
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x00173138 File Offset: 0x00171338
		private void Update()
		{
			this._rotAngle += this._currentSpeed * Time.deltaTime;
			if (this._rotAngle > 360f)
			{
				this._rotAngle = 0f;
			}
			base.transform.localRotation = this._originalRotation * Quaternion.AngleAxis(this._rotAngle, Vector3.forward);
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x0005AC40 File Offset: 0x00058E40
		public void SetMoveState(bool newMoveState, float goalSpeed)
		{
			this.IsMoving = newMoveState;
			if (this._lerpSpeedCoroutine != null)
			{
				base.StopCoroutine(this._lerpSpeedCoroutine);
			}
			this._lerpSpeedCoroutine = base.StartCoroutine(this.LerpToSpeed(goalSpeed));
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x0005AC70 File Offset: 0x00058E70
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

		// Token: 0x060042DB RID: 17115 RVA: 0x0005AC86 File Offset: 0x00058E86
		private IEnumerator PlaySoundDelayed(AudioClip initial, AudioClip clip, float timeDelayAfterInitial)
		{
			this.PlaySound(initial, false);
			yield return new WaitForSeconds(timeDelayAfterInitial);
			this.PlaySound(clip, true);
			yield break;
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x0005ACAA File Offset: 0x00058EAA
		private void PlaySound(AudioClip clip, bool loop = false)
		{
			this._audioSource.loop = loop;
			this._audioSource.timeSamples = 0;
			this._audioSource.clip = clip;
			this._audioSource.Play();
		}

		// Token: 0x040043F3 RID: 17395
		private const float MAX_TIME = 1f;

		// Token: 0x040043F4 RID: 17396
		[SerializeField]
		private AudioSource _audioSource;

		// Token: 0x040043F5 RID: 17397
		[SerializeField]
		private AudioClip _windMillRotationSound;

		// Token: 0x040043F6 RID: 17398
		[SerializeField]
		private AudioClip _windMillStartSound;

		// Token: 0x040043F7 RID: 17399
		[SerializeField]
		private AudioClip _windMillStopSound;

		// Token: 0x040043F9 RID: 17401
		private float _currentSpeed;

		// Token: 0x040043FA RID: 17402
		private Coroutine _lerpSpeedCoroutine;

		// Token: 0x040043FB RID: 17403
		private Coroutine _audioChangeCr;

		// Token: 0x040043FC RID: 17404
		private Quaternion _originalRotation;

		// Token: 0x040043FD RID: 17405
		private float _rotAngle;
	}
}
