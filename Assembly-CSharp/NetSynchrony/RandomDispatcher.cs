using System;
using System.Collections.Generic;
using UnityEngine;

namespace NetSynchrony
{
	// Token: 0x02000B12 RID: 2834
	[CreateAssetMenu(fileName = "RandomDispatcher", menuName = "NetSynchrony/RandomDispatcher", order = 0)]
	public class RandomDispatcher : ScriptableObject
	{
		// Token: 0x14000080 RID: 128
		// (add) Token: 0x060046E0 RID: 18144 RVA: 0x001507F8 File Offset: 0x0014E9F8
		// (remove) Token: 0x060046E1 RID: 18145 RVA: 0x00150830 File Offset: 0x0014EA30
		public event RandomDispatcher.RandomDispatcherEvent Dispatch;

		// Token: 0x060046E2 RID: 18146 RVA: 0x00150868 File Offset: 0x0014EA68
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

		// Token: 0x060046E3 RID: 18147 RVA: 0x0015090C File Offset: 0x0014EB0C
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

		// Token: 0x060046E4 RID: 18148 RVA: 0x00150960 File Offset: 0x0014EB60
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

		// Token: 0x04004856 RID: 18518
		[SerializeField]
		private float minWaitTime = 1f;

		// Token: 0x04004857 RID: 18519
		[SerializeField]
		private float maxWaitTime = 10f;

		// Token: 0x04004858 RID: 18520
		[SerializeField]
		private float totalMinutes = 60f;

		// Token: 0x04004859 RID: 18521
		private List<float> dispatchTimes;

		// Token: 0x0400485A RID: 18522
		private int index = -1;

		// Token: 0x02000B13 RID: 2835
		// (Invoke) Token: 0x060046E7 RID: 18151
		public delegate void RandomDispatcherEvent(RandomDispatcher randomDispatcher);
	}
}
