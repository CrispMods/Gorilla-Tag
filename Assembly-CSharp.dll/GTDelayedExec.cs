using System;
using UnityEngine;

// Token: 0x020007F6 RID: 2038
public class GTDelayedExec : ITickSystemTick
{
	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06003232 RID: 12850 RVA: 0x00050541 File Offset: 0x0004E741
	// (set) Token: 0x06003233 RID: 12851 RVA: 0x00050548 File Offset: 0x0004E748
	public static GTDelayedExec instance { get; private set; }

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06003234 RID: 12852 RVA: 0x00050550 File Offset: 0x0004E750
	// (set) Token: 0x06003235 RID: 12853 RVA: 0x00050558 File Offset: 0x0004E758
	public int listenerCount { get; private set; }

	// Token: 0x06003236 RID: 12854 RVA: 0x00050561 File Offset: 0x0004E761
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void InitializeAfterAssemblies()
	{
		GTDelayedExec.instance = new GTDelayedExec();
		TickSystem<object>.AddTickCallback(GTDelayedExec.instance);
	}

	// Token: 0x06003237 RID: 12855 RVA: 0x00133D2C File Offset: 0x00131F2C
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

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06003238 RID: 12856 RVA: 0x00050577 File Offset: 0x0004E777
	// (set) Token: 0x06003239 RID: 12857 RVA: 0x0005057F File Offset: 0x0004E77F
	bool ITickSystemTick.TickRunning { get; set; }

	// Token: 0x0600323A RID: 12858 RVA: 0x00133D9C File Offset: 0x00131F9C
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

	// Token: 0x040035BF RID: 13759
	public const int kMaxListeners = 1024;

	// Token: 0x040035C1 RID: 13761
	private static readonly float[] _listenerDelays = new float[1024];

	// Token: 0x040035C2 RID: 13762
	private static readonly GTDelayedExec.Listener[] _listeners = new GTDelayedExec.Listener[1024];

	// Token: 0x020007F7 RID: 2039
	private struct Listener
	{
		// Token: 0x0600323D RID: 12861 RVA: 0x000505A8 File Offset: 0x0004E7A8
		public Listener(IDelayedExecListener listener, int contextId)
		{
			this.listener = listener;
			this.contextId = contextId;
		}

		// Token: 0x040035C4 RID: 13764
		public readonly IDelayedExecListener listener;

		// Token: 0x040035C5 RID: 13765
		public readonly int contextId;
	}
}
