using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Token: 0x020006BA RID: 1722
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct Id128 : IEquatable<Id128>, IComparable<Id128>, IEquatable<Guid>, IEquatable<Hash128>
{
	// Token: 0x06002A8E RID: 10894 RVA: 0x000D3F28 File Offset: 0x000D2128
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

	// Token: 0x06002A8F RID: 10895 RVA: 0x000D3F7C File Offset: 0x000D217C
	public Id128(long x, long y)
	{
		this.a = (this.b = (this.c = (this.d = 0)));
		this.guid = Guid.Empty;
		this.h128 = default(Hash128);
		this.x = x;
		this.y = y;
	}

	// Token: 0x06002A90 RID: 10896 RVA: 0x000D3FD0 File Offset: 0x000D21D0
	public Id128(Hash128 hash)
	{
		this.x = (this.y = 0L);
		this.a = (this.b = (this.c = (this.d = 0)));
		this.guid = Guid.Empty;
		this.h128 = hash;
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x000D4024 File Offset: 0x000D2224
	public Id128(Guid guid)
	{
		this.a = (this.b = (this.c = (this.d = 0)));
		this.x = (this.y = 0L);
		this.h128 = default(Hash128);
		this.guid = guid;
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x000D4078 File Offset: 0x000D2278
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

	// Token: 0x06002A93 RID: 10899 RVA: 0x000D40E4 File Offset: 0x000D22E4
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

	// Token: 0x06002A94 RID: 10900 RVA: 0x000D4161 File Offset: 0x000D2361
	[return: TupleElementNames(new string[]
	{
		"l1",
		"l2"
	})]
	public ValueTuple<long, long> ToLongs()
	{
		return new ValueTuple<long, long>(this.x, this.y);
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x000D4174 File Offset: 0x000D2374
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

	// Token: 0x06002A96 RID: 10902 RVA: 0x000D4193 File Offset: 0x000D2393
	public byte[] ToByteArray()
	{
		return this.guid.ToByteArray();
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x000D41A0 File Offset: 0x000D23A0
	public bool Equals(Id128 id)
	{
		return this.x == id.x && this.y == id.y;
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x000D41C0 File Offset: 0x000D23C0
	public bool Equals(Guid g)
	{
		return this.guid == g;
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x000D41CE File Offset: 0x000D23CE
	public bool Equals(Hash128 h)
	{
		return this.h128 == h;
	}

	// Token: 0x06002A9A RID: 10906 RVA: 0x000D41DC File Offset: 0x000D23DC
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

	// Token: 0x06002A9B RID: 10907 RVA: 0x000D422F File Offset: 0x000D242F
	public override string ToString()
	{
		return this.guid.ToString();
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x000D4242 File Offset: 0x000D2442
	public override int GetHashCode()
	{
		return StaticHash.Compute(this.a, this.b, this.c, this.d);
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x000D4264 File Offset: 0x000D2464
	public int CompareTo(Id128 id)
	{
		int num = this.x.CompareTo(id.x);
		if (num == 0)
		{
			num = this.y.CompareTo(id.y);
		}
		return num;
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x000D429C File Offset: 0x000D249C
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

	// Token: 0x06002A9F RID: 10911 RVA: 0x000D4302 File Offset: 0x000D2502
	public static Id128 NewId()
	{
		return new Id128(Guid.NewGuid());
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x000D4310 File Offset: 0x000D2510
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

	// Token: 0x06002AA1 RID: 10913 RVA: 0x000D436C File Offset: 0x000D256C
	public static Id128 ComputeSHV2(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return Id128.Empty;
		}
		return Hash128.Compute(s);
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x000D4387 File Offset: 0x000D2587
	public static bool operator ==(Id128 j, Id128 k)
	{
		return j.Equals(k);
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x000D4391 File Offset: 0x000D2591
	public static bool operator !=(Id128 j, Id128 k)
	{
		return !j.Equals(k);
	}

	// Token: 0x06002AA4 RID: 10916 RVA: 0x000D439E File Offset: 0x000D259E
	public static bool operator ==(Id128 j, Guid k)
	{
		return j.Equals(k);
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x000D43A8 File Offset: 0x000D25A8
	public static bool operator !=(Id128 j, Guid k)
	{
		return !j.Equals(k);
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x000D43B5 File Offset: 0x000D25B5
	public static bool operator ==(Guid j, Id128 k)
	{
		return j.Equals(k.guid);
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x000D43C4 File Offset: 0x000D25C4
	public static bool operator !=(Guid j, Id128 k)
	{
		return !j.Equals(k.guid);
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x000D43D6 File Offset: 0x000D25D6
	public static bool operator ==(Id128 j, Hash128 k)
	{
		return j.Equals(k);
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x000D43E0 File Offset: 0x000D25E0
	public static bool operator !=(Id128 j, Hash128 k)
	{
		return !j.Equals(k);
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x000D43ED File Offset: 0x000D25ED
	public static bool operator ==(Hash128 j, Id128 k)
	{
		return j.Equals(k.h128);
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x000D43FC File Offset: 0x000D25FC
	public static bool operator !=(Hash128 j, Id128 k)
	{
		return !j.Equals(k.h128);
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x000D440E File Offset: 0x000D260E
	public static bool operator <(Id128 j, Id128 k)
	{
		return j.CompareTo(k) < 0;
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x000D441B File Offset: 0x000D261B
	public static bool operator >(Id128 j, Id128 k)
	{
		return j.CompareTo(k) > 0;
	}

	// Token: 0x06002AAE RID: 10926 RVA: 0x000D4428 File Offset: 0x000D2628
	public static bool operator <=(Id128 j, Id128 k)
	{
		return j.CompareTo(k) <= 0;
	}

	// Token: 0x06002AAF RID: 10927 RVA: 0x000D4438 File Offset: 0x000D2638
	public static bool operator >=(Id128 j, Id128 k)
	{
		return j.CompareTo(k) >= 0;
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x000D4448 File Offset: 0x000D2648
	public static implicit operator Guid(Id128 id)
	{
		return id.guid;
	}

	// Token: 0x06002AB1 RID: 10929 RVA: 0x000D4450 File Offset: 0x000D2650
	public static implicit operator Id128(Guid guid)
	{
		return new Id128(guid);
	}

	// Token: 0x06002AB2 RID: 10930 RVA: 0x000D4458 File Offset: 0x000D2658
	public static implicit operator Id128(Hash128 h)
	{
		return new Id128(h);
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x000D4460 File Offset: 0x000D2660
	public static implicit operator Hash128(Id128 id)
	{
		return id.h128;
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x000D4468 File Offset: 0x000D2668
	public static explicit operator Id128(string s)
	{
		return Id128.ComputeMD5(s);
	}

	// Token: 0x04003018 RID: 12312
	[SerializeField]
	[FieldOffset(0)]
	public long x;

	// Token: 0x04003019 RID: 12313
	[SerializeField]
	[FieldOffset(8)]
	public long y;

	// Token: 0x0400301A RID: 12314
	[NonSerialized]
	[FieldOffset(0)]
	public int a;

	// Token: 0x0400301B RID: 12315
	[NonSerialized]
	[FieldOffset(4)]
	public int b;

	// Token: 0x0400301C RID: 12316
	[NonSerialized]
	[FieldOffset(8)]
	public int c;

	// Token: 0x0400301D RID: 12317
	[NonSerialized]
	[FieldOffset(12)]
	public int d;

	// Token: 0x0400301E RID: 12318
	[NonSerialized]
	[FieldOffset(0)]
	public Guid guid;

	// Token: 0x0400301F RID: 12319
	[NonSerialized]
	[FieldOffset(0)]
	public Hash128 h128;

	// Token: 0x04003020 RID: 12320
	public static readonly Id128 Empty;
}
