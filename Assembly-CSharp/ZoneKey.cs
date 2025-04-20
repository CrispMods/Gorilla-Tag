using System;

// Token: 0x0200090E RID: 2318
[Serializable]
public struct ZoneKey : IEquatable<ZoneKey>, IComparable<ZoneKey>, IComparable
{
	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x060037C9 RID: 14281 RVA: 0x00054E04 File Offset: 0x00053004
	public int intValue
	{
		get
		{
			return ZoneKey.ToIntValue(this.zoneId, this.subZoneId);
		}
	}

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x060037CA RID: 14282 RVA: 0x00054E17 File Offset: 0x00053017
	public string zoneName
	{
		get
		{
			return this.zoneId.GetName<GTZone>();
		}
	}

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x060037CB RID: 14283 RVA: 0x00054E24 File Offset: 0x00053024
	public string subZoneName
	{
		get
		{
			return this.subZoneId.GetName<GTSubZone>();
		}
	}

	// Token: 0x060037CC RID: 14284 RVA: 0x00054E31 File Offset: 0x00053031
	public ZoneKey(GTZone zone, GTSubZone subZone)
	{
		this.zoneId = zone;
		this.subZoneId = subZone;
	}

	// Token: 0x060037CD RID: 14285 RVA: 0x00054E41 File Offset: 0x00053041
	public override int GetHashCode()
	{
		return this.intValue;
	}

	// Token: 0x060037CE RID: 14286 RVA: 0x00054E49 File Offset: 0x00053049
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

	// Token: 0x060037CF RID: 14287 RVA: 0x00054E80 File Offset: 0x00053080
	public static ZoneKey GetKey(GTZone zone, GTSubZone subZone)
	{
		return new ZoneKey(zone, subZone);
	}

	// Token: 0x060037D0 RID: 14288 RVA: 0x00054E89 File Offset: 0x00053089
	public static int ToIntValue(GTZone zone, GTSubZone subZone)
	{
		if (zone == GTZone.none && subZone == GTSubZone.none)
		{
			return 0;
		}
		return StaticHash.Compute(zone.GetLongValue<GTZone>(), subZone.GetLongValue<GTSubZone>());
	}

	// Token: 0x060037D1 RID: 14289 RVA: 0x00054EA6 File Offset: 0x000530A6
	public bool Equals(ZoneKey other)
	{
		return this.intValue == other.intValue && this.zoneId == other.zoneId && this.subZoneId == other.subZoneId;
	}

	// Token: 0x060037D2 RID: 14290 RVA: 0x00149914 File Offset: 0x00147B14
	public override bool Equals(object obj)
	{
		if (obj is ZoneKey)
		{
			ZoneKey other = (ZoneKey)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x060037D3 RID: 14291 RVA: 0x00054ED5 File Offset: 0x000530D5
	public static bool operator ==(ZoneKey x, ZoneKey y)
	{
		return x.Equals(y);
	}

	// Token: 0x060037D4 RID: 14292 RVA: 0x00054EDF File Offset: 0x000530DF
	public static bool operator !=(ZoneKey x, ZoneKey y)
	{
		return !x.Equals(y);
	}

	// Token: 0x060037D5 RID: 14293 RVA: 0x0014993C File Offset: 0x00147B3C
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

	// Token: 0x060037D6 RID: 14294 RVA: 0x0014998C File Offset: 0x00147B8C
	public int CompareTo(object obj)
	{
		if (obj is ZoneKey)
		{
			ZoneKey other = (ZoneKey)obj;
			return this.CompareTo(other);
		}
		return 1;
	}

	// Token: 0x060037D7 RID: 14295 RVA: 0x00054EEC File Offset: 0x000530EC
	public static bool operator <(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) < 0;
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x00054EF9 File Offset: 0x000530F9
	public static bool operator >(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) > 0;
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x00054F06 File Offset: 0x00053106
	public static bool operator <=(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) <= 0;
	}

	// Token: 0x060037DA RID: 14298 RVA: 0x00054F16 File Offset: 0x00053116
	public static bool operator >=(ZoneKey x, ZoneKey y)
	{
		return x.CompareTo(y) >= 0;
	}

	// Token: 0x060037DB RID: 14299 RVA: 0x00054F26 File Offset: 0x00053126
	public static explicit operator int(ZoneKey key)
	{
		return key.intValue;
	}

	// Token: 0x04003AD3 RID: 15059
	public GTZone zoneId;

	// Token: 0x04003AD4 RID: 15060
	public GTSubZone subZoneId;

	// Token: 0x04003AD5 RID: 15061
	public static readonly ZoneKey Null = new ZoneKey(GTZone.none, GTSubZone.none);
}
