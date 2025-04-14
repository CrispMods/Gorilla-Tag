using System;

// Token: 0x020008F4 RID: 2292
[Serializable]
public struct ZoneKey : IEquatable<ZoneKey>, IComparable<ZoneKey>, IComparable
{
	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06003704 RID: 14084 RVA: 0x00104EAE File Offset: 0x001030AE
	public int intValue
	{
		get
		{
			return ZoneKey.ToIntValue(this.zoneId, this.subZoneId);
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06003705 RID: 14085 RVA: 0x00104EC1 File Offset: 0x001030C1
	public string zoneName
	{
		get
		{
			return this.zoneId.GetName<GTZone>();
		}
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06003706 RID: 14086 RVA: 0x00104ECE File Offset: 0x001030CE
	public string subZoneName
	{
		get
		{
			return this.subZoneId.GetName<GTSubZone>();
		}
	}

	// Token: 0x06003707 RID: 14087 RVA: 0x00104EDB File Offset: 0x001030DB
	public ZoneKey(GTZone zone, GTSubZone subZone)
	{
		this.zoneId = zone;
		this.subZoneId = subZone;
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x00104EEB File Offset: 0x001030EB
	public override int GetHashCode()
	{
		return this.intValue;
	}

	// Token: 0x06003709 RID: 14089 RVA: 0x00104EF3 File Offset: 0x001030F3
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

	// Token: 0x0600370A RID: 14090 RVA: 0x00104F2A File Offset: 0x0010312A
	public static ZoneKey GetKey(GTZone zone, GTSubZone subZone)
	{
		return new ZoneKey(zone, subZone);
	}

	// Token: 0x0600370B RID: 14091 RVA: 0x00104F33 File Offset: 0x00103133
	public static int ToIntValue(GTZone zone, GTSubZone subZone)
	{
		if (zone == GTZone.none && subZone == GTSubZone.none)
		{
			return 0;
		}
		return StaticHash.Compute(zone.GetLongValue<GTZone>(), subZone.GetLongValue<GTSubZone>());
	}

	// Token: 0x0600370C RID: 14092 RVA: 0x00104F50 File Offset: 0x00103150
	public bool Equals(ZoneKey other)
	{
		return this.intValue == other.intValue && this.zoneId == other.zoneId && this.subZoneId == other.subZoneId;
	}

	// Token: 0x0600370D RID: 14093 RVA: 0x00104F80 File Offset: 0x00103180
	public override bool Equals(object obj)
	{
		if (obj is ZoneKey)
		{
			ZoneKey other = (ZoneKey)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x0600370E RID: 14094 RVA: 0x00104FA5 File Offset: 0x001031A5
	public static bool operator ==(ZoneKey x, ZoneKey y)
	{
		return x.Equals(y);
	}

	// Token: 0x0600370F RID: 14095 RVA: 0x00104FAF File Offset: 0x001031AF
	public static bool operator !=(ZoneKey x, ZoneKey y)
	{
		return !x.Equals(y);
	}

	// Token: 0x06003710 RID: 14096 RVA: 0x00104FBC File Offset: 0x001031BC
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

	// Token: 0x06003711 RID: 14097 RVA: 0x0010500C File Offset: 0x0010320C
	public int CompareTo(object obj)
	{
		if (obj is ZoneKey)
		{
			ZoneKey other = (ZoneKey)obj;
			return this.CompareTo(other);
		}
		return 1;
	}

	// Token: 0x06003712 RID: 14098 RVA: 0x00105031 File Offset: 0x00103231
	public static bool operator <(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) < 0;
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x0010503E File Offset: 0x0010323E
	public static bool operator >(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) > 0;
	}

	// Token: 0x06003714 RID: 14100 RVA: 0x0010504B File Offset: 0x0010324B
	public static bool operator <=(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) <= 0;
	}

	// Token: 0x06003715 RID: 14101 RVA: 0x0010505B File Offset: 0x0010325B
	public static bool operator >=(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) >= 0;
	}

	// Token: 0x06003716 RID: 14102 RVA: 0x0010506B File Offset: 0x0010326B
	public static explicit operator int(ZoneKey key)
	{
		return key.intValue;
	}

	// Token: 0x04003A20 RID: 14880
	public GTZone zoneId;

	// Token: 0x04003A21 RID: 14881
	public GTSubZone subZoneId;

	// Token: 0x04003A22 RID: 14882
	public static readonly ZoneKey Null = new ZoneKey(GTZone.none, GTSubZone.none);
}
