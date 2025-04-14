using System;
using System.Runtime.InteropServices;
using Fusion;

// Token: 0x0200027C RID: 636
[NetworkStructWeaved(1)]
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct HitTargetStruct : INetworkStruct
{
	// Token: 0x06000EE2 RID: 3810 RVA: 0x0004BD78 File Offset: 0x00049F78
	public HitTargetStruct(int v)
	{
		this.Score = v;
	}

	// Token: 0x040011AD RID: 4525
	[FieldOffset(0)]
	public int Score;
}
