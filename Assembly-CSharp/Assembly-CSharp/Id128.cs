using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Token: 0x020006BB RID: 1723
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct Id128 : IEquatable<Id128>, IComparable<Id128>, IEquatable<Guid>, IEquatable<Hash128>
{
	// Token: 0x06002A96 RID: 10902 RVA: 0x000D43A8 File Offset: 0x000D25A8
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

	// Token: 0x06002A97 RID: 10903 RVA: 0x000D43FC File Offset: 0x000D25FC
	public Id128(long x, long y)
	{
		this.a = (this.b = (this.c = (this.d = 0)));
		this.guid = Guid.Empty;
		this.h128 = default(Hash128);
		this.x = x;
		this.y = y;
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x000D4450 File Offset: 0x000D2650
	public Id128(Hash128 hash)
	{
		this.x = (this.y = 0L);
		this.a = (this.b = (this.c = (this.d = 0)));
		this.guid = Guid.Empty;
		this.h128 = hash;
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x000D44A4 File Offset: 0x000D26A4
	public Id128(Guid guid)
	{
		this.a = (this.b = (this.c = (this.d = 0)));
		this.x = (this.y = 0L);
		this.h128 = default(Hash128);
		this.guid = guid;
	}

	// Token: 0x06002A9A RID: 10906 RVA: 0x000D44F8 File Offset: 0x000D26F8
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

	// Token: 0x06002A9B RID: 10907 RVA: 0x000D4564 File Offset: 0x000D2764
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

	// Token: 0x06002A9C RID: 10908 RVA: 0x000D45E1 File Offset: 0x000D27E1
	[return: TupleElementNames(new string[]
	{
		"l1",
		"l2"
	})]
	public ValueTuple<long, long> ToLongs()
	{
		return new ValueTuple<long, long>(this.x, this.y);
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x000D45F4 File Offset: 0x000D27F4
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

	// Token: 0x06002A9E RID: 10910 RVA: 0x000D4613 File Offset: 0x000D2813
	public byte[] ToByteArray()
	{
		return this.guid.ToByteArray();
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x000D4620 File Offset: 0x000D2820
	public bool Equals(Id128 id)
	{
		return this.x == id.x && this.y == id.y;
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x000D4640 File Offset: 0x000D2840
	public bool Equals(Guid g)
	{
		return this.guid == g;
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x000D464E File Offset: 0x000D284E
	public bool Equals(Hash128 h)
	{
		return this.h128 == h;
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x000D465C File Offset: 0x000D285C
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

	// Token: 0x06002AA3 RID: 10915 RVA: 0x000D46AF File Offset: 0x000D28AF
	public override string ToString()
	{
		return this.guid.ToString();
	}

	// Token: 0x06002AA4 RID: 10916 RVA: 0x000D46C2 File Offset: 0x000D28C2
	public override int GetHashCode()
	{
		return StaticHash.Compute(this.a, this.b, this.c, this.d);
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x000D46E4 File Offset: 0x000D28E4
	public int CompareTo(Id128 id)
	{
		int num = this.x.CompareTo(id.x);
		if (num == 0)
		{
			num = this.y.CompareTo(id.y);
		}
		return num;
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x000D471C File Offset: 0x000D291C
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

	// Token: 0x06002AA7 RID: 10919 RVA: 0x000D4782 File Offset: 0x000D2982
	public static Id128 NewId()
	{
		return new Id128(Guid.NewGuid());
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x000D4790 File Offset: 0x000D2990
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

	// Token: 0x06002AA9 RID: 10921 RVA: 0x000D47EC File Offset: 0x000D29EC
	public static Id128 ComputeSHV2(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return Id128.Empty;
		}
		return Hash128.Compute(s);
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x000D4807 File Offset: 0x000D2A07
	public static bool operator ==(Id128 j, Id128 k)
	{
		return j.Equals(k);
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x000D4811 File Offset: 0x000D2A11
	public static bool operator !=(Id128 j, Id128 k)
	{
		return !j.Equals(k);
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x000D481E File Offset: 0x000D2A1E
	public static bool operator ==(Id128 j, Guid k)
	{
		return j.Equals(k);
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x000D4828 File Offset: 0x000D2A28
	public static bool operator !=(Id128 j, Guid k)
	{
		return !j.Equals(k);
	}

	// Token: 0x06002AAE RID: 10926 RVA: 0x000D4835 File Offset: 0x000D2A35
	public static bool operator ==(Guid j, Id128 k)
	{
		return j.Equals(k.guid);
	}

	// Token: 0x06002AAF RID: 10927 RVA: 0x000D4844 File Offset: 0x000D2A44
	public static bool operator !=(Guid j, Id128 k)
	{
		return !j.Equals(k.guid);
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x000D4856 File Offset: 0x000D2A56
	public static bool operator ==(Id128 j, Hash128 k)
	{
		return j.Equals(k);
	}

	// Token: 0x06002AB1 RID: 10929 RVA: 0x000D4860 File Offset: 0x000D2A60
	public static bool operator !=(Id128 j, Hash128 k)
	{
		return !j.Equals(k);
	}

	// Token: 0x06002AB2 RID: 10930 RVA: 0x000D486D File Offset: 0x000D2A6D
	public static bool operator ==(Hash128 j, Id128 k)
	{
		return j.Equals(k.h128);
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x000D487C File Offset: 0x000D2A7C
	public static bool operator !=(Hash128 j, Id128 k)
	{
		return !j.Equals(k.h128);
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x000D488E File Offset: 0x000D2A8E
	public static bool operator <(Id128 j, Id128 k)
	{
		return j.CompareTo(k) < 0;
	}

	// Token: 0x06002AB5 RID: 10933 RVA: 0x000D489B File Offset: 0x000D2A9B
	public static bool operator >(Id128 j, Id128 k)
	{
		return j.CompareTo(k) > 0;
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x000D48A8 File Offset: 0x000D2AA8
	public static bool operator <=(Id128 j, Id128 k)
	{
		return j.CompareTo(k) <= 0;
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x000D48B8 File Offset: 0x000D2AB8
	public static bool operator >=(Id128 j, Id128 k)
	{
		return j.CompareTo(k) >= 0;
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x000D48C8 File Offset: 0x000D2AC8
	public static implicit operator Guid(Id128 id)
	{
		return id.guid;
	}

	// Token: 0x06002AB9 RID: 10937 RVA: 0x000D48D0 File Offset: 0x000D2AD0
	public static implicit operator Id128(Guid guid)
	{
		return new Id128(guid);
	}

	// Token: 0x06002ABA RID: 10938 RVA: 0x000D48D8 File Offset: 0x000D2AD8
	public static implicit operator Id128(Hash128 h)
	{
		return new Id128(h);
	}

	// Token: 0x06002ABB RID: 10939 RVA: 0x000D48E0 File Offset: 0x000D2AE0
	public static implicit operator Hash128(Id128 id)
	{
		return id.h128;
	}

	// Token: 0x06002ABC RID: 10940 RVA: 0x000D48E8 File Offset: 0x000D2AE8
	public static explicit operator Id128(string s)
	{
		return Id128.ComputeMD5(s);
	}

	// Token: 0x0400301E RID: 12318
	[SerializeField]
	[FieldOffset(0)]
	public long x;

	// Token: 0x0400301F RID: 12319
	[SerializeField]
	[FieldOffset(8)]
	public long y;

	// Token: 0x04003020 RID: 12320
	[NonSerialized]
	[FieldOffset(0)]
	public int a;

	// Token: 0x04003021 RID: 12321
	[NonSerialized]
	[FieldOffset(4)]
	public int b;

	// Token: 0x04003022 RID: 12322
	[NonSerialized]
	[FieldOffset(8)]
	public int c;

	// Token: 0x04003023 RID: 12323
	[NonSerialized]
	[FieldOffset(12)]
	public int d;

	// Token: 0x04003024 RID: 12324
	[NonSerialized]
	[FieldOffset(0)]
	public Guid guid;

	// Token: 0x04003025 RID: 12325
	[NonSerialized]
	[FieldOffset(0)]
	public Hash128 h128;

	// Token: 0x04003026 RID: 12326
	public static readonly Id128 Empty;
}
