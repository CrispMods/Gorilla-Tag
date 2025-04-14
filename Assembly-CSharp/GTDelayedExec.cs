using System;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
public class GTDelayedExec : ITickSystemTick
{
	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06003226 RID: 12838 RVA: 0x000F0C8E File Offset: 0x000EEE8E
	// (set) Token: 0x06003227 RID: 12839 RVA: 0x000F0C95 File Offset: 0x000EEE95
	public static GTDelayedExec instance { get; private set; }

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06003228 RID: 12840 RVA: 0x000F0C9D File Offset: 0x000EEE9D
	// (set) Token: 0x06003229 RID: 12841 RVA: 0x000F0CA5 File Offset: 0x000EEEA5
	public int listenerCount { get; private set; }

	// Token: 0x0600322A RID: 12842 RVA: 0x000F0CAE File Offset: 0x000EEEAE
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void InitializeAfterAssemblies()
	{
		GTDelayedExec.instance = new GTDelayedExec();
		TickSystem<object>.AddTickCallback(GTDelayedExec.instance);
	}

	// Token: 0x0600322B RID: 12843 RVA: 0x000F0CC4 File Offset: 0x000EEEC4
	internal static void Add(IDelayedExecListener listener, float delay, int contextId)
	{
		if (GTDelayedExec.instance.listenerCount >= 1024)
		{
			Debug.LogError("Maximum number of delayed listeners reached.");
			return;
		}
		GTDelayedExec._listenerDelays[GTDelayedExec.instance.listenerCount] = Time.unscaledTime + delay;
		GTDelayedExec._listeners[GTDelayedExec.instance.listenerCount] = new GTDelayedExec.Listener(listener, contextId);
		GTDelayedExec instance = GTDelayedExec.instance;
		int listenerCount = instance.listenerCount;
		instance.listenerCount = listenerCount + 1;
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x0600322C RID: 12844 RVA: 0x000F0D33 File Offset: 0x000EEF33
	// (set) Token: 0x0600322D RID: 12845 RVA: 0x000F0D3B File Offset: 0x000EEF3B
	bool ITickSystemTick.TickRunning { get; set; }

	// Token: 0x0600322E RID: 12846 RVA: 0x000F0D44 File Offset: 0x000EEF44
	void ITickSystemTick.Tick()
	{
		for (int i = 0; i < this.listenerCount; i++)
		{
			if (Time.unscaledTime >= GTDelayedExec._listenerDelays[i])
			{
				try
				{
					GTDelayedExec._listeners[i].listener.OnDelayedAction(GTDelayedExec._listeners[i].contextId);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				int listenerCount = this.listenerCount;
				this.listenerCount = listenerCount - 1;
				GTDelayedExec._listenerDelays[i] = GTDelayedExec._listenerDelays[this.listenerCount];
				GTDelayedExec._listeners[i] = GTDelayedExec._listeners[this.listenerCount];
				i--;
			}
		}
	}

	// Token: 0x040035AD RID: 13741
	public const int kMaxListeners = 1024;

	// Token: 0x040035AF RID: 13743
	private static readonly float[] _listenerDelays = new float[1024];

	// Token: 0x040035B0 RID: 13744
	private static readonly GTDelayedExec.Listener[] _listeners = new GTDelayedExec.Listener[1024];

	// Token: 0x020007F4 RID: 2036
	private struct Listener
	{
		// Token: 0x06003231 RID: 12849 RVA: 0x000F0E18 File Offset: 0x000EF018
		public Listener(IDelayedExecListener listener, int contextId)
		{
			this.listener = listener;
			this.contextId = contextId;
		}

		// Token: 0x040035B2 RID: 13746
		public readonly IDelayedExecListener listener;

		// Token: 0x040035B3 RID: 13747
		public readonly int contextId;
	}
}
