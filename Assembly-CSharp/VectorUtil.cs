using System;
using UnityEngine;

// Token: 0x020002FB RID: 763
public static class VectorUtil
{
	// Token: 0x06001237 RID: 4663 RVA: 0x00056E81 File Offset: 0x00055081
	public static Vector4 ToVector(this Rect rect)
	{
		return new Vector4(rect.x, rect.y, rect.width, rect.height);
	}
}
