using System;

// Token: 0x020006D7 RID: 1751
public struct TimeSince
{
	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06002B8D RID: 11149 RVA: 0x0011D010 File Offset: 0x0011B210
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
	// (get) Token: 0x06002B8E RID: 11150 RVA: 0x0004CB9B File Offset: 0x0004AD9B
	public float secondsElapsedFloat
	{
		get
		{
			return (float)this.secondsElapsed;
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06002B8F RID: 11151 RVA: 0x0004CBA4 File Offset: 0x0004ADA4
	public int secondsElapsedInt
	{
		get
		{
			return (int)this.secondsElapsed;
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06002B90 RID: 11152 RVA: 0x0004CBAD File Offset: 0x0004ADAD
	public uint secondsElapsedUint
	{
		get
		{
			return (uint)this.secondsElapsed;
		}
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06002B91 RID: 11153 RVA: 0x0004CBB6 File Offset: 0x0004ADB6
	public long secondsElapsedLong
	{
		get
		{
			return (long)this.secondsElapsed;
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06002B92 RID: 11154 RVA: 0x0004CBBF File Offset: 0x0004ADBF
	public TimeSpan secondsElapsedSpan
	{
		get
		{
			return TimeSpan.FromSeconds(this.secondsElapsed);
		}
	}

	// Token: 0x06002B93 RID: 11155 RVA: 0x0004CBCC File Offset: 0x0004ADCC
	public TimeSince(DateTime dt)
	{
		this._dt = dt;
	}

	// Token: 0x06002B94 RID: 11156 RVA: 0x0011D050 File Offset: 0x0011B250
	public TimeSince(int elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x0011D074 File Offset: 0x0011B274
	public TimeSince(uint elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds(-1.0 * elapsed);
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x0011D050 File Offset: 0x0011B250
	public TimeSince(float elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x0011D0A4 File Offset: 0x0011B2A4
	public TimeSince(double elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds(-elapsed);
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x0011D050 File Offset: 0x0011B250
	public TimeSince(long elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x0011D0C8 File Offset: 0x0011B2C8
	public TimeSince(TimeSpan elapsed)
	{
		this._dt = DateTime.UtcNow.Add(-elapsed);
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x0004CBD5 File Offset: 0x0004ADD5
	public bool HasElapsed(int seconds)
	{
		return this.secondsElapsedInt >= seconds;
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x0004CBE3 File Offset: 0x0004ADE3
	public bool HasElapsed(uint seconds)
	{
		return this.secondsElapsedUint >= seconds;
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x0004CBF1 File Offset: 0x0004ADF1
	public bool HasElapsed(float seconds)
	{
		return this.secondsElapsedFloat >= seconds;
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x0004CBFF File Offset: 0x0004ADFF
	public bool HasElapsed(double seconds)
	{
		return this.secondsElapsed >= seconds;
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x0004CC0D File Offset: 0x0004AE0D
	public bool HasElapsed(long seconds)
	{
		return this.secondsElapsedLong >= seconds;
	}

	// Token: 0x06002B9F RID: 11167 RVA: 0x0004CC1B File Offset: 0x0004AE1B
	public bool HasElapsed(TimeSpan seconds)
	{
		return this.secondsElapsedSpan >= seconds;
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x0004CC29 File Offset: 0x0004AE29
	public void Reset()
	{
		this._dt = DateTime.UtcNow;
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x0004CC36 File Offset: 0x0004AE36
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

	// Token: 0x06002BA2 RID: 11170 RVA: 0x0004CC5A File Offset: 0x0004AE5A
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

	// Token: 0x06002BA3 RID: 11171 RVA: 0x0004CC7E File Offset: 0x0004AE7E
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

	// Token: 0x06002BA4 RID: 11172 RVA: 0x0004CCA2 File Offset: 0x0004AEA2
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

	// Token: 0x06002BA5 RID: 11173 RVA: 0x0004CCC6 File Offset: 0x0004AEC6
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

	// Token: 0x06002BA6 RID: 11174 RVA: 0x0004CCEA File Offset: 0x0004AEEA
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

	// Token: 0x06002BA7 RID: 11175 RVA: 0x0004CD13 File Offset: 0x0004AF13
	public override string ToString()
	{
		return string.Format("{0:F3} seconds since {{{1:s}", this.secondsElapsed, this._dt);
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x0004CD35 File Offset: 0x0004AF35
	public override int GetHashCode()
	{
		return StaticHash.Compute(this._dt);
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x0004CD42 File Offset: 0x0004AF42
	public static TimeSince Now()
	{
		return new TimeSince(DateTime.UtcNow);
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x0004CD4E File Offset: 0x0004AF4E
	public static implicit operator long(TimeSince ts)
	{
		return ts.secondsElapsedLong;
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x0004CD57 File Offset: 0x0004AF57
	public static implicit operator double(TimeSince ts)
	{
		return ts.secondsElapsed;
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x0004CD60 File Offset: 0x0004AF60
	public static implicit operator float(TimeSince ts)
	{
		return ts.secondsElapsedFloat;
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x0004CD69 File Offset: 0x0004AF69
	public static implicit operator int(TimeSince ts)
	{
		return ts.secondsElapsedInt;
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x0004CD72 File Offset: 0x0004AF72
	public static implicit operator uint(TimeSince ts)
	{
		return ts.secondsElapsedUint;
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x0004CD7B File Offset: 0x0004AF7B
	public static implicit operator TimeSpan(TimeSince ts)
	{
		return ts.secondsElapsedSpan;
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x0004CD84 File Offset: 0x0004AF84
	public static implicit operator TimeSince(int elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB1 RID: 11185 RVA: 0x0004CD8C File Offset: 0x0004AF8C
	public static implicit operator TimeSince(uint elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x0004CD94 File Offset: 0x0004AF94
	public static implicit operator TimeSince(float elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB3 RID: 11187 RVA: 0x0004CD9C File Offset: 0x0004AF9C
	public static implicit operator TimeSince(double elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x0004CDA4 File Offset: 0x0004AFA4
	public static implicit operator TimeSince(long elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x0004CDAC File Offset: 0x0004AFAC
	public static implicit operator TimeSince(TimeSpan elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x0004CDB4 File Offset: 0x0004AFB4
	public static implicit operator TimeSince(DateTime dt)
	{
		return new TimeSince(dt);
	}

	// Token: 0x040030C9 RID: 12489
	private DateTime _dt;

	// Token: 0x040030CA RID: 12490
	private const double INT32_MAX = 2147483647.0;
}
