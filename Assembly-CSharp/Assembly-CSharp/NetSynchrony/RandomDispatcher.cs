﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace NetSynchrony
{
	// Token: 0x02000B15 RID: 2837
	[CreateAssetMenu(fileName = "RandomDispatcher", menuName = "NetSynchrony/RandomDispatcher", order = 0)]
	public class RandomDispatcher : ScriptableObject
	{
		// Token: 0x14000080 RID: 128
		// (add) Token: 0x060046EC RID: 18156 RVA: 0x00150DC0 File Offset: 0x0014EFC0
		// (remove) Token: 0x060046ED RID: 18157 RVA: 0x00150DF8 File Offset: 0x0014EFF8
		public event RandomDispatcher.RandomDispatcherEvent Dispatch;

		// Token: 0x060046EE RID: 18158 RVA: 0x00150E30 File Offset: 0x0014F030
		public void Init(double seconds)
		{
			seconds %= (double)(this.totalMinutes * 60f);
			this.index = 0;
			this.dispatchTimes = new List<float>();
			float num = 0f;
			float num2 = this.totalMinutes * 60f;
			Random.InitState(StaticHash.Compute(Application.buildGUID));
			while (num < num2)
			{
				float num3 = Random.Range(this.minWaitTime, this.maxWaitTime);
				num += num3;
				if ((double)num < seconds)
				{
					this.index = this.dispatchTimes.Count;
				}
				this.dispatchTimes.Add(num);
			}
			Random.InitState((int)DateTime.Now.Ticks);
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x00150ED4 File Offset: 0x0014F0D4
		public void Sync(double seconds)
		{
			seconds %= (double)(this.totalMinutes * 60f);
			this.index = 0;
			for (int i = 0; i < this.dispatchTimes.Count; i++)
			{
				if ((double)this.dispatchTimes[i] < seconds)
				{
					this.index = i;
				}
			}
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x00150F28 File Offset: 0x0014F128
		public void Tick(double seconds)
		{
			seconds %= (double)(this.totalMinutes * 60f);
			if ((double)this.dispatchTimes[this.index] < seconds)
			{
				this.index = (this.index + 1) % this.dispatchTimes.Count;
				if (this.Dispatch != null)
				{
					this.Dispatch(this);
				}
			}
		}

		// Token: 0x04004868 RID: 18536
		[SerializeField]
		private float minWaitTime = 1f;

		// Token: 0x04004869 RID: 18537
		[SerializeField]
		private float maxWaitTime = 10f;

		// Token: 0x0400486A RID: 18538
		[SerializeField]
		private float totalMinutes = 60f;

		// Token: 0x0400486B RID: 18539
		private List<float> dispatchTimes;

		// Token: 0x0400486C RID: 18540
		private int index = -1;

		// Token: 0x02000B16 RID: 2838
		// (Invoke) Token: 0x060046F3 RID: 18163
		public delegate void RandomDispatcherEvent(RandomDispatcher randomDispatcher);
	}
}
