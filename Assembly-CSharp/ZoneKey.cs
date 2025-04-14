using System;

// Token: 0x020008F1 RID: 2289
[Serializable]
public struct ZoneKey : IEquatable<ZoneKey>, IComparable<ZoneKey>, IComparable
{
	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x060036F8 RID: 14072 RVA: 0x001048E6 File Offset: 0x00102AE6
	public int intValue
	{
		get
		{
			return ZoneKey.ToIntValue(this.zoneId, this.subZoneId);
		}
	}

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x060036F9 RID: 14073 RVA: 0x001048F9 File Offset: 0x00102AF9
	public string zoneName
	{
		get
		{
			return this.zoneId.GetName<GTZone>();
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x060036FA RID: 14074 RVA: 0x00104906 File Offset: 0x00102B06
	public string subZoneName
	{
		get
		{
			return this.subZoneId.GetName<GTSubZone>();
		}
	}

	// Token: 0x060036FB RID: 14075 RVA: 0x00104913 File Offset: 0x00102B13
	public ZoneKey(GTZone zone, GTSubZone subZone)
	{
		this.zoneId = zone;
		this.subZoneId = subZone;
	}

	// Token: 0x060036FC RID: 14076 RVA: 0x00104923 File Offset: 0x00102B23
	public override int GetHashCode()
	{
		return this.intValue;
	}

	// Token: 0x060036FD RID: 14077 RVA: 0x0010492B File Offset: 0x00102B2B
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"ZoneKey { ",
			this.zoneName,
			" : ",
			this.subZoneName,
			" }"
		});
	}

	// Token: 0x060036FE RID: 14078 RVA: 0x00104962 File Offset: 0x00102B62
	public static ZoneKey GetKey(GTZone zone, GTSubZone subZone)
	{
		return new ZoneKey(zone, subZone);
	}

	// Token: 0x060036FF RID: 14079 RVA: 0x0010496B File Offset: 0x00102B6B
	public static int ToIntValue(GTZone zone, GTSubZone subZone)
	{
		if (zone == GTZone.none && subZone == GTSubZone.none)
		{
			return 0;
		}
		return StaticHash.Compute(zone.GetLongValue<GTZone>(), subZone.GetLongValue<GTSubZone>());
	}

	// Token: 0x06003700 RID: 14080 RVA: 0x00104988 File Offset: 0x00102B88
	public bool Equals(ZoneKey other)
	{
		return this.intValue == other.intValue && this.zoneId == other.zoneId && this.subZoneId == other.subZoneId;
	}

	// Token: 0x06003701 RID: 14081 RVA: 0x001049B8 File Offset: 0x00102BB8
	public override bool Equals(object obj)
	{
		if (obj is ZoneKey)
		{
			ZoneKey other = (ZoneKey)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x001049DD File Offset: 0x00102BDD
	public static bool operator ==(ZoneKey x, ZoneKey y)
	{
		return x.Equals(y);
	}

	// Token: 0x06003703 RID: 14083 RVA: 0x001049E7 File Offset: 0x00102BE7
	public static bool operator !=(ZoneKey x, ZoneKey y)
	{
		return !x.Equals(y);
	}

	// Token: 0x06003704 RID: 14084 RVA: 0x001049F4 File Offset: 0x00102BF4
	public int CompareTo(ZoneKey other)
	{
		int num = this.intValue.CompareTo(other.intValue);
		if (num == 0)
		{
			num = string.CompareOrdinal(this.zoneName, other.zoneName);
		}
		if (num == 0)
		{
			num = string.CompareOrdinal(this.subZoneName, other.subZoneName);
		}
		return num;
	}

	// Token: 0x06003705 RID: 14085 RVA: 0x00104A44 File Offset: 0x00102C44
	public int CompareTo(object obj)
	{
		if (obj is ZoneKey)
		{
			ZoneKey other = (ZoneKey)obj;
			return this.CompareTo(other);
		}
		return 1;
	}

	// Token: 0x06003706 RID: 14086 RVA: 0x00104A69 File Offset: 0x00102C69
	public static bool operator <(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) < 0;
	}

	// Token: 0x06003707 RID: 14087 RVA: 0x00104A76 File Offset: 0x00102C76
	public static bool operator >(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) > 0;
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x00104A83 File Offset: 0x00102C83
	public static bool operator <=(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) <= 0;
	}

	// Token: 0x06003709 RID: 14089 RVA: 0x00104A93 File Offset: 0x00102C93
	public static bool operator >=(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) >= 0;
	}

	// Token: 0x0600370A RID: 14090 RVA: 0x00104AA3 File Offset: 0x00102CA3
	public static explicit operator int(ZoneKey key)
	{
		return key.intValue;
	}

	// Token: 0x04003A0E RID: 14862
	public GTZone zoneId;

	// Token: 0x04003A0F RID: 14863
	public GTSubZone subZoneId;

	// Token: 0x04003A10 RID: 14864
	public static readonly ZoneKey Null = new ZoneKey(GTZone.none, GTSubZone.none);
}
