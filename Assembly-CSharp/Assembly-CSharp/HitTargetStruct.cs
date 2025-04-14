using System;
using System.Runtime.InteropServices;
using Fusion;

// Token: 0x0200027C RID: 636
[NetworkStructWeaved(1)]
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct HitTargetStruct : INetworkStruct
{
	// Token: 0x06000EE4 RID: 3812 RVA: 0x0004C0BC File Offset: 0x0004A2BC
	public HitTargetStruct(int v)
	{
		this.Score = v;
	}

	// Token: 0x040011AE RID: 4526
	[FieldOffset(0)]
	public int Score;
}
