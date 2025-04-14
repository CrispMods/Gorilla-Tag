using System;

// Token: 0x020006D6 RID: 1750
public struct TimeSince
{
	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06002B85 RID: 11141 RVA: 0x000D6684 File Offset: 0x000D4884
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

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06002B86 RID: 11142 RVA: 0x000D66C1 File Offset: 0x000D48C1
	public float secondsElapsedFloat
	{
		get
		{
			return (float)this.secondsElapsed;
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06002B87 RID: 11143 RVA: 0x000D66CA File Offset: 0x000D48CA
	public int secondsElapsedInt
	{
		get
		{
			return (int)this.secondsElapsed;
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06002B88 RID: 11144 RVA: 0x000D66D3 File Offset: 0x000D48D3
	public uint secondsElapsedUint
	{
		get
		{
			return (uint)this.secondsElapsed;
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06002B89 RID: 11145 RVA: 0x000D66DC File Offset: 0x000D48DC
	public long secondsElapsedLong
	{
		get
		{
			return (long)this.secondsElapsed;
		}
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06002B8A RID: 11146 RVA: 0x000D66E5 File Offset: 0x000D48E5
	public TimeSpan secondsElapsedSpan
	{
		get
		{
			return TimeSpan.FromSeconds(this.secondsElapsed);
		}
	}

	// Token: 0x06002B8B RID: 11147 RVA: 0x000D66F2 File Offset: 0x000D48F2
	public TimeSince(DateTime dt)
	{
		this._dt = dt;
	}

	// Token: 0x06002B8C RID: 11148 RVA: 0x000D66FC File Offset: 0x000D48FC
	public TimeSince(int elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002B8D RID: 11149 RVA: 0x000D6720 File Offset: 0x000D4920
	public TimeSince(uint elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds(-1.0 * elapsed);
	}

	// Token: 0x06002B8E RID: 11150 RVA: 0x000D6750 File Offset: 0x000D4950
	public TimeSince(float elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002B8F RID: 11151 RVA: 0x000D6774 File Offset: 0x000D4974
	public TimeSince(double elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds(-elapsed);
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x000D6798 File Offset: 0x000D4998
	public TimeSince(long elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x000D67BC File Offset: 0x000D49BC
	public TimeSince(TimeSpan elapsed)
	{
		this._dt = DateTime.UtcNow.Add(-elapsed);
	}

	// Token: 0x06002B92 RID: 11154 RVA: 0x000D67E2 File Offset: 0x000D49E2
	public bool HasElapsed(int seconds)
	{
		return this.secondsElapsedInt >= seconds;
	}

	// Token: 0x06002B93 RID: 11155 RVA: 0x000D67F0 File Offset: 0x000D49F0
	public bool HasElapsed(uint seconds)
	{
		return this.secondsElapsedUint >= seconds;
	}

	// Token: 0x06002B94 RID: 11156 RVA: 0x000D67FE File Offset: 0x000D49FE
	public bool HasElapsed(float seconds)
	{
		return this.secondsElapsedFloat >= seconds;
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x000D680C File Offset: 0x000D4A0C
	public bool HasElapsed(double seconds)
	{
		return this.secondsElapsed >= seconds;
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x000D681A File Offset: 0x000D4A1A
	public bool HasElapsed(long seconds)
	{
		return this.secondsElapsedLong >= seconds;
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x000D6828 File Offset: 0x000D4A28
	public bool HasElapsed(TimeSpan seconds)
	{
		return this.secondsElapsedSpan >= seconds;
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x000D6836 File Offset: 0x000D4A36
	public void Reset()
	{
		this._dt = DateTime.UtcNow;
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x000D6843 File Offset: 0x000D4A43
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

	// Token: 0x06002B9A RID: 11162 RVA: 0x000D6867 File Offset: 0x000D4A67
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

	// Token: 0x06002B9B RID: 11163 RVA: 0x000D688B File Offset: 0x000D4A8B
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

	// Token: 0x06002B9C RID: 11164 RVA: 0x000D68AF File Offset: 0x000D4AAF
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

	// Token: 0x06002B9D RID: 11165 RVA: 0x000D68D3 File Offset: 0x000D4AD3
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

	// Token: 0x06002B9E RID: 11166 RVA: 0x000D68F7 File Offset: 0x000D4AF7
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

	// Token: 0x06002B9F RID: 11167 RVA: 0x000D6920 File Offset: 0x000D4B20
	public override string ToString()
	{
		return string.Format("{0:F3} seconds since {{{1:s}", this.secondsElapsed, this._dt);
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x000D6942 File Offset: 0x000D4B42
	public override int GetHashCode()
	{
		return StaticHash.Compute(this._dt);
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x000D694F File Offset: 0x000D4B4F
	public static TimeSince Now()
	{
		return new TimeSince(DateTime.UtcNow);
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x000D695B File Offset: 0x000D4B5B
	public static implicit operator long(TimeSince ts)
	{
		return ts.secondsElapsedLong;
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x000D6964 File Offset: 0x000D4B64
	public static implicit operator double(TimeSince ts)
	{
		return ts.secondsElapsed;
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x000D696D File Offset: 0x000D4B6D
	public static implicit operator float(TimeSince ts)
	{
		return ts.secondsElapsedFloat;
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x000D6976 File Offset: 0x000D4B76
	public static implicit operator int(TimeSince ts)
	{
		return ts.secondsElapsedInt;
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x000D697F File Offset: 0x000D4B7F
	public static implicit operator uint(TimeSince ts)
	{
		return ts.secondsElapsedUint;
	}

	// Token: 0x06002BA7 RID: 11175 RVA: 0x000D6988 File Offset: 0x000D4B88
	public static implicit operator TimeSpan(TimeSince ts)
	{
		return ts.secondsElapsedSpan;
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x000D6991 File Offset: 0x000D4B91
	public static implicit operator TimeSince(int elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x000D6999 File Offset: 0x000D4B99
	public static implicit operator TimeSince(uint elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x000D69A1 File Offset: 0x000D4BA1
	public static implicit operator TimeSince(float elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x000D69A9 File Offset: 0x000D4BA9
	public static implicit operator TimeSince(double elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x000D69B1 File Offset: 0x000D4BB1
	public static implicit operator TimeSince(long elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x000D69B9 File Offset: 0x000D4BB9
	public static implicit operator TimeSince(TimeSpan elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x000D69C1 File Offset: 0x000D4BC1
	public static implicit operator TimeSince(DateTime dt)
	{
		return new TimeSince(dt);
	}

	// Token: 0x040030C3 RID: 12483
	private DateTime _dt;

	// Token: 0x040030C4 RID: 12484
	private const double INT32_MAX = 2147483647.0;
}
