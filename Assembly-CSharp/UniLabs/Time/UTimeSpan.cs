using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace UniLabs.Time
{
	// Token: 0x02000982 RID: 2434
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class UTimeSpan : ISerializationCallbackReceiver, IComparable<UTimeSpan>, IComparable<TimeSpan>
	{
		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06003B6A RID: 15210 RVA: 0x00056C42 File Offset: 0x00054E42
		// (set) Token: 0x06003B6B RID: 15211 RVA: 0x00056C4A File Offset: 0x00054E4A
		[JsonProperty("TimeSpan")]
		public TimeSpan TimeSpan { get; set; }

		// Token: 0x06003B6C RID: 15212 RVA: 0x00056C53 File Offset: 0x00054E53
		[JsonConstructor]
		public UTimeSpan()
		{
			this.TimeSpan = TimeSpan.Zero;
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x00056C66 File Offset: 0x00054E66
		public UTimeSpan(TimeSpan timeSpan)
		{
			this.TimeSpan = timeSpan;
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x00056C75 File Offset: 0x00054E75
		public UTimeSpan(long ticks) : this(new TimeSpan(ticks))
		{
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x00056C83 File Offset: 0x00054E83
		public UTimeSpan(int hours, int minutes, int seconds) : this(new TimeSpan(hours, minutes, seconds))
		{
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x00056C93 File Offset: 0x00054E93
		public UTimeSpan(int days, int hours, int minutes, int seconds) : this(new TimeSpan(days, hours, minutes, seconds))
		{
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x00056CA5 File Offset: 0x00054EA5
		public UTimeSpan(int days, int hours, int minutes, int seconds, int milliseconds) : this(new TimeSpan(days, hours, minutes, seconds, milliseconds))
		{
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x00056CB9 File Offset: 0x00054EB9
		public static implicit operator TimeSpan(UTimeSpan uTimeSpan)
		{
			if (uTimeSpan == null)
			{
				return TimeSpan.Zero;
			}
			return uTimeSpan.TimeSpan;
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x00056CCA File Offset: 0x00054ECA
		public static implicit operator UTimeSpan(TimeSpan timeSpan)
		{
			return new UTimeSpan(timeSpan);
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x00150D50 File Offset: 0x0014EF50
		public int CompareTo(TimeSpan other)
		{
			return this.TimeSpan.CompareTo(other);
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x00150D6C File Offset: 0x0014EF6C
		public int CompareTo(UTimeSpan other)
		{
			if (this == other)
			{
				return 0;
			}
			if (other == null)
			{
				return 1;
			}
			return this.TimeSpan.CompareTo(other.TimeSpan);
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x00150D98 File Offset: 0x0014EF98
		protected bool Equals(UTimeSpan other)
		{
			return this.TimeSpan.Equals(other.TimeSpan);
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x00056CD2 File Offset: 0x00054ED2
		public override bool Equals(object obj)
		{
			return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((UTimeSpan)obj)));
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x00150DBC File Offset: 0x0014EFBC
		public override int GetHashCode()
		{
			return this.TimeSpan.GetHashCode();
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x00150DE0 File Offset: 0x0014EFE0
		public void OnAfterDeserialize()
		{
			TimeSpan timeSpan;
			this.TimeSpan = (TimeSpan.TryParse(this._TimeSpan, CultureInfo.InvariantCulture, out timeSpan) ? timeSpan : TimeSpan.Zero);
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x00150E10 File Offset: 0x0014F010
		public void OnBeforeSerialize()
		{
			this._TimeSpan = this.TimeSpan.ToString();
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x00056D00 File Offset: 0x00054F00
		[OnSerializing]
		internal void OnSerializingMethod(StreamingContext context)
		{
			this.OnBeforeSerialize();
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x00056D08 File Offset: 0x00054F08
		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			this.OnAfterDeserialize();
		}

		// Token: 0x04003C84 RID: 15492
		[HideInInspector]
		[SerializeField]
		private string _TimeSpan;
	}
}
