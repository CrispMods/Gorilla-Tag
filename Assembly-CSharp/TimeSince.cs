using System;

// Token: 0x020006EB RID: 1771
public struct TimeSince
{
	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06002C1B RID: 11291 RVA: 0x00121BC8 File Offset: 0x0011FDC8
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

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06002C1C RID: 11292 RVA: 0x0004DEE0 File Offset: 0x0004C0E0
	public float secondsElapsedFloat
	{
		get
		{
			return (float)this.secondsElapsed;
		}
	}

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06002C1D RID: 11293 RVA: 0x0004DEE9 File Offset: 0x0004C0E9
	public int secondsElapsedInt
	{
		get
		{
			return (int)this.secondsElapsed;
		}
	}

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06002C1E RID: 11294 RVA: 0x0004DEF2 File Offset: 0x0004C0F2
	public uint secondsElapsedUint
	{
		get
		{
			return (uint)this.secondsElapsed;
		}
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06002C1F RID: 11295 RVA: 0x0004DEFB File Offset: 0x0004C0FB
	public long secondsElapsedLong
	{
		get
		{
			return (long)this.secondsElapsed;
		}
	}

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06002C20 RID: 11296 RVA: 0x0004DF04 File Offset: 0x0004C104
	public TimeSpan secondsElapsedSpan
	{
		get
		{
			return TimeSpan.FromSeconds(this.secondsElapsed);
		}
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x0004DF11 File Offset: 0x0004C111
	public TimeSince(DateTime dt)
	{
		this._dt = dt;
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x00121C08 File Offset: 0x0011FE08
	public TimeSince(int elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x00121C2C File Offset: 0x0011FE2C
	public TimeSince(uint elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds(-1.0 * elapsed);
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x00121C08 File Offset: 0x0011FE08
	public TimeSince(float elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x00121C5C File Offset: 0x0011FE5C
	public TimeSince(double elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds(-elapsed);
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x00121C08 File Offset: 0x0011FE08
	public TimeSince(long elapsed)
	{
		this._dt = DateTime.UtcNow.AddSeconds((double)(-(double)elapsed));
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x00121C80 File Offset: 0x0011FE80
	public TimeSince(TimeSpan elapsed)
	{
		this._dt = DateTime.UtcNow.Add(-elapsed);
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x0004DF1A File Offset: 0x0004C11A
	public bool HasElapsed(int seconds)
	{
		return this.secondsElapsedInt >= seconds;
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x0004DF28 File Offset: 0x0004C128
	public bool HasElapsed(uint seconds)
	{
		return this.secondsElapsedUint >= seconds;
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x0004DF36 File Offset: 0x0004C136
	public bool HasElapsed(float seconds)
	{
		return this.secondsElapsedFloat >= seconds;
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x0004DF44 File Offset: 0x0004C144
	public bool HasElapsed(double seconds)
	{
		return this.secondsElapsed >= seconds;
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x0004DF52 File Offset: 0x0004C152
	public bool HasElapsed(long seconds)
	{
		return this.secondsElapsedLong >= seconds;
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x0004DF60 File Offset: 0x0004C160
	public bool HasElapsed(TimeSpan seconds)
	{
		return this.secondsElapsedSpan >= seconds;
	}

	// Token: 0x06002C2E RID: 11310 RVA: 0x0004DF6E File Offset: 0x0004C16E
	public void Reset()
	{
		this._dt = DateTime.UtcNow;
	}

	// Token: 0x06002C2F RID: 11311 RVA: 0x0004DF7B File Offset: 0x0004C17B
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

	// Token: 0x06002C30 RID: 11312 RVA: 0x0004DF9F File Offset: 0x0004C19F
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

	// Token: 0x06002C31 RID: 11313 RVA: 0x0004DFC3 File Offset: 0x0004C1C3
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

	// Token: 0x06002C32 RID: 11314 RVA: 0x0004DFE7 File Offset: 0x0004C1E7
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

	// Token: 0x06002C33 RID: 11315 RVA: 0x0004E00B File Offset: 0x0004C20B
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

	// Token: 0x06002C34 RID: 11316 RVA: 0x0004E02F File Offset: 0x0004C22F
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

	// Token: 0x06002C35 RID: 11317 RVA: 0x0004E058 File Offset: 0x0004C258
	public override string ToString()
	{
		return string.Format("{0:F3} seconds since {{{1:s}", this.secondsElapsed, this._dt);
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x0004E07A File Offset: 0x0004C27A
	public override int GetHashCode()
	{
		return StaticHash.Compute(this._dt);
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x0004E087 File Offset: 0x0004C287
	public static TimeSince Now()
	{
		return new TimeSince(DateTime.UtcNow);
	}

	// Token: 0x06002C38 RID: 11320 RVA: 0x0004E093 File Offset: 0x0004C293
	public static implicit operator long(TimeSince ts)
	{
		return ts.secondsElapsedLong;
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x0004E09C File Offset: 0x0004C29C
	public static implicit operator double(TimeSince ts)
	{
		return ts.secondsElapsed;
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x0004E0A5 File Offset: 0x0004C2A5
	public static implicit operator float(TimeSince ts)
	{
		return ts.secondsElapsedFloat;
	}

	// Token: 0x06002C3B RID: 11323 RVA: 0x0004E0AE File Offset: 0x0004C2AE
	public static implicit operator int(TimeSince ts)
	{
		return ts.secondsElapsedInt;
	}

	// Token: 0x06002C3C RID: 11324 RVA: 0x0004E0B7 File Offset: 0x0004C2B7
	public static implicit operator uint(TimeSince ts)
	{
		return ts.secondsElapsedUint;
	}

	// Token: 0x06002C3D RID: 11325 RVA: 0x0004E0C0 File Offset: 0x0004C2C0
	public static implicit operator TimeSpan(TimeSince ts)
	{
		return ts.secondsElapsedSpan;
	}

	// Token: 0x06002C3E RID: 11326 RVA: 0x0004E0C9 File Offset: 0x0004C2C9
	public static implicit operator TimeSince(int elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002C3F RID: 11327 RVA: 0x0004E0D1 File Offset: 0x0004C2D1
	public static implicit operator TimeSince(uint elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002C40 RID: 11328 RVA: 0x0004E0D9 File Offset: 0x0004C2D9
	public static implicit operator TimeSince(float elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002C41 RID: 11329 RVA: 0x0004E0E1 File Offset: 0x0004C2E1
	public static implicit operator TimeSince(double elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002C42 RID: 11330 RVA: 0x0004E0E9 File Offset: 0x0004C2E9
	public static implicit operator TimeSince(long elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002C43 RID: 11331 RVA: 0x0004E0F1 File Offset: 0x0004C2F1
	public static implicit operator TimeSince(TimeSpan elapsed)
	{
		return new TimeSince(elapsed);
	}

	// Token: 0x06002C44 RID: 11332 RVA: 0x0004E0F9 File Offset: 0x0004C2F9
	public static implicit operator TimeSince(DateTime dt)
	{
		return new TimeSince(dt);
	}

	// Token: 0x04003160 RID: 12640
	private DateTime _dt;

	// Token: 0x04003161 RID: 12641
	private const double INT32_MAX = 2147483647.0;
}
