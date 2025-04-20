using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009E8 RID: 2536
	public class GorillaTimer : MonoBehaviourPun
	{
		// Token: 0x06003F4D RID: 16205 RVA: 0x00059416 File Offset: 0x00057616
		private void Awake()
		{
			this.ResetTimer();
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x0005941E File Offset: 0x0005761E
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

		// Token: 0x06003F4F RID: 16207 RVA: 0x00059438 File Offset: 0x00057638
		public IEnumerator DelayedReStartTimer(float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			this.RestartTimer();
			yield break;
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x0005944E File Offset: 0x0005764E
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

		// Token: 0x06003F51 RID: 16209 RVA: 0x00059468 File Offset: 0x00057668
		private void ResetTimer()
		{
			this.passedTime = 0f;
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x00059475 File Offset: 0x00057675
		public void RestartTimer()
		{
			if (this.useRandomDuration)
			{
				this.SetTimerDuration(UnityEngine.Random.Range(this.randTimeMin, this.randTimeMax));
			}
			this.ResetTimer();
			this.StartTimer();
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x000594A2 File Offset: 0x000576A2
		public void SetTimerDuration(float timer)
		{
			this.timerDuration = timer;
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x000594AB File Offset: 0x000576AB
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

		// Token: 0x06003F55 RID: 16213 RVA: 0x000594E9 File Offset: 0x000576E9
		public float GetPassedTime()
		{
			return this.passedTime;
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x000594F1 File Offset: 0x000576F1
		public void SetPassedTime(float time)
		{
			this.passedTime = time;
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x000594FA File Offset: 0x000576FA
		public float GetRemainingTime()
		{
			return this.timerDuration - this.passedTime;
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x00059509 File Offset: 0x00057709
		public void OnEnable()
		{
			GorillaTimerManager.RegisterGorillaTimer(this);
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x00059511 File Offset: 0x00057711
		public void OnDisable()
		{
			GorillaTimerManager.UnregisterGorillaTimer(this);
		}

		// Token: 0x0400404A RID: 16458
		[SerializeField]
		private float timerDuration;

		// Token: 0x0400404B RID: 16459
		[SerializeField]
		private bool useRandomDuration;

		// Token: 0x0400404C RID: 16460
		[SerializeField]
		private float randTimeMin;

		// Token: 0x0400404D RID: 16461
		[SerializeField]
		private float randTimeMax;

		// Token: 0x0400404E RID: 16462
		private float passedTime;

		// Token: 0x0400404F RID: 16463
		private bool startTimer;

		// Token: 0x04004050 RID: 16464
		private bool resetTimer;

		// Token: 0x04004051 RID: 16465
		public UnityEvent<GorillaTimer> onTimerStarted;

		// Token: 0x04004052 RID: 16466
		public UnityEvent<GorillaTimer> onTimerStopped;
	}
}
