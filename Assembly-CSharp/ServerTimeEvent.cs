using System;
using System.Collections.Generic;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020008C8 RID: 2248
public class ServerTimeEvent : TimeEvent
{
	// Token: 0x0600368F RID: 13967 RVA: 0x00053F96 File Offset: 0x00052196
	private void Awake()
	{
		this.eventTimes = new HashSet<ServerTimeEvent.EventTime>(this.times);
	}

	// Token: 0x06003690 RID: 13968 RVA: 0x00144D4C File Offset: 0x00142F4C
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

	// Token: 0x040038BA RID: 14522
	[SerializeField]
	private ServerTimeEvent.EventTime[] times;

	// Token: 0x040038BB RID: 14523
	[SerializeField]
	private float queryTime = 60f;

	// Token: 0x040038BC RID: 14524
	private float lastQueryTime;

	// Token: 0x040038BD RID: 14525
	private HashSet<ServerTimeEvent.EventTime> eventTimes;

	// Token: 0x020008C9 RID: 2249
	[Serializable]
	public struct EventTime
	{
		// Token: 0x06003692 RID: 13970 RVA: 0x00053FBC File Offset: 0x000521BC
		public EventTime(int h, int m)
		{
			this.hour = h;
			this.minute = m;
		}

		// Token: 0x040038BE RID: 14526
		public int hour;

		// Token: 0x040038BF RID: 14527
		public int minute;
	}
}
