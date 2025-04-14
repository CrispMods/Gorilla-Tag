using System;

// Token: 0x02000530 RID: 1328
[Serializable]
public struct GroupJoinZoneAB
{
	// Token: 0x0600201C RID: 8220 RVA: 0x000A1EC0 File Offset: 0x000A00C0
	public static GroupJoinZoneAB operator &(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return new GroupJoinZoneAB
		{
			a = (one.a & two.a),
			b = (one.b & two.b)
		};
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000A1F00 File Offset: 0x000A0100
	public static GroupJoinZoneAB operator |(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return new GroupJoinZoneAB
		{
			a = (one.a | two.a),
			b = (one.b | two.b)
		};
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x000A1F40 File Offset: 0x000A0140
	public static GroupJoinZoneAB operator ~(GroupJoinZoneAB z)
	{
		return new GroupJoinZoneAB
		{
			a = ~z.a,
			b = ~z.b
		};
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x000A1F72 File Offset: 0x000A0172
	public static bool operator ==(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return one.a == two.a && one.b == two.b;
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x000A1F92 File Offset: 0x000A0192
	public static bool operator !=(GroupJoinZoneAB one, GroupJoinZoneAB two)
	{
		return one.a != two.a || one.b != two.b;
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x000A1FB5 File Offset: 0x000A01B5
	public override bool Equals(object other)
	{
		return this == (GroupJoinZoneAB)other;
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000A1FC8 File Offset: 0x000A01C8
	public override int GetHashCode()
	{
		return this.a.GetHashCode() ^ this.b.GetHashCode();
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x000A1FF0 File Offset: 0x000A01F0
	public static implicit operator GroupJoinZoneAB(int d)
	{
		return new GroupJoinZoneAB
		{
			a = (GroupJoinZoneA)d
		};
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x000A2010 File Offset: 0x000A0210
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
