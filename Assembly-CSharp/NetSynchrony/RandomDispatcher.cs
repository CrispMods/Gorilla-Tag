using System;
using System.Collections.Generic;
using UnityEngine;

namespace NetSynchrony
{
	// Token: 0x02000B3F RID: 2879
	[CreateAssetMenu(fileName = "RandomDispatcher", menuName = "NetSynchrony/RandomDispatcher", order = 0)]
	public class RandomDispatcher : ScriptableObject
	{
		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06004829 RID: 18473 RVA: 0x0018D058 File Offset: 0x0018B258
		// (remove) Token: 0x0600482A RID: 18474 RVA: 0x0018D090 File Offset: 0x0018B290
		public event RandomDispatcher.RandomDispatcherEvent Dispatch;

		// Token: 0x0600482B RID: 18475 RVA: 0x0018D0C8 File Offset: 0x0018B2C8
		public void Init(double seconds)
		{
			seconds %= (double)(this.totalMinutes * 60f);
			this.index = 0;
			this.dispatchTimes = new List<float>();
			float num = 0f;
			float num2 = this.totalMinutes * 60f;
			UnityEngine.Random.InitState(StaticHash.Compute(Application.buildGUID));
			while (num < num2)
			{
				float num3 = UnityEngine.Random.Range(this.minWaitTime, this.maxWaitTime);
				num += num3;
				if ((double)num < seconds)
				{
					this.index = this.dispatchTimes.Count;
				}
				this.dispatchTimes.Add(num);
			}
			UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x0018D16C File Offset: 0x0018B36C
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

		// Token: 0x0600482D RID: 18477 RVA: 0x0018D1C0 File Offset: 0x0018B3C0
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

		// Token: 0x0400494B RID: 18763
		[SerializeField]
		private float minWaitTime = 1f;

		// Token: 0x0400494C RID: 18764
		[SerializeField]
		private float maxWaitTime = 10f;

		// Token: 0x0400494D RID: 18765
		[SerializeField]
		private float totalMinutes = 60f;

		// Token: 0x0400494E RID: 18766
		private List<float> dispatchTimes;

		// Token: 0x0400494F RID: 18767
		private int index = -1;

		// Token: 0x02000B40 RID: 2880
		// (Invoke) Token: 0x06004830 RID: 18480
		public delegate void RandomDispatcherEvent(RandomDispatcher randomDispatcher);
	}
}
