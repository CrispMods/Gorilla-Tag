using System;
using UnityEngine;

// Token: 0x02000838 RID: 2104
public class BitPackDebug : MonoBehaviour
{
	// Token: 0x0400369D RID: 13981
	public bool debugPos;

	// Token: 0x0400369E RID: 13982
	public Vector3 pos;

	// Token: 0x0400369F RID: 13983
	public Vector3 min = Vector3.one * -2f;

	// Token: 0x040036A0 RID: 13984
	public Vector3 max = Vector3.one * 2f;

	// Token: 0x040036A1 RID: 13985
	public float rad = 4f;

	// Token: 0x040036A2 RID: 13986
	[Space]
	public bool debug32;

	// Token: 0x040036A3 RID: 13987
	public uint packed;

	// Token: 0x040036A4 RID: 13988
	public Vector3 unpacked;

	// Token: 0x040036A5 RID: 13989
	[Space]
	public bool debug16;

	// Token: 0x040036A6 RID: 13990
	public ushort packed16;

	// Token: 0x040036A7 RID: 13991
	public Vector3 unpacked16;
}
