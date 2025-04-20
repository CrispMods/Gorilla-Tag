using System;
using Newtonsoft.Json;
using UnityEngine;

namespace UniLabs.Time
{
	// Token: 0x02000983 RID: 2435
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class UTimeSpanRange
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06003B7D RID: 15229 RVA: 0x00056D10 File Offset: 0x00054F10
		// (set) Token: 0x06003B7E RID: 15230 RVA: 0x00056D1D File Offset: 0x00054F1D
		public TimeSpan Start
		{
			get
			{
				return this._Start;
			}
			set
			{
				this._Start = value;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06003B7F RID: 15231 RVA: 0x00056D2B File Offset: 0x00054F2B
		// (set) Token: 0x06003B80 RID: 15232 RVA: 0x00056D38 File Offset: 0x00054F38
		public TimeSpan End
		{
			get
			{
				return this._End;
			}
			set
			{
				this._End = value;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06003B81 RID: 15233 RVA: 0x00056D46 File Offset: 0x00054F46
		public TimeSpan Duration
		{
			get
			{
				return this.End - this.Start;
			}
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x00056D59 File Offset: 0x00054F59
		public bool IsInRange(TimeSpan time)
		{
			return time >= this.Start && time <= this.End;
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x00030490 File Offset: 0x0002E690
		[JsonConstructor]
		public UTimeSpanRange()
		{
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x00056D77 File Offset: 0x00054F77
		public UTimeSpanRange(TimeSpan start)
		{
			this._Start = start;
			this._End = start;
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x00056D97 File Offset: 0x00054F97
		public UTimeSpanRange(TimeSpan start, TimeSpan end)
		{
			this._Start = start;
			this._End = end;
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x00056DB7 File Offset: 0x00054FB7
		private void OnStartChanged()
		{
			if (this._Start.CompareTo(this._End) > 0)
			{
				this._End.TimeSpan = this._Start.TimeSpan;
			}
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x00056DE3 File Offset: 0x00054FE3
		private void OnEndChanged()
		{
			if (this._End.CompareTo(this._Start) < 0)
			{
				this._Start.TimeSpan = this._End.TimeSpan;
			}
		}

		// Token: 0x04003C85 RID: 15493
		[JsonProperty("Start")]
		[SerializeField]
		private UTimeSpan _Start;

		// Token: 0x04003C86 RID: 15494
		[JsonProperty("End")]
		[SerializeField]
		private UTimeSpan _End;
	}
}
