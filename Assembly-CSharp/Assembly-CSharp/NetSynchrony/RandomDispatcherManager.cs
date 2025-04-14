using System;
using GorillaNetworking;
using UnityEngine;

namespace NetSynchrony
{
	// Token: 0x02000B17 RID: 2839
	public class RandomDispatcherManager : MonoBehaviour
	{
		// Token: 0x060046F6 RID: 18166 RVA: 0x00150FBC File Offset: 0x0014F1BC
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

		// Token: 0x060046F7 RID: 18167 RVA: 0x00151008 File Offset: 0x0014F208
		private void OnTimeChanged()
		{
			this.AdjustedServerTime();
			for (int i = 0; i < this.randomDispatchers.Length; i++)
			{
				this.randomDispatchers[i].Sync(this.serverTime);
			}
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x00151044 File Offset: 0x0014F244
		private void AdjustedServerTime()
		{
			DateTime dateTime = new DateTime(2020, 1, 1);
			long num = GorillaComputer.instance.GetServerTime().Ticks - dateTime.Ticks;
			this.serverTime = (double)((float)num / 10000000f);
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x0015108C File Offset: 0x0014F28C
		private void Start()
		{
			GorillaComputer instance = GorillaComputer.instance;
			instance.OnServerTimeUpdated = (Action)Delegate.Combine(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
			for (int i = 0; i < this.randomDispatchers.Length; i++)
			{
				this.randomDispatchers[i].Init(this.serverTime);
			}
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x001510E8 File Offset: 0x0014F2E8
		private void Update()
		{
			for (int i = 0; i < this.randomDispatchers.Length; i++)
			{
				this.randomDispatchers[i].Tick(this.serverTime);
			}
			this.serverTime += (double)Time.deltaTime;
		}

		// Token: 0x0400486D RID: 18541
		[SerializeField]
		private RandomDispatcher[] randomDispatchers;

		// Token: 0x0400486E RID: 18542
		private static RandomDispatcherManager __instance;

		// Token: 0x0400486F RID: 18543
		private double serverTime;
	}
}
