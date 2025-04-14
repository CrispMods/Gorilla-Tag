using System;

// Token: 0x020006D7 RID: 1751
public struct TimeSince
{
	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06002B8D RID: 11149 RVA: 0x000D6B04 File Offset: 0x000D4D04
	public double secondsElapsed
	{
		get
		{
			double totalSeconds = (DateTime.UtcNow - this._dt).TotalSeconds;
			if (totalSeconds <= 2147483647.0)
			{
				return totalSeconds;
			}
			return 2147483647.0;
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06002B8E RID: 11150 RVA: 0x000D6B41 File Offset: 0x000D4D41
	public float secondsElapsedFloat
	{
		get
		{
			return (float)this.secondsElapsed;
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06002B8F RID: 11151 RVA: 0x000D6B4A File Offset: 0x000D4D4A
	public int secondsElapsedInt
	{
		get
		{
			return (int)this.secondsElapsed;
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06002B90 RID: 11152 RVA: 0x000D6B53 File Offset: 0x000D4D53
	public uint secondsElapsedUint
	{
		get
		{
			return (uint)this.secondsElapsed;
		}
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06002B91 RID: 11153 RVA: 0x000D6B5C File Offset: 0x000D4D5C
	public long secondsElapsedLong
	{
		get
		{
			return (long)this.secondsElapsed;
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06002B92 RID: 11154 RVA: 0x000D6B65 File Offset: 0x000D4D65
	public TimeSpan secondsElapsedSpan
	{
		get
		{
			return TimeSpan.FromSeconds(this.secondsElapsed);
		}
	}

	// Token: 0x06002B93 RID: 11155 RVA: 0x000D6B72 File Offset: 0x000D4D72
	public TimeSince(DateTime dt)
	{
		this._dt = dt;
	}

	// Token: 0x06002B94 RID: 11156 RVA: 0x000D6B7C File Offset: 0x000D4D7C
	public TimeSince(int elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x000D6BA0 File Offset: 0x000D4DA0
	public TimeSince(uint elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds(-1.0 * elapsed);
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x000D6BD0 File Offset: 0x000D4DD0
	public TimeSince(float elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x000D6BF4 File Offset: 0x000D4DF4
	public TimeSince(double elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds(-elapsed);
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x000D6C18 File Offset: 0x000D4E18
	public TimeSince(long elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x000D6C3C File Offset: 0x000D4E3C
	public TimeSince(TimeSpan elapsed)
	{
		this._dt = DateTime.UtcNow.Add(-elapsed);
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x000D6C62 File Offset: 0x000D4E62
	public bool HasElapsed(int seconds)
	{
		return this.secondsElapsedInt >= seconds;
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x000D6C70 File Offset: 0x000D4E70
	public bool HasElapsed(uint seconds)
	{
		return this.secondsElapsedUint >= seconds;
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x000D6C7E File Offset: 0x000D4E7E
	public bool HasElapsed(float seconds)
	{
		return this.secondsElapsedFloat >= seconds;
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x000D6C8C File Offset: 0x000D4E8C
	public bool HasElapsed(double seconds)
	{
		return this.secondsElapsed >= seconds;
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x000D6C9A File Offset: 0x000D4E9A
	public bool HasElapsed(long seconds)
	{
		return this.secondsElapsedLong >= seconds;
	}

	// Token: 0x06002B9F RID: 11167 RVA: 0x000D6CA8 File Offset: 0x000D4EA8
	public bool HasElapsed(TimeSpan seconds)
	{
		return this.secondsElapsedSpan >= seconds;
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x000D6CB6 File Offset: 0x000D4EB6
	public void Reset()
	{
		this._dt = DateTime.UtcNow;
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x000D6CC3 File Offset: 0x000D4EC3
	public bool HasElapsed(int seconds, bool resetOnElapsed)
	{
		if (!resetOnElapsed)
		{
			return this.secondsElapsedInt >= seconds;
		}
		if (this.secondsElapsedInt < seconds)
		{
			return false;
		}
		this.Reset();
		return true;
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x000D6CE7 File Offset: 0x000D4EE7
	public bool HasElapsed(uint seconds, bool resetOnElapsed)
	{
		if (!resetOnElapsed)
		{
			return this.secondsElapsedUint >= seconds;
		}
		if (this.secondsElapsedUint < seconds)
		{
			return false;
		}
		this.Reset();
		return true;
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x000D6D0B File Offset: 0x000D4F0B
	public bool HasElapsed(float seconds, bool resetOnElapsed)
	{
		if (!resetOnElapsed)
		{
			return this.secondsElapsedFloat >= seconds;
		}
		if (this.secondsElapsedFloat < seconds)
		{
			return false;
		}
		this.Reset();
		return true;
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x000D6D2F File Offset: 0x000D4F2F
	public bool HasElapsed(double seconds, bool resetOnElapsed)
	{
		if (!resetOnElapsed)
		{
			return this.secondsElapsed >= seconds;
		}
		if (this.secondsElapsed < seconds)
		{
			return false;
		}
		this.Reset();
		return true;
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x000D6D53 File Offset: 0x000D4F53
	public bool HasElapsed(long seconds, bool resetOnElapsed)
	{
		if (!resetOnElapsed)
		{
			return this.secondsElapsedLong >= seconds;
		}
		if (this.secondsElapsedLong < seconds)
		{
			return false;
		}
		this.Reset();
		return true;
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x000D6D77 File Offset: 0x000D4F77
	public bool HasElapsed(TimeSpan seconds, bool resetOnElapsed)
	{
		if (!resetOnElapsed)
		{
			return this.secondsElapsedSpan >= seconds;
		}
		if (this.secondsElapsedSpan < seconds)
		{
			return false;
		}
		this.Reset();
		return true;
	}

	// Token: 0x06002BA7 RID: 11175 RVA: 0x000D6DA0 File Offset: 0x000D4FA0
	public override string ToString()
	{
		return string.Format("{0:F3} seconds since {{{1:s}", this.secondsElapsed, this._dt);
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x000D6DC2 File Offset: 0x000D4FC2
	public override int GetHashCode()
	{
		return StaticHash.Compute(this._dt);
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x000D6DCF File Offset: 0x000D4FCF
	public static TimeSince Now()
	{
		return new TimeSince(DateTime.UtcNow);
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x000D6DDB File Offset: 0x000D4FDB
	public static implicit operator long(TimeSince ts)
	{
		return ts.secondsElapsedLong;
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x000D6DE4 File Offset: 0x000D4FE4
	public static implicit operator double(TimeSince ts)
	{
		return ts.secondsElapsed;
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x000D6DED File Offset: 0x000D4FED
	public static implicit operator float(TimeSince ts)
	{
		return ts.secondsElapsedFloat;
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x000D6DF6 File Offset: 0x000D4FF6
	public static implicit operator int(TimeSince ts)
	{
		return ts.secondsElapsedInt;
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x000D6DFF File Offset: 0x000D4FFF
	public static implicit operator uint(TimeSince ts)
	{
		return ts.secondsElapsedUint;
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x000D6E08 File Offset: 0x000D5008
	public static implicit operator TimeSpan(TimeSince ts)
	{
		return ts.secondsElapsedSpan;
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x000D6E11 File Offset: 0x000D5011
	public static implicit operator TimeSince(int elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB1 RID: 11185 RVA: 0x000D6E19 File Offset: 0x000D5019
	public static implicit operator TimeSince(uint elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x000D6E21 File Offset: 0x000D5021
	public static implicit operator TimeSince(float elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB3 RID: 11187 RVA: 0x000D6E29 File Offset: 0x000D5029
	public static implicit operator TimeSince(double elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x000D6E31 File Offset: 0x000D5031
	public static implicit operator TimeSince(long elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x000D6E39 File Offset: 0x000D5039
	public static implicit operator TimeSince(TimeSpan elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x000D6E41 File Offset: 0x000D5041
	public static implicit operator TimeSince(DateTime dt)
	{
		return new TimeSince(dt);
	}

	// Token: 0x040030C9 RID: 12489
	private DateTime _dt;

	// Token: 0x040030CA RID: 12490
	private const double INT32_MAX = 2147483647.0;
}
