using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020006E6 RID: 1766
[Serializable]
public struct SRand
{
	// Token: 0x06002BCB RID: 11211 RVA: 0x0004DA94 File Offset: 0x0004BC94
	public SRand(int seed)
	{
		this._seed = (uint)seed;
		this._state = this._seed;
	}

	// Token: 0x06002BCC RID: 11212 RVA: 0x0004DA94 File Offset: 0x0004BC94
	public SRand(uint seed)
	{
		this._seed = seed;
		this._state = this._seed;
	}

	// Token: 0x06002BCD RID: 11213 RVA: 0x0004DAA9 File Offset: 0x0004BCA9
	public SRand(long seed)
	{
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002BCE RID: 11214 RVA: 0x0004DAC3 File Offset: 0x0004BCC3
	public SRand(DateTime seed)
	{
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002BCF RID: 11215 RVA: 0x0004DADD File Offset: 0x0004BCDD
	public SRand(string seed)
	{
		if (string.IsNullOrEmpty(seed))
		{
			throw new ArgumentException("Seed cannot be null or empty", "seed");
		}
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x0004DB0F File Offset: 0x0004BD0F
	public SRand(byte[] seed)
	{
		if (seed == null || seed.Length == 0)
		{
			throw new ArgumentException("Seed cannot be null or empty", "seed");
		}
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x0004DB40 File Offset: 0x0004BD40
	public double NextDouble()
	{
		return this.NextState() % 268435457U / 268435456.0;
	}

	// Token: 0x06002BD2 RID: 11218 RVA: 0x0004DB5A File Offset: 0x0004BD5A
	public double NextDouble(double max)
	{
		if (max < 0.0)
		{
			return 0.0;
		}
		return this.NextDouble() * max;
	}

	// Token: 0x06002BD3 RID: 11219 RVA: 0x0012155C File Offset: 0x0011F75C
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

	// Token: 0x06002BD4 RID: 11220 RVA: 0x0004DB7A File Offset: 0x0004BD7A
	public float NextFloat()
	{
		return (float)this.NextDouble();
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x0004DB83 File Offset: 0x0004BD83
	public float NextFloat(float max)
	{
		return (float)this.NextDouble((double)max);
	}

	// Token: 0x06002BD6 RID: 11222 RVA: 0x0004DB8E File Offset: 0x0004BD8E
	public float NextFloat(float min, float max)
	{
		return (float)this.NextDouble((double)min, (double)max);
	}

	// Token: 0x06002BD7 RID: 11223 RVA: 0x0004DB9B File Offset: 0x0004BD9B
	public bool NextBool()
	{
		return this.NextState() % 2U == 1U;
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x0004DBA8 File Offset: 0x0004BDA8
	public uint NextUInt()
	{
		return this.NextState();
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x0004DBA8 File Offset: 0x0004BDA8
	public int NextInt()
	{
		return (int)this.NextState();
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x0004DBB0 File Offset: 0x0004BDB0
	public int NextInt(int max)
	{
		if (max <= 0)
		{
			return 0;
		}
		return (int)((ulong)this.NextState() % (ulong)((long)max));
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x00121588 File Offset: 0x0011F788
	public int NextInt(int min, int max)
	{
		int num = max - min;
		if (num <= 0)
		{
			return min;
		}
		return min + this.NextInt(num);
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x001215A8 File Offset: 0x0011F7A8
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

	// Token: 0x06002BDD RID: 11229 RVA: 0x001215D8 File Offset: 0x0011F7D8
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

	// Token: 0x06002BDE RID: 11230 RVA: 0x00121640 File Offset: 0x0011F840
	public Color32 NextColor32()
	{
		byte r = (byte)this.NextInt(256);
		byte g = (byte)this.NextInt(256);
		byte b = (byte)this.NextInt(256);
		return new Color32(r, g, b, byte.MaxValue);
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x00121680 File Offset: 0x0011F880
	public Color NextColor()
	{
		float r = this.NextFloat();
		float g = this.NextFloat();
		float b = this.NextFloat();
		return new Color(r, g, b, 1f);
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x001216B0 File Offset: 0x0011F8B0
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

	// Token: 0x06002BE1 RID: 11233 RVA: 0x00121700 File Offset: 0x0011F900
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

	// Token: 0x06002BE2 RID: 11234 RVA: 0x0004DBC3 File Offset: 0x0004BDC3
	public void Reset()
	{
		this._state = this._seed;
	}

	// Token: 0x06002BE3 RID: 11235 RVA: 0x0004DA94 File Offset: 0x0004BC94
	public void Reset(int seed)
	{
		this._seed = (uint)seed;
		this._state = this._seed;
	}

	// Token: 0x06002BE4 RID: 11236 RVA: 0x0004DA94 File Offset: 0x0004BC94
	public void Reset(uint seed)
	{
		this._seed = seed;
		this._state = this._seed;
	}

	// Token: 0x06002BE5 RID: 11237 RVA: 0x0004DAA9 File Offset: 0x0004BCA9
	public void Reset(long seed)
	{
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002BE6 RID: 11238 RVA: 0x0004DAC3 File Offset: 0x0004BCC3
	public void Reset(DateTime seed)
	{
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002BE7 RID: 11239 RVA: 0x0004DADD File Offset: 0x0004BCDD
	public void Reset(string seed)
	{
		if (string.IsNullOrEmpty(seed))
		{
			throw new ArgumentException("Seed cannot be null or empty", "seed");
		}
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002BE8 RID: 11240 RVA: 0x0004DB0F File Offset: 0x0004BD0F
	public void Reset(byte[] seed)
	{
		if (seed == null || seed.Length == 0)
		{
			throw new ArgumentException("Seed cannot be null or empty", "seed");
		}
		this._seed = (uint)StaticHash.Compute(seed);
		this._state = this._seed;
	}

	// Token: 0x06002BE9 RID: 11241 RVA: 0x00121758 File Offset: 0x0011F958
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private uint NextState()
	{
		return this._state = this.Mix(this._state + 184402071U);
	}

	// Token: 0x06002BEA RID: 11242 RVA: 0x0004DBD1 File Offset: 0x0004BDD1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private uint Mix(uint x)
	{
		x = (x >> 16 ^ x) * 73244475U;
		x = (x >> 16 ^ x) * 73244475U;
		x = (x >> 16 ^ x);
		return x;
	}

	// Token: 0x06002BEB RID: 11243 RVA: 0x0004DBF8 File Offset: 0x0004BDF8
	public override int GetHashCode()
	{
		return StaticHash.Compute((int)this._seed, (int)this._state);
	}

	// Token: 0x06002BEC RID: 11244 RVA: 0x00121780 File Offset: 0x0011F980
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

	// Token: 0x06002BED RID: 11245 RVA: 0x0004DC0B File Offset: 0x0004BE0B
	public static SRand New()
	{
		return new SRand(DateTime.UtcNow);
	}

	// Token: 0x06002BEE RID: 11246 RVA: 0x0004DC17 File Offset: 0x0004BE17
	public static explicit operator SRand(int seed)
	{
		return new SRand(seed);
	}

	// Token: 0x06002BEF RID: 11247 RVA: 0x0004DC1F File Offset: 0x0004BE1F
	public static explicit operator SRand(uint seed)
	{
		return new SRand(seed);
	}

	// Token: 0x06002BF0 RID: 11248 RVA: 0x0004DC27 File Offset: 0x0004BE27
	public static explicit operator SRand(long seed)
	{
		return new SRand(seed);
	}

	// Token: 0x06002BF1 RID: 11249 RVA: 0x0004DC2F File Offset: 0x0004BE2F
	public static explicit operator SRand(string seed)
	{
		return new SRand(seed);
	}

	// Token: 0x06002BF2 RID: 11250 RVA: 0x0004DC37 File Offset: 0x0004BE37
	public static explicit operator SRand(byte[] seed)
	{
		return new SRand(seed);
	}

	// Token: 0x06002BF3 RID: 11251 RVA: 0x0004DC3F File Offset: 0x0004BE3F
	public static explicit operator SRand(DateTime seed)
	{
		return new SRand(seed);
	}

	// Token: 0x04003148 RID: 12616
	private const uint MAX_PLUS_ONE = 268435457U;

	// Token: 0x04003149 RID: 12617
	private const double MAX_AS_DOUBLE = 268435456.0;

	// Token: 0x0400314A RID: 12618
	[SerializeField]
	private uint _seed;

	// Token: 0x0400314B RID: 12619
	[SerializeField]
	private uint _state;
}
