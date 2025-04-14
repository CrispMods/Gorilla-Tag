using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009C2 RID: 2498
	public class GorillaTimer : MonoBehaviourPun
	{
		// Token: 0x06003E35 RID: 15925 RVA: 0x00126F69 File Offset: 0x00125169
		private void Awake()
		{
			this.ResetTimer();
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x00126F71 File Offset: 0x00125171
		public void StartTimer()
		{
			this.startTimer = true;
			UnityEvent<GorillaTimer> unityEvent = this.onTimerStarted;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x00126F8B File Offset: 0x0012518B
		public IEnumerator DelayedReStartTimer(float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			this.RestartTimer();
			yield break;
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x00126FA1 File Offset: 0x001251A1
		private void StopTimer()
		{
			this.startTimer = false;
			UnityEvent<GorillaTimer> unityEvent = this.onTimerStopped;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x00126FBB File Offset: 0x001251BB
		private void ResetTimer()
		{
			this.passedTime = 0f;
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x00126FC8 File Offset: 0x001251C8
		public void RestartTimer()
		{
			if (this.useRandomDuration)
			{
				this.SetTimerDuration(Random.Range(this.randTimeMin, this.randTimeMax));
			}
			this.ResetTimer();
			this.StartTimer();
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x00126FF5 File Offset: 0x001251F5
		public void SetTimerDuration(float timer)
		{
			this.timerDuration = timer;
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x00126FFE File Offset: 0x001251FE
		public void InvokeUpdate()
		{
			if (this.startTimer)
			{
				this.passedTime += Time.deltaTime;
			}
			if (this.startTimer && this.passedTime >= this.timerDuration)
			{
				this.StopTimer();
				this.ResetTimer();
			}
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x0012703C File Offset: 0x0012523C
		public float GetPassedTime()
		{
			return this.passedTime;
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x00127044 File Offset: 0x00125244
		public void SetPassedTime(float time)
		{
			this.passedTime = time;
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x0012704D File Offset: 0x0012524D
		public float GetRemainingTime()
		{
			return this.timerDuration - this.passedTime;
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x0012705C File Offset: 0x0012525C
		public void OnEnable()
		{
			GorillaTimerManager.RegisterGorillaTimer(this);
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x00127064 File Offset: 0x00125264
		public void OnDisable()
		{
			GorillaTimerManager.UnregisterGorillaTimer(this);
		}

		// Token: 0x04003F70 RID: 16240
		[SerializeField]
		private float timerDuration;

		// Token: 0x04003F71 RID: 16241
		[SerializeField]
		private bool useRandomDuration;

		// Token: 0x04003F72 RID: 16242
		[SerializeField]
		private float randTimeMin;

		// Token: 0x04003F73 RID: 16243
		[SerializeField]
		private float randTimeMax;

		// Token: 0x04003F74 RID: 16244
		private float passedTime;

		// Token: 0x04003F75 RID: 16245
		private bool startTimer;

		// Token: 0x04003F76 RID: 16246
		private bool resetTimer;

		// Token: 0x04003F77 RID: 16247
		public UnityEvent<GorillaTimer> onTimerStarted;

		// Token: 0x04003F78 RID: 16248
		public UnityEvent<GorillaTimer> onTimerStopped;
	}
}
