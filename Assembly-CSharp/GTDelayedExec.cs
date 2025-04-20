using System;
using UnityEngine;

// Token: 0x0200080D RID: 2061
public class GTDelayedExec : ITickSystemTick
{
	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x060032DC RID: 13020 RVA: 0x00051943 File Offset: 0x0004FB43
	// (set) Token: 0x060032DD RID: 13021 RVA: 0x0005194A File Offset: 0x0004FB4A
	public static GTDelayedExec instance { get; private set; }

	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x060032DE RID: 13022 RVA: 0x00051952 File Offset: 0x0004FB52
	// (set) Token: 0x060032DF RID: 13023 RVA: 0x0005195A File Offset: 0x0004FB5A
	public int listenerCount { get; private set; }

	// Token: 0x060032E0 RID: 13024 RVA: 0x00051963 File Offset: 0x0004FB63
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void InitializeAfterAssemblies()
	{
		GTDelayedExec.instance = new GTDelayedExec();
		TickSystem<object>.AddTickCallback(GTDelayedExec.instance);
	}

	// Token: 0x060032E1 RID: 13025 RVA: 0x00138F4C File Offset: 0x0013714C
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

	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x060032E2 RID: 13026 RVA: 0x00051979 File Offset: 0x0004FB79
	// (set) Token: 0x060032E3 RID: 13027 RVA: 0x00051981 File Offset: 0x0004FB81
	bool ITickSystemTick.TickRunning { get; set; }

	// Token: 0x060032E4 RID: 13028 RVA: 0x00138FBC File Offset: 0x001371BC
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

	// Token: 0x04003668 RID: 13928
	public const int kMaxListeners = 1024;

	// Token: 0x0400366A RID: 13930
	private static readonly float[] _listenerDelays = new float[1024];

	// Token: 0x0400366B RID: 13931
	private static readonly GTDelayedExec.Listener[] _listeners = new GTDelayedExec.Listener[1024];

	// Token: 0x0200080E RID: 2062
	private struct Listener
	{
		// Token: 0x060032E7 RID: 13031 RVA: 0x000519AA File Offset: 0x0004FBAA
		public Listener(IDelayedExecListener listener, int contextId)
		{
			this.listener = listener;
			this.contextId = contextId;
		}

		// Token: 0x0400366D RID: 13933
		public readonly IDelayedExecListener listener;

		// Token: 0x0400366E RID: 13934
		public readonly int contextId;
	}
}
