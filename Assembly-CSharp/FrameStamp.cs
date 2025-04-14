using System;
using UnityEngine;

// Token: 0x020006AE RID: 1710
[Serializable]
public struct FrameStamp
{
	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06002A6E RID: 10862 RVA: 0x000D36B1 File Offset: 0x000D18B1
	public int framesElapsed
	{
		get
		{
			return Time.frameCount - this._lastFrame;
		}
	}

	// Token: 0x06002A6F RID: 10863 RVA: 0x000D36C0 File Offset: 0x000D18C0
	public static FrameStamp Now()
	{
		return new FrameStamp
		{
			_lastFrame = Time.frameCount
		};
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x000D36E2 File Offset: 0x000D18E2
	public override string ToString()
	{
		return string.Format("{0} frames elapsed", this.framesElapsed);
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x000D36F9 File Offset: 0x000D18F9
	public override int GetHashCode()
	{
		return StaticHash.Compute(this._lastFrame);
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x000D3706 File Offset: 0x000D1906
	public static implicit operator int(FrameStamp fs)
	{
		return fs.framesElapsed;
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x000D3710 File Offset: 0x000D1910
	public static implicit operator FrameStamp(int framesElapsed)
	{
		return new FrameStamp
		{
			_lastFrame = Time.frameCount - framesElapsed
		};
	}

	// Token: 0x04002FDC RID: 12252
	private int _lastFrame;
}
