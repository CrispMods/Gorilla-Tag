﻿using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009C5 RID: 2501
	public class GorillaTimer : MonoBehaviourPun
	{
		// Token: 0x06003E41 RID: 15937 RVA: 0x00057B7F File Offset: 0x00055D7F
		private void Awake()
		{
			this.ResetTimer();
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x00057B87 File Offset: 0x00055D87
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

		// Token: 0x06003E43 RID: 15939 RVA: 0x00057BA1 File Offset: 0x00055DA1
		public IEnumerator DelayedReStartTimer(float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			this.RestartTimer();
			yield break;
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x00057BB7 File Offset: 0x00055DB7
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

		// Token: 0x06003E45 RID: 15941 RVA: 0x00057BD1 File Offset: 0x00055DD1
		private void ResetTimer()
		{
			this.passedTime = 0f;
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x00057BDE File Offset: 0x00055DDE
		public void RestartTimer()
		{
			if (this.useRandomDuration)
			{
				this.SetTimerDuration(UnityEngine.Random.Range(this.randTimeMin, this.randTimeMax));
			}
			this.ResetTimer();
			this.StartTimer();
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x00057C0B File Offset: 0x00055E0B
		public void SetTimerDuration(float timer)
		{
			this.timerDuration = timer;
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x00057C14 File Offset: 0x00055E14
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

		// Token: 0x06003E49 RID: 15945 RVA: 0x00057C52 File Offset: 0x00055E52
		public float GetPassedTime()
		{
			return this.passedTime;
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00057C5A File Offset: 0x00055E5A
		public void SetPassedTime(float time)
		{
			this.passedTime = time;
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x00057C63 File Offset: 0x00055E63
		public float GetRemainingTime()
		{
			return this.timerDuration - this.passedTime;
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x00057C72 File Offset: 0x00055E72
		public void OnEnable()
		{
			GorillaTimerManager.RegisterGorillaTimer(this);
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x00057C7A File Offset: 0x00055E7A
		public void OnDisable()
		{
			GorillaTimerManager.UnregisterGorillaTimer(this);
		}

		// Token: 0x04003F82 RID: 16258
		[SerializeField]
		private float timerDuration;

		// Token: 0x04003F83 RID: 16259
		[SerializeField]
		private bool useRandomDuration;

		// Token: 0x04003F84 RID: 16260
		[SerializeField]
		private float randTimeMin;

		// Token: 0x04003F85 RID: 16261
		[SerializeField]
		private float randTimeMax;

		// Token: 0x04003F86 RID: 16262
		private float passedTime;

		// Token: 0x04003F87 RID: 16263
		private bool startTimer;

		// Token: 0x04003F88 RID: 16264
		private bool resetTimer;

		// Token: 0x04003F89 RID: 16265
		public UnityEvent<GorillaTimer> onTimerStarted;

		// Token: 0x04003F8A RID: 16266
		public UnityEvent<GorillaTimer> onTimerStopped;
	}
}
