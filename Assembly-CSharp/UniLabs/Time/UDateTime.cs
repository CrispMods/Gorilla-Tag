using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace UniLabs.Time
{
	// Token: 0x02000981 RID: 2433
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class UDateTime : ISerializationCallbackReceiver, IComparable<UDateTime>, IComparable<DateTime>
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06003B5A RID: 15194 RVA: 0x00056BBB File Offset: 0x00054DBB
		// (set) Token: 0x06003B5B RID: 15195 RVA: 0x00056BC3 File Offset: 0x00054DC3
		[JsonProperty("DateTime")]
		public DateTime DateTime { get; set; }

		// Token: 0x06003B5C RID: 15196 RVA: 0x00056BCC File Offset: 0x00054DCC
		[JsonConstructor]
		public UDateTime()
		{
			this.DateTime = DateTime.UnixEpoch;
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x00056BDF File Offset: 0x00054DDF
		public UDateTime(DateTime dateTime)
		{
			this.DateTime = dateTime;
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x00056BEE File Offset: 0x00054DEE
		public static implicit operator DateTime(UDateTime udt)
		{
			return udt.DateTime;
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x00056BF6 File Offset: 0x00054DF6
		public static implicit operator UDateTime(DateTime dt)
		{
			return new UDateTime
			{
				DateTime = dt
			};
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x00150C48 File Offset: 0x0014EE48
		public int CompareTo(DateTime other)
		{
			return this.DateTime.CompareTo(other);
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x00150C64 File Offset: 0x0014EE64
		public int CompareTo(UDateTime other)
		{
			if (this == other)
			{
				return 0;
			}
			if (other == null)
			{
				return 1;
			}
			return this.DateTime.CompareTo(other.DateTime);
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x00150C90 File Offset: 0x0014EE90
		protected bool Equals(UDateTime other)
		{
			return this.DateTime.Equals(other.DateTime);
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x00056C04 File Offset: 0x00054E04
		public override bool Equals(object obj)
		{
			return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((UDateTime)obj)));
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x00150CB4 File Offset: 0x0014EEB4
		public override int GetHashCode()
		{
			return this.DateTime.GetHashCode();
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x00150CD0 File Offset: 0x0014EED0
		public override string ToString()
		{
			return this.DateTime.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x00150CF0 File Offset: 0x0014EEF0
		public void OnAfterDeserialize()
		{
			DateTime dateTime;
			this.DateTime = (DateTime.TryParse(this._DateTime, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dateTime) ? dateTime : DateTime.MinValue);
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x00150D24 File Offset: 0x0014EF24
		public void OnBeforeSerialize()
		{
			this._DateTime = this.DateTime.ToString("o", CultureInfo.InvariantCulture);
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x00056C32 File Offset: 0x00054E32
		[OnSerializing]
		internal void OnSerializing(StreamingContext context)
		{
			this.OnBeforeSerialize();
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x00056C3A File Offset: 0x00054E3A
		[OnDeserialized]
		internal void OnDeserialized(StreamingContext context)
		{
			this.OnAfterDeserialize();
		}

		// Token: 0x04003C82 RID: 15490
		[HideInInspector]
		[SerializeField]
		private string _DateTime;
	}
}
