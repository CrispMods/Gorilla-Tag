using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Token: 0x020006CF RID: 1743
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct Id128 : IEquatable<Id128>, IComparable<Id128>, IEquatable<Guid>, IEquatable<Hash128>
{
	// Token: 0x06002B24 RID: 11044 RVA: 0x001201B4 File Offset: 0x0011E3B4
	public Id128(int a, int b, int c, int d)
	{
		this.guid = Guid.Empty;
		this.h128 = default(Hash128);
		this.x = (this.y = 0L);
		this.a = a;
		this.b = b;
		this.c = c;
		this.d = d;
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x00120208 File Offset: 0x0011E408
	public Id128(long x, long y)
	{
		this.a = (this.b = (this.c = (this.d = 0)));
		this.guid = Guid.Empty;
		this.h128 = default(Hash128);
		this.x = x;
		this.y = y;
	}

	// Token: 0x06002B26 RID: 11046 RVA: 0x0012025C File Offset: 0x0011E45C
	public Id128(Hash128 hash)
	{
		this.x = (this.y = 0L);
		this.a = (this.b = (this.c = (this.d = 0)));
		this.guid = Guid.Empty;
		this.h128 = hash;
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x001202B0 File Offset: 0x0011E4B0
	public Id128(Guid guid)
	{
		this.a = (this.b = (this.c = (this.d = 0)));
		this.x = (this.y = 0L);
		this.h128 = default(Hash128);
		this.guid = guid;
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x00120304 File Offset: 0x0011E504
	public Id128(string guid)
	{
		if (string.IsNullOrWhiteSpace(guid))
		{
			throw new ArgumentNullException("guid");
		}
		this.a = (this.b = (this.c = (this.d = 0)));
		this.x = (this.y = 0L);
		this.h128 = default(Hash128);
		this.guid = Guid.Parse(guid);
	}

	// Token: 0x06002B29 RID: 11049 RVA: 0x00120370 File Offset: 0x0011E570
	public Id128(byte[] bytes)
	{
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		if (bytes.Length != 16)
		{
			throw new ArgumentException("Input buffer must be exactly 16 bytes", "bytes");
		}
		this.a = (this.b = (this.c = (this.d = 0)));
		this.x = (this.y = 0L);
		this.h128 = default(Hash128);
		this.guid = new Guid(bytes);
	}

	// Token: 0x06002B2A RID: 11050 RVA: 0x0004D193 File Offset: 0x0004B393
	[return: TupleElementNames(new string[]
	{
		"l1",
		"l2"
	})]
	public ValueTuple<long, long> ToLongs()
	{
		return new ValueTuple<long, long>(this.x, this.y);
	}

	// Token: 0x06002B2B RID: 11051 RVA: 0x0004D1A6 File Offset: 0x0004B3A6
	[return: TupleElementNames(new string[]
	{
		"i1",
		"i2",
		"i3",
		"i4"
	})]
	public ValueTuple<int, int, int, int> ToInts()
	{
		return new ValueTuple<int, int, int, int>(this.a, this.b, this.c, this.d);
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x0004D1C5 File Offset: 0x0004B3C5
	public byte[] ToByteArray()
	{
		return this.guid.ToByteArray();
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x0004D1D2 File Offset: 0x0004B3D2
	public bool Equals(Id128 id)
	{
		return this.x == id.x && this.y == id.y;
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x0004D1F2 File Offset: 0x0004B3F2
	public bool Equals(Guid g)
	{
		return this.guid == g;
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x0004D200 File Offset: 0x0004B400
	public bool Equals(Hash128 h)
	{
		return this.h128 == h;
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x001203F0 File Offset: 0x0011E5F0
	public override bool Equals(object obj)
	{
		if (obj is Id128)
		{
			Id128 id = (Id128)obj;
			return this.Equals(id);
		}
		if (obj is Guid)
		{
			Guid g = (Guid)obj;
			return this.Equals(g);
		}
		if (obj is Hash128)
		{
			Hash128 h = (Hash128)obj;
			return this.Equals(h);
		}
		return false;
	}

	// Token: 0x06002B31 RID: 11057 RVA: 0x0004D20E File Offset: 0x0004B40E
	public override string ToString()
	{
		return this.guid.ToString();
	}

	// Token: 0x06002B32 RID: 11058 RVA: 0x0004D221 File Offset: 0x0004B421
	public override int GetHashCode()
	{
		return StaticHash.Compute(this.a, this.b, this.c, this.d);
	}

	// Token: 0x06002B33 RID: 11059 RVA: 0x00120444 File Offset: 0x0011E644
	public int CompareTo(Id128 id)
	{
		int num = this.x.CompareTo(id.x);
		if (num == 0)
		{
			num = this.y.CompareTo(id.y);
		}
		return num;
	}

	// Token: 0x06002B34 RID: 11060 RVA: 0x0012047C File Offset: 0x0011E67C
	public int CompareTo(object obj)
	{
		if (obj is Id128)
		{
			Id128 id = (Id128)obj;
			return this.CompareTo(id);
		}
		if (obj is Guid)
		{
			Guid value = (Guid)obj;
			return this.guid.CompareTo(value);
		}
		if (obj is Hash128)
		{
			Hash128 rhs = (Hash128)obj;
			return this.h128.CompareTo(rhs);
		}
		throw new ArgumentException("Object must be of type Id128 or Guid");
	}

	// Token: 0x06002B35 RID: 11061 RVA: 0x0004D240 File Offset: 0x0004B440
	public static Id128 NewId()
	{
		return new Id128(Guid.NewGuid());
	}

	// Token: 0x06002B36 RID: 11062 RVA: 0x001204E4 File Offset: 0x0011E6E4
	public static Id128 ComputeMD5(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return Id128.Empty;
		}
		Id128 result;
		using (MD5 md = MD5.Create())
		{
			result = new Guid(md.ComputeHash(Encoding.UTF8.GetBytes(s)));
		}
		return result;
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x0004D24C File Offset: 0x0004B44C
	public static Id128 ComputeSHV2(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return Id128.Empty;
		}
		return Hash128.Compute(s);
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x0004D267 File Offset: 0x0004B467
	public static bool operator ==(Id128 j, Id128 k)
	{
		return j.Equals(k);
	}

	// Token: 0x06002B39 RID: 11065 RVA: 0x0004D271 File Offset: 0x0004B471
	public static bool operator !=(Id128 j, Id128 k)
	{
		return !j.Equals(k);
	}

	// Token: 0x06002B3A RID: 11066 RVA: 0x0004D27E File Offset: 0x0004B47E
	public static bool operator ==(Id128 j, Guid k)
	{
		return j.Equals(k);
	}

	// Token: 0x06002B3B RID: 11067 RVA: 0x0004D288 File Offset: 0x0004B488
	public static bool operator !=(Id128 j, Guid k)
	{
		return !j.Equals(k);
	}

	// Token: 0x06002B3C RID: 11068 RVA: 0x0004D295 File Offset: 0x0004B495
	public static bool operator ==(Guid j, Id128 k)
	{
		return j.Equals(k.guid);
	}

	// Token: 0x06002B3D RID: 11069 RVA: 0x0004D2A4 File Offset: 0x0004B4A4
	public static bool operator !=(Guid j, Id128 k)
	{
		return !j.Equals(k.guid);
	}

	// Token: 0x06002B3E RID: 11070 RVA: 0x0004D2B6 File Offset: 0x0004B4B6
	public static bool operator ==(Id128 j, Hash128 k)
	{
		return j.Equals(k);
	}

	// Token: 0x06002B3F RID: 11071 RVA: 0x0004D2C0 File Offset: 0x0004B4C0
	public static bool operator !=(Id128 j, Hash128 k)
	{
		return !j.Equals(k);
	}

	// Token: 0x06002B40 RID: 11072 RVA: 0x0004D2CD File Offset: 0x0004B4CD
	public static bool operator ==(Hash128 j, Id128 k)
	{
		return j.Equals(k.h128);
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x0004D2DC File Offset: 0x0004B4DC
	public static bool operator !=(Hash128 j, Id128 k)
	{
		return !j.Equals(k.h128);
	}

	// Token: 0x06002B42 RID: 11074 RVA: 0x0004D2EE File Offset: 0x0004B4EE
	public static bool operator <(Id128 j, Id128 k)
	{
		return j.CompareTo(k) < 0;
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x0004D2FB File Offset: 0x0004B4FB
	public static bool operator >(Id128 j, Id128 k)
	{
		return j.CompareTo(k) > 0;
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x0004D308 File Offset: 0x0004B508
	public static bool operator <=(Id128 j, Id128 k)
	{
		return j.CompareTo(k) <= 0;
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x0004D318 File Offset: 0x0004B518
	public static bool operator >=(Id128 j, Id128 k)
	{
		return j.CompareTo(k) >= 0;
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x0004D328 File Offset: 0x0004B528
	public static implicit operator Guid(Id128 id)
	{
		return id.guid;
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x0004D330 File Offset: 0x0004B530
	public static implicit operator Id128(Guid guid)
	{
		return new Id128(guid);
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x0004D338 File Offset: 0x0004B538
	public static implicit operator Id128(Hash128 h)
	{
		return new Id128(h);
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x0004D340 File Offset: 0x0004B540
	public static implicit operator Hash128(Id128 id)
	{
		return id.h128;
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x0004D348 File Offset: 0x0004B548
	public static explicit operator Id128(string s)
	{
		return Id128.ComputeMD5(s);
	}

	// Token: 0x040030B5 RID: 12469
	[SerializeField]
	[FieldOffset(0)]
	public long x;

	// Token: 0x040030B6 RID: 12470
	[SerializeField]
	[FieldOffset(8)]
	public long y;

	// Token: 0x040030B7 RID: 12471
	[NonSerialized]
	[FieldOffset(0)]
	public int a;

	// Token: 0x040030B8 RID: 12472
	[NonSerialized]
	[FieldOffset(4)]
	public int b;

	// Token: 0x040030B9 RID: 12473
	[NonSerialized]
	[FieldOffset(8)]
	public int c;

	// Token: 0x040030BA RID: 12474
	[NonSerialized]
	[FieldOffset(12)]
	public int d;

	// Token: 0x040030BB RID: 12475
	[NonSerialized]
	[FieldOffset(0)]
	public Guid guid;

	// Token: 0x040030BC RID: 12476
	[NonSerialized]
	[FieldOffset(0)]
	public Hash128 h128;

	// Token: 0x040030BD RID: 12477
	public static readonly Id128 Empty;
}
