using System;

// Token: 0x02000530 RID: 1328
[Serializable]
public struct GroupJoinZoneAB
{
	// Token: 0x06002019 RID: 8217 RVA: 0x000A1B3C File Offset: 0x0009FD3C
	public static GroupJoinZoneAB operator &(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return new GroupJoinZoneAB
		{
			a = (one.a & two.a),
			b = (one.b & two.b)
		};
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x000A1B7C File Offset: 0x0009FD7C
	public static GroupJoinZoneAB operator |(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return new GroupJoinZoneAB
		{
			a = (one.a | two.a),
			b = (one.b | two.b)
		};
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x000A1BBC File Offset: 0x0009FDBC
	public static GroupJoinZoneAB operator ~(GroupJoinZoneAB z)
	{
		return new GroupJoinZoneAB
		{
			a = ~z.a,
			b = ~z.b
		};
	}

	// Token: 0x0600201C RID: 8220 RVA: 0x000A1BEE File Offset: 0x0009FDEE
	public static bool operator ==(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return one.a == two.a && one.b == two.b;
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000A1C0E File Offset: 0x0009FE0E
	public static bool operator !=(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return one.a != two.a || one.b != two.b;
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x000A1C31 File Offset: 0x0009FE31
	public override bool Equals(object other)
	{
		return this == (GroupJoinZoneAB)other;
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x000A1C44 File Offset: 0x0009FE44
	public override int GetHashCode()
	{
		return this.a.GetHashCode() ^ this.b.GetHashCode();
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x000A1C6C File Offset: 0x0009FE6C
	public static implicit operator GroupJoinZoneAB(int d)
	{
		return new GroupJoinZoneAB
		{
			a = (GroupJoinZoneA)d
		};
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x000A1C8C File Offset: 0x0009FE8C
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

	// Token: 0x0400244B RID: 9291
	public GroupJoinZoneA a;

	// Token: 0x0400244C RID: 9292
	public GroupJoinZoneB b;
}
