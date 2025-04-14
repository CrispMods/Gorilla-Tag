using System;
using UnityEngine;

// Token: 0x0200083B RID: 2107
public class BitPackDebug : MonoBehaviour
{
	// Token: 0x040036AF RID: 13999
	public bool debugPos;

	// Token: 0x040036B0 RID: 14000
	public Vector3 pos;

	// Token: 0x040036B1 RID: 14001
	public Vector3 min = Vector3.one * -2f;

	// Token: 0x040036B2 RID: 14002
	public Vector3 max = Vector3.one * 2f;

	// Token: 0x040036B3 RID: 14003
	public float rad = 4f;

	// Token: 0x040036B4 RID: 14004
	[Space]
	public bool debug32;

	// Token: 0x040036B5 RID: 14005
	public uint packed;

	// Token: 0x040036B6 RID: 14006
	public Vector3 unpacked;

	// Token: 0x040036B7 RID: 14007
	[Space]
	public bool debug16;

	// Token: 0x040036B8 RID: 14008
	public ushort packed16;

	// Token: 0x040036B9 RID: 14009
	public Vector3 unpacked16;
}
