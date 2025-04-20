using System;
using System.Collections;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000AA4 RID: 2724
	public class WindmillBladesController : MonoBehaviour
	{
		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x0600440E RID: 17422 RVA: 0x0005C61E File Offset: 0x0005A81E
		// (set) Token: 0x0600440F RID: 17423 RVA: 0x0005C626 File Offset: 0x0005A826
		public bool IsMoving { get; private set; }

		// Token: 0x06004410 RID: 17424 RVA: 0x0005C62F File Offset: 0x0005A82F
		private void Start()
		{
			this._originalRotation = base.transform.localRotation;
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x00179FBC File Offset: 0x001781BC
		private void Update()
		{
			this._rotAngle += this._currentSpeed * Time.deltaTime;
			if (this._rotAngle > 360f)
			{
				this._rotAngle = 0f;
			}
			base.transform.localRotation = this._originalRotation * Quaternion.AngleAxis(this._rotAngle, Vector3.forward);
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x0005C642 File Offset: 0x0005A842
		public void SetMoveState(bool newMoveState, float goalSpeed)
		{
			this.IsMoving = newMoveState;
			if (this._lerpSpeedCoroutine != null)
			{
				base.StopCoroutine(this._lerpSpeedCoroutine);
			}
			this._lerpSpeedCoroutine = base.StartCoroutine(this.LerpToSpeed(goalSpeed));
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x0005C672 File Offset: 0x0005A872
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

		// Token: 0x06004414 RID: 17428 RVA: 0x0005C688 File Offset: 0x0005A888
		private IEnumerator PlaySoundDelayed(AudioClip initial, AudioClip clip, float timeDelayAfterInitial)
		{
			this.PlaySound(initial, false);
			yield return new WaitForSeconds(timeDelayAfterInitial);
			this.PlaySound(clip, true);
			yield break;
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x0005C6AC File Offset: 0x0005A8AC
		private void PlaySound(AudioClip clip, bool loop = false)
		{
			this._audioSource.loop = loop;
			this._audioSource.timeSamples = 0;
			this._audioSource.clip = clip;
			this._audioSource.Play();
		}

		// Token: 0x040044DB RID: 17627
		private const float MAX_TIME = 1f;

		// Token: 0x040044DC RID: 17628
		[SerializeField]
		private AudioSource _audioSource;

		// Token: 0x040044DD RID: 17629
		[SerializeField]
		private AudioClip _windMillRotationSound;

		// Token: 0x040044DE RID: 17630
		[SerializeField]
		private AudioClip _windMillStartSound;

		// Token: 0x040044DF RID: 17631
		[SerializeField]
		private AudioClip _windMillStopSound;

		// Token: 0x040044E1 RID: 17633
		private float _currentSpeed;

		// Token: 0x040044E2 RID: 17634
		private Coroutine _lerpSpeedCoroutine;

		// Token: 0x040044E3 RID: 17635
		private Coroutine _audioChangeCr;

		// Token: 0x040044E4 RID: 17636
		private Quaternion _originalRotation;

		// Token: 0x040044E5 RID: 17637
		private float _rotAngle;
	}
}
