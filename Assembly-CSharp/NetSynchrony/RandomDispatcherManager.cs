using System;
using GorillaNetworking;
using UnityEngine;

namespace NetSynchrony
{
	// Token: 0x02000B41 RID: 2881
	public class RandomDispatcherManager : MonoBehaviour
	{
		// Token: 0x06004833 RID: 18483 RVA: 0x0018D224 File Offset: 0x0018B424
		private void OnDisable()
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			if (GorillaComputer.instance != null)
			{
				GorillaComputer instance = GorillaComputer.instance;
				instance.OnServerTimeUpdated = (Action)Delegate.Remove(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
			}
		}

		// Token: 0x06004834 RID: 18484 RVA: 0x0018D270 File Offset: 0x0018B470
		private void OnTimeChanged()
		{
			this.AdjustedServerTime();
			for (int i = 0; i < this.randomDispatchers.Length; i++)
			{
				this.randomDispatchers[i].Sync(this.serverTime);
			}
		}

		// Token: 0x06004835 RID: 18485 RVA: 0x0018D2AC File Offset: 0x0018B4AC
		private void AdjustedServerTime()
		{
			DateTime dateTime = new DateTime(2020, 1, 1);
			long num = GorillaComputer.instance.GetServerTime().Ticks - dateTime.Ticks;
			this.serverTime = (double)((float)num / 10000000f);
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x0018D2F4 File Offset: 0x0018B4F4
		private void Start()
		{
			GorillaComputer instance = GorillaComputer.instance;
			instance.OnServerTimeUpdated = (Action)Delegate.Combine(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
			for (int i = 0; i < this.randomDispatchers.Length; i++)
			{
				this.randomDispatchers[i].Init(this.serverTime);
			}
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x0018D350 File Offset: 0x0018B550
		private void Update()
		{
			for (int i = 0; i < this.randomDispatchers.Length; i++)
			{
				this.randomDispatchers[i].Tick(this.serverTime);
			}
			this.serverTime += (double)Time.deltaTime;
		}

		// Token: 0x04004950 RID: 18768
		[SerializeField]
		private RandomDispatcher[] randomDispatchers;

		// Token: 0x04004951 RID: 18769
		private static RandomDispatcherManager __instance;

		// Token: 0x04004952 RID: 18770
		private double serverTime;
	}
}
