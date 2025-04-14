using System;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020008AF RID: 2223
public class ServerTimeEvent : TimeEvent
{
	// Token: 0x060035D3 RID: 13779 RVA: 0x000FF55D File Offset: 0x000FD75D
	private void Awake()
	{
		this.eventTimes = new HashSet<ServerTimeEvent.EventTime>(this.times);
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x000FF570 File Offset: 0x000FD770
	private void Update()
	{
		if (GorillaComputer.instance == null || Time.time - this.lastQueryTime < this.queryTime)
		{
			return;
		}
		ServerTimeEvent.EventTime item = new ServerTimeEvent.EventTime(GorillaComputer.instance.GetServerTime().Hour, GorillaComputer.instance.GetServerTime().Minute);
		bool flag = this.eventTimes.Contains(item);
		if (!this._ongoing && flag)
		{
			base.StartEvent();
		}
		if (this._ongoing && !flag)
		{
			base.StopEvent();
		}
		this.lastQueryTime = Time.time;
	}

	// Token: 0x0400380B RID: 14347
	[SerializeField]
	private ServerTimeEvent.EventTime[] times;

	// Token: 0x0400380C RID: 14348
	[SerializeField]
	private float queryTime = 60f;

	// Token: 0x0400380D RID: 14349
	private float lastQueryTime;

	// Token: 0x0400380E RID: 14350
	private HashSet<ServerTimeEvent.EventTime> eventTimes;

	// Token: 0x020008B0 RID: 2224
	[Serializable]
	public struct EventTime
	{
		// Token: 0x060035D6 RID: 13782 RVA: 0x000FF61F File Offset: 0x000FD81F
		public EventTime(int h, int m)
		{
			this.hour = h;
			this.minute = m;
		}

		// Token: 0x0400380F RID: 14351
		public int hour;

		// Token: 0x04003810 RID: 14352
		public int minute;
	}
}
