using System;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020008AC RID: 2220
public class ServerTimeEvent : TimeEvent
{
	// Token: 0x060035C7 RID: 13767 RVA: 0x000FEF95 File Offset: 0x000FD195
	private void Awake()
	{
		this.eventTimes = new HashSet<ServerTimeEvent.EventTime>(this.times);
	}

	// Token: 0x060035C8 RID: 13768 RVA: 0x000FEFA8 File Offset: 0x000FD1A8
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

	// Token: 0x040037F9 RID: 14329
	[SerializeField]
	private ServerTimeEvent.EventTime[] times;

	// Token: 0x040037FA RID: 14330
	[SerializeField]
	private float queryTime = 60f;

	// Token: 0x040037FB RID: 14331
	private float lastQueryTime;

	// Token: 0x040037FC RID: 14332
	private HashSet<ServerTimeEvent.EventTime> eventTimes;

	// Token: 0x020008AD RID: 2221
	[Serializable]
	public struct EventTime
	{
		// Token: 0x060035CA RID: 13770 RVA: 0x000FF057 File Offset: 0x000FD257
		public EventTime(int h, int m)
		{
			this.hour = h;
			this.minute = m;
		}

		// Token: 0x040037FD RID: 14333
		public int hour;

		// Token: 0x040037FE RID: 14334
		public int minute;
	}
}
