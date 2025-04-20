using System;
using System.Collections.Generic;

// Token: 0x0200022C RID: 556
[Serializable]
public class ScenePerformanceData
{
	// Token: 0x06000CE1 RID: 3297 RVA: 0x000A0AC8 File Offset: 0x0009ECC8
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

	// Token: 0x0400103A RID: 4154
	public string _mapName;

	// Token: 0x0400103B RID: 4155
	public int _gorillaCount;

	// Token: 0x0400103C RID: 4156
	public int _droppedFrames;

	// Token: 0x0400103D RID: 4157
	public int _msHigh;

	// Token: 0x0400103E RID: 4158
	public int _medianMS;

	// Token: 0x0400103F RID: 4159
	public int _medianFPS;

	// Token: 0x04001040 RID: 4160
	public int _medianDrawCallCount;

	// Token: 0x04001041 RID: 4161
	public List<int> _msCaptures;
}
