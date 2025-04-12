using System;
using UnityEngine;

// Token: 0x020002FB RID: 763
public static class VectorUtil
{
	// Token: 0x0600123A RID: 4666 RVA: 0x0003B88E File Offset: 0x00039A8E
	public static Vector4 ToVector(this Rect rect)
	{
		return new Vector4(rect.x, rect.y, rect.width, rect.height);
	}
}
