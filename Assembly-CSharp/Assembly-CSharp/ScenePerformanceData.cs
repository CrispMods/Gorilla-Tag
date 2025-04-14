using System;
using System.Collections.Generic;

// Token: 0x02000221 RID: 545
[Serializable]
public class ScenePerformanceData
{
	// Token: 0x06000C98 RID: 3224 RVA: 0x00042D3C File Offset: 0x00040F3C
	public ScenePerformanceData(string mapName, int gorillaCount, int droppedFrames, int msHigh, int medianMS, int medianFPS, int medianDrawCalls, List<int> msCaptures)
	{
		this._mapName = mapName;
		this._gorillaCount = gorillaCount;
		this._droppedFrames = droppedFrames;
		this._msHigh = msHigh;
		this._medianMS = medianMS;
		this._medianFPS = medianFPS;
		this._medianDrawCallCount = medianDrawCalls;
		this._msCaptures = new List<int>(msCaptures);
	}

	// Token: 0x04000FF5 RID: 4085
	public string _mapName;

	// Token: 0x04000FF6 RID: 4086
	public int _gorillaCount;

	// Token: 0x04000FF7 RID: 4087
	public int _droppedFrames;

	// Token: 0x04000FF8 RID: 4088
	public int _msHigh;

	// Token: 0x04000FF9 RID: 4089
	public int _medianMS;

	// Token: 0x04000FFA RID: 4090
	public int _medianFPS;

	// Token: 0x04000FFB RID: 4091
	public int _medianDrawCallCount;

	// Token: 0x04000FFC RID: 4092
	public List<int> _msCaptures;
}
