using System;
using UnityEngine;

// Token: 0x020006C3 RID: 1731
[Serializable]
public struct FrameStamp
{
	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x06002B04 RID: 11012 RVA: 0x0004CFB8 File Offset: 0x0004B1B8
	public int framesElapsed
	{
		get
		{
			return Time.frameCount - this._lastFrame;
		}
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x0011FB18 File Offset: 0x0011DD18
	public static FrameStamp Now()
	{
		return new FrameStamp
		{
			_lastFrame = Time.frameCount
		};
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x0004CFC6 File Offset: 0x0004B1C6
	public override string ToString()
	{
		return string.Format("{0} frames elapsed", this.framesElapsed);
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x0004CFDD File Offset: 0x0004B1DD
	public override int GetHashCode()
	{
		return StaticHash.Compute(this._lastFrame);
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x0004CFEA File Offset: 0x0004B1EA
	public static implicit operator int(FrameStamp fs)
	{
		return fs.framesElapsed;
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x0011FB3C File Offset: 0x0011DD3C
	public static implicit operator FrameStamp(int framesElapsed)
	{
		return new FrameStamp
		{
			_lastFrame = Time.frameCount - framesElapsed
		};
	}

	// Token: 0x04003079 RID: 12409
	private int _lastFrame;
}
