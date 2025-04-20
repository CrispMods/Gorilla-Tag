using System;

// Token: 0x0200053D RID: 1341
[Serializable]
public struct GroupJoinZoneAB
{
	// Token: 0x06002072 RID: 8306 RVA: 0x000F2F54 File Offset: 0x000F1154
	public static GroupJoinZoneAB operator &(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return new GroupJoinZoneAB
		{
			a = (one.a & two.a),
			b = (one.b & two.b)
		};
	}

	// Token: 0x06002073 RID: 8307 RVA: 0x000F2F94 File Offset: 0x000F1194
	public static GroupJoinZoneAB operator |(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return new GroupJoinZoneAB
		{
			a = (one.a | two.a),
			b = (one.b | two.b)
		};
	}

	// Token: 0x06002074 RID: 8308 RVA: 0x000F2FD4 File Offset: 0x000F11D4
	public static GroupJoinZoneAB operator ~(GroupJoinZoneAB z)
	{
		return new GroupJoinZoneAB
		{
			a = ~z.a,
			b = ~z.b
		};
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x00046170 File Offset: 0x00044370
	public static bool operator ==(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return one.a == two.a && one.b == two.b;
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x00046190 File Offset: 0x00044390
	public static bool operator !=(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return one.a != two.a || one.b != two.b;
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x000461B3 File Offset: 0x000443B3
	public override bool Equals(object other)
	{
		return this == (GroupJoinZoneAB)other;
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x000461C6 File Offset: 0x000443C6
	public override int GetHashCode()
	{
		return this.a.GetHashCode() ^ this.b.GetHashCode();
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x000F3008 File Offset: 0x000F1208
	public static implicit operator GroupJoinZoneAB(int d)
	{
		return new GroupJoinZoneAB
		{
			a = (GroupJoinZoneA)d
		};
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x000F3028 File Offset: 0x000F1228
	public override string ToString()
	{
		if (this.b == (GroupJoinZoneB)0)
		{
			return this.a.ToString();
		}
		if (this.a != (GroupJoinZoneA)0)
		{
			return this.a.ToString() + "," + this.b.ToString();
		}
		return this.b.ToString();
	}

	// Token: 0x0400249E RID: 9374
	public GroupJoinZoneA a;

	// Token: 0x0400249F RID: 9375
	public GroupJoinZoneB b;
}
