using System;

// Token: 0x02000530 RID: 1328
[Serializable]
public struct GroupJoinZoneAB
{
	// Token: 0x0600201C RID: 8220 RVA: 0x000F01D0 File Offset: 0x000EE3D0
	public static GroupJoinZoneAB operator &(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return new GroupJoinZoneAB
		{
			a = (one.a & two.a),
			b = (one.b & two.b)
		};
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000F0210 File Offset: 0x000EE410
	public static GroupJoinZoneAB operator |(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return new GroupJoinZoneAB
		{
			a = (one.a | two.a),
			b = (one.b | two.b)
		};
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x000F0250 File Offset: 0x000EE450
	public static GroupJoinZoneAB operator ~(GroupJoinZoneAB z)
	{
		return new GroupJoinZoneAB
		{
			a = ~z.a,
			b = ~z.b
		};
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x00044DD1 File Offset: 0x00042FD1
	public static bool operator ==(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return one.a == two.a && one.b == two.b;
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x00044DF1 File Offset: 0x00042FF1
	public static bool operator !=(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return one.a != two.a || one.b != two.b;
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x00044E14 File Offset: 0x00043014
	public override bool Equals(object other)
	{
		return this == (GroupJoinZoneAB)other;
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x00044E27 File Offset: 0x00043027
	public override int GetHashCode()
	{
		return this.a.GetHashCode() ^ this.b.GetHashCode();
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x000F0284 File Offset: 0x000EE484
	public static implicit operator GroupJoinZoneAB(int d)
	{
		return new GroupJoinZoneAB
		{
			a = (GroupJoinZoneA)d
		};
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x000F02A4 File Offset: 0x000EE4A4
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

	// Token: 0x0400244C RID: 9292
	public GroupJoinZoneA a;

	// Token: 0x0400244D RID: 9293
	public GroupJoinZoneB b;
}
