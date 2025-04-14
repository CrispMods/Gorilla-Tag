using System;
using GorillaNetworking;
using UnityEngine;

namespace NetSynchrony
{
	// Token: 0x02000B14 RID: 2836
	public class RandomDispatcherManager : MonoBehaviour
	{
		// Token: 0x060046EA RID: 18154 RVA: 0x001509F4 File Offset: 0x0014EBF4
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

		// Token: 0x060046EB RID: 18155 RVA: 0x00150A40 File Offset: 0x0014EC40
		private void OnTimeChanged()
		{
			this.AdjustedServerTime();
			for (int i = 0; i < this.randomDispatchers.Length; i++)
			{
				this.randomDispatchers[i].Sync(this.serverTime);
			}
		}

		// Token: 0x060046EC RID: 18156 RVA: 0x00150A7C File Offset: 0x0014EC7C
		private void AdjustedServerTime()
		{
			DateTime dateTime = new DateTime(2020, 1, 1);
			long num = GorillaComputer.instance.GetServerTime().Ticks - dateTime.Ticks;
			this.serverTime = (double)((float)num / 10000000f);
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x00150AC4 File Offset: 0x0014ECC4
		private void Start()
		{
			GorillaComputer instance = GorillaComputer.instance;
			instance.OnServerTimeUpdated = (Action)Delegate.Combine(instance.OnServerTimeUpdated, new Action(this.OnTimeChanged));
			for (int i = 0; i < this.randomDispatchers.Length; i++)
			{
				this.randomDispatchers[i].Init(this.serverTime);
			}
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x00150B20 File Offset: 0x0014ED20
		private void Update()
		{
			for (int i = 0; i < this.randomDispatchers.Length; i++)
			{
				this.randomDispatchers[i].Tick(this.serverTime);
			}
			this.serverTime += (double)Time.deltaTime;
		}

		// Token: 0x0400485B RID: 18523
		[SerializeField]
		private RandomDispatcher[] randomDispatchers;

		// Token: 0x0400485C RID: 18524
		private static RandomDispatcherManager __instance;

		// Token: 0x0400485D RID: 18525
		private double serverTime;
	}
}
