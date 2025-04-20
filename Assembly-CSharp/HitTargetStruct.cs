using System;
using System.Runtime.InteropServices;
using Fusion;

// Token: 0x02000287 RID: 647
[NetworkStructWeaved(1)]
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct HitTargetStruct : INetworkStruct
{
	// Token: 0x06000F2D RID: 3885 RVA: 0x0003AA27 File Offset: 0x00038C27
	public HitTargetStruct(int v)
	{
		this.Score = v;
	}

	// Token: 0x040011F3 RID: 4595
	[FieldOffset(0)]
	public int Score;
}
