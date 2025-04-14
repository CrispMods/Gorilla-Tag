using System;
using UnityEngine;

// Token: 0x020006AF RID: 1711
[Serializable]
public struct FrameStamp
{
	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06002A76 RID: 10870 RVA: 0x000D3B31 File Offset: 0x000D1D31
	public int framesElapsed
	{
		get
		{
			return Time.frameCount - this._lastFrame;
		}
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x000D3B40 File Offset: 0x000D1D40
	public static FrameStamp Now()
	{
		return new FrameStamp
		{
			_lastFrame = Time.frameCount
		};
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x000D3B62 File Offset: 0x000D1D62
	public override string ToString()
	{
		return string.Format("{0} frames elapsed", this.framesElapsed);
	}

	// Token: 0x06002A79 RID: 10873 RVA: 0x000D3B79 File Offset: 0x000D1D79
	public override int GetHashCode()
	{
		return StaticHash.Compute(this._lastFrame);
	}

	// Token: 0x06002A7A RID: 10874 RVA: 0x000D3B86 File Offset: 0x000D1D86
	public static implicit operator int(FrameStamp fs)
	{
		return fs.framesElapsed;
	}

	// Token: 0x06002A7B RID: 10875 RVA: 0x000D3B90 File Offset: 0x000D1D90
	public static implicit operator FrameStamp(int framesElapsed)
	{
		return new FrameStamp
		{
			_lastFrame = Time.frameCount - framesElapsed
		};
	}

	// Token: 0x04002FE2 RID: 12258
	private int _lastFrame;
}
