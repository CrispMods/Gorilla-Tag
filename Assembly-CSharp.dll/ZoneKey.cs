using System;

// Token: 0x020008F4 RID: 2292
[Serializable]
public struct ZoneKey : IEquatable<ZoneKey>, IComparable<ZoneKey>, IComparable
{
	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06003704 RID: 14084 RVA: 0x00053862 File Offset: 0x00051A62
	public int intValue
	{
		get
		{
			return ZoneKey.ToIntValue(this.zoneId, this.subZoneId);
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06003705 RID: 14085 RVA: 0x00053875 File Offset: 0x00051A75
	public string zoneName
	{
		get
		{
			return this.zoneId.GetName<GTZone>();
		}
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06003706 RID: 14086 RVA: 0x00053882 File Offset: 0x00051A82
	public string subZoneName
	{
		get
		{
			return this.subZoneId.GetName<GTSubZone>();
		}
	}

	// Token: 0x06003707 RID: 14087 RVA: 0x0005388F File Offset: 0x00051A8F
	public ZoneKey(GTZone zone, GTSubZone subZone)
	{
		this.zoneId = zone;
		this.subZoneId = subZone;
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x0005389F File Offset: 0x00051A9F
	public override int GetHashCode()
	{
		return this.intValue;
	}

	// Token: 0x06003709 RID: 14089 RVA: 0x000538A7 File Offset: 0x00051AA7
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

	// Token: 0x0600370A RID: 14090 RVA: 0x000538DE File Offset: 0x00051ADE
	public static ZoneKey GetKey(GTZone zone, GTSubZone subZone)
	{
		return new ZoneKey(zone, subZone);
	}

	// Token: 0x0600370B RID: 14091 RVA: 0x000538E7 File Offset: 0x00051AE7
	public static int ToIntValue(GTZone zone, GTSubZone subZone)
	{
		if (zone == GTZone.none && subZone == GTSubZone.none)
		{
			return 0;
		}
		return StaticHash.Compute(zone.GetLongValue<GTZone>(), subZone.GetLongValue<GTSubZone>());
	}

	// Token: 0x0600370C RID: 14092 RVA: 0x00053904 File Offset: 0x00051B04
	public bool Equals(ZoneKey other)
	{
		return this.intValue == other.intValue && this.zoneId == other.zoneId && this.subZoneId == other.subZoneId;
	}

	// Token: 0x0600370D RID: 14093 RVA: 0x001442C8 File Offset: 0x001424C8
	public override bool Equals(object obj)
	{
		if (obj is ZoneKey)
		{
			ZoneKey other = (ZoneKey)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x0600370E RID: 14094 RVA: 0x00053933 File Offset: 0x00051B33
	public static bool operator ==(ZoneKey x, ZoneKey y)
	{
		return x.Equals(y);
	}

	// Token: 0x0600370F RID: 14095 RVA: 0x0005393D File Offset: 0x00051B3D
	public static bool operator !=(ZoneKey x, ZoneKey y)
	{
		return !x.Equals(y);
	}

	// Token: 0x06003710 RID: 14096 RVA: 0x001442F0 File Offset: 0x001424F0
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

	// Token: 0x06003711 RID: 14097 RVA: 0x00144340 File Offset: 0x00142540
	public int CompareTo(object obj)
	{
		if (obj is ZoneKey)
		{
			ZoneKey other = (ZoneKey)obj;
			return this.CompareTo(other);
		}
		return 1;
	}

	// Token: 0x06003712 RID: 14098 RVA: 0x0005394A File Offset: 0x00051B4A
	public static bool operator <(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) < 0;
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x00053957 File Offset: 0x00051B57
	public static bool operator >(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) > 0;
	}

	// Token: 0x06003714 RID: 14100 RVA: 0x00053964 File Offset: 0x00051B64
	public static bool operator <=(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) <= 0;
	}

	// Token: 0x06003715 RID: 14101 RVA: 0x00053974 File Offset: 0x00051B74
	public static bool operator >=(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) >= 0;
	}

	// Token: 0x06003716 RID: 14102 RVA: 0x00053984 File Offset: 0x00051B84
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
