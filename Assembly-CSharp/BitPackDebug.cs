using System;
using UnityEngine;

// Token: 0x02000852 RID: 2130
public class BitPackDebug : MonoBehaviour
{
	// Token: 0x04003759 RID: 14169
	public bool debugPos;

	// Token: 0x0400375A RID: 14170
	public Vector3 pos;

	// Token: 0x0400375B RID: 14171
	public Vector3 min = Vector3.one * -2f;

	// Token: 0x0400375C RID: 14172
	public Vector3 max = Vector3.one * 2f;

	// Token: 0x0400375D RID: 14173
	public float rad = 4f;

	// Token: 0x0400375E RID: 14174
	[Space]
	public bool debug32;

	// Token: 0x0400375F RID: 14175
	public uint packed;

	// Token: 0x04003760 RID: 14176
	public Vector3 unpacked;

	// Token: 0x04003761 RID: 14177
	[Space]
	public bool debug16;

	// Token: 0x04003762 RID: 14178
	public ushort packed16;

	// Token: 0x04003763 RID: 14179
	public Vector3 unpacked16;
}
