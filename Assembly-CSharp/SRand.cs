﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020006D1 RID: 1745
[Serializable]
public struct SRand
{
	// Token: 0x06002B35 RID: 11061 RVA: 0x000D5BC3 File Offset: 0x000D3DC3
	public SRand(int seed)
	{
		this._seed = (uint)seed;
		this._state = this._seed;
	}

	// Token: 0x06002B36 RID: 11062 RVA: 0x000D5BC3 File Offset: 0x000D3DC3
	public SRand(uint seed)
	{
		this._seed = seed;
		this._state = this._seed;
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x000D5BD8 File Offset: 0x000D3DD8
	public SRand(long seed)
	{
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x000D5BF2 File Offset: 0x000D3DF2
	public SRand(DateTime seed)
	{
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002B39 RID: 11065 RVA: 0x000D5C0C File Offset: 0x000D3E0C
	public SRand(string seed)
	{
		if (string.IsNullOrEmpty(seed))
		{
			throw new ArgumentException("Seed cannot be null or empty", "seed");
		}
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002B3A RID: 11066 RVA: 0x000D5C3E File Offset: 0x000D3E3E
	public SRand(byte[] seed)
	{
		if (seed == null || seed.Length == 0)
		{
			throw new ArgumentException("Seed cannot be null or empty", "seed");
		}
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002B3B RID: 11067 RVA: 0x000D5C6F File Offset: 0x000D3E6F
	public double NextDouble()
	{
		return this.NextState() % 268435457U / 268435456.0;
	}

	// Token: 0x06002B3C RID: 11068 RVA: 0x000D5C89 File Offset: 0x000D3E89
	public double NextDouble(double max)
	{
		if (max < 0.0)
		{
			return 0.0;
		}
		return this.NextDouble() * max;
	}

	// Token: 0x06002B3D RID: 11069 RVA: 0x000D5CAC File Offset: 0x000D3EAC
	public double NextDouble(double min, double max)
	{
		double num = max - min;
		if (num <= 0.0)
		{
			return min;
		}
		double num2 = this.NextDouble() * num;
		return min + num2;
	}

	// Token: 0x06002B3E RID: 11070 RVA: 0x000D5CD7 File Offset: 0x000D3ED7
	public float NextFloat()
	{
		return (float)this.NextDouble();
	}

	// Token: 0x06002B3F RID: 11071 RVA: 0x000D5CE0 File Offset: 0x000D3EE0
	public float NextFloat(float max)
	{
		return (float)this.NextDouble((double)max);
	}

	// Token: 0x06002B40 RID: 11072 RVA: 0x000D5CEB File Offset: 0x000D3EEB
	public float NextFloat(float min, float max)
	{
		return (float)this.NextDouble((double)min, (double)max);
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x000D5CF8 File Offset: 0x000D3EF8
	public bool NextBool()
	{
		return this.NextState() % 2U == 1U;
	}

	// Token: 0x06002B42 RID: 11074 RVA: 0x000D5D05 File Offset: 0x000D3F05
	public uint NextUInt()
	{
		return this.NextState();
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x000D5D05 File Offset: 0x000D3F05
	public int NextInt()
	{
		return (int)this.NextState();
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x000D5D0D File Offset: 0x000D3F0D
	public int NextInt(int max)
	{
		if (max <= 0)
		{
			return 0;
		}
		return (int)((ulong)this.NextState() % (ulong)((long)max));
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x000D5D20 File Offset: 0x000D3F20
	public int NextInt(int min, int max)
	{
		int num = max - min;
		if (num <= 0)
		{
			return min;
		}
		return min + this.NextInt(num);
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x000D5D40 File Offset: 0x000D3F40
	public int NextIntWithExclusion(int min, int max, int exclude)
	{
		int num = max - min - 1;
		if (num <= 0)
		{
			return min;
		}
		int num2 = min + 1 + this.NextInt(num);
		if (num2 > exclude)
		{
			return num2;
		}
		return num2 - 1;
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x000D5D70 File Offset: 0x000D3F70
	public int NextIntWithExclusion2(int min, int max, int exclude, int exclude2)
	{
		if (exclude == exclude2)
		{
			return this.NextIntWithExclusion(min, max, exclude);
		}
		int num = max - min - 2;
		if (num <= 0)
		{
			return min;
		}
		int num2 = min + 2 + this.NextInt(num);
		int num3;
		int num4;
		if (exclude >= exclude2)
		{
			num3 = exclude2 + 1;
			num4 = exclude;
		}
		else
		{
			num3 = exclude + 1;
			num4 = exclude2;
		}
		if (num2 <= num3)
		{
			return num2 - 2;
		}
		if (num2 <= num4)
		{
			return num2 - 1;
		}
		return num2;
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x000D5DD8 File Offset: 0x000D3FD8
	public Color32 NextColor32()
	{
		byte r = (byte)this.NextInt(256);
		byte g = (byte)this.NextInt(256);
		byte b = (byte)this.NextInt(256);
		return new Color32(r, g, b, byte.MaxValue);
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x000D5E18 File Offset: 0x000D4018
	public Color NextColor()
	{
		float r = this.NextFloat();
		float g = this.NextFloat();
		float b = this.NextFloat();
		return new Color(r, g, b, 1f);
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x000D5E48 File Offset: 0x000D4048
	public void Shuffle<T>(T[] array)
	{
		int i = array.Length;
		while (i > 1)
		{
			int num = this.NextInt(i--);
			int num2 = i;
			int num3 = num;
			T t = array[num];
			T t2 = array[i];
			array[num2] = t;
			array[num3] = t2;
		}
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x000D5E98 File Offset: 0x000D4098
	public void Shuffle<T>(List<T> list)
	{
		int i = list.Count;
		while (i > 1)
		{
			int num = this.NextInt(i--);
			int index = i;
			int index2 = num;
			T value = list[num];
			T value2 = list[i];
			list[index] = value;
			list[index2] = value2;
		}
	}

	// Token: 0x06002B4C RID: 11084 RVA: 0x000D5EF0 File Offset: 0x000D40F0
	public void Reset()
	{
		this._state = this._seed;
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x000D5BC3 File Offset: 0x000D3DC3
	public void Reset(int seed)
	{
		this._seed = (uint)seed;
		this._state = this._seed;
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x000D5BC3 File Offset: 0x000D3DC3
	public void Reset(uint seed)
	{
		this._seed = seed;
		this._state = this._seed;
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x000D5BD8 File Offset: 0x000D3DD8
	public void Reset(long seed)
	{
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x000D5BF2 File Offset: 0x000D3DF2
	public void Reset(DateTime seed)
	{
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x000D5C0C File Offset: 0x000D3E0C
	public void Reset(string seed)
	{
		if (string.IsNullOrEmpty(seed))
		{
			throw new ArgumentException("Seed cannot be null or empty", "seed");
		}
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x000D5C3E File Offset: 0x000D3E3E
	public void Reset(byte[] seed)
	{
		if (seed == null || seed.Length == 0)
		{
			throw new ArgumentException("Seed cannot be null or empty", "seed");
		}
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002B53 RID: 11091 RVA: 0x000D5F00 File Offset: 0x000D4100
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private uint NextState()
	{
		return this._state = this.Mix(this._state + 184402071U);
	}

	// Token: 0x06002B54 RID: 11092 RVA: 0x000D5F28 File Offset: 0x000D4128
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private uint Mix(uint x)
	{
		x = (x >> 16 ^ x) * 73244475U;
		x = (x >> 16 ^ x) * 73244475U;
		x = (x >> 16 ^ x);
		return x;
	}

	// Token: 0x06002B55 RID: 11093 RVA: 0x000D5F4F File Offset: 0x000D414F
	public override int GetHashCode()
	{
		return StaticHash.Compute((int)this._seed, (int)this._state);
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x000D5F64 File Offset: 0x000D4164
	public override string ToString()
	{
		return string.Format("{0} {{ {1}: {2:X8} {3}: {4:X8} }}", new object[]
		{
			"SRand",
			"_seed",
			this._seed,
			"_state",
			this._state
		});
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x000D5FB5 File Offset: 0x000D41B5
	public static SRand New()
	{
		return new SRand(DateTime.UtcNow);
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x000D5FC1 File Offset: 0x000D41C1
	public static explicit operator SRand(int seed)
	{
		return new SRand(seed);
	}

	// Token: 0x06002B59 RID: 11097 RVA: 0x000D5FC9 File Offset: 0x000D41C9
	public static explicit operator SRand(uint seed)
	{
		return new SRand(seed);
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x000D5FD1 File Offset: 0x000D41D1
	public static explicit operator SRand(long seed)
	{
		return new SRand(seed);
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x000D5FD9 File Offset: 0x000D41D9
	public static explicit operator SRand(string seed)
	{
		return new SRand(seed);
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x000D5FE1 File Offset: 0x000D41E1
	public static explicit operator SRand(byte[] seed)
	{
		return new SRand(seed);
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x000D5FE9 File Offset: 0x000D41E9
	public static explicit operator SRand(DateTime seed)
	{
		return new SRand(seed);
	}

	// Token: 0x040030AB RID: 12459
	private const uint MAX_PLUS_ONE = 268435457U;

	// Token: 0x040030AC RID: 12460
	private const double MAX_AS_DOUBLE = 268435456.0;

	// Token: 0x040030AD RID: 12461
	[SerializeField]
	private uint _seed;

	// Token: 0x040030AE RID: 12462
	[SerializeField]
	private uint _state;
}
